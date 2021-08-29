using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ClassifierAlphabetOnlyMono : ClassifierMono
    {
        public override int Classify(string s)
        {
            foreach(char c in s)
            {
                if (c < 'A')
                    return 0;
                if ('Z' < c && c < 'a')
                    return 0;
                if ('z' < c)
                    return 0;
            }
            return 1;
        }
    }
}
