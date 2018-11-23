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
        /*  NAME:   createMaterialCategory
         *  DESC:   Takes a CMOMaterialCategroy, checks that it is formatted correctly, then converts it to a Description and sends to the CatalogFacade.
         *  PARAM:  {0} matCat - the new CMOMaterialCategory to be added to the BUILDER Catalog.
         *  RETURN: The same matCat CMO with updated Status Code and Details.
         */
        public CMOMaterialCategory createMaterialCategory(CMOMaterialCategory matCat)
        {
            try
            {
                // check for required fields
                string missingField = "";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("Object is missing required field '{0}'.", missingField));

                // convert to description
                MaterialCategoryDescription desc = new MaterialCategoryDescription(){
                    CatDesc = matCat.CatalogDescription,
                    ID = matCat.MaterialCategoryId
                };

                // Create Material Category 
                var result = cf.CreateCatalogMaterialCategory(desc);

                // Set Status
                matCat.StatusCode = result.StatusCode;
                matCat.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if(!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while attempting to create a Material Category: {0}", ex.Message), ex);

                matCat.StatusCode = ((CMException)ex).ErrorCode;
                matCat.StatusDetail = ex.Message;
            }

            return matCat;
        }

    }
}
