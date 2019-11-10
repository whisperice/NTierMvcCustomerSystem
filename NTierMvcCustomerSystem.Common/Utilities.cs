using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.Common
{
    public static class Utilities
    {
        /// <summary>
        /// Check whether all elements in two Ilist are equal (element order sensitive)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Return true only when two list are null, or all elements are equal </returns>
        public static bool EqualsAll<T>(IList<T> a, IList<T> b)
        {
            if (a == null || b == null)
                return (a == null && b == null);

            if (a.Count != b.Count)
                return false;

            return a.SequenceEqual(b);
        }

        /// <summary>
        /// Get Hashcode according to all elements in a Ilist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int GetHashCode<T>(IList<T> a)
        {
            int res = 0x2D2816FE;
            unchecked
            {
                foreach (var item in a)
                {
                    res = res * 31 + (item == null ? 0 : item.GetHashCode());
                }

                return res;
            }
        }
    }
}
