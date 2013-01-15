using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using Winky.Properties;


namespace Winky
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
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
    
    public partial class Window1 : Window
    {
        private MainWindow mainWindow;

        public Window1 ( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
            InitializeComponent ( );
        }
       
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void options_Loaded(object sender, RoutedEventArgs e)
        {
            //Sets the content to last saved entry
            txtWeatherLocation.Text = Settings.Default.txtWOEID;
            comboNic.SelectedIndex = Settings.Default.nic;
            comboDisk.SelectedIndex = Settings.Default.driveSelection;
            lightButton.IsChecked = Settings.Default.lightCheck;
            darkButton.IsChecked = Settings.Default.darkCheck;
           // setHeight();
            
            if (darkButton.IsChecked == true)
            {
                winkyGrid.Background = mainWindow.grid.Background;
            }
            else
            {
                winkyGrid.Background = Brushes.White;
                lightButton.IsChecked = true;
            }

            loadDevices();          
        }
        public double lightHeight;
        public double darkHeight;

        public void height()
        {
            if (mainWindow.locationWindow.WindowStyle == System.Windows.WindowStyle.None)
            {
                lightHeight = 520;
                darkHeight = 505;
            }
            else
            {
                lightHeight = 545;
                darkHeight = 527;
            }
        }


        public void setHeight()
        {
            if (Settings.Default.lightCheck == true)
            {
                height();
                mainWindow.Height = lightHeight;
            }
            else if (Settings.Default.darkCheck == true)
            {
                height();
                mainWindow.Height = darkHeight;
            }
 
        }
    

        //Saves changes and then close the form
        public void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Settings.Default.nic = comboNic.SelectedIndex;
            Settings.Default.driveSelection = comboDisk.SelectedIndex;
            Settings.Default.txtWOEID = txtWeatherLocation.Text;
            Settings.Default.darkCheck = Convert.ToBoolean(darkButton.IsChecked);
            Settings.Default.lightCheck = Convert.ToBoolean(lightButton.IsChecked);
            settingsSave();
            Settings.Default.Save();
            this.Close();
        }

        


        public void settingsSave()
        {       
            if (txtWeatherLocation.Text != "")
            {
                Settings.Default.textboxLocation = "http://weather.yahooapis.com/forecastrss?z=" + txtWeatherLocation.Text;
            }

            if (Settings.Default.lightCheck == true)
            {
                setHeight();
                mainWindow.Height = lightHeight;
                mainWindow.imgCloudy.Opacity = 100;
                Settings.Default.lightCheck = true;
                Settings.Default.circleDark = false;
                mainWindow.txtTime.Foreground = new SolidColorBrush(Color.FromRgb(0,0,0));
                Settings.Default.theme = Convert.ToString(mainWindow.grid.Background = new SolidColorBrush(Color.FromRgb(255, 243, 243)));
                Settings.Default.darkCheck = false;
                mainWindow.txtDarkCondition.Opacity = 0;
                mainWindow.cTimer.Interval = 1;
                Settings.Default.Save();
               // mainWindow.fifteenMinutes(null,null);
                mainWindow.number2 = 0;
               // mainWindow.bw2.RunWorkerAsync();
            }
            else if (Settings.Default.darkCheck == true)
            {
                setHeight();
                mainWindow.Height = darkHeight;
                mainWindow.txtDarkCondition.Opacity = 100;
                mainWindow.cTimer.Interval = 1;
                mainWindow.imgCloudy.Opacity = 0;
                Settings.Default.darkCheck = true;
                Settings.Default.circleDark = true;
                mainWindow.txtTime.Foreground = new SolidColorBrush(Color.FromRgb(255, 243, 243));
                Settings.Default.theme = Convert.ToString(mainWindow.grid.Background = mainWindow.Background);
                Settings.Default.lightCheck = false;
                Settings.Default.Save();
                mainWindow.number2 = 0;
              //  mainWindow.bw2.RunWorkerAsync();
                
              //  mainWindow.fifteenMinutes(null, null);
            }
        }

       


        //a method that is called from the main class to set theme on startup
        internal void setTheme()
        {
            darkButton.IsChecked = Settings.Default.darkCheck;
            lightButton.IsChecked = Settings.Default.lightCheck;
            
            settingsSave();
        }

        public void loadDevices()
        {
            //Scans for all NIC'sand adds them to the ComboBox
            NetworkInterface[] interfaces
               = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface ni in interfaces)
            {
                comboNic.Items.Add(ni.Name);
            }

            //Scans for all Drives and adds them to the ComboBox
            DriveInfo[] drives
                        = DriveInfo.GetDrives();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                comboDisk.Items.Add(drive.Name);
            }
        }

        private void Options_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
