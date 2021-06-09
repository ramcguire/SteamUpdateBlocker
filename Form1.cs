using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SteamUpdateBlocker {
    public partial class Form1 : Form {
        private readonly SteamEnumerator steamEnum = new SteamEnumerator();
        public Form1() {
            InitializeComponent();
            this.installedApplicationList.SelectedIndexChanged += new System.EventHandler(InstalledApplicationList_Changed);
            this.Text = "Steam Update Blocker";
            loadToolTip.SetToolTip(loadInstalledApplications, "Scans for all installed Steam applications");
            retrieveToolTip.SetToolTip(getUpdatedManifests, "Retrieves the most recent manifests for the selection application");
            disableAutoUpdateToolTip.SetToolTip(disableAutoUpdate, "Disables auto update setting for the selected application");
            applyToolTip.SetToolTip(applyButton, "Applies the update blocking manifests");
        }

        private async void LoadInstalledApplications_Click(object sender, EventArgs e) {
            loadInstalledApplications.Enabled = false;
            loadInstalledApplications.Text = "Loading...";
            installedApplicationList.Enabled = false;
            getUpdatedManifests.Enabled = false;
            updatedManifests.Items.Clear();
            int filesProcessed = await steamEnum.ProcessManifests(new Progress<int>(percent => loadInstalledProgress.Value = percent));
            installedApplicationList.DataSource = steamEnum.GetNames();
            loadInstalledApplications.Enabled = true;
            installedApplicationList.Enabled = true;
            loadInstalledApplications.Text = "Refresh";
            getUpdatedManifests.Enabled = true;
        }

        private void InstalledApplicationList_Changed(object sender, EventArgs e) {
            updatedManifests.Items.Clear();
            applyButton.Enabled = false;
        }

        private async void GetUpdatedManifests_Click(object sender, EventArgs e) {
            getUpdatedManifests.Enabled = false;
            getUpdatedManifests.Text = "Loading...";
            updatedManifests.Items.Clear();
            Dictionary<string, string> latest = await steamEnum.GetUpdatedManifests(installedApplicationList.Text);
            foreach (KeyValuePair<string, string> kv in latest) {
                updatedManifests.Items.Add(new ListViewItem(new string[] { kv.Key, kv.Value }));
            }
            applyButton.Enabled = true;
            getUpdatedManifests.Enabled = true;
            getUpdatedManifests.Text = "Retrieve";

        }

        private void ApplyButton_Click(object sender, EventArgs e) {
            applyButton.Enabled = false;
            steamEnum.PreventUpdate(installedApplicationList.Text, disableAutoUpdate.Checked);
            applyButton.Enabled = true;
        }
    }
}
