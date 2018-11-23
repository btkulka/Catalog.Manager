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

        /*  NAME:   deleteMaterialCategory
         *  DESC:   Calls the CatalogFacade to delete the requested Material Category
         *  PARAM:  {0} matCatId - the Id of the material category to be deleted
         *  RETURN: a boolean indicating whether or not the call was successful.
         */
        public bool deleteMaterialCategory(int matCatId)
        {
            try
            {
                return cf.DeleteCatalogMaterialCategory(matCatId);
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an error while attempting to delete Material Category [ID: {0}]: {1}", matCatId, ex.Message), ex);

                return false;
            }
        }

    }
}
