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

        public NovelReaderForm()
        {
            
            InitializeComponent();
            SetControl();
            BackgroundService.Instance.novelReaderForm = this;
            this.editModeOn = false;
            this.currentChapterDirty = false;
            this.rtbChapterTextBox.BackColor = Color.AliceBlue;
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
            this.dgvChapterList.DataSource = novel.Chapters;
            this.novelDirectoryWatcher.Path = Path.Combine(Configuration.Instance.NovelFolderLocation, novel.NovelTitle);
            BackgroundService.Instance.ResetTTSList();
            this.novelDirectoryWatcher.EnableRaisingEvents = true;

            if (novel.LastReadChapter != null)
            {
                Chapter nextChapter = novel.GetChapter(novel.LastReadChapter.Index + 1);
                //Console.WriteLine("last read chapter " + novel.LastReadChapter.ChapterTitle);
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
                ReadChapter(novel.LastViewedChapter);
                //dgvChapterList.FirstDisplayedScrollingRowIndex = currentReadingChapter.Index;
            }
            else if (currentReadingNovel.ChapterCount > 0)
            {
                ReadChapter(novel.GetChapter(0));
                //dgvChapterList.FirstDisplayedScrollingRowIndex = 0;
            }
            else
            {
                rtbChapterTextBox.Text = "No chapters available";
            }

            if (currentReadingChapter != null && dgvChapterList.Rows.Count > currentReadingChapter.Index)
            {
                try
                {
                    dgvChapterList.FirstDisplayedScrollingRowIndex = currentReadingChapter.Index;
                }
                catch (Exception e)
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
            if (Int32.TryParse(parts[0], out fileIndex) && fileIndex >= 0 && fileIndex < currentReadingNovel.Chapters.Count)
            {
                if (e.FullPath.Equals(currentReadingNovel.Chapters[fileIndex].GetAudioFileLocation()) || e.FullPath.Equals(currentReadingNovel.Chapters[fileIndex].GetTextFileLocation()))
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
            ReadChapter(currentReadingNovel.Chapters[e.RowIndex]);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!rtbChapterTextBox.ReadOnly) //Finished Editing
                FinishEditing();
            else //Start Editing
                StartEditing();     
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            FinishEditing();
            if (currentReadingNovel != null && currentReadingChapter != null)
            {
                Chapter nextChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index + 1);
                ReadChapter(nextChapter);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            FinishEditing();
            if (currentReadingNovel != null && currentReadingChapter != null)
            {
                Chapter nextChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index - 1);
                ReadChapter(nextChapter);
            }
        }


        private void btnFinishReading_Click(object sender, EventArgs e)
        {
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
            BackgroundService.Instance.novelReaderForm = null;
        }

        private void dgvChapterList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvChapterList.IsCurrentCellDirty && dgvChapterList.CurrentCell.ColumnIndex == dgvChapterList.Columns["Read"].Index)
               dgvChapterList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvChapterList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvChapterList.Columns[e.ColumnIndex].Name.Equals("Read"))
                ModifyCellStyle(e.RowIndex);
        }

        private void dgvChapterList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            ModifyCellStyle(e.RowIndex);
        }


        private void btnRedownload_Click(object sender, EventArgs e)
        {
            if (!rtbChapterTextBox.ReadOnly && currentChapterDirty)
            {
                MessageBox.Show("Please finish editing first.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (currentReadingNovel != null)
            {
                if (currentReadingChapter != null && currentReadingChapter.SourceURL != null)
                {
                    Thread t = new Thread(new ParameterizedThreadStart(DownloadAndReadChapter));
                    t.Start(currentReadingChapter);
                }
                else if (currentReadingChapter != null && currentReadingChapter.SourceURL == null)
                {
                    MessageBox.Show("This chapter does not contain a source link to download from.", "No Source Link", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }


        private void btnDeleteChapter_Click(object sender, EventArgs e)
        {
            if (currentReadingNovel != null && currentReadingChapter != null)
            {
                DialogResult deleteResult = MessageBox.Show("Are you sure you want to delete " + currentReadingChapter.ChapterTitle, "Delete Chapter", MessageBoxButtons.YesNo);
                if (deleteResult == DialogResult.No)
                    return;
                if (currentReadingChapter.SourceURL != null)
                {
                    DialogResult blackListResult = MessageBox.Show("Do you want to blacklist " + currentReadingChapter.ChapterTitle + "'s Source Link?", "Blacklist Link.", MessageBoxButtons.YesNo);
                    if (blackListResult == DialogResult.Yes)
                        currentReadingNovel.DeleteChapter(currentReadingChapter, true);
                    else
                        currentReadingNovel.DeleteChapter(currentReadingChapter, false);
                }
                else
                    currentReadingNovel.DeleteChapter(currentReadingChapter, false);
                ReadChapter(currentReadingNovel.LastViewedChapter);
            }

        }


        private void btnAddChapter_Click(object sender, EventArgs e)
        {
            if (currentReadingNovel != null)
            {
                
                Chapter chapter = currentReadingNovel.AddChapter();
                
                ReadChapter(chapter);
                dgvChapterList.CurrentCell = dgvChapterList[0, chapter.Index];
                dgvChapterList.BeginEdit(true);
            }
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


        /*============PrivateFunction=======*/

        private void SetControl()
        {
            cbAutoPlay.Checked = Configuration.Instance.AutoPlay;
            dgvChapterList.AutoGenerateColumns = false;

            DataGridViewCell indexCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCheckBoxCell hasReadCell = new DataGridViewCheckBoxCell();

            DataGridViewTextBoxColumn indexColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = indexCell,
                Name = "Index",
                HeaderText = "Index",
                DataPropertyName = "Index",
                Width = 75
            };

            DataGridViewTextBoxColumn titleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = indexCell,
                Name = "ChapterTitle",
                HeaderText = "Chapter Title",
                DataPropertyName = "ChapterTitle",
                Width = 200
            };

            DataGridViewCheckBoxColumn makeAudioColumn = new DataGridViewCheckBoxColumn()
            {
                CellTemplate = hasReadCell,
                Name = "Read",
                HeaderText = "Read",
                DataPropertyName = "Read",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dgvChapterList.Columns.Add(titleColumn);
            dgvChapterList.Columns.Add(indexColumn);
            dgvChapterList.Columns.Add(makeAudioColumn);
        }

        private void ReadChapter(Chapter chapter)
        {
            if (chapter == null || chapter.NovelTitle != currentReadingNovel.NovelTitle)
                return;
            labelTitle.DataBindings.Clear();
            labelTitle.DataBindings.Add(new Binding("Text", chapter, "ChapterTitle", false, DataSourceUpdateMode.OnPropertyChanged));
            currentReadingNovel.StartReadingChapter(chapter);
            currentReadingChapter = chapter;
            
            if (Configuration.Instance.AutoPlay)
                PlayAudio(chapter);
            if (chapter.HasText)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(chapter.GetTextFileLocation()))
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
                //Console.WriteLine("play " + chapter.Index);
                string tempMp3FileLocation = chapter.GetAudioFileLocation();
                //File.Copy(chapter.GetAudioFileLocation(), tempMp3FileLocation, true);
                mp3Player.URL = new Uri(tempMp3FileLocation).ToString();
                mp3Player.Ctlcontrols.play();
            }
        }

        private void DownloadAndReadChapter(Object chapterObj)
        {
            Chapter chapter = (Chapter)chapterObj;
            currentReadingNovel.DownloadChapterContent(chapter);
            this.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
            {
                ReadChapter(chapter);
            }));
            
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
            rtbChapterTextBox.ReadOnly = true;
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

        private void ModifyCellStyle(int rowIndex)
        {
            if (dgvChapterList == null || rowIndex >= dgvChapterList.Rows.Count || rowIndex >= currentReadingNovel.Chapters.Count)
                return;
            
            DataGridViewRow row = dgvChapterList.Rows[rowIndex];
            Chapter chapter = currentReadingNovel.Chapters[rowIndex];
            row.DefaultCellStyle.SelectionForeColor = Color.DimGray;
            if (chapter.Equals(currentReadingChapter))
            {
                row.DefaultCellStyle.BackColor = Color.Orange;
                row.DefaultCellStyle.SelectionBackColor = Color.Orange;
            }
            else if (!chapter.HasText)
            {
                row.DefaultCellStyle.BackColor = Color.LightPink;
                row.DefaultCellStyle.SelectionBackColor = Color.LightPink;
            }
            else if (!chapter.Read && chapter.HasText && chapter.HasAudio)
            {
                row.DefaultCellStyle.BackColor = Color.LightBlue;
                row.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            }
            else if (!chapter.Read && chapter.HasText && !chapter.HasAudio)
            {
                row.DefaultCellStyle.BackColor = Color.Cornsilk;
                row.DefaultCellStyle.SelectionBackColor = Color.Cornsilk;
            }
            else if (chapter.Read && chapter.HasText)
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
                row.DefaultCellStyle.SelectionBackColor = Color.LightGreen;
            }
        }

    }
}
