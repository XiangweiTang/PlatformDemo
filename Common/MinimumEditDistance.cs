using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MinimumEditDistance<T> where T : IComparable
    {
        T[] StandardArray = null;
        int SCount = -1;
        T[] CompareArray = null;
        int CCount = -1;
        int[,] InternalMatrix = new int[0, 0];
        public int DEL { get; private set; } = 0;
        public int INS { get; private set; } = 0;
        public int SUB { get; private set; } = 0;        
        public MinimumEditDistance(T[] standardArray, T[] compareArray)
        {
            StandardArray = standardArray;
            CompareArray = compareArray;
            Init();
        }

        private void Init()
        {
            SCount = StandardArray.Length;
            CCount = CompareArray.Length;
        }

        public int RunWithBacktrack()
        {
            InternalMatrix = new int[CCount + 1, SCount + 1];
            for (int i = 1; i <= SCount; i++)
                InternalMatrix[0, i] = i;
            for (int i = 1; i <= CCount; i++)
                InternalMatrix[i, 0] = i;

            for(int i = 1; i <= CCount; i++)
            {
                for(int j = 1; j <= SCount; j++)
                {
                    int diag = InternalMatrix[i - 1, j - 1];
                    if (!CompareArray[i - 1].Equals(StandardArray[j - 1]))
                        diag++;
                    int left = InternalMatrix[i, j - 1] + 1;
                    int top = InternalMatrix[i - 1, j] + 1;
                    int min = Math.Min(diag, Math.Min(left, top));
                    InternalMatrix[i, j] = min;
                }
            }
            return InternalMatrix[CCount, SCount];
        }

        public void BackTrace()
        {
            int i = CCount;
            int j = SCount;
            while (i >= 0 && j >= 0)
            {
                if (i == 0 && j == 0)
                {
                    break;
                }
                if (i == 0)
                {
                    j--;
                    DEL++;
                    continue;
                }
                if (j == 0)
                {
                    i--;
                    INS++;
                    continue;
                }
                int diag = InternalMatrix[i - 1, j - 1];                
                int left = InternalMatrix[i, j - 1];
                int top = InternalMatrix[i - 1, j];
                int current = InternalMatrix[i, j];
                if(current==diag&& StandardArray[j-1].Equals(CompareArray[i-1]))
                {
                    i--;
                    j--;
                    continue;
                }
                if(current==diag+1&&!StandardArray[j - 1].Equals(CompareArray[i - 1]))
                {
                    i--;
                    j--;
                    SUB++;
                    continue;
                }
                if (current == top + 1)
                {
                    i--;
                    INS++;
                    continue;
                }
                if (current == left + 1)
                {
                    j--;
                    DEL++;
                    continue;
                }
                throw new CommonException("Mismatch in internal matrix.");
            }
        }
    }
}
