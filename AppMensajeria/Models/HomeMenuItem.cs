namespace AppMensajeria.Models
{
    public enum MenuItemType
    {    
        Perfil,
        Mensajeria,   
        Grupos,
        Usuarios,
        About,
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
