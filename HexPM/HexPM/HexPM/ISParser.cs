using IWshRuntimeLibrary;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace HexPM
{
    internal class ISParser
    {
        private Program program = new Program();
        private static bool downloadComplete = false;

        public static void IcoLessCreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string Desc)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = Desc;   // The description of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        public static void IcoFullCreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string Desc, string IcoPath)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = Desc;   // The description of the shortcut
            shortcut.IconLocation = IcoPath;           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        public static void ParseFile(string filename)
        {
            downloadComplete = false;
            string[] text = System.IO.File.ReadAllLines(filename);
            string installDir = "";
            string installLink = "";
            string targetZip = "";
            string msg = "";
            string packageName = "";
            string shortPath = "";
            string shortIcoPath = "";
            string shortDesc = "";
            for (int i = 0; i < text.Length; i++)
            {
                string[] textSplit = text[i].Split('=');
                if (textSplit[0] == "dir")
                {
                    installDir = textSplit[1];
                }
                if (textSplit[0] == "inst")
                {
                    installLink = textSplit[1];
                }
                if (textSplit[0] == "zip")
                {
                    targetZip = textSplit[1];
                }
                if (textSplit[0] == "msg")
                {
                    msg = textSplit[1];
                }
                if (textSplit[0] == "name")
                {
                    packageName = textSplit[1];
                }
                if (textSplit[0] == "shortpath")
                {
                    shortPath = textSplit[1];
                }
                if (textSplit[0] == "icopath")
                {
                    shortIcoPath = textSplit[1];
                }
                if (textSplit[0] == "shortdesc")
                {
                    shortDesc = textSplit[1];
                }
            }
            string[] installDirSplit = installDir.Split('/');
            string[] targetZipSplit = targetZip.Split('/');
            string[] shortPathSplit = shortPath.Split('/');
            for (int i = 0; i < installDirSplit.Length; i++)
            {
                if (installDirSplit[i] == "Username")
                {
                    installDirSplit[i] = Environment.UserName;
                }
                installDirSplit[i] = installDirSplit[i] + "/";
            }
            for (int i = 0; i < targetZipSplit.Length; i++)
            {
                if (targetZipSplit[i] == "Username")
                {
                    targetZipSplit[i] = Environment.UserName;
                }
            }
            for (int i = 0; i < shortPathSplit.Length; i++)
            {
                if (shortPathSplit[i] == "Username")
                {
                    shortPathSplit[i] = Environment.UserName;
                }
                shortPathSplit[i] = shortPathSplit[i] + "/";
            }

            installDir = string.Join("", installDirSplit);
            targetZip = string.Join("", targetZipSplit);
            shortPath = string.Join("", shortPathSplit);
            if (Directory.Exists(installDir))
            {
                Console.WriteLine("App is already installed! (Press any key to continue)");
                Console.ReadKey(true);
            }
            else if (System.IO.File.Exists(packageName + ".zip"))
            {
                System.IO.File.Delete(packageName + ".zip");
                Console.WriteLine("An error has occured, please press any button to continue, then run the command again");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine(msg + " (Press any key to continue)");
                Console.ReadKey(true);
                System.IO.Directory.CreateDirectory(installDir);
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileAsync(new System.Uri(installLink), packageName + ".zip");
                }
                while (downloadComplete == false)
                {
                    Console.ReadKey(true);
                }
                Console.WriteLine("Continuing installation...");
                ZipFile.ExtractToDirectory(packageName + ".zip", installDir);
                System.IO.File.Delete(packageName + ".zip");
                Console.WriteLine("Saving in install history...");
                DateTime now = DateTime.Now;
                System.IO.File.AppendAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\installhistory.txt", "\n" + packageName + ";" + installDir + ";" + @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs" + @"\" + packageName + ".lnk" + ";" + now.ToString("F"));
                Console.WriteLine("Finalizing installation...");
                if (shortPath == "none/")
                {
                    Console.WriteLine("Install successful! (Press any key to continue)");
                }
                else
                {
                    if (System.IO.File.Exists(packageName + ".lnk"))
                    {
                        if (shortIcoPath == "none")
                        {
                            System.IO.File.Delete(packageName + ".lnk");
                            IcoLessCreateShortcut(packageName, @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs", shortPath, shortDesc);
                            Console.WriteLine("Install successful! (Press any key to continue)");
                        }
                        else
                        {
                            System.IO.File.Delete(packageName + ".lnk");
                            IcoFullCreateShortcut(packageName, @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs", shortPath, shortDesc, shortIcoPath);
                            Console.WriteLine("Install successful! (Press any key to continue)");
                        }
                    }
                    else
                    {
                        if (shortIcoPath == "none")
                        {
                            IcoLessCreateShortcut(packageName, @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs", shortPath, shortDesc);
                            Console.WriteLine("Install successful! (Press any key to continue)");
                        }
                        else
                        {
                            IcoFullCreateShortcut(packageName, @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs", shortPath, shortDesc, shortIcoPath);
                            Console.WriteLine("Install successful! (Press any key to continue)");
                        }
                    }
                }
            }
        }
        public static string bar = "";
        private static void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (downloadComplete == false)
            {
                if (e.ProgressPercentage == 100)
                {
                    bar = "####################";
                    downloadComplete = true;
                    Console.Write("\r" + e.ProgressPercentage + "%" + " | " + bar);
                    Console.WriteLine("\nDownloaded files! Press any key to continue installation");
                }
                if (e.ProgressPercentage != 100)
                {
                    Console.Write("\r" + e.ProgressPercentage + "%" + " | " + bar);
                    if (e.ProgressPercentage == 0)
                    {
                        bar = "[--------------------]";
                    }
                    if (e.ProgressPercentage == 5)
                    {
                        bar = "[#-------------------]";
                    }
                    if (e.ProgressPercentage == 10)
                    {
                        bar = "[##------------------]";
                    }
                    if (e.ProgressPercentage == 15)
                    {
                        bar = "[###-----------------]";
                    }
                    if (e.ProgressPercentage == 20)
                    {
                        bar = "[####----------------]";
                    }
                    if (e.ProgressPercentage == 25)
                    {
                        bar = "[#####---------------]";
                    }
                    if (e.ProgressPercentage == 30)
                    {
                        bar = "[######--------------]";
                    }
                    if (e.ProgressPercentage == 35)
                    {
                        bar = "[#######-------------]";
                    }
                    if (e.ProgressPercentage == 40)
                    {
                        bar = "[########------------]";
                    }
                    if (e.ProgressPercentage == 45)
                    {
                        bar = "[#########-----------]";
                    }
                    if (e.ProgressPercentage == 50)
                    {
                        bar = "[##########----------]";
                    }
                    if (e.ProgressPercentage == 55)
                    {
                        bar = "[###########---------]";
                    }
                    if (e.ProgressPercentage == 60)
                    {
                        bar = "[############--------]";
                    }
                    if (e.ProgressPercentage == 65)
                    {
                        bar = "[#############-------]";
                    }
                    if (e.ProgressPercentage == 70)
                    {
                        bar = "[##############------]";
                    }
                    if (e.ProgressPercentage == 75)
                    {
                        bar = "[###############-----]";
                    }
                    if (e.ProgressPercentage == 80)
                    {
                        bar = "[################----]";
                    }
                    if (e.ProgressPercentage == 85)
                    {
                        bar = "[#################---]";
                    }
                    if (e.ProgressPercentage == 90)
                    {
                        bar = "[##################--]";
                    }
                    if (e.ProgressPercentage == 95)
                    {
                        bar = "[###################-]";
                    }
                }
            }
        }
    }
}