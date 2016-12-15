using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Framework.Common.Exceptions
{
    public class CusException:Exception
    {
        public CusException(string message)
            : base(message) { }
    }
}
