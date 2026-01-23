using System.Drawing;
using System.Windows.Forms;
using LibVLCSharp.WinForms;
namespace myRTSPStreamer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Panel panelTop;
        private TextBox txtRtspUrl;
        private Button btnStart;
        private Button btnStop;
        private Button btnSnapshot;
        private CheckBox chkAutoSnapshot;
        private NumericUpDown numInterval;
        private TextBox txtSnapshotFolder;
        private Button btnBrowseFolder;
        private Timer timerAutoSnapshot;
        private Button btnClearLog;
        private Button btnExit;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtIpAddress;
        private TextBox txtPort;
        private TextBox txtStreamPath;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelTop = new System.Windows.Forms.Panel();
            this.txtSnapshotNumber = new System.Windows.Forms.TextBox();
            this.btnBrowseFolder = new System.Windows.Forms.Button();
            this.txtSnapshotFolder = new System.Windows.Forms.TextBox();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.chkAutoSnapshot = new System.Windows.Forms.CheckBox();
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtRtspUrl = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtStreamPath = new System.Windows.Forms.TextBox();
            this.timerAutoSnapshot = new System.Windows.Forms.Timer(this.components);
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.txtSnapshotNumber);
            this.panelTop.Controls.Add(this.btnBrowseFolder);
            this.panelTop.Controls.Add(this.txtSnapshotFolder);
            this.panelTop.Controls.Add(this.numInterval);
            this.panelTop.Controls.Add(this.chkAutoSnapshot);
            this.panelTop.Controls.Add(this.btnSnapshot);
            this.panelTop.Controls.Add(this.btnStop);
            this.panelTop.Controls.Add(this.btnStart);
            this.panelTop.Controls.Add(this.txtRtspUrl);
            this.panelTop.Controls.Add(this.btnClearLog);
            this.panelTop.Controls.Add(this.btnExit);
            this.panelTop.Controls.Add(this.txtUsername);
            this.panelTop.Controls.Add(this.txtPassword);
            this.panelTop.Controls.Add(this.txtIpAddress);
            this.panelTop.Controls.Add(this.txtPort);
            this.panelTop.Controls.Add(this.txtStreamPath);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1244, 84);
            this.panelTop.TabIndex = 1;
            // 
            // txtSnapshotNumber
            // 
            this.txtSnapshotNumber.Location = new System.Drawing.Point(1004, 65);
            this.txtSnapshotNumber.Name = "txtSnapshotNumber";
            this.txtSnapshotNumber.Size = new System.Drawing.Size(125, 26);
            this.txtSnapshotNumber.TabIndex = 17;
            // 
            // btnBrowseFolder
            // 
            this.btnBrowseFolder.Location = new System.Drawing.Point(578, 64);
            this.btnBrowseFolder.Name = "btnBrowseFolder";
            this.btnBrowseFolder.Size = new System.Drawing.Size(32, 23);
            this.btnBrowseFolder.TabIndex = 0;
            this.btnBrowseFolder.Text = "...";
            this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
            // 
            // txtSnapshotFolder
            // 
            this.txtSnapshotFolder.Location = new System.Drawing.Point(12, 65);
            this.txtSnapshotFolder.Name = "txtSnapshotFolder";
            this.txtSnapshotFolder.Size = new System.Drawing.Size(560, 26);
            this.txtSnapshotFolder.TabIndex = 1;
            this.txtSnapshotFolder.Text = "C:\\snapshots";
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(130, 39);
            this.numInterval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(120, 26);
            this.numInterval.TabIndex = 2;
            this.numInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // chkAutoSnapshot
            // 
            this.chkAutoSnapshot.Location = new System.Drawing.Point(12, 39);
            this.chkAutoSnapshot.Name = "chkAutoSnapshot";
            this.chkAutoSnapshot.Size = new System.Drawing.Size(104, 24);
            this.chkAutoSnapshot.TabIndex = 3;
            this.chkAutoSnapshot.Text = "Auto snapshot";
            this.chkAutoSnapshot.CheckedChanged += new System.EventHandler(this.chkAutoSnapshot_CheckedChanged);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Location = new System.Drawing.Point(620, 39);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(126, 25);
            this.btnSnapshot.TabIndex = 4;
            this.btnSnapshot.Text = "Snapshot now";
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(686, 8);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(60, 25);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(620, 8);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(60, 25);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtRtspUrl
            // 
            this.txtRtspUrl.Location = new System.Drawing.Point(12, 10);
            this.txtRtspUrl.Name = "txtRtspUrl";
            this.txtRtspUrl.Size = new System.Drawing.Size(600, 26);
            this.txtRtspUrl.TabIndex = 7;
            this.txtRtspUrl.Text = "rtsp://admin:@192.168.1.140:554/live/0/MAIN";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(620, 65);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(90, 23);
            this.btnClearLog.TabIndex = 8;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(716, 65);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 23);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(838, 12);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 26);
            this.txtUsername.TabIndex = 16;
            this.txtUsername.Text = "admin";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(838, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 26);
            this.txtPassword.TabIndex = 11;
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(838, 64);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(120, 26);
            this.txtIpAddress.TabIndex = 12;
            this.txtIpAddress.Text = "192.168.1.140";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(1000, 10);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(60, 26);
            this.txtPort.TabIndex = 13;
            this.txtPort.Text = "554";
            // 
            // txtStreamPath
            // 
            this.txtStreamPath.Location = new System.Drawing.Point(1000, 36);
            this.txtStreamPath.Name = "txtStreamPath";
            this.txtStreamPath.Size = new System.Drawing.Size(180, 26);
            this.txtStreamPath.TabIndex = 14;
            this.txtStreamPath.Text = "live/0/MAIN";
            // 
            // timerAutoSnapshot
            // 
            this.timerAutoSnapshot.Tick += new System.EventHandler(this.timerAutoSnapshot_Tick);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 25);
            this.lblStatus.Text = "Ready";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 580);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1250, 32);
            this.statusStrip1.TabIndex = 2;
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView1.Location = new System.Drawing.Point(0, 0);
            this.videoView1.MediaPlayer = null;
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(1244, 364);
            this.videoView1.TabIndex = 0;
            this.videoView1.Text = "videoView1";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1244, 114);
            this.txtLog.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1250, 580);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelTop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1244, 84);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.videoView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1244, 364);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtLog);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 463);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1244, 114);
            this.panel3.TabIndex = 2;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1250, 612);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "myRTSPStreamer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private TextBox txtSnapshotNumber;
        private ToolStripStatusLabel lblStatus;
        private StatusStrip statusStrip1;
        private VideoView videoView1;
        private TextBox txtLog;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
    }
}

