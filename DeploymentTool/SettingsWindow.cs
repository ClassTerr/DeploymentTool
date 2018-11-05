using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeploymentTool
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();

            var profiles = SettingsManager.Instance.Profiles;
            comboBoxProfiles.DataSource = profiles;
            
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsManager.SaveConfig();
            Application.Exit();
        }

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProfileData(comboBoxProfiles.SelectedItem as Profile);
        }

        private void LoadProfileData(Profile profile)
        {
            
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {

        }
    }
}
