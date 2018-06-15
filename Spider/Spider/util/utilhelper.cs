using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.util
{
    internal class utilhelper
    {
        public static string CreateFilePath(string dirName, string fileName)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string dirPath = Path.Combine(appPath, dirName);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);

            }
            string filePath = Path.Combine(dirPath, fileName);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                }
            }
            return filePath;
        }
    }
}
