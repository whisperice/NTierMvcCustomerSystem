using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.Common
{
    public class CommonException : Exception
    {
        public CommonException()
        {
        }

        public CommonException(string message) : base(message)
        {
        }

        public CommonException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
