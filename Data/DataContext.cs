using Microsoft.EntityFrameworkCore;
using banka_net_core.Models;

namespace banka_net_core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Accounts> Accounts { get; set; }

    }
}