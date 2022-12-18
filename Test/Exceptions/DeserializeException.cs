using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class DeserializeException : Exception
    {
        public DeserializeException() : base() { }
        public DeserializeException(string message) : base(message) { }
    }
}
