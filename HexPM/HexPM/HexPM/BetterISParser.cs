using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace HexPM
{
    internal class BetterISParser
    {
        private Program program = new Program();

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string Desc)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = Desc;   // The description of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        private static bool restrictInput = true;
        public static string bar = "";

        private static void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (restrictInput == true)
            {
                if (e.ProgressPercentage == 100)
                {
                    bar = "####################";
                    restrictInput = false;
                    Console.Write("\r     (" + e.ProgressPercentage + " % | " + bar + ")\n");
                    Console.WriteLine("     (Downloaded files)");
                }
                if (e.ProgressPercentage != 100)
                {
                    Console.Write("\r     (" + e.ProgressPercentage + " % | " + bar + ")");
                    if (e.ProgressPercentage == 0)
                    {
                        bar = "--------------------";
                    }
                    if (e.ProgressPercentage == 5)
                    {
                        bar = "#>------------------";
                    }
                    if (e.ProgressPercentage == 10)
                    {
                        bar = "##>-----------------";
                    }
                    if (e.ProgressPercentage == 15)
                    {
                        bar = "###>----------------";
                    }
                    if (e.ProgressPercentage == 20)
                    {
                        bar = "####>---------------";
                    }
                    if (e.ProgressPercentage == 25)
                    {
                        bar = "#####>--------------";
                    }
                    if (e.ProgressPercentage == 30)
                    {
                        bar = "######>-------------";
                    }
                    if (e.ProgressPercentage == 35)
                    {
                        bar = "#######>------------";
                    }
                    if (e.ProgressPercentage == 40)
                    {
                        bar = "########>-----------";
                    }
                    if (e.ProgressPercentage == 45)
                    {
                        bar = "#########>----------";
                    }
                    if (e.ProgressPercentage == 50)
                    {
                        bar = "##########>---------";
                    }
                    if (e.ProgressPercentage == 55)
                    {
                        bar = "###########>--------";
                    }
                    if (e.ProgressPercentage == 60)
                    {
                        bar = "############>-------";
                    }
                    if (e.ProgressPercentage == 65)
                    {
                        bar = "#############>------";
                    }
                    if (e.ProgressPercentage == 70)
                    {
                        bar = "##############>-----";
                    }
                    if (e.ProgressPercentage == 75)
                    {
                        bar = "###############>----";
                    }
                    if (e.ProgressPercentage == 80)
                    {
                        bar = "################>---";
                    }
                    if (e.ProgressPercentage == 85)
                    {
                        bar = "#################>--";
                    }
                    if (e.ProgressPercentage == 90)
                    {
                        bar = "##################>-";
                    }
                    if (e.ProgressPercentage == 95)
                    {
                        bar = "###################>";
                    }
                }
            }
        }
        public static void parseIS(string fileName)
        {
            string[] text = System.IO.File.ReadAllLines(fileName);
            for (int i = 0; i < text.Length; i++)
            {
                string[] textSplit = text[i].Split('|');
                if (textSplit[0] == "msg")
                {
                    Console.WriteLine(textSplit[1] + " (Press any key to continue)\n");
                    Console.ReadKey(true);
                }
                if (textSplit[0] == "down")
                {
                    string[] downSplit = textSplit[1].Split(',');
                    Console.WriteLine("-- Downloading files...");
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                        wc.DownloadFileAsync(new System.Uri(downSplit[0]), @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\" + downSplit[1]);
                    }
                    restrictInput = true;
                    while (restrictInput)
                    {
                        if (restrictInput == true) { }
                        else
                        {
                            restrictInput = false;
                            break;
                        }
                    }
                }
                if (textSplit[0] == "createdir")
                {
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    Directory.CreateDirectory(dir);
                }
                if (textSplit[0] == "unzip")
                {
                    Console.WriteLine("-- Extracting files...");
                    string[] unzipSplit = textSplit[1].Split(',');
                    string[] dirSplit = unzipSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    ZipFile.ExtractToDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\" + unzipSplit[0], dir);
                    Console.WriteLine("     (Extracted files)");
                }
                if (textSplit[0] == "delfile")
                {
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    System.IO.File.Delete(dir);
                }
                if (textSplit[0] == "deldir")
                {
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    if (Directory.Exists(dir))
                    {
                        Directory.Delete(dir, true);
                    }
                }
                if (textSplit[0] == "cpathvar")
                {
                    string[] currentValueArray = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
                    var currentValue = string.Join(";", currentValueArray); ;
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    var newValue = currentValue + ";" + dir;
                    Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
                }
                if (textSplit[0] == "dpathvar")
                {
                    var currentUninstallValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
                    List<string> newValueUninstall = new List<string>();
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
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
                }
                if (textSplit[0] == "saveinstall")
                {
                    string[] saveInstallSplit = textSplit[1].Split(',');
                    string[] dirSplit = saveInstallSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    DateTime now = DateTime.Now;
                    if (saveInstallSplit[2].Contains("True"))
                    {
                        System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt", "\n" + saveInstallSplit[0] + ";" + dir + ";" + @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs" + @"\" + saveInstallSplit[0] + ".lnk" + ";" + now.ToString("F") + ";" + saveInstallSplit[2]);
                    }
                    else
                    {
                        System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt", "\n" + saveInstallSplit[0] + ";" + dir + ";" + @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs" + @"\" + saveInstallSplit[0] + ".lnk" + ";" + now.ToString("F") + ";EnvVarFalse");
                    }
                }
                if (textSplit[0] == "createshortcut")
                {
                    string[] shortcutSplit = textSplit[1].Split(',');
                    System.IO.File.Delete(shortcutSplit[0] + ".lnk");
                    string[] dirSplit = shortcutSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    CreateShortcut(shortcutSplit[0], @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs", dir, shortcutSplit[2]);
                }
                if (textSplit[0] == "createdeskshortcut")
                {
                    string[] shortcutSplit = textSplit[1].Split(',');
                    System.IO.File.Delete(shortcutSplit[0] + ".lnk");
                    string[] dirSplit = shortcutSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    CreateShortcut(shortcutSplit[0], @"C:\Users\" + Environment.UserName + @"\Desktop", dir, shortcutSplit[2]);
                }
                if (textSplit[0] == "complete")
                {
                    Console.WriteLine("-- Installation complete. (Press any key to continue)");
                }
            }
        }

        public static void silentParseIS(string fileName)
        {
            string[] text = System.IO.File.ReadAllLines(fileName);
            for (int i = 0; i < text.Length; i++)
            {
                string[] textSplit = text[i].Split('|');
                if (textSplit[0] == "down")
                {
                    string[] downSplit = textSplit[1].Split(',');
                    var client = new WebClient();
                    client.DownloadFile(downSplit[0], @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\" + downSplit[1]);
                }
                if (textSplit[0] == "createdir")
                {
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    Directory.CreateDirectory(dir);
                }
                if (textSplit[0] == "unzip")
                {
                    string[] unzipSplit = textSplit[1].Split(',');
                    string[] dirSplit = unzipSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    ZipFile.ExtractToDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\" + unzipSplit[0], dir);
                }
                if (textSplit[0] == "delfile")
                {
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    System.IO.File.Delete(dir);
                }
                if (textSplit[0] == "deldir")
                {
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    if (Directory.Exists(dir))
                    {
                        Directory.Delete(dir, true);
                    }
                }
                if (textSplit[0] == "dpathvar")
                {
                    var currentUninstallValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
                    List<string> newValueUninstall = new List<string>();
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
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
                }
                if (textSplit[0] == "cpathvar")
                {
                    string[] currentValueArray = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User).Split(';');
                    var currentValue = string.Join(";", currentValueArray); ;
                    string[] dirSplit = textSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                        dirSplit[d] = dirSplit[d] + "\\";
                    }
                    string dir = string.Join("", dirSplit);
                    var newValue = currentValue + ";" + dir;
                    Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
                }
                if (textSplit[0] == "saveinstall")
                {
                    string[] saveInstallSplit = textSplit[1].Split(',');
                    string[] dirSplit = saveInstallSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    DateTime now = DateTime.Now;
                    if (saveInstallSplit[2].Contains("True"))
                    {
                        System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt", "\n" + saveInstallSplit[0] + ";" + dir + ";" + @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs" + @"\" + saveInstallSplit[0] + ".lnk" + ";" + now.ToString("F") + ";" + saveInstallSplit[2]);
                    }
                    else
                    {
                        System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt", "\n" + saveInstallSplit[0] + ";" + dir + ";" + @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs" + @"\" + saveInstallSplit[0] + ".lnk" + ";" + now.ToString("F") + ";EnvVarFalse");
                    }
                }
                if (textSplit[0] == "createshortcut")
                {
                    string[] shortcutSplit = textSplit[1].Split(',');
                    System.IO.File.Delete(shortcutSplit[0] + ".lnk");
                    string[] dirSplit = shortcutSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    CreateShortcut(shortcutSplit[0], @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs", dir, shortcutSplit[2]);
                }
                if (textSplit[0] == "createdeskshortcut")
                {
                    string[] shortcutSplit = textSplit[1].Split(',');
                    System.IO.File.Delete(shortcutSplit[0] + ".lnk");
                    string[] dirSplit = shortcutSplit[1].Split('\\');
                    for (int d = 0; d < dirSplit.Length; d++)
                    {
                        if (dirSplit[d] == "Username")
                        {
                            dirSplit[d] = Environment.UserName;
                        }
                    }
                    string dir = string.Join("\\", dirSplit);
                    CreateShortcut(shortcutSplit[0], @"C:\Users\" + Environment.UserName + @"\Desktop", dir, shortcutSplit[2]);
                }
                if (textSplit[0] == "complete") { }
            }
        }
    }
}