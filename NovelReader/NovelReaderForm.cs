using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelReader
{

    

    public partial class NovelReaderForm : Form
    {
        private Novel currentReadingNovel;
        private Chapter currentReadingChapter;
        private bool editModeOn;
        private bool currentChapterDirty;
        private FileSystemWatcher novelDirectoryWatcher;

        private Font readingFont;
        private Font selectedFont;
        private Font regularFont;

        public NovelReaderForm()
        {
            
            InitializeComponent();
            SetControl();
            BackgroundService.Instance.novelReaderForm = this;
            this.editModeOn = false;
            this.currentChapterDirty = false;
            this.rtbChapterTextBox.BackColor = Color.AliceBlue;
            this.readingFont = new Font(dgvChapterList.Font, FontStyle.Bold);
            this.regularFont = new Font(dgvChapterList.Font, FontStyle.Regular);
            this.novelDirectoryWatcher = new FileSystemWatcher();
            this.novelDirectoryWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.novelDirectoryWatcher.Created += new FileSystemEventHandler(OnFileChange);
            this.novelDirectoryWatcher.Deleted += new FileSystemEventHandler(OnFileChange);
            this.novelDirectoryWatcher.IncludeSubdirectories = true;
        }

        public void SetReadingNovel(Novel novel)
        {
            if (this.currentReadingNovel != null)
                this.currentReadingNovel.StopReading();
            this.currentReadingNovel = novel;
            this.currentReadingNovel.StartReading();
            this.Text = novel.NovelTitle;
            this.dgvChapterList.DataSource = novel.NovelChapters;
            novel.NovelChapters.ListChanged += NovelChapters_ListChanged;
            this.novelDirectoryWatcher.Path = Path.Combine(Configuration.Instance.NovelFolderLocation, novel.NovelTitle);
            BackgroundService.Instance.ResetTTSList();
            this.novelDirectoryWatcher.EnableRaisingEvents = true;

            
                

            if (novel.LastReadChapter != null)
            {
                Chapter nextChapter = novel.GetChapter(novel.LastReadChapter.Index + 1);
                Console.WriteLine("last read chapter " + novel.LastReadChapter.ChapterTitle);
                //Console.WriteLine("next chapter " + nextChapter.ChapterTitle);
                if (nextChapter != null && !nextChapter.Read)
                {
                    ReadChapter(nextChapter);
                    //dgvChapterList.FirstDisplayedScrollingRowIndex = nextChapter.Index;
                }
                else
                {
                    ReadChapter(novel.LastReadChapter);
                    //dgvChapterList.FirstDisplayedScrollingRowIndex = currentReadingChapter.Index;
                }
            }
            else if (novel.LastViewedChapter != null)
            {
                Console.WriteLine("LastViewedChapter");
                ReadChapter(novel.LastViewedChapter);
                //dgvChapterList.FirstDisplayedScrollingRowIndex = currentReadingChapter.Index;
            }
            else if (currentReadingNovel.ChapterCount > 0)
            {
                Console.WriteLine("Get 0");
                ReadChapter(novel.GetChapter(0));
                //dgvChapterList.FirstDisplayedScrollingRowIndex = 0;
            }
            else
            {
                rtbChapterTextBox.Text = "No chapters available";
            }

            if (currentReadingChapter != null && dgvChapterList.Rows.Count > currentReadingChapter.Index && 0 <= currentReadingChapter.Index)
            {
                try
                {
                    dgvChapterList.FirstDisplayedScrollingRowIndex = currentReadingChapter.Index;
                }
                catch (Exception)
                {
                }
            }

        }

        

        public bool InvokeRequiredForNovel(Novel n)
        {
            if (currentReadingNovel != null && currentReadingNovel.NovelTitle == n.NovelTitle)
            {
                return this.InvokeRequired;
            }
            return false;
        }

        public bool InvokeRequiredForChapter(Chapter c)
        {
            if (currentReadingNovel != null && currentReadingNovel.NovelTitle == c.NovelTitle)
                return true;
            return false;
        }

        /*============EventHandler==========*/


        private void NovelReaderForm_Load(object sender, EventArgs e)
        {

        }


        private void NovelChapters_ListChanged(object sender, ListChangedEventArgs e)
        {
            dgvChapterList.InvalidateRow(e.NewIndex);
        }

        private void cbAutoPlay_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.Instance.AutoPlay = cbAutoPlay.Checked;
        }

        private void OnFileChange(object source, FileSystemEventArgs e)
        {
            int fileIndex = -1;
            string[] parts = Path.GetFileName(e.Name).Split('_');
                
            if (parts.Length != 2)
                return;
            if (Int32.TryParse(parts[0], out fileIndex) && fileIndex >= 0 && fileIndex < currentReadingNovel.ChapterCount)
            {
                if (e.FullPath.Equals(currentReadingNovel.NovelChapters[fileIndex].GetAudioFileLocation()) || e.FullPath.Equals(currentReadingNovel.NovelChapters[fileIndex].GetTextFileLocation()))
                {
                    ModifyCellStyle(fileIndex);
                    if (currentReadingChapter != null && fileIndex == currentReadingChapter.Index)
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                            {
                                ReadChapter(currentReadingChapter);
                            }));
                        }
                        else
                            ReadChapter(currentReadingChapter);
                    }
                }
                
            }

        }

        private void dgvChapterList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
                ReadChapter(currentReadingNovel.NovelChapters[e.RowIndex]);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (editModeOn) //Finished Editing
                FinishEditing();
            else //Start Editing
                StartEditing();     
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(editModeOn)
                FinishEditing();
            if (currentReadingNovel != null && currentReadingChapter != null)
            {
                Chapter nextChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index + 1);
                ReadChapter(nextChapter);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (editModeOn)
                FinishEditing();
            if (currentReadingNovel != null && currentReadingChapter != null)
            {
                Chapter nextChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index - 1);
                ReadChapter(nextChapter);
            }
        }


        private void btnFinishReading_Click(object sender, EventArgs e)
        {
            if (editModeOn)
                FinishEditing();
            if (currentReadingNovel != null && currentReadingChapter != null)
            {
                if (mp3Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    mp3Player.Ctlcontrols.stop();
                currentReadingNovel.MarkOffChapter(currentReadingChapter);
                Chapter nextChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index + 1);
                ReadChapter(nextChapter);
            }
        }

        private void rtbChapterTextBox_TextChanged(object sender, EventArgs e)
        {
            currentChapterDirty = true;
        }

        private void NovelReaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Configuration.Instance.NovelReaderRect = this.DesktopBounds;
            if (this.WindowState == FormWindowState.Maximized)
                Configuration.Instance.NovelReaderMaximized = true;
            else
                Configuration.Instance.NovelReaderMaximized = false;
            if (currentReadingNovel != null)
                currentReadingNovel.StopReading();
            mp3Player.Ctlcontrols.stop();
            mp3Player.close();
            mp3Player = null;
            BackgroundService.Instance.novelReaderForm = null;
        }

        private void dgvChapterList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvChapterList.IsCurrentCellDirty && dgvChapterList.CurrentCell.ColumnIndex == dgvChapterList.Columns["SelectColumn"].Index)
               dgvChapterList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvChapterList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if(dgvChapterList.Columns[e.ColumnIndex].Name.Equals("SelectColumn"))
            ModifyCellStyle(e.RowIndex);
        }

        private void dgvChapterList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            ModifyCellStyle(e.RowIndex);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (currentReadingChapter != null)
                PlayAudio(currentReadingChapter);
        }

        private void mp3Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8) //wmppMediaEnded
            {
                Thread.Sleep(1000);
                if (currentReadingNovel != null && currentReadingChapter != null)
                {
                    currentReadingNovel.MarkOffChapter(currentReadingChapter);
                    Chapter nextChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index + 1);
                    ReadChapter(nextChapter);
                }
            }
            else if (e.newState == 10) //wmppsMediaReady
            {
                mp3Player.Ctlcontrols.play();
            }
        }


        private void chapterContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            Source[] novelSources = (from s in currentReadingNovel.Sources
                                     orderby s.Priority ascending
                                     select s).ToArray();
            (chapterContextMenuStrip.Items[2] as ToolStripMenuItem).DropDownItems.Clear();
            foreach (Source s in novelSources)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = s.SourceNovelLocation;
                item.Click += Redownload_ItemClicked;
                (chapterContextMenuStrip.Items[2] as ToolStripMenuItem).DropDownItems.Add(item);
            }
        }

        private void dgvChapterList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = dgvChapterList.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    Console.WriteLine(hit.RowIndex);
                    if(!dgvChapterList.Rows[hit.RowIndex].Selected)
                    {
                        dgvChapterList.ClearSelection();
                        dgvChapterList.Rows[hit.RowIndex].Selected = true;
                    }
                        
                }
            }
        }

        private void Redownload_ItemClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string sourceName = item.Text;
            var result = (from source in currentReadingNovel.Sources
                          where source.SourceNovelLocation == sourceName
                          select source);
            if (!result.Any())
                return;
            Source s = result.First<Source>();
            RedownloadChapters(s);
        }


        private void chapterContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string clickedItemName = e.ClickedItem.Text;
            Console.WriteLine(clickedItemName);
            if (clickedItemName == "Set Read")
                SetReadChapters();
            else if (clickedItemName == "Set Not Read")
                SetNotReadChapters();
            else if (clickedItemName == "Remake Audio")
                RemakeAudio();
            else if (clickedItemName == "Delete Chapters")
                DeleteChapters();

        }


        /*============PrivateFunction=======*/

        private void SetControl()
        {
            cbAutoPlay.Checked = Configuration.Instance.AutoPlay;
            dgvChapterList.AutoGenerateColumns = false;

            DataGridViewCell indexCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCheckBoxCell selectCell = new DataGridViewCheckBoxCell();

            DataGridViewTextBoxColumn indexColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = indexCell,
                Name = "Index",
                HeaderText = "Index",
                DataPropertyName = "Index",
                Width = 50,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn titleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = indexCell,
                Name = "ChapterTitle",
                HeaderText = "Chapter Title",
                DataPropertyName = "ChapterTitle",
                Width = 200,
                ReadOnly = true
            };

            DataGridViewCheckBoxColumn selectColumn = new DataGridViewCheckBoxColumn()
            {
                CellTemplate = selectCell,
                Name = "SelectColumn",
                HeaderText = "Select",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FlatStyle = FlatStyle.Popup,
                
            };

            dgvChapterList.Columns.Add(titleColumn);
            dgvChapterList.Columns.Add(indexColumn);
            dgvChapterList.Columns.Add(selectColumn);



        }

        private void ReadChapter(Chapter chapter)
        {
            if (chapter == null || chapter.NovelTitle != currentReadingNovel.NovelTitle)
                return;
            labelTitle.Text = chapter.ChapterTitle;
            currentReadingNovel.StartReadingChapter(chapter);
            currentReadingChapter = chapter;
            
            if (Configuration.Instance.AutoPlay)
                PlayAudio(chapter);
            if (chapter.HasText)
            {
                try
                {
                    string cacheLocation = Path.Combine(Configuration.Instance.CacheFolderLocation, chapter.GetHash().ToString() + ".txt");
                    File.Copy(chapter.GetTextFileLocation(), cacheLocation, true);
                    using (StreamReader sr = new StreamReader(cacheLocation))
                    {
                        string chapterText = sr.ReadToEnd();
                        rtbChapterTextBox.Text = chapterText;
                        rtbChapterTextBox.Select(0, 0);
                        rtbChapterTextBox.ScrollToCaret();
                    }
                }
                catch (Exception e)
                {
                    rtbChapterTextBox.Text = "Please Reload.";
                }
            }
            else
            {
                rtbChapterTextBox.Text = "No text file available";
            }
            
            currentChapterDirty = false;

            if (dgvChapterList.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvChapterList.SelectedRows)
                {
                    row.Selected = false;
                }
            }
            
        }

        private void PlayAudio(Chapter chapter)
        {
            if (chapter.HasAudio)
            {
                if (mp3Player != null)
                {
                    mp3Player.Ctlcontrols.stop();
                    mp3Player.currentPlaylist.clear();
                    mp3Player.URL = null;
                }
                string cacheLocation = Path.Combine(Configuration.Instance.CacheFolderLocation, chapter.GetHash().ToString() + ".mp3");
                File.Copy(chapter.GetAudioFileLocation(), cacheLocation, true);
                mp3Player.URL = new Uri(cacheLocation).ToString();
                mp3Player.Ctlcontrols.play();
            }
        }

        private void SetReadChapters()
        {
            Chapter[] chapters = GetSelectedChapterItem();
            foreach (Chapter chapter in chapters)
                chapter.Read = true;
        }

        private void SetNotReadChapters()
        {
            Chapter[] chapters = GetSelectedChapterItem();
            foreach (Chapter chapter in chapters)
                chapter.Read = false;
        }

        private void RemakeAudio()
        {
            Chapter[] chapters = GetSelectedChapterItem();
            BackgroundService.Instance.PerformImmediateTTS(chapters);
        }

        private void DeleteChapters()
        {
            Chapter[] chapters = GetSelectedChapterItem();
            if(chapters.Contains(currentReadingChapter) && editModeOn)
            {
                MessageBox.Show("Please finish editing first.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            new Thread(delegate ()
            {
                currentReadingNovel.DeleteAllChapter(chapters, true);
                Console.WriteLine("Finish deleting");
            }).Start();

        }

        private void RedownloadChapters(Source source)
        {
            Chapter[] chapters = GetSelectedChapterItem();
            if (chapters.Contains(currentReadingChapter) && editModeOn)
            {
                MessageBox.Show("Please finish editing first.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new Thread(delegate ()
            {
                foreach (Chapter chapter in chapters)
                    currentReadingNovel.DownloadChapter(chapter, source);
            }).Start();
        }

        private void StartEditing()
        {
            rtbChapterTextBox.ReadOnly = false;
            currentChapterDirty = false;
            rtbChapterTextBox.BackColor = Color.White;
            btnEdit.Text = "Finish Edit";
            editModeOn = true;

        }

        private void FinishEditing()
        {
            if (!editModeOn)
                return;
            rtbChapterTextBox.BackColor = Color.AliceBlue;
            btnEdit.Text = "Edit";
            if (currentChapterDirty)
            {
                if (currentReadingChapter != null)
                {
                    string text = rtbChapterTextBox.Text;
                    System.IO.File.WriteAllText(currentReadingChapter.GetTextFileLocation(), text);
                }
            }
            editModeOn = false;
        }

        private Chapter[] GetSelectedChapterItem()
        {
            List<Chapter> chapters = new List<Chapter>();
            foreach(DataGridViewRow selectedRow in dgvChapterList.SelectedRows)
                chapters.Add(currentReadingNovel.GetChapter(selectedRow.Index));

            return (from chapter in chapters
             orderby chapter.Index ascending
             select chapter).ToArray<Chapter>();
        }

        private void ModifyCellStyle(int rowIndex)
        {
            if (dgvChapterList == null || rowIndex >= dgvChapterList.Rows.Count || rowIndex >= currentReadingNovel.ChapterCount || currentReadingNovel.NovelChapters[rowIndex].Index != rowIndex)
                return;
            
            DataGridViewRow row = dgvChapterList.Rows[rowIndex];
            Chapter chapter = currentReadingNovel.NovelChapters[rowIndex];
            bool read = chapter.Read;
            bool hasText = chapter.HasText;
            bool hasAudio = chapter.HasAudio;
            
            if (currentReadingChapter != null && chapter.ID == currentReadingChapter.ID)
                row.DefaultCellStyle.Font = readingFont;
            else
                row.DefaultCellStyle.Font = regularFont;

            if(read)
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
                row.DefaultCellStyle.SelectionBackColor = Color.LightGreen;
            }
            else if(!hasText)
            {
                row.DefaultCellStyle.BackColor = Color.LightPink;
                row.DefaultCellStyle.SelectionBackColor = Color.LightPink;
            }
            else if(hasText && !hasAudio)
            {
                row.DefaultCellStyle.BackColor = Color.Cornsilk;
                row.DefaultCellStyle.SelectionBackColor = Color.Cornsilk;
            }
            else if (hasText && hasAudio)
            {
                row.DefaultCellStyle.BackColor = Color.LightBlue;
                row.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            }
            
        }

    }
}
