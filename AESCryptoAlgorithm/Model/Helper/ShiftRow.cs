using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESCryptoAlgorithm.Model.Helper
{
    class ShiftRow
    {
        private void ByteShift(byte[,] state)
        {
            byte tmp;
            //First row Now Shift

            //Second Row, 1byte left shift
            tmp = state[1, 0];
            state[1, 0] = state[1, 1];
            state[1, 1] = state[1, 2];
            state[1, 2] = state[1, 3];
            state[1, 3] = tmp;

            //Third row 2 byte left shift
            tmp = state[2, 0];
            state[2, 0] = state[2, 2];
            state[2, 2] = tmp;
            tmp = state[2, 1];
            state[2, 1] = state[2, 3];
            state[2, 3] = tmp;

            //Fourth row 3byte left shift
            tmp = state[3, 0];
            state[3, 0] = state[3, 3];
            state[3, 3] = state[3, 2];
            state[3, 2] = state[3, 1];
            state[3, 1] = tmp;
        }
    }
}
