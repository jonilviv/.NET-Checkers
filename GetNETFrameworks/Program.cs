using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace GetNETFrameworks
{
    public static class VersionTest
    {
        public static void Main()
        {
            Console.WriteLine("Environment.Version = " + Environment.Version);

            PauseCode();
        }

        [Conditional("DEBUG")]
        private static void PauseCode()
        {
            Console.ReadKey();
        }
    }
}