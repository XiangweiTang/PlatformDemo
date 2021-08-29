using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Common
{
    public static class OpenSubtitle
    {
        public static OpenSubtitleMono LoadMono(string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(OpenSubtitleMono));
            using(StreamReader sr=new StreamReader(filePath))
            {
                return (OpenSubtitleMono)xs.Deserialize(sr);
            }
        }
    }
    [XmlRoot("document")]
    public struct OpenSubtitleMono
    {
        [XmlElement("s")]
        public OpenSubtitleSingleSentence[] Sentences { get; set; }
    }
    public struct OpenSubtitleSingleSentence
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("time")]
        public OpenSubtitleTime[] Times { get; set; }
        [XmlElement("w")]
        public OpenSubtitleWord[] Words { get; set; }
    }
    public struct OpenSubtitleTime
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
    public struct OpenSubtitleWord
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
