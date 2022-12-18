using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tests
{
    public class CSVConfigReaderTests
    {
        string path = "configs/";
        IConfigReader reader;
        public CSVConfigReaderTests()
        {
            CreateCSVFiles();
            reader = new CSVConfigReader();
        }

        [Fact]
        public void ReadConfig_NotNullValueProps_ReturnConfig()
        {
            string fileName = path + "config2.csv";

            var config = reader.ReadConfigFromFile<Configuration>(fileName);

            Assert.NotNull(config);
            Assert.NotNull(config.Name);
            Assert.NotNull(config.Description);
        }


        [Fact]
        public void ReadConfig_OnePropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = path + "config1.csv";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        [Fact]
        public void ReadConfig_AllPropHaveNullValue_ThrowDeserializeException()
        {
            string fileName = path + "config0.csv";

            Assert.Throws<DeserializeException>(() => reader.ReadConfigFromFile<Configuration>(fileName));
        }

        private void CreateCSVFiles()
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
                using (var fs = new FileStream($"{path}config{i}.csv", FileMode.Create, FileAccess.Write))
                {
                    var csv = config.ToCSV();
                    using(var sw = new StreamWriter(fs))
                        sw.WriteLine(csv);
                }
            }
        }
    }
}
