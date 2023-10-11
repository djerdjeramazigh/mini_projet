using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TodoApi2.Models
{
    public class VehiculeContext : DbContext
    {
        public VehiculeContext(DbContextOptions<VehiculeContext> options)
            : base(options)
        {
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "D:\\mini_projet\\database.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
        public DbSet<Vehicule> Vehicule { get; set; } = null!;
    }
}
