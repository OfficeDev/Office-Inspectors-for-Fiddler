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
            this.mapiTreeView.ContextMenu = mapiTreeViewContextMenu;
            MenuItem mapiTreeViewMenuItem1 = this.mapiTreeView.ContextMenu.MenuItems.Add("Copy selected text");
            MenuItem mapiTreeViewMenuItem2 = this.mapiTreeView.ContextMenu.MenuItems.Add("Copy tree");
            MenuItem mapiTreeViewMenuItem3 = this.mapiTreeView.ContextMenu.MenuItems.Add("Expand");
            MenuItem mapiTreeViewMenuItem4 = this.mapiTreeView.ContextMenu.MenuItems.Add("Collapse");
            MenuItem mapiTreeViewMenuItem5 = this.mapiTreeView.ContextMenu.MenuItems.Add("Toggle Debug");
            mapiTreeViewMenuItem1.Click += new EventHandler(MapiTreeViewMenuItem1_Click);
            mapiTreeViewMenuItem2.Click += new EventHandler(MapiTreeViewMenuItem2_Click);
            mapiTreeViewMenuItem3.Click += new EventHandler(MapiTreeViewMenuItem3_Click);
            mapiTreeViewMenuItem4.Click += new EventHandler(MapiTreeViewMenuItem4_Click);
            mapiTreeViewMenuItem5.Click += new EventHandler(MapiTreeViewMenuItem5_Click);

            // MAPI HexBox context menu
            ContextMenu cm = new ContextMenu();
            this.mapiHexBox.ContextMenu = cm;
            MenuItem item = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy (no spaces)");
            item.Click += new EventHandler(MAPI_Copy);
            MenuItem item4 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy (with spaces)");
            item4.Click += new EventHandler(MAPI_CopyWithSpaces);
            MenuItem item2 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy as 16 byte blocks");
            item2.Click += new EventHandler(MAPI_CopyAsByteBlocks);
            MenuItem item3 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy as 16 byte blocks (with prefix)");
            item3.Click += new EventHandler(MAPI_CopyAsByteBlocksWithPrefix);
            MenuItem item5 = this.mapiHexBox.ContextMenu.MenuItems.Add("Copy as 0x00 code block");
            item5.Click += new EventHandler(MAPI_CopyAsCodeBlock);

            // CROPS HexBox context menu
            ContextMenu cm_crops = new ContextMenu();
            this.cropsHexBox.ContextMenu = cm_crops;
            MenuItem item_crops = this.cropsHexBox.ContextMenu.MenuItems.Add("Copy");
            item_crops.Click += new EventHandler(CROPS_Copy);
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

        private void MAPI_Copy(object sender, EventArgs e)
        {
            CopyMethod(sender, e, this.mapiHexBox);
        }

        private void CROPS_Copy(object sender, EventArgs e)
        {
            CopyMethod(sender, e, this.CROPSHexBox);
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

        private void MapiTreeViewMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetNodeText(this.mapiTreeView.SelectedNode));
        }

        private void MapiTreeViewMenuItem2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            GetNodeTreeText(sb, mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0], -1);
            Clipboard.SetText(sb.ToString());
        }

        private void MapiTreeViewMenuItem3_Click(object sender, EventArgs e)
        {
            var node = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            node.ExpandAll();
        }

        private void MapiTreeViewMenuItem4_Click(object sender, EventArgs e)
        {
            var node = mapiTreeView.SelectedNode ?? mapiTreeView.Nodes[0];
            node.Collapse();
        }

        private void MapiTreeViewMenuItem5_Click(object sender, EventArgs e)
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

        private void MAPI_CopyWithSpaces(object sender, EventArgs e)
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

        private void MAPI_CopyAsByteBlocks(object sender, EventArgs e)
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

        private void MAPI_CopyAsByteBlocksWithPrefix(object sender, EventArgs e)
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

        private void MAPI_CopyAsCodeBlock(object sender, EventArgs e)
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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            bool bIsShift = (ModifierKeys & Keys.Shift) == Keys.Shift;
            PerformSearch(bIsShift);
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

            if (isCtrlF)
            {
                searchTextBox.Focus();
                searchTextBox.SelectAll();
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
                ? FindPrevNode(mapiTreeView.Nodes, startNode, searchText)
                : FindNextNode(mapiTreeView.Nodes, startNode, searchText);

            if (foundNode != null)
            {
                mapiTreeView.SelectedNode = foundNode;
                mapiTreeView.Focus();
                foundNode.EnsureVisible();
            }
        }

        // Find next node (downwards, wraps around)
        private TreeNode FindNextNode(TreeNodeCollection nodes, TreeNode startNode, string searchText)
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

                if (foundStart && GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    return node;
                if (firstMatch == null && GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    firstMatch = node;
            }

            // Wrap around
            return firstMatch;
        }

        // Find previous node (upwards, wraps around)
        private TreeNode FindPrevNode(TreeNodeCollection nodes, TreeNode startNode, string searchText)
        {
            TreeNode lastMatch = null;
            foreach (var node in FlattenNodes(nodes))
            {
                if (node == startNode)
                    break;
                if (GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    lastMatch = node;
            }

            // Wrap around: if nothing before, search from end
            if (lastMatch == null)
            {
                foreach (var node in FlattenNodes(nodes))
                {
                    if (GetNodeText(node).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        lastMatch = node;
                }
            }

            return lastMatch;
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
    }
}
