using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BackgroundChanger
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(UAction uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        static void Main(string[] args)
        {
            GetBackgroud();
            SetBackgroud();
        }
        public enum UAction
        {
            SPI_SETDESKWALLPAPER = 0x0014,
            SPI_GETDESKWALLPAPER = 0x0073,
        }
        public static string GetBackgroud()
        {
            StringBuilder s = new StringBuilder(300);
            SystemParametersInfo(UAction.SPI_GETDESKWALLPAPER, 300, s, 0);
            return s.ToString();
        }
        public static int SetBackgroud()
        {
            int result = 0;
            string url = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "backgrounds");
            if (Directory.Exists(url))
            {
                Console.WriteLine("");
                for (int i = 0; i <= 100; i++)
                {
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    Console.WriteLine("Changing background: " + i + "%");
                    Thread.Sleep(150);
                }
                string[] images = Directory.GetFiles(url, "*.jpg")
                                     .Select(Path.GetFileName)
                                     .ToArray();
                Random r = new Random();
                int rand = r.Next(0, images.Count());
                StringBuilder s = new StringBuilder(Path.Combine(url,images[rand]));
                result = SystemParametersInfo(UAction.SPI_SETDESKWALLPAPER, 0, s, 0x2);
            }
            else
            {
                Directory.CreateDirectory(url);
                Console.WriteLine("Directory: " + url + "\nPress Enter to exit...");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            }
            return result;
        }
    }
}

