
namespace SteamUpdateBlocker {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.installedApplicationLabel = new System.Windows.Forms.Label();
            this.installedApplicationList = new System.Windows.Forms.ComboBox();
            this.loadInstalledApplications = new System.Windows.Forms.Button();
            this.updatedManifests = new System.Windows.Forms.ListView();
            this.depotId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.manifest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.recentManifests = new System.Windows.Forms.Label();
            this.getUpdatedManifests = new System.Windows.Forms.Button();
            this.disableAutoUpdate = new System.Windows.Forms.CheckBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.loadInstalledProgress = new System.Windows.Forms.ProgressBar();
            this.loadToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.retrieveToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.disableAutoUpdateToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.applyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // installedApplicationLabel
            // 
            this.installedApplicationLabel.AutoSize = true;
            this.installedApplicationLabel.Location = new System.Drawing.Point(13, 13);
            this.installedApplicationLabel.Name = "installedApplicationLabel";
            this.installedApplicationLabel.Size = new System.Drawing.Size(132, 13);
            this.installedApplicationLabel.TabIndex = 0;
            this.installedApplicationLabel.Text = "Select installed application";
            // 
            // installedApplicationList
            // 
            this.installedApplicationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.installedApplicationList.Enabled = false;
            this.installedApplicationList.FormattingEnabled = true;
            this.installedApplicationList.Location = new System.Drawing.Point(13, 30);
            this.installedApplicationList.Name = "installedApplicationList";
            this.installedApplicationList.Size = new System.Drawing.Size(247, 21);
            this.installedApplicationList.TabIndex = 1;
            // 
            // loadInstalledApplications
            // 
            this.loadInstalledApplications.Location = new System.Drawing.Point(274, 28);
            this.loadInstalledApplications.Name = "loadInstalledApplications";
            this.loadInstalledApplications.Size = new System.Drawing.Size(75, 23);
            this.loadInstalledApplications.TabIndex = 2;
            this.loadInstalledApplications.Text = "Load";
            this.loadInstalledApplications.UseVisualStyleBackColor = true;
            this.loadInstalledApplications.Click += new System.EventHandler(this.LoadInstalledApplications_Click);
            // 
            // updatedManifests
            // 
            this.updatedManifests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.depotId,
            this.manifest});
            this.updatedManifests.GridLines = true;
            this.updatedManifests.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.updatedManifests.HideSelection = false;
            this.updatedManifests.Location = new System.Drawing.Point(13, 102);
            this.updatedManifests.Name = "updatedManifests";
            this.updatedManifests.Size = new System.Drawing.Size(247, 74);
            this.updatedManifests.TabIndex = 4;
            this.updatedManifests.UseCompatibleStateImageBehavior = false;
            this.updatedManifests.View = System.Windows.Forms.View.Details;
            // 
            // depotId
            // 
            this.depotId.Text = "Depot ID";
            // 
            // manifest
            // 
            this.manifest.Text = "Manifest";
            this.manifest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.manifest.Width = 160;
            // 
            // recentManifests
            // 
            this.recentManifests.AutoSize = true;
            this.recentManifests.Location = new System.Drawing.Point(13, 84);
            this.recentManifests.Name = "recentManifests";
            this.recentManifests.Size = new System.Drawing.Size(95, 13);
            this.recentManifests.TabIndex = 6;
            this.recentManifests.Text = "Updated manifests";
            // 
            // getUpdatedManifests
            // 
            this.getUpdatedManifests.Enabled = false;
            this.getUpdatedManifests.Location = new System.Drawing.Point(274, 124);
            this.getUpdatedManifests.Name = "getUpdatedManifests";
            this.getUpdatedManifests.Size = new System.Drawing.Size(75, 23);
            this.getUpdatedManifests.TabIndex = 7;
            this.getUpdatedManifests.Text = "Retrieve";
            this.getUpdatedManifests.UseVisualStyleBackColor = true;
            this.getUpdatedManifests.Click += new System.EventHandler(this.GetUpdatedManifests_Click);
            // 
            // disableAutoUpdate
            // 
            this.disableAutoUpdate.AutoSize = true;
            this.disableAutoUpdate.Location = new System.Drawing.Point(13, 186);
            this.disableAutoUpdate.Name = "disableAutoUpdate";
            this.disableAutoUpdate.Size = new System.Drawing.Size(130, 17);
            this.disableAutoUpdate.TabIndex = 8;
            this.disableAutoUpdate.Text = "Disable Auto Update?";
            this.disableAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // applyButton
            // 
            this.applyButton.Enabled = false;
            this.applyButton.Location = new System.Drawing.Point(185, 182);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(164, 23);
            this.applyButton.TabIndex = 9;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // loadInstalledProgress
            // 
            this.loadInstalledProgress.Location = new System.Drawing.Point(13, 58);
            this.loadInstalledProgress.Name = "loadInstalledProgress";
            this.loadInstalledProgress.Size = new System.Drawing.Size(247, 23);
            this.loadInstalledProgress.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 218);
            this.Controls.Add(this.loadInstalledProgress);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.disableAutoUpdate);
            this.Controls.Add(this.getUpdatedManifests);
            this.Controls.Add(this.recentManifests);
            this.Controls.Add(this.updatedManifests);
            this.Controls.Add(this.loadInstalledApplications);
            this.Controls.Add(this.installedApplicationList);
            this.Controls.Add(this.installedApplicationLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label installedApplicationLabel;
        private System.Windows.Forms.ComboBox installedApplicationList;
        private System.Windows.Forms.Button loadInstalledApplications;
        private System.Windows.Forms.ListView updatedManifests;
        private System.Windows.Forms.Label recentManifests;
        private System.Windows.Forms.Button getUpdatedManifests;
        private System.Windows.Forms.ColumnHeader depotId;
        private System.Windows.Forms.ColumnHeader manifest;
        private System.Windows.Forms.CheckBox disableAutoUpdate;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.ProgressBar loadInstalledProgress;
        private System.Windows.Forms.ToolTip loadToolTip;
        private System.Windows.Forms.ToolTip retrieveToolTip;
        private System.Windows.Forms.ToolTip disableAutoUpdateToolTip;
        private System.Windows.Forms.ToolTip applyToolTip;
    }
}

