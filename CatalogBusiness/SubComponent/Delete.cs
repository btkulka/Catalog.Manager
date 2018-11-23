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

        /*  NAME:   deleteSubComponent
        *  DESC:   Calls the CatalogFacade to delete the requested SubComponent
        *  PARAM:  {0} subCompId - the Id of the SubComponent to be deleted
        *  RETURN: a boolean indicating whether or not the call was successful.
        */
        public bool deleteSubComponent(int subCompId)
        {
            try
            {
                // Call the Facade
                return cf.DeleteCatalogSubComponent(subCompId);
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an error while attempting to delete SubComponent [ID: {0}]: {1}", subCompId, ex.Message), ex);

                return false;
            }
        }

    }
}
