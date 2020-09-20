using System;

namespace lesson._32.cs
{
    class BloomFilter
    {
        static double ln2 = Math.Log(2);

        UInt32 hashCount;
        UInt32 bitCount;

        UInt64[] data;

        public BloomFilter(int capacity, double errorRate)
        {
            hashCount = (UInt32)(Math.Ceiling(Math.Log(1.0 / errorRate)));
            bitCount = (UInt32)(Math.Ceiling(-capacity * Math.Log2(errorRate) / ln2));
            data = new UInt64[1 + (bitCount >> 6)];
        }

        public UInt64 ByteSize() { return (UInt64)sizeof(UInt64) * (UInt64)data.Length; }

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

        (UInt32, UInt64) BitPosMask(UInt32 pos)
        {
            return (pos >> 6, 1ul << (int)(pos & 0x3f));
        }

        void SetBit(UInt32 hash)
        {
            (UInt32 pos, UInt64 mask) = BitPosMask(hash);
            data[pos] |= mask;
        }

        bool CheckBit(UInt32 hash)
        {
            (UInt32 pos, UInt64 mask) = BitPosMask(hash);
            return (data[pos] & mask) == mask;
        }

    }
}
