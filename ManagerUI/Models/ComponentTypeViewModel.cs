using CatalogData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class ComponentTypeViewModel
    {

        public ComponentType CompType { get; set; }
        public List<ComponentType> CompTypeHistory { get; set; }

    }
}