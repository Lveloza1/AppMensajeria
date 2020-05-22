using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMensajeria.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("UsuarioID")]
        public int UsuarioID { get; set; }
        [Column("Imagen")]
        public byte[] Imagen { get; set; }
        [Required]
        [StringLength(255)]
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Required]
        [Column("Telefono")]
        public string Telefono { get; set; }
        [Column("Edad")]
        public int Edad { get; set; }
    }
}
