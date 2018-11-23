using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogData.Classes.CMOs
{
    public class CMOCatalogItem : CatalogMessageObject
    {
        public int CMCID { get; set; }
        public int ComponentId { get; set; }
        public int MaterialCategoryId { get; set; }
        public int ComponentTypeId { get; set; }
        public int UoM { get; set; }
        public int uiiId { get; set; }
        public float adjFactor { get; set; }
        public float CII { get; set; }
        public int? useAltDesc { get; set; }
        public int? taskListId { get; set; }
        public Guid? ciiSetLink { get; set; }

    }
}
