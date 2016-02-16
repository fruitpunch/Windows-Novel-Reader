using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Source
{
    public class SourceWeb69 : NovelSource
    {
        public string BaseURL = "http://www.69shu.com";
        string _novelTitle;
        string _novelID;
        //Regex linkParser;
        CultureInfo cultureInfo;

        public SourceLocation SourceLocation
        {
            get { return SourceLocation.Web69; }
        }

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

        private Dictionary<string, string> replaceRegex = new Dictionary<string, string>()
            {
                {"<script>txttopshow7();</script><!--章节内容结束-->", ""},
                {"&nbsp;", ""},
                {"<!--章节内容开始-->", ""},
                {"<br />", "\n"}
            };

        public SourceWeb69(string novelID)
        {
            this._novelID = novelID;
            cultureInfo = new CultureInfo("zh-CN", false);
        }

        public Tuple<bool, string> VerifySource()
        {
            string url = BaseURL + "/" + _novelID.ToString() + "/";
            string[] lines = WebUtil.GetUrlContents(url);
            if (lines == null)
                return new Tuple<bool, string>(false, "");
            string title = null;
            foreach (string line in lines)
            {
                if (line.Contains("69书吧_404"))
                    return new Tuple<bool, string>(false, null);
            }
            foreach (string line in lines)
            {
                if (line.Contains("<div class=\"mu_h1\"><h1>"))
                {
                    title = line.Replace("\t<div class=\"mu_h1\"><h1>", "");
                    title = title.Replace("最新章节列表</h1></div>", "");
                    break;
                }
            }
            return new Tuple<bool, string>(true, title);
        }

        public ChapterSource[] GetMenuURLs()
        {
            List<ChapterSource> chapterURLs = new List<ChapterSource>();
            string url = BaseURL + "/" + _novelID.ToString() + "/";
            string chapterMatchingSubstring = "<li><a href=\"/txt/" + _novelID.ToString() + "/";
            string[] lines = WebUtil.GetUrlContents(url);
            if (lines == null)
                return null;

            string title, chURL;
            foreach (string line in lines)
            {
                if (line.Contains(chapterMatchingSubstring))
                {
                    MatchCollection titleMatch = Regex.Matches(line, @"\>(.*?)\<");
                    MatchCollection urlMatch = Regex.Matches(line, "\"([^\"]*)\"");

                    title = titleMatch[1].ToString();
                    chURL = urlMatch[0].ToString();
                    title = title.Replace(">", "").Replace("<", "");
                    chURL = chURL.Replace("\"", "");
                    chapterURLs.Add(new ChapterSource(chURL, title, false));
                }
            }
            chapterURLs.RemoveRange(0, 6);
            return chapterURLs.ToArray();
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            string[] lines = WebUtil.GetUrlContents(BaseURL + url);
            if (lines == null)
                return null;

            List<string> novelContent = new List<string>();
            //linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
