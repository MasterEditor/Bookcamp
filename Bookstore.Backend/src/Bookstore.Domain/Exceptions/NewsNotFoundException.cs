using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Exceptions
{
    public class NewsNotFoundException : Exception
    {
        public NewsNotFoundException() : base($"Article does not exist")
        {

        }
    }
}
