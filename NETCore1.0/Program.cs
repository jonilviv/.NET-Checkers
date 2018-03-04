using System;
using System.Reflection;

namespace NETCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string product = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            Console.Write(product.PadRight(25));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("OK");
            Console.ResetColor();

            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "--console":
                    case "-c":
                    case "/c":
                        return;
                }
            }

            Console.ReadKey();
        }
    }
}