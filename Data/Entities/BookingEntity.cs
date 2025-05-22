using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class BookingEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EventId { get; set; } = null!;
    public int TicketQuantity { get; set; } = 1;
    public DateTime BookingDate { get; set; }

    [ForeignKey(nameof(BookingCustomer))]
    public string? BookingCustomerId { get; set; }
    public BookingCustomerEntity? BookingCustomer { get; set; }
}