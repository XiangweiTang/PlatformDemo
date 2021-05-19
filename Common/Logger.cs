﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Common
{
    public static class Logger
    {
        public static string LogPath { get; set; } = "Log.txt";
        public static string ErrorPath { get; set; } = "Error.txt";
        private static readonly object Lock = new object();
        public static void WriteLine(string content, bool inError = false, bool inLog = true, bool inConsole = true)
        {
            string s = $"{DateTime.Now.ToStringLog()}\t{content}";
            if (inConsole)
                Console.WriteLine(s);
            if (inLog)
                File.AppendAllLines(LogPath, s.ToSequence());
            if (inError)
                File.AppendAllLines(ErrorPath, s.ToSequence());
        }
        public static void WriteLineWithLock(string content, bool inError=false,bool inLog=true,bool inConsole = true)
        {
            lock (Lock)
            {
                WriteLine(content, inError, inLog, inConsole);
            }
        }
    }
}
