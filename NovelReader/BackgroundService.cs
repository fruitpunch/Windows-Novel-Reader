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
        private Thread scheduleTTSThread;
        private Thread updateThread;
        private System.Timers.Timer updateTimer;
        public TTSScheduler ttsScheduler;

        private volatile bool shutDown;
        private volatile bool hasTTSShutDown;
        private volatile bool hasUpdateShutDown;

        public volatile TTSController ttsController;
        public volatile NovelListController novelListController;
        public volatile NovelReaderForm novelReaderForm;

        private volatile ManualResetEvent mre;

        public static Chapter lastUpdatedChapter = null;

        /*============Properties============*/

        public static BackgroundService Instance
        {
            get
            {
                if (instance == null)
                {
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
            this.hasTTSShutDown = true;
            this.hasUpdateShutDown = true;
            this.updateThread = new Thread(Update);
            this.ttsScheduler = new TTSScheduler(Configuration.Instance.TTSThreadCount);
            this.ttsScheduler.ttsProgressEventHandler += TTSProgress;
            this.updateTimer = new System.Timers.Timer(Configuration.Instance.UpdateInterval);
            this.updateTimer.Enabled = true;
            this.updateTimer.Elapsed += new ElapsedEventHandler(OnUpdateTimer);
            
            this.scheduleTTSThread = new Thread(ScheduleTTS);
            this.mre = new ManualResetEvent(false);
            
            this.updateTimer.Start();
            this.scheduleTTSThread.Start();
            this.ttsScheduler.StartTTSService();
            this.updateThread.Start();
        }

        public void CloseService()
        {
            //this.scheduleTTSThread.
            mre.Set();
            this.shutDown = true;
            this.updateTimer.Stop();
            this.updateTimer = null;
            //this.scheduleTTSThread = null;
            this.ttsScheduler.ShutdownService();
            while (!hasTTSShutDown || !hasUpdateShutDown)
            {

            }
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
            if(e.type == TTSProgressEventArgs.ProgressType.RequestComplete)
                NovelLibrary.Instance.GetNovel(e.request.Chapter.NovelTitle).FinishRequest(e.request.Chapter);
            else if(e.type == TTSProgressEventArgs.ProgressType.RequestRemoved)
                NovelLibrary.Instance.GetNovel(e.request.Chapter.NovelTitle).FinishRequest(e.request.Chapter);
        }

        /*============Public Function=======*/

        public Tuple<bool, string> AddNovel(string novelTitle, NovelSource novelSource)
        {
            return NovelLibrary.Instance.AddNovel(novelTitle, novelSource);
        }

        public void DeleteNovel(string novelTitle, bool deleteData)
        {
            NovelLibrary.Instance.DeleteNovel(novelTitle, deleteData);
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
            mre.Set();
        }

        /*============Private Function======*/

        private void Update()
        {
            hasUpdateShutDown = false;
            DoUpdate();
            NovelLibrary.Instance.SaveNovelLibrary();
            hasUpdateShutDown = true;
        }

        private void ScheduleTTS()
        {
            hasTTSShutDown = false;
            int roundRobin = 0;
            Dictionary<string, int> position = new Dictionary<string, int>();
            int idleCounter = 0;
            while (!shutDown)
            {
                mre.Reset();
                if (ttsScheduler.Threads > 0 && NovelLibrary.Instance.GetNovelCount() > 0)
                {
                    Novel n = NovelLibrary.Instance.NovelList[roundRobin % NovelLibrary.Instance.GetNovelCount()];
                    roundRobin++;
                    Request request = n.GetTTSRequest(Configuration.Instance.TTSSpeed);
                    if (request == null)
                    {
                        //If no new chapter to synthesize, then sleep for 15 seconds before checking.
                        idleCounter++;
                        if (idleCounter >= NovelLibrary.Instance.GetNovelCount())
                        {
                            idleCounter = 0;
                            mre.WaitOne(15000);
                        }
                        continue;
                    }
                    ttsScheduler.AddRequest(request);
                    idleCounter = 0;
                    mre.WaitOne(500 * ttsScheduler.RequestCount / ttsScheduler.Threads);
                }
                else
                {
                    mre.WaitOne(15000);
                }
            }
            hasTTSShutDown = true;
        }

        private void DoUpdate()
        {
            Novel[] updateNovels = NovelLibrary.Instance.GetUpdatingNovel();
            bool[] results = new bool[updateNovels.Length];

            for (int i = 0; i < updateNovels.Length && !shutDown; i++)
            {
                if (updateNovels[i] == null)
                    continue;
                results[i] = updateNovels[i].CheckForUpdate();
                NovelLibrary.Instance.db.Store(updateNovels[i]);
            }
            NovelLibrary.Instance.db.Commit();
            Configuration.Instance.LastFullUpdateTime = DateTime.Now;
            bool newUpdate = false;
            for (int i = 0; i < updateNovels.Length && !shutDown; i++)
            {
                if (updateNovels[i] == null || results[i])
                    continue;

                Chapter[] downloadChapters = (from chapter in NovelLibrary.libraryData.Chapters
                              where chapter.NovelTitle == updateNovels[i].NovelTitle && !chapter.HasText
                              orderby chapter.Index ascending
                              select chapter).ToArray<Chapter>();

                int failure = 0;
                bool success;
                for(int j = 0; j < downloadChapters.Length && !shutDown; j++)
                {
                    success = updateNovels[i].DownloadChapter(downloadChapters[j], j+1, downloadChapters.Length);
                    if (!success)
                        failure++;
                }
                if (updateNovels[i] == null)
                    continue;
                if (downloadChapters.Length > 0)
                    newUpdate = true;
                updateNovels[i].NewChaptersNotReadCount = updateNovels[i].NewChaptersNotReadCount + downloadChapters.Length - failure;
                updateNovels[i].SetUpdateProgress(0, 0, Novel.UpdateStates.UpToDate);
                updateNovels[i].ClearChapters();
            }
            if (newUpdate)
                mre.Set();
        }


    }
}
