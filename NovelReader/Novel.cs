using System;
using System.Collections.Generic;using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Source;
using System.IO;
using Db4objects.Db4o;

namespace NovelReader
{
    public partial class Novel : INotifyPropertyChanged
    {
        public enum NovelState : int { Active = 0, Inactive = 1, Completed = 2, Dropped = 3 };

        /*              Check Update    Download Update     Make Audio
         * Active       O               O                   D
         * Inactive     O               X                   D
         * Complete     X               X                   D
         * Dropped      X               X                   X
         * 
         * 
         *              X = No          O = Yes             D = If specified
         */

        public enum UpdateStates { Default, Waiting, Checking, UpdateAvailable, Fetching, UpToDate, Inactive, Completed, Dropped, Error };

        //public event PropertyChangedEventHandler PropertyChanged;

        private NovelState _state { get; set; }
        private int _chapterCount { get; set; }
        private int _newChaptersNotReadCount { get; set; }
        private int _rank { get; set; }
        private Chapter _lastViewedChapter { get; set; }
        private Chapter _lastReadChapter { get; set; }
        private bool _makeAudio { get; set; }
        private bool _isReading { get; set; }
        NovelSource _novelSource { get; set; }
        UpdateStates _updateState { get; set; }
        private Tuple<int, string> _updateProgress { get; set; }
        private int _dbRequest { get; set; }
        [Transient]
        //private volatile BindingList<Chapter> _chapters;

        private HashSet<int> validUrlIdSet;
        private HashSet<int> invalidUrlIdSet;

        //==========
        private Dictionary<Chapter, Request> queuedTTSChapters;
        private int requestIndex;
        //==========

        /*============Properties============*/

        public NovelState State
        {
            get { return (NovelState)this.StateID; }
            set {
                if (State != NovelState.Dropped && value == NovelState.Dropped)
                    NovelLibrary.Instance.DropNovel(NovelTitle);
                else if (State == NovelState.Dropped && value != NovelState.Dropped)
                    NovelLibrary.Instance.PickUpNovel(NovelTitle);
                StateID = (int)value;
                SetUpdateProgress();
                NovelLibrary.libraryData.SubmitChanges();
            }
        }

        public int ChapterCount
        {
            get {
                return NovelChapters.Count();
            }
            set
            {
                this._chapterCount = value;
                NotifyPropertyChanged("ChapterCountStatus");
            }
        }

        public int NewChaptersNotReadCount
        {
            get {
                int newChaptersNotReadCount = (from novel in NovelLibrary.libraryData.Novels
                             where novel.NovelTitle == this.NovelTitle
                             select novel.ChaptersNotReadCount).First();
                return newChaptersNotReadCount;
            }
            set
            {
                this._newChaptersNotReadCount = value;
                NotifyPropertyChanged("ChapterCountStatus");
            }
        }

        public string ChapterCountStatus
        {
            get {
                if(this._newChaptersNotReadCount > 0)
                    return this._chapterCount.ToString() + "  ( " + this._newChaptersNotReadCount + " new chapters )";
                return this._chapterCount.ToString() + "  ( " + this._newChaptersNotReadCount + " )";
            }
        }

        public Chapter LastViewedChapter
        {
            get { return this._lastViewedChapter; }
            set { this._lastViewedChapter = value; }
        }

        public Chapter LastReadChapter
        {
            get { return this._lastReadChapter; }
            set { this._lastReadChapter = value; }
        }

        public IQueryable<Chapter> NovelChapters
        {
            get {
                var novelChapter = (from chapter in NovelLibrary.libraryData.Chapters
                                     where chapter.NovelTitle == this.NovelTitle
                                     select chapter);

                var joinedResult = (from chapter in novelChapter
                                    join url in NovelLibrary.libraryData.ChapterUrls
                                    on chapter.ID equals url.ChapterID into jr
                                    from subChapterUrl in jr.DefaultIfEmpty()
                                    select new { chapter, Url = (subChapterUrl == null ? null : subChapterUrl)});

                var finalResult = (from result in joinedResult
                                   where result.Url.Valid || result.Url == null
                                   orderby result.chapter.Index ascending
                                   select result.chapter);
                
                return finalResult;
            }
        }

