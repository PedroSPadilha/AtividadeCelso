using Microsoft.EntityFrameworkCore;

namespace Atividade1.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Cadastrante> Cadastrantes { get; set; }
        public DbSet<Evento> Eventos { get; set; }
    }
}
