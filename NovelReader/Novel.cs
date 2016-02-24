using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Source;
using System.IO;
using System.Transactions;
using System.Threading;

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

        public enum UpdateStates { Default, Waiting, Syncing, Checking, UpdateAvailable, Fetching, UpToDate, Inactive, Completed, Dropped, Error };

        //public event PropertyChangedEventHandler PropertyChanged;

        private Chapter _lastViewedChapter { get; set; }
        private Chapter _lastReadChapter { get; set; }
        private bool _isReading { get; set; }
        private bool isDirty = true;
        private BindingList<Chapter> chapterList = null;
        private Dictionary<string, Chapter> chapterDictionary = null;
        private Dictionary<string, ISource> sourceDictionary = null;
        UpdateStates _updateState { get; set; }
        private Tuple<int, string> _updateProgress { get; set; }



        //==========
        private Dictionary<Chapter, Request> queuedTTSChapters;
        private int requestIndex;
        //==========

        /*============Properties============*/

        public NovelState State
        {
            get { return (NovelState)StateID; }
            set {
                if (State != NovelState.Dropped && value == NovelState.Dropped)
                    NovelLibrary.Instance.DropNovel(NovelTitle);
                else if (State == NovelState.Dropped && value != NovelState.Dropped)
                    NovelLibrary.Instance.PickUpNovel(NovelTitle);
                StateID = (int)value;
                SetUpdateProgress();
                SendPropertyChanged("State");
                NovelLibrary.libraryData.SubmitChanges();
            }
        }

        public int ChapterCount
        {
            get {
                var result = NovelChapters;
                if(result == null)
                    return 0;
                return result.Count;
            }
        }

        public string ChapterCountStatus
        {
            get {
                if(ChaptersNotReadCount > 0)
                    return ChapterCount.ToString() + "  ( " + ChaptersNotReadCount + " new chapters )";
                return ChapterCount.ToString();
            }
        }

        public Chapter LastViewedChapter
        {
            get { return this._lastViewedChapter; }
            set { this._lastViewedChapter = value; }
        }

        public Chapter LastReadChapter
        {
            get {
                var result = (from chapter in NovelLibrary.libraryData.Chapters
                                           where chapter.ID == LastReadChapterID
                                           select chapter);
                if (result.Any())
                {
                    Chapter c = result.First<Chapter>();
                    if (c.Index < 0)
                        return null;
                    return c;
                }
                    
                return null;
            }
            set {
                if (value != null)
                    LastReadChapterID = value.ID;
                else
                    LastReadChapterID = null;
            }
        }

        public BindingList<Chapter> NovelChapters
        {
            get {
                if (!isDirty && chapterList != null)
                    return chapterList;
                else if (chapterList == null)
                    chapterList = new BindingList<Chapter>();
                /*
                if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
                {
                    BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        RefreshCacheData();
                    }));
                }
                else
                {
                    RefreshCacheData();
                }*/
                RefreshCacheData();
                return chapterList;
            }
        }

        public Dictionary<string, Chapter> ChapterDictionary
        {
            get
            {
                if (!isDirty && chapterList != null)
                    return chapterDictionary;
                else if (chapterList == null)
                    chapterDictionary = new Dictionary<string, Chapter>();
                /*
                if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
                {
                    BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        RefreshCacheData();
                    }));
                }
                else
                {
                    RefreshCacheData();
                }*/
                RefreshCacheData();
                return chapterDictionary;
            }
        }

        public bool Reading
        {
            get { return this._isReading; }
            set { this._isReading = value; }
        }

        public Source OriginSource
        {
            get
            {
                try
                {
                    Source s = (from source in Sources
                                where !source.Mirror
                                select source).First<Source>();
                    return s;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Source[] OrderedSources
        {
            get
            {
                try
                {
                    Source[] sources = (from source in Sources
                                        orderby source.Priority
                                        select source).ToArray<Source>();
                    return sources;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Tuple<int, string> UpdateProgress
        {
            get {
                if(_updateProgress == null)
                    SetUpdateProgress();
                return this._updateProgress;
            }
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


        /*============Constructor===========*/

        partial void OnLoaded()
        {
            Initiate();
        }

        public void Initiate()
        {
            this.requestIndex = 0;
            this.queuedTTSChapters = new Dictionary<Chapter, Request>();
            SetUpdateProgress();
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

        public Request GetTTSRequest(int speed)
        {
            List<Chapter> chapters = NovelChapters.ToList();
            if (chapters == null)
                return null;
            Request request = null;
            Chapter c = null;
            for (requestIndex = 0; requestIndex < chapters.Count; requestIndex++)
            {
                c = chapters[requestIndex];
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
            if (requestIndex >= chapters.Count || request == null)
            {
                requestIndex = 0;
                for (requestIndex = 0; requestIndex < chapters.Count; requestIndex++)
                {
                    c = chapters[requestIndex];
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
            //if (_chapters == null || _chapters.Count == 0)
            //    return false;
            if (_lastReadChapter != null && _lastReadChapter.Index > chapter.Index && !secondaryPass)
                return false;
            //if (Configuration.Instance.LanguageVoiceDictionary[NovelSource.NovelLanguage] == null || Configuration.Instance.LanguageVoiceDictionary[NovelSource.NovelLanguage].Equals("No Voice Selected"))
            //    return false;
            //Do not make audio for novel not selected
            if (!MakeAudio)
                return false;
            //Do not make audio for novel that is dropped
            if (State == NovelState.Dropped)
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
            priority += (NovelLibrary.Instance.GetNovelCount() - Rank) * 5;

            //State also changes priority
            switch (State)
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
            if (LastReadChapter != null)
            {
                lastReadChapterIndex = LastReadChapter.Index;
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

        //Sync the chapter list to the origin source.
        public bool SyncToOrigin()
        {
            
            SetUpdateProgress(0, 0, UpdateStates.Syncing);
            if (OriginSource == null)
                return false;
            ChapterSource[] menuItems = OriginSource.GetMenuURLs();
            if (menuItems == null)
            {
                SetUpdateProgress(0, 0, UpdateStates.Error);
                return false;
            }
            List<Chapter> chapterListing = new List<Chapter>();
            int updateCount = 0;
            for (int i = 0; i < menuItems.Length; i++)
            {
                //add new chapter if url does not exist in database
                if (!OriginSource.ChapterUrls.Where(originUrl => originUrl.Hash == menuItems[i].UrlHash).Any())
                {
                    Chapter newChapter = new Chapter();
                    newChapter.ChapterTitle = menuItems[i].Title;
                    newChapter.NovelTitle = NovelTitle;
                    newChapter.Read = false;
                    newChapter.Index = -1;
                    newChapter.Novel = this;
                    NovelLibrary.libraryData.Chapters.InsertOnSubmit(newChapter);
                    NovelLibrary.libraryData.SubmitChanges();
                    chapterListing.Add(newChapter);
                    isDirty = true;
                    updateCount++;

                    ChapterUrl newChapterUrl = new ChapterUrl();
                    newChapterUrl.ChapterID = newChapter.ID;
                    newChapterUrl.Url = menuItems[i].Url;
                    newChapterUrl.Hash = menuItems[i].UrlHash;
                    newChapterUrl.Valid = true;
                    newChapterUrl.Vip = menuItems[i].Vip;
                    newChapterUrl.SourceID = OriginSource.ID;
                    newChapterUrl.Source = OriginSource;
                    newChapterUrl.Chapter = newChapter;
                    NovelLibrary.libraryData.ChapterUrls.InsertOnSubmit(newChapterUrl);
                    NovelLibrary.libraryData.SubmitChanges();

                }
                //ignore if url already exists in database and that url is invalid.
                else if (OriginSource.ChapterUrls.Where(originUrl => originUrl.Url == menuItems[i].Url && !originUrl.Valid).Any())
                {
                    continue;
                }
                //chapter already exist within database and is valid so add chapter to list to correct indexing.
                else
                {
                    Chapter c = (from chapterUrl in OriginSource.ChapterUrls
                                 where chapterUrl.Url == menuItems[i].Url
                                 select chapterUrl.Chapter).First<Chapter>();
                    if (c.ChapterTitle != menuItems[i].Title)
                        c.ChangeChapterTitle(menuItems[i].Title);
                    chapterListing.Add(c);
                }
            }
            //Remove chapter that no longer belong in the list.
            HashSet<int> includedID = new HashSet<int>(chapterListing.Select(c => c.ID));
            Chapter[] allChapters = Chapters.ToArray();
            for(int i = 0; i < allChapters.Count(); i++)
            {
                if (!includedID.Contains(allChapters[i].ID) && allChapters[i].Index >= 0)
                {
                    Console.WriteLine("Deleting " + allChapters[i].ChapterTitle + " " + allChapters[i].ID);
                    DeleteChapter(allChapters[i], false, false);
                }
                    
            }
            VeryifyAndCorrectChapterIndexing(chapterListing.ToArray());
            RefreshCacheData();
            if (updateCount == 0)
                SetUpdateProgress(0, 0, UpdateStates.UpToDate);
            else
                SetUpdateProgress(updateCount, 0, UpdateStates.UpdateAvailable);
            return true;
        }

        //Check and see if there is new chapter available for download.
        public bool CheckForUpdate()
        {
            Source[] sources = OrderedSources;
            Dictionary<string, Chapter> chapters = ChapterDictionary;
            ChapterSource[] menuItems;
            SetUpdateProgress(0, 0, UpdateStates.Checking);
            foreach (Source source in sources)
            {
                if (source == null || !source.Valid ||source.ID == OriginSource.ID)
                    continue;
                menuItems = source.GetMenuURLs();
                if (menuItems == null)
                    continue;
                HashSet<string> usedTitle = new HashSet<string>();
                for (int i = 0; i < menuItems.Length; i++)
                {
                    //Check for duplicate chapter titles and changes the name of it so it will not conflict.
                    string chapterTitle = menuItems[i].Title;
                    int dupIdx = 2;
                    while (usedTitle.Contains(chapterTitle))
                    {
                        chapterTitle = menuItems[i].Title + "-duplicate x " + dupIdx;
                        dupIdx++;
                    }
                    usedTitle.Add(chapterTitle);

                    if (chapters.Keys.Contains(chapterTitle) && !NovelLibrary.libraryData.ChapterUrls.Where(url => url.Hash == menuItems[i].UrlHash).Any())
                    {
                        ChapterUrl newChapterUrl = new ChapterUrl();
                        newChapterUrl.ChapterID = chapters[chapterTitle].ID;
                        newChapterUrl.Url = menuItems[i].Url;
                        newChapterUrl.Hash = menuItems[i].UrlHash;
                        newChapterUrl.Valid = true;
                        newChapterUrl.Vip = menuItems[i].Vip;
                        newChapterUrl.SourceID = source.ID;
                        newChapterUrl.Source = source;
                        newChapterUrl.Chapter = chapters[chapterTitle];
                        NovelLibrary.libraryData.ChapterUrls.InsertOnSubmit(newChapterUrl);
                        NovelLibrary.libraryData.SubmitChanges();
                    }
                }

            }
            return true;
        }

        public bool DownloadChapter(Chapter downloadChapter, Source specificSource = null)
        {
            if (downloadChapter == null)
            {
                SetUpdateProgress(0, 0, UpdateStates.Error);
                return false;
            }

            if (UpdateState == UpdateStates.Syncing || UpdateState == UpdateStates.Checking)
                return false;

            var result = downloadChapter.ChapterUrls;
            if (!result.Any())
            {
                SetUpdateProgress(0, 0, UpdateStates.Error);
                return false;
            }

            if(specificSource != null)
            {
                if (!Sources.Contains(specificSource))
                    return false;
                var urlResult = (from url in downloadChapter.ChapterUrls
                                  where url.Source.ID == specificSource.ID
                                  select url);
                if (!urlResult.Any())
                    return false;
                ChapterUrl specificUrl = urlResult.First<ChapterUrl>();
                string[] novelContent = specificSource.GetChapterContent(downloadChapter.ChapterTitle, specificUrl.Url);
                if (novelContent == null)
                    return false;
                System.IO.File.WriteAllLines(downloadChapter.GetTextFileLocation(), novelContent);
                return true;
            }

            ChapterUrl[] urls = result.ToArray<ChapterUrl>();
            foreach (ChapterUrl url in urls)
            {
                //Console.WriteLine("Download chapter " + downloadChapter.ChapterTitle + " " + (url.Vip));
                if (url == null || url.Vip || !url.Valid)
                    continue;
                Source source = url.Source;
                
                if (source == null)
                    continue;
                
                string[] novelContent = source.GetChapterContent(downloadChapter.ChapterTitle, url.Url);
                if (novelContent == null)
                    continue;
                System.IO.File.WriteAllLines(downloadChapter.GetTextFileLocation(), novelContent);
                return true;
            }

            return false;
        }

        //Change the index of the chapter and change the file name of the text and audio file.
        public void ChangeIndex(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
                return;
            if (oldIndex < 0 || oldIndex >= chapterList.Count)
                return;
            if (newIndex < 0)
                return;
            if (newIndex >= chapterList.Count)
                newIndex = chapterList.Count - 1;

            Chapter tmp = chapterList[oldIndex];
            chapterList.RemoveAt(oldIndex);
            chapterList.Insert(newIndex, tmp);

            VeryifyAndCorrectChapterIndexing(chapterList.ToArray());
        }

        public Chapter GetChapter(int chapterIndex = -1)
        {
            if (chapterIndex < 0 || chapterIndex >= Chapters.Count )
                return null;

            var result = Chapters.Where(chapter => chapter.Index == chapterIndex);
            if(result != null && result.Any())
                return result.First<Chapter>();
            return null;
        }

        public void StartReading()
        {
            _isReading = true;
        }

        public void StopReading()
        {
            _isReading = false;
        }

        public void StartReadingChapter(Chapter chapter)
        {
            //chapter.NotifyPropertyChanged("Read");
            //_lastViewedChapter = chapter;
            ChaptersNotReadCount = 0;
        }

        public void StopReadingChapter(Chapter chapter)
        {
            LastReadChapter = chapter;
        }

        public void MarkOffChapter(Chapter chapter)
        {
            chapter.Read = true;
            //LastViewedChapter = chapter;
            LastReadChapter = chapter;
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
            NovelLibrary.libraryData.SubmitChanges();
            isDirty = true;
            NotifyPropertyChanged("ChapterCountStatus");
            return newChapter;
        }

        public void DeleteAllChapter(Chapter[] deleteChapters, bool blackList)
        {
            foreach (Chapter deleteChapter in deleteChapters)
                DeleteChapter(deleteChapter, blackList, false);

            VeryifyAndCorrectChapterIndexing(NovelChapters.ToArray<Chapter>());
            RefreshCacheData();
        }

        public void DeleteChapter(Chapter deleteChapter, bool blackList, bool verifyData = true)
        {
            if(NovelLibrary.libraryData.Chapters.Any(chapter => chapter.ID == deleteChapter.ID))
            {   
                if (deleteChapter.HasAudio)
                    File.Delete(deleteChapter.GetAudioFileLocation());
                if (deleteChapter.HasText)
                    File.Delete(deleteChapter.GetTextFileLocation());
                if (deleteChapter.ID == LastReadChapterID)
                {
                    Console.WriteLine(deleteChapter.ID + " " + deleteChapter.Index + " " + deleteChapter.ChapterTitle);
                    if (deleteChapter.Index > 0)
                        LastReadChapter = GetChapter(deleteChapter.Index - 1);
                    else if (deleteChapter.Index == 0 && ChapterCount > 1)
                        LastReadChapter = GetChapter(1);
                    else
                        LastReadChapter = null;
                }
                ChapterUrl[] urls = deleteChapter.ChapterUrls.ToArray<ChapterUrl>();
                if (urls != null)
                {
                    if (blackList)
                    {
                        deleteChapter.Index = Int32.MinValue;
                        foreach(ChapterUrl url in urls)
                            url.Valid = false;
                    } 
                    else
                    {
                        NovelLibrary.libraryData.Chapters.DeleteOnSubmit(deleteChapter);
                        NovelLibrary.libraryData.ChapterUrls.DeleteAllOnSubmit(deleteChapter.ChapterUrls);
                    }
                }
                else
                    NovelLibrary.libraryData.Chapters.DeleteOnSubmit(deleteChapter);
                isDirty = true;
                NovelLibrary.libraryData.SubmitChanges();
                
                //foreach (Chapter c in chapters)
                //    Console.WriteLine(c.ChapterTitle + " " + c.Index +" " + c.ID);
                if(verifyData)
                {
                    Chapter[] chapters = NovelChapters.ToArray<Chapter>();
                    VeryifyAndCorrectChapterIndexing(chapters);
                    RefreshCacheData();
                }
                Console.WriteLine(LastReadChapterID == null);
                NotifyPropertyChanged("ChapterCountStatus");
            }
        }

        public bool AddSource(ISource newSource, bool mirror, out string message)
        {
            message = newSource + " successfully added.";
            if (Sources.Where(source => source.SourceNovelLocation == newSource.SourceNovelLocation).Any())
            {
                message = "Duplicate Source Location \"" + newSource.SourceNovelLocation.ToString() + "\" found for novel " + NovelTitle;
                return false;
            }
                
            if (UpdateState == UpdateStates.Syncing || UpdateState == UpdateStates.Checking || UpdateState == UpdateStates.Fetching)
            {
                message = "Novel is currently updating. Please wait until finish before making changes to novel source.";
                return false;
            }
            Source s = new Source();
            s.SourceNovelLocation = newSource.SourceNovelLocation;
            s.SourceNovelID = newSource.NovelID;
            s.NovelTitle = NovelTitle;
            s.Mirror = Sources.Count > 1;
            s.Priority = Sources.Count;
            s.Mirror = mirror;
            s.Novel = this;
            s.Valid = true;
            NovelLibrary.libraryData.Sources.InsertOnSubmit(s);
            NovelLibrary.libraryData.SubmitChanges();
            return true;
        }

        public bool DeleteSource(Source deleteSource, out string message)
        {
            message = deleteSource.SourceNovelLocation + " successfully removed.";
            if (!Sources.Contains(deleteSource))
            {
                message = "Novel does not contain source.";
                return false;
            }
                
            if (UpdateState == UpdateStates.Syncing || UpdateState == UpdateStates.Checking || UpdateState == UpdateStates.Fetching)
            {
                message = "Novel is currently updating. Please wait until finish before making changes to novel source.";
                return false;
            }

            if(!deleteSource.Mirror)
            {
                message = "Cannot delete novel's origin source.";
                return false;
            }
                
            NovelLibrary.libraryData.ChapterUrls.DeleteAllOnSubmit(deleteSource.ChapterUrls);
            NovelLibrary.libraryData.Sources.DeleteOnSubmit(deleteSource);
            NovelLibrary.libraryData.SubmitChanges();
            return true;
        }

        public bool RankUpSource(Source source, out string message)
        {
            message = "";
            if (!Sources.Contains(source))
            {
                message = "Invalid source";
                return false;
            }

            if (UpdateState == UpdateStates.Syncing || UpdateState == UpdateStates.Checking || UpdateState == UpdateStates.Fetching)
            {
                message = "Novel is currently updating. Please wait until finish before making changes to novel source.";
                return false;
            }

            if (!source.Mirror)
            {
                message = "Cannot change priority of origin source.";
                return false;
            }

            if (source.Priority < 2)
            {
                message = "Cannot give higher prioirty than origin source.";
                return false;
            }
                
            List<Source> sourceList = (from sources in Sources
                                       orderby sources.Priority ascending
                                       select sources).ToList<Source>();

            sourceList.RemoveAt(source.Priority);
            sourceList.Insert(source.Priority - 1, source);
            RefreshSourcePriority(sourceList.ToArray());
            return true;
        }

        public bool RankDownSource(Source source, out string message)
        {
            message = "";
            if (!Sources.Contains(source))
            {
                message = "Invalid source";
                return false;
            }

            if (UpdateState == UpdateStates.Syncing || UpdateState == UpdateStates.Checking || UpdateState == UpdateStates.Fetching)
            {
                message = "Novel is currently updating. Please wait until finish before making changes to novel source.";
                return false;
            }

            if (!source.Mirror)
            {
                message = "Cannot change priority of origin source.";
                return false;
            }

            if (source.Priority == Sources.Count - 1)
            {
                return false;
            }

            List<Source> sourceList = (from sources in Sources
                                       orderby sources.Priority ascending
                                       select sources).ToList<Source>();

            sourceList.RemoveAt(source.Priority);
            sourceList.Insert(source.Priority + 1, source);
            RefreshSourcePriority(sourceList.ToArray());
            return true;
        }

        private void RefreshSourcePriority(Source[] sources)
        {
            for(int i = 1; i < sources.Length; i++)
            {
                sources[i].Priority = i;
            }
            NovelLibrary.libraryData.SubmitChanges();
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
                switch (State)
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
                case UpdateStates.Syncing:
                    progress = 0;
                    message = "Syncing to origin";
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
                    if (State == NovelState.Active)
                        message = "Novel Up To Date";
                    else if (State == NovelState.Completed)
                        message = "Complted Novel Up To Date";
                    else if (State == NovelState.Inactive)
                        message = "Inactive Novel Up To Date";
                    else if (State == NovelState.Dropped)
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
            UpdateProgress = new Tuple<int, string>(progress, message);
        }

        public void NotifyPropertyChanged(string propertyName)
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
                //NovelLibrary.libraryData.SubmitChanges();
            }
        }

        /*============Private Function======*/


        private void VeryifyAndCorrectChapterIndexing(Chapter[] chapters)
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            System.Windows.Forms.MethodInvoker method = new System.Windows.Forms.MethodInvoker(delegate
            {
                bool result = true;

                using (var transaction = new TransactionScope())
                {
                    for (int i = 0; i < chapters.Length; i++)
                    {
                        //Console.WriteLine(chapters[i].ChapterTitle);
                        if (chapters[i].Index != i)
                        {
                            result = chapters[i].ChangeIndex(i);
                            if (!result)
                                break;
                        }
                    }
                    if (result)
                    {
                        NovelLibrary.libraryData.SubmitChanges();
                        transaction.Complete();
                    }   
                }
                mre.Set();
            });

            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
            {
                BackgroundService.Instance.novelReaderForm.BeginInvoke(method);
            }
            else
            {
                method.Invoke();
            }
            
            mre.WaitOne(-1);
        }


        private void RefreshCacheData()
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            System.Windows.Forms.MethodInvoker method = new System.Windows.Forms.MethodInvoker(delegate
            {
                List<Chapter> newChapterList = (from chapter in Chapters
                                                where chapter.Index >= 0
                                                orderby chapter.Index ascending
                                                select chapter).ToList<Chapter>();

                List<int> newChapterListId = (from chapter in newChapterList
                                              select chapter.ID).ToList<int>();
                for (int i = 0; i < chapterList.Count;)
                {
                    if (newChapterListId.Contains(chapterList[i].ID))
                        i++;
                    else
                        chapterList.RemoveAt(i);
                }

                for (int i = 0; i < newChapterList.Count; i++)
                {
                    if (i < chapterList.Count && chapterList[i].ID != newChapterList[i].ID)
                        chapterList.Insert(i, newChapterList[i]);
                    else if (i >= chapterList.Count)
                        chapterList.Add(newChapterList[i]);
                }

                chapterDictionary = new Dictionary<string, Chapter>();

                foreach (Chapter c in newChapterList)
                {
                    string chapterTitle = c.ChapterTitle;
                    int dupIdx = 2;
                    //if duplicate title is found, find a unique representing string for its key.
                    while (chapterDictionary.ContainsKey(chapterTitle))
                    {
                        chapterTitle = c.ChapterTitle + "-duplicate x " + dupIdx;
                        dupIdx++;
                    }
                    //if (dupIdx > 2)
                    //    Console.WriteLine("Duplicate " + chapterTitle);
                    chapterDictionary.Add(chapterTitle, c);
                }

                isDirty = false;
                mre.Set();
            });

            if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(this))
            {
                BackgroundService.Instance.novelReaderForm.BeginInvoke(method);
            }
            else
            {
                method.Invoke();
            }
            mre.WaitOne(-1);


        }
        /*
        private NovelSource GetNovelSourceFromSource(Source s)
        {
            if (sourceDictionary == null)
                sourceDictionary = new Dictionary<string, NovelSource>();
            if (sourceDictionary.Keys.Contains(s.SourceNovelLocation))
                return sourceDictionary[s.SourceNovelLocation];
            else
            {
                SourceLocation location = (SourceLocation)Enum.Parse(typeof(SourceLocation), s.SourceNovelLocation);
                NovelSource source = SourceManager.GetSource(location, s.SourceNovelID, s.NovelTitle);
                sourceDictionary.Add(s.SourceNovelLocation, source);
                return source;
            }
            
        }
        */
    }
}
