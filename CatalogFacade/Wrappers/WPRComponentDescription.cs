using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Facade.Wrappers
{
    public class WPRComponentDescription : ComponentCatalogDescription
    {

        public string StatusCode { get; set; }
        public string StatusDetail { get; set; }

        internal static WPRComponentDescription Wrap(ComponentCatalogDescription upd)
        {
            WPRComponentDescription wpr = new WPRComponentDescription()
            {
                Description = upd.Description,
                ID = upd.ID,
                IsEquip = upd.IsEquip,
                IsUII = upd.IsUII,
                SysId = upd.SysId,
                UIICode = upd.UIICode
            };

            return wpr;
        }
    }
}
