using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Text;
using System.Threading.Tasks;

namespace NovelReader
{
    class NovelLibrary
    {

        private static NovelLibrary instance;

        private BindingList<Novel> _novelList { get; set; }
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

        public BindingList<Novel> NovelList
        {
            get { return this._novelList; }
        }

        /*============Constructor===========*/

        private NovelLibrary()
        {
            this._novelList = new BindingList<Novel>();
            
        }

        public void LoadNovelLibrary()
        {
            try
            {
                string dbFileName = Path.Combine(Configuration.Instance.NovelFolderLocation, Configuration.Instance.LibraryDataName);
                string dbName = "NovelDataBase";
                string connectionString = String.Format(@"Data Source=(LocalDB)\mssqllocaldb;AttachDBFileName={1};Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=5", dbName, dbFileName);
                libraryData = new LibraryDataContext(connectionString);
                //DetachDatabase(dbName);
                if (!libraryData.DatabaseExists())
                {
                    libraryData.CreateDatabase();
                }
                    
                //else
                //    libraryData.DeleteDatabase();
                _novelList = new BindingList<Novel>((from novel in libraryData.Novels
                             orderby novel.Rank ascending
                             select novel).ToList<Novel>());
            }catch(System.Data.SqlClient.SqlException e)
            {
                Console.WriteLine("Failed to create db");
                Console.WriteLine(e.ToString());
            }
            
        }

        private bool DetachDatabase(string dbName)
        {
            try
            {
                string connectionString = String.Format(@"Data Source=(LocalDB)\mssqllocaldb;Initial Catalog=master;Integrated Security=True");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = String.Format("exec sp_detach_db '{0}'", dbName);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void SaveNovelLibrary()
        {
           
        }

        public void CloseNovelLibrary()
        {
            libraryData.SubmitChanges();
            libraryData.Connection.Close();
        }

        /*============Getter/Setter=========*/

        public Novel GetNovel(string novelTitle)
        {
            var result = (from novel in NovelLibrary.libraryData.Novels
                          where novel.NovelTitle == novelTitle
                          select novel);
            if (result.Any())
                return result.First<Novel>();
            return null;
        }

        public int GetNovelCount()
        {
            return _novelList.Count;
        }

        public Novel[] GetUpdatingNovel()
        {
            List<Novel> updatingNovels = new List<Novel>();

            var results = from novel in _novelList
                          where novel.State == Novel.NovelState.Active || novel.State == Novel.NovelState.Inactive
                          orderby novel.Rank ascending
                          select novel;

            foreach (Novel n in results)
                updatingNovels.Add(n);

            return updatingNovels.ToArray();
        }

        /*============Public Function=======*/

        public bool AddNovel(string novelTitle, NovelSource novelSource, out string message)
        {
            foreach (Novel n in _novelList)
            {
                if (novelTitle.Equals(n.NovelTitle))
                {
                    message = novelTitle + " already exists.";
                    return false;
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
            
            using (var transaction = new TransactionScope())
            {
                try
                {
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
                    newNovel.Reading = false;
                    newNovel.Rank = GetNonDroppedNovelCount();
                    libraryData.Novels.InsertOnSubmit(newNovel);
                    libraryData.SubmitChanges();
                    _novelList.Insert(GetNonDroppedNovelCount(), newNovel);
                    UpdateNovelRanking();
                    transaction.Complete();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unable to add novel");
                }
                
            }
            message = novelTitle + " successfully added.";
            return true;

        }

        public void DeleteNovel(string novelTitle, bool deleteData)
        {
            Novel deleteNovel = GetNovel(novelTitle);
            if (deleteNovel == null)
                return;
            using (var transaction = new TransactionScope())
            {
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
                    NovelList.Remove(deleteNovel);
                    libraryData.SubmitChanges();
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to delete novel");
                }
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
            Move(pos, _novelList.Count - 1);
        }

        public void PickUpNovel(string novelTitle)
        {
            int pos = GetPosition(novelTitle);
            Move(pos, GetNonDroppedNovelCount());
        }

        /*============Private Function======*/

        private bool Move(int oldPosition, int newPosition)
        {
            if (oldPosition < 0 || oldPosition >= _novelList.Count)
                return false;
            if (newPosition < 0 || newPosition >= _novelList.Count)
                return false;
            if (oldPosition == newPosition)
                return false;

            Novel tmp = _novelList[oldPosition];
            _novelList.RemoveAt(oldPosition);
            _novelList.Insert(newPosition, tmp);
            UpdateNovelRanking();
            return true;
        }

        private int GetPosition(string novelTitle)
        {
            for (int i = 0; i < _novelList.Count; i++)
            {
                if (_novelList[i].NovelTitle.Equals(novelTitle))
                    return i;
            }
            return -1;
        }

        private int GetNonDroppedNovelCount()
        {
            int count = 0;
            for (int i = 0; i < _novelList.Count; i++)
            {
                if (_novelList[i].State != Novel.NovelState.Dropped)
                    count++;
            }
            return count;
        }

        private void UpdateNovelRanking()
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < _novelList.Count; i++)
                    {
                        _novelList[i].Rank = i + 1;
                    }
                    libraryData.SubmitChanges();
                    transaction.Complete();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unable to Update Novel Ranking");
                }
                
            }
        }
    }
}