        public bool Reading
        {
            get { return this._isReading; }
            set { this._isReading = value; }
        }

        public NovelSource NovelSource
        {
            get { return this._novelSource; }
            set { this._novelSource = value; }
        }

        public Tuple<int, string> UpdateProgress
        {
            get { return this._updateProgress; }
            set
            {
                this._updateProgress = value;
                NotifyPropertyChanged("UpdateProgress");
            }
        }

        public UpdateStates UpdateState
        {
            get { return this._updateState; }
            set { this._updateState = value; }
        }

        public int DBRequest
        {
            get { return this._dbRequest; }
            set { this._dbRequest = value; }
        }

        /*============Constructor===========*/

        public Novel(string novelTitle, NovelSource novelSource, NovelState state = NovelState.Active, int rank = 0, bool isReading = false)
        {
            this._novelSource = novelSource;
            this._state = state;
            this._rank = rank;
            this._newChaptersNotReadCount = 0;
            this._isReading = isReading;

            //this._chapters = new BindingList<Chapter>();
            this.validUrlIdSet = new HashSet<int>();
            this.invalidUrlIdSet = new HashSet<int>();
            this._dbRequest = 0;
            this.queuedTTSChapters = new Dictionary<Chapter, Request>();
            SetUpdateProgress();

        }

        public void Init()
        {
            this._dbRequest = 0;
            this.requestIndex = 0;
            this.queuedTTSChapters = new Dictionary<Chapter, Request>();
            if (this._makeAudio)
                LoadChapterFromDB();
        }

        /*============Getter/Setter=========*/

        public string GetDeleteSpecificationLocation()
        {
            string path = System.IO.Path.Combine(Configuration.Instance.NovelFolderLocation, NovelTitle, Configuration.Instance.DeleteSpecification);
            if (File.Exists(path))
                return path;
            return null;
        }

        public string GetReplaceSpecificationLocation()
        {
            string path = System.IO.Path.Combine(Configuration.Instance.NovelFolderLocation, NovelTitle, Configuration.Instance.ReplaceSpecification);
            if (File.Exists(path))
                return path;
            return null;
        }

        /*============Public Function=======*/

        public string GetNovelDirectory()
        {
            return System.IO.Path.Combine(Configuration.Instance.NovelFolderLocation, NovelTitle);
        }

        public void LoadChapterFromDB()
        {
            /*
            _dbRequest++;
            //Console.WriteLine("Load Chapters for " + _novelTitle + " " + _dbRequest);
            if (_dbRequest > 1)
                return;

            if (_chapters == null)
                _chapters = new BindingList<Chapter>();
            var results = NovelLibrary.Instance.db.Query<Chapter>();
            var sorted = from chapters in results
                         where chapters.NovelTitle == _novelTitle
                         orderby chapters.Index
                         select chapters;
            foreach (Chapter chapter in sorted)
            {
                AppendNewChapter(chapter);
                //Console.WriteLine("added chapter " + chapter.Index);
                if (_lastViewedChapter != null && _lastViewedChapter.Index == chapter.Index)
                    _lastViewedChapter = chapter;
                if (_lastReadChapter != null && _lastReadChapter.Index == chapter.Index)
                    _lastReadChapter = chapter;
            }
            */
        }

        public void SaveChapterToDB()
        {
            if (_chapters == null)
                return;
            for (int i = 0; i < _chapters.Count; i++)
            {
                NovelLibrary.Instance.db.Store(_chapters[i]);
            }
            NovelLibrary.Instance.db.Commit();
        }

        public void DeleteChapterFromDB()
        {
            LoadChapterFromDB();
            foreach (Chapter c in _chapters)
            {
                NovelLibrary.Instance.db.Delete(c);
            }
            NovelLibrary.Instance.db.Commit();
            _dbRequest = 0;
        }

        public void ClearChapters()
        {
            _dbRequest--;
            //Console.WriteLine("Clear Chapters for " + _novelTitle + " " + _dbRequest);
            if (_dbRequest > 0)
                return;
            if (_chapters == null)
                return;
            if (_updateState != UpdateStates.Checking || _updateState != UpdateStates.Fetching)
                return;
            SaveChapterToDB();
            _chapters.Clear();
            
        }

