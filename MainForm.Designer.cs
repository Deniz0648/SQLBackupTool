using System.Windows.Forms;

namespace BackupTool
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Temizleme işlemi (Designer tarafından)
        /// </summary>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            setting = new Button();
            loggerArea = new RichTextBox();
            _notifyIcon = new NotifyIcon(components);
            restart = new Button();
            stop = new Button();
            start = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // setting
            // 
            setting.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            setting.Location = new Point(12, 530);
            setting.Name = "setting";
            setting.Size = new Size(533, 38);
            setting.TabIndex = 2;
            setting.Text = "Ayarlar";
            setting.Click += Setting_Click;
            // 
            // loggerArea
            // 
            loggerArea.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            loggerArea.Location = new Point(12, 70);
            loggerArea.Name = "loggerArea";
            loggerArea.Size = new Size(533, 450);
            loggerArea.TabIndex = 1;
            loggerArea.Text = "";
            // 
            // restart
            // 
            restart.Dock = DockStyle.Fill;
            restart.Location = new Point(357, 3);
            restart.Name = "restart";
            restart.Size = new Size(173, 34);
            restart.TabIndex = 2;
            restart.Text = "Yeniden Başlat";
            restart.Click += Restart_Click;
            // 
            // stop
            // 
            stop.Dock = DockStyle.Fill;
            stop.Location = new Point(180, 3);
            stop.Name = "stop";
            stop.Size = new Size(171, 34);
            stop.TabIndex = 1;
            stop.Text = "Durdur";
            stop.Click += Stop_Click;
            // 
            // start
            // 
            start.Dock = DockStyle.Fill;
            start.Location = new Point(3, 3);
            start.Name = "start";
            start.Size = new Size(171, 34);
            start.TabIndex = 0;
            start.Text = "Başlat";
            start.Click += Start_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel1.Controls.Add(start, 0, 0);
            tableLayoutPanel1.Controls.Add(stop, 1, 0);
            tableLayoutPanel1.Controls.Add(restart, 2, 0);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(533, 40);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // MainForm
            // 
            ClientSize = new Size(557, 591);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(loggerArea);
            Controls.Add(setting);
            Name = "MainForm";
            Text = "SQL Backup Tool";
            Load += MainForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }


        #endregion
        private Button setting;
        private RichTextBox loggerArea;
        private Panel panel1;
        private Button restart;
        private Button stop;
        private Button start;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
