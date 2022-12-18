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

        public IEnumerable<T> ReadConfigFromFile<T>(string path)
        {
            try
            {
                List<T> config = new List<T>();
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var sr = new StreamReader(fs);
                    var json = sr.ReadToEnd();

                    if (string.IsNullOrEmpty(json))
                        throw new DeserializeException($"Exception when trying to deserialize an object from JSON. " +
                                                $"There is no content in the file. Path to file: {path}.");
                    try
                    {
                        config = JsonSerializer.Deserialize<List<T>>(json);
                    }
                    catch
                    {
                        config.Add(JsonSerializer.Deserialize<T>(json));
                    }

                    if (AnyPropIsNull<T>(config))
                        throw new DeserializeException($"Json deserialization. Some object properties have null value. Path to file {path}.");

                    if (config == null || config.Equals(default) || config.Count() == 0)
                        throw new DeserializeException($"Exception when trying to deserialize an object from JSON. " +
                                                $"The {config} object contains the default value for {typeof(T)} or have not items.");

                    return config;
                }

            }
            catch (InvalidOperationException ex)
            {
                throw new DeserializeException($"Exception when trying to deserialize an object from JSON. " +
                                               $"Exception message: {ex.Message} Path: {path}.");
            }
        }

        private bool AnyPropIsNull<T>(IEnumerable<T> config)
        {
            foreach (var item in config)
            {
                if (typeof(T).GetProperties().Any(p => p.GetValue(item) is null || string.IsNullOrEmpty((string)p.GetValue(item))))
                    return true;
            }
            return false;
        }
    }
}
