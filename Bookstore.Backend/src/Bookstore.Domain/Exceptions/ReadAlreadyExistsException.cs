﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Exceptions
{
    public class ReadAlreadyExistsException : Exception
    {
        public ReadAlreadyExistsException() : base($"Such read already exists")
        {

        }
    }
}
