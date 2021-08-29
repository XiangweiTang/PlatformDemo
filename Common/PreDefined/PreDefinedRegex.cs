using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Common
{
    public static class PreDefinedRegex
    {
        public static Regex SpacesReg { get; } = new Regex("\\s+", RegexOptions.Compiled);        
    }
}
