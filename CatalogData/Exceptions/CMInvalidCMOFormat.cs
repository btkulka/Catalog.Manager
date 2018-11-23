using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogData.Exceptions
{
    public class CMInvalidCMOFormat : CMBusinessException
    {

        public CMInvalidCMOFormat() : base()
        {
            ErrorCode = "541";
        }

        public CMInvalidCMOFormat(string Message) : base(Message)
        {
            ErrorCode = "541";
        }

        public CMInvalidCMOFormat(string Message, Exception ex) : base(Message, ex)
        {
            ErrorCode = "541";
        }

    }
}
