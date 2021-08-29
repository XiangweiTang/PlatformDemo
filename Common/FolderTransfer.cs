using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Common
{
    public abstract class FolderTransfer
    {
        private bool RunParallelly = false;
        public FolderTransfer() { }
        public void Run(string inputFolderPath, string outputFolderPath, bool runParallelly)
        {
            RunParallelly = runParallelly;
            PreProcess();
            Transfer(inputFolderPath, outputFolderPath);
            PostProcess();
        }
        private void Transfer(string inputFolderPath, string outputFolderPath)
        {
            var files = EnumerateFiles(inputFolderPath);
            if (RunParallelly)
            {
                Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = 10 }, inputFilePath =>
                 {
                     AtomicTransfer(inputFilePath, outputFolderPath);
                 });
            }
            else
            {
                foreach (string inputFilePath in files)
                    AtomicTransfer(inputFilePath, outputFolderPath);
            }
            foreach(string inputSubFolderPath in Directory.EnumerateDirectories(inputFolderPath))
            {
                string folderName = inputSubFolderPath.Split('\\').Last();
                string outputSubFolderPath = Path.Combine(outputFolderPath, folderName);
                Directory.CreateDirectory(outputSubFolderPath);
                Transfer(inputSubFolderPath, outputSubFolderPath);
            }
        }
        private void AtomicTransfer(string inputFilePath, string outputFolderPath)
        {
            string fileName = inputFilePath.Split('\\').Last();
            string newFileName = RenameFile(fileName);
            string outputFilePath = Path.Combine(outputFolderPath, newFileName);
            ItemTransfer(inputFilePath, outputFilePath);
        }
        protected virtual void PreProcess() { }
        protected virtual IEnumerable<string> EnumerateDirectories(string folderPath)
        {
            return Directory.EnumerateDirectories(folderPath);
        }
        protected virtual IEnumerable<string> EnumerateFiles(string folderPath)
        {
            return Directory.EnumerateFiles(folderPath);
        }
        protected virtual string RenameFile(string fileName)
        {
            return fileName;
        }
        protected abstract void ItemTransfer(string inputFilePath, string outputFilePath);
        protected virtual void PostProcess() { }
    }
}
