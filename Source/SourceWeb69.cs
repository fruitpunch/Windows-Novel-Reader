using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Source
{
    public class SourceWeb69 : Source
    {
        string baseURL = "http://www.69shu.com";
        string novelTitle;
        int novelID;
        CultureInfo cultureInfo;

        public SourceLocation SourceLocation
        {
            get { return SourceLocation.Web69; }
        }

        public int NovelID
        {
            get { return this.novelID; }
        }

        private Dictionary<string, string> replaceRegex = new Dictionary<string, string>()
            {
                {"<script>txttopshow7();</script><!--章节内容结束-->", ""},
                {"&nbsp;", ""},
                {"<!--章节内容开始-->", ""},
                {"<br />", "\n"}
            };

        public SourceWeb69(string novelTitle, int novelID)
        {
            this.novelTitle = novelTitle;
            this.novelID = novelID;
            cultureInfo = new CultureInfo("en-US", false);
        }

        public string GetNovelTitle()
        {
            throw new NotImplementedException();
        }

        public bool IsValidID(string novelTitle, int sourceID)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, string>[] GetMenuURLs()
        {
            List<Tuple<string, string>> chapterURLs = new List<Tuple<string, string>>();

            string url = baseURL + "/" + novelID.ToString() + "/";
            string chapterMatchingSubstring = "<li><a href=\"/txt/" + novelID.ToString() + "/";
            string[] lines = WebUtil.GetUrlContents(url);
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
                    chapterURLs.Add(new Tuple<string, string>(title, chURL));
                }

            }
            chapterURLs.RemoveRange(0, 6);
            return chapterURLs.ToArray();
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            //Console.WriteLine(baseURL + url);

            string[] lines = WebUtil.GetUrlContents(baseURL + url);
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
            //content = Util.GeneralNovelContentCleanup(content);

            foreach (KeyValuePair<string, string> entry in replaceRegex)
                content = content.Replace(entry.Key, entry.Value);
            content = content.ToLower(cultureInfo);
            return content;
        }
    }
}
