using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Maths
{
    public static class FastExp
    {
        public static T Exp<T>(T baseT, int e, Func<T,T,T> multiply)
        {
            Sanity.Requires(e >= 0, "Exponential smaller than 0.");
            if (e == 1)
                return baseT;
            int half = (e >> 1);
            int mono = (e & 1);
            T halfT = Exp(baseT, half, multiply);
            T doubleHalfT = multiply(halfT, halfT);
            if (mono == 0)
                return doubleHalfT;
            return multiply(doubleHalfT, baseT);
        }

        public static double Exp<T>(double baseDouble, int e)
        {
            return Exp(baseDouble, e, (x, y) => x * y);
        }
    }
}
