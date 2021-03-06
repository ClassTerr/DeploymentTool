﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeploymentTool.Core.Controls
{
    public partial class FilesystemMultiselectDialog : Form
    {
        public FilesystemMultiselectDialog()
        {
            InitializeComponent();
        }
        public FilesystemMultiselectDialog(string rootDirectory)
        {
            InitializeComponent();
            filesystemTreeView.RootDirectory = rootDirectory;
        }
        public string RootDirectory
        {
            get
            {
                return filesystemTreeView.RootDirectory;
            }
            set
            {
                filesystemTreeView.RootDirectory = value;
            }
        }

        public List<string> SelectedPaths
        {
            get { return GetSelectedPaths(); }
        }

        public List<string> GetSelectedPaths()
        {
            return filesystemTreeView.GetSelectedPaths();
        }
    }
}
