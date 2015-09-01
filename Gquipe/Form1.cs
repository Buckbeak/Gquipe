using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;
using System.Linq;

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
            //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Focus();
            }
            else if(textBox1.Text!="")
            {
                string input_sub_code;
                string[] sub_code = textBox1.Text.Split(',');
                for (int i = 0; i < sub_code.Length; i++)
                {
                    sub_code[i] = sub_code[i] + ".zip";
                }
                input_sub_code=ConvertStringArrayToString(sub_code);
                input_sub_code=input_sub_code.Substring(0, input_sub_code.Length - 1);

                string foldername=null;
                string output = string.Empty;
                string error = string.Empty;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    foldername = this.folderBrowserDialog1.SelectedPath;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/C wget --continue --timestamping --directory-prefix=" + foldername + " --recursive --level=0 --accept="+input_sub_code+" http://www.gtu.ac.in/Qpaper.html");
                    //processStartInfo.RedirectStandardOutput = true;
                    //processStartInfo.RedirectStandardError = true;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    //processStartInfo.UseShellExecute = false;

                    Process process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    
 

                    /*
                    using (StreamReader streamReader = process.StandardOutput)
                    {
                        output = streamReader.ReadToEnd();
                    }

                    using (StreamReader streamReader = process.StandardError)
                    {
                        error = streamReader.ReadToEnd();
                    }

                    //Console.WriteLine("The following output was detected:");
                    //Console.WriteLine(output);

                    if (!string.IsNullOrEmpty(error))
                    {
                        //Console.WriteLine("The following error was detected:");
                        Console.WriteLine(error);
                    }
                    process.WaitForExit(6000000);
                    //Console.Read();
                    */
                }

                //MessageBox.Show(Application.StartupPath);
                
                /*
                ProcessStartInfo opencurrentdir = new ProcessStartInfo("cmd", "/C " + foldername.Substring(0, 2).ToString());
                opencurrentdir.RedirectStandardOutput = true;
                opencurrentdir.RedirectStandardError = true;
                //processStartInfo1.WindowStyle = ProcessWindowStyle.Maximized;
                opencurrentdir.WindowStyle = ProcessWindowStyle.Normal;
                opencurrentdir.UseShellExecute = false;

                Process process1 = new Process();
                process1.StartInfo = opencurrentdir;
                //process1.WaitForExit(10000);
                process1.Start();
                 * 
                ProcessStartInfo setpath = new ProcessStartInfo("cmd", "/C " + foldername.Substring(0, 2).ToString());
                setpath.RedirectStandardOutput = true;
                setpath.RedirectStandardError = true;
                setpath.WindowStyle = ProcessWindowStyle.Normal;
                setpath.UseShellExecute = false;
                process1 = Process.Start(setpath);
                process1.WaitForExit();

                */

                RemoveEmptyFolders(foldername + @"\www.gtu.ac.in\GTU_Papers");
                MessageBox.Show("Complete");
            }
        }

        private void RemoveEmptyFolders(string path)
        {
            foreach (string subFolder in Directory.GetDirectories(path))
                RemoveEmptySubFolders(subFolder);
        }

        private bool RemoveEmptySubFolders(string path)
        {
            bool isEmpty = Directory.GetDirectories(path).Aggregate(true, (current, subFolder) => current & RemoveEmptySubFolders(subFolder))
                && Directory.GetFiles(path).Length == 0;
            if (isEmpty)
                Directory.Delete(path);
            return isEmpty;
        }

        static string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value+',');
            }
            return builder.ToString();
        }
    }
}
