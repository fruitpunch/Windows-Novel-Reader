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


        public AddNovelForm()
        {
            InitializeComponent();

            sourceSelector.DataSource = Enum.GetValues(typeof(SourceManager.Sources));
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string newNovelTitle = null;
            SourceManager.Sources source;
            int sourceID = -1;
            

            if(inputNovelTitle.Text.Length == 0)
            {
                MessageBox.Show("Invalid Novel Title", "Novel title must not be empty.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Int32.TryParse(inputSourceID.Text, out sourceID))
            {
                MessageBox.Show("Invalid Novel Source ID", "Novel Source ID must be a positive integer.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            newNovelTitle = inputNovelTitle.Text;
            source = (SourceManager.Sources)Enum.Parse(typeof(SourceManager.Sources), sourceSelector.SelectedItem.ToString());
            Tuple<bool, string> result = BackgroundService.Instance.AddNovel(newNovelTitle, source, sourceID);

            if (!result.Item1)
            {
                MessageBox.Show("Add Novel Failed", result.Item2, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
