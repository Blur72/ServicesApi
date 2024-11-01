using Biblioteka.Model;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.DataBaseContext
{
    public class TestApiDB2 : DbContext
    {
        public TestApiDB2(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Photo> Photos { get; set; }

    }
}

