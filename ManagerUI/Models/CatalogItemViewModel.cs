using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class CatalogItemViewModel
    {
        public int CMCID { get; set; }
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public int MaterialCategoryId { get; set; }
        public string MaterialCategoryName { get; set; }
        public int ComponentTypeId { get; set; }
        public string ComponentTypeDesc { get; set; }
        public decimal CII { get; set; }
        public decimal AdjFactor { get; set; }
        public int? TaskListId { get; set; }
        public int UoM { get; set; }
        public int? UseAltDesc { get; set; }
    }
}