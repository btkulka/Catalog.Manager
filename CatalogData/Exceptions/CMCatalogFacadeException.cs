using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogData.Exceptions
{
    public class CMCatalogFacadeException : CMException
    {
        public CMCatalogFacadeException() : base("An error occurred at the CatalogFacade level.")
        {
            ErrorCode = "550";
        }

        public CMCatalogFacadeException(string message) : base(message)
        {
            ErrorCode = "550";
        }

        public CMCatalogFacadeException(string message, Exception ex) : base(message, ex)
        {
            ErrorCode = "550";
        }
    }
}
