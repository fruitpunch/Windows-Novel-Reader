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

    public class TTSDataGridViewProgressColumn : DataGridViewImageColumn
    {
        public TTSDataGridViewProgressColumn()
        {
            CellTemplate = new TTSDataGridViewProgressCell();
        }
    }

    class TTSDataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        static TTSDataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public TTSDataGridViewProgressCell()
        {
            this.ValueType = typeof(int);
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
            int progressVal = (int)value;
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
                g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
            }
            else
            {
                // draw the text
                if (this.DataGridView.CurrentRow.Index == rowIndex)
                    g.DrawString(progressVal.ToString() + "%", cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 2);
                else
                    g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
            }
        }
    }

    public partial class TTSController : UserControl
    {
        public TTSController()
        {
            InitializeComponent();
            BindGrid();
            BackgroundService.Instance.ttsController = this;
        }

        /*============EventHandler==========*/

        private void udThreadCount_ValueChanged(object sender, EventArgs e)
        {
            Configuration.Instance.TTSThreadCount = (int)udThreadCount.Value;
            BackgroundService.Instance.ttsScheduler.Threads = (int)udThreadCount.Value;
        }

        private void TTSController_Load(object sender, EventArgs e)
        {
            dgvTTS.DataSource = BackgroundService.Instance.ttsScheduler.RequestList;
        }

        /*============PublicFunction========*/

        public void SetThreadCount(int count)
        {
            udThreadCount.Value = count;
        }

        /*============PrivateFunction=======*/

        private void BindGrid()
        {
            udThreadCount.Value = Configuration.Instance.TTSThreadCount;

            dgvTTS.AutoGenerateColumns = false;

            DataGridViewCell novelTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterIndexCell = new DataGridViewTextBoxCell();
            DataGridViewCell requestPriorityCell = new DataGridViewTextBoxCell();
            TTSDataGridViewProgressCell progressCell = new TTSDataGridViewProgressCell();

            DataGridViewTextBoxColumn novelTitleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = novelTitleCell,
                Name = "NovelTitle",
                HeaderText = "Novel Title",
                DataPropertyName = "NovelTitle",
                Width = 200,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn chapterTitleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterTitleCell,
                Name = "ChapterTitle",
                HeaderText = "Chapter Title",
                DataPropertyName = "ChapterTitle",
                Width = 200,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn chapterIndexColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterIndexCell,
                Name = "ChapterIndex",
                HeaderText = "Chapter Index",
                DataPropertyName = "ChapterIndex",
                Width = 100,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn requestPriorityColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = requestPriorityCell,
                Name = "Priority",
                HeaderText = "Priority",
                DataPropertyName = "Priority",
                Width = 100,
                ReadOnly = true
            };
            
            TTSDataGridViewProgressColumn progressColumn = new TTSDataGridViewProgressColumn()
            {
                Name = "Progress",
                HeaderText = "Progress",
                DataPropertyName = "Progress",
                Width = 197
            };
            
            dgvTTS.Columns.Add(novelTitleColumn);
            dgvTTS.Columns.Add(chapterTitleColumn);
            dgvTTS.Columns.Add(chapterIndexColumn);
            dgvTTS.Columns.Add(requestPriorityColumn);
            dgvTTS.Columns.Add(progressColumn);

            

        }

        
    }
}
