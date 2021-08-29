using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Maths;
using Common;
using System.IO;

namespace PlatformDemo
{
    public class Test
    {
        public Argument Arg { get; set; } = null;
        public Config Cfg { get; set; } = null;
        public LocalCommon LCommon { get; set; } = null;

        public void Run()
        {
            string bpeFolderPath= @"D:\Files\Data\Opus\OpenSubtitles\bpe";
            string inputPath = @"D:\Files\Data\Opus\OpenSubtitles\xml";
            string outputPath = @"D:\Files\Data\Opus\OpenSubtitles\txt";
            string bpeStoragePath = @"D:\Files\Data\Opus\OpenSubtitles\bpe\0.xml";
            string vocabPath = @"D:\Files\Data\Opus\OpenSubtitles\bpe\vocab.txt";
            //new ExtractOpenSubtitle().Run(inputPath, outputPath, true);

            //var sentences = OpenSubtitle.LoadMono(@"D:\Files\Data\Opus\OpenSubtitles\xml\en\2000\16041_192375_255341_sylvia.xml").Sentences;
            //var sequence = sentences.SelectMany(x => x.Words.Select(x => x.Value.ToLower())).ToArray();
            //var seq = WordFrequncies(outputPath);
            //var input = seq.Select(x => BPE.GenerateBPE(x)).ToArray();
            //BPE.Serialize(input, bpeStoragePath);            
            var array = BPE.Deserialize(bpeStoragePath);
            var vocabs = BPE.RawBPE(array, 1000, bpeFolderPath, 100)
                .Select(x => $"{x.Item1}{x.Item2}");
            File.WriteAllLines(vocabPath, vocabs);
        }

        private IEnumerable<(string,int)> WordFrequncies(string folderPath)
        {
            return Directory.EnumerateFiles(folderPath, "*.txt", SearchOption.AllDirectories)
                .SelectMany(x => File.ReadLines(x))
                .GroupBy(x => x)
                .Select(x => (x.Key, x.Count()));
        }

        class ExtractOpenSubtitle : FolderTransfer
        {
            protected override void ItemTransfer(string inputFilePath, string outputFilePath)
            {
                var sequence = OpenSubtitle.LoadMono(inputFilePath)
                    .Sentences
                    .Where(x=>x.Words!=null)
                    .SelectMany(x => x.Words.Select(x => x.Value.ToLower()));
                var filtered = Cleanup(sequence);
                File.WriteAllLines(outputFilePath, filtered);
            }
            private IEnumerable<string> Cleanup(IEnumerable<string> seq)
            {
                CleanerMono[] Cleaners =
                {
                new CleanerSpaceMono()
            };
                ClassifierMono[] Classifiers =
                {
                new ClassifierAlphabetOnlyMono(),
                new ClassifierEmptyMono(),
            };
                foreach (string s in seq)
                {
                    string newS = s;
                    foreach (CleanerMono cleaner in Cleaners)
                        newS = cleaner.Clean(newS);
                    bool kept = true;
                    foreach (ClassifierMono classifier in Classifiers)
                    {
                        if (classifier.Remove(newS))
                        {
                            kept = false;
                        }
                    }
                    if (kept)
                        yield return newS;
                }
            }
            protected override IEnumerable<string> EnumerateFiles(string folderPath)
            {
                return Directory.EnumerateFiles(folderPath, "*.xml");
            }
            protected override string RenameFile(string fileName)
            {
                return fileName.Substring(0, fileName.Length - 3) + ".txt";
            }
        }
    }
}
