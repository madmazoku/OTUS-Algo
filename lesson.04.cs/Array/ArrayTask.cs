using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace lesson._04.cs
{
    class ArrayTask<T> : ITask
        where T : IArray<int>, new()
    {
        string name;
        List<string[]> commands = new List<string[]>();
        List<string> responses = new List<string>();

        public ArrayTask(string name)
        {
            this.name = name;
        }

        public string Name() { return name;  }

        public void Prepare(string[] data)
        {
            commands.Clear();
            foreach(string line in data)
                commands.Add(line.Split(' '));
        }
        public bool Result(string[] expect)
        {
            int index = 0;
            foreach (string response in responses)
            {
                if (response != expect[index])
                    return false;
                ++index;
            }
            return true;
        }

        public void Run()
        {
            responses.Clear();
            T array = new T();
           
            foreach(string[] command in commands)
            {
                string response;
                switch(command[0].ToUpper())
                {
                    case "ADD":
                        {
                            int x = int.Parse(command[1]);
                            for (int i = 0; i < x; ++i)
                                array.Add(i + 1);
                            response = array.Size().ToString();
                        }
                        break;
                    case "INS":
                        {
                            int j = int.Parse(command[1]);
                            int x = int.Parse(command[2]);
                            for (int i = 0; i < x; ++i)
                                array.Add(i + 1, i + j);
                            response = array.Size().ToString();
                        }
                        break;
                    case "UPD":
                        {
                            int j = int.Parse(command[1]);
                            int x = int.Parse(command[2]);
                            for (int i = 0; i < x; ++i)
                                array.Set(i + 1, i + j);
                            response = array.Size().ToString();
                        }
                        break;
                    case "DEL":
                        {
                            int j = int.Parse(command[1]);
                            int x = int.Parse(command[2]);
                            for (int i = 0; i < x; ++i)
                                array.Remove(j);
                            response = array.Size().ToString();
                        }
                        break;
                    case "GET":
                        {
                            int j = int.Parse(command[1]);
                            response = array.Get(j).ToString();
                        }
                        break;
                    case "SUM":
                        {
                            int sum = 0;
                            for (int i = 0; i < array.Size(); ++i)
                                sum += array.Get(i);
                            response = sum.ToString();
                        }
                        break;
                    case "REV":
                        {
                            int sum = 0;
                            for (int i = array.Size() - 1; i >= 0; --i)
                                sum += array.Get(i);
                            response = sum.ToString();
                        }
                        break;
                    default:
                        throw new Exception($"Unknown command {command[0]}");
                }
                responses.Add(response);
            }
        }


    }
}
