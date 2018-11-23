using CatalogData;
using CatalogData.Classes.CMOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models.ResyncAnnotations
{
    public class MaterialCategoryAnnotation
    {
        public MaterialCategory MaterialCategory { get; set; }
        public string Annotation { get; set; }

        public MaterialCategoryAnnotation(MaterialCategory matCat, string a)
        {
            MaterialCategory = matCat;
            Annotation = a;
        }

        internal static bool HasBeenUpdated(MaterialCategory matCat, CMOMaterialCategory cmo)
        {
            return cmo.CatalogDescription != matCat.MatCatDescription;
        }
    }
}