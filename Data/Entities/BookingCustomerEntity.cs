using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class BookingCustomerEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    [ForeignKey(nameof(BookingAddress))]
    public string? BookingAddressId { get; set; }
    public BookingAddressEntity? BookingAddress { get; set; }
}
