using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gquipe
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if(result==DialogResult.OK)
            {
                string foldername = this.folderBrowserDialog1.SelectedPath;
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C wget --continue --timestamping --directory-prefix="+foldername+" --recursive --level=0 --accept=170701.zip  http://www.gtu.ac.in/Qpaper.html > error.log";
                process.StartInfo = startInfo;
                
                process.Start();
                process.WaitForExit();
                Console.Write(process.StandardOutput.ReadToEnd());
            }
        }
    }
}
