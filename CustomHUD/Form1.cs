using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomHUD
{
    public partial class Form1 : Form
    {

        FormOverlay frm;
        string windowTitle;
        string crosshairColor;
        
        public Form1()
        {
            InitializeComponent();

            // Get titles of all processes
            string[] titles = Process.GetProcesses().Select(x => x.MainWindowTitle).ToArray();

            // Assign the SavedWindowTitle setting to the windowTitle variable
            windowTitle = Properties.Settings.Default.SavedWindowTitle;

            // Assign the SaveCrosshairColor setting to the crosshaircolor variable
            crosshairColor = Properties.Settings.Default.SavedCrosshairColor;
            comboBoxColors.Text = crosshairColor;

            // Clear items in combobox
            comboBoxProcess.Items.Clear();

            // Input titles into combox box
            foreach (string title in titles)
            {
                // If string is not empty or null, add it to the combox box
                if (!string.IsNullOrEmpty(title))
                {
                    comboBoxProcess.Items.Add(title);

                    if (windowTitle == title)
                    {
                        comboBoxProcess.Text = windowTitle;
                    }
                }
            }

        }

        private void checkBoxEnable_CheckedChanged(object sender, EventArgs e)
        {
            // Set windowTitle to combobox text 
            windowTitle = comboBoxProcess.Text;

            // On Checkbox change enable or disable the overlay
            if (checkBoxEnable.Checked)
            {
                frm = new FormOverlay(this, windowTitle);
                frm.Show();
            }
            else
            {
                frm.Hide();
                frm = null;
            }
        }

        private void comboBoxProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save combobox text as windowTitle setting
            Properties.Settings.Default.SavedWindowTitle = comboBoxProcess.Text;

            // Save settings
            Properties.Settings.Default.Save();
        }

        private void comboBoxColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save combobox text as crosshaircolor setting
            Properties.Settings.Default.SavedCrosshairColor = comboBoxColors.Text;

            // Save settings
            Properties.Settings.Default.Save();

            if(frm != null)
                frm.SetCrosshairColor(Color.FromName(comboBoxColors.Text));
        }
    }
}
