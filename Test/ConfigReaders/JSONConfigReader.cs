using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
    public class JSONConfigReader : IConfigReader
    {
        public string FilesFormat { get; } = "json";

        public T ReadConfigFromFile<T>(string path)
        {
            try
            {
                T? config = default;
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var sr = new StreamReader(fs);
                    var json = sr.ReadToEnd();

                    if (string.IsNullOrEmpty(json))
                        throw new DeserializeException($"Exception when trying to deserialize an object from JSON. " +
                                                $"There is no content in the file. Path to file {path}.");

                    config = JsonSerializer.Deserialize<T>(json);

                    if (AnyPropIsNull<T>(config))
                        throw new DeserializeException($"Some object properties have null value. Path to file {path}.");

                    if (config == null || config.Equals(default))
                        throw new DeserializeException($"Exception when trying to deserialize an object from JSON. " +
                                                $"The {config} object contains the default value for {typeof(T)}.");

                    return config;
                }

            }
            catch (InvalidOperationException ex)
            {
                throw new DeserializeException($"Exception when trying to deserialize an object from JSON. " +
                                               $"Exception message: {ex.Message} Path: {path}.");
            }
        }

        private bool AnyPropIsNull<T>(T config)
        {
            if (typeof(T).GetProperties().Any(p => p.GetValue(config) is null))
                return true;
            return false;
        }
    }
}
