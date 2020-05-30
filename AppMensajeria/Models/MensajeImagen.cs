using Xamarin.Forms;
namespace AppMensajeria.Models
{
    public class MensajeImagen
    {

        public int MensajeID { get; set; }
        public int UsuarioChatID { get; set; }
        public UsuarioChat UsuarioChat { get; set; }
        public string Contenido { get; set; }
        public ImageSource Imagen { get; set; }
        public string InfoDate { get; set; }
        public bool Estado { get; set; }

    }
}
