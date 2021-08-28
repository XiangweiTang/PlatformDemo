using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public static class XmlHelper
    {
        public static string GetValue(this XmlNode node, string xPath, string attribute = "")
        {
            if (xPath.Contains('/'))
            {
                int index = xPath.IndexOf('/');
                string head = xPath.Substring(0, index);
                string tail = xPath.Substring(index + 1);
                Sanity.Requires(node[head] != null, $"Node {head} doesn't exist.");
                return GetValue(node[head], tail, attribute);
            }
            var subNode = node;
            if (xPath != "")
            {
                subNode = node[xPath];
                Sanity.Requires(subNode != null, $"Node {xPath} doesn't exist.");
            }
            if (attribute == "")
                return subNode.InnerText;
            var attrib = subNode.Attributes[attribute];
            Sanity.Requires(attrib != null, $"Attribute {attribute} doesn't exist.");
            return attrib.Value;
        }
        public static void Save(this XmlElement element, string path)
        {
            using(XmlWriter xw = XmlWriter.Create(path))
            {
                element.WriteTo(xw);
            }
        }
    }
}
