using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DESLib
{
    public static class ExtensionMethods
    {
        public static BitArray SubArray(this BitArray input, int index, int length)
        {
            var output = new BitArray(length);
            for (int i = index; i < index + length; i++)
                output[i - index] = input[i];

            return output;
        }

        public static BitArray Concat(this BitArray input1, BitArray input2)
        {
            int length = input1.Length + input2.Length;
            var output = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                if (i < input1.Length)
                    output[i] = input1[i];
                else output[i] = input2[i - input1.Length];
            }

            return output;
        }

        public static BitArray ShiftLeft(this BitArray input, int shiftcount)
        {
            var clone = new BitArray(input);
            var output = new BitArray(input);

            for (int k = 0; k < shiftcount; k++)
            {
                for (int i = clone.Count - 1; i >= 0; i--)
                {
                    int j = i - 1;
                    if (j >= 0)
                        output[j] = clone[i];
                    else output[output.Count - 1] = clone[i];
                }
                clone = new BitArray(output);
            }

            return output;
        }

        public static BitArray PadRight(this BitArray input, int totallength, bool padvalue)
        {
            var output = new BitArray(totallength);
            for (int i = 0; i < output.Count; i++)
            {
                if (i < input.Count)
                    output[i] = input[i];
                else output[i] = padvalue;
            }   

            return output;
        }

        public static void Insert(this BitArray input, int index, BitArray block)
        {
            for (int i = index; i < index + block.Length; i++)
                input[i] = block[i - index];
        }

        public static BitArray Reverse(this BitArray input)
        {
            var output = new BitArray(input.Length);
            for (int i = input.Length - 1; i >= 0; i--)
                output[input.Length - 1 - i] = input[i];

            return output;
        }

        public static byte[] ToByteArray(this BitArray input)
        {
            var reverse = input.Reverse();
            byte[] output = new byte[(int)Math.Ceiling((double)reverse.Length / 8)];
            reverse.CopyTo(output, 0);

            return output;
        }

        public static BitArray ToBitArray(this List<byte> input)
        {
            return new BitArray(input.ToArray());
        }

        public static BitArray FromBitStringToBitArray(this string input)
        {
            var output = new BitArray(input.Length);
            for (int i = 0; i < input.Length; i++)
                output[i] = input[i] == '1';

            return output;
        }

        public static BitArray FromHexStringToBitArray(this string input)
        {
            var builder = new StringBuilder();
            foreach (var c in input)
                builder.Append(c.ToBitString());

            return builder.ToString().FromBitStringToBitArray();
        }

        public static string ToBitString(this BitArray input)
        {
            var output = new StringBuilder(input.Count);

            foreach (var bit in input)
                output.Append((bool)bit ? "1" : "0");

            return output.ToString();
        }

        public static string ToHexString(this BitArray input)
        {
            var output = new StringBuilder();
            for (int i = 0; i < input.Count; i+= 4)
            {
                BitArray nibble = new BitArray(
                    new bool[]
                    {
                        input[i],
                        input[i + 1],
                        input[i + 2],
                        input[i + 3]
                    });

                byte[] b = new byte[1];
                nibble.Reverse().CopyTo(b, 0);
                output.Append(b.ToHexChar());
            }

            return output.ToString();
        }

        public static char ToHexChar(this byte[] input)
        {
            if (input[0] < 10)
                return input[0].ToString()[0];
            else if (input[0] == 10)
                return 'A';
            else if (input[0] == 11)
                return 'B';
            else if (input[0] == 12)
                return 'C';
            else if (input[0] == 13)
                return 'D';
            else if (input[0] == 14)
                return 'E';
            else return 'F';
        }

        public static string ToBitString(this string hex)
        {
            var output = new StringBuilder();
            foreach (char c in hex)
                output.Append(c.ToBitString());

            return output.ToString();
        }

        public static string ToBitString(this char input)
        {
            switch (input.ToString().ToUpper())
            {
                default:
                case "0": return "0000";
                case "1": return "0001";
                case "2": return "0010";
                case "3": return "0011";
                case "4": return "0100";
                case "5": return "0101";
                case "6": return "0110";
                case "7": return "0111";
                case "8": return "1000";
                case "9": return "1001";
                case "A": return "1010";
                case "B": return "1011";
                case "C": return "1100";
                case "D": return "1101";
                case "E": return "1110";
                case "F": return "1111";
            }
        }

        public static int ToInt32(this BitArray input)
        {
            if (input.Length > 32)
                throw new ArgumentException();

            var bytes = input.ToByteArray().ToList();
            while (bytes.Count < 4)
                bytes.Add(0);

            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }

        public static BitArray ApplyPermutation(this BitArray input, List<int> permutations)
        {
            var output = new BitArray(permutations.Count);

            for (int i = 0; i < permutations.Count; i++)
            {
                output[i] = input[permutations[i] - 1]; // Permutation lists are 1 based
            }

            return output;
        }        
    }
}
