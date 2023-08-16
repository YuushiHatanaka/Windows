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
    /// ファイルListView
    /// </summary>
    public class FileListView : ListView
    {
        /// <summary>
        /// パス
        /// </summary>
        private string m_Path = string.Empty;

        /// <summary>
        /// パス
        /// </summary>
        public string Path { get { return this.m_Path; } }

        /// <summary>
        /// マスク
        /// </summary>
        private string m_Mask = "*";

        /// <summary>
        /// マスク
        /// </summary>
        public string Mask { get { return this.m_Mask; } }

        /// <summary>
        /// 仮想Item
        /// </summary>
        private FileListViewItem[] m_Items = null;

        /// <summary>
        /// イベントハンドラ
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileListView()
            : base()
        {
            Trace.WriteLine("FileListView::FileListView()");

            // 初期化
            this.Initialize();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        public FileListView(string path)
            : base()
        {
            Trace.WriteLine("FileListView::FileListView(string)");
            Debug.WriteLine("path：" + path);

            // 初期化
            this.Initialize();

            // 更新
            this.Update(path, "*");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mask"></param>
        public FileListView(string path, string mask)
            : base()
        {
            Trace.WriteLine("FileListView::FileListView(string, string)");
            Debug.WriteLine("path：" + path);
            Debug.WriteLine("mask：" + mask);

            // 初期化
            this.Initialize();

            // 更新
            this.Update(path, mask);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            // 各設定
            this.FullRowSelect = true;
            this.GridLines = true;
            this.Sorting = SortOrder.None;
            this.View = View.Details;
            this.ShowItemToolTips = true;
            this.VirtualMode = true;
            this.VirtualListSize = 0;
            this.RetrieveVirtualItem += retrieveVirtualItem;

            // カラムヘッダ設定
            ColumnHeader[] colHeaderRegValue =
            {
                new ColumnHeader(){Text="名前"     ,Width=160},
                new ColumnHeader(){Text="更新日時" ,Width=128, TextAlign=HorizontalAlignment.Right},
                new ColumnHeader(){Text="種類"     ,Width=128, TextAlign=HorizontalAlignment.Left},
                new ColumnHeader(){Text="サイズ"   ,Width= 96, TextAlign=HorizontalAlignment.Right},
            };

            // カラムヘッダ追加
            this.Columns.AddRange(colHeaderRegValue);
        }

        /// <summary>
        /// 仮想モードアイテム取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void retrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            // アイテム表示
            if (this.m_Items != null)
            {
                // 表示
                e.Item = this.m_Items[e.ItemIndex];
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="path"></param>
        public void Update(string path)
        {
            Trace.WriteLine("FileListView::Update(string)");
            Debug.WriteLine("path：" + path);

            // 更新
            this.Update(path, this.m_Mask);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mask"></param>
        public void Update(string path, string mask)
        {
            Trace.WriteLine("FileListView::Update(string, string)");
            Debug.WriteLine("path：" + path);
            Debug.WriteLine("mask：" + mask);

            // 設定
            this.m_Path = path;
            this.m_Mask = mask;

            // イベント情報生成
            FileListViewUpdatedEventArgs _args = new FileListViewUpdatedEventArgs();
            _args.Path = this.m_Path;
            _args.Mask = this.m_Mask;

            // 更新開始
            this.BeginUpdate();

            // クリア
            this.Items.Clear();

            // ディレクトリ存在判定
            if (Directory.Exists(path))
            {
                // TODO:追加(カレントディレクトリ)

                // TODO:追加(親ディレクトリ)

                // 追加用リスト
                List<FileListViewItem> list = new List<FileListViewItem>();

                try
                {
                    // 配下のディレクトリを取得
                    foreach (string directory in Directory.GetDirectories(this.m_Path))
                    {
                        // ファイルListViewItemオブジェクト生成
                        FileListViewItem _item = new FileListViewItem(directory);

                        // 追加
                        list.Add(_item);
                    }

                    // 配下のファイルを取得
                    foreach (string file in Directory.EnumerateFiles(this.m_Path, this.m_Mask, SearchOption.TopDirectoryOnly))
                    {
                        // ファイルListViewItemオブジェクト生成
                        FileListViewItem _item = new FileListViewItem(file);

                        // 追加
                        list.Add(_item);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                // 仮想モード設定
                this.VirtualListSize = list.Count;
                this.m_Items = list.ToArray();
            }
            else
            {
                // TODO:ディレクトリ存在なし
            }

            // 更新イベント
            this.OnUpdated(_args);

            // 更新終了
            this.EndUpdate();
        }

        /// <summary>
        /// 更新イベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdated(FileListViewUpdatedEventArgs e)
        {
            // イベントハンドラ呼出し
            if (this.Updated != null)
            {
                // 呼出し
                this.Updated(this, e);
            }
        }

        /// <summary>
        /// DirectoryTreeView選択時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DirectoryTreeView_Selected(object sender, EventArgs e)
        {
            DirectoryTreeViewSelectedEventArgs args = (DirectoryTreeViewSelectedEventArgs)e;
            this.Update(args.Info.FullName);
        }
    }

    /// <summary>
    /// イベント引数型
    /// </summary>
    public class FileListViewUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// パス
        /// </summary>
        public string Path = string.Empty;

        /// <summary>
        /// マスク
        /// </summary>
        public string Mask = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileListViewUpdatedEventArgs()
            : base()
        {

        }
    }
}
