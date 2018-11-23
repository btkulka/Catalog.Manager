using CatalogData;
using CatalogData.Classes.CMOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models.ResyncAnnotations
{
    public class SystemAnnotation
    {
        public CatalogSystem System { get; set; }
        public string Annotation { get; set; }

        public SystemAnnotation(CatalogSystem sys, string a)
        {
            System = sys;
            Annotation = a;
        }

        public static bool HasBeenUpdated(CatalogSystem oldSys, CMOSystem newSys)
        {
            return oldSys.UiiCode != newSys.UIICode | oldSys.IsUii != newSys.IsUII | oldSys.SystemDescription != newSys.Description;
        }
    }
}