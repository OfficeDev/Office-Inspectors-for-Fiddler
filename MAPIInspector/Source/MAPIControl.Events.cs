using System;
using System.Text;
using System.Windows.Forms;

namespace MapiInspector
{
    public partial class MAPIControl
    {
        private void InitializeContextMenus()
        {
            // TreeView context menu
            ContextMenu mapiTreeViewContextMenu = new ContextMenu();
            mapiTreeView.ContextMenu = mapiTreeViewContextMenu;
            MenuItem copyNodeTextMenuItem = mapiTreeView.ContextMenu.MenuItems.Add("Copy selected text");
            MenuItem copySubtreeMenuItem = mapiTreeView.ContextMenu.MenuItems.Add("Copy tree");
            MenuItem expandAllMenuItem = mapiTreeView.ContextMenu.MenuItems.Add("Expand");
            MenuItem collapseNodeMenuItem = mapiTreeView.ContextMenu.MenuItems.Add("Collapse");
            MenuItem toggleDebugMenuItem = mapiTreeView.ContextMenu.MenuItems.Add("Toggle Debug");
            copyNodeTextMenuItem.Click += new EventHandler(TreeView_CopyNodeText_Click);
            copySubtreeMenuItem.Click += new EventHandler(TreeView_CopySubtree_Click);
            expandAllMenuItem.Click += new EventHandler(TreeView_ExpandAll_Click);
            collapseNodeMenuItem.Click += new EventHandler(TreeView_CollapseNode_Click);
            toggleDebugMenuItem.Click += new EventHandler(TreeView_ToggleDebug_Click);

            // MAPI HexBox context menu
            ContextMenu cm = new ContextMenu();
            mapiHexBox.ContextMenu = cm;
            MenuItem copyHexNoSpacesMenuItem = mapiHexBox.ContextMenu.MenuItems.Add("Copy (no spaces)");
            copyHexNoSpacesMenuItem.Click += new EventHandler(HexBox_CopyNoSpaces_Click);
            MenuItem copyHexWithSpacesMenuItem = mapiHexBox.ContextMenu.MenuItems.Add("Copy (with spaces)");
            copyHexWithSpacesMenuItem.Click += new EventHandler(HexBox_CopyWithSpaces_Click);
            MenuItem copyHex16BlocksMenuItem = mapiHexBox.ContextMenu.MenuItems.Add("Copy as 16 byte Blocks");
            copyHex16BlocksMenuItem.Click += new EventHandler(HexBox_Copy16ByteBlocks_Click);
            MenuItem copyHex16BlocksWithPrefixMenuItem = mapiHexBox.ContextMenu.MenuItems.Add("Copy as 16 byte blocks (with prefix)");
            copyHex16BlocksWithPrefixMenuItem.Click += new EventHandler(HexBox_Copy16ByteBlocksWithPrefix_Click);
            MenuItem copyHexAsCodeBlockMenuItem = mapiHexBox.ContextMenu.MenuItems.Add("Copy as 0x00 code block");
            copyHexAsCodeBlockMenuItem.Click += new EventHandler(HexBox_CopyAsCodeBlock_Click);

            // CROPS HexBox context menu
            ContextMenu cm_crops = new ContextMenu();
            cropsHexBox.ContextMenu = cm_crops;
            MenuItem copyCropsHexMenuItem = cropsHexBox.ContextMenu.MenuItems.Add("Copy");
            copyCropsHexMenuItem.Click += new EventHandler(CropsHexBox_CopyHex_Click);

            AttachHexBoxKeyboardHandler(mapiHexBox);
            AttachHexBoxKeyboardHandler(cropsHexBox);
        }

        private void CopyMethod(object sender, EventArgs e, Be.Windows.Forms.HexBox hexBox)
        {
            byte[] targetBytes = new byte[hexBox.SelectionLength];
            Array.Copy(hexBox.GetAllBytes(), hexBox.SelectionStart, targetBytes, 0, hexBox.SelectionLength);
            string hex = BitConverter.ToString(targetBytes).Replace("-", string.Empty);
            if (!string.IsNullOrEmpty(hex))
            {
                Clipboard.SetText(hex);
            }
        }

        private string CleanString(string text)
        {
            return text.TrimEnd('\0').Replace("\0", "\\0");
        }

        private string GetNodeText(TreeNode node)
        {
            if (node == null) return string.Empty;
            if (node.Tag is global::MAPIInspector.Parsers.BaseStructure.Position position && position.SourceBlock != null)
            {
                return CleanString(position.SourceBlock.Text);
            }
            return CleanString(node.Text);
        }

