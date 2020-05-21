using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMensajeria.Models
{
    [Table("Mensaje")]
    public class Mensaje
    {
        [Key]
        [Column("MensajeID")]
        public int MensajeID { get; set; }
        [Required]
        [Column("UsuarioID")]
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        [Column("ChatID")]
        public int ChatID { get; set; }
        public Chat Chat { get; set; }
        [Required]
        [StringLength(255)]
        [Column("Contenido")]
        public string Contenido { get; set; }
    }
}