using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

}
