using System.Xml.Serialization;

namespace Test.Tests
{
    public class XMLConfigReaderTests
    {
        string path = "configs/";
        IConfigReader reader;
        public XMLConfigReaderTests()
        {
            CreateXMLFiles();
            reader = new XMLConfigReader();
        }
                
        [Fact]
        public void ReadConfig_NotNullValueProps_ReturnIEnumerableWithOneConfig()
        {
            string fileName = path + "config2.xml";

            var configs = reader.ReadConfigFromFile<Configuration>(fileName);

            foreach (var config in configs)
            {
                Assert.NotNull(config);
                Assert.NotNull(config.Name);
                Assert.NotNull(config.Description);
            }
        }

        
        [Fact]
        public void ReadConfig_OnePropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = path + "config1.xml";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }
        
        [Fact]
        public void ReadConfig_AllPropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = path + "config0.xml";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }
        
        [Fact]
        public void ReadConfig_IEnumerableAllPropHaveNotNullValue_ReturnIEnumerableWithManyConfigs()
        {
            string fileName = path + "configs.xml";

            var configs = reader.ReadConfigFromFile<Configuration>(fileName);

            Assert.True(configs.Count() > 1);
            foreach (var config in configs)
            {
                Assert.NotNull(config);
                Assert.NotNull(config.Name);
                Assert.NotNull(config.Description);
            }
        }

        [Fact]
        public void ReadConfig_IEnumerableAnyPropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = path + "configs2.xml";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        private void CreateXMLFiles()
        {
            var configs = new List<Configuration>()
            {
                new Configuration(),
                new Configuration() { Name = "Config 2"},
                new Configuration() { Name = "Config 3", Description = "Config 3"},
                new Configuration() { Name = "Config 4", Description = "Config 4 xml"}
            };

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var xmlSerializer = new XmlSerializer(typeof(Configuration));

            for (int i = 0; i < configs.Count; i++)
            {
                Configuration? config = configs[i];
                using (var fs = new FileStream($"{path}config{i}.xml", FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fs, config);
                }
            }

            xmlSerializer = new XmlSerializer(typeof(List<Configuration>));
            using (var fs = new FileStream($"{path}configs.xml", FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(fs, new List<Configuration>()
                {
                    new Configuration() { Name = "Config 5", Description = "Config 5 xml"},
                    new Configuration() { Name = "Config 55", Description = "Config 55 xml"},
                    new Configuration() { Name = "Config 555", Description = "Config 555 xml"}
                });
            }

            using (var fs = new FileStream($"{path}configs2.xml", FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(fs, new List<Configuration>()
                {
                    new Configuration() { Description = "Config 5 xml"},
                    new Configuration() { Name = "Config 55"},
                    new Configuration() { Name = "Config 555", Description = "Config 555 xml"}
                });
            }
        }
    }
}