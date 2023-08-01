using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.Management;

namespace Common.Control
{
    public class CpuListView : ListView
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CpuListView()
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
                new ColumnHeader() { Text = "DeviceID", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "Name", Width = 256, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "MaxClockSpeed", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "L2CacheSize", Width = 96, TextAlign = HorizontalAlignment.Right },
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
        /// リスト更新
        /// </summary>
        public void UpdateList()
        {
            // 更新開始
            this.BeginUpdate();

            // すべての物理CPUを取得する
            ManagementClass mc = new ManagementClass("Win32_Processor");
            // CPU一覧を取得
            ManagementObjectCollection moc = mc.GetInstances();

            // CPU情報を一つずつ取り出す
            foreach (ManagementObject mo in moc)
            {
                // 登録がなければリストに追加
                ListViewItem _ListViewItem = this.FindItemWithText(mo["DeviceID"].ToString());
                if (_ListViewItem == null)
                {
                    _ListViewItem = this.Items.Add(mo["DeviceID"].ToString());
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

                _ListViewItem.SubItems[1].Text = mo["Name"].ToString();
                _ListViewItem.SubItems[2].Text = mo["MaxClockSpeed"].ToString();
                _ListViewItem.SubItems[3].Text = mo["L2CacheSize"].ToString();
            }

            // 更新終了
            this.EndUpdate();
        }
    }
}