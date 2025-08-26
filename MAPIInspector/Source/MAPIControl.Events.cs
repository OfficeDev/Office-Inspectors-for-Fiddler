using Fiddler;
using MAPIInspector.Parsers;
using System;
using System.Text;
using System.Windows.Forms;

namespace MapiInspector
{
    public partial class MAPIControl
    {
        public static readonly string SearchDefaultText = "Search (Ctrl+F) F3 to search. Hold SHIFT to search backwards. Hold CTRL to search across frames.";
        private readonly System.Drawing.Color SearchDefaultColor = System.Drawing.Color.Gray;
        private readonly System.Drawing.Color SearchNormalColor = System.Drawing.Color.Black;

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
            var isCtrl = (ModifierKeys & Keys.Control) == Keys.Control;
            var isShift = (ModifierKeys & Keys.Shift) == Keys.Shift;
            PerformSearch(isShift, isCtrl);

        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool bEnter = e.KeyCode == Keys.Enter;
            bool isCtrl = (ModifierKeys & Keys.Control) == Keys.Control;
            bool isShift = (ModifierKeys & Keys.Shift) == Keys.Shift;

            if (bEnter)
            {
                PerformSearch(isShift, isCtrl);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SearchTextBox_GotFocus(object sender, EventArgs e)
        {
            if (searchTextBox.Text == SearchDefaultText)
            {
                searchTextBox.Text = "";
                searchTextBox.ForeColor = SearchNormalColor;
            }
        }

        private void SearchTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = SearchDefaultText;
                searchTextBox.ForeColor = SearchDefaultColor;
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            bool isF3 = keyData.HasFlag(Keys.F3);
            bool isCtrlF = keyData == (Keys.Control | Keys.F);
            bool isCtrlRight = keyData == (Keys.Control | Keys.Right);

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

            if (isF3)
            {
                PerformSearch(keyData.HasFlag(Keys.Shift), keyData.HasFlag(Keys.Control));
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Combined search logic for single and multi-frame search
        private void PerformSearch(bool searchUp, bool searchFrames)
        {
            var searchText = searchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchText) || searchText == SearchDefaultText) return;

            FiddlerApplication.UI.SetStatusText($"Searching for {searchText}");
            var includeRoot = false; // Don't include the currently selected node for a seach
            var startNode = mapiTreeView?.SelectedNode;
            if (startNode == null && mapiTreeView?.Nodes.Count > 0)
            {
                startNode = mapiTreeView.Nodes[0];
                includeRoot = true; // But if no node was selected, incude whatever node we start from
            }

            if (startNode != null)
            {
                var nodes = mapiTreeView.Nodes;
                TreeNode foundNode = searchUp
                    ? FindPrevNode(nodes, startNode, searchText, !searchFrames, includeRoot)
                    : FindNextNode(nodes, startNode, searchText, !searchFrames, includeRoot);

                if (foundNode != null)
                {
                    mapiTreeView.SelectedNode = foundNode;
                    mapiTreeView.Focus();
                    foundNode.EnsureVisible();
                    return;
                }
            }

            if (searchFrames)
            {
                includeRoot = true; // Now that we're searching new frames, always include the root node in our search
                var currentSession = Inspector.session;
                while (currentSession != null)
                {
                    var nextSession = searchUp ? currentSession?.Previous() : currentSession?.Next();
                    if (nextSession == null) break;
                    var parseResult = MAPIParser.ParseHTTPPayload(nextSession, Inspector.Direction, out var bytes);
                    var rootNode = new TreeNode();
                    var node = BaseStructure.AddBlock(parseResult, false);
                    rootNode.Nodes.Add(node);
                    var foundMatch = searchUp
                        ? FindPrevNode(rootNode.Nodes, node, searchText, true, includeRoot)
                        : FindNextNode(rootNode.Nodes, node, searchText, true, includeRoot);
                    if (foundMatch != null)
                    {
                        FiddlerApplication.UI.SelectSessionsMatchingCriteria(s => s.id == nextSession.id);
                        nextSession.ViewItem.EnsureVisible();
                        return;
                    }

                    currentSession = nextSession;
                }
            }

            FiddlerApplication.UI.SetStatusText("No match found");
        }

        // Find next node (downwards, wraps around)
        private TreeNode FindNextNode(TreeNodeCollection nodes, TreeNode startNode, string searchText, bool wrap, bool matchStartNode)
        {
            var allNodes = FlattenNodes(nodes);
            int startIndex = 0;
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i] == startNode)
                {
                    startIndex = i;
                    break;
                }
            }
            // Search from startNode (inclusive/exclusive) to end
            int first = matchStartNode ? startIndex : startIndex + 1;
            for (int i = first; i < allNodes.Count; i++)
            {
                if (GetNodeText(allNodes[i]).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    return allNodes[i];
            }
            // If wrap is enabled, search from beginning up to startNode (exclusive)
            if (wrap)
            {
                for (int i = 0; i < startIndex; i++)
                {
                    if (GetNodeText(allNodes[i]).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        return allNodes[i];
                }
            }
            return null;
        }

        // Find previous node (upwards, wraps around)
        private TreeNode FindPrevNode(TreeNodeCollection nodes, TreeNode startNode, string searchText, bool wrap, bool matchStartNode)
        {
            var allNodes = FlattenNodes(nodes);
            int startIndex = 0;
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i] == startNode)
                {
                    startIndex = i;
                    break;
                }
            }
            // Search backwards from startNode (inclusive/exclusive) to beginning
            int first = matchStartNode ? startIndex : startIndex - 1;
            for (int i = first; i >= 0; i--)
            {
                if (GetNodeText(allNodes[i]).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    return allNodes[i];
            }
            // If wrap is enabled, search from end down to startNode (exclusive)
            if (wrap)
            {
                for (int i = allNodes.Count - 1; i > startIndex; i--)
                {
                    if (GetNodeText(allNodes[i]).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        return allNodes[i];
                }
            }
            return null;
        }

        // Helper: flatten all nodes in tree (preorder) and return as a list
        private System.Collections.Generic.List<TreeNode> FlattenNodes(TreeNodeCollection nodes)
        {
            var result = new System.Collections.Generic.List<TreeNode>();
            var stack = new System.Collections.Generic.Stack<TreeNode>();
            for (int i = nodes.Count - 1; i >= 0; i--)
                stack.Push(nodes[i]);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                result.Add(node);
                for (int i = node.Nodes.Count - 1; i >= 0; i--)
                    stack.Push(node.Nodes[i]);
            }
            return result;
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
