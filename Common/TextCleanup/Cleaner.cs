using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class CleanerMono
    {
        public abstract string Clean(string s);
    }
    public abstract class CleanParallel
    {
        public abstract string Clean(SentencePair pair);
    }
}
