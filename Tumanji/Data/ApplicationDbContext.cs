using Microsoft.EntityFrameworkCore;
using Tumanji.Models;

namespace Tumanji.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> User { get; set; }
    }
}
