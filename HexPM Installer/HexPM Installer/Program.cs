using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HexPM_Installer
{
    class Program
    {
        static void Main ()
        {
            Console.WriteLine("HexPM Installer >>  Would you like to uninstall(0) or install(1) HexPM (type corresponding number, then press enter)");
            int textinput = int.Parse(Console.ReadLine());
            try
            {
                if (textinput == 1)
                {
                    PrepareInstall();
                    Install();
                }
                if (textinput == 0)
                {
                    Uninstall();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HexPM Installer  >>  An error has occured\n" + ex);
                Console.ReadKey(true);
            }
        }
        static void Uninstall()
        {
            if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM"))
            {
                Directory.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM", true);
            }
            var currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
            List<string> newValueUninstall = new List<string>();
            if (Environment.GetEnvironmentVariable("Path").Contains(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM"))
            {
                for (int i = 0; i < currentValue.Length; i++)
                {
                    if (currentValue[i] == @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM")
                    {
                    }
                    else
                    {
                        newValueUninstall.Add(currentValue[i]);
                    }
                }
                string[] newValue2 = newValueUninstall.ToArray();
                Environment.SetEnvironmentVariable("Path", string.Join(";", newValue2), EnvironmentVariableTarget.User);
            }
            Console.WriteLine("HexPM Installer >>  HexPM should be uninstalled, please contact support through our Discord server if something fails");
            Console.ReadKey(true);
        }
        static void PrepareInstall()
        {
            if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM"))
            {
                if (File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.exe"))
                {
                    File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.exe");
                }
                if (File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\FuzzySharp.dll"))
                {
                    File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\FuzzySharp.dll");
                }
                if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\cache")) {
                    Directory.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\cache", true);
                }
                Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\cache");
                if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history")) { }
                else
                {
                    Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history");
                }
            }
            else
            {
                Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM");
            }
            if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\HexPM")) { }
            else
            {
                Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\HexPM");
            }
            var currentUninstallValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
            List<string> newValueUninstall = new List<string>();
            if (Environment.GetEnvironmentVariable("Path").Contains(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM"))
            {
                for (int g = 0; g < currentUninstallValue.Length; g++)
                {
                    if (currentUninstallValue[g] == @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM")
                    {
                    }
                    else
                    {
                        newValueUninstall.Add(currentUninstallValue[g]);
                    }
                }
                string[] newValue2 = newValueUninstall.ToArray();
                Environment.SetEnvironmentVariable("Path", string.Join(";", newValue2), EnvironmentVariableTarget.User);
            }
            string[] currentValueArray = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
            string currentValue = string.Join(";", currentValueArray);
            var newValue = currentValue + @";C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM";
            Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);

        }
        static void Install()
        {
            var client = new WebClient();
            Console.WriteLine("HexPM Installer >>  In order to continue the installation, you must agree to the software's license, The Unlicense (https://unlicense.org/). (By pressing any button to continue the installation you agree to the software's license)");
            Console.ReadKey(true);
            Console.WriteLine("\nHexPM Installer >>  Installing...");
            client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/HexPM.zip", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.zip");
            ZipFile.ExtractToDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.zip", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM");
            File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.zip");
            Console.WriteLine("\nHexPM Installer >>  HexPM should be installed, please contact support through our Discord server if something fails");
            Console.ReadKey(true);
        }
    }
}
