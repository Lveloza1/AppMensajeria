using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMensajeria.Models
{
    [Table("Chat")]
    public class Chat
    {
        [Key]
        [Column("ChatID")]
        public int ChatID { get; set; }
        [Column("UsuarioID")]
        public int UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }
        [Required]
        [Column("Titulo")]
        public string Titulo { get; set; }
        [Required]
        [Column("Tipo")]
        public string Tipo { get; set; }
    }
}