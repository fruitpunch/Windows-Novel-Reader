using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelReader
{
    public partial class Chapter : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        private string _chapterTitle{ get; set; }
        private string _novelTitle{ get; set; }
        private bool _hasRead { get; set; }
        private int _index { get; set; }
        private ChapterUrl _chapterUrl { get; set; }

        /*============Properties============*/

        public ChapterUrl ChapterUrl
        {
            get {
                if (_chapterUrl == null)
                {
                    var result = from chapterUrl in NovelLibrary.Instance.libraryData.ChapterUrls
                                 where chapterUrl.ChapterID == this.ID
                                 select chapterUrl;
                    if (result.Any())
                        this._chapterUrl = (ChapterUrl)result;
                    else
                        return null;
                }
                return _chapterUrl;
            }

        }

        public bool HasAudio
        {
            get { return File.Exists(GetAudioFileLocation()); }
        }

        public bool HasText
        {
            get { return File.Exists(GetTextFileLocation()); }
        }

        /*============Constructor===========*/

        public Chapter(string chapterTitle, string novelTitle, bool hasRead = false, int index = -1)
        {
            this._chapterTitle = chapterTitle;
            this._novelTitle = novelTitle;
            this._hasRead = hasRead;
            this._index = index;
        }

        /*============Getter/Setter=========*/

        public string GetAudioFileLocation()
        {
            return Path.Combine(Configuration.Instance.NovelFolderLocation, _novelTitle, "audios", _index.ToString() + "_" + Util.CleanFileTitle(_chapterTitle) + ".mp3");
        }

        public string GetTextFileLocation()
        {
            return Path.Combine(Configuration.Instance.NovelFolderLocation, _novelTitle, "texts", _index.ToString() + "_" + Util.CleanFileTitle(_chapterTitle) + ".txt");
        }

        /*============Public Function=======*/

        public void ChangeIndex(int newIndex)
        {
            Console.WriteLine("old index: " + _index + "  new index: " + newIndex);
            if (newIndex == this._index)
                return;
            string oldAudioFileLocation = GetAudioFileLocation();
            string oldTextFileLocation = GetTextFileLocation();
            this._index = newIndex;
            string newAudioFileLocation = GetAudioFileLocation();
            string newTextFileLocation = GetTextFileLocation();

            if (File.Exists(oldAudioFileLocation))
            {
                if (!File.Exists(newAudioFileLocation))
                    File.Move(oldAudioFileLocation, newAudioFileLocation);
                else
                {
                    Console.WriteLine("Cannot move " + oldAudioFileLocation + " to " + newAudioFileLocation + ". " + newAudioFileLocation + " already exists.");
                    return;
                }
            }

            if (File.Exists(oldTextFileLocation))
            {
                if (!File.Exists(newTextFileLocation))
                    File.Move(oldTextFileLocation, newTextFileLocation);
                else
                {
                    Console.WriteLine("Cannot move " + oldTextFileLocation + " to " + newTextFileLocation + ". " + newTextFileLocation + " already exists.");
                    return;
                }
            }
            NovelLibrary.Instance.db.Store(this);
        }


        /*============Private Function======*/

        private void ChangeChapterTitle(string newChapterTitle)
        {
            string oldAudioFileLocation = GetAudioFileLocation();
            string oldTextFileLocation = GetTextFileLocation();
            this._chapterTitle = newChapterTitle;
            string newAudioFileLocation = GetAudioFileLocation();
            string newTextFileLocation = GetTextFileLocation();

            if (File.Exists(oldAudioFileLocation))
            {
                if (!File.Exists(newAudioFileLocation))
                    File.Move(oldAudioFileLocation, newAudioFileLocation);
                else
                {
                    Console.WriteLine("Cannot move " + oldAudioFileLocation + " to " + newAudioFileLocation + ". " + newAudioFileLocation + " already exists.");
                    return;
                }
            }


            if (File.Exists(oldTextFileLocation))
            {
                if (!File.Exists(newTextFileLocation))
                    File.Move(oldTextFileLocation, newTextFileLocation);
                else
                {
                    Console.WriteLine("Cannot move " + oldTextFileLocation + " to " + newTextFileLocation + ". " + newTextFileLocation + " already exists.");
                    return;
                }
            }
            NovelLibrary.Instance.db.Store(this);
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
