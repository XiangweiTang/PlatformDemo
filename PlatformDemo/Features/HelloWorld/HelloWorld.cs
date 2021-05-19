using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformDemo.HelloWorld
{
    class HelloWorld : Feature
    {
        ConfigHelloWorld Config = null;
        protected override void Run()
        {
            Console.WriteLine($"Hello world {Config.Name}!");
            Console.WriteLine($"Validate Common: Python path is {Common.PythonPath}");
        }

        protected override void SetConfig()
        {
            Config = (ConfigHelloWorld)ConfigGeneral;
        }
    }
}
