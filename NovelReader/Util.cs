using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelReader
{
    class Util
    {
        private static readonly Regex ValidFileRegex = new Regex("[\\/:*?\"<>|]");
        public static readonly Random random = new Random();

        public static void LoadComponents()
        {
            Configuration.LoadConfiguration();
            NovelLibrary.Instance.LoadNovelLibrary();
            BackgroundService.Instance.StartService();
            
        }

        public static void SaveComponents()
        {
            
            BackgroundService.Instance.CloseService();
            NovelLibrary.Instance.SaveNovelLibrary();
            NovelLibrary.Instance.CloseNovelLibrary();
            Configuration.SaveConfiguration();
        }

        public static string CleanFileTitle(string input)
        {
            return ValidFileRegex.Replace(input, "");
        }

        public static string GetUpdateTimeString(DateTime dt)
        {
            string str = "Last Updated: ";

            TimeSpan diff = DateTime.Now.Subtract(dt);

            int dayAgo = diff.Days;
            int hourAgo = diff.Hours;
            int minuteAgo = diff.Minutes;
            int secondAgo = diff.Seconds;

            if (dayAgo > 0)
                if (dayAgo > 0)
                    str += dayAgo + " days ago";
                else
                    str += dayAgo + " day ago";
            else if (hourAgo > 0)
                if (hourAgo > 1)
                    str += hourAgo + " hours ago";
                else
                    str += hourAgo + " hour ago";
            else if (minuteAgo > 0)
                if (minuteAgo > 1)
                    str += minuteAgo + " minutes ago";
                else
                    str += minuteAgo + " minute ago";
            else
                str += secondAgo + " seconds ago";

            return str;
        }

    }
}
