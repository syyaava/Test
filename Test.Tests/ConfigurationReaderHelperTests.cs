using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tests
{
    public class ConfigurationReaderHelperTests
    {
        string path = "configs/";
        ConfigurationReaderHelper configHelper;

        public ConfigurationReaderHelperTests()
        {
            Thread.Sleep(100);
            configHelper = new ConfigurationReaderHelper(new List<IConfigReader>()
            {
                new JSONConfigReader(),
                new CSVConfigReader(),
                new XMLConfigReader(),
            });
        }

        [Fact]
        public void GetConfigFromFile_AllFilesCanRead_Success()
        {
            var files = Directory.GetFiles(path).Where(x => x.Contains("3")).ToArray();

            if (files.Length == 0)
                throw new Exception("No files for tests.");

            foreach(var file in files)
            {
                var config = configHelper.GetConfigFromFile(file);
                Assert.NotNull(config);
                Assert.NotNull(config.Result);
                Assert.True(config.Status == OperationResult<Configuration>.StatusCode.Success);
            }
        }

        [Fact]
        public void GetConfigFromFile_AllFilesCanRead_DeserializationException()
        {
            var files = Directory.GetFiles(path).Where(x => x.Contains("0")).ToArray();

            if (files.Length == 0)
                throw new Exception("No files for tests.");

            foreach(var file in files)
            {
                var config = configHelper.GetConfigFromFile(file);
                Assert.NotNull(config);
                Assert.IsType<DeserializeException>(config.Exception);
                Assert.Null(config.Result);
            }
        }

        [Fact]
        public void GetConfigFromFile_AllFilesCanRead_ArgumentNullException()
        {
            var files = Directory.GetFiles(path).Where(x => x.Contains(".txt")).ToArray();

            if (files.Length == 0)
                throw new Exception("No files for tests.");

            foreach(var file in files)
            {
                var config = configHelper.GetConfigFromFile(file);
                Assert.NotNull(config);
                Assert.IsType<ArgumentNullException>(config.Exception);
                Assert.Null(config.Result);
            }
        }
    }
}
