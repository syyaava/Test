using System.Text.Json;
using System.Xml.Serialization;

namespace Test.Tests
{
    public class JSONConfigReaderTests
    {
        string path = "configs/";
        IConfigReader reader;
        public JSONConfigReaderTests()
        {           
            CreateJsonFiles();
            reader = new JSONConfigReader();
        }

        [Fact]
        public void ReadConfig_NotNullValueProps_ReturnConfig()
        {
            string fileName = path + "config2.json";

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
            string fileName = path + "config1.json";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        [Fact]
        public void ReadConfig_AllPropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = path + "config0.json";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        [Fact]
        public void ReadConfig_IEnumerableAllPropHaveNotNullValue_ReturnIEnumerableWithManyConfigs()
        {
            string fileName = path + "configs.json";

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
            string fileName = path + "configs2.json";

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

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (int i = 0; i < configs.Count; i++)
            {
                Configuration? config = configs[i];
                using (var fs = new FileStream($"{path}config{i}.json", FileMode.Create, FileAccess.Write))
                {
                    JsonSerializer.Serialize(fs, config);
                }
            }

            using (var fs = new FileStream($"{path}configs.json", FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize(fs, new List<Configuration>()
                {
                    new Configuration() { Name = "Config 4", Description = "Config 4 json"},
                    new Configuration() { Name = "Config 44", Description = "Config 44 json"},
                    new Configuration() { Name = "Config 444", Description = "Config 444 json"}
                });
            }

            using (var fs = new FileStream($"{path}configs2.json", FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize(fs, new List<Configuration>()
                {
                    new Configuration() { Description = "Config 4 json"},
                    new Configuration() { Name = "Config 44"},
                    new Configuration() { Name = "Config 444", Description = "Config 444 json"}
                });
            }
        }
    }
}
