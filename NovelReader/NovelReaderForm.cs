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

        private CurrencyManager cm;

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
            this.chapterBindingSource = new BindingSource();
            this.chapterBindingSource.DataSource = novel.NovelChapters;
            this.dgvChapterList.DataSource = this.chapterBindingSource;
            novel.NovelChapters.ListChanged += NovelChapters_ListChanged;
            this.novelDirectoryWatcher.Path = Path.Combine(Configuration.Instance.NovelFolderLocation, novel.NovelTitle);
            BackgroundService.Instance.ResetTTSList();
            this.novelDirectoryWatcher.EnableRaisingEvents = true;

            if (novel.LastReadChapter != null)
            {
                Chapter nextChapter = novel.GetChapter(GetDisplayedIndex(novel.LastReadChapter) + 1);
                if (nextChapter != null && !nextChapter.Read)
                {
                    ReadChapter(nextChapter);
                }
                else
                {
                    ReadChapter(novel.LastReadChapter);
                }
            }
            else if (novel.LastViewedChapter != null)
            {
                ReadChapter(novel.LastViewedChapter);
            }
            else if (currentReadingNovel.ChapterCount > 0)
            {
                ReadChapter(novel.GetChapter(0));
            }
            else
            {
                rtbChapterTextBox.Text = "No chapters available";
            }

            if (currentReadingChapter != null && dgvChapterList.Rows.Count > GetDisplayedIndex(currentReadingChapter) && 0 <= GetDisplayedIndex(currentReadingChapter))
            {
                try
                {
                    dgvChapterList.FirstDisplayedScrollingRowIndex = GetDisplayedIndex(currentReadingChapter);
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
            if (e.NewIndex >= 0 && e.NewIndex < dgvChapterList.RowCount)
                dgvChapterList.InvalidateRow(e.NewIndex);
        }

        private void cbAutoPlay_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.Instance.AutoPlay = cbAutoPlay.Checked;
        }

        private void OnFileChange(object source, FileSystemEventArgs e)
        {
            
            string fileName = Path.GetFileNameWithoutExtension(e.Name);
            var result = currentReadingNovel.Chapters.Where(c => c.HashID == fileName);
            if (result.Any())
            {
                int fileIndex = GetDisplayedIndex(result.First<Chapter>());
                if (fileIndex < 0 || fileIndex >= dgvChapterList.RowCount)
                    return;
                dgvChapterList.InvalidateRow(fileIndex);
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
                Chapter nextChapter = currentReadingNovel.GetChapter(GetDisplayedIndex(currentReadingChapter) + 1);
                ReadChapter(nextChapter);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (editModeOn)
                FinishEditing();
            if (currentReadingNovel != null && currentReadingChapter != null)
            {
                Chapter nextChapter = currentReadingNovel.GetChapter(GetDisplayedIndex(currentReadingChapter) - 1);
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
                
                Chapter nextChapter = currentReadingNovel.GetChapter(GetDisplayedIndex(currentReadingChapter) + 1);
                //Console.WriteLine(currentReadingChapter.Index + " " + nextChapter.Index);
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
                    Chapter nextChapter = currentReadingNovel.GetChapter(GetDisplayedIndex(currentReadingChapter) + 1);
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
            //else if (clickedItemName == "Export")
            //    ExportChapter();

        }


        /*============PrivateFunction=======*/

        private void SetControl()
        {
            cbAutoPlay.Checked = Configuration.Instance.AutoPlay;
            dgvChapterList.AutoGenerateColumns = false;

            DataGridViewCell indexCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCheckBoxCell validCell = new DataGridViewCheckBoxCell();

            DataGridViewTextBoxColumn indexColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = indexCell,
                Name = "Index",
                HeaderText = "Index",
                DataPropertyName = "Index",
                Width = 50,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells 
            };

            DataGridViewCheckBoxColumn validColumn = new DataGridViewCheckBoxColumn()
            {
                CellTemplate = validCell,
                Name = "Valid",
                HeaderText = "Valid",
                DataPropertyName = "Valid",
                Width = 60,
                ReadOnly = true,
                Visible = true,
            };

            DataGridViewTextBoxColumn titleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = indexCell,
                Name = "ChapterTitle",
                HeaderText = "Chapter Title",
                DataPropertyName = "ChapterTitle",
                Width = 200,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dgvChapterList.Columns.Add(titleColumn);
            //dgvChapterList.Columns.Add(indexColumn);
            //dgvChapterList.Columns.Add(validColumn);


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
                    string cacheLocation = Path.Combine(Configuration.Instance.CacheFolderLocation, chapter.HashID + ".txt");
                    File.Copy(chapter.GetTextFileLocation(), cacheLocation, true);
                    using (StreamReader sr = new StreamReader(cacheLocation))
                    {
                        string chapterText = sr.ReadToEnd();
                        rtbChapterTextBox.Text = chapterText;
                        rtbChapterTextBox.Select(0, 0);
                        rtbChapterTextBox.ScrollToCaret();
                    }
                }
                catch (Exception)
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
                string cacheLocation = Path.Combine(Configuration.Instance.CacheFolderLocation, chapter.HashID + ".mp3");
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
            if (chapters.Count() == 0)
                return;
            if (chapters.Contains(currentReadingChapter) && editModeOn)
            {
                MessageBox.Show("Please finish editing first.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string deleteMessage = chapters.Count() > 1 ? "Are you sure you want to delete these " + chapters.Count() + " items?" : "Are you sure you want to delete " + chapters[0].ChapterTitle;
            DialogResult dialogResult = MessageBox.Show(deleteMessage, "Delete Chapters", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
                return;

            new Thread(delegate ()
            {
                currentReadingNovel.DeleteAllChapter(chapters, true);
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
                {
                    currentReadingNovel.DownloadChapter(chapter, source);
                    if(currentReadingChapter.ID == chapter.ID)
                    {
                        this.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                        {
                            ReadChapter(chapter);
                        }));
                    }
                }
                    
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

        private int GetDisplayedIndex(Chapter chapter)
        {
            if (currentReadingNovel != null && chapter != null)
                return currentReadingNovel.NovelChapters.IndexOf(chapter);
            return -1;
        }

        private void ModifyCellStyle(int rowIndex)
        {
            if (dgvChapterList == null || rowIndex >= dgvChapterList.Rows.Count || rowIndex >= currentReadingNovel.ChapterCount)
                return;
            
            DataGridViewRow row = dgvChapterList.Rows[rowIndex];
            //Console.WriteLine("Feedback");
            Chapter chapter = currentReadingNovel.NovelChapters[rowIndex];
            bool valid = chapter.Valid;
            bool read = chapter.Read;
            bool hasText = chapter.HasText;
            bool hasAudio = chapter.HasAudio;

            //Console.WriteLine(chapter.ChapterTitle + " " + valid + " " + read + " " + hasText + " " + hasAudio);

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
        private void ExportItem_Click(object sender, EventArgs e)
        {
            Chapter[] chapters = GetSelectedChapterItem();
            if (chapters.Length == 0)
                return;

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            string destinationFolder = dialog.SelectedPath;
            Console.WriteLine(destinationFolder);
            new Thread(delegate ()
            {
                if (sender.ToString() == "Text")
                {
                    foreach(Chapter chapter in chapters)
                    {
                        chapter.ExportAudio(destinationFolder);
                    }
                }

                else if (sender.ToString() == "Audio")
                {
                    foreach (Chapter chapter in chapters)
                    {
                        chapter.ExportText(destinationFolder);
                    }
                }

                else if (sender.ToString() == "Audo and Text")
                {
                    foreach (Chapter chapter in chapters)
                    {
                        chapter.ExportAudio(destinationFolder);
                        chapter.ExportText(destinationFolder);
                    }
                }

            }).Start();
            
        }
    }
}
