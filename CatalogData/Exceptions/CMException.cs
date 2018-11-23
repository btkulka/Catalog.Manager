using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogData.Exceptions
{
    public abstract class CMException : Exception
    {
        public string ErrorCode;

        public CMException() : base()
        {

        }

        public CMException(string Message) : base(Message)
        {

        }

        public CMException(string Message, Exception ex) : base(Message, ex)
        {

        }
    }
}
