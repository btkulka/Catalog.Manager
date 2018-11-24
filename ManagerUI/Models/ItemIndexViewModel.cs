using CatalogData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class ItemIndexViewModel
    {

        public CatalogItem CatalogItem { get; set; }
        public List<CatalogItem> History { get; set; }

        // References to tagged entities
        public Component Component { get; set; }
        public MaterialCategory MaterialCategory { get; set; }
        public ComponentType ComponentType { get; set; }
    }
}