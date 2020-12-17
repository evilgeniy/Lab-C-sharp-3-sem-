using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Config_Manager
{
    class JSON_parser<T> : IConfParser<T> where T : class
    {

        private readonly string jPath;

        public JSON_parser(string jPath)
        {
            this.jPath = jPath;
        }

        public T Parse()
        {
            using (var fileStream = new FileStream(jPath, FileMode.OpenOrCreate))
            {
                using (var document = JsonDocument.Parse(fileStream))
                {
                    var element = document.RootElement;

                    if (typeof(T).GetProperties().First().Name
                        != element.EnumerateObject().First().Name)
                    {
                        element = element.GetProperty(typeof(T).Name);
                    }
                    try
                    {
                        return JsonSerializer.Deserialize<T>(element.GetRawText());
                    }
                    catch (Exception ex)
                    {
                        using (var streamWriter = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"), true, Encoding.Default))
                        {
                            streamWriter.WriteLine("Error: " + ex.Message);
                        }

                        return null;
                    }

                }
            }
        }

    }
}