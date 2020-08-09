using System;
using System.Diagnostics;
using System.IO;

namespace lesson._23.cs
{
    class Program
    {
        interface IData
        {
            public int ReadByte();
            public int Read(byte[] array, int offset, int length);

            public void WriteByte(byte value);
            public void Write(byte[] array, int offset, int length);

            public void Flush();
        }

        class ArrayData : IData
        {
            public byte[] Data { get; private set; }
            public int Offset { get; private set; }

            public ArrayData()
            {
                Data = new byte[0];
                Offset = 0;
            }

            public void Print(string title)
            {
                Console.WriteLine($"{title}");
                Console.WriteLine($"\toffset: {Offset}");
                Console.WriteLine($"\tlength: {Data.Length}");
                if (Data.Length == 0)
                    return;
                Console.Write($"\tdata: {Data[0],3}");
                for (int n = 1; n < Data.Length; ++n)
                    Console.Write($"; {Data[n],3}");
                Console.WriteLine("");
            }
            public void Rewind()
            {
                Offset = 0;
            }
            public void Clear()
            {
                Data = new byte[0];
                Offset = 0;
            }
            void Resize(int size)
            {
                if (size <= Data.Length)
                    return;
                byte[] data = new byte[size];
                Array.Copy(Data, data, Data.Length);
                Data = data;
            }

            public int ReadByte()
            {
                if (Offset < Data.Length)
                    return Data[Offset++];
                else
                    return -1;
            }
            public int Read(byte[] array, int offset, int length)
            {
                if (Offset + length > Data.Length)
                    length = Data.Length - Offset;
                Array.Copy(Data, Offset, array, offset, length);
                Offset += length;
                return length;
            }

            public void WriteByte(byte value)
            {
                Resize(Offset + 1);
                Data[Offset++] = value;
            }
            public void Write(byte[] array, int offset, int length)
            {
                Resize(Offset + length);
                Array.Copy(array, offset, Data, Offset, length);
                Offset += length;
            }

            public void Flush()
            {

            }
        }

        class FileStreamData : IData
        {
            FileStream fs;

            public FileStreamData(FileStream fs)
            {
                this.fs = fs;
            }

            public int ReadByte()
            {
                return fs.ReadByte();
            }
            public int Read(byte[] array, int offset, int length)
            {
                return fs.Read(array, offset, length);
            }

            public void WriteByte(byte value)
            {
                fs.WriteByte(value);
            }
            public void Write(byte[] array, int offset, int length)
            {
                fs.Write(array, offset, length);
            }

            public void Flush()
            {
                fs.Flush();
            }
        }

        interface ICodec
        {
            public void Encode(IData input, IData output);
            public void Decode(IData input, IData output);
        }

        class RLESimple : ICodec
        {
            public void Encode(IData input, IData output)
            {
                int count = 1;
                int prev = input.ReadByte();

                while (prev != -1)
                {
                    int next = input.ReadByte();
                    if (next != prev || count == byte.MaxValue)
                    {
                        output.WriteByte((byte)count);
                        output.WriteByte((byte)prev);
                        count = 0;
                        prev = next;
                    }
                    ++count;
                }
                output.Flush();
            }

            public void Decode(IData input, IData output)
            {
                int count;
                while ((count = input.ReadByte()) != -1)
                {
                    int value = input.ReadByte();
                    while (count-- > 0)
                        output.WriteByte((byte)value);
                }
                output.Flush();
            }
        }

        class RLEComplex : ICodec
        {
            public void Encode(IData input, IData output)
            {
                byte[] buffer = new byte[-sbyte.MinValue];
                int uniq = 0;
                int count = 1;
                int prev = input.ReadByte();

                while (prev != -1)
                {
                    int next = input.ReadByte();
                    if (next != prev)
                    {
                        if (count > 1)
                        {
                            output.WriteByte((byte)count);
                            output.WriteByte((byte)prev);
                        }
                        else
                        {
                            buffer[uniq++] = (byte)prev;
                            if (uniq == -sbyte.MinValue || next == -1)
                            {
                                output.WriteByte((byte)(uniq + sbyte.MaxValue));
                                output.Write(buffer, 0, uniq);
                                uniq = 0;
                            }
                        }
                        count = 1;
                        prev = next;
                    }
                    else
                    {
                        if (uniq > 0)
                        {
                            output.WriteByte((byte)(uniq + sbyte.MaxValue));
                            output.Write(buffer, 0, uniq);
                            uniq = 0;
                        }
                        if (count == sbyte.MaxValue)
                        {
                            output.WriteByte((byte)count);
                            output.WriteByte((byte)prev);
                            count = 0;
                        }
                        ++count;
                    }
                }
                output.Flush();
            }

            public void Decode(IData input, IData output)
            {
                byte[] buffer = new byte[-sbyte.MinValue];
                int count;
                while ((count = input.ReadByte()) != -1)
                    if (count < sbyte.MaxValue)
                    {
                        int value = input.ReadByte();
                        while (count-- > 0)
                            output.WriteByte((byte)value);
                    }
                    else
                    {
                        count -= sbyte.MaxValue;
                        input.Read(buffer, 0, count);
                        output.Write(buffer, 0, count);
                    }
                output.Flush();
            }
        }

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

            ICodec simple = new RLESimple();
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

            ICodec complex = new RLEComplex();
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

            if (action != "encode" && action != "decode")
            {
                Console.WriteLine($"action can't be {action}");
                Help();
                return;
            }
            if (!File.Exists(source))
            {
                Console.WriteLine($"source {source} not exists");
                Help();
                return;
            }
            if (File.Exists(destination))
            {
                Console.WriteLine($"destination {destination} exists");
                Help();
                return;
            }
            if (type != "simple" && type != "complex")
            {
                Console.WriteLine($"type can't be {type}");
                Help();
                return;
            }

            IData input = new FileStreamData(new FileStream(source, FileMode.Open, FileAccess.Read));
            IData output = new FileStreamData(new FileStream(destination, FileMode.Create, FileAccess.Write));
            ICodec codec = null;

            switch (type)
            {
                case "simple":
                    codec = new RLESimple();
                    break;
                case "complex":
                    codec = new RLEComplex();
                    break;
            }

            switch (action)
            {
                case "encode":
                    codec.Encode(input, output);
                    break;
                case "decode":
                    codec.Decode(input, output);
                    break;
            }

            Console.WriteLine("Finished");

            output = null;
            input = null;

            GC.Collect();
        }
    }
}
