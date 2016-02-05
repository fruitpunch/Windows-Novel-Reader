
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

        private BindingList<Novel> _novelList { get; set; }
        public volatile IObjectContainer db;
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
            libraryData = new LibraryDataContext(Path.Combine(Configuration.Instance.NovelFolderLocation, Configuration.Instance.LibraryDataName));
            try
            {
                IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
                config.Common.ObjectClass(typeof(Novel)).CascadeOnUpdate(true);
                config.Common.ObjectClass(typeof(Chapter)).CascadeOnUpdate(true);
                config.Common.ObjectClass(typeof(NovelSource)).CascadeOnUpdate(true);
                db = Db4oEmbedded.OpenFile(config, Path.Combine(Configuration.Instance.NovelFolderLocation, Configuration.Instance.NovelListDBName));
                IObjectSet novels = db.QueryByExample(typeof(Novel));
                InsertNovels(novels);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void SaveNovelLibrary()
        {
            try
            {
                foreach (Novel n in _novelList)
                {
                    n.SaveChapterToDB();
                    db.Store(n);
                }
                db.Commit();
            }
            catch(Exception e)
            {
            }
           
        }

        public void CloseNovelLibrary()
        {
            db.Close();
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

        public Tuple<bool, string> AddNovel(string novelTitle, NovelSource novelSource)
        {
            foreach (Novel n in _novelList)
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

            Novel newNovel = new Novel(novelTitle, novelSource);
            _novelList.Insert(GetNonDroppedNovelCount(), newNovel);

            try
            {
                db.Store(newNovel);
                db.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            UpdateNovelRanking();
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
                deleteNovel.DeleteChapterFromDB();
                db.Delete(deleteNovel);
                if (deleteData)
                {
                    Directory.Delete(deleteNovel.GetNovelDirectory(), true);
                }
                if (BackgroundService.Instance.novelListController != null && BackgroundService.Instance.novelListController.InvokeRequired)
                {
                    BackgroundService.Instance.novelListController.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        _novelList.Remove(deleteNovel);
                    }));
                }
                else
                {
                    _novelList.Remove(deleteNovel);
                }
                deleteNovel = null;
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
            for (int i = 0; i < _novelList.Count; i++)
            {
                _novelList[i].Rank = i + 1;
            }
        }

        private void InsertNovels(IObjectSet novelSet)
        {
            Novel[] novels = new Novel[novelSet.Count];
            for (int i = 0; i < novelSet.Count; i++)
                novels[i] = (Novel)novelSet[i];

            if (BackgroundService.Instance.novelListController != null && BackgroundService.Instance.novelListController.InvokeRequired)
            {
                BackgroundService.Instance.novelListController.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    foreach (Novel n in novels)
                    {
                        int i = 0;
                        for (; i < _novelList.Count; i++)
                        {
                            if (n.Rank < _novelList[i].Rank)
                                break;
                        }
                        _novelList.Insert(i, n);
                        n.Init();
                    }
                }));
            }
            else
            {
                foreach (Novel n in novels)
                {
                    int i = 0;
                    for (; i < _novelList.Count; i++)
                    {
                        if (n.Rank < _novelList[i].Rank)
                            break;
                    }
                    _novelList.Insert(i, n);
                    n.Init();
                }
            }   
        }
    }
}
