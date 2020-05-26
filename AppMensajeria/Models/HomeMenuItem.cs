using System;
using System.Collections.Generic;
using System.Text;

namespace AppMensajeria.Models
{
    public enum MenuItemType
    {
        About,
        Perfil,
        Privado,
        Grupos
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
