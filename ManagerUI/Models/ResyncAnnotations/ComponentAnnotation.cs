using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CatalogData;
using CatalogData.Classes.CMOs;

namespace ManagerUI.Models.ResyncAnnotations
{
    public class ComponentAnnotation
    {
        public Component Component { get; set; }
        public string Annotation { get; set; }

        public ComponentAnnotation(Component comp, string a)
        {
            Component = comp;
            Annotation = a;
        }

        internal static bool HasBeenUpdated(Component comp, CMOComponent cmo)
        {
            bool updated = false;

            updated = updated | cmo.IsEquip != comp.IsEquip;
            updated = updated | cmo.IsUII != comp.IsUii;
            updated = updated | cmo.SystemId != comp.SystemId;
            updated = updated | cmo.UIICode != comp.UiiCode;

            return updated;
        }
    }
}