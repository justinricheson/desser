using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DESLib
{
    public static class DES
    {
        #region Private Variables
        private static readonly List<int> IP = new List<int>
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17,  9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };
        private static readonly List<int> IP_INVERSE = new List<int>
        {
            40,  8, 48, 16, 56, 24, 64, 32,
            39,  7, 47, 15, 55, 23, 63, 31,
            38,  6, 46, 14, 54, 22, 62, 30,
            37,  5, 45, 13, 53, 21, 61, 29,
            36,  4, 44, 12, 52, 20, 60, 28,
            35,  3, 43, 11, 51, 19, 59, 27,
            34,  2, 42, 10, 50, 18, 58, 26,
            33,  1, 41,  9, 49, 17, 57, 25
        };
        private static readonly List<int> E_SELECT = new List<int>
        {
            32,  1,  2,  3,  4,  5,
             4,  5,  6,  7,  8,  9,
             8,  9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32,  1,
        };
        private static readonly List<int> P = new List<int>
        {
            16,  7, 20, 21,
            29, 12, 28, 17,
             1, 15, 23, 26,
             5, 18, 31, 10,
             2,  8, 24, 14,
            32, 27,  3, 9,
            19, 13, 30,  6,
            22, 11,  4, 25
        };
        #endregion

        #region Public
        public static BitArray Encrypt(BitArray plaintext, BitArray key)
        {
            if (plaintext == null || key == null)
                throw new ArgumentNullException();
            if (key.Length != 64)
                throw new ArgumentException();

            var keys = KeyGenerator.GetKeyPermutations(key);

            int mod = plaintext.Length % 64;
            var pad = mod == 0 ? 0 : 64 - mod;
            var plaintextbits = plaintext.PadRight(plaintext.Length + pad, false);

            return EncryptDecrypt(plaintextbits, keys);
        }
        public static BitArray Decrypt(BitArray ciphertext, BitArray key)
        {
            if (ciphertext == null || key == null)
                throw new ArgumentNullException();
            if (key.Length != 64)
                throw new ArgumentException();
            if (ciphertext.Length % 64 != 0)
                throw new ArgumentException();

            var keys = KeyGenerator.GetKeyPermutations(key);
            keys.Reverse();

            return EncryptDecrypt(ciphertext, keys);
        }
        #endregion

        #region Private
        private static BitArray EncryptDecrypt(BitArray input, List<BitArray> keys)
        {
            var output = new BitArray(input.Count);
            for (int i = 0; i < input.Count; i += 64)
            {
                var block = input.SubArray(i, 64);
                var cipher = EncryptDecryptBlock(block, keys);
                output.Insert(i, cipher);
            }

            return output;
        }
        private static BitArray EncryptDecryptBlock(BitArray block, List<BitArray> keys)
        {
            var ip = block.ApplyPermutation(IP);
            int mid = ip.Count / 2;
            var left = ip.SubArray(0, mid);
            var right = ip.SubArray(mid, mid);

            for (int i = 0; i < 16; i++)
            {
                var temp = left;
                left = right;
                right = temp.Xor(ApplyKey(right, keys[i]));
            }

            var concat = right.Concat(left);
            return concat.ApplyPermutation(IP_INVERSE);
        }
        private static BitArray ApplyKey(BitArray input, BitArray key)
        {
            var e = input.ApplyPermutation(E_SELECT);
            var xor = e.Xor(key);

            var output = new BitArray(32);
            for (int i = 0; i < 8; i++)
            {
                var bits = xor.SubArray(i * 6, 6);
                output.Insert(i * 4, SBoxEnumerator.Enumerate(bits, i));
            }

            return output.ApplyPermutation(P);
        }
        #endregion
    }
}
