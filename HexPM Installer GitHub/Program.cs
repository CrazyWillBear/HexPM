using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                Console.WriteLine("HexPM Installer  >>  An error has occured" + ex);
                Console.ReadKey(true);
            }
        }
        static void Uninstall()
        {
            if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM"))
            {
                Directory.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM", true);
            }
            var currentValue = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User).Split(';');
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
                Directory.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM", true);
                Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM");
            }
            else
            {
                Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM");
            }
            var currentValue = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            var newValue = currentValue + @";C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM";
            Environment.SetEnvironmentVariable("PATH", newValue, EnvironmentVariableTarget.User);

        }
        public static int intInput;
        public static bool choosingVersion = true;
        static void Install()
        {
            var client = new WebClient();
            string[] availableVersions = client.DownloadString("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/availableversions.txt").Split('\n');
            Console.WriteLine("HexPM Installer >>  In order to continue the installation, you must agree to the license (https://unlicense.org/). (Press any button to agree with the license and continue the installation, press ctrl+c or close out of the installer to cancel the installation)");
            Console.ReadKey(true);
            while (choosingVersion)
            {
                Console.WriteLine("HexPM Installer >>  Which version of HexPM would you like to download? (Enter the corresponding number)\n");
                for (int i = 0; i < availableVersions.Length; i++)
                {
                    Console.Write("   (" + i + ")" + availableVersions[i] + "   ");
                }
                try
                {
                    intInput = int.Parse(Console.ReadLine());
                    if (intInput > -1 && intInput < availableVersions.Length + 1)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFile(new System.Uri("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/" + availableVersions[intInput] + ".exe"), @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.exe");
                            wc.DownloadFile(new System.Uri("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/" + "license.txt"), @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\license.txt");
                            choosingVersion = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("HexPM Installer >>  That is not a valid entry, please try again\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("HexPM Installer >>  An error has occured");
                    Console.ReadKey(true);
                }
            }
            Console.WriteLine("HexPM Installer >>  HexPM should be installed, please contact support through our Discord server if something fails");
            Console.ReadKey(true);

        }
    }
}
