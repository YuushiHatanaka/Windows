using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// DataGridViewTrainTypeクラス
    /// </summary>
    public class DataGridViewTrainType : DataGridView
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
        public delegate void UpdateEventHandler(object sender, TrainTypePropertiesUpdateEventArgs e);

        /// <summary>
        /// 更新 event
        /// </summary>
        public event UpdateEventHandler OnUpdate = delegate { };
        #endregion

        /// <summary>
        /// TrainTypePropertiesオブジェクト
        /// </summary>
        private TrainTypeProperties Property { get; set; } = new TrainTypeProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataGridViewTrainType()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::DataGridViewTrainType()");

            ReadOnly = true;                      // 読取専用
            AllowUserToDeleteRows = false;        // 行削除禁止
            AllowUserToAddRows = false;           // 行挿入禁止
            AllowUserToResizeRows = false;        // 行の高さ変更禁止
            RowHeadersVisible = false;            // 行ヘッダーを非表示にする
            MultiSelect = false;                  // セル、行、列が複数選択禁止
            GridColor = Color.DarkGray;
            BackgroundColor = Color.White;
            //ヘッダーの色等
            EnableHeadersVisualStyles = false;     // Visualスタイルを使用しない
            ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            ColumnHeadersDefaultCellStyle.Font = new Font("MS UI Gothic", 9);
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //ヘッダ高さ
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            ColumnHeadersHeight = 28;
            Columns.Add("Number", "");
            Columns.Add("TrainTypeName", "列車種別名");
            Columns.Add("Abbreviation", "略称");
            Columns.Add("LineStyle", "線スタイル");
            CellPainting += DataGridViewTrainType_CellPainting;
            CellDoubleClick += DataGridViewTrainType_CellDoubleClick;

            // コンテキストメニュー追加
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange
            (
                new ToolStripItem[]
                {
                    new ToolStripMenuItem("切り取り(&T)", null, TrainTypeCutoutOnClick,"cutout"){ ShortcutKeys = (Keys.Control|Keys.X) },
                    new ToolStripMenuItem("コピー(&C)", null, TrainTypeCopyOnClick,"copy"){ ShortcutKeys = (Keys.Control|Keys.C) },
                    new ToolStripMenuItem("貼り付け(&P)", null, TrainTypePastingOnClick,"pasting"){ ShortcutKeys = (Keys.Control|Keys.V) },
                    new ToolStripMenuItem("消去", null, TrainTypeDeleteOnClick,"delete"){ ShortcutKeys = Keys.Delete },
                    new ToolStripMenuItem("列車種別を挿入(&I)", null, TrainTypeInsertOnClick,"insert"){ ShortcutKeys = (Keys.Control|Keys.Insert) },
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("上へ", null, TrainTypeUpOnClick,"up"),
                    new ToolStripMenuItem("下へ", null, TrainTypeDownOnClick,"down"),
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("列車種別のプロパティ(&A)", null, TrainTypePropertyOnClick,"property"){ ShortcutKeys = (Keys.Control|Keys.Enter) },
                }
            );

            // コントロール設定
            ContextMenuStrip = contextMenuStrip;
            ContextMenuStrip.Opened += ContextMenuStripOpened;
            ContextMenuStrip.Items["pasting"].Enabled = false;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType()");
        }
        #endregion

        private void TrainTypeCutoutOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeCutoutOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCutoutOnClick(object, EventArgs)");
        }

        private void TrainTypeCopyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeCopyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCopyOnClick(object, EventArgs)");
        }

        private void TrainTypePastingOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypePastingOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypePastingOnClick(object, EventArgs)");
        }

        private void TrainTypeDeleteOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeDeleteOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeDeleteOnClick(object, EventArgs)");
        }

        private void TrainTypeInsertOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeInsertOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeInsertOnClick(object, EventArgs)");
        }

        private void TrainTypeUpOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeUpOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeUpOnClick(object, EventArgs)");
        }

        private void TrainTypeDownOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeDownOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeDownOnClick(object, EventArgs)");
        }

        private void TrainTypePropertyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypePropertyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainTypeProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result != null)
            {
                FormTrainTypeProperty form = new FormTrainTypeProperty(result);
                form.OnUpdate += DataGridViewTrainType_OnUpdate;

                DialogResult dialogResult = form.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    // イベント呼出
                    OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Property = this.Property });
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypePropertyOnClick(object, EventArgs)");
        }

        private void ContextMenuStripOpened(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::ContextMenuStripOpened(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainTypeProperty result = GetSelectedCondition();

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
            Logger.Debug("<<<<= DataGridViewTrainType::ContextMenuStripOpened(object, EventArgs)");
        }

        private TrainTypeProperty GetSelectedCondition()
        {
            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // 選択なし
                return null;
            }

            // 選択オブジェクト
            TrainTypeProperty result = Property[SelectedCells[0].RowIndex];

            // 返却
            return result;
        }

        public DataGridViewTrainType(TrainTypeProperties properties)
            : this()
        {
            Update(properties);
        }

        private void DataGridViewTrainType_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewTrainType dataGridViewTrainType = (DataGridViewTrainType)sender;

            // 線スタイルのセルか？
            if (e.RowIndex >= 0 && e.ColumnIndex == 3)
            {
                // 背景を塗りつぶす色を指定
                e.Graphics.FillRectangle(Brushes.White, e.CellBounds);

                // セル内に線を描画するためのペンを作成
                using (Pen pen = new Pen(Property[e.RowIndex].DiagramLineColor))
                {
                    // スタイル設定
                    pen.DashStyle = Property[e.RowIndex].DiagramLineStyle;

                    // 描画
                    e.Graphics.DrawLine(
                        pen,
                        e.CellBounds.Right - e.CellBounds.Width + 8,
                        e.CellBounds.Top + (e.CellBounds.Height / 2),
                        e.CellBounds.Right - 8,
                        e.CellBounds.Top + (e.CellBounds.Height / 2));
                }

                // セル内のグリッド線を描画するためのペンを作成
                using (Pen pen = new Pen(Brushes.DarkGray))
                {
                    // セル内に線を描画
                    e.Graphics.DrawRectangle(pen, e.CellBounds.Left - 1, e.CellBounds.Top - 1, e.CellBounds.Width, e.CellBounds.Height);
                }

                // イベントを処理したことを通知
                e.Handled = true;
            }
        }


        private void DataGridViewTrainType_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

            FormTrainTypeProperty form = new FormTrainTypeProperty(Property[e.RowIndex]);
            form.OnUpdate += DataGridViewTrainType_OnUpdate;

            DialogResult dialogResult = form.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                // イベント呼出
                OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Property = this.Property });
            }
        }

        private void DataGridViewTrainType_OnUpdate(object sender, TrainTypePropertyUpdateEventArgs e)
        {
            Update(e.Property);
        }

        private void Update(TrainTypeProperty property)
        {
            Property[property.Seq - 1].Copy(property);
            Update(Property);
        }

        private void Update(TrainTypeProperties properties)
        {
            Rows.Clear();

            foreach (var property in properties)
            {
                List<string> values = new List<string>
                {
                    property.Seq.ToString(),
                    property.Name,
                    property.Abbreviation,
                    ""
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