        public Request GetTTSRequest(int speed)
        {
            /*
            double novelCount = NovelLibrary.Instance.GetNovelCount();
            double sum = (novelCount * (novelCount + 1)) / 2;
            double position = novelCount - _rank + 1;
            int maxCount = (int)Math.Ceiling(50 * position / sum) + 5;
            if (queuedTTSChapters.Count > maxCount)
                return null;
            //Console.WriteLine(NovelTitle + " mc:" + maxCount + " sum:" + sum + " pos:" + position);
            */
            if (_chapters == null)
                return null;
            Request request = null;
            Chapter c = null;
            for (requestIndex = 0; requestIndex < _chapters.Count; requestIndex++)
            {
                c = _chapters[requestIndex];
                if(ShouldMakeAudio(c, false))
                {
                    if (!queuedTTSChapters.ContainsKey(c))
                    {
                        request = new Request("VW Hui", c, GetReplaceSpecificationLocation(), GetDeleteSpecificationLocation(), speed, GetTTSPriority(c));
                        queuedTTSChapters.Add(c, request);
                        break;
                    }
                }
            }
            if (requestIndex >= _chapterCount || request == null)
            {
                requestIndex = 0;
                for (requestIndex = 0; requestIndex < _chapters.Count; requestIndex++)
                {
                    c = _chapters[requestIndex];
                    if (ShouldMakeAudio(c, true))
                    {
                        if (!queuedTTSChapters.ContainsKey(c))
                        {
                            //request = new Request(Configuration.Instance.LanguageVoiceDictionary[NovelSource.NovelLanguage], c, GetReplaceSpecificationLocation(), GetDeleteSpecificationLocation(), speed, GetTTSPriority(c));
                            request = new Request("VW Hui", c, GetReplaceSpecificationLocation(), GetDeleteSpecificationLocation(), speed, GetTTSPriority(c));
                            queuedTTSChapters.Add(c, request);
                            break;
                        }
                    }
                }
            }
            return request;
        }

        public void ResetTTSRequest()
        {
            var chapterArray = queuedTTSChapters.Keys.ToArray<Chapter>();
            foreach (Chapter c in chapterArray)
            {
                if (queuedTTSChapters[c].TakenBy == -1)
                {
                    queuedTTSChapters.Remove(c);
                }
            }
            requestIndex = 0;
        }

        public void FinishRequest(Chapter c)
        {
            queuedTTSChapters.Remove(c);
        }

        public bool ShouldMakeAudio(Chapter chapter, bool secondaryPass)
        {
            if (_chapters == null || _chapters.Count == 0)
                return false;
            if (_lastReadChapter != null && _lastReadChapter.Index > chapter.Index && !secondaryPass)
                return false;
            if (Configuration.Instance.LanguageVoiceDictionary[NovelSource.NovelLanguage].Equals("No Voice Selected"))
                return false;
            //Do not make audio for novel not selected
            if (!_makeAudio)
                return false;
            //Do not make audio for novel that is dropped
            if (_state == NovelState.Dropped)
                return false;
            //Do not make audio for novel already read and setting checked for making tts for chapter already read
            if (chapter.Read && !Configuration.Instance.MakeTTSForChapterAlreadyRead)
                return false;
            //Do make audio for chapter that has text and chapter that has no audio
            if (chapter.HasText && !chapter.HasAudio)
                return true;
            //Remake audio if text is editted after an audio file is made
            if (chapter.HasText && chapter.HasAudio)
            {
                if (File.GetLastWriteTime(chapter.GetAudioFileLocation()) < File.GetLastWriteTime(chapter.GetTextFileLocation()))
                    return true;
                else
                    return false;
            }
            return false;
        }

