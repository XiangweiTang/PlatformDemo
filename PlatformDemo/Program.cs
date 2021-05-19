using System;

namespace PlatformDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Argument arg = new Argument(args);
            PlatformHelper.RunWorkflow(arg);
        }
    }
}
