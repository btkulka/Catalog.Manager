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
    }
}