using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net.NetworkInformation;
using System.IO;
using TestBed.Properties;

namespace TestBed
{
    public partial class Settings : Form
    {
        private MainWindow mainWindow;

        public Settings( MainWindow mainWindow )
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double Result = 0;
            try
            {
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

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            try
            {
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
    }
}
