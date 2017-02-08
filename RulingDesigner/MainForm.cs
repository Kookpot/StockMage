using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DynamicRuling.Conditional;
using DynamicRuling.Instruction;
using DynamicRuling;
using System.Reflection;
using System.Runtime.Serialization;
using DynamicRuling.Value;

namespace RulingDesigner
{
    /// <summary>
    /// main form for designing
    /// </summary>
    public partial class MainForm : Form
    {
        #region Members

        //currently selected node
        private TreeNode _selectedNode;

        #endregion

        #region Constructors

        /// <summary>
        /// standard constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods
    
        /// <summary>
        /// new (clear content)
        /// </summary>
        /// <param name="sender">sender of event</param>
        /// <param name="e">argum</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Text = @"Converter Designer 1.0";
            treeViewContent.Nodes.Clear();
            Converter.Clear();
            treeViewContent.Nodes.Add("InstructionsNode", "Instructions");
        }

        /// <summary>
        /// loading of mainform
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            GetTypesInNamespace("DynamicRuling.Conditional", treeViewTemplates.Nodes[0].Nodes[0]);
            GetTypesInNamespace("DynamicRuling.Instruction", treeViewTemplates.Nodes[0].Nodes[1]);
            GetTypesInNamespace("DynamicRuling.Value", treeViewTemplates.Nodes[0].Nodes[2]);
            treeViewContent.ExpandAll();
            treeViewTemplates.ItemDrag += TreeViewItemDrag;
            treeViewContent.ItemDrag += TreeViewItemDrag;
            treeViewContent.DragOver += TreeViewDragOver;
            treeViewContent.DragDrop += TreeViewDragDrop;
            treeViewContent.MouseClick += TreeViewClicked;
            treeViewContent.MouseMove += TreeViewHover;
        }

        /// <summary>
        /// get types in namespace and create nodes depending on the mode
        /// </summary>
        /// <param name="nameSpace">namespace of loaded dll's</param>
        /// <param name="node">node to add types to</param>
        /// <param name="blnAdvancedMode">advanced mode or not</param>
        private static void GetTypesInNamespace(string nameSpace, TreeNode node)
        {
            node.Nodes.Clear();
            foreach (var typ in Assembly.Load("DynamicRuling, Version=1.0.0.0, Culture=neutral").GetTypes()
                .Where(foundType => nameSpace.Equals(foundType.Namespace)))
            {
                try
                {
                    var check = (IElement)Activator.CreateInstance(typ);
                    if (check.Showable())
                    {
                        var newNode = check.GetNode(string.Empty);
                        node.Nodes.Add(newNode);
                        newNode.Text = typ.Name;
                    }
                }
// ReSharper disable EmptyGeneralCatchClause
                catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
                {
                }
            }
        }

        /// <summary>
        /// hover over item to set the tooltiptext
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event of mousearguments</param>
        private void TreeViewHover(object sender, MouseEventArgs e)
        {
            // Get the node at the current mouse pointer location.
            var node = treeViewContent.GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if ((node != null))
            {
                // Verify that the tag property is not "null".
                if (node.Tag != null)
                {
                    // Change the ToolTip only if the pointer moved to a new node.
                    if (node.ToolTipText != toolTip1.GetToolTip(treeViewContent))
                    {
                        toolTip1.SetToolTip(treeViewContent, node.ToolTipText);
                    }
                }
                else
                {
                    toolTip1.SetToolTip(treeViewContent, string.Empty);
                }
            }
            else     // Pointer is not over a node so clear the ToolTip.
            {
                toolTip1.SetToolTip(treeViewContent, string.Empty);
            }
        }


        /// <summary>
        /// clicked on a treeview (check rightclick for contextmenu's
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of mouseevent</param>
        private void TreeViewClicked(object sender, MouseEventArgs e)
        {
            var destinationNode = ((TreeView)sender).GetNodeAt(new Point(e.X, e.Y));
            _selectedNode = destinationNode;
            if (e.Button == MouseButtons.Right)
            {
                treeViewContent.SelectedNode = destinationNode;
                if (destinationNode.Tag != null)
                {
                    contextMenuStripFile.Show((Control)sender, new Point(e.X, e.Y));
                    contextMenuStripFile.Tag = destinationNode.Tag;
                }
                else
                {
                    contextMenuStripFile3.Show((Control)sender, new Point(e.X, e.Y));
                    contextMenuStripFile3.Tag = destinationNode.Tag;
                }
            }
        }

        /// <summary>
        /// begin drag
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of item drag event</param>
        private void TreeViewItemDrag(object sender, ItemDragEventArgs e)
        {
            ((TreeView)sender).SelectedNode = (TreeNode)e.Item;
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        /// <summary>
        /// drag over
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of dragevent</param>
        private static void TreeViewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                var treeView = (TreeView)sender;
                var point = treeView.PointToClient(new Point(e.X, e.Y));
                var destinationNode = treeView.GetNodeAt(point);
                var newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                e.Effect = AllowThisDrop(newNode, destinationNode) ? DragDropEffects.Move : DragDropEffects.None;
            }
        }

        /// <summary>
        /// drop on item
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of dragevent</param>
        private void TreeViewDragDrop(Object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                var treeView = (TreeView)sender;
                var point = treeView.PointToClient(new Point(e.X, e.Y));
                var destinationNode = treeView.GetNodeAt(point);
                var newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (AllowThisDrop(newNode, destinationNode))
                {
                    var cloneNode = (TreeNode)newNode.Clone();
                    var clone = ((ICloneable)newNode.Tag).Clone();
                    cloneNode.Tag = clone;
                    if (!newNode.TreeView.Equals(destinationNode.TreeView))
                    {
                        cloneNode.Text = cloneNode.Text + @" " + cloneNode.Tag;
                    }
                    if (destinationNode.Tag != null)
                    {
                        var temp = destinationNode.Tag.ToString();
                        AttachItem(destinationNode, ref cloneNode);
                        if (!temp.Equals(string.Empty))
                        {
                            destinationNode.Text = destinationNode.Text.Replace(temp, destinationNode.Tag.ToString());
                        }
                        else
                        {
                            destinationNode.Text = destinationNode.Text + destinationNode.Tag;
                        }
                        CascadeUp(destinationNode);
                    }
                    else
                    {
                        AttachItem(destinationNode, ref cloneNode);
                    }
                    destinationNode.Nodes.Add(cloneNode);
                    destinationNode.Expand();
                    ValidateCheckAll();
                }
            }
        }

        /// <summary>
        /// cascade up depending on currently selected node
        /// </summary>
        /// <param name="node">current node</param>
        private static void CascadeUp(TreeNode node)
        {
            if (node.Tag is ICondition || node.Tag is IValue)
            {
                if (node.Parent != null)
                {
                    CascadeUpper(node.Parent);
                }
            }
        }

        /// <summary>
        /// cascade up depending on currently selected node
        /// </summary>
        /// <param name="objNode">current node</param>
        private static void CascadeUpper(TreeNode objNode)
        {
            if (objNode.Tag is ICondition || objNode.Tag is IValue)
            {
                if (objNode.Parent != null)
                {
                    CascadeUpper(objNode.Parent);
                }
            }
            if (objNode.Tag is IInstruction)
            {
                objNode.Text = objNode.Tag.ToString();
            }
        }

        /// <summary>
        /// check validity of a specific node and does this for its children if asked (blnDeep)
        /// </summary>
        /// <param name="objNode">node to check</param>
        /// <param name="blnDeep">deep or not?</param>
        private static void ValidateCheck(TreeNode objNode, Boolean blnDeep)
        {
            if (!((IElement)objNode.Tag).Check(false))
            {
                objNode.BackColor = Color.Red;
                objNode.ToolTipText = ((IElement)objNode.Tag).CheckToString(false).Replace(Environment.NewLine, " ");
                objNode.Parent.Expand();
            }
            else if (objNode.BackColor != Color.White)
            {
                objNode.BackColor = Color.White;
                objNode.ToolTipText = string.Empty;
            }
            if (blnDeep)
            {
                foreach (TreeNode innerNode in objNode.Nodes)
                {
                    ValidateCheck(innerNode, true);
                }
            }
        }

        /// <summary>
        /// check validaty of all nodes
        /// </summary>
        private void ValidateCheckAll()
        {
            for (var index = 0; index < treeViewContent.Nodes.Count; index++)
            {
                var node = treeViewContent.Nodes[index];
                foreach (TreeNode innerNode in node.Nodes)
                {
                    ValidateCheck(innerNode, true);
                }
            }
        }

        /// <summary>
        /// attach an item to an existing item
        /// </summary>
        /// <param name="destinationNode">the parent node</param>
        /// <param name="newNode">the new node to be attached</param>
        private static void AttachItem(TreeNode destinationNode, ref TreeNode newNode)
        {
            if (destinationNode.Tag != null)
            {
                try
                {
                    var destinationElement = (IElement)destinationNode.Tag;
                    var newElement = (IElement)newNode.Tag;
                    destinationElement.AttachItem(newElement);
                    newNode = newElement.GetNode(destinationElement.GetPrefix(newElement));
                }
// ReSharper disable EmptyGeneralCatchClause
                catch (Exception) { }
// ReSharper restore EmptyGeneralCatchClause
            }
            else if (destinationNode.Name.Equals("InstructionsNode"))
            {
                var instr = (IInstruction)newNode.Tag;
                Converter.GetInstance().AddInstruction(instr);
                newNode = instr.GetNode(string.Empty);
            }
        }

        /// <summary>
        /// is the drop allowed?
        /// </summary>
        /// <param name="newNode">node to be attached</param>
        /// <param name="destinationNode">parent node</param>
        /// <returns>true if drop is allowed, false otherwise</returns>
        private static bool AllowThisDrop(TreeNode newNode, TreeNode destinationNode)
        {
            if (destinationNode != null)
            {
                if (destinationNode.Tag != null && newNode.Tag != null)
                {
                    try
                    {
                        return ((IElement)destinationNode.Tag).AllowDrop(newNode.Tag);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                if (destinationNode.Name.Equals("InstructionsNode"))
                {
                    return newNode.Tag is IInstruction;
                }
            }
            return false;
        }

        /// <summary>
        /// open properties of a node
        /// </summary>
        /// <param name="sender">evetn sender</param>
        /// <param name="e">arguments of event</param>
        private void PropertiesToolStripMenuItemClick(object sender, EventArgs e)
        {
            var propDialog = new PropertyDialog { PropertyObject = ((ToolStripMenuItem) sender).Owner.Tag };
            propDialog.ShowDialog();
            var elem = (IElement)_selectedNode.Tag;
            if (_selectedNode.Parent!=null && _selectedNode.Parent.Tag != null)
            {
                _selectedNode.Text = ((IElement) _selectedNode.Parent.Tag).GetPrefix(elem) + elem.GetNode(string.Empty).Text;
            } 
            else
            {
                _selectedNode.Text =elem.GetNode(string.Empty).Text;
            }
            ValidateCheck(_selectedNode, false);
            CascadeUp(_selectedNode);
        }

        /// <summary>
        /// remove an element
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void RemoveToolStripMenuItemClick(Object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                var destinationNode = _selectedNode.Parent;
                var newNode = _selectedNode;
                if (destinationNode.Tag != null)
                {
                    try
                    {
                        var tempValue = destinationNode.Tag.ToString();
                        ((IElement)destinationNode.Tag).RemoveItem(newNode.Tag);
                        if (!tempValue.Equals(string.Empty))
                        {
                            destinationNode.Text = destinationNode.Text.Replace(tempValue, destinationNode.Tag.ToString());
                        }
                        else
                        {
                            destinationNode.Text = destinationNode.Text + destinationNode.Tag;
                        }
                    }
                    // ReSharper disable EmptyGeneralCatchClause
                    catch (Exception) { }
                    // ReSharper restore EmptyGeneralCatchClause
                }
                else
                {
                    switch (destinationNode.Name)
                    {
                        case "InstructionsNode":
                            Converter.GetInstance().RemoveInstruction((IInstruction)newNode.Tag);
                            break;
                    }
                }
                _selectedNode.Remove();
                ValidateCheckAll();
            }
        }

        /// <summary>
        /// open a file (configuration)
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Serializer.Deserialize(openFileDialog1.FileName);
            }
            ResetInstructionsTreeview();
            treeViewContent.Nodes[0].Expand();
            treeViewContent.Nodes[1].Expand();
            ValidateCheckAll();
            Text = @"Converter Designer 1.0 : " + openFileDialog1.FileName;
        }

        /// <summary>
        /// save the configuration to a file
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Serializer.Serialize(saveFileDialog1.FileName);
                Text = @"Converter Designer 1.0 : " + saveFileDialog1.FileName;
            }
        }

        /// <summary>
        /// save a specific node to to a file
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void SaveToolStripMenuItem1Click(Object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Serializer.Serialize(saveFileDialog1.FileName, (ISerializable)_selectedNode.Tag);
                }
            }
        }

        /// <summary>
        /// add an element from a file to a specific node
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void AddToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_selectedNode != null && _selectedNode.Tag != null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var serial = (IElement)Serializer.DeserializeTo(openFileDialog1.FileName);
                    var newNode = serial.GetNode(string.Empty);
                    if (AllowThisDrop(newNode, _selectedNode))
                    {
                        var tempVal = _selectedNode.Tag.ToString();
                        AttachItem(_selectedNode, ref newNode);
                        if (!tempVal.Equals(string.Empty))
                        {
                            _selectedNode.Text = _selectedNode.Text.Replace(tempVal, _selectedNode.Tag.ToString());
                        }
                        else
                        {
                            _selectedNode.Text = _selectedNode.Text + _selectedNode.Tag;
                        }
                        _selectedNode.Nodes.Add(newNode);
                        _selectedNode.Expand();
                        ValidateCheckAll();
                    }
                }
            }
        }

        /// <summary>
        /// test-convert this solution
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void ConvertToolStripMenuItemClick(object sender, EventArgs e)
        {
            
        }
        
        /// <summary>
        /// search all nodes for a specific word or attribute
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void SearchToolStripMenuItemClick(object sender, EventArgs e)
        {
            var searchDialog = new SearchDialog("Search Pattern");
            if (searchDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (TreeNode node in treeViewContent.Nodes)
                {
                    Search(node, searchDialog.Value);
                }
            }
        }

        /// <summary>
        /// search a node (and all childnodes) for a specific word or attribute
        /// </summary>
        /// <param name="node">node to search</param>
        /// <param name="search">searchpattern</param>
        /// <returns>true if found, false otherwise</returns>
        private static bool Search(TreeNode node, string search)
        {
            var child = false;
#pragma warning disable 168
            foreach (var objChild in
#pragma warning restore 168
                node.Nodes.Cast<TreeNode>().Where(otherchild => Search(otherchild, search)))
            {
                child = true;
                node.Expand();
            }
            if (node.Text.Contains(search))
            {
                node.BackColor = Color.LightSkyBlue;
                child = true;
            }
            return child;
        }

        /// <summary>
        /// add an element from another file to a specific node
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void AddToolStripMenuItemClick1(object sender, EventArgs e)
        {
            if (_selectedNode != null && _selectedNode.Tag == null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var serial = (IElement)Serializer.DeserializeTo(openFileDialog1.FileName);
                    var newNode = serial.GetNode(string.Empty);
                    if (AllowThisDrop(newNode, _selectedNode))
                    {
                        AttachItem(_selectedNode, ref newNode);
                        _selectedNode.Nodes.Add(newNode);
                        _selectedNode.Expand();
                        ValidateCheckAll();
                    }
                }
            }
        }

        /// <summary>
        /// a specific node needs to go up in order
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void UpToolStripMenuItemClick(object sender, EventArgs e)
        {
            MoveNode(false, () => _selectedNode.Index > 0);
        }

        /// <summary>
        /// a specific node needs to go down in order
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">arguments of event</param>
        private void DownToolStripMenuItemClick(object sender, EventArgs e)
        {
            MoveNode(true, () => _selectedNode.Index + 1 < _selectedNode.Parent.Nodes.Count);
        }

        /// <summary>
        /// move a node up or down
        /// </summary>
        /// <param name="positive">down? or up?</param>
        /// <param name="test">test to see if we overflow or not</param>
        private void MoveNode(bool positive, Func<bool> test)
        {
            var toAdd = 1;
            if (positive)
            {
                toAdd = -1;
            }
            if (_selectedNode != null && _selectedNode.Tag != null)
            {
                var index = _selectedNode.Index;
                var parent = _selectedNode.Parent;
                if (parent != null && test())
                {
                    var moveToNode = parent.Nodes[index];
                    var toMoveElement = (IElement)_selectedNode.Tag;
                    var moveToElement = (IElement)moveToNode.Tag;

                    if (parent.Tag != null)
                    {
                        parent.Nodes.Remove(_selectedNode);
                        parent.Nodes.Insert(index + toAdd, _selectedNode);
                        treeViewContent.SelectedNode = _selectedNode;
                        var parentElement = ((IElement)parent.Tag);
                        parentElement.Move(index, positive, toMoveElement, moveToElement);
                        _selectedNode.Text = toMoveElement.GetNode(parentElement.GetPrefix(toMoveElement)).Text;
                        moveToNode.Text = moveToElement.GetNode(parentElement.GetPrefix(moveToElement)).Text;
                    }
                    else if (_selectedNode.Tag is IInstruction)
                    {
                        parent.Nodes.Remove(_selectedNode);
                        parent.Nodes.Insert(index + toAdd, _selectedNode);
                        treeViewContent.SelectedNode = _selectedNode;
                        Converter.GetInstance().Move(index, positive, toMoveElement, moveToElement);
                    }
                }
            }
        }

        /// <summary>
        /// reset instructions node of the content-treeview
        /// </summary>
        private void ResetInstructionsTreeview()
        {
            var rootNode = treeViewContent.Nodes["InstructionsNode"];
            if (rootNode != null)
            {
                rootNode.Nodes.Clear();
                foreach (var node in Converter.GetInstance().GetInstructionNodes())
                {
                    rootNode.Nodes.Add(node);
                }
            }
        }

        #endregion Private Methods
    }
}