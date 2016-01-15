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
            //novelDirectoryWatcher.NotifyFilter = NotifyFilters.
        }

        public void SetReadingNovel(Novel novel)
        {
            this.currentReadingNovel = novel;
            this.Text = novel.NovelTitle;
            this.dgvChapterList.DataSource = novel.Chapters;
            this.novelDirectoryWatcher.Path = Path.Combine(Configuration.Instance.NovelFolderLocation, novel.NovelTitle);
            if (novel.LastReadChapter != null)
                ReadChapter(novel.LastReadChapter);
            else
                ReadChapter(novel.GetChapter(0));
        }

        /*============EventHandler==========*/

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
            if (currentReadingChapter != null)
            {
                Chapter nextChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index + 1);
                ReadChapter(nextChapter);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            FinishEditing();
            if (currentReadingChapter != null)
            {
                Chapter prevChapter = currentReadingNovel.GetChapter(currentReadingChapter.Index - 1);
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
        }

        private void dgvChapterList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvChapterList.IsCurrentCellDirty)
                dgvChapterList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvChapterList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
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
                Thread t = new Thread(new ParameterizedThreadStart(DownloadAndReadChapter));
                t.Start(currentReadingChapter);
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
            labelTitle.Text = currentReadingNovel.NovelTitle + " - " + chapter.ChapterTitle;
            currentReadingNovel.ReadChapter(chapter);
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
            dgvChapterList.Rows[chapter.Index].Selected = true;
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
                Console.WriteLine("Current Chapter dirty");
                if (currentReadingChapter != null)
                {
                    string text = rtbChapterTextBox.Text;
                    System.IO.File.WriteAllText(currentReadingChapter.GetTextFileLocation(), text);
                }
            }
            else
            {
                Console.WriteLine("Current Chapter clean");
            }
            editModeOn = false;
        }

        private void ModifyCellStyle(int rowIndex)
        {
            DataGridViewRow row = dgvChapterList.Rows[rowIndex];
            Chapter chapter = currentReadingNovel.Chapters[rowIndex];

            if (!chapter.HasText)
            {

                row.DefaultCellStyle.BackColor = Color.LightPink;
                row.DefaultCellStyle.SelectionBackColor = Color.Firebrick;
            }
            else if (!chapter.Read && chapter.HasText && chapter.HasAudio)
            {

                row.DefaultCellStyle.BackColor = Color.LightBlue;
                row.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;

            }
            else if (!chapter.Read && chapter.HasText && !chapter.HasAudio)
            {

                row.DefaultCellStyle.BackColor = Color.LemonChiffon;
                row.DefaultCellStyle.SelectionBackColor = Color.BurlyWood;

            }
            else if (chapter.Read && chapter.HasText)
            {

                row.DefaultCellStyle.BackColor = Color.LightGreen;
                row.DefaultCellStyle.SelectionBackColor = Color.Green;
            }
        }




    }
}
