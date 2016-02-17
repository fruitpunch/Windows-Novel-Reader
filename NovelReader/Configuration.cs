using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelReader
{
    //This class saves the setting of the program.
    class Configuration : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static Configuration instance;
        private readonly static string configLocation = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NovelReader", "novel_reader.config");

        private string _novelFolderLocation { get; set; }
        private string _metaFileLocation { get; set; }
        private int _ttsThreadCount { get; set; }
        private int _updateInterval;
        private int _ttsSpeed { get; set; }
        private bool _autoPlay { get; set; }
        private bool _makeTTSForChaptersAlreadyRead { get; set; }
        private DateTime _lastFullUpdateTime { get; set; }
        private Dictionary<string, string> _langaugeVoiceDictionary { get; set; }

        private Rectangle _novelReaderFormRect { get; set; }
        private bool _novelReaderMaximized { get; set; }

        private Rectangle _applicationFormRect { get; set; }
        private bool _applicationMaximized { get; set; }

        /*============Properties============*/

        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    Console.WriteLine("Create instance of Config");
                    instance = new Configuration();
                }
                return instance;
            }
        }

        public string NovelFolderLocation
        {
            get { return this._novelFolderLocation; }
            set { this._novelFolderLocation = value; }
        }

        public string MetaFileLocation
        {
            get { return this._metaFileLocation; }
            set { this._metaFileLocation = value; }
        }

        public string CacheFolderLocation
        {
            get { return Path.Combine(NovelFolderLocation, "Cache"); }
        }

        public int TTSThreadCount
        {
            get { return this._ttsThreadCount; }
            set { this._ttsThreadCount = value; }
        }

        public int TTSSpeed
        {
            get { return this._ttsSpeed; }
            set { this._ttsSpeed = value; }
        }

        public int UpdateInterval
        {
            get { return this._updateInterval; }
            set { this._updateInterval = value; }
        }

        public bool AutoPlay
        {
            get { return this._autoPlay; }
            set { this._autoPlay = value; }
        }

        public DateTime LastFullUpdateTime
        {
            get { return this._lastFullUpdateTime; }
            set { this._lastFullUpdateTime = value; }
        }

        public Dictionary<string, string> LanguageVoiceDictionary
        {
            get { return this._langaugeVoiceDictionary; }
            set { this._langaugeVoiceDictionary = value; }
        }

        public bool MakeTTSForChapterAlreadyRead
        {
            get { return this._makeTTSForChaptersAlreadyRead; }
            set { this._makeTTSForChaptersAlreadyRead = value; }
        }

        public string DeleteSpecification
        {
            get { return "DeleteSpecification.txt"; }
        }

        public string ReplaceSpecification
        {
            get { return "ReplaceSpecification.txt"; }
        }

        public string NovelListDBName
        {
            get { return "NovelList.db"; }
        }

        public string LibraryDataName
        {
            get { return "LibraryData.mdf"; }
        }

        public Rectangle NovelReaderRect
        {
            get { return this._novelReaderFormRect; }
            set { this._novelReaderFormRect = value; }
        }

        public bool NovelReaderMaximized
        {
            get { return this._novelReaderMaximized; }
            set { this._novelReaderMaximized = value; }
        }

        public Rectangle ApplicationRect
        {
            get { return this._applicationFormRect; }
            set { this._applicationFormRect = value; }
        }

        public bool ApplicationMaximized
        {
            get { return this._applicationMaximized; }
            set { this._applicationMaximized = value; }
        }

        /*============Constructor===========*/

        private Configuration()
        {
        }

        public static void LoadConfiguration()
        {
            bool loadSuccessful = false;
            if (File.Exists(configLocation))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(configLocation)))
                {
                    try
                    {
                        string json = reader.ReadLine();
                        instance = JsonConvert.DeserializeObject<Configuration>(json);
                        loadSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Configuration Loading Error", e.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if(!loadSuccessful)
            {
                Directory.CreateDirectory(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NovelReader"));
                Configuration.Instance.init();
            }
        }

        public static void SaveConfiguration()
        {
            string output = JsonConvert.SerializeObject(instance);
            try
            {
                System.IO.File.WriteAllText(configLocation, output);
            }
            catch (Exception e)
            {
                MessageBox.Show("Configuration Saving Error", e.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void init()
        {
            SetNovelFolderLocation();
            this._ttsThreadCount = 0;
            this._ttsSpeed = 0;
            this._updateInterval = 300000;
            this._applicationFormRect = new Rectangle(0, 0, 1075, 788);
            this._novelReaderFormRect = new Rectangle(0, 0, 1200, 768);
        }

        /*============Public Function=======*/

        public void SetNovelFolderLocation()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this._novelFolderLocation = fbd.SelectedPath;
                this._metaFileLocation = Path.Combine(_novelFolderLocation, "meta.json");
            }
        }

        /*============Private Function======*/

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
