using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms;

namespace AppMensajeria.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("UsuarioID")]
        public int UsuarioID { get; set; }               
        [Column("Imagen", TypeName = "BLOB")]
        public string Imagen { get; set; }
        [Required]
        [Column("Nombre")]
        [StringLength(255)]
        public string Nombre { get; set; }
        [Required]
        [Column("Telefono")]
        public string Telefono { get; set; }
        [Column("Biografia")]
        [StringLength(255)]
        public string Biografia { get; set; }
    }
}
