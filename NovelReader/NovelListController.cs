﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace NovelReader
{
    public class UpdateDataGridViewProgressColumn : DataGridViewImageColumn
    {
        public UpdateDataGridViewProgressColumn()
        {
            CellTemplate = new UpdateDataGridViewProgressCell();
        }
    }

    class UpdateDataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        static UpdateDataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public UpdateDataGridViewProgressCell()
        {
            this.ValueType = typeof(Tuple<int, string>);
        }
        // Method required to make the Progress Cell consistent with the default Image Cell. 
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override object GetFormattedValue(object value,
                            int rowIndex, ref DataGridViewCellStyle cellStyle,
                            TypeConverter valueTypeConverter,
                            TypeConverter formattedValueTypeConverter,
                            DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }

        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Tuple<int, string> updateInfo = (Tuple<int, string>)value;
            int progressVal = updateInfo.Item1;
            string message = updateInfo.Item2;

            float percentage = ((float)progressVal / 100.0f); // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
            Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
            // Draws the cell grid
            base.Paint(g, clipBounds, cellBounds,
             rowIndex, cellState, value, formattedValue, errorText,
             cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));

            
            if (percentage > 0.0)
            {
                // Draw the progress bar and the text
                g.FillRectangle(new SolidBrush(Color.FromArgb(163, 189, 242)), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
                g.DrawString(message, cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
            }
            else
            {
                // draw the text
                if (this.DataGridView.CurrentRow.Index == rowIndex)
                    g.DrawString(message, cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 2);
                else
                    g.DrawString(message, cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
            }
        }
    }


    public partial class NovelListController : UserControl
    {

        private NovelReaderForm nrf;
        private Pen redPen;
        private Pen whitePen;
        private readonly int penWidth = 2;

        public NovelListController()
        {
            InitializeComponent();
            BindGrid();
            this.redPen = new Pen(Color.Red);
            this.redPen.Width = penWidth;
            this.whitePen = new Pen(Color.FromArgb(0, Color.White.R, Color.White.G, Color.White.B));
            this.whitePen.Width = penWidth;
            this.labelLastUpdateTime.Text = "";

            BackgroundService.Instance.novelListController = this;
        }

        /*============EventHandler==========*/

        private void NovelListController_Load(object sender, EventArgs e)
        {
            refreshUpdateLabelTimer.Start();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //NovelReaderForm nrf = new NovelReaderForm();
            //nrf.ShowDialog();
            if (dgvNovelList.SelectedRows.Count > 0)
            {
                string novelTitle = dgvNovelList.SelectedRows[0].Cells["NovelTitle"].Value.ToString();
                BackgroundService.Instance.UpdateTTSTest(novelTitle);
            }
        }


        private void btnAddNovel_Click(object sender, EventArgs e)
        {
            AddNovelForm anf = new AddNovelForm();
            anf.Location = this.Location;
            anf.ShowDialog();
        }

        private void btnRankUp_Click(object sender, EventArgs e)
        {
            if (dgvNovelList.SelectedRows.Count > 0)
            {
                string novelTitle = dgvNovelList.SelectedRows[0].Cells["NovelTitle"].Value.ToString();
                int selectedRowIndex = dgvNovelList.SelectedRows[0].Index;
                if (BackgroundService.Instance.RankUpNovel(novelTitle))
                {
                    foreach (DataGridViewRow row in dgvNovelList.SelectedRows)
                        row.Selected = false;
                    dgvNovelList.Rows[selectedRowIndex - 1].Selected = true;
                }
            }
        }

        private void btnRankDown_Click(object sender, EventArgs e)
        {
            if (dgvNovelList.SelectedRows.Count > 0)
            {
                string novelTitle = dgvNovelList.SelectedRows[0].Cells["NovelTitle"].Value.ToString();
                int selectedRowIndex = dgvNovelList.SelectedRows[0].Index;
                if (BackgroundService.Instance.RankDownNovel(novelTitle))
                {
                    foreach(DataGridViewRow row in dgvNovelList.SelectedRows)
                        row.Selected = false;
                    dgvNovelList.Rows[selectedRowIndex + 1].Selected = true;
                }
            }
        }

        private void dgvNovelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool validRow = (e.RowIndex != -1); //Make sure the clicked row isn't the header.
            bool validCol = (e.ColumnIndex != -1);
            var datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox 
            if (validRow && validCol && datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }

        private void dgvNovelList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvNovelList.IsCurrentCellDirty)
                dgvNovelList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvNovelList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ModifyCellStyle(e.RowIndex);
        }

        private void dgvNovelList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            ModifyCellStyle(e.RowIndex);
        }

        private void dgvNovelList_MouseClick(object sender, MouseEventArgs e)
        {
            var ht = dgvNovelList.HitTest(e.X, e.Y);

            if (ht.Type == DataGridViewHitTestType.None)
            {
                dgvNovelList.ClearSelection();
            }
        }

        private void dgvNovelList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvNovelList.Rows[e.RowIndex];
            string novelTilte = row.Cells["NovelTitle"].Value.ToString();
            Novel novel = NovelLibrary.Instance.GetNovel(novelTilte);

            SetAllNovelUnread();
            novel.Reading = true;

            if (nrf == null || !nrf.Visible)
            {
                nrf = new NovelReaderForm();
                nrf.StartPosition = FormStartPosition.Manual;
                nrf.DesktopBounds = Configuration.Instance.NovelReaderRect;
                nrf.Show();
            }
            nrf.SetReadingNovel(novel);
        }

        private void dgvNovelList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            
            DataGridViewRow row = dgvNovelList.Rows[e.RowIndex];
            Novel.NovelState state = (Novel.NovelState)Enum.Parse(typeof(Novel.NovelState), row.Cells["State"].Value.ToString());
            bool isReading = NovelLibrary.Instance.GetNovel(row.Cells["NovelTitle"].Value.ToString()).Reading;
            int x = e.RowBounds.Left + (penWidth / 2);
            int y = e.RowBounds.Top + (penWidth / 2);
            int width = e.RowBounds.Width - penWidth;
            int height = e.RowBounds.Height - penWidth;
            /*
            if (isReading)
                e.Graphics.DrawRectangle(redPen, x, y, width, height);
            else
                e.Graphics.DrawRectangle(whitePen, x, y, width, height);
            Console.WriteLine("Drawing with " + e.RowIndex + " " + isReading);
            */
        }

        private void refreshUpdateLabelTimer_Tick(object sender, EventArgs e)
        {
            labelLastUpdateTime.Text = Util.GetUpdateTimeString(Configuration.Instance.LastFullUpdateTime);
        }

        /*============PrivateFunction=======*/

        private void BindGrid()
        {
            dgvNovelList.AutoGenerateColumns = false;

            DataGridViewCell novelTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCell rankingCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterCountCell = new DataGridViewTextBoxCell();
            DataGridViewCell newChapterCountCell = new DataGridViewTextBoxCell();
            DataGridViewCheckBoxCell makeAudioCell = new DataGridViewCheckBoxCell();
            UpdateDataGridViewProgressCell updateProgressCell = new UpdateDataGridViewProgressCell();

            DataGridViewTextBoxColumn novelTitleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = novelTitleCell,
                Name = "NovelTitle",
                HeaderText = "Novel Title",
                DataPropertyName = "NovelTitle",
                Width = 200,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn rankingColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = rankingCell,
                Name = "Rank",
                HeaderText = "Rank",
                DataPropertyName = "Rank",
                Width = 100,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn chapterCountColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterCountCell,
                Name = "ChapterCount",
                HeaderText = "Chapter Count",
                DataPropertyName = "ChapterCount",
                Width = 150,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn newChapterCountColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = newChapterCountCell,
                Name = "NewChapterCount",
                HeaderText = "New Chapter Count",
                DataPropertyName = "NewChapterCount",
                Width = 150,
                ReadOnly = true
            };

            DataGridViewComboBoxColumn stateColumn = new DataGridViewComboBoxColumn()
            {
                Name = "State",
                HeaderText = "State",
                DataPropertyName = "State",
                DataSource = Enum.GetValues(typeof(Novel.NovelState)),
                ValueType = typeof(Novel.NovelState),
                Width = 100
            };

            DataGridViewCheckBoxColumn makeAudioColumn = new DataGridViewCheckBoxColumn()
            {
                CellTemplate = makeAudioCell,
                Name = "MakeAudio",
                HeaderText = "Make Audio",
                DataPropertyName = "MakeAudio",
                Width = 100,
            };

            UpdateDataGridViewProgressColumn updateProgressColumn = new UpdateDataGridViewProgressColumn()
            {
                CellTemplate = updateProgressCell,
                Name = "UpdateProgress",
                HeaderText = "Update Progress",
                DataPropertyName = "UpdateProgress",
                Width = 100,
            };
            
            dgvNovelList.Columns.Add(novelTitleColumn);
            dgvNovelList.Columns.Add(rankingColumn);
            dgvNovelList.Columns.Add(chapterCountColumn);
            dgvNovelList.Columns.Add(newChapterCountColumn);
            dgvNovelList.Columns.Add(stateColumn);
            dgvNovelList.Columns.Add(makeAudioColumn);
            dgvNovelList.Columns.Add(updateProgressColumn);

            dgvNovelList.DataSource = NovelLibrary.Instance.NovelList;


        }

        private void ModifyCellStyle(int rowIndex)
        {
            DataGridViewRow row = dgvNovelList.Rows[rowIndex];

            Novel.NovelState state = (Novel.NovelState)Enum.Parse(typeof(Novel.NovelState), row.Cells["State"].Value.ToString());
            bool isReading = NovelLibrary.Instance.GetNovel(row.Cells["NovelTitle"].Value.ToString()).Reading;

            if (state == Novel.NovelState.Active)
            {

                row.DefaultCellStyle.BackColor = Color.LightBlue;
                row.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;

            }
            else if (state == Novel.NovelState.Inactive)
            {

                row.DefaultCellStyle.BackColor = Color.LightPink;
                row.DefaultCellStyle.SelectionBackColor = Color.Firebrick;
            }
            else if (state == Novel.NovelState.Completed)
            {

                row.DefaultCellStyle.BackColor = Color.LightGreen;
                row.DefaultCellStyle.SelectionBackColor = Color.Green;
            }
            else if (state == Novel.NovelState.Dropped)
            {

                row.DefaultCellStyle.BackColor = Color.LightGray;
                row.DefaultCellStyle.SelectionBackColor = Color.Gray;
            }
        }

        private void SetAllNovelUnread()
        {
            foreach (Novel n in NovelLibrary.Instance.NovelList)
                n.Reading = false;
        }

        

        

        

    }
}
