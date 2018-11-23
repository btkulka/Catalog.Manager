using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Facade.Wrappers
{
    public class WPRComponentTypeDescription : ComponentTypeDescription
    {

        public string StatusCode { get; set; }
        public string StatusDetail { get; set; }

        public static WPRComponentTypeDescription Wrap(ComponentTypeDescription compTypeUpdates)
        {
            WPRComponentTypeDescription wpr = new WPRComponentTypeDescription()
            {
                Description = compTypeUpdates.Description,
                ID = compTypeUpdates.ID
            };

            return wpr;
        }
    }
}
