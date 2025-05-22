using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;

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
            return result.Success
                ? new BookingResult { Success = true }
                : new BookingResult { Success = false, Error = result.Error };
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