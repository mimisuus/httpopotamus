using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Windows.Threading;

namespace httpopotamus
{
    public partial class MainWindow : Window
    {
        int lines = 0;
        int value = 0;
        int multiplier;
        int lineLength;
        String osoite;
        public String contentToBeChecked;
        bool writeToFile;
        bool running = false;
        NotifyIcon nIcon = new NotifyIcon();
        private System.Windows.Threading.DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                if (LogYN.IsChecked == true)
                {
                    writeToFile = true;
                }
                else
                {
                    writeToFile = false;
                }
                osoite = PageURL.Text;
                lineLength = 53 + osoite.Length;
                contentToBeChecked = FetchContent.GetPageHash(osoite);
                int timeInt = Convert.ToInt32(AikaVali.Text);
                this.ValueMultiplier();
                _timer = new System.Windows.Threading.DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(multiplier * timeInt);
                _timer.Tick += _timerTick;
                _timer.Start();
                running = true;
                AcceptButton.Content = "Stop";
                LogYN.IsEnabled = false;
            } else
            {
                StopTimer();
            }
            this.nIcon.Icon = new Icon("hippo.ico");
            this.nIcon.Visible = true;
        }
        public void StopTimer()
        {
            AcceptButton.Content = "Start";
            running = false;
            _timer.Stop();
            LogYN.IsEnabled = true;
        }

        private void ValueMultiplier()
        {
            switch (value)
            {
                case 1:
                    multiplier = 1000;
                    break;
                case 2:
                    multiplier = 60000;
                    break;
                case 3:
                    multiplier = 3600000;
                    break;
                default:
                    multiplier = 1000;
                    break;
            }
        }

        private void _timerTick(object sender, EventArgs e)
        {
            if (FetchContent.GetPageHash(osoite) != contentToBeChecked)
            {
                string message = osoite + " was checked at " + DateTime.Now.ToString("h:mm:ss tt") + " and the site has updates \n";
                SiteLog.Text += message;
                if (writeToFile)
                {
                    WriteLogs(message);
                }
                this.nIcon.ShowBalloonTip(5000, osoite, "Changes have been found", ToolTipIcon.Info);
                StopTimer();
            }
            else
            {
                string message = osoite + " was checked at " + DateTime.Now.ToString("h:mm:ss tt") + " and there was no changes \n";
                SiteLog.Text += message;
                if (writeToFile)
                {
                    WriteLogs(message);
                }
                lines++;
                if(lines > 15)
                {
                    SiteLog.Text = SiteLog.Text.Substring(lineLength, SiteLog.Text.Length - lineLength);
                }
            }
            SiteLog.ScrollToEnd();
        }

        public void WriteLogs(String msg)
        {
            File.AppendAllText(@".\Site_Logs.txt", msg + Environment.NewLine);
        }

        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            SiteLog.Text = "";
        }

        private void LogYN_Checked(object sender, RoutedEventArgs e)
        {
            LogYN.IsChecked = true;
        }

        private void LogYN_Unchecked(object sender, RoutedEventArgs e)
        {
            LogYN.IsChecked = false;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(((sender as System.Windows.Controls.ListBox).SelectedItem as ListBoxItem).Content)
            {
                case ("Seconds"):
                    value = 1;
                    break;
                case ("Minutes"):
                    value = 2;
                    break;
                case ("Hours"):
                    value = 3;
                    break;
            }
        }
    }
}
