using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace Business.Services;

public class BookingService(IBookingRepository bookingRepository, IHttpClientFactory httpClientFactory) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<BookingResult> CreateBookingAsync(CreateBookingRequest request)
    {
        try
        {
            var bookingEntity = new BookingEntity
            {
                EventId = request.EventId,
                BookingDate = DateTime.Now,
                TicketQuantity = request.TicketQuantity,
                BookingCustomer = new BookingCustomerEntity
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    BookingAddress = new BookingAddressEntity
                    {
                        StreetAddress = request.StreetAddress,
                        PostalCode = request.PostalCode,
                        City = request.City,
                    }
                }
            };

            var result = await _bookingRepository.AddAsync(bookingEntity);

            if (!result.Success)
                return new BookingResult { Success = false, Error = result.Error };

            try {
                var emailClient = _httpClientFactory.CreateClient("EmailService");

                var emailRequest = new
                {
                    to = request.Email,
                    subject = "Booking Confirmation",
                    htmlBody = $@"
                    <h1>Booking Confirmed</h1>
                    <p>Hi {request.FirstName},</p>
                    <p>Your booking for event ID {request.EventId} is confirmed.</p>
                    <p>We look forward to seeing you!</p>"
                };

                await emailClient.PostAsJsonAsync("api/email/send", emailRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email not sent: {ex.Message}");
            }

            return new BookingResult { Success = true };
        }
        catch (Exception ex)
        {
            return new BookingResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}