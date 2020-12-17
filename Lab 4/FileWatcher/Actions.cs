using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher_lab3
{
    class Actions
    {
        public Actions() { }
        public Actions(string CryptMode, string fileName) { }
        public static void Encryption(string inputFile, string outputFile, string key_)
        {
            try
            {

                string password = key_;
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                byte[] arr = File.ReadAllBytes(inputFile);

                string cryptFile = outputFile;
                using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create))
                {
                    using (RijndaelManaged RMCrypto = new RijndaelManaged())
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key),
                            CryptoStreamMode.Write))
                        {
                            foreach (byte bt in arr)
                            {
                                cs.WriteByte(bt);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("File encryption error: " + e.Message);
                }
            }
        }
        public static void Decryption(string inputFile, string outputFile, string key_)
        {
            string password = key_;
            try
            {

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);
            var bt = new List<byte>();

            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                using (RijndaelManaged RMCrypto = new RijndaelManaged())
                {


                    using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read))
                    {
                        int data;
                        while ((data = cs.ReadByte()) != -1)
                        {
                            bt.Add((byte)data);
                        }
                    }
                }
                using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                {
                    foreach (var b in bt)
                    {
                        fsOut.WriteByte(b);
                    }
                }
            }
            }
            catch (Exception e)
            {
                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("File decryption error: " + e.Message);
                }

            }
        }
        public static void Compress(string sourceFile, string compressedFile)
        {
            try
            {

            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
            }
            catch (Exception e)
            {
                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("File compress error: " + e.Message);
                }
            }
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            try
            {

            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        
                    }
                }
            }
            }
            catch (Exception e)
            {
                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("File decompress error: " + e.Message);
                }
            }
        }

        public static void AddToArchive(string filePath, string arch_path)
        {
            var archivePath = arch_path;
            try
            {

            using (ZipArchive zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Update))
            {
                zipArchive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            }
            }
            catch (Exception e)
            {
                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("File archive error: " + e.Message);
                }
            }
        }
    }
}
