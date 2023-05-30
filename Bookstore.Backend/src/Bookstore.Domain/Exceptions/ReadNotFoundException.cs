using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Exceptions
{
    public class ReadNotFoundException : Exception
    {
        public ReadNotFoundException() : base($"Read does not exist")
        {

        }
    }
}
