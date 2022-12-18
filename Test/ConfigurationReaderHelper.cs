using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ConfigurationReaderHelper
    {
        List<IConfigReader> configReaders = new List<IConfigReader>();

        public ConfigurationReaderHelper()
        {

        }

        public ConfigurationReaderHelper(List<IConfigReader> configReaders)
        {
            this.configReaders = configReaders;
        }

        public Configuration GetConfigFromFile(string fileName)
        {
            var reader = SelectReader(fileName);
            return reader.ReadConfigFromFile<Configuration>(fileName);
        }

        IConfigReader SelectReader(string fileName)
        {
            var fileExtention = new string(fileName.Reverse().TakeWhile(c => c != '.').Reverse().ToArray()).ToLower();
            var reader = configReaders.Where(x => x.FilesFormat.ToLower() == fileExtention).FirstOrDefault();
            if (reader is not null)
                return reader;
            else
                throw new ArgumentNullException($"Failed to select reader");
        }
    }
}
