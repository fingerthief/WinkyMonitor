using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using WUApiLib;
using System.Net.NetworkInformation;
using System.Configuration;
using TestBed.Properties;

namespace TestBed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                currentWeather = "Loading...";
                Task.Factory.StartNew(new Action(() =>
                {
                    //My sketchy attempt at making an animation
                    for (int i = 0; i < 100; i++)
                    {
                        height = (200.00 / 100.00) * i;
                        winOpacity += 0.01;

                        Thread.Sleep(3);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Call methods to start this beast    
            UpdateUI();     
            NetworkUsage();
            PercentOfDayFinished();
           // CheckForUpdates();
            MoneyEarned();
            SystemInfo();
            GetWeather();
        }

        private void SystemInfo()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            double ramFree, ramTotal, ramUsed, driveTotal;

            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    while (true)
                    {
                        //Grabs Needed Info For Disk Space etc...
                        DriveInfo[] drives
                            = DriveInfo.GetDrives();

                        DriveInfo drive = drives[Config.Default.Disk];

                        //Make sure the drive is ready before trying to get info from it
                        if (drive.IsReady)
                        {
                            driveTotal = drive.TotalSize / 1073741824.004733;
                            driveSpace = drive.AvailableFreeSpace / 1073741824.004733;
                            driveUsed = driveTotal - driveSpace;
                        }

                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                        {
                            NetworkInterface ni = interfaces[Config.Default.NIC]; 
                            totalSent = (ni.GetIPv4Statistics().BytesSent / 1048576.0).ToString("f2") + " MB"; 
                            totalReceived = (ni.GetIPv4Statistics().BytesReceived / 1048576.0).ToString("f2") + " MB";
                        }

                        //Retrieve RAM total and calculate how much is being used at the moment
                        ramFree = new Microsoft.VisualBasic.Devices.ComputerInfo().AvailablePhysicalMemory / 1073741824.004733;
                        ramTotal = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 1073741824.004733;
                        ramUsed = ramTotal - ramFree;
                        ramPercent = ramUsed / ramTotal * 100;

                        //Lets grab some CPU usage -- You have to call .NextValue() twice with a delay to actually get a reading
                        cpuCounter.NextValue();
                        Thread.Sleep(1000);
                        cpuUsage = cpuCounter.NextValue();
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NetworkUsage()
        {
            try
            {
                double upLoadNew, upLoadTotal, upLoadOld = 0, downLoadNew,
                downLoadTotal, downLoadOld = 0;

                Task.Factory.StartNew(new Action(() =>
                {
                    //Grabs all the needed info for network usage
                    NetworkInterface ni = interfaces[Config.Default.NIC];

                    while (true)
                    {
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                        {
                            upLoadNew = (ni.GetIPv4Statistics().BytesSent / 131072.0);
                            upLoadTotal = upLoadNew - upLoadOld;

                            if (upLoadTotal > 1)
                            {
                                bytesSent = (upLoadTotal).ToString("f2") + " Mbps";
                                upLoadOld = upLoadNew;
                            }
                            else
                            {
                                bytesSent = (upLoadTotal * 1024).ToString("f2") + " Kbps";
                                upLoadOld = upLoadNew;
                            }

                            downLoadNew = (ni.GetIPv4Statistics().BytesReceived / 131072.0);
                            downLoadTotal = downLoadNew - downLoadOld;

                            if (downLoadTotal > 1)
                            {
                                bytesRecieved = (downLoadTotal).ToString("f2") + " Mbps";
                                downLoadOld = downLoadNew;
                            }
                            else
                            {
                                bytesRecieved = (downLoadTotal * 1024).ToString("f2") + " Kbps";
                                downLoadOld = downLoadNew;
                            }
                        }

                        Thread.Sleep(1000);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckForUpdates()
        {
            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    while (true)
                    {
                        //Lets check how many updates we have
                        UpdateSession uSession = new UpdateSession();
                        IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
                        ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type='Software'");
                        updateCount = uResult.Updates.Count;

                        //Will check every 15 minutes
                        Thread.Sleep(900000);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PercentOfDayFinished()
        {
            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    
                    while (true)
                    {
                        TimeSpan TotalMS = TimeSpan.FromHours(Config.Default.TotalHours);
                        TimeSpan TotalMSMins = TimeSpan.FromMinutes(Config.Default.EndHoursMins);
                        
                        TotalMSDay = TotalMS.TotalMilliseconds + TotalMSMins.TotalMilliseconds;

                        //Toal MS in 9 hour day 32400000
                        TimeSpan CurrentTimeSpan = (DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,Config.Default.StartHour, Config.Default.StartMinute, 0, 0));

                        //Get all the ms since the day has started
                        percentDayFinished = CurrentTimeSpan.TotalMilliseconds;

                        if (actualPercentFinished <= 100)
                        {
                            //get the percentage of day finished
                            actualPercentFinished = (100 / TotalMSDay) * percentDayFinished;
                        }
                        Thread.Sleep(30);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MoneyEarned()
        {
            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    while (true)
                    {
                        //Total seconds in 8 hour day 28800
                        TimeSpan CurrentTimeSpan = (DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Config.Default.StartHour, Config.Default.StartMinute, 0, 0));

                        DateTime dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Config.Default.StartHour, Config.Default.StartMinute, 0, 0);
                        DateTime dtEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Config.Default.EndHours, Config.Default.EndHoursMins, 0, 0);

                        //Lets check if it's lunch time
                        if (DateTime.Now > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0, 0) &&
                            DateTime.Now < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 0, 0, 0))
                        {
                            isLunch = true;
                        } //Make sure it's actually part of the work day
                        else if (DateTime.Now > dtStart &&
                            DateTime.Now < dtEnd)
                        {
                            isLunch = false;
                            double secondsToday = CurrentTimeSpan.TotalSeconds;

                            if (DateTime.Now.Hour > 12)
                            {
                                //Net Pay after lunch, 3600 being seconds in an hour
                                moneyMade = (Config.Default.Money / 3600) * (secondsToday - 3600);
                            }
                            else
                            {
                                //Net Pay before lunch, 3600 being seconds in an hour
                                moneyMade = (Config.Default.Money / 3600) * secondsToday;
                            }
                        }
                        Thread.Sleep(1000);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetWeather()
        {
            //Instantiate the weather class
            Weather Weather = new Weather();

            Task.Factory.StartNew(new Action(() =>
               {
                   while (true)
                   {
                       //return a formatted string of the weather
                       currentWeather = Weather.GetWeatherXMLAndReturnString(Config.Default.ZIP);

                       //Check every fifteen minutes
                       Thread.Sleep(900000);
                   }
               }));

        }

        //Including the ability to install updates causes the application to be ran as an admin,
        //I will leave it out for now

        //private void progressBar1_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        //Be sure the user actually wants to install update
        //        if (MessageBox.Show("Are You Sure You Want To Install Updates?", "Update Windows", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //        {
        //            Task.Factory.StartNew(new Action(() =>
        //            {
        //                UpdateSession uSession = new UpdateSession();
        //                IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
        //                ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type='Software'");

        //                updateCount = uResult.Updates.Count;

        //                if (uResult.Updates.Count == 0)
        //                {
        //                    MessageBox.Show("No Available Updates.");
        //                }
        //                else if (uResult.Updates.Count != 0)
        //                {
        //                    UpdateDownloader downloader = uSession.CreateUpdateDownloader();
        //                    downloader.Updates = uResult.Updates;
        //                    progressIncrement = (100 / uResult.Updates.Count) / 2;

        //                    //Go through and download updates
        //                    foreach (IUpdate update in uResult.Updates)
        //                    {
        //                        downloader.Download();
        //                    }

        //                    UpdateCollection updatesToInstall = new UpdateCollection();

        //                    foreach (IUpdate update in uResult.Updates)
        //                    {
        //                        if (update.IsDownloaded)
        //                        {
        //                            updatesToInstall.Add(update);
        //                        }

        //                        IUpdateInstaller installer = uSession.CreateUpdateInstaller();
        //                        installer.Updates = updatesToInstall;

        //                        //Actually install updates
        //                        IInstallationResult installationRes = installer.Install();
        //                    }

        //                    //Reset update count now that we're finished
        //                    CheckForUpdates();
        //                }
        //            }));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private double ramPercent, driveSpace = 0, driveUsed = 0,
                percentDayFinished = 0,
                actualPercentFinished = 0, moneyMade, TotalMSDay = 0, opacity = 0, winOpacity = 0, height = 0;

        private float cpuUsage = 0.0f;
        private string bytesSent, bytesRecieved, currentWeather, totalSent, totalReceived;
        private int updateCount;
        private bool isLunch;

        private void UpdateUI()
        {
            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    while (true)
                    {
                        //Use this to update the UI
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            //CPU Data
                            progCpu.Value = cpuUsage;
                            lblCPUNum.Content = cpuUsage.ToString("f2") + "%";

                            //Ram data
                            progRam.Value = ramPercent;
                            lblRam.Content = ramPercent.ToString("f2") + "%";

                            //Drive data
                            progDriveSpace.Value = driveUsed;
                            lblDriveFree.Content = driveSpace.ToString("f2") + " GB";

                            //Real time network data
                            txtRecieving.Text = bytesRecieved;
                            txtSending.Text = bytesSent;

                            //Lets set the tooltip text to the total data used
                            txtSending.ToolTip = "Total Sent: " + totalSent;
                            txtRecieving.ToolTip = "Total Received: " + totalReceived;

                            //Set label to number of updates
                           // lblUpdateCount.Content = updateCount.ToString();
                            //progUpdates.Value += progressIncrement;

                            //Set values for percent of day finishes
                            if ( actualPercentFinished >= 100)
                            { 
                                lblDayfinished.Content = "0.00%";
                                actualPercentFinished = 100;
                                ProgDay.Value = 0;
                            }
                            else
                            {
                                lblDayfinished.Content = actualPercentFinished.ToString("f2") + "%";
                                ProgDay.Maximum = TotalMSDay;
                                ProgDay.Value = percentDayFinished;
                            }

                            //Set how much money has been made today
                            if (isLunch == false)
                            {
                                txtMoney.Text = "$" + moneyMade.ToString("F2");
                            }
                            else
                            {
                                txtMoney.Text = "Lunch Time";
                            }

                            //Set the weather
                            txtWeather.Text = currentWeather;

                            //Set the button opacity for the fade-in animation
                            btnSettings.Opacity = opacity;
                            btnExit.Opacity = opacity;

                            //Set the window and grid opacity for the startup animation
                            Window.Opacity = winOpacity;
                            Grid.Opacity = winOpacity;

                            //Set the grid height for the startup animation
                            Grid.Height = height;

                            //if (height >= 194)
                            //{
                            //    Window.Height = 194;
                            //}
                           // else
                           // {
                                Window.Height = height;
                           // }
                        }));
                        Thread.Sleep(3);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Use this so that the program won't go behind other windows. 
            //That is, it's alway on top
            if (this.Topmost == true)
            {
                this.Topmost = false;
            }
            else
            {
                this.Topmost = true;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Begin dragging the window 
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Exit and close all processes
            try
            {
                Task.Factory.StartNew(new Action(() =>
                    {
                        //My sketchy attempt at making an animation
                        for (int i = 100; i > 0; i--)
                        {
                            height = (200.00 / 100.00) * i ;
                            winOpacity -= 0.01;

                            Thread.Sleep(3);
                        }
                        Environment.Exit(0);
                    }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //Use this to open the settings page
            Settings SettingsForm = new Settings(this);
            SettingsForm.ShowDialog();

        }

        private void txtWeather_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentWeather = "Loading...";
            GetWeather();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (opacity < 0.1)
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            opacity += 0.1;
                            Thread.Sleep(20);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (opacity > 0.1)
                {
                    //This makes the buttons fade-in
                    Task.Factory.StartNew(new Action(() =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            opacity -= 0.1;
                            Thread.Sleep(20);
                        }

                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
