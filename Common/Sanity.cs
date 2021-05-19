using System;

namespace Common
{
    public static class Sanity
    {
        public static void Requires(bool valid, string message, int hResult=-1)
        {
            if (!valid)
                throw new CommonException(message, hResult);
        }
        public static void Requires(bool valid, int hResult=-1)
        {
            if (!valid)
                throw new CommonException(hResult);
        }
    }

    public class CommonException : Exception
    {
        public CommonException(int hResult = -1) : base()
        {
            HResult = hResult;
        }
        public CommonException(string message, int hResult = -1) : base(message)
        {
            HResult = hResult;
        }
    }
}
