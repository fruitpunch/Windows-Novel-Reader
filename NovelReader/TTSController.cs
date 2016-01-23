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
            SetControl();
            BackgroundService.Instance.ttsController = this;
        }

        /*============EventHandler==========*/

        private void udThreadCount_ValueChanged(object sender, EventArgs e)
        {
            Configuration.Instance.TTSThreadCount = (int)udThreadCount.Value;
            BackgroundService.Instance.ttsScheduler.Threads = (int)udThreadCount.Value;
        }

        private void udTTSSpeed_ValueChanged(object sender, EventArgs e)
        {
            Configuration.Instance.TTSSpeed = (int)udTTSSpeed.Value;
            BackgroundService.Instance.ResetTTSList();
        }

        private void TTSController_Load(object sender, EventArgs e)
        {
            //dgvTTS.DataSource = BackgroundService.Instance.ttsScheduler.RequestList;
        }

        private void btnResetList_Click(object sender, EventArgs e)
        {
            BackgroundService.Instance.ResetTTSList();
        }

        /*============PublicFunction========*/

        public void SetThreadCount(int count)
        {
            udThreadCount.Value = count;
        }

        /*============PrivateFunction=======*/

        private void SetControl()
        {
            udThreadCount.Value = Configuration.Instance.TTSThreadCount;
            udTTSSpeed.Value = Configuration.Instance.TTSSpeed;

            dgvTTS.AutoGenerateColumns = false;

            DataGridViewCell novelTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterTitleCell = new DataGridViewTextBoxCell();
            DataGridViewCell chapterIndexCell = new DataGridViewTextBoxCell();
            DataGridViewCell requestPriorityCell = new DataGridViewTextBoxCell();
            DataGridViewCell requestRateCell = new DataGridViewTextBoxCell();
            TTSDataGridViewProgressCell progressCell = new TTSDataGridViewProgressCell();

            DataGridViewTextBoxColumn novelTitleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = novelTitleCell,
                Name = "NovelTitle",
                HeaderText = "Novel Title",
                DataPropertyName = "NovelTitle",
                Width = 150,
                ReadOnly = true
            };

            DataGridViewTextBoxColumn chapterTitleColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = chapterTitleCell,
                Name = "ChapterTitle",
                HeaderText = "Chapter Title",
                DataPropertyName = "ChapterTitle",
                Width = 150,
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

            DataGridViewTextBoxColumn requestRateColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = requestRateCell,
                Name = "Rate",
                HeaderText = "Speak Rate",
                DataPropertyName = "Rate",
                Width = 100,
                ReadOnly = true
            };
            
            TTSDataGridViewProgressColumn progressColumn = new TTSDataGridViewProgressColumn()
            {
                Name = "Progress",
                HeaderText = "Progress",
                DataPropertyName = "Progress",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            
            dgvTTS.Columns.Add(novelTitleColumn);
            dgvTTS.Columns.Add(chapterTitleColumn);
            dgvTTS.Columns.Add(chapterIndexColumn);
            dgvTTS.Columns.Add(requestPriorityColumn);
            dgvTTS.Columns.Add(requestRateColumn);
            dgvTTS.Columns.Add(progressColumn);

            

        }

        


        

        
    }
}
