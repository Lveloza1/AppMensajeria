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
        [Required]
        [Column("Titulo")]
        public string Titulo { get; set; }
        [Required]
        [Column("Titulo")]
        public string Tipo { get; set; }
    }
}