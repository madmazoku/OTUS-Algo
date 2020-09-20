using System;

namespace lesson._32.cs
{
    class BloomFilter
    {
        static double ln2 = Math.Log(2);
        static byte[] masks = new byte[] {
            0b00000001,
            0b00000010,
            0b00000100,
            0b00001000,
            0b00010000,
            0b00100000,
            0b01000000,
            0b10000000,
        };

        UInt32 hashCount;
        UInt32 bitCount;

        byte[] data;

        public BloomFilter(int capacity, double errorRate)
        {
            hashCount = (UInt32)(Math.Ceiling(Math.Log(1.0 / errorRate)));
            bitCount = (UInt32)(Math.Ceiling(-capacity * Math.Log2(errorRate) / ln2));
            data = new byte[1 + (bitCount >> 3)];
        }

        public int ByteSize() { return data.Length; }

        public void Add(string key)
        {
            (UInt32 hash1, UInt32 hash2) = DoubleHash(key);
            for (UInt32 k = 0; k < hashCount; ++k)
                SetBit(CalcSubHash(hash1, hash2, k));
        }

        public bool Exists(string key)
        {
            bool exists = true;
            (UInt32 hash1, UInt32 hash2) = DoubleHash(key);
            for (UInt32 k = 0; exists && k < hashCount; ++k)
                exists = exists && CheckBit(CalcSubHash(hash1, hash2, k));
            return exists;
        }

        (UInt32, UInt32) DoubleHash(string key)
        {
            int halfLength = key.Length >> 1;
            UInt32 hash1 = (UInt32)key.Substring(0, halfLength).GetHashCode();
            UInt32 hash2 = (UInt32)key.Substring(halfLength, key.Length - halfLength).GetHashCode();
            return (hash1, hash2);
        }

        UInt32 CalcSubHash(UInt32 hash1, UInt32 hash2, UInt32 k)
        {
            return (UInt32)(((UInt64)0 + hash1 + k * hash2) % bitCount);
        }

        (UInt32, byte) BitPosMask(UInt32 pos)
        {
            return (pos >> 3, masks[pos & 0x7]);
        }

        void SetBit(UInt32 hash)
        {
            (UInt32 pos, byte mask) = BitPosMask(hash);
            data[pos] |= mask;
        }

        bool CheckBit(UInt32 hash)
        {
            (UInt32 pos, byte mask) = BitPosMask(hash);
            return (data[pos] & mask) == mask;
        }

    }
}
