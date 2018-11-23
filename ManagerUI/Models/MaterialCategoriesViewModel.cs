using CatalogData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class MaterialCategoriesViewModel
    {

        public MaterialCategory MatCat {get;set;}
        public List<MaterialCategory> MatCatHistory { get; set; }

    }
}