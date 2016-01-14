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
