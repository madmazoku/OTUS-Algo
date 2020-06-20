using System.IO;

namespace lesson._11.cs
{

    class Program
    {

        static void Main(string[] args)
        {
            StreamReader streamIn = new StreamReader("sum.in");
            StreamWriter streamOut = new StreamWriter("sum.out");

            int size, queries;
            {
                string[] tokens = streamIn.ReadLine().Trim().Split();
                (size, queries) = (int.Parse(tokens[0]), int.Parse(tokens[1]));
            }
            {
                RangeTree tree = new RangeTree(size, (x, y) => { return x + y; }, 0);
                for (int lines = 0; lines < queries; ++lines)
                {
                    string[] tokens = streamIn.ReadLine().Trim().Split();
                    switch (tokens[0])
                    {
                        case "A":
                            tree.SetAt(int.Parse(tokens[1]), int.Parse(tokens[2]));
                            break;
                        case "Q":
                            streamOut.WriteLine(tree.GetRange(int.Parse(tokens[1]), int.Parse(tokens[2])));
                            break;
                        default:
                            break;
                    }
                }
            }

            streamOut.Close();
            streamIn.Close();
        }

    }

}