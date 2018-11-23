using CatalogData;
using ManagerUI.Models.ResyncAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class ResyncResultViewModel
    {
        public BUILDERInstance BuilderInstance { get; set; }
        public List<CatalogItemAnnotation> CatalogItemResults { get; set; }
        public List<SystemAnnotation> SystemResults { get; set; }
        public List<ComponentAnnotation> ComponentResults { get; set; }
        public List<MaterialCategoryAnnotation> MatCatResults { get; set; }
        public List<SubComponentAnnotation> SubComponentResults { get; set; }
        public List<ComponentTypeAnnotation> ComponentTypeResults { get; set; }

        public ResyncResultViewModel()
        {
            CatalogItemResults = new List<CatalogItemAnnotation>();
            SystemResults = new List<SystemAnnotation>();
            ComponentResults = new List<ComponentAnnotation>();
            MatCatResults = new List<MaterialCategoryAnnotation>();
            SubComponentResults = new List<SubComponentAnnotation>();
            ComponentTypeResults = new List<ComponentTypeAnnotation>();
        }
    }
}