using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogData.Classes.CMOs
{
    public class CMOComponent : CatalogMessageObject
    {
        public string Description { get; set; }
        public int ComponentId { get; set; }
        public bool IsEquip { get; set; }
        public bool IsUII { get; set; }
        public int SystemId { get; set; }
        public string UIICode { get; set; }
    }
}
