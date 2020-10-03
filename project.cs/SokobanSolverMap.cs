using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project.cs
{
    class SokobanSolverMap
    {
        const int MAX_WIDTH_HEIGHT = 1 << 8;

        string path;

        public int width;
        public int height;
        public int boxesCount;

        public BitArray stones;
        public BitArray boxes;
        public BitArray targets;
        public ushort[] targetXYs;
        public ushort[] boxXYs;

        public ushort playerXY;

        public SokobanSolverMap(SokobanSolverMap map)
        {
            path = map.path;

            width = map.width;
            height = map.height;
            boxesCount = map.boxesCount;

            stones = new BitArray(map.stones);
            boxes = new BitArray(map.boxes);
            targets = new BitArray(map.targets);

            targetXYs = new ushort[map.boxesCount];
            boxXYs = new ushort[map.boxesCount];

            Array.Copy(map.targetXYs, targetXYs, map.boxesCount);
            Array.Copy(map.boxXYs, boxXYs, map.boxesCount);

            playerXY = map.playerXY;
        }

        public SokobanSolverMap(string path)
        {
            ReadMap(path);
        }

        const string VALID_MAP_CHARS = " .$*@+#";
        bool IsValidMapLine(string line)
        {
            foreach (char c in line)
                if (VALID_MAP_CHARS.IndexOf(c) == -1)
                    return false;
            return line.Length > 0;
        }

        void ReadMap(string path)
        {
            this.path = path;

            string[] lines = File.ReadAllLines(path).Where(x => IsValidMapLine(x)).ToArray();
            width = lines.Select(x => x.Length).Max();
            height = lines.Length;

            if (width > MAX_WIDTH_HEIGHT || height > MAX_WIDTH_HEIGHT)
                throw new Exception("Too big map");

            stones = new BitArray(width * height);
            boxes = new BitArray(width * height);
            targets = new BitArray(width * height);
            List<ushort> targetXYList = new List<ushort>();
            List<ushort> boxXYList = new List<ushort>();

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    int pos = x + y * width;
                    ushort xy = (ushort)(x | (y << 8));
                    switch (lines[y][x])
                    {
                        case ' ': break;
                        case '.': targetXYList.Add(xy); targets.Set(pos, true); break;
                        case '$': boxXYList.Add(xy); boxes.Set(pos, true);  break;
                        case '*': targetXYList.Add(xy); targets.Set(pos, true); boxXYList.Add(xy); boxes.Set(pos, true); break;
                        case '@': playerXY = xy; break;
                        case '+': targetXYList.Add(xy); targets.Set(pos, true); playerXY = xy; break;
                        case '#': stones.Set(pos, true); break;
                        default: break;
                    }
                }

            boxesCount = boxXYList.Count;

            targetXYs = targetXYList.ToArray();
            boxXYs = boxXYList.ToArray();

            Array.Sort<ushort>(targetXYs);
            Array.Sort<ushort>(boxXYs);
        }

    }
}
