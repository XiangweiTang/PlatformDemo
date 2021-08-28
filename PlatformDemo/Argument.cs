using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformDemo
{
    public class Argument
    {
        public Dictionary<string, string> DashArgDict { get; set; } = new Dictionary<string, string>();
        public List<string> FreeArgList { get; set; } = new List<string>();
        public ArgumentType ArgType { get; set; } = ArgumentType.Xml;
        public bool PostSetFlag { get; set; } = false;
        public bool SkipConfirmFlag { get; set; } = false;
        public string XmlConfigFilePath { get; set; } = "Config.xml";
        public Argument(string[] args)
        {
            ArgType = ArgumentType.Test;
        }
    }
    [Flags]
    public enum ArgumentType
    {
        NA=
            0b0000_0000,
        Xml=
            0b0000_0001,
        Test=
            0b0000_0010,
    }
}
