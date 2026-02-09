using LibVLCSharp.Shared;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace myRTSPStreamer
{
    public partial class Form1 : Form
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private int restartAttempts;
        private const int MaxRestartAttempts = 5;
        private readonly bool startedByWatchdog = false;


        public Form1(bool myRestartMode)
        {
            InitializeComponent(); // initialises libVLCSharp

            //true = watchdog is restarting it, false = open app only
            startedByWatchdog = myRestartMode;

            Core.Initialize();

            timerAutoSnapshot.Interval = 60000; // default, will be updated from numInterval

            LoadSettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitialiseVlc();

            LoadSettings(); //load settings from last session
            Text += " : v" + Assembly.GetExecutingAssembly().GetName().Version; // put in the version number

            txtbx_Next_Image_Number.Text = "1"; //reset to 1
            Properties.Settings.Default.LastSnapshotDate = DateTime.Now.ToShortDateString();

            if (startedByWatchdog)
            {
                Start(); //Auto start app
                Log("Auto restart as App was locked: " + DateTime.Now.ToString("HHmmss"));
                
                // overwrite with last saved number
                txtbx_Next_Image_Number.Text = Properties.Settings.Default.Next_Image_Number;
            }
        }

        private void LoadSettings()
        {
            txtUsername.Text = Properties.Settings.Default.Username;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtIpAddress.Text = Properties.Settings.Default.IpAddress;
            txtPort.Text = Properties.Settings.Default.Port;
            txtStreamPath.Text = Properties.Settings.Default.StreamPath;
            txtSnapshotFolder.Text = Properties.Settings.Default.SnapshotFolder;
            numupdn_Interval.Value = Properties.Settings.Default.Interval;
            chkbx_AutoSnapshot.Checked = Properties.Settings.Default.AutoSnapshot;
            txtbx_Next_Image_Number.Text = Properties.Settings.Default.Next_Image_Number;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            try
            {
                string rtspUrl = BuildRtspUrl();
                Log("Starting stream: " + rtspUrl);

                var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
                _mediaPlayer.Play(media);

                lblStatus.Text = "Streaming...";
                restartAttempts = 0;
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
               // timerStreamMonitor.Stop();
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
            if (chkbx_AutoSnapshot.Checked)
            {
                int seconds = (int)numupdn_Interval.Value;
                timerAutoSnapshot.Interval = seconds * 1000;
                timerAutoSnapshot.Start();
            }
            else
            {
                timerAutoSnapshot.Stop();
            }
        }

        private async void SaveSnapshot()
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

                //Reset unique number if we are past midnight and clear the logfile.
                string today = DateTime.Now.ToShortDateString();

                if (Properties.Settings.Default.LastSnapshotDate != today)
                {
                    // New day → reset counter
                    txtbx_Next_Image_Number.Text = "1";
                    Properties.Settings.Default.LastSnapshotDate = today;
                    Properties.Settings.Default.Save();
                    Log("New day detected — snapshot counter reset to 1.");
                    await SetTime(txtIpAddress.Text, txtUsername.Text, txtPassword.Text);
                    Log("Time Sync carried out");

                    ClearLog(); // clear log file
                }

                if (!int.TryParse(txtbx_Next_Image_Number.Text, out int snapNum)) snapNum = 1;//if garbage reset to 1


                string year = DateTime.Now.ToString("yyyy");    //DateTime.Now.Year.ToString("yyyy");
                string month = DateTime.Now.ToString("MMM");
                string day = DateTime.Now.ToString("dd");

                string folderPath = Path.Combine(basePath, year, month, day);
                Directory.CreateDirectory(folderPath);

                string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");

                string filename = $"{timestamp}_{snapNum}.jpg";
                string fullPath = Path.Combine(folderPath, filename);

                _mediaPlayer.TakeSnapshot(0, fullPath, 0, 0);
                UpdateHeartbeat(folderPath);

                // Increment for next time
                snapNum++;
                txtbx_Next_Image_Number.Text = snapNum.ToString();

                // Save incase it gets watchdog reset
                Properties.Settings.Default.Next_Image_Number = txtbx_Next_Image_Number.Text;
                Properties.Settings.Default.Save();


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

            txtLog.Invoke(new Action(() => { txtLog.AppendText(line + Environment.NewLine); }));

            //if (InvokeRequired)
            //{
            //    Invoke((MethodInvoker)delegate
            //    {
            //        // Write to on-screen log
            //        txtLog.AppendText(line + Environment.NewLine);
            //    });
            //}

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
            Properties.Settings.Default.Interval = numupdn_Interval.Value;
            Properties.Settings.Default.AutoSnapshot = chkbx_AutoSnapshot.Checked;
            Properties.Settings.Default.Next_Image_Number = txtbx_Next_Image_Number.Text;

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

        //Sync camera to laptop time
        public async Task SetTime(string myCameraIP, string myUsername, string myPassword)
        {

            // Build Basic Auth header
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{myUsername}:{myPassword}"));

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{myCameraIP}/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                // Format current PC time as YYYYMMDDThhmmss
                string systemTime = DateTime.Now.ToString("yyyyMMdd'T'HHmmss");

                // Build XML payload
                string xmlPayload = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                                        <Time Version=""1.0"">
                                            <DateTimeFormat>DDMMYYYYWhhmmss</DateTimeFormat>
                                            <TimeFormat>24hour</TimeFormat>
                                            <SystemTime>{systemTime}</SystemTime>
                                            <SyncNTPFlag>NoSync</SyncNTPFlag>
                                        </Time>";

                var content = new StringContent(xmlPayload, Encoding.UTF8, "application/xml");

                // POST or PUT depending on camera API (try PUT first)
                HttpResponseMessage response = await client.PutAsync("System/Time", content);

                if (response.IsSuccessStatusCode)
                {
                    Log("Time sync successfull");
                    string resp = await response.Content.ReadAsStringAsync();
                    Log(resp);
                }
                else
                {
                    Log($"Failed to sync time. Status: {response.StatusCode}");
                    string resp = await response.Content.ReadAsStringAsync();
                    Log(resp);
                }
            }
        }

        private async void btn_sync_time_Click(object sender, EventArgs e)
        {
            await SetTime(txtIpAddress.Text, txtUsername.Text, txtPassword.Text);
            Log("Time Sync carried out");
        }

        private void btn_save_settings_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void UpdateHeartbeat(string myHeartBeatPath)
        {
            //heartbeat stored on drive C:
            myHeartBeatPath = "C" + myHeartBeatPath.Substring(1) + "\\heartbeat.txt";

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(myHeartBeatPath));
                File.WriteAllText(myHeartBeatPath, DateTime.Now.ToString("O"));
            }
            catch
            {
                // Optional: log or ignore
            }
        }

        private void InitialiseVlc()
        {
            _libVLC = new LibVLC("--no-disable-screensaver"); //parameter will allow screen to sleep
            _mediaPlayer = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mediaPlayer;

            _mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            _mediaPlayer.Stopped += MediaPlayer_Stopped;
            _mediaPlayer.EndReached += MediaPlayer_EndReached;
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            Log("VLC reported an error — likely network loss.");
            RestartStreamWithBackoff();
        }

        private void MediaPlayer_Stopped(object sender, EventArgs e)
        {
            Log("VLC stopped unexpectedly.");
            RestartStreamWithBackoff();
        }

        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            Log("Stream ended (EndReached).");
            RestartStreamWithBackoff();
        }

        private void RestartStreamWithBackoff()
        {
            restartAttempts++;

            if (restartAttempts > MaxRestartAttempts)
            {
                Log("Max restart attempts reached. Giving up.");
                return;
            }

            Log($"Restart attempt {restartAttempts}/{MaxRestartAttempts}...");

            Task.Delay(2000 * restartAttempts).ContinueWith(_ =>
            {
                TryRestartStream();
            });
        }
    }
}
