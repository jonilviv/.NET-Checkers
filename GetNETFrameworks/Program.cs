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
            Get45PlusFromRegistry();
            Console.WriteLine();
            GetNETUpdates();
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


        //https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
        private static void Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                using (RegistryKey ndpKey = baseKey.OpenSubKey(subkey))
                {
                    if (ndpKey == null)
                    {
                        return;
                    }

                    if (ndpKey.GetValue("Release") == null)
                    {
                        Console.WriteLine(".NET Framework Version is not detected.");
                    }
                    else
                    {
                        int releaseKey = (int)ndpKey.GetValue("Release");
                        Console.WriteLine(".NET Framework Version: " + CheckFor45PlusVersion(releaseKey));
                    }

                    ndpKey.Close();
                }

                baseKey.Close();
            }
        }

        private static string CheckFor45PlusVersion(int releaseKey)
        {
            switch (releaseKey)
            {
                case 378389: return ".NET Framework 4.5";
                case 378675: return ".NET Framework 4.5.1 installed with Windows 8.1";
                case 378758: return ".NET Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2";
                case 379893: return ".NET Framework 4.5.2";
                case 393295: return ".NET Framework 4.6 installed with Windows 10";
                case 393297: return ".NET Framework 4.6 installed on all other Windows OS versions";
                case 394254: return ".NET Framework 4.6.1 installed on Windows 10";
                case 394271: return ".NET Framework 4.6.1 installed on all other Windows OS versions";
                case 394802: return ".NET Framework 4.6.2 installed on Windows 10 Anniversary Update";
                case 394806: return ".NET Framework 4.6.2 installed on all other Windows OS versions";
                case 460798: return ".NET Framework 4.7 installed on Windows 10 Creators Update";
                case 460805: return ".NET Framework 4.7 installed on all other Windows OS versions";
                case 461308: return ".NET Framework 4.7.1 installed on Windows 10 Fall Creators Update";
                case 461310: return ".NET Framework 4.7.1 installed on all other Windows OS versions";
                case 461808: return ".NET Framework 4.7.2 installed on On Windows 10 April 2018 Update";
                case 461814: return ".NET Framework 4.7.2 installed on all other Windows OS versions";
                default: return "No 4.5 or later version detected";
            }
        }

        private static void GetNETUpdates()
        {
            const string key = @"SOFTWARE\Microsoft\Updates";

            using (RegistryKey mainKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                using (RegistryKey baseKey = mainKey.OpenSubKey(key))
                {
                    if (baseKey == null)
                    {
                        return;
                    }

                    string[] baseSubKeys = baseKey.GetSubKeyNames();

                    foreach (string baseKeyName in baseSubKeys)
                    {
                        if (!baseKeyName.Contains(".NET Framework"))
                        {
                            continue;
                        }

                        using (RegistryKey updateKey = baseKey.OpenSubKey(baseKeyName))
                        {
                            if (updateKey == null)
                            {
                                continue;
                            }

                            Console.WriteLine(baseKeyName);

                            string[] updateSubKeys = updateKey.GetSubKeyNames();

                            foreach (string kbKeyName in updateSubKeys)
                            {
                                Console.WriteLine("  " + kbKeyName);
                            }

                            updateKey.Close();
                        }
                    }

                    baseKey.Close();
                }

                mainKey.Close();
            }
        }
    }
}