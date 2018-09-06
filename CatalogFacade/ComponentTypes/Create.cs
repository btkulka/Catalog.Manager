using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.ComponentType
{
    public partial class CatalogFacade
    {
        /*  NAME:   CreateCatalogComponentType
         *  DESC:   Creates a new Catalog Component Type in the BUILDER database.
         *  PARAMS: {0} description - a description of the component type
         *  RETURN: The newly created Catalog Component Type.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void CreateCatalogComponentType(string description)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given Component Type[Desc: {0}]: {1}", description, ex.Message), ex);
            }
        }
    }
}
