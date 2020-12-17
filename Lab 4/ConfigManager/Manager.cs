using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config_Manager
{
    public class Manager<T> where T : class
    {
        public readonly IConfParser<T> configurationParser;

        public Manager(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".xml":
                    configurationParser = new XML_parser<T>(path);
                    break;

                case ".json":
                    configurationParser = new JSON_parser<T>(path);
                    break;
            }
        }
    }
}