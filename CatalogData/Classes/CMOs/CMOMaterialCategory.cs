using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogData.Classes.CMOs
{
    public class CMOMaterialCategory : CatalogMessageObject
    {
        public int MaterialCategoryId { get; set; }
        public string CatalogDescription { get; set; }
    }
}
