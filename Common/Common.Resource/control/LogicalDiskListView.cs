using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace Common.Resource
{
    public class LogicalDiskListView : ListView
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogicalDiskListView()
            : base()
        {
            // 初期化
            Initialization();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // 2次バッファ設定
            this.DoubleBuffered = true;

            // 列ヘッダ初期化
            ColumnHeaderInitialization();

            // 属性初期化
            AttributeInitialization();
        }
        /// <summary>
        /// 列ヘッダ初期化
        /// </summary>
        private void ColumnHeaderInitialization()
        {
            // 列ヘッダ初期設定
            ColumnHeader[] _ColumnHeader = new ColumnHeader[]
                {
                new ColumnHeader() { Text = "ドライブ名", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "状態", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "フォーマット", Width = 72, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "種類", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "ボリュームラベル", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "合計サイズ", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "空き領域", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "空き領域(合計)", Width = 96, TextAlign = HorizontalAlignment.Right },
                };

            // 列ヘッダをリストに追加
            this.Columns.AddRange(_ColumnHeader);
        }
        /// <summary>
        /// 属性初期化
        /// </summary>
        private void AttributeInitialization()
        {
            // 初期設定
            FullRowSelect = true;                           // 全ての行を選択状態
            GridLines = true;                               // グリッド線を表示
            HeaderStyle = ColumnHeaderStyle.Nonclickable;   // ヘッダはクリック不可
            MultiSelect = false;                            // 複数行選択不可
            View = View.Details;                            // 詳細ビュー
        }
        /// <summary>
        /// リスト初期化
        /// </summary>
        public void AllClearList()
        {
            // 更新開始
            this.BeginUpdate();

            // 項目クリア
            this.Items.Clear();

            // 更新終了
            this.EndUpdate();
        }
        /// <summary>
        /// 現在のリストにあって、最新のリストにないもを削除する
        /// </summary>
        private void Delete()
        {
            // すべてのドライブを取得する
            DriveInfo[] _DriveInfoList = DriveInfo.GetDrives();

            // 現在のリストにあって、最新のリストにないものは削除
            foreach (ListViewItem _ListViewItem in this.Items)
            {
                // すべてのドライブを分繰り返し
                foreach (DriveInfo _DriveInfo in _DriveInfoList)
                {
                    if (_ListViewItem.Text == _DriveInfo.Name)
                    {
                        // 互いのリストに存在するので削除しない
                        return;
                    }
                }

                // 最新リストにないので削除
                this.Items.Remove(_ListViewItem);
            }
        }
        /// <summary>
        /// リスト更新
        /// </summary>
        public void UpdateList()
        {
            // 更新開始
            this.BeginUpdate();

            // リスト削除
            Delete();

            // すべてのドライブを取得する
            DriveInfo[] _DriveInfoList = DriveInfo.GetDrives();

            // すべてのドライブ分繰り返し
            foreach (DriveInfo _DriveInfo in _DriveInfoList)
            {
                // 登録がなければリストに追加
                ListViewItem _ListViewItem = this.FindItemWithText(_DriveInfo.Name);
                if (_ListViewItem == null)
                {
                    _ListViewItem = this.Items.Add(_DriveInfo.Name);
                    for (int i = 1; i < this.Columns.Count; i++)
                    {
                        _ListViewItem.SubItems.Add("");
                    }
                }
                else
                {
                    for (int i = 1; i < this.Columns.Count; i++)
                    {
                        _ListViewItem.SubItems[i].Text = "";
                    }
                }

                // ドライブの準備はOK？
                if (_DriveInfo.IsReady)
                {
                    // アクセスできる場合
                    _ListViewItem.SubItems[1].Text = _DriveInfo.IsReady.ToString();
                    _ListViewItem.SubItems[2].Text = _DriveInfo.DriveFormat;
                    _ListViewItem.SubItems[3].Text = _DriveInfo.DriveType.ToString();
                    _ListViewItem.SubItems[4].Text = _DriveInfo.VolumeLabel;
                    _ListViewItem.SubItems[5].Text = _DriveInfo.TotalSize.ToString("#,0");
                    _ListViewItem.SubItems[6].Text = _DriveInfo.AvailableFreeSpace.ToString("#,0");
                    _ListViewItem.SubItems[7].Text = _DriveInfo.TotalFreeSpace.ToString("#,0");
                }
                else
                {
                    // アクセスできない場合
                    _ListViewItem.SubItems[1].Text = _DriveInfo.IsReady.ToString();
                }
            }

            // 更新終了
            this.EndUpdate();
        }
    }
}