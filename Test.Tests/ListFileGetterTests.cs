using System.Xml.Serialization;

namespace Test.Tests
{
    public class ListFileGetterTests
    {
        IListFilesGetter listFilesGetter;
        public ListFileGetterTests()
        {
            listFilesGetter = new ListFileGetter();
        }

        [Fact]
        public void GetConfigFiles_NotExistingDirectory_ThrowDeserializeException()
        {
            string path = "configs/config0.xml";

            Assert.Throws<DirectoryNotFoundException>(() => listFilesGetter.GetConfigFiles(path, "xml"));
        }

        [Fact]
        public void GetConfigFiles_ExistingDirectory_ThrowDeserializeException()
        {
            string path = "confs";

            Directory.CreateDirectory(path);
            CreateXMLFiles();
            var files = listFilesGetter.GetConfigFiles(path, "xml");

            Assert.Equal(Directory.GetFiles(path).Length, files.Count());
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
                using (var fs = new FileStream($"confs/config{i}.xml", FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fs, config);
                }
            }
        }
    }
}