        public double GetTTSPriority(Chapter chapter)
        {
            double priority = 0;
            
            int lastReadChapterIndex = 0;
            //Higher rank gets higher priority
            priority += (NovelLibrary.Instance.GetNovelCount() - _rank) * 5;

            //State also changes priority
            switch (_state)
            {
                case NovelState.Active:
                    priority += 3;
                    break;
                case NovelState.Completed:
                    priority += 2;
                    break;
                case NovelState.Inactive:
                    priority += 1;
                    break;
                case NovelState.Dropped:
                    break;
            }

            /*
             * The more chapters buffered, the lower the priority. 
             */
            if (_lastReadChapter != null)
            {
                lastReadChapterIndex = _lastViewedChapter.Index;
            }
            int chapterBuffer = chapter.Index - lastReadChapterIndex;

            if (chapterBuffer > 0)
            {
                priority += 150.0f / chapterBuffer;
            }
            else if (chapterBuffer == 0)
            {
                priority += 150.0f;
            }
            else if (chapterBuffer < 0)
            {
                priority -= 100;
            }

            //Increase priority if is reading
            if (_isReading && priority > 0)
                priority *= 1.5;

            //Decrease priority if chapter has been read
            if (chapter.Read && priority > 0)
                priority /= 2;

            priority = (double)Math.Round((decimal)priority, 1);

            return priority;
        }


        //Check and see if there is new chapter available for download.
        public bool CheckForUpdate()
        {
            //LoadChapterFromDB();

            SetUpdateProgress(0, 0, UpdateStates.Checking);
            Tuple<string, string>[] menuItems = _novelSource.GetMenuURLs();
            if(menuItems == null)
            {
                SetUpdateProgress(0, 0, UpdateStates.Error);
                return false;
            }
            
            List<string> urls = (from url in NovelLibrary.libraryData.ChapterUrls
                                 select url.Url).ToList<string>();
            int maxIndex = ChapterCount;

            for (int i = 0; i < menuItems.Length; i++)
            {
                if(!urls.Contains(menuItems[i].Item2))
                {
                    Chapter newChapter = new Chapter();
                    newChapter.ChapterTitle = menuItems[i].Item1;
                    newChapter.NovelTitle = NovelTitle;
                    newChapter.Index = maxIndex++;
                    newChapter.Read = false;
                    NovelLibrary.libraryData.Chapters.InsertOnSubmit(newChapter);
                    ChapterUrl newChapterUrl = new ChapterUrl();
                    newChapterUrl.ChapterID = newChapter.ID;
                    newChapterUrl.Url = menuItems[i].Item2;
                    newChapterUrl.Valid = true;
                    NovelLibrary.libraryData.ChapterUrls.InsertOnSubmit(newChapterUrl);
                }
            }
            int updateCount = (from chapter in NovelLibrary.libraryData.Chapters
                               where chapter.NovelTitle == NovelTitle && !chapter.HasText
                               select chapter).Count();
            if (updateCount == 0)
                SetUpdateProgress(0, 0, UpdateStates.UpToDate);
            else
                SetUpdateProgress(updateCount, 0, UpdateStates.UpdateAvailable);
            return updateCount > 0;
        }

        public bool DownloadChapter(Chapter downloadChapter, int index, int totalUpdateCount)
        {
            bool success = false;

            success = DownloadChapterContent(downloadChapter);

            if (success)
                SetUpdateProgress(index, totalUpdateCount, UpdateStates.Fetching);
            else
                SetUpdateProgress(0, 0, UpdateStates.Error);
            return success;
        }
        /*
        public int GetUpdateCount()
        {
            int updateCount = 0;
            foreach (Chapter c in _chapters)
                if (c.ChapterUrl != null && !c.HasText)
                    updateCount++;
            return updateCount;
        }

        public bool HasUpdate()
        {
            for (int i = 0; i < _chapters.Count; i++)
            {
                if (!_chapters[i].HasText && _chapters[i].ChapterUrl != null)
                {
                    if (_chapters[i].Equals(BackgroundService.lastUpdatedChapter))
                        continue;                 
                    return true;
                }  
            }
            return false;
        }
        */
        public bool DownloadChapterContent(Chapter chapter)
        {
            if (chapter == null || chapter.ChapterUrl == null)
                return false;
            BackgroundService.lastUpdatedChapter = chapter;
            string[] novelContent = _novelSource.GetChapterContent(chapter.ChapterTitle, chapter.ChapterUrl.Url);
            if (novelContent == null)
                return false;
            System.IO.File.WriteAllLines(chapter.GetTextFileLocation(), novelContent);
            return true;
        }

