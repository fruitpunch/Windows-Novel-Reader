using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Source;

namespace NovelReader
{
    public partial class AddNovelForm : Form
    {

        private SourceLocation sourceLocation;
        private string sourceID;
        private string novelTitle;
        private NovelSource source;
        private bool validSource = false;
        public AddNovelForm()
        {
            InitializeComponent();
            
            sourceSelector.DataSource = Enum.GetValues(typeof(SourceLocation));
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            

            if(inputNovelTitle.Text.Length == 0)
            {
                MessageBox.Show("Invalid Novel Title", "Novel title must not be empty.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (inputSourceID.Text.Length == 0)
            {
                MessageBox.Show("Invalid Novel Source ID", "Novel Source ID must not be empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!validSource)
            {
                MessageBox.Show("Please validate novel source first", "Invalid Novel Source ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            novelTitle = inputNovelTitle.Text;
            sourceLocation = (SourceLocation)Enum.Parse(typeof(SourceLocation), sourceSelector.SelectedItem.ToString());
            string message = "";
            bool result = BackgroundService.Instance.AddNovel(novelTitle, source, out message);

            if (!result)
            {
                MessageBox.Show(message, "Add Novel Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void inputSourceID_TextChanged(object sender, EventArgs e)
        {
            validSource = false;
            labelStatus.Text = "";
            labelStatus.ForeColor = Color.Black;
            if (inputSourceID.Text.Length > 0)
            {
                if (textCheckerTimer.Enabled)
                    textCheckerTimer.Stop();
                textCheckerTimer.Start();
            }
            else
            {
                textCheckerTimer.Stop();
                labelStatus.Text = "";
            }
        }

        private void sourceSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            InitiateValidation();
        }

        private void sourceChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            source = SourceManager.GetSource(sourceLocation, sourceID);
            Tuple<bool, string> result = source.VerifySource();
            e.Result = result;
            
        }

        private void sourceChecker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Tuple<bool, string> result = (Tuple<bool, string>)e.Result;
            if (result.Item1)
            {
                labelStatus.Text = "Valid ID.";
                labelStatus.ForeColor = Color.Green;
                inputNovelTitle.Text = result.Item2;
                validSource = true;
            }
            else
            {
                labelStatus.Text = "Invalid ID";
                labelStatus.ForeColor = Color.Red;
                inputNovelTitle.Text = "";
                validSource = false;
            }
            networkTimeoutTimer.Stop();
        }

        private void networkTimer_Tick(object sender, EventArgs e)
        {
            if (sourceChecker.IsBusy)
            {
                labelStatus.Text = "Network Timeout";
                sourceChecker.Dispose();
            }
            networkTimeoutTimer.Stop();
        }

        private void textCheckerTimer_Tick(object sender, EventArgs e)
        {
            InitiateValidation();
        }

        private void btnSourceLink_Click(object sender, EventArgs e)
        {
            sourceLocation = (SourceLocation)Enum.Parse(typeof(SourceLocation), sourceSelector.SelectedItem.ToString());
            string url = SourceManager.GetSourceURL(sourceLocation);
            if (url != null)
            {
                System.Diagnostics.ProcessStartInfo sourceWebProcess = new System.Diagnostics.ProcessStartInfo(url);
                System.Diagnostics.Process.Start(sourceWebProcess);
            }
        }

        /*============Private Function======*/
        private void InitiateValidation()
        {
            if (inputSourceID.Text.Length == 0)
                return;

            string novelId = inputSourceID.Text;
            textCheckerTimer.Stop();

            if (!sourceChecker.IsBusy)
            {
                sourceLocation = (SourceLocation)Enum.Parse(typeof(SourceLocation), sourceSelector.SelectedItem.ToString());
                sourceID = inputSourceID.Text;
                labelStatus.Text = "Checking Source ID.....";
                labelStatus.ForeColor = Color.Black;
                networkTimeoutTimer.Start();
                sourceChecker.RunWorkerAsync();
            }
        }



    }
}
