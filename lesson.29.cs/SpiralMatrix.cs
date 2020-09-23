using System.Collections.Generic;

// https://leetcode.com/problems/spiral-matrix/

namespace lesson._29.cs
{
    class SpiralMatrix
    {
        public IList<int> Do(int[][] matrix)
        {
            List<int> list = new List<int>();

            int R = matrix.Length;
            if (R == 0) return list;
            int C = matrix[0].Length;

            int r = 0;
            int c = 0;

            int[] deltaRow = { 0, 1, 0, -1 };
            int[] deltaCol = { 1, 0, -1, 0 };

            int di = 0;
            bool[,] seen = new bool[R, C];

            for (int i = R * C - 1; i >= 0; --i)
            {
                list.Add(matrix[r][c]);
                seen[r, c] = true;

                int r1 = r + deltaRow[di];
                int c1 = c + deltaCol[di];

                if (0 <= r1 && r1 < R && 0 <= c1 && c1 < C && !seen[r1, c1])
                {
                    r = r1;
                    c = c1;
                }
                else
                {
                    di = (di + 1) & 0b11;
                    r = r + deltaRow[di];
                    c = c + deltaCol[di];
                }
            }
            return list;
        }

    }
}
