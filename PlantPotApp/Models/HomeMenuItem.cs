using System;
using System.Collections.Generic;
using System.Text;

namespace PlantPotApp.Models
{
    public enum MenuItemType
    {
        Home,
        About,
        Colour,
        Settings
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
