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
        /*  NAME:   getMaterialCategories
         *  DESC:   Retrieves all available Catalog Material Categories from the CatalogFacade and converts them to CMOs.
         *  RETURN: A list of all available Catalog Material Categories as a list of CMOs.
         */
        public List<CMOMaterialCategory> getMaterialCategories()
        {
            // Initialize return value
            List<CMOMaterialCategory> CMOResults = new List<CMOMaterialCategory>();

            try
            {
                // Fetch all Material Categories
                var matCats = cf.GetCatalogMaterialCategories();

                // Convert to from Descriptions CMOs
                foreach(MaterialCategoryDescription mcd in matCats)
                {
                    CMOMaterialCategory cmo = new CMOMaterialCategory()
                    {
                        CatalogDescription = mcd.CatDesc,
                        MaterialCategoryId = mcd.ID,
                        StatusCode = "200",
                        StatusDetail = "Success"
                    };

                    CMOResults.Add(cmo);
                }
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while retrieving all Components: {0}", ex.Message), ex);

                throw ex;
            }

            return CMOResults;
        }

        /*  NAME:   getMaterialCategory
         *  DESC:   Retrieves the Catalog Material Category associated with the given ID.
         *  PARAM:  {0} matCatId - the requested Material Category Id
         *  RETURN: The requested Material Category as a CMO.
         */
        public CMOMaterialCategory getMaterialCategory(int matCatId)
        {
            // Initialize result
            CMOMaterialCategory Result = null;

            try
            {
                // Fetch from CatalogFacade
                var matCat = cf.GetCatalogMaterialCategory(matCatId);

                CMOMaterialCategory cmo = new CMOMaterialCategory()
                {
                    CatalogDescription = matCat.CatDesc,
                    MaterialCategoryId = matCat.ID,
                    StatusCode = "200",
                    StatusDetail = "Success"
                };

                Result = cmo;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while retrieving all Components: {0}", ex.Message), ex);

                throw ex;
            }

            return Result;
        }

    }
}
