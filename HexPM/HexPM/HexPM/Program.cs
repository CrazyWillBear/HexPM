using FuzzySharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace HexPM
{
    internal class Program
    {
        public static string version = "v0.7 beta";
        
        private static void Main(string[] args)
        {
            try
            {
                if (args[0] == "--updatelist" || args[0] == "-l")
                {
                    var client = new WebClient();
                    try
                    {
                        Console.WriteLine("\n-- Updating packagelist...");
                        client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        Console.WriteLine("     (Updated packagelist)");
                        Environment.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR! Exception:\n " + ex);
                    }
                }
                if (args[0] == "--install" || args[0] == "-i")
                {
                    var client = new WebClient();
                    Console.WriteLine("\n-- Updating packagelist...");
                    client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    Console.WriteLine("     (Updated packagelist)");
                    if (File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt"))
                    {
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Please specify a package!");
                            Environment.Exit(0);
                        }
                        string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        string packageName = args[1];
                        Console.WriteLine("-- Searching for package in packagelist...");
                        bool packagefound = false;
                        for (int i = 0; i < text.Length; i++)
                        {
                            string[] textSplit = text[i].Split(';');
                            if (textSplit[0].ToLower() == packageName.ToLower())
                            {
                                Console.WriteLine("     (Package exists)\n");
                                packagefound = true;
                                try
                                {
                                    BetterISParser BetterISParser = new BetterISParser();
                                    client = new WebClient();
                                    client.DownloadFile(textSplit[1], "C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                                    BetterISParser.parseIS("C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                                    Console.ReadKey(true);
                                    DateTime now = DateTime.Now;
                                    System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\versionhistory.txt", "\n" + packageName.ToLower() + ";" + textSplit[2] + ";" + now.ToString("F"));
                                    File.Delete("C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
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
                if (args[0] == "--hexpmversion" || args[0] == "-V")
                {
                    var client = new WebClient();
                    string latestVersion = client.DownloadString("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/latestversion.txt");
                    Console.WriteLine("\n-- Current software version: " + version);
                    Console.WriteLine("-- Latest available version: " + latestVersion);
                    if (latestVersion == version)
                    {
                        Console.WriteLine("     (You do not need to update)");
                        Environment.Exit(0);
                    }
                    if (latestVersion != version)
                    {
                        Console.WriteLine("     (You should update this software *by running the latest installer*)");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("ERROR! Exception: \nUnable to determine if update is necessary");
                        Environment.Exit(0);
                    }
                }
                if (args[0] == "--uninstall" || args[0] == "-u")
                {
                    Console.WriteLine("\n-- Searching install history...");
                    string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt");
                    for (int i = 0; i < text.Length; i++)
                    {
                        string[] textSplit = text[i].Split(';');
                        if (args[1].ToLower() == textSplit[0].ToLower())
                        {
                            if (Directory.Exists(textSplit[1]))
                            {
                                Console.WriteLine("-- Deleting files...");
                                Directory.Delete(textSplit[1], true);
                                Console.WriteLine("     (Deleted program files)");
                                File.Delete(textSplit[2]);
                                Console.WriteLine("     (Deleted shortcut)");
                                if (textSplit[4].Contains("EnvVarTrue"))
                                {
                                    string[] envVarSplit = textSplit[4].Split('-');
                                    var currentUninstallValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
                                    List<string> newValueUninstall = new List<string>();
                                    string[] dirSplit = envVarSplit[1].Split('\\');
                                    for (int d = 0; d < dirSplit.Length; d++)
                                    {
                                        if (dirSplit[d] == "Username")
                                        {
                                            dirSplit[d] = Environment.UserName;
                                        }
                                        dirSplit[d] = dirSplit[d] + "\\";
                                    }
                                    string dir = string.Join("", dirSplit);
                                    if (Environment.GetEnvironmentVariable("Path").Contains(dir))
                                    {
                                        Console.WriteLine("-- Deleting environment variable...");
                                        for (int g = 0; g < currentUninstallValue.Length; g++)
                                        {
                                            if (currentUninstallValue[g] == dir)
                                            {
                                            }
                                            else
                                            {
                                                newValueUninstall.Add(currentUninstallValue[g]);
                                            }
                                        }
                                        string[] newValue2 = newValueUninstall.ToArray();
                                        Environment.SetEnvironmentVariable("Path", string.Join(";", newValue2), EnvironmentVariableTarget.User);
                                        Console.WriteLine("     (Deleted environment variable)");
                                    }
                                }
                                Console.WriteLine("     (Uninstalled " + textSplit[0] + ")");
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("     (This package was already uninstalled)");
                                Environment.Exit(0);
                            }
                        }
                    }
                    Console.WriteLine("-- This package isn't in your install history! Did you install it through this package manager?");
                    Environment.Exit(0);
                }
                if (args[0] == "--help" || args[0] == "-h")
                {
                    Console.WriteLine("\nHelp Menu:");
                    Console.WriteLine("For details on proper syntax and tips on how to more effectively use HexPM, visit our Wiki (https://github.com/CrazyWillBear/HexPM/wiki)");
                    Console.WriteLine("\n-- install <packagename>, i <packagename>:\n     (installs selected package)");
                    Console.WriteLine("-- uninstall <packagename>, u <packagename>:\n     (uninstalls selected package)");
                    Console.WriteLine("-- hexpmversion, V:\n     (displays HexPM version information)");
                    Console.WriteLine("-- version <packagename>, v <packagename>:\n     (displays version information about the selected package)");
                    Console.WriteLine("-- updatelist, l:\n     (updates packagelist)");
                    Console.WriteLine("-- search <query>, s <query>:\n     (searches packagelist for the query you input, displays packages who's names contain your query)");
                    Console.WriteLine("-- runis <.isfilename>, r <.isfilename>:\n     (runs an installer script (go to HexPM wiki for usage info)");
                    Console.WriteLine("-- updateall, U:\n     (updates all applications that require updates)");
                    Console.WriteLine("-- installed packages, p:\n     (displays list of all currently installed packages)\n");
                    Environment.Exit(0);
                }
                if (args[0] == "--search" || args[0] == "-s")
                {
                    string searchQuery = args[1];
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
                    Environment.Exit(0);
                }
                if (args[0] == "--installedpackages" || args[0] == "-p")
                {
                    string[] directories = Directory.GetDirectories("C:/Users/" + Environment.UserName + "/HexPM");
                    for (int i = 0; i < directories.Length; i++)
                    {
                        directories[i] = directories[i].Split('\\')[1];
                    }
                    string combinedDirectories = string.Join(", ", directories);
                    Console.WriteLine("\n-- Installed packages:\n     (" + combinedDirectories + ")");
                    Environment.Exit(0);
                }
                if (args[0] == "--runis" || args[0] == "-r")
                {
                    try
                    {
                        BetterISParser.parseIS(@"C:\Users\" + Environment.UserName + @"\Desktop\" + args[1]);
                        Environment.Exit(0);
                    }
                    catch
                    {
                        Console.WriteLine("\nERROR! Exception: \nInstaller Script Doesn't Exist!");
                        Console.WriteLine("Your installer script (.is) file needs to be on your desktop. Then run the 'r' or 'runis' command once again followed by the installer script file name with the file extension included");
                        Environment.Exit(0);
                    }
                }
                if (args[0] == "--updateall" || args[0] == "-U")
                {
                    try
                    {
                        var client = new WebClient();
                        Console.WriteLine("\n-- Updating packagelist...");
                        client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        Console.WriteLine("     (Updated packagelist)");
                        string[] pkgListText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                        string[] directories = Directory.GetDirectories("C:/Users/" + Environment.UserName + "/HexPM");
                        for (int i = 0; i < directories.Length; i++)
                        {
                            string mostRecentVersion = "0";
                            directories[i] = directories[i].Split('\\')[1];
                            for (int x = 0; i < pkgListText.Length; x++)
                            {
                                string[] pkgListTextSplit = pkgListText[x].Split(';');
                                if (pkgListTextSplit[0] == directories[i])
                                {
                                    mostRecentVersion = pkgListTextSplit[2];
                                    break;
                                }
                            }
                            string[] versionHistoryText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\versionhistory.txt");
                            for (int v = versionHistoryText.Length - 1; v > -1; v--)
                            {
                                string[] versionHistoryTextSplit = versionHistoryText[v].Split(';');
                                if (versionHistoryTextSplit[0] == directories[i].ToLower())
                                {
                                    if (versionHistoryTextSplit[1] != mostRecentVersion)
                                    {
                                        Console.WriteLine("-- Updating " + directories[i] + "...");
                                        client.DownloadFile("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/" + directories[i].ToLower() + ".is", "C:/Users/" + Environment.UserName + "/Downloads/" + directories[i].ToLower() + ".is");
                                        BetterISParser.silentParseIS("C:/Users/" + Environment.UserName + "/Downloads/" + directories[i].ToLower() + ".is");
                                        DateTime now = DateTime.Now;
                                        System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\versionhistory.txt", "\n" + directories[i].ToLower() + ";" + mostRecentVersion + ";" + now.ToString("F"));
                                        Console.WriteLine("     (Updated " + directories[i] + ")");
                                        break;
                                    }
                                    if (versionHistoryTextSplit[1] == mostRecentVersion)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        Console.WriteLine("-- Successfully updated all installed packages that require updates and packagelist");
                        string latestVersion = client.DownloadString("https://HexPM-Installer-Script-Mirrors.crazywillbear.repl.co/latestversion.txt");
                        if (latestVersion == version)
                        {
                            Console.WriteLine("-- You do not need to update HexPM by running the latest installer");
                            Environment.Exit(0);
                        }
                        if (latestVersion != version)
                        {
                            Console.WriteLine("-- You should update HexPM by running the latest installer");
                            Environment.Exit(0);
                        }
                        Environment.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR! Exception:\n" + ex);
                        Environment.Exit(1);
                    }
                }
                if (args[0] == "--version" || args[0] == "-v")
                {
                    var client = new WebClient();
                    Console.WriteLine("\n-- Updating packagelist...");
                    client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/packagelist.txt", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    Console.WriteLine("     (Updated packagelist)");
                    string[] pkgListText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    string mostRecentVersion = "";
                    string packageName = "";
                    for (int i = 0; i < pkgListText.Length; i++)
                    {
                        string[] pkgListTextSplit = pkgListText[i].Split(';');
                        if (pkgListTextSplit[0].ToLower() == args[1].ToLower())
                        {
                            mostRecentVersion = pkgListTextSplit[2];
                            packageName = pkgListTextSplit[0];
                            break;
                        }
                    }
                    string[] versionHistoryText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\versionhistory.txt");
                    for (int v = versionHistoryText.Length - 1; v > -1; v--)
                    {
                        string[] versionHistoryTextSplit = versionHistoryText[v].Split(';');
                        if (versionHistoryTextSplit[0].ToLower() == args[1].ToLower())
                        {
                            Console.WriteLine("-- " + packageName + " is currently on version " + versionHistoryTextSplit[1]);
                            Console.WriteLine("-- " + packageName + " is available on version " + mostRecentVersion);
                            Environment.Exit(0);
                        }
                    }
                    Console.WriteLine("-- Package isn't installed! Check your spelling and/or make sure this package is installed!");
                }
                else
                {
                    Console.WriteLine("ERROR! Exception: \nCommand unknown.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR! Exception:\n" + ex);
                Environment.Exit(1);
            }
        }
    }
}