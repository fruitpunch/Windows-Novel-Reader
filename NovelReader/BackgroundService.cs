using Source;
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
        private HashSet<Chapter> requestSet;

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
            this.requestSet = new HashSet<Chapter>();
            this.ttsScheduler = new Scheduler(Configuration.Instance.TTSThreadCount);
            this.ttsScheduler.ttsCompleteEventHandler += TTSProgress;
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

        private void TTSProgress(Object sender, TTSProgressEventArgs e)
        {
            Console.WriteLine(e.request.ChapterTitle + " complete.");
        }

        /*============Public Function=======*/

        public Tuple<bool, string> AddNovel(string novelTitle, SourceLocation source, int sourceID)
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
            ScheduleTTS();
            
        }

        private void ScheduleTTS()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();
            int[] pos = new int[updateNovels.Length];
            while (true)
            {
                for (int i = 0; i < updateNovels.Length; i++)
                {
                    Console.WriteLine("add request");
                    Request r = new Request("VW Hui", updateNovels[i].Chapters[pos[i]], updateNovels[i].GetReplaceSpecificationLocation(), updateNovels[i].GetDeleteSpecificationLocation(), 3, updateNovels.Length-i);
                    pos[i]++;
                    ttsScheduler.AddRequest(r);
                    Thread.Sleep(5000);
                }
            }
        }

        private bool CheckUpdates()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();
            bool newChapterAvailable = false;
            foreach (Novel n in updateNovels)
            {
                n.CheckForUpdate();
            }
            return newChapterAvailable;
        }

        private void DownloadUpdates()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();
            foreach (Novel n in updateNovels)
            {
                if(n.UpdateState == Novel.UpdateStates.UpdateAvailable)
                    n.DownloadUpdate();
            }

            Configuration.Instance.LastFullUpdateTime = DateTime.Now;
        }


    }
}
