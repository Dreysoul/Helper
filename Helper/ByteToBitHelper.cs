using System.Collections.Generic;

namespace YiRongMachine
{
    internal class ByteToBitHelper
    {
        /// <summary>
        /// 一个Byte包含8个bit,将其转换为一个8位数的Byte数组
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte[] bytetobit(byte b)
        {
            byte[] array = new byte[8];
            for (int i = 0; i < 7; i++)
            {
                array[i] = (byte)(b & 1);
                b = (byte)(b >> 1);
            }
            return array;
        }

        /// <summary>
        /// Byte数组转换为8倍的Byte数组，然后转换为bool
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool[] manybytetobit(byte[] b)
        {
            bool[] endbool = new bool[(b.Length) * 8];
            List<byte> listbyte = new List<byte>();
            for (int i = 0; i < b.Length; i++)
            {
                byte[] a = bytetobit(b[i]);
                listbyte.AddRange(a);
            }

            for (int i = 0; i < listbyte.Count; i++)
            {
                if (listbyte[i].ToString() == "0")
                {
                    endbool[i] = false;
                }
                else if (listbyte[i].ToString() == "1")
                {
                    endbool[i] = true;
                }
            }
            return endbool;
        }
    }
}