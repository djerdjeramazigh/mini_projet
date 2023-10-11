using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PDFGeneratorWeb.Pages;

namespace PDFGeneratorWeb.Data
{
    public class FactoryContext : DbContext
    {
        public FactoryContext (DbContextOptions<FactoryContext> options)
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
        public DbSet<PDFGeneratorWeb.Pages.VehiculeModel> VehiculeModel { get; set; } = default!;
        public DbSet<IncidentModel> IncidentModel { get; set; } = default!;
        
    }
}
