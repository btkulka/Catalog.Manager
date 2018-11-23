using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Facade.Wrappers
{
    public class WPRMaterialCategoryDescription : MaterialCategoryDescription
    {

        public string StatusCode { get; set; }
        public string StatusDetail { get; set; }

        internal static WPRMaterialCategoryDescription Wrap(MaterialCategoryDescription matCatUpdates)
        {
            WPRMaterialCategoryDescription wpr = new WPRMaterialCategoryDescription()
            {
                CatDesc = matCatUpdates.CatDesc,
                ID = matCatUpdates.ID
            };

            return wpr;
        }
    }
}
