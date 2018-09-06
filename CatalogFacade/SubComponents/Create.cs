using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.SubComponents
{
    public partial class CatalogFacade
    {
        /*  NAME:   CreateCatalogSubComponent
         *  DESC:   Creates a new Catalog SubComponent in the BUILDER database.
         *  PARAMS: {0} description - a description for the sub component
         *          {1} unitId - the id of the unit type
         *  RETURN: The newly created Sub Component
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void CreateCatalogSubComponent(string description, int unitId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the requested SubComponent [ID: {0}]: {1}", description, ex.Message), ex);
            }
        }
    }
}
