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
                        height = (170.00 / 100.00) * i;
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
            SystemInfo();
            GetWeather();
        }

        private void SystemInfo()
        {

            double ramFree, ramTotal, ramUsed, driveTotal;

            try
            {
                Microsoft.VisualBasic.Devices.ComputerInfo RAM = new Microsoft.VisualBasic.Devices.ComputerInfo();
                PerformanceCounter cpuCounter = new PerformanceCounter();

                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";

                Task.Factory.StartNew(() =>
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
                        ramFree = RAM.AvailablePhysicalMemory / 1073741824.004733;
                        ramTotal = RAM.TotalPhysicalMemory / 1073741824.004733;
                        ramUsed = ramTotal - ramFree;
                        ramPercent = ramUsed / ramTotal * 100;

                        //Lets grab some CPU usage -- You have to call .NextValue() twice with a delay to actually get a reading
                        cpuCounter.NextValue();
                        Thread.Sleep(1000);
                        cpuUsage = cpuCounter.NextValue();
                    }
                });
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

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        //Grabs all the needed info for network usage
                        NetworkInterface ni = interfaces[Config.Default.NIC];

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
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetWeather()
        {
            Weather WeatherLoad = new Weather();
            try
            {
                currentWeather = "Loading...";

                if (currentWeather == "Loading..." || currentWeather == "Loading failed!")
                {
                    Task.Factory.StartNew(() =>
                    {
                        currentWeather = WeatherLoad.GetWeatherXMLAndReturnString(Config.Default.ZIP);
                    });
                }
                else
                {
                    Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            currentWeather = WeatherLoad.GetWeatherXMLAndReturnString(Config.Default.ZIP);
                            Thread.Sleep(900000);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                currentWeather = "Loading failed!";
            }
        }

        private double ramPercent, driveSpace = 0, driveUsed = 0,
                percentDayFinished = 0,
                actualPercentFinished = 0,TotalMSDay = 0, opacity = 0, winOpacity = 0, height = 0;

        private float cpuUsage = 0.0f;
        private string bytesSent, bytesRecieved, currentWeather, totalSent, totalReceived;
        private int updateCount;
        private bool isLunch;

        private void UpdateUI()
        {
            try
            {
                Task.Factory.StartNew(() =>
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

                            Window.Height = height;
                        }));
                        Thread.Sleep(3);
                    }
                });
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
                Task.Factory.StartNew(() =>
                    {
                        //My sketchy attempt at making an animation
                        for (int i = 100; i > 0; i--)
                        {
                            height = (170.00 / 100.00) * i ;
                            winOpacity -= 0.01;
                            
                            Thread.Sleep(3);
                        }
                        Environment.Exit(0);
                    });
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
            SettingsForm = null;

        }

        private void txtWeather_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetWeather();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (opacity < 0.1)
                {
                    Task.Factory.StartNew(() =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            opacity += 0.1;
                            Thread.Sleep(20);
                        }
                    });
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
                    Task.Factory.StartNew(() =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            opacity -= 0.1;
                            Thread.Sleep(20);
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
