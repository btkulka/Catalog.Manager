using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogData.Classes.CMOs
{
    public class CMOComponentType: CatalogMessageObject
    {
        public string Description { get; set; }
        public int ComponentTypeId { get; set; }
    }
}
