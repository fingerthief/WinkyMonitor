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
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;

namespace TestBed
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private MainWindow mainWindow;

        public Settings( MainWindow mainWindow )
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            double Result = 0;
            try
            {
                button1_Click(null, null);

                if (Convert.ToInt16(cmbDayEndHour.Text) < Convert.ToInt16(cmbStartHour.Text))
                {
                    MessageBox.Show("Day start must be before day end");
                    return;
                }
                else if (Convert.ToInt16(cmbDayEndHour.Text) == Convert.ToInt16(cmbStartHour.Text) && Convert.ToInt16(cmbDayEndHourMins.Text) <= Convert.ToInt16(cmbStartMinute.Text))
                {
                    MessageBox.Show("Day start must be before day end");
                    return;
                }

                //Switch over to the new zip code and save settings
                Config.Default.ZIP = txtZip.Text;
                Config.Default.Disk = comboDisk.SelectedIndex;
                Config.Default.NIC = comboNic.SelectedIndex;
                Config.Default.StartHour = Convert.ToInt16(cmbStartHour.Text);
                Config.Default.StartMinute = Convert.ToInt16(cmbStartMinute.Text);
                Config.Default.EndHours = Convert.ToInt16(cmbDayEndHour.Text);
                Config.Default.EndHoursMins = Convert.ToInt16(cmbDayEndHourMins.Text);
                Config.Default.TotalHours = Convert.ToInt16(cmbDayEndHour.Text) - Convert.ToInt16(cmbStartHour.Text);
                if (double.TryParse(txtMoney.Text, out Result) == true && Convert.ToDouble(txtMoney.Text) >= 0)
                {
                    Config.Default.Money = Convert.ToDouble(txtMoney.Text);
                }
                else
                {
                    MessageBox.Show("Hourly Wage must be a positive number that's greater than 0");
                    return;
                }
                Config.Default.Save();

                //Refresh the weather
                mainWindow.GetWeather();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                height = 0;

                Task.Factory.StartNew(new Action(() =>
                {
                    //My sketchy attempt at making an animation
                    for (int i = 0; i < 100; i++)
                    {
                        height = (200.00 / 100.00) * i;
                        winOpacity += 0.01;

                        Thread.Sleep(3);
                        
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            SettingsWindow.Height = height;
                            SettingsWindow.Opacity = winOpacity;
                            SettingsGrid.Opacity = winOpacity;
                        }));
                         
                    }
                       
                }));

                UpdateUI();
                //Populate each control with the saved values
                txtZip.Text = Config.Default.ZIP;
                LoadDevices();

                comboDisk.SelectedIndex = Config.Default.Disk;
                comboNic.SelectedIndex = Config.Default.NIC;
                cmbStartHour.Text = Config.Default.StartHour.ToString();
                cmbStartMinute.Text = Config.Default.StartMinute.ToString();
                cmbDayEndHour.Text = Config.Default.EndHours.ToString();
                cmbDayEndHourMins.Text = Config.Default.EndHoursMins.ToString();
                txtMoney.Text = Config.Default.Money.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDevices()
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    //My sketchy attempt at making an animation
                    for (int i = 100; i > 0; i--)
                    {
                        height = (200.00 / 100.00) * i;
                        winOpacity -= 0.01;

                        Thread.Sleep(3);
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            SettingsWindow.Height = height;
                            SettingsWindow.Opacity = winOpacity;
                            SettingsGrid.Opacity = winOpacity;
                        }));
                    }

                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        this.Close();
                       
                    }));
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        double opacity = 0, height = 0, winOpacity = 0;

        private void UpdateUI()
        {
            try
            {
                bool isActive = true;

                Task.Factory.StartNew(new Action(() =>
                {
                    while (isActive)
                    {
                        
                        //Use this to update the UI
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                        {
                            btnExit.Opacity = opacity;
                            isActive = SettingsWindow.IsActive;
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

        private void SettingsWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SettingsGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSave_Click(null, null);
            }
        }
    }
}
