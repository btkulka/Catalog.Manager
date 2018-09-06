using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.SubComponents
{
    public partial class CatalogFacade
    {
        /*  NAME:   DeleteCatalogSubComponent
         *  DESC:   Deletes the Catalog SubComponent with the associated Id from the BUILDER database.
         *  PARAMS: {0} subCompId - the sub component Id
         *  RETURN: The newly deleted Sub Component
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void DeleteCatalogSubComponent(int subCompId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the requested SubComponent [ID: {0}]: {1}", subCompId, ex.Message), ex);
            }
        }
    }
}
