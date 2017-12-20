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
            Console.WriteLine();
            GetVersionFromRegistry();
            Console.WriteLine();

            PauseCode();
        }

        [Conditional("DEBUG")]
        private static void PauseCode()
        {
            Console.ReadKey();
        }

        private static void GetVersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                if (ndpKey == null)
                {
                    return;
                }

                // As an alternative, if you know the computers you will query are running .NET Framework 4.5 or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (!versionKeyName.StartsWith("v"))
                    {
                        continue;
                    }

                    string vKeyName = versionKeyName.PadRight(12);

                    RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);

                    if (versionKey == null)
                    {
                        continue;
                    }

                    string name = versionKey.GetValue("Version", string.Empty).ToString();

                    if (!string.IsNullOrEmpty(name))
                    {
                        string resultString = vKeyName + " " + name.PadRight(14);
                        string sp = versionKey.GetValue("SP", string.Empty).ToString();

                        if (!string.IsNullOrEmpty(sp))
                        {
                            resultString += " SP" + sp;
                        }
                        else
                        {
                            resultString += "    ";
                        }

                        string installPath = versionKey.GetValue("InstallPath", string.Empty).ToString();

                        resultString += " " + installPath;

                        Console.WriteLine(resultString);

                        continue;
                    }

                    foreach (string subKeyName in versionKey.GetSubKeyNames())
                    {
                        RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                        name = subKey?.GetValue("Version", string.Empty).ToString();

                        if (string.IsNullOrEmpty(name))
                        {
                            continue;
                        }

                        string resultString = versionKeyName + " " + subKeyName;
                        resultString = resultString.PadRight(12);
                        resultString += " " + name.PadRight(14);
                        string sp = subKey.GetValue("SP", string.Empty).ToString();

                        if (!string.IsNullOrEmpty(sp))
                        {
                            resultString += "  SP" + sp;
                        }
                        else
                        {
                            resultString += "    ";
                        }

                        string installPath = subKey.GetValue("InstallPath", string.Empty).ToString();

                        resultString += " " + installPath;

                        Console.WriteLine(resultString);
                    }
                }
            }
        }
    }
}