        //Change the index of the chapter and change the file name of the text and audio file.
        public void ChangeIndex(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
                return;
            if (oldIndex < 0 || oldIndex >= _chapters.Count)
                return;
            if (newIndex < 0)
                return;
            if (newIndex >= _chapters.Count)
                newIndex = _chapters.Count - 1;

            Chapter tmp = _chapters[oldIndex];
            _chapters.RemoveAt(oldIndex);
            _chapters.Insert(newIndex, tmp);

            VeryifyAndCorrectChapterIndexing();
        }

        public Chapter GetChapter(int chapterIndex = -1)
        {
            if (chapterIndex >= 0 && chapterIndex < ChapterCount && _chapters.Count > 0)
                return _chapters[chapterIndex];
            return null;
        }

        public void StartReading()
        {
            _isReading = true;
            LoadChapterFromDB();
        }

        public void StopReading()
        {
            _isReading = false;
            SaveChapterToDB();
            ClearChapters(); 
        }

        public void StartReadingChapter(Chapter chapter)
        {
            if(_chapters.Contains(chapter))
            {
                chapter.NotifyPropertyChanged("Read");
                _lastViewedChapter = chapter;
                NewChaptersNotReadCount = 0;
                NovelLibrary.Instance.db.Store(this);
            }
        }

        public void StopReadingChapter(Chapter chapter)
        {
            if (_chapters.Contains(chapter))
            {
                _lastViewedChapter = chapter;
                NovelLibrary.Instance.db.Store(this);
            }
        }

        public void MarkOffChapter(Chapter chapter)
        {
            if (_chapters.Contains(chapter))
            {
                chapter.Read = true;
                LastViewedChapter = chapter;
                _lastReadChapter = chapter;
                NovelLibrary.Instance.db.Store(this);
            }
        }

        public Chapter AddChapter()
        {
            int maxIndex = (from chapter in NovelLibrary.libraryData.Chapters
                            where chapter.NovelTitle == NovelTitle
                            orderby chapter.Index descending
                            select chapter.Index).First();
            Chapter newChapter = new Chapter();
            newChapter.NovelTitle = NovelTitle;
            newChapter.ChapterTitle = "<Enter Chapter Title>";
            newChapter.Read = false;
            newChapter.Index = maxIndex + 1;
            NovelLibrary.libraryData.Chapters.InsertOnSubmit(newChapter);
            return newChapter;
        }

        public void DeleteChapter(Chapter deleteChapter, bool blackList)
        {
            if(NovelLibrary.libraryData.Chapters.Any(chapter => chapter.ID == deleteChapter.ID))
            {   
                if (deleteChapter.HasAudio)
                    File.Delete(deleteChapter.GetAudioFileLocation());
                if (deleteChapter.HasText)
                    File.Delete(deleteChapter.GetTextFileLocation());
                if (deleteChapter.Equals(_lastViewedChapter))
                {
                    if (deleteChapter.Index > 0)
                        _lastViewedChapter = GetChapter(deleteChapter.Index - 1);
                    else if (deleteChapter.Index == 0)
                        _lastViewedChapter = GetChapter(0);
                    else
                        _lastViewedChapter = null;
                }

                deleteChapter.Index = -1;
                if (deleteChapter.ChapterUrl != null)
                {
                    if (blackList)
                        deleteChapter.ChapterUrl.Valid = false;
                    else
                    {
                        NovelLibrary.libraryData.Chapters.DeleteOnSubmit(deleteChapter);
                        NovelLibrary.libraryData.ChapterUrls.DeleteOnSubmit(deleteChapter.ChapterUrl);
                    }
                }
                NovelLibrary.libraryData.SubmitChanges();
                VeryifyAndCorrectChapterIndexing();
            }
        }

