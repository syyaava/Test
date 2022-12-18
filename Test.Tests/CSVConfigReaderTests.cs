using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tests
{
    public class CSVConfigReaderTests
    {
        IConfigReader reader;
        public CSVConfigReaderTests()
        {
            CreateJsonFiles();
            reader = new CSVConfigReader();
        }

        [Fact]
        public void ReadConfig_NotNullValueProps_ReturnConfig()
        {
            string fileName = "config2.csv";

            var config = reader.ReadConfigFromFile<Configuration>(fileName);

            Assert.NotNull(config);
            Assert.NotNull(config.Name);
            Assert.NotNull(config.Description);
        }


        [Fact]
        public void ReadConfig_OnePropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = "config1.csv";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        [Fact]
        public void ReadConfig_AllPropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = "config0.csv";

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
                using (var fs = new FileStream($"config{i}.csv", FileMode.Create, FileAccess.Write))
                {
                    var csv = config.ToCSV();
                    using(var sw = new StreamWriter(fs))
                        sw.WriteLine(csv);
                }
            }
        }
    }
}
