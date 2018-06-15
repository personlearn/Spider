using Spider.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Service
{
    internal class SaveToTxt
    {
        private string path;
        private string filename;
        private string fullfilename;
        public SaveToTxt(string path,string filename)
        {
            this.path = path;
            this.filename = filename + ".txt";
            fullfilename = utilhelper.CreateFilePath(path, this.filename);
        }
        public bool save(string input)
        {
            try
            {
                FileInfo finfo = new FileInfo(fullfilename);
                using (FileStream fs = finfo.OpenWrite())
                {
                    StreamWriter w = new StreamWriter(fs);
                    w.BaseStream.Seek(0, SeekOrigin.End);
                    w.Write(input.Replace("\n", "\r\n") + "\n\r");
                    w.Write("------------------------------------\n\r");
                    w.Flush();
                    w.Close();
                }
                return true;
            }
            catch
            {
                return false;
            };
        }    
    }
}
