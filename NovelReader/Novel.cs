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
    public class Novel : INotifyPropertyChanged
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


        public enum UpdateState { Default, Waiting, Checking, UpdateAvailable, Fetching, UpToDate, Inactive, Completed, Dropped };

        public event PropertyChangedEventHandler PropertyChanged;

        private string _novelTitle { get; set; }
        private NovelState _state { get; set; }
        private int _chapterCount { get; set; }
        private int _newChaptersNotReadCount { get; set; }
        private int _rank { get; set; }
        private Chapter _lastReadChapter { get; set; }
        private bool _makeAudio { get; set; }
        private bool _isReading { get; set; }
        Source.Source _novelSource { get; set; }
        private Tuple<int, string> _updateProgress { get; set; }
        [Transient] 
        private BindingList<Chapter> _chapters;

        private HashSet<int> validUrlIdSet;
        private HashSet<int> invalidUrlIdSet;
        private List<Tuple<string, string>> updateUrlList;

        /*============Properties============*/

        public string NovelTitle
        {
            get { return this._novelTitle; }
        }

        public NovelState State
        {
            get { return this._state; }
            set {
                if (this._state != NovelState.Dropped && value == NovelState.Dropped)
                    NovelLibrary.Instance.DropNovel(this._novelTitle);
                else if (this._state == NovelState.Dropped && value != NovelState.Dropped)
                    NovelLibrary.Instance.PickUpNovel(this._novelTitle);
                this._state = value;
                SetUpdateProgress();
            }
        }

        public int ChapterCount
        {
            get { return this._chapterCount; }
            set
            {
                this._chapterCount = value;
                NotifyPropertyChanged("ChapterCountStatus");
            }
        }

        public int NewChaptersNotReadCount
        {
            get { return this._newChaptersNotReadCount; }
            set
            {
                this._newChaptersNotReadCount = value;
                NotifyPropertyChanged("ChapterCountStatus");
            }
        }

        public string ChapterCountStatus
        {
            get { return this._chapterCount.ToString() + "  ( " + this._newChaptersNotReadCount + " )"; }
        }

        public int Rank
        {
            get { return this._rank; }
            set { 
                this._rank = value;
                NotifyPropertyChanged("Rank");
            }
        }

        public Chapter LastReadChapter
        {
            get { return this._lastReadChapter; }
            set { this._lastReadChapter = value; }
        }

        public bool MakeAudio
        {
            get { return this._makeAudio; }
            set { this._makeAudio = value; }
        }

        public BindingList<Chapter> Chapters
        {
            get { return this._chapters; }
        }

        public bool Reading
        {
            get { return this._isReading; }
            set { this._isReading = value; }
        }

        public Source.Source NovelSource
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

        public int NewChapterCount
        {
            get { return this.updateUrlList.Count; }
        }

        /*============Constructor===========*/

        public Novel(string novelTitle, SourceManager.Sources source, int sourceId, NovelState state = NovelState.Active, int rank = 0, bool isReading = false)
        {
            this._novelTitle = novelTitle;
            this._novelSource = SourceManager.GetSource(novelTitle, sourceId, source);
            this._state = state;
            this._rank = rank;
            this._newChaptersNotReadCount = 0;
            this._isReading = isReading;

            this._chapters = new BindingList<Chapter>();
            this.validUrlIdSet = new HashSet<int>();
            this.invalidUrlIdSet = new HashSet<int>();
            this.updateUrlList = new List<Tuple<string, string>>();
            SetUpdateProgress();
        }

        /*============Getter/Setter=========*/

        public string GetDeleteSpecificationLocation()
        {
            string path = System.IO.Path.Combine(Configuration.Instance.NovelFolderLocation, _novelTitle, Configuration.Instance.DeleteSpecification);
            if (File.Exists(path))
                return path;
            return null;
        }

        public string GetReplaceSpecificationLocation()
        {
            string path = System.IO.Path.Combine(Configuration.Instance.NovelFolderLocation, _novelTitle, Configuration.Instance.ReplaceSpecification);
            if (File.Exists(path))
                return path;
            return null;
        }

        /*============Public Function=======*/

        public void LoadChapterFromDB()
        {
            if (_chapters == null)
                _chapters = new BindingList<Chapter>();
            //var results = NovelLibrary.Instance.db.QueryByExample(new Chapter(null, _novelTitle, null, false, -1));
            //var results = NovelLibrary.Instance.db.QueryByExample(typeof(Chapter));
            var results = NovelLibrary.Instance.db.Query<Chapter>();
            var sorted = from chapters in results
                         where chapters.NovelTitle == _novelTitle
                         orderby chapters.Index
                         select chapters;
            foreach (Chapter chapter in sorted)
            {
                AppendNewChapter(chapter);
                //Console.WriteLine("added chapter " + chapter.Index);
                if (_lastReadChapter != null && _lastReadChapter.Index == chapter.Index)
                    _lastReadChapter = chapter;
            }
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

        public void ClearChapters()
        {
            if (_chapters == null)
                return;
            _chapters.Clear();
        }

        //Check and see if there is new chapter available for download.
        public bool CheckForUpdate()
        {
            LoadChapterFromDB();

            SetUpdateProgress(0, 0, UpdateState.Checking);
            updateUrlList.Clear();
            Tuple<string, string>[] menuItems = _novelSource.GetMenuURLs();
            int updateChapterCount = 0;
            int chapterIndex = 0;
            for (int i = 0; i < menuItems.Length; i++)
            {
                //if (chapterIndex < _chapters.Count && menuItems[i].Item2.Equals(_chapters[chapterIndex].SourceURL))
                if (chapterIndex < _chapters.Count && validUrlIdSet.Contains(menuItems[i].Item2.GetHashCode()))
                {
                    chapterIndex++;
                }
                else if (!invalidUrlIdSet.Contains(menuItems[i].Item2.GetHashCode()) && !validUrlIdSet.Contains(menuItems[i].Item2.GetHashCode()))
                {
                    Chapter newChapter = new Chapter(menuItems[i].Item1, _novelTitle, menuItems[i].Item2, false, chapterIndex);
                    if (i < chapterIndex)
                        InsertNewChapter(newChapter, i);
                    else
                        InsertNewChapter(newChapter, chapterIndex);
                    validUrlIdSet.Add(menuItems[i].Item2.GetHashCode());
                    chapterIndex++;
                    updateChapterCount++;
                    updateUrlList.Add(menuItems[i]);
                }
                
            }
            
            SetUpdateProgress(updateChapterCount, 0, UpdateState.UpdateAvailable);
            return updateUrlList.Count > 0;
        }

        //Download the new chapters.
        public void DownloadUpdate()
        {
            int newlyAddedChapter = 0;
            int index = _chapters.Count;

            for (int i = 0; i < _chapters.Count; i++)
            {
                if (!_chapters[i].HasText)
                {
                    newlyAddedChapter++;
                    DownloadChapterContent(_chapters[i]);
                    SetUpdateProgress(i, updateUrlList.Count, UpdateState.Fetching);
                }
            }
            SaveChapterToDB();
            if (!_isReading)
                ClearChapters();

            updateUrlList.Clear();
            SetUpdateProgress(0, 0, UpdateState.UpToDate);
            if (BackgroundService.Instance.novelListController.InvokeRequired)
            {
                BackgroundService.Instance.novelListController.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    NewChaptersNotReadCount = _newChaptersNotReadCount + newlyAddedChapter;
                }));
            }
        }

        public void DownloadChapterContent(Chapter chapter)
        {
            if (chapter == null || chapter.SourceURL == null)
                return;
            string[] novelContent = _novelSource.GetChapterContent(chapter.ChapterTitle, chapter.SourceURL);
            System.IO.File.WriteAllLines(chapter.GetTextFileLocation(), novelContent);
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

        public void ReadChapter(Chapter chapter)
        {
            if(_chapters.Contains(chapter))
            {
                chapter.Read = true;
                LastReadChapter = chapter;
                NewChaptersNotReadCount = 0;
            }
        }

        public Chapter AddChapter()
        {
            Chapter chapter = new Chapter("<Enter Chapter Title>", _novelTitle, null, false, _chapterCount);
            AppendNewChapter(chapter);
            NovelLibrary.Instance.db.Store(chapter);
            NovelLibrary.Instance.db.Commit();
            return chapter;
        }

        public void DeleteChapter(Chapter chapter, bool blackList)
        {
            if (_chapters.Contains(chapter))
            {
                if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequired)
                {
                    BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        _chapters.Remove(chapter);
                    }));
                }
                else
                {
                    _chapters.Remove(chapter);
                }
                if (chapter.SourceURL != null)
                {
                    if(blackList)
                        invalidUrlIdSet.Add(chapter.SourceURL.GetHashCode());
                    validUrlIdSet.Remove(chapter.SourceURL.GetHashCode());
                }

                
                if (chapter.HasAudio)
                    File.Delete(chapter.GetAudioFileLocation());
                if (chapter.HasText)
                    File.Delete(chapter.GetTextFileLocation());
                if (chapter.Equals(_lastReadChapter))
                {
                    if (chapter.Index > 0)
                        _lastReadChapter = GetChapter(chapter.Index - 1);
                    else if (chapter.Index == 0)
                        _lastReadChapter = GetChapter(0);
                    else
                        _lastReadChapter = null;
                }
                NovelLibrary.Instance.db.Delete(chapter);
                NovelLibrary.Instance.db.Commit();
                VeryifyAndCorrectChapterIndexing();
            }
        }

        /*============Private Function======*/
        

        //Add a new chapter to the end of chapter list.
        private void AppendNewChapter(Chapter chapter)
        {
            if (_chapters.Contains(chapter))
                return;
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequired)
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
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequired)
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

        private void VeryifyAndCorrectChapterIndexing()
        {
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequired)
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
            ChapterCount = _chapters.Count;
        }

        //Set the progress to be displayed in NovelListControl.
        private void SetUpdateProgress(int updatedItemCount = 0, int totalUpdateItemCount = 0, UpdateState updateState = UpdateState.Default)
        {
            int progress = 0;
            string message = "";

            if (totalUpdateItemCount > 0)
                progress = (int)(100.0f * (float)updatedItemCount / (float)totalUpdateItemCount);

            if (updateState == UpdateState.Default)
            {
                switch (_state)
                {
                    case NovelState.Active:
                        updateState = UpdateState.Waiting;
                        break;
                    case NovelState.Completed:
                        updateState = UpdateState.Completed;
                        break;
                    case NovelState.Inactive:
                        updateState = UpdateState.Inactive;
                        break;
                    case NovelState.Dropped:
                        updateState = UpdateState.Dropped;
                        break;

                }
            }

            switch (updateState)
            {
                case UpdateState.Waiting:
                    progress = 0;
                    message = "Waiting For Updates";
                    break;
                case UpdateState.Checking:
                    progress = 0;
                    message = "Checking For Updates";
                    break;
                case UpdateState.UpdateAvailable:
                    progress = 0;
                    if (updatedItemCount > 0)
                        message = updatedItemCount + " Updates Available";
                    else
                        message = "Novel Up To Date";
                    break;
                case UpdateState.Fetching:
                    message = "Fetching Updates: " + updatedItemCount + " / " + totalUpdateItemCount;
                    break;
                case UpdateState.UpToDate:
                    if (_state == NovelState.Active)
                        message = "Novel Up To Date";
                    else if(_state == NovelState.Completed)
                        message = "Complted Novel Up To Date";
                    else if (_state == NovelState.Inactive)
                        message = "Inactive Novel Up To Date";
                    else if (_state == NovelState.Dropped)
                        message = "Dropped Novel Up To Date";
                    progress = 0;
                    break;
                case UpdateState.Completed:
                    progress = 0;
                    message = "Novel Completed";
                    break;
                case UpdateState.Inactive:
                    progress = 0;
                    message = "Novel Inactive";
                    break;
                case UpdateState.Dropped:
                    progress = 0;
                    message = "Novel Dropped";
                    break;
            }

            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequired)
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

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