        //Set the progress to be displayed in NovelListControl.
        public void SetUpdateProgress(int updatedItemCount = 0, int totalUpdateItemCount = 0, UpdateStates updateState = UpdateStates.Default)
        {
            int progress = 0;
            string message = "";

            if (totalUpdateItemCount > 0)
                progress = (int)(100.0f * (float)updatedItemCount / (float)totalUpdateItemCount);

            if (updateState == UpdateStates.Default)
            {
                switch (_state)
                {
                    case NovelState.Active:
                        updateState = UpdateStates.Waiting;
                        break;
                    case NovelState.Completed:
                        updateState = UpdateStates.Completed;
                        break;
                    case NovelState.Inactive:
                        updateState = UpdateStates.Inactive;
                        break;
                    case NovelState.Dropped:
                        updateState = UpdateStates.Dropped;
                        break;

                }
            }

            switch (updateState)
            {
                case UpdateStates.Waiting:
                    progress = 0;
                    message = "Waiting For Updates";
                    break;
                case UpdateStates.Checking:
                    progress = 0;
                    message = "Checking For Updates";
                    break;
                case UpdateStates.UpdateAvailable:
                    progress = 0;
                    if (updatedItemCount > 0)
                        message = updatedItemCount + " Updates Available";
                    else
                        message = "Novel Up To Date";
                    break;
                case UpdateStates.Fetching:
                    message = "Fetching Updates: " + updatedItemCount + " / " + totalUpdateItemCount;
                    break;
                case UpdateStates.UpToDate:
                    if (_state == NovelState.Active)
                        message = "Novel Up To Date";
                    else if (_state == NovelState.Completed)
                        message = "Complted Novel Up To Date";
                    else if (_state == NovelState.Inactive)
                        message = "Inactive Novel Up To Date";
                    else if (_state == NovelState.Dropped)
                        message = "Dropped Novel Up To Date";
                    progress = 0;
                    break;
                case UpdateStates.Completed:
                    progress = 0;
                    message = "Novel Completed";
                    break;
                case UpdateStates.Inactive:
                    progress = 0;
                    message = "Novel Inactive";
                    break;
                case UpdateStates.Dropped:
                    progress = 0;
                    message = "Novel Dropped";
                    break;
                case UpdateStates.Error:
                    progress = 0;
                    message = "Network Error";
                    break;
            }
            _updateState = updateState;
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
            {
                BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    UpdateProgress = new Tuple<int, string>(progress, message);
                }));
            }
            else
            {
                UpdateProgress = new Tuple<int, string>(progress, message);
            }
        }

        /*============Private Function======*/
        
        /*
        //Add a new chapter to the end of chapter list.
        private void AppendNewChapter(Chapter chapter)
        {
            if (_chapters.Contains(chapter))
                return;
            
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
            {
                System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(false);
                BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    
                    _chapters.Add(chapter);
                    mre.Set();
                }));
                mre.WaitOne(-1);
            }
            else
            {
                _chapters.Add(chapter);
            }
            _chapterCount = _chapters.Count;
        }

        private void InsertNewChapter(Chapter chapter, int pos)
        {
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
            {
                System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(false);
                BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    _chapters.Insert(pos, chapter);
                    mre.Set();
                }));
                mre.WaitOne(-1);
            }
            else
            {
                _chapters.Insert(pos, chapter);
            }
            VeryifyAndCorrectChapterIndexing();
        }
        */
        private void VeryifyAndCorrectChapterIndexing()
        {
            Chapter[] chapters = NovelChapters.ToArray<Chapter>();
            for (int i = 0; i < chapters.Length; i++)
            {
                if (chapters[i].Index != i)
                    chapters[i].ChangeIndex(i);
            }
            /*
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
            {
                BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    for (int i = 0; i < _chapters.Count; i++)
                    {
                        if (_chapters[i].Index != i)
                            _chapters[i].ChangeIndex(i);
                    }
                }));
            }
            else
            {
                for (int i = 0; i < _chapters.Count; i++)
                {
                    if (_chapters[i].Index != i)
                        _chapters[i].ChangeIndex(i);
                }
            }
            */
            //ChapterCount = _chapters.Count;
        }


        private void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                if (BackgroundService.Instance.novelListController != null && BackgroundService.Instance.novelListController.InvokeRequired)
                {
                    BackgroundService.Instance.novelListController.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    }));
                }
                else
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                NovelLibrary.libraryData.SubmitChanges();
            }
        }

    }
}
