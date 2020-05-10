using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lesson._02.cs
{
    class FibonacciMatrixParallelTask : FibonacciTask
    {
        public override string Name() { return "Параллельная Матрица"; }

        BigInteger[,] MulMatrix(BigInteger[,] a, BigInteger[,] b)
        {
            BigInteger[,] r = new BigInteger[2, 2];

            Task<BigInteger> at_a00_b00 = Task.Run(() => { return a[0, 0] * b[0, 0]; });
            Task<BigInteger> at_a10_b01 = Task.Run(() => { return a[1, 0] * b[0, 1]; });

            Task<BigInteger> at_a00_b10 = Task.Run(() => { return a[0, 0] * b[1, 0]; });
            Task<BigInteger> at_a10_b11 = Task.Run(() => { return a[1, 0] * b[1, 1]; });

            Task<BigInteger> at_a01_b00 = Task.Run(() => { return a[0, 1] * b[0, 0]; });
            Task<BigInteger> at_a11_b01 = Task.Run(() => { return a[1, 1] * b[0, 1]; });

            Task<BigInteger> at_a01_b10 = Task.Run(() => { return a[0, 1] * b[1, 0]; });
            Task<BigInteger> at_a11_b11 = Task.Run(() => { return a[1, 1] * b[1, 1]; });

            at_a00_b00.Wait();
            at_a10_b01.Wait();

            at_a00_b10.Wait();
            at_a10_b11.Wait();

            at_a01_b00.Wait();
            at_a11_b01.Wait();

            at_a01_b10.Wait();
            at_a11_b11.Wait();

            Task<BigInteger> at_r00 = Task.Run(() => { return at_a00_b00.Result + at_a10_b01.Result; });
            Task<BigInteger> at_r10 = Task.Run(() => { return at_a00_b10.Result + at_a10_b11.Result; });
            Task<BigInteger> at_r01 = Task.Run(() => { return at_a01_b00.Result + at_a11_b01.Result; });
            Task<BigInteger> at_r11 = Task.Run(() => { return at_a01_b10.Result + at_a11_b11.Result; });

            at_r00.Wait();
            at_r10.Wait();
            at_r01.Wait();
            at_r11.Wait();

            r[0, 0] = at_r00.Result;
            r[1, 0] = at_r10.Result;
            r[0, 1] = at_r01.Result;
            r[1, 1] = at_r11.Result;

            return r;
        }

        BigInteger[,] FibonacciMatrix()
        {
            BigInteger[,] f = new BigInteger[2, 2];
            f[0, 0] = 1; f[0, 1] = 1;
            f[1, 0] = 1; f[1, 1] = 0;
            return f;
        }

        BigInteger[,] IdentityMatrix()
        {
            BigInteger[,] i = new BigInteger[2, 2];
            i[0, 0] = 1; i[1, 0] = 0;
            i[0, 1] = 0; i[1, 1] = 1;
            return i;
        }

        BigInteger[,] PowMatrix(BigInteger[,] x, BigInteger y)
        {
            BigInteger[,] r = IdentityMatrix();

            while (y > 1)
            {
                if ((y & 0x1) == 1)
                    r = MulMatrix(r, x);
                x = MulMatrix(x, x);
                y >>= 1;
            }
            if (y > 0)
                r = MulMatrix(r, x);

            return r;
        }

        BigInteger PowBI(BigInteger x, BigInteger y)
        {
            BigInteger r = 1;
            while (y > 1)
            {
                if ((y & 0x1) == 1)
                    r *= x;
                x *= x;
                y >>= 1;
            }
            if (y > 0)
                r *= x;
            return r;
        }

        public override BigInteger Fibonacci(BigInteger n)
        {
            if (n < 2)
                return n;

            BigInteger[,] f = PowMatrix(FibonacciMatrix(), n-1);

            return f[0,0];
        }
    }
}
