using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlatformDemo
{
    [XmlRoot("Common")]
    public class LocalCommon: Config
    {        
        public string PythonPath { get; set; } = "Python.exe";
    }
}
