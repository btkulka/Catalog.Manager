using CatalogData;
using CatalogData.Classes.CMOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models.ResyncAnnotations
{
    public class SubComponentAnnotation
    {
        public SubComponent SubComponent { get; set; }
        public string Annotation { get; set; }

        public SubComponentAnnotation(SubComponent subComp, string a)
        {
            SubComponent = subComp;
            Annotation = a;
        }

        internal static bool HasBeenUpdated(SubComponent subComp, CMOSubComponent cmo)
        {
            bool updated = false;

            updated = updated | cmo.CompUnitsId != subComp.SubCompUnitId;
            updated = updated | cmo.Description != subComp.SubComponentDescription;

            return updated;
        }
    }
}