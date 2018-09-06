using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.Systems
{
    public partial class CatalogFacade
    {

        /*  NAME:   CreateCatalogSystem
         *  DESC:   Creates a new Catalog System in the BUILDER database.
         *  PARAMS: {0} description - a description of the system
         *          {1} uiiCode - the uiiCode that identifies the system in the catalog
         *          {2} isUII - indicates if the system is UII format
         *  RETURN: The newly created Catalog System.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void CreateCatalogSystems(string description, string uiiCode, bool isUII )
        {
            try
            {

            }catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given System [Desc: {0}]: {1}", description, ex.Message), ex);
            }
        }

    }
}
