using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

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

        private StationProperties Property { get; set; } = new StationProperties();

        public DataGridViewStation()
        {
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
            Columns.Add("Number"           , "番号");
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
        }

        private void StationCutoutOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationCutoutOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationCutoutOnClick(object, EventArgs)");
        }

        private void StationCopyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationCopyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationCopyOnClick(object, EventArgs)");
        }

        private void StationPastingOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationPastingOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationPastingOnClick(object, EventArgs)");
        }

        private void StationDeleteOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationDeleteOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationDeleteOnClick(object, EventArgs)");
        }

        private void StationInsertOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewStation::StationInsertOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationInsertOnClick(object, EventArgs)");
        }

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
                    OnUpdate(this, new StationPropertiesUpdateEventArgs() { Property = this.Property });
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::StationPropertyOnClick(object, EventArgs)");
        }

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

            // ロギング
            Logger.Debug("<<<<= DataGridViewStation::ContextMenuStripOpened(object, EventArgs)");
        }

        private StationProperty GetSelectedCondition()
        {
            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // 選択なし
                return null;
            }

            // 選択オブジェクト
            StationProperty result = Property[SelectedCells[0].RowIndex];

            // 返却
            return result;
        }

        public DataGridViewStation(StationProperties properties)
            : this()
        {
            Update(properties);
        }

        private void DataGridViewStation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
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

            FormStationProperty form = new FormStationProperty(Property[e.RowIndex]);
            form.OnUpdate += DataGridViewStation_OnUpdate;

            DialogResult dialogResult = form.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                // イベント呼出
                OnUpdate(this, new StationPropertiesUpdateEventArgs() { Property = this.Property });
            }
        }

        private void DataGridViewStation_OnUpdate(object sender, StationPropertyUpdateEventArgs e)
        {
            Update(e.Property);
        }

        private void Update(StationProperty property)
        {
            Property[property.Seq - 1].Copy(property);
            Update(Property);
        }

        private void Update(StationProperties properties)
        {
            Rows.Clear();

            foreach (var property in properties)
            {
                List<string> values = new List<string>
                {
                    property.Seq.ToString(),
                    property.Name,
                    property.TimeFormat.GetStringValue(),
                    property.StationScale.GetStringValue()
                };

                // 行追加
                Rows.Add(values.ToArray());
            }

            // ソート禁止
            foreach (DataGridViewColumn column in Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            Property.Copy(properties);
        }
    }
}
