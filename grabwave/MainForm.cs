using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NAudio.Wave;
using System.IO;

namespace grabwave
{
    class MainForm: Form
    {
        private string m_currentWavFile;
        private WaveIn m_waveInStream;
        private WaveFileWriter m_waveWriter;

        private ToolStripButton recordButton;
        private ToolStripButton playButton;
        private ToolStripButton stopButton;
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exportToolStripMenuItem;
        private StatusStrip statusBar;
        private Panel waveformView;
        private HScrollBar timeScroll;
        private ToolStrip toolBar;

        struct complex
        {
            double re, im;
        }

        [DllImport("dsplib.dll")]
        static extern int test(int x);

        [DllImport("dsplib.dll")]
        static extern void PronyClassic(int M, IntPtr Sig, ref IntPtr Amp, ref IntPtr Pol, ref byte Err);

        [DllImport("dsplib.dll")]
        static extern void PronyMNK(int N, int P, int Q, IntPtr Sig, IntPtr Amp, IntPtr Pol);

        [DllImport("dsplib.dll")]
        static extern void PronySort(int M, ref IntPtr Amp, ref IntPtr Pol, ref int Mr);

        [DllImport("dsplib.dll")]
        static extern void PronySPM(int P, int M, bool Spec, IntPtr Amp, IntPtr Pol, ref IntPtr spm);

        [DllImport("dsplib.dll")]
        static extern void PronySectrum(int P, int M, IntPtr Amp, IntPtr Pol, ref IntPtr spec);


        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.recordButton = new System.Windows.Forms.ToolStripButton();
            this.playButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.waveformView = new System.Windows.Forms.Panel();
            this.timeScroll = new System.Windows.Forms.HScrollBar();
            this.toolBar.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.waveformView.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordButton,
            this.playButton,
            this.stopButton});
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(702, 25);
            this.toolBar.TabIndex = 0;
            this.toolBar.Text = "toolStrip1";
            // 
            // recordButton
            // 
            this.recordButton.CheckOnClick = true;
            this.recordButton.Image = ((System.Drawing.Image)(resources.GetObject("recordButton.Image")));
            this.recordButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(64, 22);
            this.recordButton.Text = "Record";
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // playButton
            // 
            this.playButton.CheckOnClick = true;
            this.playButton.Enabled = false;
            this.playButton.Image = ((System.Drawing.Image)(resources.GetObject("playButton.Image")));
            this.playButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(85, 22);
            this.playButton.Text = "Play/Pause";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(51, 22);
            this.stopButton.Text = "Stop";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(702, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 416);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(702, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // waveformView
            // 
            this.waveformView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.waveformView.Controls.Add(this.timeScroll);
            this.waveformView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.waveformView.Location = new System.Drawing.Point(0, 49);
            this.waveformView.Name = "waveformView";
            this.waveformView.Size = new System.Drawing.Size(702, 367);
            this.waveformView.TabIndex = 3;
            // 
            // timeScroll
            // 
            this.timeScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.timeScroll.Location = new System.Drawing.Point(0, 346);
            this.timeScroll.Name = "timeScroll";
            this.timeScroll.Size = new System.Drawing.Size(698, 17);
            this.timeScroll.TabIndex = 0;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(702, 438);
            this.Controls.Add(this.waveformView);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.waveformView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (TextReader tr = new StreamReader("Signal1.txt"))
            {
                int N = int.Parse(tr.ReadLine());
                double[] sig = new double[N];
                for (int i = 0; i < N; i++)
                {
                    sig[i] = double.Parse(tr.ReadLine().Replace('.', ','));
                }
                IntPtr signalBuffer = Marshal.AllocHGlobal(N * sizeof(double));
                Marshal.Copy(sig, 0, signalBuffer, N);
                byte err = 255;
                int P = 7;
                int Q = 5;
                IntPtr Amp = Marshal.AllocHGlobal(P * sizeof(double) * 2);
                IntPtr Pol = Marshal.AllocHGlobal(P * sizeof(double) * 2);                
                PronyMNK(N, P, Q, signalBuffer, Amp, Pol);
                Marshal.FreeHGlobal(signalBuffer);
                Marshal.FreeHGlobal(Amp);
                Marshal.FreeHGlobal(Pol);

                double[] amplitude = new double[P*2];
                double[] pole = new double[P*2];                
                Marshal.Copy(Amp, amplitude, 0, P * 2);
                Marshal.Copy(Pol, pole, 0, P * 2);
            }
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());*/
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (recordButton.Checked)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Title = "Select output file:";
                saveDialog.Filter = "WAV Files (*.wav)|*.wav";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    m_currentWavFile = saveDialog.FileName;
                    m_waveInStream = new WaveIn();
                    m_waveInStream.WaveFormat = new WaveFormat(44100, 1);
                    m_waveWriter = new WaveFileWriter(m_currentWavFile, m_waveInStream.WaveFormat);

                    m_waveInStream.DataAvailable += new EventHandler<WaveInEventArgs>(m_waveInStream_DataAvailable);

                    m_waveInStream.StartRecording();
                }
                else
                {
                    recordButton.Checked = false;
                }
            }
            else
            {
                if (m_waveInStream != null)
                {
                    m_waveInStream.StopRecording();
                    m_waveInStream.Dispose();
                    m_waveWriter.Close();
                }
            }
        }

        void m_waveInStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            m_waveWriter.WriteData(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
