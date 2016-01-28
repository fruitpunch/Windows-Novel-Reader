using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Speech.Synthesis;
//using Microsoft.Speech.Synthesis;

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
            Configuration.SaveConfiguration();
            NovelLibrary.Instance.CloseNovelLibrary();
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

        public static Tuple<List<string>, List<string>> GetLanguageVoice()
        {
            //Microsoft.Speech.Synthesis.SpeechSynthesizer msftSynth = new Microsoft.Speech.Synthesis.SpeechSynthesizer();
            System.Speech.Synthesis.SpeechSynthesizer sysSynth = new System.Speech.Synthesis.SpeechSynthesizer();
            List<string> languageList = new List<string>();
            List<string> voiceList = new List<string>();
            /*
            foreach (Microsoft.Speech.Synthesis.InstalledVoice voice in msftSynth.GetInstalledVoices())
            {
                Microsoft.Speech.Synthesis.VoiceInfo info = voice.VoiceInfo;
                Console.WriteLine("===================================");
                Console.WriteLine(" Name:          " + info.Name);
                Console.WriteLine(" Culture:       " + info.Culture);
                Console.WriteLine(" ID:            " + info.Id);
                if (!languageList.Contains(info.Culture.ToString()))
                    languageList.Add(info.Culture.ToString());
                if (!voiceList.Contains(info.Name))
                    voiceList.Add(info.Name);
            }
             * */
            foreach (System.Speech.Synthesis.InstalledVoice voice in sysSynth.GetInstalledVoices())
            {
                System.Speech.Synthesis.VoiceInfo info = voice.VoiceInfo;
                //Console.WriteLine("===================================");
                //Console.WriteLine(" Name:          " + info.Name);
                //Console.WriteLine(" Culture:       " + info.Culture);
               // Console.WriteLine(" ID:            " + info.Id);
                if (!languageList.Contains(info.Culture.ToString()))
                    languageList.Add(info.Culture.ToString());
                if (!voiceList.Contains(info.Name))
                    voiceList.Add(info.Name);
            }

            return new Tuple<List<string>, List<string>>(languageList, voiceList);
        }

    }
}
