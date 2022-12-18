using System.Xml.Serialization;

namespace Test.Tests
{
    public class XMLConfigReaderTests
    {
        IConfigReader reader;
        public XMLConfigReaderTests()
        {
            CreateXMLFiles();
            reader = new XMLConfigReader();
        }

        
        [Fact]
        public void ReadConfig_NotNullValueProps_ReturnConfig()
        {
            string fileName = "config2.xml";

            var config = reader.ReadConfigFromFile<Configuration>(fileName);

            Assert.NotNull(config);
            Assert.NotNull(config.Name);
            Assert.NotNull(config.Description);
        }

        
        [Fact]
        public void ReadConfig_OnePropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = "config1.xml";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }
        
        [Fact]
        public void ReadConfig_AllPropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = "config0.xml";

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
            
            var xmlSerializer = new XmlSerializer(typeof(Configuration));

            for (int i = 0; i < configs.Count; i++)
            {
                Configuration? config = configs[i];
                using (var fs = new FileStream($"config{i}.xml", FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fs, config);
                }
            }
        }
    }
}