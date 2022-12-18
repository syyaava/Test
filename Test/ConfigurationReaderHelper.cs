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

        public OperationResult<IEnumerable<Configuration>> GetConfigFromFile(string fileName)
        {
            try
            {
                var reader = SelectReader(fileName);
                var config = reader.ReadConfigFromFile<Configuration>(fileName);
                var result = new OperationResult<IEnumerable<Configuration>>(config);
                return result;
            }
            catch(ArgumentNullException ex)
            {
                return new OperationResult<IEnumerable<Configuration>>(null, OperationResult<IEnumerable<Configuration>>.StatusCode.Error, 
                    ex);
            }
            catch(DeserializeException ex)
            {
                return new OperationResult<IEnumerable<Configuration>>(null, OperationResult<IEnumerable<Configuration>>.StatusCode.Error,
                    ex);
            }
        }

        IConfigReader SelectReader(string fileName)
        {
            var fileExtention = new string(fileName.Reverse().TakeWhile(c => c != '.').Reverse().ToArray()).ToLower();
            var reader = configReaders.Where(x => x.FilesFormat.ToLower() == fileExtention.ToLower()).FirstOrDefault();
            if (reader is not null)
                return reader;
            else
                throw new ArgumentNullException($"Failed to select reader. File: {fileName}.");
        }
    }
}
