using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMensajeria.Models
{
    [Table("UsuarioChat")]
    public class UsuarioChat
    {
        [Key]
        [Column("UsuarioChatID")]
        public int UsuarioChatID { get; set; }
        [Required]
        [Column("UsuarioID")]
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        [Column("ChatID")]
        public int ChatID { get; set; }
        public Chat Chat { get; set; }
    }
}
