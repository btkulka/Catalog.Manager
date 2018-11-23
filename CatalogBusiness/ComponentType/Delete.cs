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

        public bool deleteComponentType(int compId)
        {
            try
            {
                return cf.DeleteCatalogComponentType(compId);
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an error while attempting to delete Component Type [ID: {0}]: {1}", compId, ex.Message), ex);

                return false;
            }
        }

    }
}
