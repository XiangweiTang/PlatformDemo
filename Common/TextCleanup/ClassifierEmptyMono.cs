using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ClassifierEmptyMono : ClassifierMono
    {
        public override int Classify(string s)
        {
            return s.Length;
        }
    }
}
