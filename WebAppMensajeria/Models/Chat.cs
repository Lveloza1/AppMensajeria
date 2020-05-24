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
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Required]
        [Column("Tipo")]
        public bool Tipo { get; set; }
    }
}