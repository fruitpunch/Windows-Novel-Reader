using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Source
{
    public class SourceWuxiaWorld : NovelSource
    {
        public string BaseURL = "http://www.wuxiaworld.com";
        string _novelTitle;
        string _novelID;
        CultureInfo cultureInfo;

        public SourceLocation SourceLocation
        {
            get { return SourceLocation.WebWuxiaWorld; }
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
            get { return "en-US"; }
        }

        private Dictionary<string, string> replaceRegex = new Dictionary<string, string>()
            {
                {"</strong>", ""},
                {"<strong>", ""},
                {"<p>", "\n"},
                {"</p>", "\n"},
                {"?&#8230;", ""},
                {"Previous Chapter", ""},
                {"Next Chapter", ""}
            };

        public SourceWuxiaWorld(string novelID, string novelTitle)
        {
            this._novelID = novelID;
            this._novelTitle = novelTitle;
            this.cultureInfo = new CultureInfo("en-US", false);
        }

        public Tuple<bool, string> VerifySource()
        {
            string url = BaseURL + "/" + _novelID.ToString() + "/";
            string[] lines = WebUtil.GetUrlContentsUTF8(url);
            
            if (lines == null)
                return new Tuple<bool, string>(false, "");
            string title = null;
            
            foreach (string line in lines)
            {
                if (line.Contains("nothing was found"))
                    return new Tuple<bool, string>(false, "");
            }
            foreach (string line in lines)
            {
                if (line.Contains("<title>"))
                {
                    title = line.Replace("<title>", "");
                    title = title.Replace(" &#8211; Index &#8211; Wuxiaworld</title>", "");
                    break;
                }
            }
            NovelTitle = title;
            return new Tuple<bool, string>(true, title);
        }

        public ChapterSource[] GetMenuURLs()
        {
            List<ChapterSource> chapterURLs = new List<ChapterSource>();

            string url = BaseURL + "/" + _novelID.ToString();
            string chapterMatchingSubstring = url;
            string[] lines = WebUtil.GetUrlContentsUTF8(url);
            if (lines == null)
                return null;

            string title, chURL;
            foreach (string line in lines)
            {
                
                if (line.Contains(chapterMatchingSubstring))
                {
                    
                    MatchCollection quoteMatch = Regex.Matches(line, "<a [^>]*href=(?:'(?<href>.*?)')|(?:\"(?<href>.*?)\")");
                    foreach (var match in quoteMatch)
                    {
                        if (match.ToString().Contains("http://www.wuxiaworld.com/" + _novelID))
                        {
                            chURL = match.ToString();
                            chURL = chURL.Replace("\"", "");
                            if (chURL.Contains("#") || chURL.Length == chapterMatchingSubstring.Length || chURL.Contains("feed") || chURL.Contains("glossary"))
                                continue;
                            title = "";
                            
                            if (chURL[chURL.Length - 1] == '/')
                                chURL = chURL.Substring(0, chURL.Length - 1);

                            title = chURL.Split('/').Last();
                            chURL = chURL.Replace("http://www.wuxiaworld.com", "");
                            //Console.WriteLine(match.ToString().Replace("\"", "") + "  |  " + chURL + "  |  " + title);
                            chapterURLs.Add(new ChapterSource(chURL, title, false));
                        }
                    }
                }

            }
            return chapterURLs.ToArray();
        }

        public string[] GetChapterContent(string chapterTitle, string url)
        {
            string[] lines = WebUtil.GetUrlContentsUTF8(BaseURL + url);
            if (lines == null)
                return null;
            Console.WriteLine(chapterTitle + " " + url);
            List<string> novelContent = new List<string>();
            bool contentFound = false;
            bool startFound = false;
            //Console.WriteLine("length: " + lines.Length);
            novelContent.Add(chapterTitle);
            novelContent.Add("\n\n");
            int contentAmount = 0;
            foreach (string line in lines)
            {
                //Console.WriteLine(line);
                if (line.Contains("<hr>") || line.Contains("<hr/>"))
                {
                    startFound = true;
                    //Console.WriteLine("began HR found");
                }
                if (startFound)
                {
                    if (line.Contains("<p>") || line.Contains("</p>"))
                    {
                        novelContent.Add(NovelContentCleanup(line));
                        contentFound = true;
                        contentAmount++;
                    }
                }
                if (contentFound && contentAmount > 5)
                {
                    if (line.Contains("<hr>") || line.Contains("<hr/>"))
                    {
                        //Console.WriteLine("ending HR found");
                        break;
                    }
                }
            }
            return novelContent.ToArray();
        }

        private string NovelContentCleanup(string content)
        {
            foreach (KeyValuePair<string, string> entry in replaceRegex)
                content = content.Replace(entry.Key, entry.Value);
            content = Regex.Replace(content, @"<[^>]+>|&nbsp;", "");
            return content;
        }
    }
}
