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
        /*  NAME:   createSubComponent
         *  DESC:   Takes a CMOSubComponent and converts it to a Description for the CatalogFacade.
         *  PARAM:  {0} subComp - the subComp object to be created
         *  RETURN: The same CMOSubComponent with updated status
         */
        public CMOSubComponent createSubComponent(CMOSubComponent subComp)
        {
            try
            {
                // assert required fields exist
                string missingField = "";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("Object is missing the required field '{0}'.", missingField));

                // Convert to desc
                SubComponentDescription desc = new SubComponentDescription()
                {
                    CompUnitsID = subComp.CompUnitsId,
                    Description = subComp.Description,
                    ID = subComp.SubComponentId
                };

                var result = cf.CreateCatalogSubComponent(desc);

                subComp.StatusCode = result.StatusCode;
                subComp.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an error while attempting to create SubComponent: {0}", ex.Message), ex);

                subComp.StatusCode = ((CMException)ex).ErrorCode;
                subComp.StatusDetail = ex.Message;
            }

            return subComp;
        }

    }
}
