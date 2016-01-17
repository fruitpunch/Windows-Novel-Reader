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
            BindGrid();
            BackgroundService.Instance.novelReaderForm = this;
            this.editModeOn = false;
            this.currentChapterDirty = false;
            this.rtbChapterTextBox.BackColor = Color.AliceBlue;
            this.novelDirectoryWatcher = new FileSystemWatcher();
            this.novelDirectoryWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.novelDirectoryWatcher.Created += new FileSystemEventHandler(OnFileChange);
            this.novelDirectoryWatcher.Deleted += new FileSystemEventHandler(OnFileChange);
            this.novelDirectoryWatcher.IncludeSubdirectories = true;
            //novelDirectoryWatcher.NotifyFilter = NotifyFilters.
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

            if (novel.LastViewedChapter != null)
            {
                ReadChapter(novel.LastViewedChapter);
                dgvChapterList.FirstDisplayedScrollingRowIndex = currentReadingChapter.Index;
            }
            else if (currentReadingNovel.ChapterCount > 0)
            {
                ReadChapter(novel.GetChapter(0));
                dgvChapterList.FirstDisplayedScrollingRowIndex = 0;
            }
            else
            {
                rtbChapterTextBox.Text = "No chapters available";
            }

        }

        /*============EventHandler==========*/

        private void OnFileChange(object source, FileSystemEventArgs e)
        {
            int fileIndex = -1;
            string[] parts = Path.GetFileName(e.Name).Split('_');
                
            if (parts.Length != 2)
                return;
            if (Int32.TryParse(parts[0], out fileIndex) && fileIndex >= 0 && fileIndex < currentReadingNovel.Chapters.Count)
            {
                if (e.FullPath.Equals(currentReadingNovel.Chapters[fileIndex].GetAudioFileLocation()) || e.FullPath.Equals(currentReadingNovel.Chapters[fileIndex].GetTextFileLocation()))
                    ModifyCellStyle(fileIndex);
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
                currentReadingNovel.MarkOffChapter(currentReadingChapter);
                Chapter prevChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index + 1);
                ReadChapter(prevChapter);
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

        /*============PrivateFunction=======*/

        private void BindGrid()
        {
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
            if (chapter == null)
                return;
            labelTitle.DataBindings.Clear();
            labelTitle.DataBindings.Add(new Binding("Text", chapter, "ChapterTitle", false, DataSourceUpdateMode.OnPropertyChanged));
            //labelTitle.Text = currentReadingNovel.NovelTitle + " - " + chapter.ChapterTitle;
            currentReadingNovel.StartReadingChapter(chapter);
            currentReadingChapter = chapter;
            rtbChapterTextBox.Select(0, 0);
            rtbChapterTextBox.ScrollToCaret();
            
            if (chapter.HasText)
            {
                using (StreamReader sr = new StreamReader(chapter.GetTextFileLocation()))
                {
                    string chapterText = sr.ReadToEnd();
                    rtbChapterTextBox.Text = chapterText;
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
            //dgvChapterList.Rows[chapter.Index].Selected = true;
        }

        private void DownloadAndReadChapter(Object chapterObj)
        {
            Chapter chapter = (Chapter)chapterObj;
            currentReadingNovel.DownloadChapterContent(chapter);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    ReadChapter(chapter);
                }));
            }
            
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
            if (dgvChapterList == null || rowIndex >= dgvChapterList.Rows.Count)
                return;
            DataGridViewRow row = dgvChapterList.Rows[rowIndex];
            Chapter chapter = currentReadingNovel.Chapters[rowIndex];
            row.DefaultCellStyle.SelectionForeColor = Color.DimGray;
            if (chapter.Equals(currentReadingChapter))
            {
                row.DefaultCellStyle.BackColor = Color.Orange;
                //row.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                //row.DefaultCellStyle.SelectionBackColor = Color.LightPink;
                
                row.DefaultCellStyle.SelectionBackColor = Color.Orange;
            }
            else if (!chapter.HasText)
            {

                row.DefaultCellStyle.BackColor = Color.LightPink;
                //row.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                row.DefaultCellStyle.SelectionBackColor = Color.LightPink;
                //row.DefaultCellStyle.SelectionBackColor = Color.Firebrick;
            }
            else if (!chapter.Read && chapter.HasText && chapter.HasAudio)
            {

                row.DefaultCellStyle.BackColor = Color.LightBlue;
                //row.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                row.DefaultCellStyle.SelectionBackColor = Color.LightBlue;

                //row.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;

            }
            else if (!chapter.Read && chapter.HasText && !chapter.HasAudio)
            {

                row.DefaultCellStyle.BackColor = Color.Cornsilk;
                //row.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                row.DefaultCellStyle.SelectionBackColor = Color.Cornsilk;

                //row.DefaultCellStyle.SelectionBackColor = Color.BurlyWood;

            }
            else if (chapter.Read && chapter.HasText)
            {

                row.DefaultCellStyle.BackColor = Color.LightGreen;
                //row.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                row.DefaultCellStyle.SelectionBackColor = Color.LightGreen;

                //row.DefaultCellStyle.SelectionBackColor = Color.Green;
            }
        }







    }
}
