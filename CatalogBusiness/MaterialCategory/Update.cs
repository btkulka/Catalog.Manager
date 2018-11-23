using Catalog.Facade.BUILDERCatalog;
using CatalogData.Classes.CMOs;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogBusiness
{
    public partial class CatalogBusiness
    {
        /*  NAME:   updateMaterialCategory
         *  DESC:   Takes a CMO, converts it to a description, then passes it to the Facade to update
         *  PARAM:  {0} cmo - a CMOMaterialCategory with all required fields
         *  RETURN: The same CMOMaterialCategory with updated Status.
         */
        public CMOMaterialCategory updateMaterialCategory(CMOMaterialCategory cmo)
        {
            try
            {
                // Check required fields
                string missingField = "";

                if (cmo.MaterialCategoryId == 0)
                    missingField = "MaterialCategoryId";

                // Convert to Description
                MaterialCategoryDescription desc = new MaterialCategoryDescription()
                {
                    CatDesc = cmo.CatalogDescription,
                    ID = cmo.MaterialCategoryId
                };

                // Update Material Category
                var result = cf.UpdateCatalogMaterialCategory(desc);

                cmo.StatusCode = result.StatusCode;
                cmo.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if(!(ex is CMException))
                    ex = new CMBusinessException(String.Format(""), ex);

                cmo.StatusCode = ((CMException)ex).ErrorCode;
                cmo.StatusDetail = ex.Message;
            }

            return cmo;
        }

    }
}
