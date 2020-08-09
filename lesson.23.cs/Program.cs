using System;
using System.Diagnostics;
using System.IO;

namespace lesson._23.cs
{
    class Program
    {
        static void TestRLE()
        {

            ArrayData input = new ArrayData();
            ArrayData encodeOutput = new ArrayData();
            ArrayData decodeOutput = new ArrayData();

            input.WriteByte(0);
            for (int n = 0; n < 4; ++n)
                input.WriteByte(1);
            input.WriteByte(2);
            input.WriteByte(3);
            for (int n = 0; n < 4; ++n)
                input.WriteByte(5);
            input.WriteByte(6);
            input.Print("input");

            ICodec simple = new RLESimpleCodec();
            Console.WriteLine("SimpleRLE");
            input.Rewind();
            simple.Encode(input, encodeOutput);
            encodeOutput.Print("encodeOutput");

            encodeOutput.Rewind();
            simple.Decode(encodeOutput, decodeOutput);
            decodeOutput.Print("decodeOutput");
            Console.WriteLine("");

            input.Rewind();
            encodeOutput.Clear();
            decodeOutput.Clear();

            ICodec complex = new RLEComplexCodec();
            Console.WriteLine("ComplexRLE");
            complex.Encode(input, encodeOutput);
            encodeOutput.Print("encodeOutput");

            encodeOutput.Rewind();
            complex.Decode(encodeOutput, decodeOutput);
            decodeOutput.Print("decodeOutput");
            Console.WriteLine("");

        }

        static void Help()
        {
            Process current = System.Diagnostics.Process.GetCurrentProcess();
            string pathName = current.MainModule.FileName;
            string appName = Path.GetFileName(pathName);
            current.Dispose();

            Console.WriteLine($"Usage: {appName} action source destination type");
            Console.WriteLine("\tWhere:");
            Console.WriteLine("\t\taction: can be 'encode' or 'decode'");
            Console.WriteLine("\t\tsource: point to source file - existed regular file");
            Console.WriteLine("\t\tdestination: point to destination file - absent file");
            Console.WriteLine("\t\ttype: optional argument, can be 'simple' (default) or 'complex'");
        }
        static void Main(string[] args)
        {
            Console.WriteLine("RLE Encoder/Decoder");
            if (args.Length < 3)
            {
                Console.WriteLine("Not enough arguments");
                Help();
                return;
            }
            string action = args[0];
            string source = args[1];
            string destination = args[2];
            string type = args.Length > 3 ? args[3] : "simple";

            Console.WriteLine($"\taction: {action}");
            Console.WriteLine($"\taction: {source}");
            Console.WriteLine($"\taction: {destination}");
            Console.WriteLine($"\taction: {type}");

            IData input = null;
            if (File.Exists(source))
                input = new FileStreamData(new FileStream(source, FileMode.Open, FileAccess.Read));
            else
            {
                Console.WriteLine($"source {source} not exists");
                Help();
                return;
            }

            IData output = null;
            if (!File.Exists(destination))
                output = new FileStreamData(new FileStream(destination, FileMode.Create, FileAccess.Write));
            else
            {
                Console.WriteLine($"destination {destination} exists");
                Help();
                return;
            }

            ICodec codec = null;
            switch (type)
            {
                case "simple":
                    codec = new RLESimpleCodec();
                    break;
                case "complex":
                    codec = new RLEComplexCodec();
                    break;
                default:
                    Console.WriteLine($"type can't be {type}");
                    Help();
                    return;
            }

            switch (action)
            {
                case "encode":
                    codec.Encode(input, output);
                    break;
                case "decode":
                    codec.Decode(input, output);
                    break;
                default:
                    Console.WriteLine($"action can't be {action}");
                    Help();
                    return;
            }

            Console.WriteLine("Finished");

            output = null;
            input = null;

            GC.Collect();
        }
    }
}
