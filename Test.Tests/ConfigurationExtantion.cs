using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Test
{
    public static class ConfigurationExtantion
    {
        public static string ToCSV(this Configuration config)
        {
            var csv = "";
            var props = typeof(Configuration).GetProperties();

            for(var i = 0; i < props.Length; i++)
            { 
                var prop = props[i];
                var propValue = prop?.GetValue(config);
                csv += propValue is null ? "" : $"{propValue};";
            }

            csv = csv.TrimEnd(';');

            return csv;
        }
    }
}
