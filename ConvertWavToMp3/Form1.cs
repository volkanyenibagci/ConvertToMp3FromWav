using System.Diagnostics;
using Xabe.FFmpeg;

namespace ConvertWavToMp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ffmeg in çalýþmasý için https://github.com/BtbN/FFmpeg-Builds/releases exe dosyasýnýn System PATH de tanýmlý olmasý gerekmektedir.Ýndirip System PATH e ekleyebilirsiniz.

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Lütfen WAV dosyalarýnýn bulunduðu klasörü seçin";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFolder = folderBrowserDialog.SelectedPath;
                    string targetFolder = Path.Combine(sourceFolder, "ConvertedMp3s");

                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                    }

                    string batchFilePath = Path.Combine(sourceFolder, "convert.bat");
                    using (StreamWriter writer = new StreamWriter(batchFilePath))
                    {
                        writer.WriteLine("@echo off");
                        writer.WriteLine("cd /d " + sourceFolder);
                        foreach (var wavFile in Directory.GetFiles(sourceFolder, "*.wav"))
                        {
                            string fileName = Path.GetFileNameWithoutExtension(wavFile);
                            string mp3File = Path.Combine(targetFolder, fileName + ".mp3");
                            writer.WriteLine($"ffmpeg -i \"{wavFile}\" \"{mp3File}\"");
                        }
                        writer.WriteLine("pause");
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = batchFilePath,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                }
                else
                {
                    MessageBox.Show("Klasör seçilmedi.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}