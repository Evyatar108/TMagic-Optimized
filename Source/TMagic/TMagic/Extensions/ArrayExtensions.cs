namespace TorannMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    static class ArrayExtensions
    {
        public static bool Contains<T>(this T[] array, T value)
        {
            for (int i =0;i<array.Length;i++)
            {
                if (array[i].Equals(value))
                    return true;
            }

            return false;
        }
    }
}
