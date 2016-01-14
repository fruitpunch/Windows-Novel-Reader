using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelReader
{

    

    public partial class NovelReaderForm : Form
    {
        private Novel novel;
        private Chapter currentChapter;
        private bool editModeOn;
        private bool currentChapterDirty;

        public NovelReaderForm()
        {
            
            InitializeComponent();
            BindGrid();
            BackgroundService.Instance.novelReaderForm = this;
            this.editModeOn = false;
            this.currentChapterDirty = false;
            this.rtbChapterTextBox.BackColor = Color.AliceBlue;
        }

        public void SetReadingNovel(Novel n)
        {
            this.novel = n;
            this.Text = n.NovelTitle;
            dgvChapterList.DataSource = n.Chapters;
        }

        /*============EventHandler==========*/

        private void dgvChapterList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ReadChapter(novel.Chapters[e.RowIndex]);
            
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (editModeOn) //Finished Editing
            {
                FinishEditing();
            }
            else //Start Editing
            {
                StartEditing();
            }
            editModeOn = !editModeOn;
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
            using (StreamReader sr = new StreamReader(chapter.GetTextFileLocation()))
            {
                string chapterText = sr.ReadToEnd();
                rtbChapterTextBox.Text = chapterText;
                chapter.Read = true;
            }
            currentChapter = chapter;
            currentChapterDirty = false;
        }

        private void StartEditing()
        {
            rtbChapterTextBox.ReadOnly = false;
            currentChapterDirty = false;
            rtbChapterTextBox.BackColor = Color.White;
            btnEdit.Text = "Finish Edit";
        }

        private void FinishEditing()
        {
            rtbChapterTextBox.ReadOnly = true;
            rtbChapterTextBox.BackColor = Color.AliceBlue;
            btnEdit.Text = "Edit";
            if (currentChapterDirty)
            {
                Console.WriteLine("Current Chapter dirty");
                if (currentChapter != null)
                {
                    string text = rtbChapterTextBox.Text;
                    System.IO.File.WriteAllText(currentChapter.GetTextFileLocation(), text);
                }
            }
            else
            {
                Console.WriteLine("Current Chapter clean");
            }
        }

        

        

    }
}
