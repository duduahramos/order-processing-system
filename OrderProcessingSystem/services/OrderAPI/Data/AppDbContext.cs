using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;

namespace OrderAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<Order> Order { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}