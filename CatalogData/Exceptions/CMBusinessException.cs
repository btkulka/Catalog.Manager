using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogData.Exceptions
{
    public class CMBusinessException : CMException
    {

        public CMBusinessException() : base()
        {
            ErrorCode = "540";
        }

        public CMBusinessException(string m) : base(m)
        {
            ErrorCode = "540";
        }

        public CMBusinessException(string m, Exception ex) : base(m, ex)
        {
            ErrorCode = "540";
        }

    }
}
