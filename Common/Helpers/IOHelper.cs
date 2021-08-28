using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Common
{
    public static class IOHelper
    {
        public static IEnumerable<string> ReadEmbed(string path, string asmbName = "Common")
        {
            Assembly asmb = Assembly.Load(asmbName);
            using (StreamReader sr = new StreamReader(asmb.GetManifestResourceStream(path)))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                    yield return s;
            }
        }
    }
}
