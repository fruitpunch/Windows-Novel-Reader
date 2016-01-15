﻿using System;
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
        private int _newChaptersNotReadCount { get; set; }
        private int _rank { get; set; }
        private int _lastPlayedChapterID { get; set; }
        private bool _makeAudio { get; set; }
        private bool _isReading { get; set; }
        private Tuple<int, string> _updateProgress { get; set; }
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
            get { return _chapters.Count; }
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
            get { return this._chapters.Count.ToString() + "  ( " + this._newChaptersNotReadCount + " )"; }
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

        public int NewChapterCount
        {
            get { return this.updateUrlList.Count; }
        }

        /*============Constructor===========*/

        public Novel(string novelTitle, NovelState state = NovelState.Active, int rank = 0, bool isReading = false)
        {
            this._novelTitle = novelTitle;
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

        //Check and see if there is new chapter available for download.
        public bool CheckForUpdate()
        {
            SetUpdateProgress(0, 0, UpdateState.Checking);
            updateUrlList.Clear();
            Source.Source s = SourceManager.GetSource(_novelTitle, 22590, SourceManager.Sources.web69);
            Tuple<string, string>[] menuItems = s.GetMenuURLs();
            foreach (Tuple<string, string> kvp in menuItems)
            {
                int urlHash = kvp.Item2.GetHashCode();
                if (!validUrlIdSet.Contains(urlHash) && !invalidUrlIdSet.Contains(urlHash))
                    updateUrlList.Add(kvp);
            }
            SetUpdateProgress(updateUrlList.Count, 0, UpdateState.UpdateAvailable);
            return updateUrlList.Count > 0;
        }

        //Download the new chapters.
        public void DownloadUpdate()
        {
            Source.Source s = SourceManager.GetSource(_novelTitle, 22590, SourceManager.Sources.web69);
            
            int newlyAddedChapter = 0;
            int index = _chapters.Count;

            for (int i = 0; i < updateUrlList.Count; i++)
            {
                string chapterTitle = updateUrlList[i].Item1;
                string url = updateUrlList[i].Item2;

                Chapter newChapter = new Chapter(chapterTitle, _novelTitle, false, index + i);
                AppendNewChapter(newChapter);
                validUrlIdSet.Add(url.GetHashCode());
                newlyAddedChapter++;
                SetUpdateProgress(i, updateUrlList.Count, UpdateState.Fetching);
                string[] novelContent = s.GetChapterContent(chapterTitle, url);
                string chapterLocation = newChapter.GetTextFileLocation();
                System.IO.File.WriteAllLines(chapterLocation, novelContent);
                
            }
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

            for (int i = 0; i < _chapters.Count; i++)
                _chapters[i].ChangeIndex(i);
        }

        

        /*============Private Function======*/
        //Add a new chapter to the end of chapter list.
        private void AppendNewChapter(Chapter chapter)
        {
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
        }

        private void InsertChapter(Chapter chapter)
        {
            for (int i = 0; i < _chapters.Count; i++)
            {
                if (chapter.Index < _chapters[i].Index)
                    _chapters.Insert(i, chapter);
            }
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
