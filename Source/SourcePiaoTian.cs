using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Source
{
    public class SourcePiaoTian : Source
    {

        string baseURL = "http://www.piaotian.cc";
        string novelTitle;
        int novelID;
        CultureInfo cultureInfo;

        public SourceLocation SourceLocation
        {
            get { return SourceLocation.WebPiaoTian; }
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

        public SourcePiaoTian(string novelTitle, int novelID)
        {
            this.novelTitle = novelTitle;
            this.novelID = novelID;
            cultureInfo = new CultureInfo("en-US", false);
            //Console.WriteLine("Piao Tian");
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

            string url = baseURL + "/read/" + novelID.ToString() + "/index.html";
            string chapterMatchingSubstring = "<a href=\"/read/" + novelID.ToString() + "/";
            string[] lines = WebUtil.GetUrlContents(url);
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
                    chapterURLs.Add(new Tuple<string, string>(title, chURL));
                }

            }
            return chapterURLs.ToArray();
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            string[] lines = WebUtil.GetUrlContents(baseURL + url);
            List<string> novelContent = new List<string>();
            bool contentFound = false;
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
            content = content.ToLower(cultureInfo);
            return content;
        }
    }
}
