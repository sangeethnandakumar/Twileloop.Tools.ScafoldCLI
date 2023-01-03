using System.Xml;
using System.Xml.Serialization;

namespace Twileloop.Tools.ScafoldCLI.Helpers
{
    public static class Helpers
    {
        public static string SerializeToXml(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  "
                }))
                {
                    serializer.Serialize(xmlWriter, obj);
                    return stringWriter.ToString();
                }
            }
        }

        public static object DeserializeFromXml(string xml, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);

            using (StringReader stringReader = new StringReader(xml))
            {
                return serializer.Deserialize(stringReader);
            }
        }
    }
}
