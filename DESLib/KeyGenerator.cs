using System.Collections;
using System.Collections.Generic;

namespace DESLib
{
    internal static class KeyGenerator
    {
        #region Private Variables
        private static readonly List<int> PC_1 = new List<int> {
            57, 49, 41, 33, 25, 17, 9,
             1, 58, 50, 42, 34, 26, 18,
            10,  2, 59, 51, 43, 35, 27,
            19, 11,  3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
             7, 62, 54, 46, 38, 30, 22,
            14,  6, 61, 53, 45, 37, 29,
            21, 13,  5, 28, 20, 12,  4 };
        private static readonly List<int> PC_2 = new List<int> {
            14, 17, 11, 24,  1, 5,
             3, 28, 15,  6, 21, 10,
            23, 19, 12,  4, 26,  8,
            16,  7, 27, 20, 13,  2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32 };
        private static readonly List<int> ShiftCounts = new List<int> {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};
        #endregion

        #region Public
        public static List<BitArray> GetKeyPermutations(BitArray key)
        {
            var pc1 = key.ApplyPermutation(PC_1);

            int mid = pc1.Count / 2;
            var left = pc1.SubArray(0, mid);
            var right = pc1.SubArray(mid, mid);

            var keys = new List<BitArray>();
            for (int i = 0; i < 16; i++)
            {
                left = left.ShiftLeft(ShiftCounts[i]);
                right = right.ShiftLeft(ShiftCounts[i]);

                var concat = left.Concat(right);
                var pc2 = concat.ApplyPermutation(PC_2);

                keys.Add(pc2);
            }

            return keys;
        }
        #endregion
    }
}
