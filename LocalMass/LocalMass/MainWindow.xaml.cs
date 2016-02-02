using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.ComponentModel;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        string filetype = ".jpg";
        public MainWindow()
        {
            InitializeComponent();
            cbExtension.Items.Add(".jpg");
            cbExtension.Items.Add(".jpeg");
            cbExtension.Items.Add(".png");
            cbExtension.Items.Add(".pdf");
            cbExtension.Items.Add(".doc");
            cbExtension.Items.Add(".mp3");            
        }

        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filetype = cbExtension.SelectedItem.ToString();
                WebClient client = new WebClient();
                string html = client.DownloadString(tbURL.Text);
                string[] elements = html.Split('"');
                int x = 0;
                List<string> loFiles = new List<string>();
                foreach (string s in elements)
                {
                    if (s.Contains(filetype))
                    {
                        loFiles.Add(s);
                    }
                }
                lblLinks.Content = "There were " + loFiles.Count() + " files found on the URL.";
            }
            catch
            {
                lblLinks.Content = "Invalid URL";
            }
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            lbErrors.Items.Clear();
            filetype = cbExtension.SelectedItem.ToString();
            try
            {
                if (tbSaveLocation.Text != "")
                {
                    pbProgress.Minimum = 0;
                    pbProgress.Maximum = 10000;
                    pbProgress.Value = 0;
                    lblLinks.Content = "Running...";
                    System.Windows.Forms.Application.DoEvents();
                    WebClient client = new WebClient();
                    string html = client.DownloadString(tbURL.Text);
                    string[] elements = html.Split('"');
                    int x = 0;
                    List<string> loFiles = new List<string>();
                    foreach (string s in elements)
                    {
                        if (s.Contains(filetype))
                        {
                            loFiles.Add(s);
                        }
                    }
                    x = 0;
                    foreach (string filepath in loFiles)
                    {
                        string[] fname = filepath.Split('/');
                        string[] name = tbURL.Text.Split('/');
                        string finalPath = filepath;
                        finalPath = finalPath.Replace("\\", "");
                        int removal = name[name.Length - 1].Length;
                        if (!filepath.Contains("https:") && !filepath.Contains("http:"))
                        {
                            string urlfin = tbURL.Text.Substring(0, tbURL.Text.Length - removal);
                            finalPath = urlfin.Trim() + filepath;
                        }
                        try
                        {
                            client.DownloadFile(new Uri(finalPath), tbSaveLocation.Text + '\\' + fname[fname.Length - 1]);
                            lbErrors.Items.Add("Downloaded file " + fname[fname.Length - 1]);
                        }
                        catch (Exception ex)
                        {
                            lbErrors.Items.Add(ex.Message + finalPath);
                            x++;
                        }
                        pbProgress.Value += 10000 / loFiles.Count;
                        System.Windows.Forms.Application.DoEvents();
                    }
                    lblLinks.Content = x + " Failed to download";
                }
                else
                {
                    lblLinks.Content = "Please set a destination";
                }
            }
            catch
            {
                lblLinks.Content = "Please enter a valid URL";
            }
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbSaveLocation.Text = fbd.SelectedPath;
            }
        }

        private void btnScanAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                string html = client.DownloadString(tbURL.Text);
                string[] elements = html.Split('"');
                int x = 0;
                List<string> loFiles = new List<string>();
                foreach (string type in cbExtension.Items)
                {
                    filetype = type;
                    loFiles.Clear();
                    foreach (string s in elements)
                    {
                        if (s.Contains(filetype))
                        {
                            loFiles.Add(s);
                        }
                    }
                    lbErrors.Items.Add("There were " + loFiles.Count() + " files found on the URL of file type " + type);
                }                
            }
            catch
            {
                lblLinks.Content = "Invalid URL";
            }
        }
    }
}
