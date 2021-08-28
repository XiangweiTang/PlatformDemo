using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Maths
{
    public static class ArgM
    {
        public static T ArgMax<T>(this IEnumerable<T> seq, Func<T, double> evalFunc)
        {
            return seq.Select(x => (x, evalFunc(x)))
                .Aggregate((x, y) => x.Item2 >= y.Item2 ? x : y).x;
        }

        public static T ArgMin<T>(this IEnumerable<T> seq, Func<T, double> evalFunc)
        {
            return seq.Select(x => (x, evalFunc(x)))
                .Aggregate((x, y) => x.Item2 <= y.Item2 ? x : y).x;
        }
    }
}
