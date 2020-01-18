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

namespace Common.Control
{
    /// <summary>
    /// ディレクトリTreeView
    /// </summary>
    public class DirectoryTreeView : TreeView
    {
        /// <summary>
        /// ルートノード
        /// </summary>
        private DirectoryTreeNode m_Root = null;

        /// <summary>
        /// ルートノード
        /// </summary>
        public DirectoryTreeNode Root {  get { return this.m_Root; } }

        /// <summary>
        /// イベントハンドラ
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DirectoryTreeView()
            : base()
        {
            // 初期化
            this.Initialize();

            // 更新
            this.Update(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        public DirectoryTreeView(string path)
            : base()
        {
            // 初期化
            this.Initialize();

            // 更新
            this.Update(path);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            // 各設定
            this.ShowNodeToolTips = true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="path"></param>
        public void Update(string path)
        {
            // 設定
            if (!Directory.Exists(path))
            {
                // 例外
                throw new DirectoryNotFoundException("ディレクトリが存在しません：[" + path + "]");
            }

            // 更新開始
            this.BeginUpdate();

            // クリア
            this.Nodes.Clear();

            // 追加(ルートノード)
            this.m_Root = new DirectoryTreeNode(path);
            this.Nodes.Add(this.m_Root);

            // 更新
            this.Update(this.m_Root);

            // イベント情報生成
            DirectoryTreeViewUpdatedEventArgs _args = new DirectoryTreeViewUpdatedEventArgs();
            _args.Path = this.m_Root.Info.FullName;

            // 更新イベント
            this.OnUpdated(_args);

            // 更新終了
            this.EndUpdate();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parentNode"></param>
        private void Update(DirectoryTreeNode parentNode)
        {
            // 配下のディレクトリを取得
            foreach (string directory in Directory.GetDirectories(parentNode.Info.FullName))
            {
                // 追加
                DirectoryTreeNode _nextNode = new DirectoryTreeNode(directory);
                parentNode.Nodes.Add(_nextNode);

                // 更新
                this.Update(_nextNode);
            }
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
    }

    /// <summary>
    /// イベント引数型
    /// </summary>
    public class DirectoryTreeViewUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// パス
        /// </summary>
        public string Path = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DirectoryTreeViewUpdatedEventArgs()
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
    }
}
