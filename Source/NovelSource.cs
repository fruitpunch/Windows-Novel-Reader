using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
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
        Tuple<string, string>[] GetMenuURLs(); //Returns an array of tuple of chapter title and url. 
        string[] GetChapterContent(string chapterTitle, string url); //Returns the novel content of the URL.
    }
}