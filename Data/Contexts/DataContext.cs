using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<BookingCustomerEntity> BookingCustomers { get; set; }
    public DbSet<BookingAddressEntity> BookingAddresses { get; set; }
}
