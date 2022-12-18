using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Configuration
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Configuration {Name}. {Description}";
        }
    }
}
