using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Demo.Document
{
    public class DocumentEntity : IXmlSerializable
    {
        public String Id { get; init; }

        public String[] Tags { get; init; }

        public JContainer Data { get; init; }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("id");
            writer.WriteString(this.Id);
            writer.WriteEndElement();
            foreach (string tag in this.Tags)
            {
                writer.WriteStartElement("tag");
                writer.WriteString(tag);
                writer.WriteEndElement();
            }
            writer.WriteStartElement("data");
            writer.WriteString(this.Data.ToString()); // todo this might be a bit more sophisticated
            writer.WriteEndElement();
        }
    }
}