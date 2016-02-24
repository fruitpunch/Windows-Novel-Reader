using Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelReader
{
    public partial class Source : ISource
    {
        private ISource source = null;

        public string NovelID
        {
            get{
                if(source == null)
                    source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
                return source.NovelID;
            }
        }

        public string NovelLanguage
        {
            get {
                if (source == null)
                    source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
                return source.NovelLanguage;
            }
        }

        public string Url
        {
            get
            {
                if (source == null)
                    source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
                return source.Url;
            }
        }

        public void DownloadNovelCoverImage(string destination)
        {
            if (source == null)
                source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
            source.DownloadNovelCoverImage(destination);
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            if (source == null)
                source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
            return source.GetChapterContent(chapterTitle, url);
        }

        public ChapterSource[] GetMenuURLs()
        {
            if (source == null)
                source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
            return source.GetMenuURLs();
        }

        public Tuple<bool, string> VerifySource()
        {
            if (source == null)
                source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
            return source.VerifySource();
        }

        partial void OnLoaded()
        {
            source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
        }
    }
}
