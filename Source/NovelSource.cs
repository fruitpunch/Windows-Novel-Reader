using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public struct ChapterSource
    {
        private string _url { get; set; }
        private string _title { get; set; }
        private bool _vip { get; set; }
        private int _urlHash { get; set; }

        public string Url
        {
            get { return this._url; }
        }

        public string Title
        {
            get { return this._title; }
        }

        public bool Vip
        {
            get { return this._vip; }
        }

        public int UrlHash
        {
            get { return this._urlHash; }
        }

        public ChapterSource(string url, string title, bool vip)
        {
            this._url = url;
            this._title = title;
            this._vip = vip;
            this._urlHash = url.GetHashCode();
        }

    }

    public interface NovelSource
    {
        //Returns the SourceLocation of the type extending this class.
        SourceLocation SourceLocation
        {
            get;
        }
        //Returns the novel ID.
        string NovelID
        {
            get;
        }
        //Returns the Novel Title.
        string NovelTitle
        {
            get;
            set;
        }
        string NovelLanguage
        {
            get;
        }

        Tuple<bool, string> VerifySource(); //Checks the ID of the source. Return true and the novel title if the novel found is valid.
        ChapterSource[] GetMenuURLs(); //Returns an array of tuple of chapter title and url. 
        string[] GetChapterContent(string chapterTitle, string url); //Returns the novel content of the URL.
    }
}