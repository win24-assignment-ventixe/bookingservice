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
        EventModel? eventDetails;

        try
        {
            var eventClient = _httpClientFactory.CreateClient("EventService");
            var res = await eventClient.GetFromJsonAsync<ApiResponse<EventModel>>(request.EventId);

            if (res == null || !res.Success || res.Result == null)
            {
                return new BookingResult
                {
                    Success = false,
                    Error = res?.Error ?? $"Event '{request.EventId}' not found"
                };
            }

            eventDetails = res.Result;
        }
        catch (Exception ex)
        {
            return new BookingResult
            {
                Success = false,
                Error = $"Could not retrieve event '{request.EventId}': {ex.Message}"
            };
        }

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
                var totalPrice = eventDetails.Price * request.TicketQuantity;

                var emailRequest = new
                {
                    to = request.Email,
                    subject = "Booking Confirmation",
                    htmlBody = $@"
                    <p>Hi {request.FirstName},</p>
                    <p>Your booking for {eventDetails.Title} is confirmed.</p>
                    <p>We look forward to seeing you!</p>

                    <h4>Booking summary:</h4>
                    <p>{request.FirstName} {request.LastName}</p>
                    <p>{request.StreetAddress}</p>
                    <p>{request.PostalCode} {request.City}</p>

                    <p>Event: {eventDetails.Title}</p>
                    <p>Location: {eventDetails.Location}</p>
                    <p>Date: {eventDetails.EventDate}</p>
                    <p>Ticket Quantity: {request.TicketQuantity} x (${eventDetails.Price} / ticket)</p>
                    <p>Total Price: ${totalPrice}</p>"
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