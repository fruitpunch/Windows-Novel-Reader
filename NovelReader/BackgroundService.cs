using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NovelReader
{
    class BackgroundService
    {

        private static BackgroundService instance;
        private Thread workerThread;
        private System.Timers.Timer updateTimer;
        public Scheduler ttsScheduler;

        public TTSController ttsController;
        public NovelListController novelListController;
        public NovelReaderForm novelReaderForm;

        /*============Properties============*/

        public static BackgroundService Instance
        {
            get
            {
                if (instance == null)
                {
                    Console.WriteLine("Create instance of PlayListScheduler");
                    instance = new BackgroundService();
                }
                return instance;
            }
        }


        /*============Constructor===========*/

        private BackgroundService()
        {
            Console.WriteLine("Backgroundservice Constructor");

        }

        public void StartService()
        {
            this.ttsScheduler = new Scheduler(Configuration.Instance.TTSThreadCount);
            this.ttsScheduler.ttsCompleteEventHandler += TTSComplete;
            this.updateTimer = new System.Timers.Timer();
            this.updateTimer.Elapsed += new ElapsedEventHandler(this.OnUpdateTimer);
            this.updateTimer.Interval = Configuration.Instance.UpdateInterval;
            this.updateTimer.Start();
            this.ttsScheduler.StartTTSService();

        }

        public void CloseService()
        {
            this.updateTimer.Stop();
            this.updateTimer = null;
            this.workerThread = null;
            this.ttsScheduler.ShutdownService();
        }

        /*============Getter/Setter=========*/
        /*============EventHandler==========*/

        private void OnUpdateTimer(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Call update");
            foreach (Novel n in NovelLibrary.Instance.NovelList)
                n.Update();
        }

        /*============Public Function=======*/

        public Tuple<bool, string> AddNovel(string novelTitle, Source.SourceManager.Sources source, int sourceID)
        {
            return NovelLibrary.Instance.AddNovel(novelTitle, source, sourceID);
        }

        public bool RankUpNovel(string novelTitle)
        {
            return NovelLibrary.Instance.RankUpNovel(novelTitle);
        }

        public bool RankDownNovel(string novelTitle)
        {
            return NovelLibrary.Instance.RankDownNovel(novelTitle);
        }

        public void UpdateTTSTest(string novelTitle)
        {
            /*
            Novel n = NovelLibrary.Instance.GetNovel(novelTitle);
            n.Update();

            for (int i = 0; i < n.Chapters.Count; i++)
            {
                Chapter c = n.Chapters[i];
                Request r = new Request("VW Hui", c, n.GetReplaceSpecificationLocation(), n.GetDeleteSpecificationLocation(), 2, 0);
                ttsScheduler.AddRequest(r);
            }*/
            Thread t = new Thread(new ParameterizedThreadStart(Test));
            t.Start(novelTitle);
        }

        

        /*============Private Function======*/

        private void Test(Object obj)
        {
            string novelTitle = (string)obj;
            Novel n = NovelLibrary.Instance.GetNovel(novelTitle);
            UpdateNovels();

            for (int i = 0; i < n.Chapters.Count; i++)
            {
                Chapter c = n.Chapters[i];
                Request r = new Request("VW Hui", c, n.GetReplaceSpecificationLocation(), n.GetDeleteSpecificationLocation(), 2, 0);
                ttsScheduler.AddRequest(r);
            }
        }

        private void TTSComplete(Object sender, TTSCompleteEventArgs e)
        {
            Console.WriteLine("TTS completed event");
        }

        private void UpdateNovels()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();

            foreach (Novel n in updateNovels)
            {
                n.Update();
            }
            Configuration.Instance.LastFullUpdateTime = DateTime.Now;
        }


    }
}
