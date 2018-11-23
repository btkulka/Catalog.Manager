using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Facade.Wrappers
{
    public class WPRSubComponentDescription : SubComponentDescription
    {

        public string StatusCode { get; set; }
        public string StatusDetail { get; set; }

        public static WPRSubComponentDescription Wrap(SubComponentDescription desc)
        {
            WPRSubComponentDescription wpr = new WPRSubComponentDescription()
            {
                Description = desc.Description,
                CompUnitsID = desc.CompUnitsID,
                ID = desc.ID
            };

            return wpr;
        }
    }
}
