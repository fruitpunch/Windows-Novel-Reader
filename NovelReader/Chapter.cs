using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelReader
{
    public class Chapter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _chapterTitle{ get; set; }
        private string _novelTitle{ get; set; }
        private bool _hasRead { get; set; }
        private int _index { get; set; }

        /*============Properties============*/

        public string ChapterTitle
        {
            get { return this._chapterTitle; }
            set {
                if (this._chapterTitle != value)
                    ChangeChapterTitle(value);
                else
                    this._chapterTitle = value;
                NotifyPropertyChanged("ChapterTitle");
            }
        }

        public string NovelTitle
        {
            get { return this._novelTitle; }
        }

        public bool Read
        {
            get { return this._hasRead; }
            set { 
                this._hasRead = value;
                NotifyPropertyChanged("Read");
            }
        }

        public int Index
        {
            get { return this._index; }
            set {
                if (this._index != -1 && this._index != value)
                    NovelLibrary.Instance.GetNovel(_novelTitle).ChangeIndex(this._index, value);
                else
                    this._index = value;
                NotifyPropertyChanged("Index");
            }
        }

        public bool HasAudio
        {
            get { return File.Exists(GetAudioFileLocation()); }
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
