using System;
using System.IO;
using System.Net;

namespace HexPM
{
    internal class Program
    {
        public static string version = "v0.3 beta";

        private static void Main(string[] args)
        {
            try
            {
                if (args[0] == "--updatelist" || args[0] == "-U")
                {
                    var client = new WebClient();
                    Console.WriteLine("Attempting to download packagelist...");
                    try
                    {
                        client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        Console.WriteLine("Download successful!");
                        Environment.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR! Exception:\n " + ex);
                    }
                }
                if (args[0] == "--install" || args[0] == "-i")
                {
                    Console.WriteLine("Updating packagelist...");
                    var client = new WebClient();
                    client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    Console.WriteLine("Packagelist successfully updated!");
                    if (File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt"))
                    {
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Please specify a package!");
                            Environment.Exit(0);
                        }
                        string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        string packageName = args[1];
                        Console.WriteLine("Searching for package in packagelist...");
                        bool packagefound = false;
                        for (int i = 0; i < text.Length; i++)
                        {
                            string[] textSplit = text[i].Split(';');
                            if (textSplit[0].ToLower() == packageName.ToLower())
                            {
                                Console.WriteLine("Package exists!");
                                packagefound = true;
                                try
                                {
                                    ISParser ISParser = new ISParser();
                                    Console.WriteLine("Gathering information from packagelist...");
                                    client = new WebClient();
                                    Console.WriteLine("Downloading installer script...");
                                    client.DownloadFile(textSplit[1], "C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                                    Console.WriteLine("Running installer script...");
                                    ISParser.ParseFile("C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                                    Console.ReadKey(true);
                                    Environment.Exit(0);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("ERROR! Exception:\n " + ex);
                                    Console.ReadKey(true);
                                }
                            }
                        }
                        if (packagefound == false)
                        {
                            Console.WriteLine("ERROR! Exception: \nPackageDoesn'tExist, try using the 'U' command");
                            Console.ReadKey(true);
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Packagelist does not exist! Run the 'ulist' command to download the latest available packagelist.");
                        Environment.Exit(0);
                    }
                }
                if (args[0] == "--clear" || args[0] == "-c")
                {
                    Console.Clear();
                    Environment.Exit(0);
                }
                if (args[0] == "--version" || args[0] == "-v")
                {
                    var client = new WebClient();
                    string latestVersion = client.DownloadString("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/latestversion.txt");
                    Console.WriteLine("Current software version: " + version + "\nLatest available version: " + latestVersion);
                    if (latestVersion == version)
                    {
                        Console.WriteLine("You do not need to update.");
                        Environment.Exit(0);
                    }
                    if (latestVersion != version)
                    {
                        Console.WriteLine("You should update this software (by running the latest installer).");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("ERROR! Exception: \nUnable to determine if update is necessary");
                        Environment.Exit(0);
                    }
                }
                if (args[0] == "--license" || args[0] == "-l")
                {
                    Console.WriteLine("HexPM: This software is licensed under the Unlicense");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("https://unlicense.org/");
                    Console.ForegroundColor = ConsoleColor.White;
                    Environment.Exit(0);
                }
                if (args[0] == "--uninstall" || args[0] == "-u")
                {
                    try
                    {
                        Console.WriteLine("Searching through install history...");
                        string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt");
                        for (int i = 0; i < text.Length; i++)
                        {
                            string[] textSplit = text[i].Split(';');
                            if (args[1].ToLower() == textSplit[0].ToLower())
                            {
                                if (Directory.Exists(textSplit[1]))
                                {
                                    Directory.Delete(textSplit[1], true);
                                    File.Delete(textSplit[2]);
                                    Console.WriteLine("Successfully uninstalled!");
                                    Environment.Exit(0);
                                }
                                else
                                {
                                    Console.WriteLine("This package was already uninstalled.");
                                    Environment.Exit(0);
                                }
                            }
                        }
                        Console.WriteLine("This package isn't in your install history! Did you install it through this package manager?");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: Exception: \n" + ex);
                    }
                }
                if (args[0] == "--help" || args[0] == "-h")
                {
                    Console.WriteLine("\nFor details on proper syntax and tips on how to more effectively use HexPM, visit our Wiki (https://succandcap.glitch.me/pages/hexpm.html)");
                    Console.WriteLine("\ninstall <packagename>, i <packagename>: installs selected package");
                    Console.WriteLine("uninstall <packagename>, u <packagename>: uninstalls selected package");
                    Console.WriteLine("version, v: displays version information");
                    Console.WriteLine("license, l: displays software license information");
                    Console.WriteLine("clear, c: clears console window");
                    Console.WriteLine("updatelist, U: updates packagelist\n");
                    Console.WriteLine("search <query>, s <query>: searches packagelist for the query you input, displays packages who's names contain your query\n");
                    Environment.Exit(0);
                }
                if (args[0] == "--search" || args[0] == "-s")
                {
                    string searchQuery = args[1];
                    Console.WriteLine("Updating packagelist...");
                    var client = new WebClient();
                    client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    Console.WriteLine("Packagelist successfully updated!");
                    Console.WriteLine("Searching packagelist for: " + searchQuery);
                    string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i].Split(';')[0].ToLower().Contains(searchQuery.ToLower()))
                        {
                            Console.WriteLine("Found: " + text[i].Split(';')[0]);
                        }
                    }
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("ERROR! Exception: \nCommand unknown.");
                }
            }
            catch
            {
                Console.WriteLine("\nHexPM  >>  You are now in Shell Mode! You no longer have to prefix commands with 'hexpm', just type the command");
                ShellMode();
            }

            void ShellMode()
            {
                while (true)
                {
                    Console.Write("HexPM  >>  ");
                    string input = Console.ReadLine();
                    string[] inputSplit = input.Split(' ');
                    if (inputSplit[0] == "U" || inputSplit[0] == "updatelist")
                    {
                        var client = new WebClient();
                        Console.WriteLine("Attempting to download packagelist...");
                        try
                        {
                            client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                            Console.WriteLine("Download successful!");
                            ShellMode();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR! Exception:\n " + ex);
                            Console.ReadKey(true);
                            ShellMode();
                        }
                    }
                    if (inputSplit[0] == "install" || inputSplit[0] == "i")
                    {
                        Console.WriteLine("Updating packagelist...");
                        var client = new WebClient();
                        client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        Console.WriteLine("Packagelist successfully updated!");
                        if (File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt"))
                        {
                            if (inputSplit.Length < 2)
                            {
                                Console.WriteLine("Please specify a package!");
                                ShellMode();
                            }
                            string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                            string packageName = inputSplit[1];
                            Console.WriteLine("Searching for package in packagelist...");
                            for (int i = 0; i < text.Length; i++)
                            {
                                string[] textSplit = text[i].Split(';');
                                if (textSplit[0].ToLower() == packageName.ToLower())
                                {
                                    Console.WriteLine("Package exists!");
                                    try
                                    {
                                        ISParser ISParser = new ISParser();
                                        Console.WriteLine("Gathering information from packagelist...");
                                        client = new WebClient();
                                        Console.WriteLine("Downloading installer script...");
                                        client.DownloadFile(textSplit[1], "C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                                        Console.WriteLine("Running installer script...");
                                        ISParser.ParseFile("C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                                        Console.ReadKey(true);
                                        ShellMode();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("ERROR! Exception: \n" + ex);
                                        Console.ReadKey(true);
                                        ShellMode();
                                    }
                                }
                            }
                            Console.WriteLine("ERROR! Exception: \nPackageDoesn'tExist, try running the 'ulist' command");
                            Console.ReadKey(true);
                            ShellMode();
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Packagelist does not exist! Run the 'U' command to download the latest available packagelist.");
                            ShellMode();
                        }
                    }
                    if (inputSplit[0] == "clear" || inputSplit[0] == "c")
                    {
                        Console.Clear();
                        ShellMode();
                    }
                    if (inputSplit[0] == "version" || inputSplit[0] == "v")
                    {
                        var client = new WebClient();
                        string latestVersion = client.DownloadString("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/latestversion.txt");
                        Console.WriteLine("Current software version: " + version + "\nLatest available version: " + latestVersion);
                        if (latestVersion == version)
                        {
                            Console.WriteLine("You do not need to update.");
                            ShellMode();
                        }
                        if (latestVersion != version)
                        {
                            Console.WriteLine("You should update this software (by running the latest installer).");
                            ShellMode();
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Exception: \nUnable to determine if update is necessary");
                            ShellMode();
                        }
                    }
                    if (inputSplit[0] == "license" || inputSplit[0] == "l")
                    {
                        Console.WriteLine("HexPM: This software is licensed under the Unlicense");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("https://unlicense.org/");
                        Console.ForegroundColor = ConsoleColor.White;
                        ShellMode();
                    }
                    if (inputSplit[0] == "uninstall" || inputSplit[0] == "u")
                    {
                        try
                        {
                            Console.WriteLine("Searching through install history...");
                            string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt");
                            for (int i = 0; i < text.Length; i++)
                            {
                                string[] textSplit = text[i].Split(';');
                                if (inputSplit[1].ToLower() == textSplit[0].ToLower())
                                {
                                    Console.WriteLine("Found! Uninstalling...");
                                    if (Directory.Exists(textSplit[1]))
                                    {
                                        Directory.Delete(textSplit[1], true);
                                        File.Delete(textSplit[2]);
                                        Console.WriteLine("Successfully uninstalled!");
                                        ShellMode();
                                    }
                                    else
                                    {
                                        Console.WriteLine("This package was already uninstalled.");
                                        ShellMode();
                                    }
                                }
                            }
                            Console.WriteLine("This package isn't in your install history! Did you install it through this package manager?");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR: Exception: \n" + ex);
                        }
                    }
                    if (inputSplit[0] == "help" || inputSplit[0] == "h")
                    {
                        Console.WriteLine("\nFor details on proper syntax and tips on how to more effectively use HexPM, visit our Wiki (https://succandcap.glitch.me/pages/hexpm.html)");
                        Console.WriteLine("\ninstall <packagename>, i <packagename>: installs selected package");
                        Console.WriteLine("uninstall <packagename>, u <packagename>: uninstalls selected package");
                        Console.WriteLine("version, v: displays version information");
                        Console.WriteLine("license, l: displays software license information");
                        Console.WriteLine("clear, c: clears console window");
                        Console.WriteLine("updatelist, U: updates packagelist\n");
                        Console.WriteLine("search <query>, s <query>: searches packagelist for the query you input, displays packages who's names contain your query\n");
                        ShellMode();
                    }
                    if (inputSplit[0] == "search" || inputSplit[0] == "s")
                    {
                        string searchQuery = inputSplit[1];
                        Console.WriteLine("Updating packagelist...");
                        var client = new WebClient();
                        client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        Console.WriteLine("Packagelist successfully updated!");
                        Console.WriteLine("Searching packagelist for: " + searchQuery);
                        string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (text[i].Split(';')[0].ToLower().Contains(searchQuery.ToLower()))
                            {
                                Console.WriteLine("Found: " + text[i].Split(';')[0]);
                            }
                        }
                        ShellMode();
                    }
                    else
                    {
                        Console.WriteLine("ERROR! Exception: \nCommand unknown.");
                    }
                }
            }
        }
    }
}