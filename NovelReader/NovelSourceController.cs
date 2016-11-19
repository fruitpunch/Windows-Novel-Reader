using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelReader
{
    public partial class NovelSourceController : UserControl
    {
        private Novel novel = null;
        private BindingList<Chapter> blackListedChapters;
        private BindingList<Source> sourceList;
        private AddSourceController addSourceController;

        public NovelSourceController()
        {
            InitializeComponent();
            SetController();
            BackgroundService.Instance.novelSourceController = this;
        }

        public void SetNovel(Novel novel)
        {
            this.novel = novel;
            RefreshBlackList();
            RefreshSourceList();
            this.Visible = true;

        }

        public void RefreshBlackList()
        {
            if (this.novel == null)
                return;
            if (blackListedChapters == null)
                blackListedChapters = new BindingList<Chapter>();
            blackListedChapters.Clear();
            var result = (from blChapters in novel.Chapters
                                       where !blChapters.Valid
                                       select blChapters);
            foreach (Chapter chapter in result)
                blackListedChapters.Add(chapter);
            dgvBlackList.DataSource = blackListedChapters;
        }

        public void RefreshSourceList()
        {
            if (this.novel == null)
                return;
            if (sourceList == null)
                sourceList = new BindingList<Source>();
            sourceList.Clear();
            var result = (from sources in novel.Sources
                          orderby sources.Priority
                          select sources);
            foreach (Source chapter in result)
                sourceList.Add(chapter);
            dgvNovelSources.DataSource = sourceList;
        }

        private void SetController()
        {
            dgvBlackList.AutoGenerateColumns = false;

            DataGridViewCell chapterCell = new DataGridViewTextBoxCell();
            DataGridViewCell sourceCell = new DataGridViewTextBoxCell();

            DataGridViewTextBoxColumn chapterColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterCell,
                Name = "ChapterTitle",
                HeaderText = "Chapter Title",
                DataPropertyName = "ChapterTitle",
                Width = 150,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            DataGridViewTextBoxColumn sourceColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterCell,
                Name = "Source",
                HeaderText = "Source",
                Width = 150,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dgvBlackList.Columns.Add(chapterColumn);
            dgvBlackList.Columns.Add(sourceColumn);


            dgvNovelSources.AutoGenerateColumns = false;

            DataGridViewCell sourceNameCell = new DataGridViewTextBoxCell();
            DataGridViewCell sourceIdCell = new DataGridViewTextBoxCell();
            DataGridViewCell sourcePriorityCell = new DataGridViewTextBoxCell();
            DataGridViewCheckBoxCell sourceValidCell = new DataGridViewCheckBoxCell();

            DataGridViewTextBoxColumn sourceLocatonColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterCell,
                Name = "SourceLocation",
                HeaderText = "Location",
                DataPropertyName = "SourceNovelLocation",
                Width = 150,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            DataGridViewTextBoxColumn sourceIdColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterCell,
                Name = "SourceId",
                HeaderText = "ID",
                DataPropertyName = "SourceNovelID",
                Width = 100,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            DataGridViewTextBoxColumn sourcePriorityColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterCell,
                Name = "SourcePriority",
                HeaderText = "Priority",
                DataPropertyName = "Priority",
                Width = 10,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            DataGridViewCheckBoxColumn sourceValidColumn = new DataGridViewCheckBoxColumn()
            {
                CellTemplate = sourceValidCell,
                Name = "SourceValid",
                HeaderText = "Valid",
                DataPropertyName = "Valid",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dgvNovelSources.Columns.Add(sourceLocatonColumn);
            dgvNovelSources.Columns.Add(sourceIdColumn);
            dgvNovelSources.Columns.Add(sourcePriorityColumn);
            dgvNovelSources.Columns.Add(sourceValidColumn);

        }

        private void dgvBlackList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if(dgv.Columns[e.ColumnIndex].Name == "Source")
            {
                e.Value = novel.OriginSource.SourceNovelLocation;
                e.FormattingApplied = true;
            }

        }

        private void dgvNovelSources_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].Name == "SourceLocation")
            {
                Source s = sourceList[e.RowIndex];
                if (!s.Mirror)
                    e.Value = s.SourceNovelLocation + " (Origin)";
                else
                    e.Value = s.SourceNovelLocation + " (Mirror)";
                e.FormattingApplied = true;
            }
        }

        private void btnFinishEdit_Click(object sender, EventArgs e)
        {
            Close();
            BackgroundService.Instance.novelListController.Visible = true;
        }

        private void mainBackPanel_SizeChanged(object sender, EventArgs e)
        {
            sourcesBackPanel.Top = 5;
            sourcesBackPanel.Left = 5;
            sourcesBackPanel.Width = (mainBackPanel.Width - 20) / 2;
            sourcesBackPanel.Height = mainBackPanel.Height - 10;

            BlackListBackPanel.Top = 5;
            BlackListBackPanel.Left = sourcesBackPanel.Right + 10;
            BlackListBackPanel.Width = (mainBackPanel.Width - 20) / 2;
            BlackListBackPanel.Height = mainBackPanel.Height - 10;
        }

        private void btnRemoveBlackListItem_Click(object sender, EventArgs e)
        {
            if (dgvBlackList.SelectedRows.Count > 0)
            {
                Chapter selectedChapter = blackListedChapters[dgvBlackList.SelectedRows[0].Index];
                novel.DeleteChapter(selectedChapter, false);
                blackListedChapters.Remove(selectedChapter);
            }
        }

        public void Close()
        {
            this.novel = null;
            this.blackListedChapters = null;
            this.sourceList = null;
            this.Visible = false;
        }

        private void sourcesBackPanel_SizeChanged(object sender, EventArgs e)
        {
            btnAddSource.Left = 0;
            btnAddSource.Width = sourcesBackPanel.Width / 4;

            btnRemoveSource.Left = btnAddSource.Right;
            btnRemoveSource.Width = sourcesBackPanel.Width / 4;

            btnRankUpSource.Left = btnRemoveSource.Right;
            btnRankUpSource.Width = sourcesBackPanel.Width / 4;

            btnRankDownSource.Left = btnRankUpSource.Right;
            btnRankDownSource.Width = sourcesBackPanel.Width / 4;
        }

        public void CloseAddSourceController()
        {
            btnAddSource.Visible = true;
            btnRemoveSource.Visible = true;
            btnRankUpSource.Visible = true;
            btnRankDownSource.Visible = true;

            sourceControllerPanel.Top -= sourcesBackPanel.Height - 45;
            sourceControllerPanel.Height = 45;
            dgvNovelSources.Height = sourcesBackPanel.Height - 45;

            if(addSourceController != null)
            {
                sourceControllerPanel.Controls.Remove(addSourceController);
                addSourceController = null;
            }
            
        }

        private void btnAddSource_Click(object sender, EventArgs e)
        {
            btnAddSource.Visible = false;
            btnRemoveSource.Visible = false;
            btnRankUpSource.Visible = false;
            btnRankDownSource.Visible = false;

            addSourceController = new AddSourceController(novel);
            addSourceController.Dock = DockStyle.Fill;

            dgvNovelSources.Height = sourcesBackPanel.Height - addSourceController.Height;
            sourceControllerPanel.Top -= sourcesBackPanel.Height - addSourceController.Height;
            sourceControllerPanel.Height = addSourceController.Height;
            
            sourceControllerPanel.Controls.Add(addSourceController);
        }

        private void btnRemoveSource_Click(object sender, EventArgs e)
        {
            if (dgvNovelSources.SelectedRows.Count > 0)
            {
                string message;
                Source selectedSource = sourceList[dgvNovelSources.SelectedRows[0].Index];
                DialogResult forceAddSourceResult = MessageBox.Show("Are you sure you want to delete " + selectedSource.SourceNovelLocation + "?", "Delete Source", MessageBoxButtons.YesNo);
                if (forceAddSourceResult == DialogResult.No)
                    return;
                bool result = novel.DeleteSource(selectedSource, out message);
                if (!result)
                {
                    MessageBox.Show(message, "Cannot Delete Source", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                RefreshSourceList();
            }
        }

        private void btnRankUpSource_Click(object sender, EventArgs e)
        {
            if (dgvNovelSources.SelectedRows.Count > 0)
            {
                string message;
                Source selectedSource = sourceList[dgvNovelSources.SelectedRows[0].Index];
                bool result = novel.RankUpSource(selectedSource, out message);
                if (!result)
                    return;
                RefreshSourceList();
                dgvNovelSources.Rows[selectedSource.Priority].Selected = true;
            }
        }

        private void btnRankDownSource_Click(object sender, EventArgs e)
        {
            if (dgvNovelSources.SelectedRows.Count > 0)
            {
                string message;
                Source selectedSource = sourceList[dgvNovelSources.SelectedRows[0].Index];
                bool result = novel.RankDownSource(selectedSource, out message);
                if (!result)
                    return;
                RefreshSourceList();
                dgvNovelSources.Rows[selectedSource.Priority].Selected = true;
            }
        }

    }
}
