using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 设置背景
{
    public partial class Form1 : Form
    {
        List<string> BitmapPaths;
        Thread thread;
        string name = @"C:\Users\LZ\Desktop\a.jpg";
        string BmpFloder = @"C:\Users\LZ\Desktop\";

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
           int uAction,
           int uParam,
           string lpvParam,
           int fuWinIni
        );



        public Form1()
        {
            InitializeComponent();
            Image image = Image.FromFile(name);
            image.Save(@"C:\Users\LZ\Desktop\AAA.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            SystemParametersInfo(20, 0, @"C:\Users\LZ\Desktop\AAA.bmp", 0x2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var count = 0;
                BitmapPaths = new List<string>();
                DirectoryInfo folder = new DirectoryInfo(dialog.SelectedPath);
                foreach (FileInfo file in folder.GetFiles())
                {
                    if (file.Extension.Equals(".jpg"))
                    {
                        Image image = Image.FromFile(file.FullName);
                        var bitPath = BmpFloder + count.ToString();
                        image.Save(bitPath, System.Drawing.Imaging.ImageFormat.Bmp);
                        BitmapPaths.Add(bitPath);
                        count++;
                    }
                }
                thread = new Thread(new ThreadStart(changeBackground));
                thread.Start();
            }
        }

        private void changeBackground()
        {
            while (true)
            {
                foreach (var path in BitmapPaths)
                {
                    SystemParametersInfo(20, 0, path, 0x2);
                    //轮换时间间隔
                    Thread.Sleep(1000);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
