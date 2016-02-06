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

        
        /*============Properties============*/
        
        public ChapterUrl ChapterUrl
        {
            get {
                var result = (from url in NovelLibrary.libraryData.ChapterUrls
                              where url.ChapterID == ID
                              select url);
                if (result.Any())
                    return result.First<ChapterUrl>();
                else
                    return null;
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

        /*============Getter/Setter=========*/

        public string GetAudioFileLocation()
        {
            return Path.Combine(Configuration.Instance.NovelFolderLocation, NovelTitle, "audios", Index.ToString() + "_" + Util.CleanFileTitle(ChapterTitle) + ".mp3");
        }

        public string GetTextFileLocation()
        {
            return Path.Combine(Configuration.Instance.NovelFolderLocation, NovelTitle, "texts", Index.ToString() + "_" + Util.CleanFileTitle(ChapterTitle) + ".txt");
        }

        public void SetChapterTitle(string title)
        {
            this._ChapterTitle = title;
        }

        public void SetIndex(int index)
        {
            this._Index = index;
        }

        /*============Public Function=======*/

        public void ChangeIndex(int newIndex)
        {
            //Console.WriteLine("old index: " + Index + "  new index: " + newIndex);
            if (newIndex == Index)
                return;
            string oldAudioFileLocation = GetAudioFileLocation();
            string oldTextFileLocation = GetTextFileLocation();
            this._Index = newIndex;
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
            NovelLibrary.libraryData.SubmitChanges();
        }


        /*============Private Function======*/

        private void ChangeChapterTitle(string newChapterTitle)
        {
            string oldAudioFileLocation = GetAudioFileLocation();
            string oldTextFileLocation = GetTextFileLocation();
            this._ChapterTitle = newChapterTitle;
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
            NovelLibrary.libraryData.SubmitChanges();
        }
        
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            NovelLibrary.libraryData.SubmitChanges();
        }

        partial void OnIndexChanging(int value)
        {
            Console.WriteLine("Original " + _ChapterTitle + " " + this._Index + " " + value);
            
            if (this._Index != value)
                if(Novel != null)
                    Novel.ChangeIndex(this._Index, value);
                else
                    NovelLibrary.Instance.GetNovel(NovelTitle).ChangeIndex(this._Index, value);
            else
                this._Index = value;
            //NotifyPropertyChanged("Index");
        }

        partial void OnChapterTitleChanging(string value)
        {
            ChangeChapterTitle(value);
            NovelLibrary.libraryData.SubmitChanges();
        }

    }
}
