using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class CreateBookingRequest
{
    [Required]
    public string EventId { get; set; } = null!;
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "Required")]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Required")]
    public string StreetAddress { get; set; } = null!;
    [Required(ErrorMessage = "Required")]
    public string PostalCode { get; set; } = null!;
    [Required(ErrorMessage = "Required")]
    public string City { get; set; } = null!;
    [Required(ErrorMessage = "Required")]
    public int TicketQuantity { get; set; } = 1;
}
