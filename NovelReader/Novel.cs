using System;
using System.Collections.Generic;using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Source;
using System.IO;

namespace NovelReader
{
    public class Novel : INotifyPropertyChanged
    {
        public enum NovelState : int { Active = 0, Inactive = 1, Completed = 2, Dropped = 3 };
        public enum UpdateState { Default, Waiting, Checking, Fetching, UpToDate, Inactive, Completed, Dropped };

        public event PropertyChangedEventHandler PropertyChanged;

        private string _novelTitle { get; set; }
        private NovelState _state { get; set; }
        private int _newChapterCount { get; set; }
        private int _rank { get; set; }
        private int _lastPlayedChapterID { get; set; }
        private bool _makeAudio { get; set; }
        private bool _isReading { get; set; }
        private Tuple<int, string> _updateProgress { get; set; }
        private BindingList<Chapter> _chapters;

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
            get { return _chapters.Count; }
        }

        public int NewChapterCount
        {
            get { return this._newChapterCount; }
            set
            {
                this._newChapterCount = value;
                NotifyPropertyChanged("NewChapterCount");
            }
        }

        public int Rank
        {
            get { return this._rank; }
            set { 
                this._rank = value;
                NotifyPropertyChanged("Rank");
            }
        }

        public int LastPlayedChapterID
        {
            get { return this._lastPlayedChapterID; }
            set { this._lastPlayedChapterID = value; }
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

        public Tuple<int, string> UpdateProgress
        {
            get { return this._updateProgress; }
            set
            {
                this._updateProgress = value;
                NotifyPropertyChanged("UpdateProgress");
            }
        }

        /*============Constructor===========*/

        public Novel(string novelTitle, NovelState state = NovelState.Active, int rank = 0, bool isReading = false)
        {
            this._novelTitle = novelTitle;
            this._state = state;
            this._rank = rank;
            this._newChapterCount = 0;
            this._isReading = isReading;

            this._chapters = new BindingList<Chapter>();
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

        public void Update()
        {
            SetUpdateProgress(0, 0, UpdateState.Checking);
            Source.Source s = SourceManager.GetSource(_novelTitle, 22590, SourceManager.Sources.web69);
            Tuple<string, string>[] menuItems = s.GetMenuURLs();
            int newlyAddedChapter = 0;
            
            for (int i = _chapters.Count; i < menuItems.Length; i++)
            {
                string chapterTitle = menuItems[i].Item1;
                string url = menuItems[i].Item2;

                Chapter newChapter = new Chapter(chapterTitle, _novelTitle, false, i);
                AppendNewChapter(newChapter);
                newlyAddedChapter++;
                SetUpdateProgress(i, menuItems.Length, UpdateState.Fetching);
                string[] novelContent = s.GetChapterContent(chapterTitle, url);
                string chapterLocation = newChapter.GetTextFileLocation();
                System.IO.File.WriteAllLines(chapterLocation, novelContent);
            }
            SetUpdateProgress(0, 0, UpdateState.UpToDate);
            if (BackgroundService.Instance.novelListController.InvokeRequired)
            {
                BackgroundService.Instance.novelListController.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    NewChapterCount = _newChapterCount + newlyAddedChapter;
                    NotifyPropertyChanged("ChapterCount");
                }));
            }
        }

        public void UpdateDB()
        {
            
        }

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

            for (int i = 0; i < _chapters.Count; i++)
                _chapters[i].ChangeIndex(i);
        }

        /*============Private Function======*/

        private void AppendNewChapter(Chapter chapter)
        {
            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequired)
            {
                BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    _chapters.Add(chapter);
                }));
            }
            else
            {
                _chapters.Add(chapter);
            }
        }

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
                case UpdateState.Fetching:
                    message = "Fetching Updates: " + updatedItemCount + " / " + totalUpdateItemCount;
                    break;
                case UpdateState.UpToDate:
                    progress = 100;
                    message = "Novel Up To Date";
                    break;
                case UpdateState.Completed:
                    progress = 100;
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
