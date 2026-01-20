using LibVLCSharp.Shared;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace myRTSPStreamer
{
    public partial class Form1 : Form
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
       private int restartAttempts = 0;
        private const int MaxRestartAttempts = 5;
        private Timer timerStreamMonitor;

        public Form1()
        {
            InitializeComponent();
            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            videoView1.MediaPlayer = _mediaPlayer;

            timerAutoSnapshot.Interval = 60000; // default, will be updated from numInterval

            LoadSettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text += " : v" + Assembly.GetExecutingAssembly().GetName().Version; // put in the version number

            txtSnapshotNumber.Text = "1";
            Properties.Settings.Default.LastSnapshotDate = DateTime.Now.ToShortDateString();

        }

        private void LoadSettings()
        {
            txtUsername.Text = Properties.Settings.Default.Username;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtIpAddress.Text = Properties.Settings.Default.IpAddress;
            txtPort.Text = Properties.Settings.Default.Port;
            txtStreamPath.Text = Properties.Settings.Default.StreamPath;
            txtSnapshotFolder.Text = Properties.Settings.Default.SnapshotFolder;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string rtspUrl = BuildRtspUrl();
                Log("Starting stream: " + rtspUrl);

                var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
                _mediaPlayer.Play(media);

                UpdateTimerFromUI();
                lblStatus.Text = "Streaming...";

                //Timer to check if stream is alive, if not restart it.
                timerStreamMonitor = new Timer();
                timerStreamMonitor.Interval = 10000; // check every 10 seconds
                timerStreamMonitor.Tick += timerStreamMonitor_Tick;
                timerStreamMonitor.Start();
            }
            catch (Exception ex)
            {
                Log("Error starting stream: " + ex.Message);
            }
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _mediaPlayer.Stop();
                timerAutoSnapshot.Stop();
                timerStreamMonitor.Stop();
                lblStatus.Text = "Stopped";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error stopping stream";
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            SaveSnapshot();
        }

        private void chkAutoSnapshot_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTimerFromUI();
        }

        private void timerAutoSnapshot_Tick(object sender, EventArgs e)
        {
            SaveSnapshot();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = txtSnapshotFolder.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtSnapshotFolder.Text = dlg.SelectedPath;
                }
            }
        }

        private void UpdateTimerFromUI()
        {
            if (chkAutoSnapshot.Checked)
            {
                int seconds = (int)numInterval.Value;
                timerAutoSnapshot.Interval = seconds * 1000;
                timerAutoSnapshot.Start();
            }
            else
            {
                timerAutoSnapshot.Stop();
            }
        }

        private void SaveSnapshot()
        {
            try
            {
                if (_mediaPlayer == null || !_mediaPlayer.IsPlaying)
                {
                    Log("Snapshot skipped: stream not playing.");
                    return;
                }

                string basePath = txtSnapshotFolder.Text.Trim();
                if (string.IsNullOrWhiteSpace(basePath))
                {
                    MessageBox.Show("Please select a snapshot folder.");
                    return;
                }

                string year = DateTime.Now.ToString("yyyy");    //DateTime.Now.Year.ToString("yyyy");
                string month = DateTime.Now.ToString("MMM");
                string day = DateTime.Now.ToString("dd");

                string folderPath = Path.Combine(basePath, year, month, day);
                Directory.CreateDirectory(folderPath);

                string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");

                //Reset unique number if we are past midnight and clear the logfile.
                string today = DateTime.Now.ToShortDateString();

                if (Properties.Settings.Default.LastSnapshotDate != today)
                {
                    // New day → reset counter
                    txtSnapshotNumber.Text = "1";
                    Properties.Settings.Default.LastSnapshotDate = today;
                    Properties.Settings.Default.Save();
                    Log("New day detected — snapshot counter reset to 1.");

                    ClearLog(); // clear log file
                }

                if (!int.TryParse(txtSnapshotNumber.Text, out int snapNum)) snapNum = 1;//if garbage reset to 1

                string filename = $"{timestamp}_{snapNum}.jpg";
                string fullPath = Path.Combine(folderPath, filename);

                _mediaPlayer.TakeSnapshot(0, fullPath, 0, 0);

                // Increment for next time
                snapNum++;
                txtSnapshotNumber.Text = snapNum.ToString();


                // Get file size
                long fileSize = new FileInfo(fullPath).Length;
                string sizeText = $"{fileSize / 1024.0:F2} KB";

                Log($"Snapshot saved: {filename} ({sizeText})");
            }
            catch (Exception ex)
            {
                Log("Error saving snapshot: " + ex.Message);
            }
        }

        private void Log(string message)
        {
            string FolderPath;
            string timestamp = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            string line = $"{timestamp}  {message}";
            DateTime myDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(txtSnapshotFolder.Text))
            {
                FolderPath = Path.Combine(txtSnapshotFolder.Text, myDate.ToString("yyyy"),
                    myDate.ToString("MMM"), myDate.ToString("dd"));
            }
            else
            {
                FolderPath = Path.Combine("C:\\timelapse", myDate.ToString("yyyy"),
                    myDate.ToString("MMM"), myDate.ToString("dd"));
            }

            Directory.CreateDirectory(FolderPath); //create if not already there

            // Write to on-screen log
            txtLog.AppendText(line + Environment.NewLine);

            // Write to log.txt if snapshot folder is set
            try
            {
                if (!string.IsNullOrWhiteSpace(txtSnapshotFolder.Text))
                {
                    string logPath = Path.Combine(FolderPath, "log.txt");
                    File.AppendAllText(logPath, line + Environment.NewLine);
                }
            }
            catch
            {
                // Avoid crashing if log file fails
            }
        }

        
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            ClearLog();
        }

        private void ClearLog()
        {
            txtLog.Clear();
            Log("Log panel cleared.");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Log("Application closing...");

            try
            {
                timerAutoSnapshot.Stop();

                if (_mediaPlayer != null)
                {
                    if (_mediaPlayer.IsPlaying)
                        _mediaPlayer.Stop();

                    _mediaPlayer.Dispose();
                }

                _libVLC?.Dispose();
            }
            catch (Exception ex)
            {
                Log("Error during cleanup: " + ex.Message);
            }

            Application.Exit();
        }

        private string BuildRtspUrl()
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();
            string ip = txtIpAddress.Text.Trim();
            string port = txtPort.Text.Trim();
            string path = txtStreamPath.Text.Trim();

            return $"rtsp://{user}:{pass}@{ip}:{port}/{path}";
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Username = txtUsername.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.IpAddress = txtIpAddress.Text;
            Properties.Settings.Default.Port = txtPort.Text;
            Properties.Settings.Default.StreamPath = txtStreamPath.Text;
            Properties.Settings.Default.SnapshotFolder = txtSnapshotFolder.Text;

            Properties.Settings.Default.Save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        // If we think stream is not running lets try restart it
        private void timerStreamMonitor_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer == null)
                return;

            // If VLC thinks it's playing, nothing to do
            if (_mediaPlayer.IsPlaying)
            {
                restartAttempts = 0; // reset counter
                return;
            }

            // If not playing, attempt restart
            restartAttempts++;

            Log($"Stream appears stopped. Attempting restart {restartAttempts}/{MaxRestartAttempts}...");

            if (restartAttempts <= MaxRestartAttempts)
            {
                TryRestartStream();
            }
            else
            {
                Log("Stream could not be restarted. Stopping snapshots.");
                timerAutoSnapshot.Stop();
            }
        }

        private void TryRestartStream()
        {
            try
            {
                string rtspUrl = BuildRtspUrl();
                Log("Restarting stream: " + rtspUrl);

                var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
                _mediaPlayer.Play(media);
            }
            catch (Exception ex)
            {
                Log("Restart failed: " + ex.Message);
            }
        }


    }
}
