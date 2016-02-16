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
    [System.ComponentModel.DefaultBindingProperty("Novel")]
    public partial class NovelTileController : UserControl
    {
        private Novel _novel;

        public Novel Novel
        {
            get { return this._novel; }
        }

        public NovelTileController(Novel novel)
        {
            InitializeComponent();
            this._novel = novel;
            
            SetController();
        }

        private void novel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName + " has changed for " + Novel);
            RefreshController();
        }


        private void novelStateCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Novel.NovelState newState;
            Enum.TryParse<Novel.NovelState>(novelStateCB.SelectedValue.ToString(), out newState);
            Novel.State = newState;
            novelTitleLabel.Focus();
        }


        private void SetController()
        {
            Novel.PropertyChanged += novel_PropertyChanged;
            this.novelTitleLabel.Text = Novel.NovelTitle;
            this.novelStateCB.DataSource = Enum.GetValues(typeof(Novel.NovelState));
            RefreshController();
        }

        private void RefreshController()
        {
            this.newChapterLabel.Visible = false;
            this.novelStateCB.SelectedItem = Novel.State;

            if (Novel.NovelChapters.Count > 0)
                this.newestChapterInfoLabel.Text = Novel.NovelChapters.Last<Chapter>().ChapterTitle;

            if (Novel.Reading)
                btnMakeAudio.BackColor = Color.DarkCyan;
            else
                btnMakeAudio.BackColor = Color.Transparent;

            if (_novel.ChapterCountStatus.Contains("new"))
                newChapterLabel.Visible = true;
            else
                newChapterLabel.Visible = false;


            UpdateBackColor();
        }

        private void UpdateBackColor()
        {

            Novel.NovelState state = Novel.State;

            if (state == Novel.NovelState.Active)
            {
                BackColor = Color.LightBlue;
                novelStateCB.BackColor = Color.LightBlue;
            }
            else if (state == Novel.NovelState.Inactive)
            {
                BackColor = Color.LightPink;
                novelStateCB.BackColor = Color.LightPink;
            }
            else if (state == Novel.NovelState.Completed)
            {
                BackColor = Color.LightGreen;
                novelStateCB.BackColor = Color.LightGreen;
            }
            else if (state == Novel.NovelState.Dropped)
            {
                BackColor = Color.LightGray;
                novelStateCB.BackColor = Color.LightGray;
            }
        }

        private void NovelTileController_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine("Double Click");
        }
    }
}
