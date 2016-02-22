using Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelReader
{
    public partial class Source : INovelSource
    {
        private INovelSource source = null;

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

        public SourceLocation SourceLocation
        {
            get{
                if (source == null)
                    source = SourceManager.GetSource(SourceNovelLocation, SourceNovelID);
                return source.SourceLocation;
            }
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
