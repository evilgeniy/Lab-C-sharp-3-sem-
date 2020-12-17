﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config_Manager
{
    public class option_Manager
    {
        private readonly string path = null;

        public option_Manager(string path)
        {
            if (File.Exists(path))
            {
                this.path = (Path.GetExtension(path) == ".xml"
                    || Path.GetExtension(path) == ".json") ? path : null;
            }
            else if (Directory.Exists(path))
            {
                var fileEntries = from file in Directory.GetFiles(path)
                                  where
                                    Path.GetExtension(file) == ".xml" ||
                                    Path.GetExtension(file) == ".json"
                                  select file;

                this.path = fileEntries.Count() != 0 ? fileEntries.First() : null;
            }
        }

        public T GetOptions<T>() where T : class
        {
            if (path is null)
            {
                using (var streamWriter = new StreamWriter(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                    true, Encoding.Default))
                {
                    streamWriter.WriteLine("Configuration file not found.");
                }

                return null;
            }

            var provider = new Manager<T>(path);
            return provider.configurationParser.Parse();
        }
    }

}