﻿using System;
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
    public class Request : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static int count = 0;

        private string _voice { get; set; }
        private int _id { get; set; }
        private string _replacementFile { get; set; }
        private string _deletetionFile { get; set; }
        private int _rate { get; set; }
        private int _priority { get; set; }
        private int _progress { get; set; }
        private int _takenBy { get; set; }
        private int _pos { get; set; }
        private Chapter chapter;

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
            get { return this.chapter.NovelTitle; }
        }

        public string ChapterTitle
        {
            get { return this.chapter.ChapterTitle; }
        }

        public int ChapterIndex
        {
            get { return this.chapter.Index; }
        }

        public string InputTextFile
        {
            get { return this.chapter.GetTextFileLocation(); }
        }

        public string OutputAudioFile
        {
            get { return this.chapter.GetAudioFileLocation(); }
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

        public int Priority
        {
            get { return this._priority; }
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

        public Request(string voice, Chapter chapter, string replacementFile = null, string deletetionFile = null, int rate = 0, int priority = 0)
        {
            this._voice = voice;
            this._id = count++;
            this.chapter = chapter;
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

    public class TTSCompleteEventArgs : EventArgs
    {

    }

    public delegate void TTSCompleteEventHandler(Object sender, TTSCompleteEventArgs e);

    public class Scheduler
    {
        public event TTSCompleteEventHandler ttsCompleteEventHandler;

        private BindingList<Request> _requestList { get; set; }
        private int _setThreadCount { get; set; }
        private Object requestListLock;
        private Thread backgroundThread;
        private Thread[] synthesizingThread;
        private ManualResetEvent idleMRE;
        private bool shutDown;
        public static readonly int MAX_THREAD_COUNT = 4;

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

        public Scheduler(int threadCount)
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

        public int GetHighestPriority()
        {
            if (_requestList.Count > 0)
                return _requestList[0].Priority;
            return 0;
        }

        public void AddRequest(Request request)
        {
            int i = 0;
            for (; i < _requestList.Count; i++)
            {
                if (request.Priority > _requestList[i].Priority && request.ID < _requestList[i].ID && _requestList[i].TakenBy == -1)
                    break;
            }
            //request.Position = i;
            ManualResetEvent mre = new ManualResetEvent(false);
            lock (requestListLock)
            {
                if (BackgroundService.Instance.ttsController.InvokeRequired)
                {
                    BackgroundService.Instance.ttsController.BeginInvoke(new MethodInvoker(delegate
                    {
                        _requestList.Insert(i, request);
                        mre.Set();
                    }));
                }
            }
            mre.WaitOne(-1);
            idleMRE.Set();
        }

        public void StartTTSService()
        {
            if (backgroundThread.IsAlive)
                return;
            backgroundThread = new Thread(Run);
            backgroundThread.Start();
        }

        public void ShutdownService()
        {
            shutDown = true;
            idleMRE.Set();
        }

        private void Run()
        {
            int threadCount = 0;

            //while (_requestList.Count > 0)
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
                else
                {
                    Thread.Sleep(1000);
                }
                threadCount = ActiveThreadCount();
                if (_requestList.Count == 0)
                {
                    TTSComplete();
                    idleMRE.Reset();
                    idleMRE.WaitOne(-1);
                }
            }
        }

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
            } while (threadId < _setThreadCount && !shutDown);
        }

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

        private void RemoveTop(Request request)
        {
            lock (requestListLock)
            {
                if (BackgroundService.Instance.ttsController.InvokeRequired)
                {
                    BackgroundService.Instance.ttsController.BeginInvoke(new MethodInvoker(delegate
                    {
                        _requestList.Remove(request);
                    }));
                }
                
            }
        }

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

        private void LaunchSubProcess(Request request)
        {
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
                if (BackgroundService.Instance.ttsController.InvokeRequired)
                {
                    BackgroundService.Instance.ttsController.BeginInvoke(new MethodInvoker(delegate
                    {
                        int percent;
                        if(Int32.TryParse(line, out percent))
                        {
                            request.Progress = percent;
                        }
                    }));
                }
            }
            p.WaitForExit();
        }

        private void TTSComplete()
        {
            RaiseTTSCompleteEvent(new TTSCompleteEventArgs());
        }

        protected virtual void RaiseTTSCompleteEvent(TTSCompleteEventArgs e)
        {
            TTSCompleteEventHandler handler = ttsCompleteEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}