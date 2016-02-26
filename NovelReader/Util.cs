using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Source;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Timers;
//using Microsoft.Speech.Synthesis;

namespace NovelReader
{
    class Util
    {
        private static readonly Regex ValidFileRegex = new Regex("[\\/:*?\"<>|]");
        public static readonly Random random = new Random();

        public static void LoadComponents()
        {
            SourceManager.LoadSourcePack();
            
            Configuration.LoadConfiguration();
            NovelLibrary.Instance.LoadNovelLibrary();
            BackgroundService.Instance.StartService();
            CreateCache();
        }

        public static void SaveComponents()
        {
            BackgroundService.Instance.CloseService();
            Configuration.SaveConfiguration();
            NovelLibrary.Instance.CloseNovelLibrary();
            ClearCache();
        }

        public static void CreateCache()
        {
            ClearCache();
            Directory.CreateDirectory(Configuration.Instance.CacheFolderLocation);
        }

        public static void ClearCache()
        {
            if(Directory.Exists(Configuration.Instance.CacheFolderLocation))
                Directory.Delete(Configuration.Instance.CacheFolderLocation, true);
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

    public class ThreadedBindingList<T> : BindingList<T>
    {
        SynchronizationContext ctx = SynchronizationContext.Current;

        IQueryable<T> constrain;

        public ThreadedBindingList(IQueryable<T> constrain) : base()
        {
            this.constrain = constrain;
        }

        public void SyncBindingToConstrain()
        {
            base.RaiseListChangedEvents = false;
            T[] target = constrain.ToArray<T>();
            base.Clear();
            foreach (T item in target)
                base.Add(item);
            base.RaiseListChangedEvents = true;
            base.ResetBindings();

        }

        protected override void OnAddingNew(AddingNewEventArgs e)
        {

            if (ctx == null)
            {
                BaseAddingNew(e);
            }
            else
            {
                ctx.Send(delegate
                {
                    BaseAddingNew(e);
                }, null);
            }
        }
        void BaseAddingNew(AddingNewEventArgs e)
        {
            base.OnAddingNew(e);
        }


        protected override void OnListChanged(ListChangedEventArgs e)
        {
            
            if (ctx == null)
            {
                BaseListChanged(e);
            }
            else
            {
                ctx.Send(delegate
                {
                    BaseListChanged(e);
                }, null);
            }
        }

        void BaseListChanged(ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemChanged)
            {
                T changedItem = base[e.NewIndex];
                if (!constrain.Contains(changedItem))
                    base.Remove(changedItem);
                else
                    base.OnListChanged(e);
            }
            else
                base.OnListChanged(e);
        }
    }
}
