using Source;
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

        public int GetHash()
        {
            string s = NovelTitle + ChapterTitle + Index;
            return s.GetHashCode();
        }

        /*============Public Function=======*/

        public bool ChangeIndex(int newIndex)
        {
            //Console.WriteLine("old index: " + Index + "  new index: " + newIndex);
            if (newIndex == Index)
                return true;
            string oldAudioFileLocation = GetAudioFileLocation();
            string oldTextFileLocation = GetTextFileLocation();
            Index = newIndex;
            string newAudioFileLocation = GetAudioFileLocation();
            string newTextFileLocation = GetTextFileLocation();

            if (File.Exists(oldAudioFileLocation))
            {
                if (!File.Exists(newAudioFileLocation))
                    File.Move(oldAudioFileLocation, newAudioFileLocation);
                else
                {
                    Console.WriteLine("Cannot move " + oldAudioFileLocation + " to " + newAudioFileLocation + ". " + newAudioFileLocation + " already exists.");
                    return false;
                }
            }

            if (File.Exists(oldTextFileLocation))
            {
                if (!File.Exists(newTextFileLocation))
                    File.Move(oldTextFileLocation, newTextFileLocation);
                else
                {
                    Console.WriteLine("Cannot move " + oldTextFileLocation + " to " + newTextFileLocation + ". " + newTextFileLocation + " already exists.");
                    return false;
                }
            }
            NotifyPropertyChanged("Index");
            return true;
        }

        public void ChangeChapterTitle(string newChapterTitle)
        {
            if (ChapterTitle == newChapterTitle)
                return;
            string oldAudioFileLocation = GetAudioFileLocation();
            string oldTextFileLocation = GetTextFileLocation();
            ChapterTitle = newChapterTitle;
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
            //NovelLibrary.libraryData.SubmitChanges();
        }

    }
}
