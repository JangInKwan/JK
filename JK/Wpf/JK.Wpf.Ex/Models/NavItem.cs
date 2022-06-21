using JK.Wpf.Ex.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Wpf.Ex.Models
{
    public class NavItem
    {
        public string NavName { get; set; }
        public NavType NavType { get; set; }
        public string NavImage { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
