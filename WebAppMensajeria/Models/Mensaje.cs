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
        [Column("UsuarioChatID")]
        public int UsuarioChatID { get; set; }
        public UsuarioChat UsuarioChat { get; set; }
        [Required]
        [StringLength(255)]
        [Column("Contenido")]
        public string Contenido { get; set; }
        [Required]
        [StringLength(255)]
        [Column("InfoDate")]
        public string InfoDate { get; set; }
        [Required]
        [Column("Estado")]
        public bool Estado { get; set; }
    }
}