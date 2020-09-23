using System;

namespace lesson._32.cs
{
    class HyperLogLog
    {
        int log2m;
        int regWidth;
        int[] baskets;

        public HyperLogLog(int log2m, int regWidth)
        {
            this.log2m = log2m;
            this.regWidth = regWidth;
            baskets = new int[1 << log2m];
        }

        double alpha()
        {
            if (log2m < staticAlpha.Length)
                return staticAlpha[log2m];
            return 0.7213 / (1 + (1.079 / (1 << log2m)));
        }

        public int Count
        {
            get
            {
                double Z = 0;
                for (int basket = 0; basket < baskets.Length; ++basket)
                    if (baskets[basket] > 0)
                        Z += 1.0 / (1 << baskets[basket]);
                Z = 1.0 / Z;
                double E = alpha() * (1 << (log2m << 1)) * Z;
                return (int)Math.Floor(E);
            }
        }

        public void Add(string key)
        {
            AddHash((UInt64)key.GetHashCode());
        }

        public void AddHash(UInt64 hash)
        {
            UInt64 basket = hash & staticMasks[log2m - 1];
            hash = (hash >> log2m) & staticMasks[regWidth - 1];
            int count = 0;
            while (++count < regWidth && (hash & 0x1) == 0)
                hash >>= 1;
            if (baskets[basket] < count)
                baskets[basket] = count;
        }

        static UInt64[] staticMasks = GenerateUInt64Masks();
        static UInt64[] GenerateUInt64Masks()
        {
            UInt64[] masks = new UInt64[64];
            masks[0] = 1;
            for (int i = 1; i < 64; ++i)
                masks[i] = (masks[i - 1] << 1) | 1;
            return masks;
        }

        static double[] staticAlpha = new double[] { 0, 0, 0.5324, 0.6027, 0.673, 0.697, 0.709 };
    }
}
