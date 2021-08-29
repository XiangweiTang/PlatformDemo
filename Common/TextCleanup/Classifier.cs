using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class ClassifierMono
    {
        public abstract int Classify(string s);
        public virtual bool Remove(string s)
        {
            return Classify(s) == 0;
        }
    }
    public abstract class ClassifierParallel
    {
        public abstract int Classify(SentencePair pair);
        public virtual bool Remove(SentencePair pair)
        {
            return Classify(pair) == 0;
        }
    }
}
