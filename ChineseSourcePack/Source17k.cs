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
        private static volatile object resourceLock;

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

        public void DownloadNovelCoverImage(string destination)
        {
            string url = BaseURL + "/book/" + NovelID + ".html";
            string[] lines = WebUtil.GetUrlContentsUTF8(url);
            if (lines == null)
                return;
            
            foreach (string line in lines)
            {
                if (line.Contains("<img itemprop=\"image\" src="))
                {
                    MatchCollection imageLineMatch = Regex.Matches(line, "\"([^\"]*)\"");
                    if (imageLineMatch.Count != 5)
                        continue;
                    string imageUrl = imageLineMatch[3].ToString();
                    if(imageUrl.Contains(NovelID))
                    {
                        imageUrl = imageUrl.Replace("\"", "");
                        WebUtil.DownloadImage(imageUrl, destination);
                    }
                        
                    break;
                }
            }
        }

        private Dictionary<string, string> replaceRegex = new Dictionary<string, string>()
            {
                {"<br>", "\n"}
            };

        public Source17k(string novelID, string novelTitle)
        {
            this._novelID = novelID;
            this._novelTitle = novelTitle;
            cultureInfo = new CultureInfo("zh-CN", false);
            if (resourceLock == null)
                resourceLock = new object();
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

            string chapterMatchingSubstring = "<a target=\"_blank\" href=\"/chapter/" + _novelID.ToString();
            //Console.WriteLine(chapterMatchingSubstring);
            string title, chURL, line;
            MatchCollection matches;
            for (int currentLineNumber = 0; currentLineNumber < lines.Length; currentLineNumber++)
            {
                if (lines[currentLineNumber].Contains(chapterMatchingSubstring))
                {
                    matches = Regex.Matches(lines[currentLineNumber], "\"([^\"]*)\"");
                    chURL = matches[1].ToString();
                    chURL = chURL.Replace("\"", "");
                    matches = Regex.Matches(lines[currentLineNumber + 1], "章节名称：(.*?)&#13;");
                    title = matches[0].ToString();
                    title = title.Replace("章节名称：", "").Replace("&#13;", "");

                    if (lines[currentLineNumber-1].Contains("icon_vip.png"))
                        chapterURLs.Add(new ChapterSource(chURL, title, true));
                    else
                        chapterURLs.Add(new ChapterSource(chURL, title, false));
                }
            }
            return chapterURLs.ToArray();
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            string[] lines;
            lock (resourceLock)
            {
                lines = WebUtil.GetUrlContentsUTF8(BaseURL + url);
            }
            if (lines == null)
                return null;

            List<string> novelContent = new List<string>();
            novelContent.Add(chapterTitle);
            novelContent.Add("\n\n");
            int currentLineNumber = 0;
            foreach (string line in lines)
            {
                if (line.Contains("<div id=\"chapterContentWapper\">"))
                {
                    novelContent.Add(NovelContentCleanup(lines[currentLineNumber+1]));
                    break;
                }
                    
                currentLineNumber++;
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
