using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public interface Source
    {
        string GetNovelTitle(); //Return title of the novel.
        bool IsValidID(string novelTitle, int sourceID); //Check and see if the title and souceID refers to the same novel
        Tuple<string, string>[] GetMenuURLs(); //Returns a array of tuple of chapter title and url. 
        string[] GetChapterContent(string chapterTitle, string url); //Returns the title of the chapter with the extracted contents.
    }
}