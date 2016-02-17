using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Source
{
    public class SourcePiaoTian : NovelSource
    {
        public string BaseURL = "http://www.piaotian.cc";
        string _novelTitle;
        string _novelID;
        //Regex linkParser;
        CultureInfo cultureInfo;

        public SourceLocation SourceLocation
        {
            get { return SourceLocation.WebPiaoTian; }
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

        public SourcePiaoTian(string novelID, string novelTitle)
        {
            this._novelID = novelID;
            this._novelTitle = novelTitle;
            this.cultureInfo = new CultureInfo("en-US", false);
        }

        public Tuple<bool, string> VerifySource()
        {
            string url = BaseURL + "/read/" + _novelID.ToString() + "/index.html";
            string[] lines = WebUtil.GetUrlContents(url);
            if (lines == null)
                return new Tuple<bool, string>(false, "");
            string title = null;
            foreach (string line in lines)
            {
                if (line.Contains("404 - 找不到文件或目录。"))
                    return new Tuple<bool, string>(false, null);
            }
            foreach (string line in lines)
            {
                if (line.Contains("<meta name=\"keywords\" content=\""))
                {
                    title = line.Replace("<meta name=\"keywords\" content=\"", "");
                    title = title.Replace("\" />", "");
                    break;
                }
            }
            NovelTitle = title;
            return new Tuple<bool, string>(true, title);
        }

        public ChapterSource[] GetMenuURLs()
        {
            List<ChapterSource> chapterURLs = new List<ChapterSource>();

            string url = BaseURL + "/read/" + _novelID.ToString() + "/index.html";
            string chapterMatchingSubstring = "<a href=\"/read/" + _novelID.ToString() + "/";
            string[] lines = WebUtil.GetUrlContents(url);
            if (lines == null)
                return null;

            string title, chURL;
            foreach (string line in lines)
            {
                if (line.Contains(chapterMatchingSubstring))
                {
                    MatchCollection quoteMatch = Regex.Matches(line, "\"[^\"]*\"");

                    title = quoteMatch[3].ToString();
                    chURL = quoteMatch[2].ToString();
                    title = title.Replace("\"", "");
                    chURL = chURL.Replace("\"", "");
                    chapterURLs.Add(new ChapterSource(chURL, title, false));
                }

            }
            return chapterURLs.ToArray();
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            string[] lines = WebUtil.GetUrlContents(BaseURL + url);
            if (lines == null)
                return null;

            List<string> novelContent = new List<string>();
            bool contentFound = false;
            //linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            novelContent.Add(chapterTitle);
            novelContent.Add("\n\n");
            foreach (string line in lines)
            {
                if (contentFound && line.Contains("<a href="))
                    break;
                if (line.Contains("&nbsp;"))
                {
                    novelContent.Add(NovelContentCleanup(line));
                    contentFound = true;
                }
            }
            return novelContent.ToArray();
        }

        private string NovelContentCleanup(string content)
        {
            foreach (KeyValuePair<string, string> entry in replaceRegex)
                content = content.Replace(entry.Key, entry.Value);
            content = Regex.Replace(content, @"<[^>]+>|&nbsp;", "");
            content = Regex.Replace(content, @"\b(?:https?://|www\.)\S+\b", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return content;
        }
    }
}