        private void TreeView_CopyNodeText_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetNodeText(mapiTreeView.SelectedNode));
        }

        private void TreeView_CopySubtree_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            GetNodeTreeText(sb, mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0], -1);
            Clipboard.SetText(sb.ToString());
        }

        private void TreeView_ExpandAll_Click(object sender, EventArgs e)
        {
            var node = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            node.ExpandAll();
        }

        private void TreeView_CollapseNode_Click(object sender, EventArgs e)
        {
            var node = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            node.Collapse();
        }

        private void TreeView_ToggleDebug_Click(object sender, EventArgs e)
        {
            Inspector.ToggleDebug();
        }

        private void GetNodeTreeText(StringBuilder sb, TreeNode node, int count)
        {
            var indents = ++count;
            for (int i = 0; i < indents; i++)
                sb.Append("   ");
            sb.AppendLine(GetNodeText(node));
            foreach (TreeNode tn in node.Nodes)
            {
                if (tn.Tag is string tag && tag == "ignore") continue;
                GetNodeTreeText(sb, tn, indents);
            }
        }

        private void HexBox_CopyNoSpaces_Click(object sender, EventArgs e)
        {
            CopyMethod(sender, e, mapiHexBox);
        }

        private void HexBox_CopyWithSpaces_Click(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (var c in targetBytes)
            {
                if (counter != 0)
                    sb.Append(" ");
                counter++;
                sb.Append(c.ToString("x2"));
            }
            Clipboard.SetText(sb.ToString());
        }

        private void HexBox_Copy16ByteBlocks_Click(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (var c in targetBytes)
            {
                if ((counter % 16) == 0 && counter != 0)
                    sb.AppendLine();
                else if (counter != 0)
                    sb.Append(" ");
                counter++;
                sb.Append(c.ToString("x2"));
            }
            Clipboard.SetText(sb.ToString());
        }

        private void HexBox_Copy16ByteBlocksWithPrefix_Click(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("POSITION | 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
            sb.AppendLine("----------------------------------------------------------");

            int counter = 0;
            foreach (var c in targetBytes)
            {
                if ((counter % 16) == 0)
                {
                    if (counter != 0)
                        sb.AppendLine();
                    sb.AppendFormat("${0:X8} | ", (counter / 16) * 16);
                }
                else if (counter != 0)
                    sb.Append(" ");
                counter++;
                sb.Append(c.ToString("x2"));
            }
            Clipboard.SetText(sb.ToString());
        }

        private void HexBox_CopyAsCodeBlock_Click(object sender, EventArgs e)
        {
            byte[] targetBytes = new byte[mapiHexBox.SelectionLength];
            Array.Copy(mapiHexBox.GetAllBytes(), mapiHexBox.SelectionStart, targetBytes, 0, mapiHexBox.SelectionLength);

            StringBuilder sb = new StringBuilder("byte[] arrOutput = { ");
            int counter = 0;
            foreach (var c in targetBytes)
            {
                if (counter != 0)
                    sb.Append(", ");
                counter++;
                sb.Append("0x" + c.ToString("x2"));
            }

            sb.Append(" };");
            Clipboard.SetText(sb.ToString());
        }

        private void CropsHexBox_CopyHex_Click(object sender, EventArgs e)
        {
            CopyMethod(sender, e, cropsHexBox);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            bool isCtrl = (ModifierKeys & Keys.Control) == Keys.Control;
            bool isShift = (ModifierKeys & Keys.Shift) == Keys.Shift;
            if (isCtrl)
            {
                SearchFrames(isShift);
            }
            else
            {
                PerformSearch(isShift);
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool bEnter = e.KeyCode == Keys.Enter;
            bool bShiftEnter = (e.KeyCode == Keys.Enter) && (e.Modifiers == Keys.Shift);

            if (bEnter || bShiftEnter)
            {
                PerformSearch(e.Modifiers == Keys.Shift);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SearchTextBox_GotFocus(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Search (Ctrl+F)")
            {
                searchTextBox.Text = "";
            }
        }

        private void SearchTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Search (Ctrl+F)";
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool isF3 = keyData == Keys.F3;
            bool isShiftF3 = keyData == (Keys.Shift | Keys.F3);
            bool isCtrlF = keyData == (Keys.Control | Keys.F);
            bool isCtrlRight = keyData == (Keys.Control | Keys.Right);
            bool isCtrlF3 = keyData == (Keys.Control | Keys.F3);
            bool isCtrlShiftF3 = keyData == (Keys.Control | Keys.Shift | Keys.F3);

            if (mapiTreeView.Focused)
            {
                if (keyData == (Keys.Control | Keys.C))
                {
                    TreeView_CopyNodeText_Click(mapiTreeView, EventArgs.Empty);
                    return true;
                }

                if (keyData == (Keys.Control | Keys.T))
                {
                    TreeView_CopySubtree_Click(mapiTreeView, EventArgs.Empty);
                    return true;
                }

                if (keyData == (Keys.Control | Keys.Shift | Keys.C))
                {
                    // If cropsHexBox is visible, use it; otherwise, use mapiHexBox
                    if (!splitContainer.Panel2Collapsed)
                        CropsHexBox_CopyHex_Click(cropsHexBox, EventArgs.Empty);
                    else
                        HexBox_CopyNoSpaces_Click(mapiHexBox, EventArgs.Empty);

                    return true;
                }

                if (isCtrlRight)
                {
                    var node = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
                    node.ExpandAll();
                    return true;
                }
            }

            if (isCtrlF)
            {
                searchTextBox.Focus();
                searchTextBox.SelectAll();
                return true;
            }

            if (isCtrlF3 || isCtrlShiftF3)
            {
                SearchFrames(isCtrlShiftF3);
                return true;
            }

            if (isF3 || isShiftF3)
            {
                PerformSearch(keyData.HasFlag(Keys.Shift));
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PerformSearch(bool searchUp)
        {
            string searchText = searchTextBox.Text.Trim();
            if (mapiTreeView.Nodes.Count == 0 ||
                string.IsNullOrEmpty(searchText) ||
                searchText == "Search (Ctrl+F)")
            {
                return;
            }

            var startNode = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            if (startNode == null)
                return;

            var foundNode = searchUp
                ? FindPrevNode(mapiTreeView.Nodes, startNode, searchText, true)
                : FindNextNode(mapiTreeView.Nodes, startNode, searchText, true);

            if (foundNode != null)
            {
                mapiTreeView.SelectedNode = foundNode;
                mapiTreeView.Focus();
                foundNode.EnsureVisible();
            }
        }

        private void SearchFrames(bool searchBackwards)
        {
            string searchText = searchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchText) || searchText == "Search (Ctrl+F)")
                return;

            var startNode = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            TreeNode foundNode = searchBackwards
                ? FindPrevNode(mapiTreeView.Nodes, startNode, searchText, false)
                : FindNextNode(mapiTreeView.Nodes, startNode, searchText, false);

            if (foundNode != null)
            {
                mapiTreeView.SelectedNode = foundNode;
                mapiTreeView.Focus();
                foundNode.EnsureVisible();
                return;
            }

            // TODO: Implement frame iteration logic (load next frame, search, switch to frame on match)
        }

        // Find next node (downwards, wraps around)
        private TreeNode FindNextNode(TreeNodeCollection nodes, TreeNode startNode, string searchText, bool wrap)
        {
            bool foundStart = false;
            TreeNode firstMatch = null;
            foreach (var node in FlattenNodes(nodes))
            {
                if (node == startNode)
                {
                    foundStart = true;
                    continue;
                }
                if (GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (foundStart)
                        return node;
                    if (firstMatch == null)
                        firstMatch = node;
                }
            }
            return wrap ? firstMatch : null;
        }

        // Find previous node (upwards, wraps around)
        private TreeNode FindPrevNode(TreeNodeCollection nodes, TreeNode startNode, string searchText, bool wrap)
        {
            TreeNode lastMatch = null;
            foreach (var node in FlattenNodes(nodes))
            {
                if (node == startNode)
                    break;
                if (GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    lastMatch = node;
            }
            if (lastMatch != null)
                return lastMatch;
            if (wrap)
            {
                foreach (var node in FlattenNodes(nodes))
                {
                    if (GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        lastMatch = node;
                }
                return lastMatch;
            }
            return null;
        }

        // Helper: flatten all nodes in tree (preorder)
        private System.Collections.Generic.IEnumerable<TreeNode> FlattenNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;
                foreach (var child in FlattenNodes(node.Nodes))
                    yield return child;
            }
        }

        private void AttachHexBoxKeyboardHandler(Be.Windows.Forms.HexBox hexBox)
        {
            hexBox.KeyDown += (sender, e) =>
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    CopyMethod(sender, e, hexBox);
                    e.Handled = true;
                }
                else if (e.Control && e.KeyCode == Keys.A)
                {
                    hexBox.Select(0, hexBox.ByteProvider.Length);
                    e.Handled = true;
                }
            };
        }
    }
}
