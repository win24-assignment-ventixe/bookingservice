using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models;

public class EventModel
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public DateTime EventDate { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
}
