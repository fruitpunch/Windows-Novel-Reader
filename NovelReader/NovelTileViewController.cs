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
    public partial class NovelTileViewController : UserControl
    {
        BindingList<Novel> novelList;
        List<Control> controllerList;

        public NovelTileViewController()
        {
            InitializeComponent();
            SetController();

        }

        private void btnAddNovel_Click(object sender, EventArgs e)
        {
            AddNovelForm anf = new AddNovelForm();
            Rectangle r = this.ParentForm.DesktopBounds;
            int x = r.X + (r.Width / 2) - 150;
            int y = r.Y + (r.Height / 2) - 150;
            anf.StartPosition = FormStartPosition.Manual;
            anf.DesktopBounds = new Rectangle(new Point(x, y), anf.Size);
            anf.ShowDialog();
        }

        private void refreshUpdateLabelTimer_Tick(object sender, EventArgs e)
        {
            labelLastUpdateTime.Text = Util.GetUpdateTimeString(Configuration.Instance.LastFullUpdateTime);
        }

        private void upUpdateFreq_ValueChanged(object sender, EventArgs e)
        {
            BackgroundService.Instance.UpdateTimerInterval((int)upUpdateFreq.Value);
        }


        private void btnDeleteNovel_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            BackgroundService.Instance.UpdateTTSTest();
        }

        private void NovelListChangedEvent(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    controllerList.Insert(e.NewIndex, new NovelTileController(novelList[e.NewIndex]));
                    RefreshController();
                    break;
                case ListChangedType.ItemDeleted:
                    controllerList.RemoveAt(e.NewIndex);
                    RefreshController();
                    break;
            }
        }


        private void SetController()
        {
            controllerList = new List<Control>();
            novelList = NovelLibrary.Instance.NovelList;
            novelList.RaiseListChangedEvents = true;
            novelList.ListChanged += NovelListChangedEvent;

            foreach (Novel novel in novelList)
                controllerList.Add(new NovelTileController(novel));

            RefreshController();
        }

        private void RefreshController()
        {
            novelFlowLayoutPanel.Controls.Clear();
            foreach (Control control in controllerList)
            {
                novelFlowLayoutPanel.Controls.Add(control);
            }
        }

        
    }
}
