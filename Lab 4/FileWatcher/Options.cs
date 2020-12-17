using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher_lab3
{
    public class Options
    {
        public Store Store { get; set; }
        public Archive Archive { get; set; }
        public Crypting Crypting { get; set; }
    }

    public class Store
    {
        public string SourseFileName { get; set; }
        public string TargetDirectory { get; set; }
    }

    public class Archive 
    {
        public string ZipName { get; set; }
    }

    public class Crypting
    {
        public string EncryptionKey { get; set; }
    }
}
