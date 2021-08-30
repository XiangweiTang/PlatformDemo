using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Common.Maths
{
    public static class BPE
    {
        const string END = "<\\w>";
        public static IEnumerable<(string, string)> RawBPE(BPEWord[] vocab, int n, string outputFolder, int step)
        {
            (string, string) prev = ("", "");
            for (int round = 1; round <= n+1; round++)
            {
                bool setFlag = false;
                Dictionary<(string, string), int> freqDict = new Dictionary<(string, string), int>();
                for (int i = 0; i < vocab.Length; i++)
                {
                    var word = vocab[i];
                    if (round == 1)
                    {
                        word.SubWords.Add(END);
                    }
                    for (int j = 0; j < word.SubWords.Count - 1; j++)
                    {
                        var biGram = (word.SubWords[j], word.SubWords[j + 1]);
                        if (biGram == prev)
                        {
                            if (!setFlag)
                            {
                                yield return (word.SubWords[j], word.SubWords[j + 1]);
                                setFlag = true;
                            }
                            word.SubWords[j] = $"{biGram.Item1}{biGram.Item2}";
                            word.SubWords.RemoveAt(j + 1);
                            j--;
                        }
                        else
                        {
                            if (freqDict.ContainsKey(biGram))
                                freqDict[biGram] += word.Frequency;
                            else
                                freqDict[biGram] = word.Frequency;
                        }
                    }
                    vocab[i] = word;

                }
                if (freqDict.Count == 0)
                    break;
                prev = ArgM.ArgMax(freqDict, x => x.Value).Key;
                if (round % step == 0)
                {
                    Logger.WriteLine($"Output round {round}...");
                    string newWPath = Path.Combine(outputFolder, $"{round}.words.txt");
                    File.WriteAllLines(newWPath, vocab.Select(x => string.Join(" ", x.SubWords)));
                    var totalNewWords = vocab.SelectMany(x => x.SubWords).Distinct();
                    string newBpepath = Path.Combine(outputFolder, $"{round}.newWords.txt");
                    File.WriteAllLines(newBpepath, totalNewWords);
                }
            }
        }
        public static BPEWord GenerateBPE((string s,int freq) v)
        {
            return GenerateBPE(v.s, v.freq);
        }
        public static BPEWord GenerateBPE(string s, int freq)
        {
            return new BPEWord
            {
                Word = s,
                SubWords = s.Select(x => $"{x}").ToList(),
                Frequency = freq
            };
        }
        public static void Serialize(this BPEWord[] bpes, string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(BPEWord[]));
            using(StreamWriter sw=new StreamWriter(filePath))
            {
                xs.Serialize(sw, bpes);
            }
        }

        public static BPEWord[] Deserialize(string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(BPEWord[]));
            using(StreamReader sr=new StreamReader(filePath))
            {
                return (BPEWord[])xs.Deserialize(sr);
            }
        }
    }

    public struct BPEWord
    {
        [XmlAttribute]
        public string Word { get; set; }
        [XmlAttribute]
        public List<string> SubWords { get; set; }
        [XmlAttribute]
        public int Frequency { get; set; }
    }
}
