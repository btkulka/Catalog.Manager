using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Facade.Wrappers
{
    public class WPRCMCDescription : CMCDescription
    {
        public string StatusCode { get; set; }
        public string StatusDetail { get; set; }
    }
}
