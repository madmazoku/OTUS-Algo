using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lesson._02.cs
{
    class FibonacciMatrixTask : FibonacciTask
    {
        public override string Name() { return "Матрица"; }

        BigInteger[,] MulMatrix(BigInteger[,] a, BigInteger[,] b)
        {
            BigInteger[,] r = new BigInteger[2, 2];

            r[0, 0] = a[0, 0] * b[0, 0] + a[1, 0] * b[0, 1];
            r[1, 0] = a[0, 0] * b[1, 0] + a[1, 0] * b[1, 1];
            r[0, 1] = a[0, 1] * b[0, 0] + a[1, 1] * b[0, 1];
            r[1, 1] = a[0, 1] * b[1, 0] + a[1, 1] * b[1, 1];

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

        public override BigInteger Fibonacci(BigInteger n)
        {
            if (n < 2)
                return n;

            BigInteger[,] f = PowMatrix(FibonacciMatrix(), n-1);

            return f[0,0];
        }
    }
}
