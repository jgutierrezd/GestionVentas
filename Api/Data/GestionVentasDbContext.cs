using ApiGestionVentas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionVentas.Data
{
    public class GestionVentasDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public GestionVentasDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Asesor> Asesor { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Venta> Venta { get; set; }
    }
}
