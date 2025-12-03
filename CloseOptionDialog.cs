using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupTool
{
    public partial class CloseOptionDialog : Form
    {
        public bool MinimizeToTraySelected => radioMinimizeTray.Checked;

        public CloseOptionDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!radioMinimizeTray.Checked && !radioClose.Checked)
            {
                MessageBox.Show("Lütfen bir seçenek seçin.");
                this.DialogResult = DialogResult.None;
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
