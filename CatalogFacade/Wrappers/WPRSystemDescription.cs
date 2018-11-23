using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Facade.Wrappers
{
    public class WPRSystemDescription : SystemCatalogDescription
    {

        public string StatusCode { get; set; }
        public string StatusDetail { get; set; }

        public static WPRSystemDescription Wrap(SystemCatalogDescription desc)
        {
            WPRSystemDescription wpr = new WPRSystemDescription()
            {
                Description = desc.Description,
                ID = desc.ID,
                IsUII = desc.IsUII,
                UIICode = desc.UIICode
            };

            return wpr;
        }

    }
}
