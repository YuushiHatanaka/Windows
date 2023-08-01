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
    public class MemoryListView : ListView
    {
        private ListViewItem m_CurrentListViewItem = null;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MemoryListView()
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

            // 事前に1件リストを登録しておく
            m_CurrentListViewItem = this.Items.Add("");
            for (int i = 1; i < this.Columns.Count; i++)
            {
                m_CurrentListViewItem.SubItems.Add("");
            }
        }
        /// <summary>
        /// 列ヘッダ初期化
        /// </summary>
        private void ColumnHeaderInitialization()
        {
            // 列ヘッダ初期設定
            ColumnHeader[] _ColumnHeader = new ColumnHeader[]
                {
                new ColumnHeader() { Text = "ID", Width = 8, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "合計物理メモリ(KB)", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "利用可能な物理メモリ(KB)", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "合計仮想メモリ(KB)", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "利用可能仮想メモリ(KB)", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "他のページをスワップアウトせずにページングファイルにマップできるサイズ(KB)", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "ページングファイルに保存できる合計サイズ(KB)", Width = 96, TextAlign = HorizontalAlignment.Right },
                new ColumnHeader() { Text = "スワップスペースの合計サイズ(KB)", Width = 96, TextAlign = HorizontalAlignment.Right },
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

            ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                m_CurrentListViewItem.SubItems[1].Text = float.Parse(mo["TotalVisibleMemorySize"].ToString()).ToString("#,0");
                m_CurrentListViewItem.SubItems[2].Text = float.Parse(mo["FreePhysicalMemory"].ToString()).ToString("#,0");
                m_CurrentListViewItem.SubItems[3].Text = float.Parse(mo["TotalVirtualMemorySize"].ToString()).ToString("#,0");
                m_CurrentListViewItem.SubItems[4].Text = float.Parse(mo["FreeVirtualMemory"].ToString()).ToString("#,0");
                m_CurrentListViewItem.SubItems[5].Text = float.Parse(mo["FreeSpaceInPagingFiles"].ToString()).ToString("#,0");
                m_CurrentListViewItem.SubItems[6].Text = float.Parse(mo["SizeStoredInPagingFiles"].ToString()).ToString("#,0");
                if (mo["TotalSwapSpaceSize"] != null)
                {
                    m_CurrentListViewItem.SubItems[7].Text = float.Parse(mo["TotalSwapSpaceSize"].ToString()).ToString("#,0");
                }
                else
                {
                    m_CurrentListViewItem.SubItems[7].Text = "-";
                }

                Debug.WriteLine("----------------------------------------");
                Debug.WriteLine("メモリ情報");
                Debug.WriteLine("----------------------------------------");
                //合計物理メモリ
                Debug.WriteLine("合計物理メモリ:{0:#,0}KB", mo["TotalVisibleMemorySize"]);
                //利用可能な物理メモリ
                Debug.WriteLine("利用可能物理メモリ:{0:#,0}KB", mo["FreePhysicalMemory"]);
                //合計仮想メモリ
                Debug.WriteLine("合計仮想メモリ:{0:#,0}KB", mo["TotalVirtualMemorySize"]);
                //利用可能な仮想メモリ
                Debug.WriteLine("利用可能仮想メモリ:{0:#,0}KB", mo["FreeVirtualMemory"]);

                //他のページをスワップアウトせずにページングファイルにマップできるサイズ
                Debug.WriteLine("FreeSpaceInPagingFiles:{0:#,0}KB", mo["FreeSpaceInPagingFiles"]);
                //ページングファイルに保存できる合計サイズ
                Debug.WriteLine("SizeStoredInPagingFiles:{0:#,0}KB", mo["SizeStoredInPagingFiles"]);
                //スワップスペースの合計サイズ
                //スワップスペースとページングファイルが区別されていなければ、NULL
                Debug.WriteLine("TotalSwapSpaceSize:{0:#,0}KB", mo["TotalSwapSpaceSize"]);
                Debug.WriteLine("");
                mo.Dispose();
            }
            moc.Dispose();
            mc.Dispose();

            // 更新終了
            this.EndUpdate();
        }
    }
}