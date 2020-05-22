using AppMensajeria.Models;
using Microsoft.EntityFrameworkCore;

namespace AppMensajeria.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly string DbPath = string.Empty;
        public AppDbContext(string dbPath)
        {
            DbPath = dbPath;
        }

        #region ListadoDeDatos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }

        #endregion

        #region Configuracion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioID);
            modelBuilder.Entity<Mensaje>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioID);

            modelBuilder.Entity<Mensaje>()
                .HasOne(e => e.Chat)
                .WithMany()
                .HasForeignKey(e => e.ChatID);
        }
        #endregion
    }
}