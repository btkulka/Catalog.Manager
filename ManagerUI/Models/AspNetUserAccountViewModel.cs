using CatalogData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class AspNetUserAccountViewModel
    {
        public AspNetUser User;
        public List<CatalogEdit> Edits;
    }
}