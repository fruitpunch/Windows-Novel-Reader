﻿using System;
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

        private BindingList<Novel> _novelList{ get; set; }
        

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

        }

        public void SaveNovelLibrary()
        {

        }

        /*============Getter/Setter=========*/

        public Novel GetNovel(string novelTitle)
        {
            foreach (Novel n in _novelList)
            {
                if (n.NovelTitle.Equals(novelTitle))
                    return n;
            }
            return null;
        }

        public Novel[] GetUpdatingNovel()
        {
            List<Novel> updatingNovels = new List<Novel>();

            var results = from novel in _novelList
                          where novel.State == Novel.NovelState.Active || novel.State == Novel.NovelState.Inactive
                          select novel;

            foreach (Novel n in results)
                updatingNovels.Add(n);

            return updatingNovels.ToArray();
        }

        /*============Public Function=======*/

        public Tuple<bool, string> AddNovel(string novelTitle, Source.SourceManager.Sources source, int sourceID)
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

            Novel newNovel = new Novel(novelTitle);
            _novelList.Insert(GetNonDroppedNovelCount(), newNovel);
            Tuple<bool, string> successfulReturn = new Tuple<bool, string>(true, novelTitle + " successfully added.");
            return successfulReturn;

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
    }
}
