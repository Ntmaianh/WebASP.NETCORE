using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Utilities.Exceptionss
{
    // lớp này để tự viết ra những cái exception 
    internal class eShopException : Exception
    {
        public eShopException()
        {
        }

        public eShopException(string message)
            : base(message)
        {
        }

        public eShopException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
