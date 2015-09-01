using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;

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
                MessageBox.Show(input_sub_code);
            
                DialogResult result = folderBrowserDialog1.ShowDialog();
                string foldername = this.folderBrowserDialog1.SelectedPath;
                string output = string.Empty;
                string error = string.Empty;

                if (result == DialogResult.OK)
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/C wget --continue --timestamping --directory-prefix=" + foldername + " --recursive --level=0 --accept="+input_sub_code +" http://www.gtu.ac.in/Qpaper.html");
                    processStartInfo.RedirectStandardOutput = true;
                    processStartInfo.RedirectStandardError = true;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    processStartInfo.UseShellExecute = false;

                    Process process = Process.Start(processStartInfo);


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
                }

                MessageBox.Show(Application.StartupPath);
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


                ProcessStartInfo setpath = new ProcessStartInfo("cmd", "/C " + foldername.Substring(0, 2).ToString());
                setpath.RedirectStandardOutput = true;
                setpath.RedirectStandardError = true;
                setpath.WindowStyle = ProcessWindowStyle.Normal;
                setpath.UseShellExecute = false;

                process1 = Process.Start(setpath);
                process1.WaitForExit();

                //E:\Github\Gquipe\Gquipe\bin\Debug
                */

            }
        }

        static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(',');
            }
            return builder.ToString();
        }

    }
}
