using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformDemo
{
    public abstract class Feature
    {
        public string WorkFolder { get; set; }
        protected Config ConfigGeneral = null;
        protected LocalCommon Common = null;
        public Feature() { }
        public void LoadAndRun(Config cfg, LocalCommon common)
        {
            ConfigGeneral = cfg;
            Common = common;
            SetConfig();
            Run();
        }
        protected abstract void SetConfig();
        protected abstract void Run();
    }
}
