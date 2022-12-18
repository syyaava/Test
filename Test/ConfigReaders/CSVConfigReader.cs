using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test
{
    public class CSVConfigReader : IConfigReader
    {
        public string FilesFormat { get; } = "csv";

        public IEnumerable<T> ReadConfigFromFile<T>(string path)
        {
            try
            {
                List<T> config = new List<T>();
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var sr = new StreamReader(fs);
                    while (!sr.EndOfStream)
                    {
                        var csv = sr.ReadLine();

                        if (string.IsNullOrEmpty(csv))
                            throw new DeserializeException($"Exception when trying to deserialize an object from CSV. " +
                                                    $"There is no content in the file. Path to file: {path}.");

                        var rawConfig = GetDefaultConfigObject<T>();

                        SetPropValues(path, rawConfig, csv);
                        config.Add(rawConfig);
                    }
                    return config;
                }

            }
            catch (InvalidOperationException ex)
            {
                throw new DeserializeException($"Exception when trying to deserialize an object from CSV. " +
                                               $"Exception message: {ex.Message} Path: {path}.");
            }
        }

        private void SetPropValues<T>(string path, T? config, string? csv)
        {
            var propValues = csv.Split(';');
            var props = typeof(T).GetProperties();

            if (propValues.Length != props.Length)
                throw new DeserializeException($"The number of public fields does not match the number of values read. Path to file: {path}.");

            for (var i = 0; i < propValues.Length; i++)
            {
                props[i].SetValue(config, propValues[i]);
            }
        }

        private T GetDefaultConfigObject<T>()
        {
            T? config;
            var constructor = typeof(T).GetConstructor(new Type[] { });
            config = (T)constructor?.Invoke(new object[] { });

            if (config == null || config.Equals(default))
                throw new DeserializeException($"Exception when trying to deserialize an object from CSV. " +
                                        $"The {config} object contains the default value for {typeof(T)}.");
            return config;
        }
    }
}
