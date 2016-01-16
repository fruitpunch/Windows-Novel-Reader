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
            this.updateTimer = new System.Timers.Timer(Configuration.Instance.UpdateInterval);
            this.updateTimer.Enabled = true;
            this.updateTimer.Elapsed += new ElapsedEventHandler(OnUpdateTimer);
            //this.updateTimer.Interval = Configuration.Instance.UpdateInterval;

            Console.WriteLine("update interval " + Configuration.Instance.UpdateInterval); 
            
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
            //Console.WriteLine("Call update");

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

        public void UpdateTTSTest()
        {
            Thread t = new Thread(Test);
            t.Start();
        }

        public void UpdateTimerInterval(int minutes)
        {
            int ms = minutes * 60000; //60000ms per min
            Configuration.Instance.UpdateInterval = ms;
            updateTimer.Interval = ms;
            this.updateTimer.Start();
            Console.WriteLine(ms);
        }

        /*============Private Function======*/

        private void Test()
        {
            CheckUpdates();
            DownloadUpdates();
            /*
            for (int i = 0; i < n.Chapters.Count; i++)
            {
                Chapter c = n.Chapters[i];
                Request r = new Request("VW Hui", c, n.GetReplaceSpecificationLocation(), n.GetDeleteSpecificationLocation(), 2, 0);
                ttsScheduler.AddRequest(r);
            }*/
        }

        private void TTSComplete(Object sender, TTSCompleteEventArgs e)
        {
            Console.WriteLine("TTS completed event");
        }

        private bool CheckUpdates()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();
            bool newChapterAvailable = false;
            foreach (Novel n in updateNovels)
            {
                n.CheckForUpdate();
                if (n.NewChapterCount > 0)
                    newChapterAvailable = true;
            }
            return newChapterAvailable;
        }

        private void DownloadUpdates()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();
            foreach (Novel n in updateNovels)
            {
                n.DownloadUpdate();
            }

            Configuration.Instance.LastFullUpdateTime = DateTime.Now;
        }


    }
}
