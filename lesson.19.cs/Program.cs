using System;
using System.Windows.Forms;

namespace lesson._19.cs
{
    class Program
    {
        static void ShowWindow()
        {

            Application.EnableVisualStyles();
            Application.Run(new TravelFrm());
        }

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Задача коммивояжёра. A*");

            ShowWindow();

            Console.WriteLine("Goodbye");
        }
    }
}
