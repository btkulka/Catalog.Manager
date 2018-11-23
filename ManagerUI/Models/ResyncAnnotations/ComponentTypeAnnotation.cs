using CatalogData;
using CatalogData.Classes.CMOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models.ResyncAnnotations
{
    public class ComponentTypeAnnotation
    {
        public ComponentType ComponentType { get; set; }
        public string Annotation { get; set; }

        public ComponentTypeAnnotation(ComponentType compType, string a)
        {
            ComponentType = compType;
            Annotation = a;
        }

        internal static bool HasBeenUpdated(ComponentType compType, CMOComponentType cmo)
        {
            return compType.CompTypeDescription != cmo.Description;
        }
    }
}