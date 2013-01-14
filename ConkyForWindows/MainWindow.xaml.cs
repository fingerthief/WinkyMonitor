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
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Timers;
using WUApiLib;
using Winky.Properties;


namespace Winky
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// <remarks>
    /// WinkyMonitor is a system monitor. Written by Tanner L. Middleton and Greg Morgan
    /// Copyright (C) 2013
    /// 
    /// This program is free software; you can redistribute it and/or
    /// modify it under the terms of the GNU General Public License
    /// as published by the Free Software Foundation; either version 2
    /// of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful,
    /// but WITHOUT ANY WARRANTY; without even the implied warranty of
    /// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    /// GNU General Public License for more details.
    /// 
    /// The author may be contacted at:
    /// middletontanner@gmail.com or gamm90@gmail.com
    /// </remarks>
    
    public partial class MainWindow : Window
    {

        public MainWindow()
        {       
            InitializeComponent();

            this.Left = Settings.Default.locationLeft;
            this.Top = Settings.Default.locationTop;
                         
            cpuUsage.Maximum = 100;
            
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork +=
                new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged +=
                new ProgressChangedEventHandler(bw_ProgressChanged);

            bw2.WorkerSupportsCancellation = true;
            bw2.WorkerReportsProgress = true;
            bw2.DoWork +=
                new DoWorkEventHandler(bw_DoWork2);
            bw2.ProgressChanged +=
                new ProgressChangedEventHandler(bw2_ProgressChanged);      
        }

        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker bw2 = new BackgroundWorker();

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
                bw2.CancelAsync();
                Environment.Exit(0);   
            }
        }
        
        //Declare all variables
        public double ramTotal, ramUsed, ramFree, ramPercent,
            driveSpace;
        public string currentUsageReceived, bytesSent, totalSent, totalReceived, 
            bytesRecieved,updates, ipLocal, ipExternal, time, cpuCount, proc;
        public float asdfa;
        private int number = 0, nic = 0, driveSelection = 0;
        public bool netavailable, netavailable2, test;

        public NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        public PerformanceCounter cpuCounter = new PerformanceCounter();  
         
        //This thread contains all the code that needs to be running constantly 
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {        
            BackgroundWorker worker = sender as BackgroundWorker;
            
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            float[] avg = new float[10];

            while (true)
            {
                //Grabs Needed Info For Disk Space etc...
                DriveInfo[] drives
                    = DriveInfo.GetDrives();

                DriveInfo drive = drives[driveSelection];
                driveSpace = drive.AvailableFreeSpace / 1073741824.004733;

                //Grabs all the needed info for network usage

                netavailable2 = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                if (netavailable2 == true)
                {
                    NetworkInterface ni = interfaces[nic];

                    totalSent = (ni.GetIPv4Statistics().BytesSent / 1048576.0).ToString("f2") + " MB";
                    totalReceived = (ni.GetIPv4Statistics().BytesReceived / 1048576.0).ToString("f2") + " MB";
                }
                else if (netavailable2 == false)
                {
                    totalReceived = "Disconnected";
                    totalSent = "Disconnected";
                    updates = "Disconnected";
                }

                ramFree = new Microsoft.VisualBasic.Devices.ComputerInfo().AvailablePhysicalMemory / 1073741824.004733;
                ramUsed = ramTotal - ramFree;
                ramPercent = ramUsed / ramTotal * 100;

                time = DateTime.Now.ToShortTimeString();

                for (int ii = 0; ii < 9; ii++)
                {
                    avg[ii] = cpuCounter.NextValue();
                    asdfa = avg.Average();
                    string aasdf = asdfa.ToString();

                    Thread.Sleep(100);
                }
                worker.ReportProgress((number++));
            }
        }

        private string oldLocation = Settings.Default.textboxLocation, newLocation;

        //Update GUI here with info from the background thread
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if user locks form set oldlocation to where the new one is
            newLocation = Settings.Default.textboxLocation;
            if (oldLocation != newLocation)
            {
                cTimer.Interval = 1;
                oldLocation = newLocation;
            }

            txtAvgBox.Text = asdfa.ToString("f2") + "%";
            cpuUsage.Value = asdfa;

            // Prints to textbox network usage
            txtTotalReceived.Text = totalReceived;
            txtTotalSent.Text = totalSent;

            if (netavailable == true)
            {
                txtNetOut.Text = bytesSent;
                txtNetIn.Text = bytesRecieved;
            }
            txtPing.Text = pingtime;
            txtLocal.Text = ipLocal;
            txtExternal.Text = ipExternal;
            nic = Settings.Default.nic;
            driveSelection = Settings.Default.driveSelection;

            //Converts Variables to a String so you can then format properly
            Convert.ToString(ramTotal);
            Convert.ToString(ramUsed);
            Convert.ToString(ramFree);
            Convert.ToString(ramPercent);
            Convert.ToString(driveSpace);

            //Prints free and used RAM to textboxes
            txtRam.Text = ramFree.ToString("f2") + " GB";
            txtRamPercent.Text = ramPercent.ToString("f2") + " %";

            //Prints Disk Drive Free Space
            txtDriveSpace.Text = driveSpace.ToString("f2") + " GB";

            //Prints Available Updates to screen.
            txtUpdates.Text = Convert.ToString(updateCount);

            //prints the time
            txtTime.Text = time;

            //prints the weather conditions and weather icon 
            txtWeather.Text = RSS;
            txtDarkCondition.Text = weather.current;
            imgCloudy.Source = new BitmapImage(WeatherImage);

        }

        private int number2 = 0;
        private bool netavailable3;
        private string condition;

        //This thread is for code that needs to be ran just at startup
        private void bw_DoWork2(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker2 = sender as BackgroundWorker;

            netavailable3 = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (netavailable3 == true && number2 < 1)
            {
                getIP();
                pingEvent(null, null);
                OnTimedEvent(null, null);
                ramTotal = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 1073741824.004733;
                weather = new WeatherRSS.Weather();
                RSS = weather.CurrentConditions();
                
                WeatherImage = new Uri(weather.getImage());
                getUpdates();   
            }

            worker2.ReportProgress((number2++));
            bw2.CancelAsync();
            bw2.Dispose();
            
        }

        private void bw2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

        public int updateCount = 0;
        public string external;
        public string RSS;
        private WeatherRSS.Weather weather;

        Uri WeatherImage = new Uri("http://nothing.com");
       
        public void pingEvent(object sender, ElapsedEventArgs e)
        {
            netavailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            
            if (netavailable == false)
            {
                pingtime = "Disconnected";
            }
            else if (netavailable == true)
            {
                //tests ping time
                pingtime = Convert.ToString(GetPingMS("www.easytel.com"))+ " ms";
            }
        }

        int GetPingMS(string hostNameOrAddress)
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
            return Convert.ToInt32(ping.Send(hostNameOrAddress).RoundtripTime);
        }

        public System.Timers.Timer cTimer = new System.Timers.Timer();
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            

            if (Settings.Default.textboxLocation == "" || Settings.Default.textboxLocation == "http://weather.yahooapis.com/forecastrss?w=")
            {
                Settings.Default.textboxLocation = "http://weather.yahooapis.com/forecastrss?w=2459115";
                newWindow = new Winky.Window1( this );
                newWindow.Show();
                
            }
            newWindow = new Winky.Window1(this);
            newWindow.height();

            newWindow.setTheme();

            txtDarkCondition.Text = "";

            cancel.Opacity = 0;
            
            bw.RunWorkerAsync();
            bw2.RunWorkerAsync();

            RSS = "Collecting Candy...";     

            setTextReadOnly();
            
            // Make timer for network usage.
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 1 seconds.
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

            // Make timer for ping and IP Address time.
            System.Timers.Timer bTimer = new System.Timers.Timer();
            bTimer.Elapsed += new ElapsedEventHandler(pingEvent);

            // Set the Interval to 5 seconds.
            bTimer.Interval = 5000;
            bTimer.Enabled = true;

            // Make timer for System Update check, and weather.
            // System.Timers.Timer cTimer = new System.Timers.Timer();
            cTimer.Elapsed += new ElapsedEventHandler(fifteenMinutes);

            // Set the Interval to 15 minutes.
            cTimer.Interval = 900000;//900000;
            cTimer.Enabled = true;
        }


        // gets local and external IP Address
        public void getIP()
        {
            //Grabs External IP Address, and uses the HTTPGet Class
            HTTPGet req = new HTTPGet();
            req.Request("http://checkip.dyndns.org");
            string[] a = req.ResponseBody.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            ipExternal = a4;

            //Grabs Local IP Address
            ipLocal = "";
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            int iter = 0;
            foreach (IPAddress i in localIPs)
            {
                string IP = i.ToString();
                bool isIP = IP.Contains(".");
                if (isIP == true)
                    ipLocal += IP + "\r\n";
                iter++;
            }
        }

        //checks for available updates and store the number of them in updateCount
        public void getUpdates()
        {
            UpdateSessionClass uSession = new UpdateSessionClass();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type='Software'");
            updateCount = uResult.Updates.Count;
        }
        
        public void fifteenMinutes(object sender, ElapsedEventArgs e)
        {
            cTimer.Interval = 90000;

            if (netavailable2 == true)
            {        

                weather = new WeatherRSS.Weather();

                getIP();

                RSS = weather.CurrentConditions();
                WeatherImage = new Uri(weather.getImage());

                if (Settings.Default.darkCheck == true)
                {
                    condition = weather.current;

                }  

                getUpdates();
            }            
        }

        private double upLoadOld = 0.0, upLoadNew = 0.0, upLoadTotal = 0.0, downLoadOld = 0.0,
            downLoadNew = 0.0, downLoadTotal = 0.0;
        private string pingtime;
        
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //Grabs all the needed info for network usage
            NetworkInterface ni = interfaces[nic];
            
            upLoadNew = (ni.GetIPv4Statistics().BytesSent / 131072.0);
            upLoadTotal = upLoadNew - upLoadOld;

            downLoadNew = (ni.GetIPv4Statistics().BytesReceived / 131072.0);
            downLoadTotal = downLoadNew - downLoadOld;

            //changes text between Mbps and Kbps
            if (netavailable == true)
            {
                if (upLoadTotal > 1 && downLoadTotal > 1)
                {
                    bytesSent = (upLoadTotal).ToString("f2") + " Mbps";
                    upLoadOld = upLoadNew;

                    bytesRecieved = (downLoadTotal).ToString("f2") + " Mbps";
                    downLoadOld = downLoadNew;
                }
                else
                {
                    bytesSent = (upLoadTotal * 1024).ToString("f2") + " Kbps";
                    upLoadOld = upLoadNew;

                    bytesRecieved = (downLoadTotal * 1024).ToString("f2") + " Kbps";
                    downLoadOld = downLoadNew;
                }
            }
        }
        // On double click resize the window. This keeps the content from shifting around.
        private void locationWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (locationWindow.WindowStyle == System.Windows.WindowStyle.None)
            {
                locationWindow.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                cancel.Opacity = 0;
                this.Left -= 3;
                this.Top -= 26;
                this.Height += 25;
            }
            else
            {
                locationWindow.WindowStyle = System.Windows.WindowStyle.None;
                cancel.Opacity = 100;
                this.Left += 3;
                this.Top += 26;
                this.Height -= 25;

                Settings.Default.locationLeft = this.Left;
                Settings.Default.locationTop = this.Top;
                Settings.Default.Save();
            }
        }

        // Create new window construct.
        public string location = "";
        public Winky.Window1 newWindow;
        
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            newWindow = new Winky.Window1 ( this );
            newWindow.ShowDialog ( );        
        }

        public void setTextReadOnly()
        {        
            txtTotalReceived.IsReadOnly = true;
            txtTotalSent.IsReadOnly = true;
            txtNetOut.IsReadOnly = true;
            txtNetIn.IsReadOnly = true;
            txtPing.IsReadOnly = true;
            txtRam.IsReadOnly = true;
            txtRamPercent.IsReadOnly = true;
            txtDriveSpace.IsReadOnly = true;
            txtUpdates.IsReadOnly = true;
            txtLocal.IsReadOnly = true;
            txtExternal.IsReadOnly = true;
            txtWeather.IsReadOnly = true; 
        }

        private void locationWindow_Closing(object sender, CancelEventArgs e)
        {
            cancel_Click(null, null);
        }
    }
 }


 