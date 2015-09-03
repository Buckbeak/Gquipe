using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.IO.Compression;
using System.Collections.Generic;

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch time_e = new Stopwatch();
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
                    time_e.Start();

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

                    Console.WriteLine(output);

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine(error);
                    }
                    */
                }
                string download_loc=foldername + @"\www.gtu.ac.in\GTU_Papers";
                RemoveEmptyFolders(download_loc);
                DirSearch(download_loc,"*.zip");

                foreach (var item in listBox1.Items)
	            {
		            ZipFile.ExtractToDirectory(item.ToString(),Path.GetDirectoryName(item.ToString()));
                    File.Delete(item.ToString());     
                }

                List<string> list = new List<string>();
                listBox1.Items.Clear();
                DirSearch(download_loc, "*.pdf");

                foreach (var item in listBox1.Items)
                {
                    list.Add(Path.GetFileNameWithoutExtension(item.ToString()).ToString());
                }

                List<string> list_file = new List<string>();
                list_file = list.Distinct().ToList();
                foreach (var item in list_file)
                {
                    Directory.CreateDirectory(Directory.GetParent(Directory.GetParent(listBox1.Items[0].ToString()).ToString()) + "\\" + Path.GetFileNameWithoutExtension(item.ToString()));
                }

                foreach (var item in listBox1.Items)
                {
                    File.Move(item.ToString(), Directory.GetParent(Directory.GetParent(listBox1.Items[0].ToString()).ToString()).ToString() + "\\" + Path.GetFileNameWithoutExtension(item.ToString()) + "\\" + Directory.GetParent(item.ToString()).Name + "_" + Path.GetFileName(item.ToString()));
                }
                RemoveEmptyFolders(download_loc);
                
                //ZipFile.CreateFromDirectory();
                time_e.Stop();
                MessageBox.Show(time_e.Elapsed.Hours.ToString() + ":" + time_e.Elapsed.Minutes.ToString() + ":" + time_e.Elapsed.Seconds.ToString() + "." + time_e.Elapsed.Milliseconds.ToString());
            }
        }

        void DirSearch(string Dir,string ext)
        {
            try
            {
                foreach (string sub_dir in Directory.GetDirectories(Dir))
                {
                    foreach (string file in Directory.GetFiles(sub_dir, ext))
                    {
                        listBox1.Items.Add(file);
                    }
                    DirSearch(sub_dir,ext);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
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
