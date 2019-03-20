using Microsoft.EntityFrameworkCore;
 
namespace LoginRegistration.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }
        public DbSet<Users> users {get;set;}
        public DbSet<Transactions> transactions {get;set;}
    }
}
