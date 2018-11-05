namespace DeploymentTool
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label labelProfiles;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button SaveButton;
            System.Windows.Forms.Button RestoreButton;
            System.Windows.Forms.Panel panel3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxIncludedPaths = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonEditIncludedPaths = new System.Windows.Forms.Button();
            this.textBoxExcludedPaths = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonEditExcludedPaths = new System.Windows.Forms.Button();
            this.textBoxAPICommand = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.taskbarIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.taskbarIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.depoyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxProfiles = new System.Windows.Forms.ComboBox();
            this.buttonDeleteProfile = new System.Windows.Forms.Button();
            this.buttonAddProfile = new System.Windows.Forms.Button();
            labelProfiles = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1 = new System.Windows.Forms.Panel();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            SaveButton = new System.Windows.Forms.Button();
            RestoreButton = new System.Windows.Forms.Button();
            panel3 = new System.Windows.Forms.Panel();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            panel3.SuspendLayout();
            this.taskbarIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProfiles
            // 
            labelProfiles.AutoSize = true;
            labelProfiles.Location = new System.Drawing.Point(12, 15);
            labelProfiles.Name = "labelProfiles";
            labelProfiles.Size = new System.Drawing.Size(36, 13);
            labelProfiles.TabIndex = 4;
            labelProfiles.Text = "Profile";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(panel1);
            groupBox1.Location = new System.Drawing.Point(12, 39);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(924, 444);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(this.splitContainer1);
            panel1.Controls.Add(this.textBoxAPICommand);
            panel1.Controls.Add(this.textBoxName);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(3, 16);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(918, 425);
            panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 85);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(909, 337);
            this.splitContainer1.SplitterDistance = 445;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 16;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.textBoxIncludedPaths);
            groupBox2.Controls.Add(this.panel4);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(445, 337);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "Included paths list";
            // 
            // textBoxIncludedPaths
            // 
            this.textBoxIncludedPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxIncludedPaths.Location = new System.Drawing.Point(3, 16);
            this.textBoxIncludedPaths.Multiline = true;
            this.textBoxIncludedPaths.Name = "textBoxIncludedPaths";
            this.textBoxIncludedPaths.Size = new System.Drawing.Size(439, 283);
            this.textBoxIncludedPaths.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.buttonEditIncludedPaths);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 299);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(439, 35);
            this.panel4.TabIndex = 1;
            // 
            // buttonEditIncludedPaths
            // 
            this.buttonEditIncludedPaths.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonEditIncludedPaths.Location = new System.Drawing.Point(185, 6);
            this.buttonEditIncludedPaths.Name = "buttonEditIncludedPaths";
            this.buttonEditIncludedPaths.Size = new System.Drawing.Size(75, 23);
            this.buttonEditIncludedPaths.TabIndex = 9;
            this.buttonEditIncludedPaths.Text = "Edit";
            this.buttonEditIncludedPaths.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.textBoxExcludedPaths);
            groupBox3.Controls.Add(this.panel2);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox3.Location = new System.Drawing.Point(0, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(456, 337);
            groupBox3.TabIndex = 15;
            groupBox3.TabStop = false;
            groupBox3.Text = "Excluded paths list";
            // 
            // textBoxExcludedPaths
            // 
            this.textBoxExcludedPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExcludedPaths.Location = new System.Drawing.Point(3, 16);
            this.textBoxExcludedPaths.Multiline = true;
            this.textBoxExcludedPaths.Name = "textBoxExcludedPaths";
            this.textBoxExcludedPaths.Size = new System.Drawing.Size(450, 283);
            this.textBoxExcludedPaths.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonEditExcludedPaths);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 299);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(450, 35);
            this.panel2.TabIndex = 1;
            // 
            // buttonEditExcludedPaths
            // 
            this.buttonEditExcludedPaths.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonEditExcludedPaths.Location = new System.Drawing.Point(185, 6);
            this.buttonEditExcludedPaths.Name = "buttonEditExcludedPaths";
            this.buttonEditExcludedPaths.Size = new System.Drawing.Size(75, 23);
            this.buttonEditExcludedPaths.TabIndex = 9;
            this.buttonEditExcludedPaths.Text = "Edit";
            this.buttonEditExcludedPaths.UseVisualStyleBackColor = true;
            // 
            // textBoxAPICommand
            // 
            this.textBoxAPICommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAPICommand.Location = new System.Drawing.Point(82, 48);
            this.textBoxAPICommand.Name = "textBoxAPICommand";
            this.textBoxAPICommand.Size = new System.Drawing.Size(833, 20);
            this.textBoxAPICommand.TabIndex = 12;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(82, 22);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(833, 20);
            this.textBoxName.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 51);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(73, 13);
            label2.TabIndex = 10;
            label2.Text = "API command";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(35, 13);
            label1.TabIndex = 11;
            label1.Text = "Name";
            // 
            // SaveButton
            // 
            SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            SaveButton.Location = new System.Drawing.Point(861, 5);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(75, 23);
            SaveButton.TabIndex = 9;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // RestoreButton
            // 
            RestoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            RestoreButton.Location = new System.Drawing.Point(780, 5);
            RestoreButton.Name = "RestoreButton";
            RestoreButton.Size = new System.Drawing.Size(75, 23);
            RestoreButton.TabIndex = 9;
            RestoreButton.Text = "Restore";
            RestoreButton.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            panel3.Controls.Add(RestoreButton);
            panel3.Controls.Add(SaveButton);
            panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel3.Location = new System.Drawing.Point(0, 489);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(948, 33);
            panel3.TabIndex = 11;
            // 
            // taskbarIcon
            // 
            this.taskbarIcon.ContextMenuStrip = this.taskbarIconContextMenu;
            this.taskbarIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskbarIcon.Icon")));
            this.taskbarIcon.Text = "Deployment Tool Client";
            this.taskbarIcon.Visible = true;
            // 
            // taskbarIconContextMenu
            // 
            this.taskbarIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.depoyToolStripMenuItem,
            this.toolStripComboBox1,
            this.toolStripSeparator1,
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.taskbarIconContextMenu.Name = "taskbarIconContextMenu";
            this.taskbarIconContextMenu.Size = new System.Drawing.Size(182, 103);
            // 
            // depoyToolStripMenuItem
            // 
            this.depoyToolStripMenuItem.Name = "depoyToolStripMenuItem";
            this.depoyToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.depoyToolStripMenuItem.Text = "&Depoy";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "RC",
            "LIVE"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.openToolStripMenuItem.Text = "&Open settings";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // comboBoxProfiles
            // 
            this.comboBoxProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProfiles.FormattingEnabled = true;
            this.comboBoxProfiles.Location = new System.Drawing.Point(54, 12);
            this.comboBoxProfiles.Name = "comboBoxProfiles";
            this.comboBoxProfiles.Size = new System.Drawing.Size(738, 21);
            this.comboBoxProfiles.TabIndex = 3;
            this.comboBoxProfiles.SelectedIndexChanged += new System.EventHandler(this.comboBoxProfiles_SelectedIndexChanged);
            // 
            // buttonDeleteProfile
            // 
            this.buttonDeleteProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteProfile.Location = new System.Drawing.Point(870, 11);
            this.buttonDeleteProfile.Name = "buttonDeleteProfile";
            this.buttonDeleteProfile.Size = new System.Drawing.Size(66, 23);
            this.buttonDeleteProfile.TabIndex = 9;
            this.buttonDeleteProfile.Text = "Delete";
            this.buttonDeleteProfile.UseVisualStyleBackColor = true;
            this.buttonDeleteProfile.Click += new System.EventHandler(this.ButtonDeleteProfile_Click);
            // 
            // buttonAddProfile
            // 
            this.buttonAddProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddProfile.Location = new System.Drawing.Point(798, 11);
            this.buttonAddProfile.Name = "buttonAddProfile";
            this.buttonAddProfile.Size = new System.Drawing.Size(66, 23);
            this.buttonAddProfile.TabIndex = 9;
            this.buttonAddProfile.Text = "Add";
            this.buttonAddProfile.UseVisualStyleBackColor = true;
            this.buttonAddProfile.Click += new System.EventHandler(this.ButtonAddProfile_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(948, 522);
            this.Controls.Add(this.buttonAddProfile);
            this.Controls.Add(this.buttonDeleteProfile);
            this.Controls.Add(panel3);
            this.Controls.Add(labelProfiles);
            this.Controls.Add(this.comboBoxProfiles);
            this.Controls.Add(groupBox1);
            this.Name = "SettingsWindow";
            this.Text = "Deployment Tool Client";
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.panel4.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            this.panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            this.taskbarIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon taskbarIcon;
        private System.Windows.Forms.ContextMenuStrip taskbarIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem depoyToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBoxProfiles;
        private System.Windows.Forms.TextBox textBoxExcludedPaths;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonEditExcludedPaths;
        private System.Windows.Forms.TextBox textBoxIncludedPaths;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonEditIncludedPaths;
        private System.Windows.Forms.TextBox textBoxAPICommand;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonDeleteProfile;
        private System.Windows.Forms.Button buttonAddProfile;
    }
}

