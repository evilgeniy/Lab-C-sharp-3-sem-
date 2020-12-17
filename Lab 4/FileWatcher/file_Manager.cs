  
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Config_Manager;


namespace FileWatcher_lab3
{
    class File_Manager
    {
        private readonly Options configOptions;
        private bool isEnabled = true;

        public File_Manager()
        {
            var optionsManager = new option_Manager(AppDomain.CurrentDomain.BaseDirectory);
            configOptions = optionsManager.GetOptions<Options>();
        }

        public void Start()
        {
            if (configOptions != null)
            {
                FileTransfer();
            }

            while (isEnabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            isEnabled = false;
        }

        private void FileTransfer()
        {
            var control = new object();

            lock (control)
            {
                var path = Path.GetDirectoryName(configOptions.Store.SourseFileName);
                var dirInfo = new DirectoryInfo(path);
                var fileName = Path.GetFileName(configOptions.Store.SourseFileName);
                var date = DateTime.Now;
                var subPath = $"{date.ToString("yyyy", DateTimeFormatInfo.InvariantInfo)}\\" +
                   $"{date.ToString("MM", DateTimeFormatInfo.InvariantInfo)}\\" +
                   $"{date.ToString("dd", DateTimeFormatInfo.InvariantInfo)}";
                var newPath = path +
                   $"\\{date.ToString("yyyy", DateTimeFormatInfo.InvariantInfo)}\\" +
                   $"{date.ToString("MM", DateTimeFormatInfo.InvariantInfo)}\\" +
                   $"{date.ToString("dd", DateTimeFormatInfo.InvariantInfo)}\\" +
                   $"{Path.GetFileNameWithoutExtension(fileName)}_" +
                   $"{date.ToString(@"yyyy_MM_dd_HH_mm_ss", DateTimeFormatInfo.InvariantInfo)}" +
                   $"{Path.GetExtension(fileName)}";
                var compressedPath = Path.ChangeExtension(newPath, "gz");
                var newCompressedPath = Path.Combine(configOptions.Store.TargetDirectory,
                    Path.GetFileName(compressedPath));
                var decompressedPath = Path.ChangeExtension(newCompressedPath, "txt");

                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                dirInfo.CreateSubdirectory(subPath);
                File.Move(configOptions.Store.SourseFileName, newPath);
                Actions.Encryption(newPath, newPath,
                    configOptions.Crypting.EncryptionKey);
                Actions.Compress(newPath, compressedPath);
                File.Move(compressedPath, newCompressedPath);
                Actions.Decompress(newCompressedPath, decompressedPath);
                Actions.Decryption(decompressedPath, decompressedPath,
                    configOptions.Crypting.EncryptionKey);
                Actions.AddToArchive(decompressedPath,
                    configOptions.Archive.ZipName);
                File.Delete(newPath);
                File.Delete(newCompressedPath);
                File.Delete(decompressedPath);
            }
        }
    }
}
