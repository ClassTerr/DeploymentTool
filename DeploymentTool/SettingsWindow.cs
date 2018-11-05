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

            UpdateProfilesComboBox();
        }

        public void UpdateProfilesComboBox()
        {
            var prevSelectedID = (comboBoxProfiles.SelectedItem as Profile)?.ID;
            comboBoxProfiles.Items.Clear();

            var profiles = SettingsManager.Instance.Profiles;

            var newSelectionIndex = profiles.FindIndex(x => x.ID == prevSelectedID);

            comboBoxProfiles.Items.AddRange(profiles.ToArray());

            if (newSelectionIndex != -1)
            {
                comboBoxProfiles.SelectedIndex = newSelectionIndex;
            }
            else
            {
                if (comboBoxProfiles.Items.Count > 0)
                    comboBoxProfiles.SelectedIndex = 0;
            }
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

        private void ButtonAddProfile_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile()
            {
                Name = "New profile"
            };

            SettingsManager.Instance.Profiles.Add(profile);
            SettingsManager.SaveConfig();

            UpdateProfilesComboBox();
        }

        private void ButtonDeleteProfile_Click(object sender, EventArgs e)
        {
            var prevSelected = (comboBoxProfiles.SelectedItem as Profile);
            if (comboBoxProfiles.SelectedItem == null || comboBoxProfiles.SelectedItem as Profile == null)
            {
                MessageBox.Show("Please select something!");
                return;
            }

            var prevSelectedName = prevSelected.Name ?? "Unnamed";

            if (MessageBox.Show($"Are you sure want to remove profile: ${prevSelectedName}?") == DialogResult.OK)
            {
                SettingsManager.Instance.RemoveProfile(prevSelected.ID);
                SettingsManager.SaveConfig();
                UpdateProfilesComboBox();
            }
        }
    }
}
