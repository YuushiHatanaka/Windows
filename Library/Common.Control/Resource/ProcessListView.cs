using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.ComponentModel;

namespace Common.Control
{
    public class ProcessListView : ListView
    {
        /// <summary>
        /// 監視対象プロセスリスト
        /// </summary>
        private ArrayList m_MonitoredList = new ArrayList();
        /// <summary>
        /// 監視対象プロセスリスト
        /// </summary>
        public ArrayList MonitoredList
        {
            get { return m_MonitoredList; }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessListView()
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
                new ColumnHeader() { Text = "プロセス名", Width = 144, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "ID", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "起動時間", Width = 144, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "CPU時間", Width = 96, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "優先度", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "スレッド数", Width = 64, TextAlign = HorizontalAlignment.Left },
                new ColumnHeader() { Text = "物理メモリ量(byte)", Width = 128, TextAlign = HorizontalAlignment.Left },
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
        /// 項目削除
        /// </summary>
        /// <param name="pProcessName">削除対象プロセス名</param>
        public void RemoveItem(String pProcessName)
        {
            // リストビューにプロセス名がなくなるまで繰り返す
            while (true)
            {
                // リストビュー上にプロセス(プロセス名で検索)の設定があるか判定
                ListViewItem _ListViewItem = this.FindItemWithText(pProcessName);
                // 結果判定
                if (_ListViewItem == null)
                {
                    break;
                }
                // 対象項目削除
                this.Items.Remove(_ListViewItem);
            }
        }
        /// <summary>
        /// 項目削除
        /// </summary>
        /// <param name="pProcessName">削除対象プロセス名</param>
        /// <param name="_ProcessList">プロセスリスト</param>
        public void RemoveItem(String pProcessName, Process[] pProcessList)
        {
            // リスト分繰り返す
            foreach (ListViewItem _ListViewItem in Items)
            {
                // 削除対象プロセス名か判定
                if (_ListViewItem.Text != pProcessName)
                {
                    // 削除対象プロセス名ではない
                    continue;
                }
                // 削除対象プロセスIDか判定
                bool _lookUp = false;
                foreach (Process _Process in pProcessList)
                {
                    // リストビューのプロセスIDと起動中プロセスIDを比較
                    if (_ListViewItem.SubItems[1].Text == _Process.Id.ToString())
                    {
                        // 起動中のプロセスIDと一致したら削除しない
                        _lookUp = true;
                        break;
                    }
                }
                // プロセスIDが見つからなかった(該当プロセスIDは終了済み)
                if (!_lookUp)
                {
                    // 項目削除
                    this.Items.Remove(_ListViewItem);
                }
            }
        }
        /// <summary>
        /// リスト更新
        /// </summary>
        public void UpdateList()
        {
            // 更新開始
            this.BeginUpdate();

            // 現在のリストを分繰り返す
            foreach (ListViewItem _ListViewItem in Items)
            {
                // 最新の監視プロセスリストにあるか判定
                if (m_MonitoredList.IndexOf(_ListViewItem.Text) < 0)
                {
                    // 最新の監視プロセスリストにない→削除
                    RemoveItem(_ListViewItem.Text);
                }
            }

            // 監視対象プロセスリスト分を繰り返す
            foreach (String _ProcessName in m_MonitoredList)
            {
                Debug.WriteLine("----------------------------------------");
                Debug.WriteLine("監視対象プロセス名：" + _ProcessName);
                Debug.WriteLine("----------------------------------------");

                // 監視対象プロセスのプロセスオブジェクトを取得する
                Process[] _ProcessList = Process.GetProcessesByName(_ProcessName);
                Debug.WriteLine("起動プロセス数　　：{0}", _ProcessList.Length);

                // 監視対象プロセスのプロセス数を判定
                if (!(_ProcessList.Length > 0))
                {
                    // 起動中のプロセスが1つもない場合は、リスト削除して次の監視対象プロセス処理に移る
                    RemoveItem(_ProcessName);
                    continue;
                }

                // プロセスリストに存在しない項目を削除
                RemoveItem(_ProcessName, _ProcessList);

                // 起動中プロセス分繰り返し
                foreach (Process _Process in _ProcessList)
                {
                    // リストビュー上にプロセス(プロセスIDで検索)の設定があるか判定
                    ListViewItem _ListViewItem = null;
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        ListViewItem _SearchListViewItem = Items[i];
                        if (_SearchListViewItem.SubItems[1].Text == _Process.Id.ToString())
                        {
                            _ListViewItem = _SearchListViewItem;
                            break;
                        }
                    }

                    try
                    {
                        // 結果判定
                        if (_ListViewItem == null)
                        {
                            // リストビュー上にプロセスの設定なし→リスト追加
                            Debug.WriteLine("リストビュー上にプロセスの設定なし→リスト追加");
                            ListViewItem _AddListViewItem = this.Items.Add(_Process.ProcessName);
                            _AddListViewItem.SubItems.Add("");
                            _AddListViewItem.SubItems.Add("");
                            _AddListViewItem.SubItems.Add("");
                            _AddListViewItem.SubItems.Add("");
                            _AddListViewItem.SubItems.Add("");
                            _AddListViewItem.SubItems.Add("");
                            Debug.WriteLine(" _AddListViewItem.SubItems.Count:" + _AddListViewItem.SubItems.Count.ToString());
                            _AddListViewItem.SubItems[1].Text = _Process.Id.ToString();
                            _AddListViewItem.SubItems[2].Text = _Process.StartTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
                            _AddListViewItem.SubItems[3].Text = _Process.TotalProcessorTime.ToString(@"hh\:mm\:ss\.fff");
                            _AddListViewItem.SubItems[4].Text = _Process.PriorityClass.ToString();
                            _AddListViewItem.SubItems[5].Text = _Process.Threads.Count.ToString();
                            _AddListViewItem.SubItems[6].Text = _Process.WorkingSet64.ToString("#,0");
                        }
                        else
                        {
                            // リストビュー上にプロセスの設定あり→リスト更新
                            Debug.WriteLine("リストビュー上にプロセスの設定あり→リスト更新");
                            _ListViewItem.SubItems[1].Text = _Process.Id.ToString();
                            _ListViewItem.SubItems[2].Text = _Process.StartTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
                            _ListViewItem.SubItems[3].Text = _Process.TotalProcessorTime.ToString(@"hh\:mm\:ss\.fff");
                            try
                            {
                                _ListViewItem.SubItems[4].Text = _Process.PriorityClass.ToString();
                            }
                            catch (Win32Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                                _ListViewItem.SubItems[4].Text = "";
                            }
                            _ListViewItem.SubItems[5].Text = _Process.Threads.Count.ToString();
                            _ListViewItem.SubItems[6].Text = _Process.WorkingSet64.ToString("#,0");
                            Debug.WriteLine(" _ListViewItem.SubItems.Count:" + _ListViewItem.SubItems.Count.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine(ex.StackTrace);
                    }
                }
            }

            // 更新終了
            this.EndUpdate();
        }
        /// <summary>
        /// リスト更新
        /// </summary>
        /// <param name="m_MonitoredList">監視対象プロセスリスト</param>
        public void UpdateList(ArrayList pMonitoredList)
        {
            // 現在の監視対象プロセスリストをクリア
            m_MonitoredList.Clear();

            // 新しい監視対象プロセスリストをコピー
            m_MonitoredList.AddRange(pMonitoredList);

            // リスト更新
            UpdateList();

            // 更新終了
            this.EndUpdate();
        }
        /// <summary>
        /// リスト更新
        /// </summary>
        /// <param name="pStreamReader">監視対象プロセスリストファイルストリーム</param>
        public void UpdateList(StreamReader pStreamReader)
        {
            // 監視対象プロセスリスト
            ArrayList _MonitoredList = new ArrayList();

            // 読み込みできる文字がなくなるまで繰り返す
            while (pStreamReader.Peek() >= 0)
            {
                // ファイルを 1 行ずつ読み込む
                String _ReadLine = pStreamReader.ReadLine();

                // 監視対象プロセスリストに追加
                _MonitoredList.Add(_ReadLine);
            }

            // リスト更新
            UpdateList(_MonitoredList);
        }
        /// <summary>
        /// リスト更新
        /// </summary>
        /// <param name="pFiileName">監視対象プロセスリストファイル名</param>
        public void UpdateList(String pFiileName)
        {
            // ファイル存在判定
            if (File.Exists(pFiileName))
            {
                // StreamReader の新しいインスタンスを生成する
                StreamReader _StreamReader = new StreamReader(pFiileName);

                // リスト更新
                UpdateList(_StreamReader);

                // オブジェクトの破棄
                _StreamReader.Close();
            }
        }
    }
}