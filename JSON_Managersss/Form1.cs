using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace JSON_Managersss
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ! REMOVE NODE FROM TREE !
        private void button2_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        // ! ADD NODE TO TREE !
        private void button3_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("You didn't write name of node.", "Node name exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    treeView1.Nodes.Add(textBox1.Text);
                }
            }
            else
            {
                treeView1.SelectedNode.Nodes.Add(textBox1.Text);
            }
        }

        // ! SAVE JSON !
        private void button4_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.Description = "Choose a folder";

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedFolder = folderBrowserDialog.SelectedPath;
                string jsonFilePath = Path.Combine(selectedFolder, $"{textBox2.Text}.json");

                List<NodeData> nodesData = new List<NodeData>();

                foreach (TreeNode node in treeView1.Nodes)
                {
                    CollectNodeData(node, nodesData);
                }

                string json = JsonConvert.SerializeObject(nodesData, Formatting.Indented);
                File.WriteAllText(jsonFilePath, json);
            }
        }

        // => Method to get data about node.
        private void CollectNodeData(TreeNode node, List<NodeData> nodesData)
        {
            NodeData nodeData = new NodeData
            {
                Text = node.Text,
                ChildNodes = new List<NodeData>()
            };

            foreach (TreeNode childNode in node.Nodes)
            {
                CollectNodeData(childNode, nodeData.ChildNodes);
            }

            nodesData.Add(nodeData);
        }

        // => Class to represent node data.
        public class NodeData
        {
            public string Text { get; set; }
            public List<NodeData> ChildNodes { get; set; }
        }
    }
}
