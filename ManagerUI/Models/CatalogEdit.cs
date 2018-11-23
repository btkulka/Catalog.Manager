using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models
{
    public class CatalogEdit
    {
        public enum ENTITY_TYPE
        {
            CatalogItem,
            System,
            Component,
            ComponentType,
            MaterialCategory,
            SubComponent
        }

        public ENTITY_TYPE Type;        // an enumeration to annotate the type of the object
        public object Entity;           // This is just a wrapper class for any 
    }
}