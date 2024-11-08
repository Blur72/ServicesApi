using Microsoft.EntityFrameworkCore;
using WebApplication2.Model;

namespace WebApplication2.DataBaseContext
{
    public class TestApiDB2 : DbContext
    {
        public TestApiDB2(DbContextOptions options) : base(options) 
        {

        }
        public DbSet<Readers> Readers { get; set; }

    }
}
