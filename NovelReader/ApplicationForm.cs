using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelReader
{
    public partial class ApplicationForm : Form
    {
        public NovelListController novelListController;
        public TTSController ttsController;
        public SettingController settingController;
        public NovelTileViewController tileViewController;
        public NovelSourceController novelSourceController;
         
        public ApplicationForm()
        {
            Util.LoadComponents();
            InitializeComponent();
            novelListController = new NovelListController();
            ttsController = new TTSController();
            settingController = new SettingController();
            //tileViewController = new NovelTileViewController();
            novelSourceController = new NovelSourceController();

            novelListController.Dock = DockStyle.Fill;
            ttsController.Dock = DockStyle.Fill;
            settingController.Dock = DockStyle.Fill;
            //tileViewController.Dock = DockStyle.Fill;
            novelSourceController.Dock = DockStyle.Fill;

            novelListController.Visible = true;
            ttsController.Visible = false;
            settingController.Visible = false;
            //tileViewController.Visible = false;
            novelSourceController.Visible = false;

            btnNovelList.BackColor = this.BackColor;
            btnTTSList.BackColor = buttonContainerPanel.BackColor;
            btnSetting.BackColor = buttonContainerPanel.BackColor;
            btnNovelTileView.BackColor = buttonContainerPanel.BackColor;

            controlContainerPanel.Controls.Add(novelListController);
            controlContainerPanel.Controls.Add(ttsController);
            controlContainerPanel.Controls.Add(settingController);
            //controlContainerPanel.Controls.Add(tileViewController);
            controlContainerPanel.Controls.Add(novelSourceController);
        }

        private void ApplicationForm_Load(object sender, EventArgs e)
        {
            
            this.DesktopBounds = Configuration.Instance.ApplicationRect;
            if (Configuration.Instance.ApplicationMaximized)
                this.WindowState = FormWindowState.Maximized;

            Console.WriteLine("Application load");
            
        }

        private void ApplicationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Configuration.Instance.ApplicationRect = this.DesktopBounds;
            if (this.WindowState == FormWindowState.Maximized)
                Configuration.Instance.ApplicationMaximized = true;
            else
                Configuration.Instance.ApplicationMaximized = false;
            Util.SaveComponents();
        }

        private void btnNovelList_Click(object sender, EventArgs e)
        {
            btnNovelList.BackColor = this.BackColor;
            btnTTSList.BackColor = buttonContainerPanel.BackColor;
            btnSetting.BackColor = buttonContainerPanel.BackColor;
            btnNovelTileView.BackColor = buttonContainerPanel.BackColor;
            novelListController.Visible = true;
            ttsController.Visible = false;
            settingController.Visible = false;
            //tileViewController.Visible = false;
            novelSourceController.Close();
        }

        private void btnTTSList_Click(object sender, EventArgs e)
        {
            btnNovelList.BackColor = buttonContainerPanel.BackColor;
            btnTTSList.BackColor = this.BackColor;
            btnSetting.BackColor = buttonContainerPanel.BackColor;
            btnNovelTileView.BackColor = buttonContainerPanel.BackColor;
            novelListController.Visible = false;
            ttsController.Visible = true;
            settingController.Visible = false;
            //tileViewController.Visible = false;
            novelSourceController.Close();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            btnNovelList.BackColor = buttonContainerPanel.BackColor;
            btnTTSList.BackColor = buttonContainerPanel.BackColor;
            btnSetting.BackColor = this.BackColor;
            btnNovelTileView.BackColor = buttonContainerPanel.BackColor;
            novelListController.Visible = false;
            ttsController.Visible = false;
            settingController.Visible = true;
            //tileViewController.Visible = false;
            novelSourceController.Close();

        }

        private void btnNovelTileView_Click(object sender, EventArgs e)
        {
            btnNovelList.BackColor = buttonContainerPanel.BackColor;
            btnTTSList.BackColor = buttonContainerPanel.BackColor;
            btnSetting.BackColor = buttonContainerPanel.BackColor;
            btnNovelTileView.BackColor = this.BackColor;
            novelListController.Visible = false;
            ttsController.Visible = false;
            settingController.Visible = false;
            //tileViewController.Visible = true;
            novelSourceController.Close();
        }
    }
}
