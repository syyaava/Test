using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ListFileGetter : IListFilesGetter
    {
        public IEnumerable<string> GetConfigFiles(string path, string fileType)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory {Directory.GetCurrentDirectory() + "/" + path} not found.");

            var files = Directory.GetFiles(path, $"*.{fileType.ToLower()}");
            return files;
        }
    }
}
