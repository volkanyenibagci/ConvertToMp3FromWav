using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Text;
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
            //ffmeg in �al��mas� i�in https://github.com/BtbN/FFmpeg-Builds/releases exe dosyas�n�n System PATH de tan�ml� olmas� gerekmektedir.�ndirip System PATH e ekleyebilirsiniz.

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "L�tfen WAV dosyalar�n�n bulundu�u klas�r� se�in";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFolder = folderBrowserDialog.SelectedPath.ReplaceTurkishChars();
                    string lastFolderName = Path.GetFileName(sourceFolder);
                    string sourceFolderOld = folderBrowserDialog.SelectedPath;

                    string targetFolder = $@"C:\Users\vyenibagci\Documents\ConvertedMp3\{lastFolderName}";
                    string targetFolderConverted = targetFolder + @"\Converted";

                    //Copy all files and folders from sourceFolder to targetFolder

                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                        Directory.CreateDirectory(targetFolderConverted);
                    }

                    // Get the files in the source folder
                    string[] files = Directory.GetFiles(sourceFolderOld);

                    foreach (string file in files)
                    {
                        // Get the filename
                        string fileName = Path.GetFileName(file).ReplaceTurkishChars();
                        // Create the destination file path
                        string destFile = Path.Combine(targetFolder, fileName);
                        // Copy the file
                        File.Copy(file, destFile, true); // true allows overwriting of existing files
                    }

                    string batchFilePath = Path.Combine(targetFolder, "convert.bat");

                    using (StreamWriter writer = new StreamWriter(batchFilePath))
                    {
                        writer.WriteLine("@echo off");
                        writer.WriteLine("cd /d " + targetFolder);
                        foreach (var wavFile in Directory.GetFiles(targetFolder, "*.wav"))
                        {
                            string fileName = Path.GetFileNameWithoutExtension(wavFile).ReplaceTurkishChars();
                            string mp3File = Path.Combine(targetFolderConverted, fileName + ".mp3");
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
                    MessageBox.Show("Klas�r se�ilmedi.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }

    public static class StringExtensions
    {
        private static readonly Dictionary<char, char> TurkishCharMap = new Dictionary<char, char>
    {
        { '�', 'c' }, { '�', 'C' },
        { '�', 'g' }, { '�', 'G' },
        { '�', 'i' }, { '�', 'I' },
        { '�', 'o' }, { '�', 'O' },
        { '�', 's' }, { '�', 'S' },
        { '�', 'u' }, { '�', 'U' }
    };

        public static string ReplaceTurkishChars(this string input)
        {
            var stringBuilder = new StringBuilder(input);

            foreach (var kvp in TurkishCharMap)
            {
                stringBuilder.Replace(kvp.Key, kvp.Value);
            }

            return stringBuilder.ToString();
        }
    }
}