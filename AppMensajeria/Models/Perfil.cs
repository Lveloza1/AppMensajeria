using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMensajeria.Models
{
    [Table("Perfil")]

    public class Perfil
    {

        [Key]
        [Column("UsuarioID")]
        public int Mi_UsuarioID { get; set; }
        [Column("Imagen", TypeName = "BLOB")]
        public string Mi_Imagen { get; set; }
        [Required]
        [Column("Nombre")]
        [StringLength(255)]
        public string Mi_Nombre { get; set; }
        [Required]
        [Column("Telefono")]
        public string Mi_Telefono { get; set; }
        
    }
}
