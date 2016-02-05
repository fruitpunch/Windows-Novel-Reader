using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelReader
{
    class NovelLibrary
    {

        private static NovelLibrary instance;

        private List<Novel> novelList { get; set; }
        public volatile static LibraryDataContext libraryData;
        

        /*============Properties============*/

        public static NovelLibrary Instance
        {
            get
            {
                if (instance == null)
                {
                    Console.WriteLine("Create instance of NovelLibrary");
                    instance = new NovelLibrary();
                }
                return instance;
            }
        }

        public List<Novel> NovelList
        {
            get { return this.novelList; }
        }

        /*============Constructor===========*/

        private NovelLibrary()
        {
            this.novelList = new List<Novel>();
            
        }

        public void LoadNovelLibrary()
        {
            try
            {
                string dbFileName = Path.Combine(Configuration.Instance.NovelFolderLocation, Configuration.Instance.LibraryDataName);
                string connectionString = String.Format(@"Data Source=(LocalDB)\v11.0;AttachDBFileName={1};Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=true", "NovelData", dbFileName);
                libraryData = new LibraryDataContext(connectionString);
                if (!libraryData.DatabaseExists())
                {
                    libraryData.CreateDatabase();
                }
                novelList = (from novel in libraryData.Novels
                             orderby novel.Rank ascending
                             select novel).ToList<Novel>();
            }catch(System.Data.SqlClient.SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        public void SaveNovelLibrary()
        {
           
        }

        public void CloseNovelLibrary()
        {
            libraryData.Connection.Close();
        }

        /*============Getter/Setter=========*/

        public Novel GetNovel(string novelTitle)
        {
            Novel result = (from novel in NovelLibrary.libraryData.Novels
                            where novel.NovelTitle == novelTitle
                            select novel).First<Novel>();
            return result;
        }

        public int GetNovelCount()
        {
            return novelList.Count;
        }

        public Novel[] GetUpdatingNovel()
        {
            List<Novel> updatingNovels = new List<Novel>();

            var results = from novel in novelList
                          where novel.State == Novel.NovelState.Active || novel.State == Novel.NovelState.Inactive
                          orderby novel.Rank ascending
                          select novel;

            foreach (Novel n in results)
                updatingNovels.Add(n);

            return updatingNovels.ToArray();
        }

        /*============Public Function=======*/

        public Tuple<bool, string> AddNovel(string novelTitle, NovelSource novelSource)
        {
            foreach (Novel n in novelList)
            {
                if (novelTitle.Equals(n.NovelTitle))
                {
                    Tuple<bool, string> failReturn = new Tuple<bool, string>(false, novelTitle + " already exists.");
                    return failReturn;
                }
            }

            string newNovelLocation = Path.Combine(Configuration.Instance.NovelFolderLocation, novelTitle);

            if (!Directory.Exists(newNovelLocation))
            {
                Directory.CreateDirectory(newNovelLocation);
                Directory.CreateDirectory(Path.Combine(newNovelLocation, "audios"));
                Directory.CreateDirectory(Path.Combine(newNovelLocation, "texts"));
                File.Create(Path.Combine(newNovelLocation, Configuration.Instance.ReplaceSpecification));
                File.Create(Path.Combine(newNovelLocation, Configuration.Instance.DeleteSpecification));
            }
            else
            {
                if (!Directory.Exists(Path.Combine(newNovelLocation, "audios")))
                    Directory.CreateDirectory(Path.Combine(newNovelLocation, "audios"));

                if (!Directory.Exists(Path.Combine(newNovelLocation, "texts")))
                    Directory.CreateDirectory(Path.Combine(newNovelLocation, "texts"));

                if (!File.Exists(Path.Combine(newNovelLocation, Configuration.Instance.ReplaceSpecification)))
                    File.Create(Path.Combine(newNovelLocation, Configuration.Instance.ReplaceSpecification));

                if (!File.Exists(Path.Combine(newNovelLocation, Configuration.Instance.DeleteSpecification)))
                    File.Create(Path.Combine(newNovelLocation, Configuration.Instance.DeleteSpecification));
            }

            Source newSource = new Source();
            newSource.SourceNovelLocation = novelSource.SourceLocation.ToString();
            newSource.SourceNovelID = novelSource.NovelID;
            libraryData.Sources.InsertOnSubmit(newSource);
            libraryData.SubmitChanges();

            Novel newNovel = new Novel();
            newNovel.NovelTitle = novelTitle;
            newNovel.LastReadChapterID = -1;
            newNovel.SourceID = newSource.ID;
            newNovel.State = Novel.NovelState.Active;
            novelList.Insert(GetNonDroppedNovelCount(), newNovel);
            UpdateNovelRanking();
            libraryData.Novels.InsertOnSubmit(newNovel);
            libraryData.SubmitChanges();
            libraryData.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues);
            Tuple<bool, string> successfulReturn = new Tuple<bool, string>(true, novelTitle + " successfully added.");
            return successfulReturn;

        }

        public void DeleteNovel(string novelTitle, bool deleteData)
        {
            Novel deleteNovel = GetNovel(novelTitle);
            if (deleteNovel == null)
                return;
            try
            {
                var deleteChapters = (from chapter in libraryData.Chapters
                                      where chapter.NovelTitle == novelTitle
                                      select chapter);
                var deleteUrls = (from url in libraryData.ChapterUrls
                                  where deleteChapters.Contains(url.Chapter)
                                  select url);
                var deleteSources = (from source in libraryData.Sources
                                    where source.ID == deleteNovel.SourceID
                                    select source);
                libraryData.Chapters.DeleteAllOnSubmit(deleteChapters);
                libraryData.ChapterUrls.DeleteAllOnSubmit(deleteUrls);
                libraryData.Sources.DeleteAllOnSubmit(deleteSources);
                libraryData.Novels.DeleteOnSubmit(deleteNovel);
                if (deleteData)
                {
                    Directory.Delete(deleteNovel.GetNovelDirectory(), true);
                }
                libraryData.SubmitChanges();
            }
            catch (Exception e)
            {

            }

        }

        public bool RankUpNovel(string novelTitle)
        {
            if (GetNovel(novelTitle).State == Novel.NovelState.Dropped)
                return false;
            int pos = GetPosition(novelTitle);
            int dropIndex = GetNonDroppedNovelCount();
            
            return Move(pos, pos - 1);
        }

        public bool RankDownNovel(string novelTitle)
        {
            if (GetNovel(novelTitle).State == Novel.NovelState.Dropped)
                return false;
            int pos = GetPosition(novelTitle);
            int dropIndex = GetNonDroppedNovelCount();
            if (pos + 1 >= dropIndex)
                return false;
            return Move(pos, pos + 1);
        }

        public void DropNovel(string novelTitle)
        {
            int pos = GetPosition(novelTitle);
            Move(pos, novelList.Count - 1);
        }

        public void PickUpNovel(string novelTitle)
        {
            int pos = GetPosition(novelTitle);
            Move(pos, GetNonDroppedNovelCount());
        }

        /*============Private Function======*/

        private bool Move(int oldPosition, int newPosition)
        {
            if (oldPosition < 0 || oldPosition >= novelList.Count)
                return false;
            if (newPosition < 0 || newPosition >= novelList.Count)
                return false;
            if (oldPosition == newPosition)
                return false;

            Novel tmp = novelList[oldPosition];
            novelList.RemoveAt(oldPosition);
            novelList.Insert(newPosition, tmp);
            UpdateNovelRanking();
            return true;
        }

        private int GetPosition(string novelTitle)
        {
            for (int i = 0; i < novelList.Count; i++)
            {
                if (novelList[i].NovelTitle.Equals(novelTitle))
                    return i;
            }
            return -1;
        }

        private int GetNonDroppedNovelCount()
        {
            int count = 0;
            for (int i = 0; i < novelList.Count; i++)
            {
                if (novelList[i].State != Novel.NovelState.Dropped)
                    count++;
            }
            return count;
        }

        private void UpdateNovelRanking()
        {
            for (int i = 0; i < novelList.Count; i++)
            {
                novelList[i].Rank = i + 1;
            }
            libraryData.SubmitChanges();
        }
    }
}
