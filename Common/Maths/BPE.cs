using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Maths
{
    public static class BPE
    {
        public static IEnumerable<(string, string)> RawBPE(BPEWord[] vocab, int n)
        {
            (string, string) prev = ("", "");
            for (int round = 0; round <= n; round++)
            {
                bool setFlag = false;
                Dictionary<(string, string), int> freqDict = new Dictionary<(string, string), int>();
                for (int i = 0; i < vocab.Length; i++)
                {
                    var word = vocab[i];
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
                prev = freqDict.Aggregate((x, y) => x.Value >= y.Value ? x : y).Key;
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
                SubWords = s.Select(x => $"{x}").Append($"{'\0'}").ToList(),
                Frequency = freq
            };
        }
    }

    public struct BPEWord
    {
        public string Word { get; set; }
        public List<string> SubWords { get; set; }
        public int Frequency { get; set; }
    }
}
