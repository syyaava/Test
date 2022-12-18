using System.Xml;
using System.Xml.Serialization;

namespace Test
{
    public class XMLConfigReader : IConfigReader
    {
        public string FilesFormat { get; } = "xml";

        public IEnumerable<T> ReadConfigFromFile<T>(string path)
        {
            try
            {
                List<T> config = new List<T>();
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var xmlSerializer = new XmlSerializer(typeof(List<T>));
                    if (xmlSerializer.CanDeserialize(XmlReader.Create(fs)))
                    {
                        fs.Position = 0;
                        config = (List<T>)xmlSerializer.Deserialize(fs);
                    }
                    else
                    {
                        fs.Position = 0;
                        xmlSerializer = new XmlSerializer(typeof(T));
                        if (xmlSerializer.Deserialize(fs) is T t)
                            config = new List<T>() { t };
                    }
                    

                    if (config == null || config.Equals(default) || config.Count() == 0)
                        throw new DeserializeException($"Exception when trying to deserialize an object from XML. " +
                                                $"The {config} object contains the default value for {typeof(T)} or have not items.");

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

        private bool AnyPropIsNull<T>(IEnumerable<T> config)
        {
            foreach (var item in config)
            {
                if (typeof(T).GetProperties().Any(p => p.GetValue(item) is null))
                    return true;
            }
            return false;
        }
    }
}
