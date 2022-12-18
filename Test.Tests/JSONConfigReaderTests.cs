using System.Text.Json;
using System.Xml.Serialization;

namespace Test.Tests
{
    public class JSONConfigReaderTests
    {
        IConfigReader reader;
        public JSONConfigReaderTests()
        {           
            CreateJsonFiles();
            reader = new JSONConfigReader();
        }

        [Fact]
        public void ReadConfig_NotNullValueProps_ReturnConfig()
        {
            string fileName = "config2.json";

            var config = reader.ReadConfigFromFile<Configuration>(fileName);

            Assert.NotNull(config);
            Assert.NotNull(config.Name);
            Assert.NotNull(config.Description);
        }


        [Fact]
        public void ReadConfig_OnePropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = "config1.json";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        [Fact]
        public void ReadConfig_AllPropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = "config0.json";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        private void CreateJsonFiles()
        {
            var configs = new List<Configuration>()
            {
                new Configuration(),
                new Configuration() { Name = "Config 2"},
                new Configuration() { Name = "Config 3", Description = "Config 3"},
                new Configuration() { Name = "Config 4", Description = "Config 4 json"}
            };

            for (int i = 0; i < configs.Count; i++)
            {
                Configuration? config = configs[i];
                using (var fs = new FileStream($"config{i}.json", FileMode.Create, FileAccess.Write))
                {
                    JsonSerializer.Serialize(fs, config);
                }
            }
        }
    }
}
