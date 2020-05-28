using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMensajeria.Models
{
    [Table("Perfil")]

    public class Perfil
    {

        [Key]
        [Column("UsuarioID")]
        public int MiUsuarioID { get; set; }
        [Column("Imagen", TypeName = "BLOB")]
        public string MiImagen { get; set; }
        [Required]
        [Column("Nombre")]
        [StringLength(255)]
        public string MiNombre { get; set; }
        [Required]
        [Column("Telefono")]
        public string MiTelefono { get; set; }
        
    }
}
