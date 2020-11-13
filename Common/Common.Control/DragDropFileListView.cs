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
    /// ドラッグアンドドロップFileListViewクラス
    /// </summary>
    public class DragDropFileListView : ListView
    {
        /// <summary>
        /// 仮想Item
        /// </summary>
        private List<DragDropFileListViewItem> m_Items = new List<DragDropFileListViewItem>();
        public DragDropFileListViewItem GetItem(int i)
        {
            return m_Items[i];
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DragDropFileListView()
            : base()
        {
            // 初期化
            this.Initialize();
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
            this.AllowDrop = true;
            this.MultiSelect = true;
            this.VirtualMode = true;
            this.VirtualListSize = 0;
            this.RetrieveVirtualItem += retrieveVirtualItem;
            this.ItemDrag += new ItemDragEventHandler(ListViewFile_ItemDrag);
            this.DragEnter += new DragEventHandler(ListViewFile_DragEnter);
            this.DragOver += new DragEventHandler(ListViewFile_DragOver);
            this.DragDrop += new DragEventHandler(ListViewFile_DragDrop);

            // カラムヘッダ設定
            ColumnHeader[] colHeaderRegValue =
            {
                new ColumnHeader(){Text="名前"         ,Width=192},
                new ColumnHeader(){Text="種類"         ,Width=128, TextAlign=HorizontalAlignment.Left},
                new ColumnHeader(){Text="サイズ"       ,Width= 96, TextAlign=HorizontalAlignment.Right},
                new ColumnHeader(){Text="作成日時"     ,Width=128, TextAlign=HorizontalAlignment.Right},
                new ColumnHeader(){Text="更新日時"     ,Width=128, TextAlign=HorizontalAlignment.Right},
                new ColumnHeader(){Text="アクセス日時" ,Width=128, TextAlign=HorizontalAlignment.Right},
                new ColumnHeader(){Text="変更内容"     ,Width=256},
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
            if (this.m_Items.Count > 0)
            {
                // 表示
                e.Item = this.m_Items[e.ItemIndex];
            }
            else
            {
                // 表示
                e.Item = new DragDropFileListViewItem();
            }
        }

        /// <summary>
        /// 要素クリア
        /// </summary>
        public void ItemsClear()
        {
            // 更新開始
            this.BeginUpdate();

            // クリア
            this.Items.Clear();

            // 仮想モード設定
            this.m_Items.Clear();
            this.VirtualListSize = this.m_Items.Count;

            // 更新終了
            this.EndUpdate();
        }

        /// <summary>
        /// ItemDrag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewFile_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Trace.WriteLine("DragDropFileListView::ListViewFile_ItemDrag(object, ItemDragEventArgs)");
        }

        /// <summary>
        /// DragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewFile_DragEnter(object sender, DragEventArgs e)
        {
            Trace.WriteLine("DragDropFileListView::ListViewFile_DragEnter(object, DragEventArgs)");

            e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// DragOver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewFile_DragOver(object sender, DragEventArgs e)
        {
            Trace.WriteLine("DragDropFileListView::ListViewFile_DragOver(object, DragEventArgs)");

        }

        /// <summary>
        /// DragDrop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewFile_DragDrop(object sender, DragEventArgs e)
        {
            Trace.WriteLine("DragDropFileListView::ListViewFile_DragDrop(object, DragEventArgs)");

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // 更新開始
                this.BeginUpdate();

                foreach (string fileName in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    // ファイル属性取得
                    FileAttributes fileAttributes = File.GetAttributes(fileName);

                    // ディレクトリの場合
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        // ファイルListViewItemオブジェクト生成
                        DragDropFileListViewItem _item = new DragDropFileListViewItem(fileName);

                        // 追加
                        this.m_Items.Add(_item);
                    }
                    else
                    {
                        // ファイルListViewItemオブジェクト生成
                        DragDropFileListViewItem _item = new DragDropFileListViewItem(fileName);

                        // 追加
                        this.m_Items.Add(_item);
                    }
                }

                // 仮想モード設定
                this.VirtualListSize = this.m_Items.Count;

                // 更新終了
                this.EndUpdate();
            }
        }
    }
}