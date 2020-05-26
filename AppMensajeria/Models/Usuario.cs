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
        [Required]
        [StringLength(255)]
        [Column("Imagen", TypeName = "BLOB")]
        public string Imagen { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Required]
        [Column("Telefono")]
        public int Telefono { get; set; }
    }
}
