using FuzzySharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HexPM
{
    class Functions
    {
        //referencing HexPM.cs
        private HexPM hexpm = new HexPM();

        //QUICK NOTE: There are no more comments in the rest of this class explaining code or anything like that
        public static void updatePkgList()
        {
            var client = new WebClient();
            Console.WriteLine("\n-- Updating packagelist...");
            client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
            Console.WriteLine("     (Updated packagelist)");
        }

        public static void checkHexPMVersion()
        {
            var client = new WebClient();
            string latestVersion = client.DownloadString("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/latestversion.txt");
            Console.WriteLine("-- Current HexPM version: " + HexPM.version);
            Console.WriteLine("-- Latest available HexPM version: " + latestVersion);
            if (latestVersion == HexPM.version)
            {
                Console.WriteLine("     (You do not need to update HexPM)");
                Environment.Exit(0);
            }
            if (latestVersion != HexPM.version)
            {
                Console.WriteLine("     (You should update HexPM *by running the latest installer*)");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("ERROR! Exception: \nUnable to determine if update is necessary");
                Environment.Exit(0);
            }
        }

        public static void searchPkgList(string searchQuery)
        {
            var client = new WebClient();
            Console.WriteLine("\n-- Updating packagelist...");
            client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
            Console.WriteLine("     (Updated packagelist)");
            Console.WriteLine("-- Searching packagelist for: " + searchQuery);
            string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
            for (int i = 0; i < text.Length; i++)
            {
                if (Fuzz.Ratio(searchQuery.ToLower(), text[i].Split(';')[0].ToLower()) >= 65)
                {
                    Console.WriteLine("     (Found: " + text[i].Split(';')[0] + ")");
                }
            }
        }

        public static void updatePkg(string directory, string mostRecentVersion)
        {
            var client = new WebClient();
            Console.WriteLine("-- Updating " + directory + "...");
            client.DownloadFile("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/" + directory.ToLower() + ".is", "C:/Users/" + Environment.UserName + "/Downloads/" + directory.ToLower() + ".is");
            BetterISParser.silentParseIS("C:/Users/" + Environment.UserName + "/Downloads/" + directory.ToLower() + ".is");
            DateTime now = DateTime.Now;
            System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\versionhistory.txt", "\n" + directory.ToLower() + ";" + mostRecentVersion + ";" + now.ToString("F"));
            Console.WriteLine("     (Updated " + directory + ")");
        }

        public static void checkPkgVersion(string input)
        {
            string[] pkgListText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
            string mostRecentVersion = "";
            string packageName = "";
            for (int i = 0; i < pkgListText.Length; i++)
            {
                string[] pkgListTextSplit = pkgListText[i].Split(';');
                if (pkgListTextSplit[0].ToLower() == input.ToLower())
                {
                    mostRecentVersion = pkgListTextSplit[2];
                    packageName = pkgListTextSplit[0];
                    break;
                }
            }
            string[] versionHistoryText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\versionhistory.txt");
            for (int v = versionHistoryText.Length - 1; v > -1; v--)
            {
                string[] versionHistoryTextSplit = versionHistoryText[v].Split(';');
                if (versionHistoryTextSplit[0].ToLower() == input.ToLower())
                {
                    Console.WriteLine("-- " + packageName + " is currently on version " + versionHistoryTextSplit[1]);
                    Console.WriteLine("-- " + packageName + " is available on version " + mostRecentVersion);
                    Environment.Exit(0);
                }
            }
        }

        public static void silentCheckHexPMVersion()
        {
            var client = new WebClient();
            string latestVersion = client.DownloadString("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/latestversion.txt");
            if (latestVersion == HexPM.version)
            {
                Console.WriteLine("-- You do not need to update HexPM");
                Environment.Exit(0);
            }
            if (latestVersion != HexPM.version)
            {
                Console.WriteLine("-- You should update HexPM *by running the latest installer*");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("ERROR! Exception: \nUnable to determine if update is necessary");
                Console.ReadKey(true);
                Environment.Exit(1);
            }
        }
    }
}
