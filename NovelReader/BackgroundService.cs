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
        private Thread scheduleTTSThread;
        private Thread updateThread;
        private System.Timers.Timer updateTimer;
        public Scheduler ttsScheduler;

        private bool shutDown;

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
            this.shutDown = false;
            this.updateThread = new Thread(Update);
            this.workerThread = new Thread(DoWork);
            this.ttsScheduler = new Scheduler(Configuration.Instance.TTSThreadCount);
            this.ttsScheduler.ttsCompleteEventHandler += TTSProgress;
            this.updateTimer = new System.Timers.Timer(Configuration.Instance.UpdateInterval);
            this.updateTimer.Enabled = true;
            this.updateTimer.Elapsed += new ElapsedEventHandler(OnUpdateTimer);
            
            this.scheduleTTSThread = new Thread(ScheduleTTS);
            //this.updateTimer.Interval = Configuration.Instance.UpdateInterval;

            Console.WriteLine("update interval " + Configuration.Instance.UpdateInterval); 
            
            this.updateTimer.Start();
            this.scheduleTTSThread.Start();
            this.workerThread.Start();
            this.ttsScheduler.StartTTSService();
            this.updateThread.Start();
        }

        public void CloseService()
        {
            //this.scheduleTTSThread.
            this.shutDown = true;
            this.updateTimer.Stop();
            this.updateTimer = null;
            this.workerThread = null;
            //this.scheduleTTSThread = null;
            this.ttsScheduler.ShutdownService();
        }

        /*============Getter/Setter=========*/
        /*============EventHandler==========*/

        private void OnUpdateTimer(Object source, ElapsedEventArgs e)
        {
            if (!updateThread.IsAlive)
            {
                updateThread = new Thread(Update);
                updateThread.Start();
            }
        }

        private void TTSProgress(Object sender, TTSProgressEventArgs e)
        {
            //Console.WriteLine(e.request.ChapterTitle + " complete.");
            NovelLibrary.Instance.GetNovel(e.request.Chapter.NovelTitle).FinishRequest(e.request.Chapter);
        }

        /*============Public Function=======*/

        public Tuple<bool, string> AddNovel(string novelTitle, SourceLocation source, string sourceID)
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
            if (!updateThread.IsAlive)
            {
                updateThread = new Thread(Update);
                updateThread.Start();
            }
        }

        public void UpdateTimerInterval(int minutes)
        {
            int ms = minutes * 60000; //60000ms per min
            Configuration.Instance.UpdateInterval = ms;
            updateTimer.Interval = ms;
            this.updateTimer.Start();
            Console.WriteLine(ms);
        }

        public void ResetTTSList()
        {
            ttsScheduler.ClearRequests();
            foreach (Novel n in NovelLibrary.Instance.NovelList)
            {
                n.ResetTTSRequest();
            }
        }

        /*============Private Function======*/

        private void DoWork()
        {
            while (!shutDown)
            {

                Thread.Sleep(1000);
            }
        }

        private void Update()
        {
            CheckUpdates();
            DownloadUpdates();
            //NovelLibrary.Instance.SaveNovelLibrary();
        }

        private void ScheduleTTS()
        {
            int roundRobin = 0;
            Dictionary<string, int> position = new Dictionary<string, int>();
            while (!shutDown)
            {
                if (NovelLibrary.Instance.GetNovelCount() > 0 && ttsScheduler.Threads > 0)
                {
                    Novel n = NovelLibrary.Instance.NovelList[roundRobin % NovelLibrary.Instance.GetNovelCount()];
                    roundRobin++;
                    Request request = n.GetTTSRequest(Configuration.Instance.TTSSpeed);
                    if (request == null)
                        continue;
                    ttsScheduler.AddRequest(request);
                    Thread.Sleep(500 * ttsScheduler.RequestCount / ttsScheduler.Threads);
                }
                else
                {
                    Thread.Sleep(1000);
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
                NovelLibrary.Instance.db.Store(n);
            }
            NovelLibrary.Instance.db.Commit();
            return newChapterAvailable;
        }

        private void DownloadUpdates()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();
            foreach (Novel n in updateNovels)
            {
                if (n.UpdateState == Novel.UpdateStates.UpdateAvailable)
                {
                    n.DownloadUpdate();
                    NovelLibrary.Instance.db.Store(n);
                }
            }
            NovelLibrary.Instance.db.Commit();
            Configuration.Instance.LastFullUpdateTime = DateTime.Now;
        }


    }
}
