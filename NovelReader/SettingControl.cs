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
    public partial class SettingControl : UserControl
    {
        public SettingControl()
        {
            InitializeComponent();
            SetControl();
        }

        /*============PrivateFunction=======*/

        private void SetControl()
        {
            dgvLanguageSelector.AutoGenerateColumns = false;
            Tuple<List<string>, List<string>> languageVoiceData = Util.GetLanguageVoice();
            List<string> languageList = languageVoiceData.Item1;
            List<string> voiceList = languageVoiceData.Item2;
            voiceList.Insert(0, "No Voice Selected");
            Dictionary<string, string> languageVoiceDictionary = Configuration.Instance.LanguageVoiceDictionary;
            if (languageVoiceDictionary == null)
                languageVoiceDictionary = new Dictionary<string, string>();

            DataGridViewCell languageCell = new DataGridViewTextBoxCell();
            DataGridViewComboBoxCell voiceCell = new DataGridViewComboBoxCell();

            DataGridViewTextBoxColumn languageColumn = new DataGridViewTextBoxColumn()
            {
                CellTemplate = languageCell,
                Name = "Language",
                HeaderText = "Language",
                Width = 250,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            DataGridViewComboBoxColumn voiceColumn = new DataGridViewComboBoxColumn()
            {
                CellTemplate = voiceCell,
                Name = "Voice",
                HeaderText = "Voice",
                DataSource = voiceList,
                ValueType = typeof(string),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = false,
                //Width = 100
            };

            dgvLanguageSelector.Columns.Add(languageColumn);
            dgvLanguageSelector.Columns.Add(voiceColumn);

            foreach (KeyValuePair<string, string> kvp in languageVoiceDictionary)
            {
                dgvLanguageSelector.Rows.Add(kvp.Key, kvp.Value);
            }
            foreach (string language in languageList)
            {
                if (!languageVoiceDictionary.ContainsKey(language))
                {
                    dgvLanguageSelector.Rows.Add(language, "No Voice Selected");
                    languageVoiceDictionary.Add(language, "No Voice Selected");
                }
            }
            
            Configuration.Instance.LanguageVoiceDictionary = languageVoiceDictionary;

        }

        private void dgvLanguageSelector_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(e.RowIndex + " " + e.ColumnIndex + " value changed");
            if (dgvLanguageSelector.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                string language = dgvLanguageSelector.Rows[e.RowIndex].Cells["Language"].Value.ToString();
                string newVoice = dgvLanguageSelector.Rows[e.RowIndex].Cells["Voice"].Value.ToString();
                Configuration.Instance.LanguageVoiceDictionary[language] = newVoice;


            }
        }

        private void dgvLanguageSelector_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvLanguageSelector.IsCurrentCellDirty)
                dgvLanguageSelector.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvLanguageSelector_CellClick(object sender, DataGridViewCellEventArgs e)
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
    }
}
