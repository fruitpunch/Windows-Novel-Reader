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
        public ApplicationForm()
        {
            InitializeComponent();
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
    }
}
