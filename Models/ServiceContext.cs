using Microsoft.EntityFrameworkCore;
using System;
namespace RestServiceFinal.Models
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
