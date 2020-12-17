using DB_ServiceLayer.Generating;
using System;
using DB_DataAccess.Logging;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq.Expressions;
using DB_DataAccess;

namespace DB_ServiceLayer
{
    public class Generator<TEntity> : IGenerator<TEntity>
    {
        private string XmlString;
        private string XsdString;
        public ControlXML_Service<TEntity> XML_Service { get; set; }
        public void XML_Generating()
        {
            try
            {

                var xmlSerializer = new XmlSerializer(typeof(IEnumerable<TEntity>));
                using (var fs = new FileStream(XML_Service._Path, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, XML_Service.Entities);
                }

                using (var fs = new FileStream(XML_Service._Path, FileMode.Open))
                {

                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    XmlString = System.Text.Encoding.Default.GetString(array);
                }
            }
            catch (Exception e)
            {
                ILogger logger = new Log_InDB();
                logger.ErrorToDB(e.Message);
            }
        }
        public void XSD_Generating()
        {
            try
            {
                XmlReader reader = XmlReader.Create(new StringReader(XmlString));
                XmlSchemaInference schema = new XmlSchemaInference();
                XmlSchemaSet schemaSet = schema.InferSchema(reader);


                foreach (XmlSchema s in schemaSet.Schemas())
                {
                    using (var stringWriter = new StringWriter())
                    {
                        XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

                        using (var writer = XmlWriter.Create(stringWriter, settings))
                        {
                            s.Write(writer);
                            XsdString = stringWriter.ToString();

                        }
                    }
                }
                var XSDpath = Path.Combine(Path.GetFileNameWithoutExtension(XML_Service._Path) + ".xsd");
                using (var fs = new FileStream(XSDpath, FileMode.OpenOrCreate))
                {

                    byte[] array = Encoding.Default.GetBytes(XsdString);
                    fs.Write(array, 0, array.Length);
                }

            }
            catch (Exception e)
            {
                ILogger logger = new Log_InDB();
                logger.ErrorToDB(e.Message);
            }
        }

          
    }
}