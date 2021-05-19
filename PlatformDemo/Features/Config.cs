using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformDemo
{
    public abstract class Config
    {
        public virtual void PostSetConfig(Argument arg)
        {
            // DO nothing here unless neccessary.
        }
    }
}
