using Microsoft.EntityFrameworkCore;

namespace WebApi.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> products { get; set; }
    }
}
