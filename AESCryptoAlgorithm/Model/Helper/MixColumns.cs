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
        readonly byte[,] InvMatrixE = new byte[4, 4]{
                                {0x0E ,0x0B ,0x0D ,0x09},
                                {0x09 ,0x0E ,0x0B ,0x0D},
                                {0x0D ,0x09 ,0x0E ,0x0B},
                                {0x0B ,0x0D ,0x09 ,0x0E}};

        internal void ApplyColumn(byte[,] state)
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

        internal void InvApplyColumn(byte[,] state)
        {
            byte[,] result = new byte[4, 4];

            for (int col = 0; col < 4; col++)
            {

                for (int row = 0; row < 4; row++)
                {
                    result[row, col] = (byte)(Multiply(InvMatrixE[row, 0], state[0, col]) ^
                                             Multiply(InvMatrixE[row, 1], state[1, col]) ^
                                             Multiply(InvMatrixE[row, 2], state[2, col]) ^
                                             Multiply(InvMatrixE[row, 3], state[3, col]));
                }
            }
        }

        private byte Multiply(byte row, byte col)
        {
            return FiniteFieldMath.Multiply(row, col);
        }
    }
}
