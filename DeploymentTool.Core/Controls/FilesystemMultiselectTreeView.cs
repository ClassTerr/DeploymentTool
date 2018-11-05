using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DeploymentTool.Core.Helpers;
using System.Text.RegularExpressions;

namespace DeploymentTool.Core.Controls
{
    public partial class FilesystemMultiselectTreeView : UserControl
    {
        public FilesystemMultiselectTreeView()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                string label = drive.RootDirectory.FullName;
                TreeNode rootNode = new TreeNode(label);
                treeView.Nodes.Add(rootNode);
                LoadNodes(rootNode);
            }
        }

        public void LoadNodes(TreeNode currentNode)
        {
            currentNode.Nodes.Clear();
            string[] subDirectories;
            string[] files;
            try
            {
                subDirectories = Directory.GetDirectories(currentNode.FullPath);

                foreach (var subDir in subDirectories)
                {
                    TreeNode node = new TreeNode()
                    {
                        Text = Path.GetFileName(subDir),
                        ImageKey = "FolderIcon.ico"
                    };

                    node.Nodes.Add("");
                    currentNode.Nodes.Add(node);
                }
            }
            catch (Exception e)
            {
                currentNode.Nodes.Add("Unable to access");
            }


            try
            {
                files = Directory.GetFiles(currentNode.FullPath);


                foreach (var file in files)
                {
                    TreeNode node = new TreeNode(Path.GetFileName(file));

                    try
                    {
                        var index = imageList1.Images.IndexOfKey(file);
                        if (index == -1)
                            imageList1.Images.Add(file, IconReader.GetFileIcon(NormalizePath(file), IconReader.IconSize.Small, true));

                        node.ImageKey = file;
                        node.SelectedImageKey = file;
                    }
                    catch (Exception e)
                    {
                        node.ImageKey = "File.ico";
                        node.SelectedImageKey = "File.ico";
                    }

                    currentNode.Nodes.Add(node);
                }
            }
            catch (Exception e)
            {
            }
        }

        public List<string> GetSelectedPaths()
        {
            List<string> resultList = new List<string>();
            GetCheckedPaths(treeView.Nodes, resultList);

            resultList = resultList.Select(NormalizePath).ToList();

            return resultList;
        }

        private void GetCheckedPaths(TreeNodeCollection nodes, List<string> resultList)
        {
            foreach (TreeNode aNode in nodes)
            {
                if (aNode.Checked)
                    resultList.Add(aNode.FullPath);

                if (aNode.Nodes.Count != 0)
                    GetCheckedPaths(aNode.Nodes, resultList);
            }
        }

        private void SetCheckedPaths(TreeNodeCollection nodes, List<string> paths)
        {
            throw new NotImplementedException();
            foreach (string item in paths)
            {
                string path = Path.GetFullPath(item);
                List<string> parts = path
                    .Split(Path.DirectorySeparatorChar)
                    .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                
            }
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            LoadNodes(e.Node);
        }

        private string NormalizePath(string path)
        {
            //pattern to remove double slashes
            var pattern = @"[\\/]{2}"; //can't use Path.DirectorySeparatorChar because regexp
            string result = Regex.Replace(path, pattern, Path.DirectorySeparatorChar.ToString());
            return result;
        }
    }
}
