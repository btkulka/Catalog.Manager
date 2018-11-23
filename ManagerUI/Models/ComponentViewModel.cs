using CatalogData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class ComponentViewModel
    {

        public Component Component { get; set; }
        public List<Component> History { get; set; }
    }
}