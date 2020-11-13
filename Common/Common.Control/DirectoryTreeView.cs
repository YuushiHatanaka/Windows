using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Common.Control
{
    /// <summary>
    /// ディレクトリTreeView
    /// </summary>
    public class DirectoryTreeView : TreeView
    {
        /*
        /// <summary>
        /// ルートノード
        /// </summary>
        private DirectoryTreeNode m_Root = null;

        /// <summary>
        /// ルートノード
        /// </summary>
        public DirectoryTreeNode Root {  get { return this.m_Root; } }
        */
        /// <summary>
        /// Updateイベントハンドラ
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Selectイベントハンドラ
        /// </summary>
        public event EventHandler Selected;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DirectoryTreeView()
            : base()
        {
            // 初期化
            this.Initialize();

            // ドライブ一覧を走査してツリーに追加
            string[] diriveList = Environment.GetLogicalDrives();
            foreach (string drive in diriveList)
            {
                // 更新
                this.Update(drive);
            }
        }

        /// <summary>
        /// SetSelected
        /// </summary>
        public void SetSelected()
        {
            Trace.WriteLine("DirectoryTreeView::SetSelected()");

            string[] diriveList = Environment.GetLogicalDrives();
            this.SetSelected(diriveList[0]);
        }

        /// <summary>
        /// SetSelected
        /// </summary>
        /// <param name="path"></param>
        public void SetSelected(string path)
        {
            Trace.WriteLine("DirectoryTreeView::SetSelected(string)");
            Debug.WriteLine("path：" + path);


            // ノード検索
            DirectoryTreeNode findNode = this.FindNode(path);

            if (findNode == null)
            {
                return;
            }
            this.SelectedNode = findNode;

            // イベント情報生成
            DirectoryTreeViewSelectedEventArgs _args = new DirectoryTreeViewSelectedEventArgs();
            _args.Info = findNode.Info;

            // 更新イベント
            this.OnSelected(_args);
        }

        private DirectoryTreeNode FindNode(string path)
        {
            Trace.WriteLine("DirectoryTreeView::FindNode(string)");
            Debug.WriteLine("path：" + path);

            for (int i = 0; i < this.Nodes.Count; i++)
            {
                if (this.Nodes[i].FullPath == path)
                {
                    return (DirectoryTreeNode)this.Nodes[i];
                }
            }
            return null;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="drive"></param>
        public DirectoryTreeView(string drive)
            : base()
        {
            // 初期化
            this.Initialize();

            // 更新
            this.Update(drive);

            // 指定されたドライブを選択状態にする
            this.SetSelected(drive);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            // 各設定
            this.ShowNodeToolTips = true;
            this.BeforeExpand += DirectoryTreeView_BeforeExpand;
            this.NodeMouseClick += DirectoryTreeView_NodeMouseClick;
        }

        /// <summary>
        /// BeforeExpand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectoryTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Trace.WriteLine("DirectoryTreeView::DirectoryTreeView_BeforeExpand(object, TreeViewCancelEventArgs)");
            DirectoryTreeNode expandNode = (DirectoryTreeNode)e.Node;
            Debug.WriteLine("Expand Node：" + expandNode.FullPath);
            string path = expandNode.FullPath;
            expandNode.Nodes.Clear();
            try
            {
                DirectoryInfo dirList = new DirectoryInfo(path);
                foreach (DirectoryInfo di in dirList.GetDirectories())
                {
                    DirectoryTreeNode child = new DirectoryTreeNode(di.FullName);
                    child.Nodes.Add(new DirectoryTreeNode());
                    expandNode.Nodes.Add(child);
                }
            }
            catch (IOException ex)
            {
                // TODO:例外
                Debug.WriteLine(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                // TODO:例外
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // TODO:例外
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// NodeMouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectoryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Trace.WriteLine("DirectoryTreeView::DirectoryTreeView_NodeMouseClick(object, TreeNodeMouseClickEventArgs)");
            DirectoryTreeNode mouseClick = (DirectoryTreeNode)e.Node;
            Debug.WriteLine("Mouse Click Node：" + mouseClick.FullPath);

            // イベント情報生成
            DirectoryTreeViewSelectedEventArgs _args = new DirectoryTreeViewSelectedEventArgs();
            _args.Info = mouseClick.Info;

            // 更新イベント
            this.OnSelected(_args);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="drive"></param>
        private void Update(string drive)
        {
            // 更新開始
            this.BeginUpdate();

            // 追加(ルートノード)
            DirectoryTreeNode node = new DirectoryTreeNode(drive);
            node.Nodes.Add(new DirectoryTreeNode());
            this.Nodes.Add(node);

            // 更新終了
            this.EndUpdate();

            // イベント情報生成
            DirectoryTreeViewUpdatedEventArgs _args = new DirectoryTreeViewUpdatedEventArgs();
            _args.Info = node.Info;

            // 更新イベント
            this.OnUpdated(_args);
        }

        /// <summary>
        /// 更新イベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdated(DirectoryTreeViewUpdatedEventArgs e)
        {
            // イベントハンドラ呼出し
            if (this.Updated != null)
            {
                // 呼出し
                this.Updated(this, e);
            }
        }

        /// <summary>
        /// 選択イベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelected(DirectoryTreeViewSelectedEventArgs e)
        {
            // イベントハンドラ呼出し
            if (this.Selected != null)
            {
                // 呼出し
                this.Selected(this, e);
            }
        }
    }

    /// <summary>
    /// イベント引数型
    /// </summary>
    public class DirectoryTreeViewUpdatedEventArgs : EventArgs
    {
        public DirectoryInfo Info = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DirectoryTreeViewUpdatedEventArgs()
            : base()
        {

        }
    }

    /// <summary>
    /// イベント引数型
    /// </summary>
    public class DirectoryTreeViewSelectedEventArgs : EventArgs
    {
        public DirectoryInfo Info = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DirectoryTreeViewSelectedEventArgs()
            : base()
        {

        }
    }

    /// <summary>
    /// ディレクトリTreeNode
    /// </summary>
    public class DirectoryTreeNode : TreeNode
    {
        /// <summary>
        /// ディレクトリ情報
        /// </summary>
        private DirectoryInfo m_Info = null;

        /// <summary>
        /// ディレクトリ情報
        /// </summary>
        public DirectoryInfo Info { get { return this.m_Info; } }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DirectoryTreeNode()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        public DirectoryTreeNode(string path)
            : base()
        {
            // ディレクトリ情報生成
            this.m_Info = new DirectoryInfo(path);
            this.Text = this.m_Info.Name;
            this.ToolTipText = this.m_Info.FullName;
        }
        #endregion
    }
}
