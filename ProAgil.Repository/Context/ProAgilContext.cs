using Microsoft.EntityFrameworkCore;
using ProAgil.Domain.Entities;

namespace ProAgil.Repository.Context {
    public class ProAgilContext : DbContext {
        public ProAgilContext (DbContextOptions<ProAgilContext> options) : base (options) { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }

        protected override void OnModelCreating (ModelBuilder builder) {
            builder.Entity<PalestranteEvento> ()
                .HasKey (PE => new { PE.EventoId, PE.PalestranteId });

        }
    }
}