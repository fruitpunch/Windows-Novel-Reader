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

        public void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                
                if (BackgroundService.Instance.novelReaderForm != null && BackgroundService.Instance.novelReaderForm.InvokeRequiredForNovel(Novel))
                {
                    BackgroundService.Instance.novelReaderForm.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    }));
                }
                else
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                
            }
            //NovelLibrary.libraryData.SubmitChanges();
        }

    }
}
