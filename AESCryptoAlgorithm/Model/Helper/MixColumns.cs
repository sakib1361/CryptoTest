using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESCryptoAlgorithm.Model.Helper
{
    class MixColumns
    {
        readonly byte[,] matrixE = new byte[4, 4]{
                                {2, 3, 1, 1} ,
                                {1, 2, 3, 1} ,
                                {1, 1, 2, 3} ,
                                {3, 1, 1, 2} };

        internal void AppluColumn(byte[,] state)
        {
            byte[,] result = new byte[4, 4];

            for (int col = 0; col < 4; col++)
            {

                for (int row = 0; row < 4; row++)
                {
                    result[row, col] = (byte)(Multiply(matrixE[row, 0], state[0, col]) ^
                                             Multiply(matrixE[row, 1], state[1, col]) ^
                                             Multiply(matrixE[row, 2], state[2, col]) ^
                                             Multiply(matrixE[row, 3], state[3, col]));
                }
            }
        }

        private byte Multiply(byte row, byte col)
        {
            switch (row)
            {
                case 3:
                    return XPlus1Time(col);
                case 2:
                    return XTime(col);
                case 1:
                    return col;
                default:
                    throw new Exception("Mix COlumn Multiply is getting unknown value");
            }
        }

        public static byte XTime(byte b)
        {
            //Left Shift
            byte top = (byte)((b << 1) & 0xFF);

            //Is Highest Bit 1
            bool highBitIsSet = (0x80 & b) == 0x80;

            //if set, we get the x4+x3+x+1 bit value
            byte bottom = highBitIsSet ? (byte)0x1B : (byte)0;

            byte sum = (byte)(top ^ bottom);
            return sum;
        }

        private static byte XPlus1Time(byte b)
        {
            //get XTime and xor with it
            return (byte)(XTime(b) ^ b);
        }
    }
}
