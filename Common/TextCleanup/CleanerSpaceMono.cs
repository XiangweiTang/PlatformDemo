using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CleanerSpaceMono : CleanerMono
    {
        public override string Clean(string s)
        {
            return PreDefinedRegex.SpacesReg.Replace(s, " ").Trim();
        }
    }
}
