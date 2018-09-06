using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.Components
{
    public partial class CatalogFacade
    {
        /*  NAME:   CreateCatalogComponent
         *  DESC:   Creates a new Catalog Component in the BUILDER database.
         *  PARAMS: {0} description - a description of the system
         *          {1} systemId - the component's parent system
         *          {2} uiiCode - the uiiCode that identifies the system in the catalog
         *          {3} isUII - indicates if the system is UII format
         *          {4} isEquip - indicates if the system is Equipment
         *  RETURN: The newly created Catalog System.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void CreateCatalogSystems(string description, int systemId, string uiiCode, bool isUII, bool isEquip)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given Component [Desc: {0}]: {1}", description, ex.Message), ex);
            }
        }
    }
}
