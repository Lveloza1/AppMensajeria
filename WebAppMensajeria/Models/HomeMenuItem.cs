using System;
using System.Collections.Generic;
using System.Text;

namespace AppMensajeria.Models
{
    public enum MenuItemType
    {
        Perfil,
        Privado,
        Grupos,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
