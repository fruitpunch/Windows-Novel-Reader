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
            return Path.Combine(Configuration.Instance.NovelFolderLocation, NovelTitle, "audios", HashID + ".mp3");
        }

        public string GetTextFileLocation()
        {
            return Path.Combine(Configuration.Instance.NovelFolderLocation, NovelTitle, "texts", HashID + ".txt");
        }


        /*============Public Function=======*/

        public void ExportAudio(string destinationFolder)
        {
            string validChapterTitle = ChapterTitle;
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                validChapterTitle = validChapterTitle.Replace(invalidChar, 'X');
            }

            if (File.Exists(GetAudioFileLocation()) && Directory.Exists(destinationFolder))
            {
                File.Copy(GetAudioFileLocation(), Path.Combine(destinationFolder, Index + " - " + validChapterTitle + ".mp3"), true);
            }
        }

        public void ExportText(string destinationFolder)
        {
            string validChapterTitle = ChapterTitle;
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                validChapterTitle = validChapterTitle.Replace(invalidChar, 'X');
            }

            if (File.Exists(GetTextFileLocation()) && Directory.Exists(destinationFolder))
            {
                File.Copy(GetTextFileLocation(), Path.Combine(destinationFolder, Index + " - " + validChapterTitle + ".txt"), true);
            }
        }
    }
}
