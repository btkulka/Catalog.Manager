using CatalogData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class CatalogSystemViewModel
    {
        public CatalogSystem System { get; set; }
        public List<CatalogSystem> SystemHistory { get; set; }
    }
}