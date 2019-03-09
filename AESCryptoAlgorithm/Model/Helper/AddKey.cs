using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESCryptoAlgorithm.Model.Helper
{
    class AddKey : IDisposable
    {
        private readonly Substitution Substitution;
        private readonly int KeySize;
        private const int Nr = 10;  //number of rounds (general)
        private readonly byte[,] EK = new byte[44, 4];
        private readonly byte[,] K = new byte[4, 4];
        readonly byte[] Rcon = new byte[10] {
              0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36}; //for 128-bit only

        internal AddKey(Substitution substitution, string password)
        {
            Substitution = substitution;
            KeySize = password.Length;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    K[j, i] = (byte)(password[i * 4 + j]);

            ExpandKey();
        }
        internal void AddRoundKey(int rnd, byte[,] state)
        {
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    state[j, i] ^= EK[rnd * 4 + i, j];
        }
        private void RotWord(byte[] arr)
        {
            byte Temp = arr[0];
            arr[0] = arr[1];
            arr[1] = arr[2];
            arr[2] = arr[3];
            arr[3] = Temp;
        }
        private void ExpandKey()
        {
            int i = 0;
            byte[] tmp = new byte[4];
            while (i < 4 * (Nr + 1))
            {
                if (i == 0)
                { //the first block copies the unexpanded key
                    for (i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            EK[i, j] = K[i, j];
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                        tmp[j] = EK[i - 1, j];
                    RotWord(tmp);
                    Substitution.SubWord(tmp);
                    for (int j = 0; j < 4; j++)
                    {
                        //executed in every 4 bytes
                        EK[i, j] = (byte)(tmp[j] ^ EK[i - 4, j]);
                        if (j == 0) //only in every first four bytes in each block
                            EK[i, j] ^= Rcon[(4 * i / KeySize) - 1];
                    }
                    i++;
                    while (i % 4 != 0) //after first four bytes in each block

                    {
                        for (int j = 0; j < 4; j++)
                            EK[i, j] = (byte)(EK[i - 1, j] ^ EK[i - 4, j]);
                        i++;
                    }
                }
            }
        }

        public void Dispose()
        {
            Array.Clear(EK, 0, EK.Length);
        }
    }
}
