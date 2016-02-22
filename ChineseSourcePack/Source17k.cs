using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Source;

namespace ChineseSourcePack
{
    public class Source17k : ISource
    {
        private string BaseURL = "http://www.17k.com";
        private string _novelTitle;
        private string _novelID;
        private CultureInfo cultureInfo;

        public string NovelID
        {
            get { return this._novelID; }
        }

        public string NovelTitle
        {
            get { return this._novelTitle; }
            set { this._novelTitle = value; }
        }

        public string NovelLanguage
        {
            get { return "zh-CN"; }
        }

        public string Url
        {
            get{ return BaseURL; }
        }

        public string SourceNovelLocation
        {
            get { return this.GetType().FullName; }
        }

        private Dictionary<string, string> replaceRegex = new Dictionary<string, string>()
            {

            };

        public Source17k(string novelID, string novelTitle)
        {
            this._novelID = novelID;
            this._novelTitle = novelTitle;
            cultureInfo = new CultureInfo("zh-CN", false);
        }

        public Tuple<bool, string> VerifySource()
        {
            string url = BaseURL + "/book/" + _novelID.ToString() + ".html";
            string[] lines = WebUtil.GetUrlContentsUTF8(url);
            if (lines == null)
                return new Tuple<bool, string>(false, "");
            string title = null;
            foreach (string line in lines)
            {
                if (line.Contains("您的访问出错了"))
                    return new Tuple<bool, string>(false, null);
            }
            foreach (string line in lines)
            {
                if (line.Contains("var bookName"))
                {
                    MatchCollection titleMatch = Regex.Matches(line, "\"([^\"]*)\"");
                    title = titleMatch[0].ToString();
                    title = title.Replace("\"", "");
                    break;
                }
            }
            NovelTitle = title;
            return new Tuple<bool, string>(true, title);
        }

        public ChapterSource[] GetMenuURLs()
        {
            List<ChapterSource> chapterURLs = new List<ChapterSource>();

            string url = BaseURL + "/list/" + _novelID.ToString() + ".html";
            string[] lines = WebUtil.GetUrlContentsUTF8(url);
            if (lines == null)
                return null;

            string chapterMatchingSubstring = "href=\"/chapter/" + _novelID.ToString() + "/";
            string title, chURL;
            foreach (string line in lines)
            {
                
                if (line.Contains(chapterMatchingSubstring))
                {
                    MatchCollection matches = Regex.Matches(line, "\"([^\"]*)\"");
                    title = matches[2].ToString();
                    chURL = matches[5].ToString();
                    title = title.Replace("\"", "");
                    chURL = chURL.Replace("\"", "");
                    chapterURLs.Add(new ChapterSource(chURL, title, true));
                }
            }
            return chapterURLs.ToArray();
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            string[] lines = WebUtil.GetUrlContentsUTF8(BaseURL + url);
            if (lines == null)
                return null;

            List<string> novelContent = new List<string>();
            novelContent.Add(chapterTitle);
            novelContent.Add("\n\n");
            foreach (string line in lines)
            {
                if (line.Contains("&nbsp;"))
                    novelContent.Add(NovelContentCleanup(line));
            }
            return novelContent.ToArray();
        }

        public string MakeFileName(string title, int index)
        {
            Regex InvalidFileSymbol = new Regex("[\t\r \\/:*?\"<>]");
            string newTitle = InvalidFileSymbol.Replace(title, "");
            return newTitle;
        }

        //Clean up each line of the novel content.
        private string NovelContentCleanup(string content)
        {
            foreach (KeyValuePair<string, string> entry in replaceRegex)
                content = content.Replace(entry.Key, entry.Value);
            content = Regex.Replace(content, @"<[^>]+>|&nbsp;", "");
            content = Regex.Replace(content, @"\b(?:https?://|www\.)\S+\b", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //content = linkParser.Replace(content, "");
            return content;
        }
    }
}
