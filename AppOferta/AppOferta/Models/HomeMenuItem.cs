using System;
using System.Collections.Generic;
using System.Text;

namespace AppOferta.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Usuario
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
