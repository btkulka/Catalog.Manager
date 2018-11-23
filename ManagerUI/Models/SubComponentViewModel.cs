using CatalogData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class SubComponentViewModel
    {

        public SubComponent SubComp { get; set; }
        public List<SubComponent> SubCompHistory { get; set; }

    }
}