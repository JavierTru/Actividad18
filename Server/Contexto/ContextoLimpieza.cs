using Actividad17.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Actividad17.Server.Contexto
{
    public class ContextoLimpieza : DbContext
    {
        public ContextoLimpieza(DbContextOptions<ContextoLimpieza> options): base(options) 
        {
            
        }
        public DbSet<Clientes> Clientes  { get; set; }
        public DbSet<Garantias> Garantias { get; set; }
        public DbSet<Servicios> Servicios { get; set; }
    }
}
