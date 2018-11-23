//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CatalogData
{
    using System;
    using System.Collections.Generic;
    
    public partial class BUILDERInstance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BUILDERInstance()
        {
            this.CatalogCredentials = new HashSet<CatalogCredential>();
            this.CatalogItems = new HashSet<CatalogItem>();
            this.CatalogSystems = new HashSet<CatalogSystem>();
            this.Components = new HashSet<Component>();
            this.ComponentTypes = new HashSet<ComponentType>();
            this.MaterialCategories = new HashSet<MaterialCategory>();
            this.SubComponents = new HashSet<SubComponent>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string RemoteServiceAddress { get; set; }
        public Nullable<System.DateTime> LastSynced { get; set; }
        public Nullable<bool> FlagDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CatalogCredential> CatalogCredentials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CatalogItem> CatalogItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CatalogSystem> CatalogSystems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Component> Components { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComponentType> ComponentTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialCategory> MaterialCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubComponent> SubComponents { get; set; }
    }
}
