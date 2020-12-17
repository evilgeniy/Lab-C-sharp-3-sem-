﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Config_Manager
{
    public class XML_parser<T> : IConfParser<T> where T : class
    {
        private readonly string xmlPath = null;
        private readonly string xsdPath = null;

        public XML_parser(string xmlPath)
        {
            this.xmlPath = xmlPath;

            if (File.Exists(Path.ChangeExtension(xmlPath, "xsd")))
            {
                xsdPath = Path.ChangeExtension(xmlPath, "xsd");
            }
        }

        public T Parse()
        {
            if (xsdPath != null && !ValidateXml(xmlPath, xsdPath))
            {
                return null;
            }

            try
            {
                var xDocument = XDocument.Load(xmlPath);
                var elements =
                    from element in xDocument.Elements(typeof(T).Name).DescendantsAndSelf()
                    select element;

                var xmlFormat = elements.First().ToString();
                var xmlSerializer = new XmlSerializer(typeof(T));

                using (TextReader textReader = new StringReader(xmlFormat))
                {
                    return xmlSerializer.Deserialize(textReader) as T;
                }
            }
            catch (Exception ex)
            {
                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("Error: " + ex.Message);
                }

                return null;
            }
        }

        private bool ValidateXml(string xmlPath, string xsdPath)
        {
            try
            {
                var settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsdPath));

                var xmlReader = XmlReader.Create(xmlPath, settings);
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);
                return true;
            }
            catch (Exception ex)
            {
                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("Validation error: " + ex.Message);
                }

                return false;
            }
        }
    }
}

