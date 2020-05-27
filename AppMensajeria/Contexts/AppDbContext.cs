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
        public DbSet<Perfil> Perfiles { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UsuarioChat> UsuarioChats { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }

        #endregion

        #region Configuracion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {         
            modelBuilder.Entity<UsuarioChat>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioID);

            modelBuilder.Entity<UsuarioChat>()
                .HasOne(e => e.Chat)
                .WithMany()
                .HasForeignKey(e => e.ChatID);

            modelBuilder.Entity<Mensaje>()
                .HasOne(e => e.UsuarioChat)
                .WithMany()
                .HasForeignKey(e => e.UsuarioChatID);
        }
        #endregion
    }
}