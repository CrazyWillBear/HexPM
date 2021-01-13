using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HexPM_Updater
{
    class Updater
    {
        static void Main(string[] args)
        {
            File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.exe");
            File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\FuzzySharp.dll");
            var client = new WebClient();
            client.DownloadFile("https://hexpm-installer-script-mirrors.crazywillbear.repl.co/HexPM.zip", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.zip");
            Thread.Sleep(500);
            ZipFile.ExtractToDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.zip", @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM");
            File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\HexPM\HexPM.zip");
        }
    }
}
