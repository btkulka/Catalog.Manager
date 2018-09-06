using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogData.Exceptions
{
    public class CMBUILDERResponseException : CMException
    {

        public CMBUILDERResponseException() : base()
        {
            ErrorCode = "551";
        }

        public CMBUILDERResponseException(string message) : base(message)
        {
            ErrorCode = "551";
        }

        public CMBUILDERResponseException(string message, Exception ex) : base(message, ex)
        {
            ErrorCode = "551";
        }

    }
}
