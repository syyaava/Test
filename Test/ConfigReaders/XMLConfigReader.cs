using System.Xml.Serialization;

namespace Test
{
    public class XMLConfigReader : IConfigReader
    {
        public string FilesFormat { get; } = "xml";

        public T ReadConfigFromFile<T>(string path)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                T? config = default;
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (xmlSerializer.Deserialize(fs) is T t)
                        config = t;

                    if (config == null || config.Equals(default))
                        throw new DeserializeException($"Exception when trying to deserialize an object from XML. " +
                                                $"The {config} object contains the default value for {typeof(T)}.");

                    if (AnyPropIsNull<T>(config))
                        throw new DeserializeException($"Some object properties have null value. Path to file {path}.");

                    return config;
                }

            }
            catch (InvalidOperationException ex)
            {
                throw new DeserializeException($"Exception when trying to deserialize an object from XML. " +
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
