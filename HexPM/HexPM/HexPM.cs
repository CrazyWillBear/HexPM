using FuzzySharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace HexPM
{
    internal class HexPM
    {
        /*
        Hello! This is CaptainBear (aka CrazyWillBear) speaking, just here to let you know that throughout
        this class there are comments explaining code and stuff. The other class, Functions.cs, is where
        most if not all functions were written. Fair warning, the uninstall command's code is a bit messy
        and I didn't want to risk messing it up by making a function in Functions.cs, just a friendly
        warning. Anyways, enjoy HexPM's code!

           -CaptainBear (aka CrazyWillBear)
        */

        //defining version variable
        public static string version = "v0.8.1 beta";

        //referencing Functions.cs
        private Functions functions = new Functions();

        private static void Main(string[] args)
        {
            //trying to run all code and catches any errors
            try
            {
                if (args[0] == "ulist")
                {
                    //updating packagelist
                    Functions.updatePkgList();

                    Environment.Exit(0);
                }
                if (args[0] == "install")
                {
                    //updating packagelist
                    Functions.updatePkgList();

                    //checking to see if the command is shorter than 2 arguments, and if so then telling the user to specify a package
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please specify a package!");
                        Environment.Exit(0);
                    }

                    //defining variables
                    string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    string packageName = args[1];
                    bool packagefound = false;

                    Console.WriteLine("-- Searching for package in packagelist...");

                    //a for loop lasting as long as the packagelist itself
                    for (int i = 0; i < text.Length; i++)
                    {
                        string[] textSplit = text[i].Split(';');

                        if (textSplit[0].ToLower() == packageName.ToLower())
                        {
                            //declaring that package exists, such that the package manager doesn't display that the package doesn't exist
                            Console.WriteLine("     (Package exists)\n");
                            packagefound = true;

                            //referencing BetterISParser.cs
                            BetterISParser BetterISParser = new BetterISParser();

                            //downloading installer script and parsing it
                            var client = new WebClient();
                            client.DownloadFile(textSplit[1], "C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                            BetterISParser.parseIS("C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");
                            Console.ReadKey(true);

                            //saving the installation in versionhistory.txt
                            DateTime now = DateTime.Now;
                            System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\versionhistory.txt", "\n" + packageName.ToLower() + ";" + textSplit[2] + ";" + now.ToString("F"));
                            File.Delete("C:/Users/" + Environment.UserName + "/Downloads/" + textSplit[0] + ".is");

                            //exiting environment
                            Environment.Exit(0);
                        }
                    }

                    //if a package wasn't found throughout the process, then display an error saying that the package doesn't exist
                    if (packagefound == false)
                    {
                        Console.WriteLine("ERROR! Exception: \nPackage Doesn't Exist, try searching for a package using the 's' command");
                        Console.ReadKey(true);
                        Environment.Exit(0);
                    }
                }
                if (args[0] == "clear") /* useless */
                {
                    //clearing console
                    Console.Clear();
                    Environment.Exit(0);
                }
                if (args[0] == "version")
                {
                    Console.WriteLine();
                    if (args.Length != 1) { Functions.checkPkgVersion(args[1]); }
                    else { Functions.checkHexPMVersion(); }
                }
                if (args[0] == "remove" || args[0] == "uninstall")
                {
                    Console.WriteLine("\n-- Searching install history...");

                    //defining `text` variable as all lines of installhistory
                    string[] text = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\installhistory.txt");

                    //setting up for loop to last as long as installhistory
                    for (int i = 0; i < text.Length; i++)
                    {
                        //defining `textSplit`
                        string[] textSplit = text[i].Split(';');

                        //if the inputted packagename = the packagename of the current installhistory line, then...
                        if (args[1].ToLower() == textSplit[0].ToLower())
                        {
                            //if the directory exists as written in installhistory, then...
                            if (Directory.Exists(textSplit[1]))
                            {
                                Console.WriteLine("-- Deleting files...");

                                //deleting program files
                                Directory.Delete(textSplit[1], true);
                                Console.WriteLine("     (Deleted program files)");

                                //deleting shortcut file
                                File.Delete(textSplit[2]);
                                Console.WriteLine("     (Deleted shortcut)");

                                //if the program created an environment variable, then remove the environment variable
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
                            //if the directory doesn't exist as written in installhistory, then cancel the uninstallion command
                            else
                            {
                                Console.WriteLine("     (This package was already uninstalled)");
                                Environment.Exit(0);
                            }
                        }
                    }

                    //if all fails, it means the package isn't in installhistory
                    Console.WriteLine("-- This package isn't in your install history! Did you install it through this package manager?");
                    Environment.Exit(0);
                }
                if (args[0] == "help")
                {
                    //writing help menu
                    Console.WriteLine("\nHelp Menu:");
                    Console.WriteLine("For details on proper syntax and tips on how to more effectively use HexPM, visit our Wiki (https://github.com/CrazyWillBear/HexPM/wiki)");
                    Console.WriteLine("\n-- install <packagename>:\n     (installs selected package)");
                    Console.WriteLine("-- remove <packagename>, r <packagename>:\n     (uninstalls/removes selected package)");
                    Console.WriteLine("-- version <packagename> *or* version:\n     (displays version information about the selected package, if package is blank will display HexPM version info)");
                    Console.WriteLine("-- ulist:\n     (updates packagelist)");
                    Console.WriteLine("-- search <query>:\n     (searches packagelist for the query you input, displays packages who's names contain your query)");
                    Console.WriteLine("-- parse <.isfilename>:\n     (runs an installer script (go to HexPM wiki for usage info)");
                    Console.WriteLine("-- updateall:\n     (updates all applications that require updates)");
                    Console.WriteLine("-- update <packagename>\n     (updates requested package if the package requires an update)");
                    Console.WriteLine("-- list:\n     (displays list of all currently installed packages)\n");

                    Environment.Exit(0);
                }
                if (args[0] == "search")
                {
                    string searchQuery = args[1];
                    Functions.searchPkgList(searchQuery);
                    Environment.Exit(0);
                }
                if (args[0] == "list")
                {
                    //defining 'directories' variable
                    string[] directories = Directory.GetDirectories("C:/Users/" + Environment.UserName + "/HexPM");

                    //for loop that lasts the length of 'directories'
                    for (int i = 0; i < directories.Length; i++)
                    {
                        directories[i] = directories[i].Split('\\')[1];
                    }

                    //defining 'combinedDirectories'
                    string combinedDirectories = string.Join(", ", directories);

                    //writing 'combinedDirectories'
                    Console.WriteLine("\n-- Installed packages:\n     (" + combinedDirectories + ")");

                    Environment.Exit(0);
                }
                if (args[0] == "parse")
                {
                    //explaining how the command is supposed to be performed
                    Console.WriteLine("Your installer script (.is) file needs to be on your desktop. You are supposed to run the 'r' or 'runis' command followed by the installer script file name with the file extension included (Press any key to continue)");
                    Console.ReadKey(true);

                    //parsing custom written .is file
                    BetterISParser.parseIS(@"C:\Users\" + Environment.UserName + @"\Desktop\" + args[1]);

                    Environment.Exit(0);
                }
                if (args[0] == "updateall")
                {
                    //updating packagelist
                    Functions.updatePkgList();

                    //defining variables
                    string[] pkgListText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    string[] directories = Directory.GetDirectories("C:/Users/" + Environment.UserName + "/HexPM");

                    //for the length of directories...
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

                        //defining versionhistory
                        if (!File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\versionhistory.txt"))
                        {
                            Console.WriteLine("ERROR: Exception:\nVersionHistory.txt doesn't exist! Do you have any packages installed?");
                            Environment.Exit(1);
                        }
                        string[] versionHistoryText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\versionhistory.txt");

                        //for length of versionhistory...
                        for (int v = versionHistoryText.Length - 1; v > -1; v--)
                        {
                            string[] versionHistoryTextSplit = versionHistoryText[v].Split(';');

                            if (versionHistoryTextSplit[0] == directories[i].ToLower())
                            {
                                if (versionHistoryTextSplit[1] != mostRecentVersion)
                                {
                                    //updating package
                                    Functions.updatePkg(directories[i], mostRecentVersion);
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

                    Functions.silentCheckHexPMVersion();

                    Environment.Exit(0);
                }
                if (args[0] == "update")
                {
                    //updating packagelist
                    Functions.updatePkgList();

                    //defining variables
                    string[] pkgListText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\packagelist.txt");
                    if (!File.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\versionhistory.txt"))
                    {
                        Console.WriteLine("ERROR: Exception:\nVersionHistory.txt doesn't exist! Do you have any packages installed?");
                        Environment.Exit(1);
                    }
                    string[] versionHistoryText = File.ReadAllLines(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\history\versionhistory.txt");

                    //defining mostRecentVersion
                    string mostRecentVersion = "0";

                    string pkgName = "";
                    for (int i = 0; i < pkgListText.Length; i++)
                    {
                        string[] pkgListTextSplit = pkgListText[i].Split(';');

                        if (pkgListTextSplit[0].ToLower() == args[1].ToLower())
                        {
                            mostRecentVersion = pkgListTextSplit[2];
                            pkgName = pkgListTextSplit[0];
                            break;
                        }
                    }

                    //for length of versionhistory.txt...
                    for (int v = versionHistoryText.Length - 1; v > -1; v--)
                    {
                        string[] versionHistoryTextSplit = versionHistoryText[v].Split(';');

                        if (versionHistoryTextSplit[0].ToLower() == pkgName.ToLower())
                        {
                            if (versionHistoryTextSplit[1] != mostRecentVersion)
                            {
                                //updating package
                                Functions.updatePkg(pkgName, mostRecentVersion);
                                break;
                            }

                            if (versionHistoryTextSplit[1] == mostRecentVersion)
                            {
                                Console.WriteLine("-- This package is already on the latest available version");
                                Console.WriteLine("     (Cancelling update)");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //if the input doesn't match any commands
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