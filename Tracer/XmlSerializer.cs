using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace Tracer
{
    class XmlSerializer : ISerializer
    {
        // Настройки 
        private XmlWriterSettings settings;

        protected readonly DataContractSerializer xmlSerializer;

        public void Serialize(TraceResult result, Stream stream)
        {
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                xmlSerializer.WriteObject(writer, result);
            }

        }

        public XmlSerializer()
        {
            // Настройки отступов
            settings = new XmlWriterSettings { Indent = true };
            xmlSerializer = new DataContractSerializer(typeof(TraceResult));
        }
    }
}
