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
        /*  NAME:   deleteCatalogItem
         *  DESC:   Calls the CatalogFacade delete Catalog Item method
         *  PARAM:  {0} cmcId - the Id of the Catalog Item to be deleted
         *  RETURN: a boolean indicating if the delete was successful.
         */
        public bool deleteCatalogItem(int cmcId)
        {
            try
            {
                return cf.DeleteCatalogItem(cmcId);
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager was unable to delete given Catalog Item [ID: {0}]: {1}", cmcId, ex.Message));

                return false;
            }
        }
    }
}
