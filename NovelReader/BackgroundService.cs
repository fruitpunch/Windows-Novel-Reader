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
        public volatile NovelSourceController novelSourceController;

        private volatile ManualResetEvent mre;

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
            if(this.novelReaderForm != null)
                this.novelReaderForm.Close();
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

        public Novel AddNovel(string novelTitle, out string message)
        {
            return NovelLibrary.Instance.AddNovel(novelTitle, out message);
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

            bool newUpdate = false;
            foreach(Novel updateNovel in updateNovels)
            {
                bool result = UpdateNovel(updateNovel);
                if (result)
                    newUpdate = true;
            }
            Configuration.Instance.LastFullUpdateTime = DateTime.Now;
            if (newUpdate)
                mre.Set();
        }

        private bool UpdateNovel(Object input)
        {
            if (!(input is Novel))
                return false;
            Novel updateNovel = input as Novel;
            if (updateNovel == null || updateNovel.UpdateState == Novel.UpdateStates.Checking || updateNovel.UpdateState == Novel.UpdateStates.Fetching || updateNovel.UpdateState == Novel.UpdateStates.Syncing)
                return false;
            updateNovel.SyncToOrigin();
            bool result = updateNovel.CheckForUpdate();
            
            if (!result)
                return false;
            

            var chapters = updateNovel.NovelChapters;
            List<Chapter> downloadChapters = new List<Chapter>();
            foreach (Chapter c in chapters)
                if (!c.HasText)
                    downloadChapters.Add(c);

            int downloadCount = 0;
            bool success;
            for (int i = 0; i < downloadChapters.Count && !shutDown && updateNovel != null; i++)
            {
                updateNovel.SetUpdateProgress(i+1, downloadChapters.Count, Novel.UpdateStates.Fetching);
                success = updateNovel.DownloadChapter(downloadChapters[i]);
                if (success)
                    downloadCount++;
            }
            if (updateNovel != null)
            {
                updateNovel.ChaptersNotReadCount = updateNovel.ChaptersNotReadCount + downloadCount;
                updateNovel.SetUpdateProgress(0, 0, Novel.UpdateStates.UpToDate);
                updateNovel.NotifyPropertyChanged("ChapterCountStatus");
                return true;
            }
            else
            {
                return false;
            }
                
        }


    }
}
