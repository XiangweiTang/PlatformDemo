using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlatformDemo.HelloWorld
{
    [XmlRoot("HelloWorld")]
    public class ConfigHelloWorld:Config
    {
        public string Name { get; set; } = "Bobo";
    }
}
