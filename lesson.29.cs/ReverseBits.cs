using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._29.cs
{
    class ReverseBits
    {
        public uint Do(uint n)
        {
            uint r = 0;
            for (int j = 0; j < 32; ++j)
            {
                r = (r << 1) | (n & 0x01);
                n >>= 1;
            }
            return r;
        }

        public uint Do2(uint n)
        {
            uint r = 0;
            byte[] rev = new byte[] { 0b00, 0b10, 0b01, 0b11 };
            for (int j = 0; j < 16; ++j)
            {
                r = (r << 2) | rev[n & 0b11];
                n >>= 2;
            }
            return r;
        }
    }
}
