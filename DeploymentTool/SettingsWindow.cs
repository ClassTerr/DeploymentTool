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

            if (prevSelectedID == null)
            {
                CurrentProfile = null;
            }
        }


        private Profile CurrentProfile
        {
            get
            {
                try
                {
                    return new Profile()
                    {
                        ID = Guid.Parse(textBoxId.Text),
                        Name = textBoxName.Text,
                        APICommand = textBoxAPICommand.Text,
                        ExcludedPaths = textBoxExcludedPaths.Text.Split('\n').ToList(),
                        IncludedPaths = textBoxIncludedPaths.Text.Split('\n').ToList()
                    };

                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {

                if (value == null)
                {
                    textBoxId.Text =
                    textBoxName.Text =
                    textBoxAPICommand.Text =
                    textBoxExcludedPaths.Text =
                    textBoxIncludedPaths.Text = "";
                }

                textBoxId.Text = value?.ID.ToString();
                textBoxName.Text = value?.Name;
                textBoxAPICommand.Text = value?.APICommand;
                textBoxExcludedPaths.Text = String.Join("\n", value?.ExcludedPaths ?? new List<string>());
                textBoxIncludedPaths.Text = String.Join("\n", value?.IncludedPaths ?? new List<string>());
            }
        }


        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsManager.SaveConfig();
            Application.Exit();
        }

        private void ComboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentProfile = comboBoxProfiles.SelectedItem as Profile;
        }


        private void SaveButtonClick(object sender, EventArgs e)
        {
            SettingsManager.Instance.UpdateProfile(CurrentProfile);
            SettingsManager.SaveConfig();
            UpdateProfilesComboBox();
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

            comboBoxProfiles.SelectedIndex = comboBoxProfiles.Items.Count - 1;
            ComboBoxProfiles_SelectedIndexChanged(null, null);
        }

        private void ButtonDeleteProfile_Click(object sender, EventArgs e)
        {
            var prevSelected = (comboBoxProfiles.SelectedItem as Profile);
            if (comboBoxProfiles.SelectedItem == null || comboBoxProfiles.SelectedItem as Profile == null)
            {
                MessageBox.Show("Please select something!");
                return;
            }

            var prevSelectedName = prevSelected.Name;
            if (String.IsNullOrWhiteSpace(prevSelectedName))
            {
                prevSelectedName = "Unnamed";
            }

            if (MessageBox.Show($"Are you sure want to remove profile: {prevSelectedName}?",
                "Conformation", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                SettingsManager.Instance.RemoveProfile(prevSelected.ID);
                SettingsManager.SaveConfig();
                UpdateProfilesComboBox();
            }
        }
    }
}
