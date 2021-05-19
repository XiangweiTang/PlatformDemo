using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace PlatformDemo
{
    public static class PlatformHelper
    {
        private const string TMP = "Tmp";
        private static string WorkFolder = null;
        private static string TaskName;
        private static Argument Arg = null;        
        private static Config Cfg = null;
        private static LocalCommon LCommon = null;
        private static Assembly Asmb = null;
        private static DateTime Now;
        private static Dictionary<string, Config> ConfigDict = new Dictionary<string, Config>();
        private static Dictionary<string, Feature> FeatureDict = new Dictionary<string, Feature>();
        static PlatformHelper()
        {
            Asmb = Assembly.Load("PlatformDemo");
            Now = DateTime.Now;
            InitPath();
            InitDict();
        }

        #region Initiate
        private static void InitDict()
        {
            ConfigDict = Asmb.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Config)))
                .ToDictionary(x => x.Name.ToLower(), x => (Config)x.GetConstructor(new Type[0]).Invoke(new object[0]));
            FeatureDict = Asmb.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Feature)))
                .ToDictionary(x => x.Name.ToLower(), x => (Feature)x.GetConstructor(new Type[0]).Invoke(new object[0]));
        }
        private static void InitPath()
        {
            Directory.CreateDirectory(TMP);            
            Logger.ErrorPath = Path.Combine(TMP, $"{Now.ToStringPathMiddle()}_log.txt");
            Logger.ErrorPath = Path.Combine(TMP, $"{Now.ToStringPathMiddle()}_error.txt");
        }
        #endregion

        public static void RunWorkflow(Argument arg)
        {
            Arg = arg;
            if (ValidateArgType(ArgumentType.Xml))
            {
                if (!File.Exists(Arg.XmlConfigFilePath))
                {
                    InitXmlConfig();
                    return;
                }
                SetConfig();
            }
            if (ValidateArgType(ArgumentType.Test))
            {
                RunTest();
            }
            else
            {
                RunFeature();
            }
        }

        #region Workflow
        private static bool ValidateArgType(ArgumentType type)
        {
            return (type & Arg.ArgType) != 0;
        }

        private static void InitXmlConfig()
        {
            WorkFolder = Path.Combine(TMP, $"{Now.ToStringPathMiddle()}_InitXmlConfig");
            Directory.CreateDirectory(WorkFolder);
            string rootXmlPath = Path.Combine(WorkFolder, "Root.xml");
            var list = IO.ReadEmbed("PlatformDemo.Internal.Script.RootXml.xml", "PlatformDemo");
            File.WriteAllLines(rootXmlPath, list);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(rootXmlPath);

            foreach(var item in ConfigDict)
            {
                if (item.Key != "localcommon")                
                    MergeNodes(item.Key, item.Value, xDoc);                
            }
            MergeNodes("localcommon", new LocalCommon(), xDoc);

            xDoc.Save("Config.xml");
        }

        private static void SetConfig()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Arg.XmlConfigFilePath);
            XmlNode root = xDoc["Root"];
            TaskName = root.GetValue("", "TaskName");
            Sanity.Requires(root["Common"] != null, $"Missing Common node.");

            WorkFolder = Path.Combine(TMP, $"{Now.ToStringPathMiddle()}_{TaskName}");
            Directory.CreateDirectory(WorkFolder);
            
            var taskNode = root[TaskName];
            if (taskNode != null)
            {
                string taskConfigPath = Path.Combine(WorkFolder, "Config.xml");
                taskNode.Save(taskConfigPath);
                string cfgKey = $"config{TaskName.ToLower()}";
                Sanity.Requires(ConfigDict.ContainsKey(cfgKey), $"Config for {TaskName} doesn't exist, may because of name mismatch.");
                var type = ConfigDict[cfgKey].GetType();
                Cfg = (Config)Deserialize(taskConfigPath, type);
                if (Arg.PostSetFlag)
                    Cfg.PostSetConfig(Arg);
            }

            var commonNode = root["Common"];
            string commonPath = Path.Combine(WorkFolder, "Common.xml");
            commonNode.Save(commonPath);
            LCommon = (LocalCommon)Deserialize(commonPath, typeof(LocalCommon));
        }

        private static object Deserialize(string xmlPath, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            using(StreamReader sr=new StreamReader(xmlPath))
            {
                return xs.Deserialize(sr);
            }
        }

        private static void Serialize(string xmlPath, Type type, object o)
        {
            XmlSerializer xs = new XmlSerializer(type);
            using(StreamWriter sw=new StreamWriter(xmlPath))
            {
                xs.Serialize(sw, o);
            }
        }

        private static void MergeNodes(string key, Config value, XmlDocument xDoc)
        {
            string featurePath = Path.Combine(WorkFolder, key + ".xml");
            Serialize(featurePath, value.GetType(), value);
            XmlDocument subDoc = new XmlDocument();
            subDoc.Load(featurePath);
            var subNode = subDoc.ChildNodes[1];
            subNode.Attributes.RemoveAll();
            var importNode = xDoc.ImportNode(subNode, true);
            xDoc["Root"].AppendChild(importNode);
        }

        private static void RunTest()
        {
            WorkFolder = Path.Combine(TMP, $"{Now.ToStringPathMiddle()}_Test");
            Directory.CreateDirectory(WorkFolder);
            Logger.LogPath = Path.Combine(WorkFolder, "Log.txt");
            Logger.ErrorPath = Path.Combine(WorkFolder, "Error.txt");
            if (!Arg.SkipConfirmFlag)
            {
                do
                {
                    Console.WriteLine("You're in test mod.");
                    Console.WriteLine("Please enter the following 4 digit/letters to continue.");
                    string key = Guid.NewGuid().ToString().Substring(0, 4);
                    Console.WriteLine(key);
                    string value = Console.ReadLine();
                    if (key == value)
                        break;
                } while (true);
            }
            new Test(Arg, Cfg, LCommon);
        }

        private static void RunFeature()
        {
            Sanity.Requires(FeatureDict.ContainsKey(TaskName.ToLower()), $"Feature {TaskName} doesn't exist.");
            if (!Arg.SkipConfirmFlag)
            {
                Console.WriteLine($"You're going to run the following feature:");
                Console.WriteLine(TaskName);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            var feature = FeatureDict[TaskName.ToLower()];
            feature.WorkFolder = WorkFolder;
            feature.LoadAndRun(Cfg, LCommon);
        }
        #endregion
    }
}
