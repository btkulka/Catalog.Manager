using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogData.Classes
{
    public class CatalogCredentials
    {
        public string RemoteAddress { get; set; }
        public string BUILDERUsername { get; set; }
        public string BUILDERPassword { get; set; }

        // Basic Contrsuctor
        public CatalogCredentials(string r, string un, string pw)
        {
            RemoteAddress = r;
            BUILDERUsername = un;
            BUILDERPassword = pw;
        }
    }
}
