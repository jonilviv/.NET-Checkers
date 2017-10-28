using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace NETCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string product = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            Console.Write(product + "   ");
            var consoleForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("OK");
            Console.ForegroundColor = consoleForegroundColor;

            //string targetFramework = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            //Console.WriteLine(targetFramework);

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