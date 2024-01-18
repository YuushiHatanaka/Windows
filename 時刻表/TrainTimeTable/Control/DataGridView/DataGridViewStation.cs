using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;
using System.Runtime.Serialization;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// DataGridViewStationクラス
    /// </summary>
    public class DataGridViewStation : DataGridView
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region 更新 Event
        /// <summary>
        /// 更新 event delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void UpdateEventHandler(object sender, StationPropertiesUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty { get; set; } = null;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataGridViewStation()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::DataGridViewStation()");

            ReadOnly = true;                      // 読取専用
            AllowUserToDeleteRows = false;        // 行削除禁止
            AllowUserToAddRows = false;           // 行挿入禁止
            AllowUserToResizeRows = false;        // 行の高さ変更禁止
            RowHeadersVisible = false;            // 行ヘッダーを非表示にする
            MultiSelect = false;                  // セル、行、列が複数選択禁止
            //ヘッダーの色等
            //EnableHeadersVisualStyles = false;     // Visualスタイルを使用しない
            ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            ColumnHeadersDefaultCellStyle.Font = new Font("MS UI Gothic", 9);
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //ヘッダ高さ
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            ColumnHeadersHeight = 28;
            Columns.Add("StationName"      , "駅名");
            Columns.Add("StationTimeFormat", "駅時刻形式");
            Columns.Add("StationSize"      , "駅規模");
            CellDoubleClick += DataGridViewStation_CellDoubleClick;

            // コンテキストメニュー追加
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange
            (
                new ToolStripItem[]
                {
                    new ToolStripMenuItem("切り取り(&T)", null, StationCutoutOnClick,"cutout"){ ShortcutKeys = (Keys.Control|Keys.X) },
                    new ToolStripMenuItem("コピー(&C)", null, StationCopyOnClick,"copy"){ ShortcutKeys = (Keys.Control|Keys.C) },
                    new ToolStripMenuItem("貼り付け(&P)", null, StationPastingOnClick,"pasting"){ ShortcutKeys = (Keys.Control|Keys.V) },
                    new ToolStripMenuItem("消去", null, StationDeleteOnClick,"delete"){ ShortcutKeys = Keys.Delete },
                    new ToolStripMenuItem("駅を挿入(&I)", null, StationInsertOnClick,"insert"){ ShortcutKeys = (Keys.Control|Keys.Insert) },
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("駅のプロパティ(&A)", null, StationPropertyOnClick,"property"){ ShortcutKeys = (Keys.Control|Keys.Enter) },
                }
            );

            // コントロール設定
            ContextMenuStrip = contextMenuStrip;
            ContextMenuStrip.Opened += ContextMenuStripOpened;
            ContextMenuStrip.Items["pasting"].Enabled = false;

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::DataGridViewStation()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public DataGridViewStation(RouteFileProperty property)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::DataGridViewStation(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            //　設定
            m_RouteFileProperty = property;

            // 更新
            Update(m_RouteFileProperty.Stations);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::DataGridViewStation(RouteFileProperty)");
        }
        #endregion

        #region イベント
        #region ContextMenuStripイベント
        /// <summary>
        /// StationCutoutOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationCutoutOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationCutoutOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            StationProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewStation::StationCutoutOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // クリップボードにコピー
            Clipboard.SetDataObject(new StationProperty(result), true);

            // シーケンス番号削除
            m_RouteFileProperty.StationSequences.DeleteSequenceNumber(result);

            // 削除
            m_RouteFileProperty.Stations.Remove(result);

            // 更新
            Update(m_RouteFileProperty.Stations);

            // イベント呼出
            OnUpdate(this, new StationPropertiesUpdateEventArgs() { Properties = m_RouteFileProperty.Stations, Sequences = m_RouteFileProperty.StationSequences });

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationCutoutOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StationCopyOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationCopyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationCopyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            StationProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewStation::StationCopyOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // クリップボードにコピー
            Clipboard.SetDataObject(new StationProperty(result), true);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationCopyOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StationPastingOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationPastingOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationPastingOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // クリップボードからコピー
            IDataObject dataObject = Clipboard.GetDataObject();

            // クリップボードの内容判定
            if (!(dataObject != null && dataObject.GetDataPresent(typeof(StationProperty))))
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewStation::StationPastingOnClick(object, EventArgs)");

                // 対象外
                return;
            }

            // コピー項目取得
            StationProperty result = dataObject.GetData(typeof(StationProperty)) as StationProperty;

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // 駅追加
                m_RouteFileProperty.AddStation(result);
            }
            else
            {
                // 駅挿入
                m_RouteFileProperty.InsertStation(selectedIndex, result);
            }

            // 更新
            Update(m_RouteFileProperty.Stations);

            // イベント呼出
            OnUpdate(this, new StationPropertiesUpdateEventArgs() { Properties = m_RouteFileProperty.Stations, Sequences = m_RouteFileProperty.StationSequences });

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationPastingOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StationDeleteOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationDeleteOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationDeleteOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            StationProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewStation::StationDeleteOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 駅削除
            m_RouteFileProperty.RemoveStation(result);

            // 更新
            Update(m_RouteFileProperty.Stations);

            // イベント呼出
            OnUpdate(this, new StationPropertiesUpdateEventArgs() { Properties = m_RouteFileProperty.Stations, Sequences = m_RouteFileProperty.StationSequences });

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationDeleteOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StationInsertOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationInsertOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationInsertOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択インデックス判定
            int index;
            if (selectedIndex == -1)
            {
                index = m_RouteFileProperty.Stations.Count + 1;
            }
            else
            {
                index = selectedIndex;
            }

            // FormStationPropertyオブジェクト生成
            FormStationProperty form = new FormStationProperty();

            // フォーム表示
            if (form.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewStation::StationInsertOnClick(object, EventArgs)");

                // キャンセルなので何もしない
                return;
            }

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // 駅追加
                m_RouteFileProperty.AddStation(form.Property);
            }
            else
            {
                // 駅挿入
                m_RouteFileProperty.InsertStation(selectedIndex, form.Property);
            }

            // 更新
            Update(m_RouteFileProperty.Stations);

            // イベント呼出
            OnUpdate(this, new StationPropertiesUpdateEventArgs() { Properties = m_RouteFileProperty.Stations, Sequences = m_RouteFileProperty.StationSequences });

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationInsertOnClick(object, EventArgs)");
        }

        /// <summary>
        /// StationPropertyOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationPropertyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationPropertyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            StationProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result != null)
            {
                FormStationProperty form = new FormStationProperty(result);
                form.OnUpdate += DataGridViewStation_OnUpdate;

                DialogResult dialogResult = form.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    // イベント呼出
                    OnUpdate(this, new StationPropertiesUpdateEventArgs() { Properties = m_RouteFileProperty.Stations, Sequences = m_RouteFileProperty.StationSequences });
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationPropertyOnClick(object, EventArgs)");
        }
        #endregion

        #region ContextMenuStripイベント
        /// <summary>
        /// ContextMenuStripOpened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuStripOpened(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::ContextMenuStripOpened(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            StationProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result == null)
            {
                ContextMenuStrip.Items["cutout"].Enabled = false;
                ContextMenuStrip.Items["copy"].Enabled = false;
                ContextMenuStrip.Items["delete"].Enabled = false;
            }
            else
            {
                ContextMenuStrip.Items["cutout"].Enabled = true;
                ContextMenuStrip.Items["copy"].Enabled = true;
                ContextMenuStrip.Items["delete"].Enabled = true;
            }

            // クリップボードからコピー
            IDataObject dataObject = Clipboard.GetDataObject();

            // クリップボードの内容判定
            if (!(dataObject != null && dataObject.GetDataPresent(typeof(StationProperty))))
            {
                ContextMenuStrip.Items["pasting"].Enabled = false;
            }
            else
            {
                ContextMenuStrip.Items["pasting"].Enabled = true;
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::ContextMenuStripOpened(object, EventArgs)");
        }
        #endregion

        #region DataGridViewStationイベント
        /// <summary>
        /// DataGridViewStation_CellDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewStation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::DataGridViewStation_CellDoubleClick(object, DataGridViewCellEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 行ヘッダー
            if (e.RowIndex == -1)
            {
                return;
            }
            // 列ヘッダー
            if (e.ColumnIndex == -1)
            {
                return;
            }

            // 駅名セルを取得
            DataGridViewCell stationCell = this[0, e.RowIndex];

            // 駅情報を取得
            StationProperty property = m_RouteFileProperty.Stations.Find(t => t.Name == stationCell.Value.ToString());

            // 駅情報判定
            if (property != null)
            {
                // StationPropertiesUpdateEventArgsオブジェクト生成
                StationPropertiesUpdateEventArgs eventArgs = new StationPropertiesUpdateEventArgs();
                eventArgs.OldProperties.Copy(m_RouteFileProperty.Stations);
                eventArgs.Properties = m_RouteFileProperty.Stations;
                eventArgs.Sequences = m_RouteFileProperty.StationSequences;
                eventArgs.OldStationName = property.Name;

                // FormStationPropertyオブジェクト生成
                FormStationProperty form = new FormStationProperty(property);

                // FormStationProperty表示
                DialogResult dialogResult = form.ShowDialog();

                // FormStationProperty表示結果判定
                if (dialogResult == DialogResult.OK)
                {
                    // 駅名(キーが変更されたか？)
                    if (eventArgs.OldStationName != form.Property.Name)
                    {
                        // 同一名判定
                        if (m_RouteFileProperty.Stations.Find(t => t.Name == form.Property.Name) != null)
                        {
                            // エラーメッセージ
                            MessageBox.Show(string.Format("既に登録されている駅名は使用できません:[{0}]", form.Property.Name), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // イベント引数設定
                        eventArgs.NewStationName = form.Property.Name;

                        // 駅名変換
                        m_RouteFileProperty.ChangeStationName(eventArgs.OldStationName, form.Property.Name);
                    }

                    // 結果保存
                    property.Copy(form.Property);

                    // 変更されたか？
                    if (!eventArgs.OldProperties.Compare(m_RouteFileProperty.Stations))
                    {
                        Update(m_RouteFileProperty.Stations);

                        // 更新通知
                        OnUpdate(this, eventArgs);
                    }
                }
            }
            else
            {
                // ロギング
                Logger.WarnFormat("選択対象駅が存在していません：[{0}]", stationCell.Value.ToString());
                Logger.Warn(m_RouteFileProperty.Stations.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::DataGridViewStation_CellDoubleClick(object, DataGridViewCellEventArgs)");
        }

        /// <summary>
        /// DataGridViewStation_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewStation_OnUpdate(object sender, StationPropertyUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::DataGridViewStation_CellDoubleClick(object, StationPropertyUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新
            Update(e.Property);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::DataGridViewStation_CellDoubleClick(object, StationPropertyUpdateEventArgs)");
        }
        #endregion
        #endregion

        #region privateメソッド
        /// <summary>
        /// 選択インデックス取得
        /// </summary>
        /// <returns></returns>
        private int GetSelectedIndex()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::GetSelectedCondition()");

            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewStation::GetSelectedCondition()");

                // 選択なし
                return -1;
            }

            // 選択インデックス設定
            int result = SelectedCells[0].RowIndex;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewStation::GetSelectedCondition()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択情報取得
        /// </summary>
        /// <returns></returns>
        private StationProperty GetSelectedCondition()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::GetSelectedCondition()");

            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewStation::GetSelectedCondition()");

                // 選択なし
                return null;
            }

            // 選択オブジェクト
            StationProperty result = m_RouteFileProperty.Stations[SelectedCells[0].RowIndex];

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewStation::GetSelectedCondition()");

            // 返却
            return result;
        }

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        private void Update(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::Update(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 設定
            m_RouteFileProperty.Stations.Find(s => s.Name == property.Name).Copy(property);

            // 更新
            Update(m_RouteFileProperty.Stations);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::Update(StationProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="properties"></param>
        private void Update(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::Update(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 全行クリア
            Rows.Clear();

            // 駅シーケンスリスト取得(昇順)
            var stationSequences = m_RouteFileProperty.StationSequences.OrderBy(t => t.Seq);

            // 駅を繰り返す
            foreach (var stationSequence in stationSequences)
            {
                // 駅情報取得
                StationProperty station = m_RouteFileProperty.Stations.Find(t => t.Name == stationSequence.Name);

                // 追加オブジェクト生成
                List<string> values = new List<string>
                {
                    station.Name,
                    station.TimeFormat.GetStringValue(),
                    station.StationScale.GetStringValue()
                };

                // 行追加
                Rows.Add(values.ToArray());
            }

            // ソート禁止
            foreach (DataGridViewColumn column in Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // コピー
            m_RouteFileProperty.Stations.Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::Update(StationProperties)");
        }
        #endregion
        #endregion
    }
}
