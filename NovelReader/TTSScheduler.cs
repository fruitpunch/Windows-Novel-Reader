using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelReader
{
    //Represent an TTS request with the necessary information to create an audio file.
    public class Request : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static int count = 0;

        private string _voice { get; set; }
        private int _id { get; set; }
        private string _replacementFile { get; set; }
        private string _deletetionFile { get; set; }
        private int _rate { get; set; }
        private double _priority { get; set; }
        private int _progress { get; set; }
        private int _takenBy { get; set; }
        private int _pos { get; set; }
        private Chapter _chapter;

        public string Voice
        {
            get { return this._voice; }
        }

        public int ID
        {
            get { return this._id; }
        }

        public string NovelTitle
        {
            get { return this._chapter.NovelTitle; }
        }

        public string ChapterTitle
        {
            get { return this._chapter.ChapterTitle; }
        }

        public int ChapterIndex
        {
            get { return this._chapter.Index; }
        }

        public string InputTextFile
        {
            get { return this._chapter.GetTextFileLocation(); }
        }

        public string OutputAudioFile
        {
            get { return this._chapter.GetAudioFileLocation(); }
        }

        public string ReplacementFile
        {
            get { return this._replacementFile; }
        }

        public string DeletionFile
        {
            get { return this._deletetionFile; }
        }

        public int Rate
        {
            get { return this._rate; }
        }

        public double Priority
        {
            get { return this._priority; }
            set { 
                this._priority = value;
                NotifyPropertyChanged("Priority"); 
            }
        }

        public int Progress
        {
            get { return this._progress; }
            set { 
                this._progress = value; 
                NotifyPropertyChanged("Progress"); 
            }
        }

        public int TakenBy
        {
            get { return this._takenBy; }
            set { this._takenBy = value; }
        }

        public int Position
        {
            get { return this._pos; }
            set { this._pos = value; }
        }


        public Chapter Chapter
        {
            get { return this._chapter; }
        }

        public Request(string voice, Chapter chapter, string replacementFile = null, string deletetionFile = null, int rate = 0, double priority = 0)
        {
            this._voice = voice;
            this._id = count++;
            this._chapter = chapter;
            this._replacementFile = replacementFile;
            this._deletetionFile = deletetionFile;
            this._rate = rate;
            this._priority = priority;
            this._progress = 0;
            this._takenBy = -1;
            
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //Notify the other class of progress in TTS Scheduler.
    public class TTSProgressEventArgs : EventArgs
    {
        public enum ProgressType { RequestComplete, RequestRemoved};
        public Request request { get; set; }
        public ProgressType type { get; set; }
    }

    public delegate void TTSProgressEventHandler(Object sender, TTSProgressEventArgs e);

    //Schedule TTS request.
    public class TTSScheduler
    {
        public event TTSProgressEventHandler ttsProgressEventHandler;

        private BindingList<Request> _requestList { get; set; }
        private int _setThreadCount { get; set; }
        private Object requestListLock;
        private Thread backgroundThread;
        private Thread[] synthesizingThread;
        private ManualResetEvent idleMRE;
        private bool shutDown;
        public static readonly int MAX_THREAD_COUNT = 4;
        public static readonly int MAX_REQUEST_COUNT = 50;

        public BindingList<Request> RequestList
        {
            get { return this._requestList; }
        }

        public int Threads
        {
            get { return this._setThreadCount; }
            set {
                if (value < 0)
                    return;
                this._setThreadCount = value; 
            }
        }

        public int RequestCount
        {
            get { return this._requestList.Count; }
        }

        public TTSScheduler(int threadCount)
        {
            this._requestList = new BindingList<Request>();
            this.requestListLock = new Object();
            this._setThreadCount = threadCount;
            this.synthesizingThread = new Thread[MAX_THREAD_COUNT];
            this.backgroundThread = new Thread(Run);
            this.idleMRE = new ManualResetEvent(false);
            this.shutDown = false;
            Console.WriteLine("TTS Initiated");
        }

        public double GetHighestPriority()
        {
            double max = Double.MinValue;
            foreach (Request r in _requestList)
            {
                if (r.Priority > max)
                    max = r.Priority;
            }
            return max;
        }

        public void AddRequest(Request request)
        {
            int i = 0;
            for (; i < _requestList.Count; i++)
            {
                if (request.Priority > _requestList[i].Priority && _requestList[i].TakenBy == -1)
                    break;
            }
            //request.Position = i;
            
            lock (requestListLock)
            {
                if (BackgroundService.Instance.ttsController != null && BackgroundService.Instance.ttsController.InvokeRequired)
                {
                    ManualResetEvent mre = new ManualResetEvent(false);
                    BackgroundService.Instance.ttsController.BeginInvoke(new MethodInvoker(delegate
                    {
                        _requestList.Insert(i, request);
                        if (_requestList.Count > MAX_REQUEST_COUNT)
                        {
                            Request removeRequest = _requestList.Last();
                            _requestList.Remove(removeRequest);
                            TTSProgress(removeRequest, TTSProgressEventArgs.ProgressType.RequestRemoved);
                        }
                            
                        mre.Set();
                    }));
                    mre.WaitOne(-1);
                }
                else
                {
                    _requestList.Insert(i, request);
                    if (_requestList.Count > MAX_REQUEST_COUNT)
                    {
                        Request removeRequest = _requestList.Last();
                        _requestList.Remove(removeRequest);
                        TTSProgress(removeRequest, TTSProgressEventArgs.ProgressType.RequestRemoved);
                    }
                }
            }

            
            
            idleMRE.Set();
        }

        //Clear all request not being processed.
        public void ClearRequests()
        {
            lock (requestListLock)
            {
                if (BackgroundService.Instance.ttsController!= null && BackgroundService.Instance.ttsController.InvokeRequired)
                {
                    BackgroundService.Instance.ttsController.BeginInvoke(new MethodInvoker(delegate
                    {
                        var requestArray = _requestList.ToArray();
                        foreach (Request r in requestArray)
                        {
                            if (r.TakenBy == -1)
                                _requestList.Remove(r);
                        }
                    }));
                }
                else
                {
                    var requestArray = _requestList.ToArray();
                    foreach (Request r in requestArray)
                    {
                        if (r.TakenBy == -1)
                            _requestList.Remove(r);
                    }
                }
            }
        }

        //Initiate the TTS Service thread.
        public void StartTTSService()
        {
            if (backgroundThread.IsAlive)
                return;
            backgroundThread = new Thread(Run);
            backgroundThread.Start();
        }

        //Shuts downt he TTS Service thread.
        public void ShutdownService()
        {
            shutDown = true;
            idleMRE.Set();
        }

        //Runs the TTS thread. Create the specified number of sub thread where each thread runs a speech synthesizer.
        private void Run()
        {
            int threadCount = 0;

            while (!shutDown)
            {
                if (threadCount < _setThreadCount && UntakenRequestCount() > 0)
                {
                    for (int i = 0; i < _setThreadCount; i++)
                    {
                        if (synthesizingThread[i] == null || !synthesizingThread[i].IsAlive)
                        {
                            synthesizingThread[i] = new Thread(new ParameterizedThreadStart(DoWork));
                            synthesizingThread[i].Start(i);
                            Thread.Sleep(100);
                            break;
                        }
                    }
                }
                else if(UntakenRequestCount() == 0)
                {
                    idleMRE.Reset();
                    idleMRE.WaitOne(-1);
                }
                else
                {
                    idleMRE.Reset();
                    idleMRE.WaitOne(1000);
                }
                threadCount = ActiveThreadCount();
            }
        }

        //Each sub thread do speech synthesis.
        private void DoWork(Object threadContext)
        {
            int threadId = (int)threadContext;
            Request request;
            do
            {
                request = GetTopRequest(threadId);
                if (request == null)
                    break;
                LaunchSubProcess(request);
                RemoveTop(request);
                TTSProgress(request, TTSProgressEventArgs.ProgressType.RequestComplete);
            } while (threadId < _setThreadCount && !shutDown);
        }

        //Get the top request not taken by a thread.
        private Request GetTopRequest(int threadID)
        {
            Request request = null;
            lock (requestListLock)
            {
                for (int i = 0; i < _requestList.Count; i++)
                {
                    if (_requestList[i].TakenBy == -1)
                    {
                        request = _requestList[i];
                        request.TakenBy = threadID;
                        break;
                    }
                }
                
            }
            return request;
        }

        //Remove the top request after it has been finished.
        private void RemoveTop(Request request)
        {
            lock (requestListLock)
            {
                if (BackgroundService.Instance.ttsController.InvokeRequired)
                {
                    BackgroundService.Instance.ttsController.BeginInvoke(new MethodInvoker(delegate
                    {
                        _requestList.Remove(request);
                        for (int i = 0; i < _requestList.Count; i++)
                        {
                            _requestList[i].Priority++;
                        }
                    }));
                }
                
            }
        }

        //Returns the number active sub thread.
        private int ActiveThreadCount()
        {
            int count = 0;
            for (int i = 0; i < MAX_THREAD_COUNT; i++)
            {
                if (synthesizingThread[i] != null && synthesizingThread[i].IsAlive)
                    count++;
            }
            return count;
        }

        //Get number of request that is waiting in queue.
        private int UntakenRequestCount()
        {
            int count = 0;
            lock (requestListLock)
            {
                for (int i = 0; i < _requestList.Count; i++)
                {
                    if (_requestList[i].TakenBy == -1)
                        count++;
                }
            }
            return count;
        }

        //Launch an TTS executable with all the request's information.
        private void LaunchSubProcess(Request request)
        {
            string specifiedOutputAudioLocation = request.OutputAudioFile;
            string args = String.Format(" \"{0}\" -i \"{1}\" -o \"{2}\" -rate {3} -replace \"{4}\" -delete \"{5}\" -utf8", 
                request.Voice, request.InputTextFile, request.OutputAudioFile, request.Rate, request.ReplacementFile, request.DeletionFile);
            Process p = new Process();
            p.StartInfo.FileName = Path.Combine("TTS", "TTS.exe");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            p.StartInfo.Arguments = args;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            StreamReader output = p.StandardOutput;
            string line;
            while ((line = output.ReadLine()) != null)
            {
                int percent;
                if (Int32.TryParse(line, out percent))
                {
                    if (BackgroundService.Instance.ttsController.InvokeRequired)
                    {
                        BackgroundService.Instance.ttsController.BeginInvoke(new MethodInvoker(delegate
                        {
                            request.Progress = percent;
                        }));
                    }
                }
            }
            p.WaitForExit();
            //For if user rename file while the process is running
            if (!specifiedOutputAudioLocation.Equals(request.OutputAudioFile))
                File.Copy(specifiedOutputAudioLocation, request.OutputAudioFile);

            string exportFolderLocation = Path.Combine(Configuration.Instance.AudioExportLocation, request.NovelTitle);

            if (!Directory.Exists(exportFolderLocation))
                Directory.CreateDirectory(exportFolderLocation);

            if (Configuration.Instance.NovelExport.ContainsKey(request.Chapter.Novel.NovelTitle))
            {
                Novel.ExportOption exportOption = Configuration.Instance.NovelExport[request.NovelTitle];
                if (exportOption == Novel.ExportOption.Both || exportOption == Novel.ExportOption.Audio)
                    request.Chapter.ExportAudio(exportFolderLocation);
            }

        }

        //Update the TTS progress.
        private void TTSProgress(Request request, TTSProgressEventArgs.ProgressType type)
        {
            TTSProgressEventArgs args = new TTSProgressEventArgs();
            args.request = request;
            args.type = type;
            RaiseProgressEvent(args);
        }

        protected virtual void RaiseProgressEvent(TTSProgressEventArgs e)
        {
            TTSProgressEventHandler handler = ttsProgressEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}