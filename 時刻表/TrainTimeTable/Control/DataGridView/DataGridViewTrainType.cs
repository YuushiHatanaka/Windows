using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using TrainTimeTable.EventArgs;
using TrainTimeTable.Property;
using static log4net.Appender.ColoredConsoleAppender;

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
        /// RouteFilePropertyオブジェクト
        /// </summary>
        private RouteFileProperty m_RouteFileProperty { get; set; } = null;

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
            Columns[0].Width = 24;
            Columns[1].Width = 128;
            Columns[2].Width = 64;
            Columns[3].Width = 96;
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
                    new ToolStripMenuItem("上へ貼り付け(&P)", null, TrainTypePasteOnTopOnClick,"paste_on_top"){ ShortcutKeys = (Keys.Control|Keys.V) },
                    new ToolStripMenuItem("下へ貼り付け(&P)", null, TrainTypePasteBelowOnClick,"paste_below"){ ShortcutKeys = (Keys.Control|Keys.V) },
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
            ContextMenuStrip.Items["paste_on_top"].Enabled = false;
            ContextMenuStrip.Items["paste_below"].Enabled = false;
            ContextMenuStrip.Items["up"].Enabled = false;
            ContextMenuStrip.Items["down"].Enabled = false;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public DataGridViewTrainType(RouteFileProperty property)
            : this()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::DataGridViewTrainType(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            //　設定
            m_RouteFileProperty = property;

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType(RouteFileProperty)");
        }
        #endregion

        #region イベント
        /// <summary>
        /// TrainTypeCutoutOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypeCutoutOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeCutoutOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainTypeProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCutoutOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // クリップボードにコピー
            Clipboard.SetDataObject(new TrainTypeProperty(result), true);

            // 列車種別削除
            m_RouteFileProperty.RemoveTrainType(result);

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // イベント呼出
            OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Sequences = m_RouteFileProperty.TrainTypeSequences, Properties = m_RouteFileProperty.TrainTypes });

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCutoutOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypeCopyOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypeCopyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeCopyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainTypeProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCopyOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // クリップボードにコピー
            Clipboard.SetDataObject(new TrainTypeProperty(result), true);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeCopyOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypePasteOnTopOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypePasteOnTopOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypePasteOnTopOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // クリップボードからコピー
            IDataObject dataObject = Clipboard.GetDataObject();

            // クリップボードの内容判定
            if (!(dataObject != null && dataObject.GetDataPresent(typeof(TrainTypeProperty))))
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypePasteOnTopOnClick(object, EventArgs)");

                // 対象外
                return;
            }

            // コピー項目取得
            TrainTypeProperty result = dataObject.GetData(typeof(TrainTypeProperty)) as TrainTypeProperty;

            // 選択インデクス取得
            int index = GetSelectedIndex();

            // 選択インデックス判定
            if (index == -1)
            {
                index = 0;
            }

            // TODO:編集(同一名はNG)

            // 列車種別挿入
            m_RouteFileProperty.InsertTrainType(index, result);

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // イベント呼出
            OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Sequences = m_RouteFileProperty.TrainTypeSequences, Properties = m_RouteFileProperty.TrainTypes });

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypePasteOnTopOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypePasteBelowOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypePasteBelowOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypePasteBelowOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // クリップボードからコピー
            IDataObject dataObject = Clipboard.GetDataObject();

            // クリップボードの内容判定
            if (!(dataObject != null && dataObject.GetDataPresent(typeof(TrainTypeProperty))))
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypePasteBelowOnClick(object, EventArgs)");

                // 対象外
                return;
            }

            // コピー項目取得
            TrainTypeProperty result = dataObject.GetData(typeof(TrainTypeProperty)) as TrainTypeProperty;

            // 選択インデクス取得
            int index = GetSelectedIndex();

            // 選択インデックス判定
            if (index == -1)
            {
                index = m_RouteFileProperty.TrainTypes.Count + 1;
            }

            // TODO:編集(同一名はNG)

            // 列車種別挿入
            m_RouteFileProperty.InsertTrainType(index + 1, result);

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // イベント呼出
            OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Sequences = m_RouteFileProperty.TrainTypeSequences, Properties = m_RouteFileProperty.TrainTypes });

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypePasteBelowOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypeDeleteOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypeDeleteOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeDeleteOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainTypeProperty result = GetSelectedCondition();

            // 選択状態設定
            if (result == null)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeDeleteOnClick(object, EventArgs)");

                // 選択なし
                return;
            }

            // 列車種別削除
            m_RouteFileProperty.RemoveTrainType(result);

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // イベント呼出
            OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Sequences = m_RouteFileProperty.TrainTypeSequences, Properties = m_RouteFileProperty.TrainTypes });

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeDeleteOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypeInsertOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypeInsertOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeInsertOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択インデックス判定
            int index;
            if (selectedIndex == -1)
            {
                index = m_RouteFileProperty.TrainTypes.Count + 1;
            }
            else
            {
                index = selectedIndex;
            }

            // FormTrainTypePropertyオブジェクト生成
            FormTrainTypeProperty form = new FormTrainTypeProperty();

            // フォーム表示
            if (form.ShowDialog() != DialogResult.OK)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeInsertOnClick(object, EventArgs)");

                // キャンセルなので何もしない
                return;
            }

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // 列車種別追加
                m_RouteFileProperty.AddTrainType(form.Property);
            }
            else
            {
                // 列車種別挿入
                m_RouteFileProperty.InsertTrainType(selectedIndex, form.Property);
            }

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // イベント呼出
            OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Sequences = m_RouteFileProperty.TrainTypeSequences, Properties = m_RouteFileProperty.TrainTypes });

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeInsertOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypeUpOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypeUpOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeUpOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeUpOnClick(object, EventArgs)");

                // 未選択なので何もしない
                return;
            }

            // インデックス判定
            if (selectedIndex == 0)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeUpOnClick(object, EventArgs)");

                // 先頭なので何もしない
                return;
            }

            // 選択項目取得
            TrainTypeProperty property = GetSelectedCondition();

            // TrainTypeSequenceProperty取得
            TrainTypeSequenceProperty sequenceProperty = m_RouteFileProperty.TrainTypeSequences.Find(t => t.Name == property.Name);

            // 入れ替え
            m_RouteFileProperty.TrainTypeSequences.ChangeOrder(sequenceProperty.Seq, sequenceProperty.Seq - 1);

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // イベント呼出
            OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Sequences = m_RouteFileProperty.TrainTypeSequences, Properties = m_RouteFileProperty.TrainTypes });

            // 選択行を全て解除
            ClearSelection();

            // 選択行設定
            Rows[selectedIndex - 1].Selected = true;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeUpOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypeDownOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypeDownOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypeDownOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // 選択状態設定
            if (selectedIndex < 0)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeDownOnClick(object, EventArgs)");

                // 未選択なので何もしない
                return;
            }

            // インデックス判定
            if (selectedIndex == RowCount - 1)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeDownOnClick(object, EventArgs)");

                // 末尾なので何もしない
                return;
            }

            // 選択項目取得
            TrainTypeProperty property = GetSelectedCondition();

            // TrainTypeSequenceProperty取得
            TrainTypeSequenceProperty sequenceProperty = m_RouteFileProperty.TrainTypeSequences.Find(t => t.Name == property.Name);

            // 入れ替え
            m_RouteFileProperty.TrainTypeSequences.ChangeOrder(sequenceProperty.Seq, sequenceProperty.Seq + 1);

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // イベント呼出
            OnUpdate(this, new TrainTypePropertiesUpdateEventArgs() { Sequences = m_RouteFileProperty.TrainTypeSequences, Properties = m_RouteFileProperty.TrainTypes });

            // 選択行を全て解除
            ClearSelection();

            // 選択行設定
            Rows[selectedIndex + 1].Selected = true;

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypeDownOnClick(object, EventArgs)");
        }

        /// <summary>
        /// TrainTypePropertyOnClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainTypePropertyOnClick(object sender, System.EventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::TrainTypePropertyOnClick(object, EventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 選択項目取得
            TrainTypeProperty result = GetSelectedCondition();

            // 編集
            Edit(result);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::TrainTypePropertyOnClick(object, EventArgs)");
        }

        #region ContextMenuStripイベント
        /// <summary>
        /// ContextMenuStripOpened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // クリップボードからコピー
            IDataObject dataObject = Clipboard.GetDataObject();

            // クリップボードの内容判定
            if (!(dataObject != null && dataObject.GetDataPresent(typeof(TrainTypeProperty))))
            {
                ContextMenuStrip.Items["paste_on_top"].Enabled = false;
                ContextMenuStrip.Items["paste_below"].Enabled = false;
            }
            else
            {
                ContextMenuStrip.Items["paste_on_top"].Enabled = true;
                ContextMenuStrip.Items["paste_below"].Enabled = true;
            }

            // 選択インデクス取得
            int selectedIndex = GetSelectedIndex();

            // インデックス判定
            if (selectedIndex == 0)
            {
                ContextMenuStrip.Items["up"].Enabled = false;
                ContextMenuStrip.Items["down"].Enabled = true;
            }
            else if (selectedIndex == RowCount - 1)
            {
                ContextMenuStrip.Items["up"].Enabled = true;
                ContextMenuStrip.Items["down"].Enabled = false;
            }
            else
            {
                ContextMenuStrip.Items["up"].Enabled = true;
                ContextMenuStrip.Items["down"].Enabled = true;
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::ContextMenuStripOpened(object, EventArgs)");
        }
        #endregion

        #region DataGridViewTrainTypeイベント
        /// <summary>
        /// DataGridViewTrainType_CellPainting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTrainType_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::DataGridViewTrainType_CellPainting(object, DataGridViewCellPaintingEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 略称のセルか？
            if (e.RowIndex >= 0 && e.ColumnIndex == 2)
            {
                // TrainTypePropertyオブジェクト取得
                TrainTypeProperty property = GetSelectedCondition(e.RowIndex);

                // 背景を塗りつぶす色を指定
                Color fillRectangleColor;
                if ((e.PaintParts & DataGridViewPaintParts.SelectionBackground) == DataGridViewPaintParts.SelectionBackground &&
                    (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                {
                    fillRectangleColor = e.CellStyle.SelectionBackColor;
                }
                else
                {
                    fillRectangleColor = e.CellStyle.BackColor;
                }

                // セルを塗りつぶすためのペンを作成
                using (Pen pen = new Pen(fillRectangleColor))
                {
                    //セルを塗りつぶす
                    e.Graphics.FillRectangle(pen.Brush, e.CellBounds);
                }

                // セル内に線を描画するためのペンを作成
                using (Pen pen = new Pen(property.StringsColor))
                {
                    // StringFormatオブジェクト生成
                    StringFormat format = new StringFormat();
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Near;

                    // 文字列描写
                    e.Graphics.DrawString(
                        property.Abbreviation,
                        m_RouteFileProperty.Fonts.GetFont(property.TimetableFontName),
                        pen.Brush,
                        new Rectangle()
                        {
                            X = e.CellBounds.X,
                            Y = e.CellBounds.Y,
                            Width = e.CellBounds.Width,
                            Height = e.CellBounds.Height
                        },
                        format);
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
            // 線スタイルのセルか？
            else if (e.RowIndex >= 0 && e.ColumnIndex == 3)
            {
                // TrainTypePropertyオブジェクト取得
                TrainTypeProperty property = GetSelectedCondition(e.RowIndex);

                // 背景を塗りつぶす色を指定
                Color fillRectangleColor;
                if ((e.PaintParts & DataGridViewPaintParts.SelectionBackground) == DataGridViewPaintParts.SelectionBackground &&
                    (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                {
                    fillRectangleColor = e.CellStyle.SelectionBackColor;
                }
                else
                {
                    fillRectangleColor = e.CellStyle.BackColor;
                }

                // セルを塗りつぶすためのペンを作成
                using (Pen pen = new Pen(fillRectangleColor))
                {
                    //セルを塗りつぶす
                    e.Graphics.FillRectangle(pen.Brush, e.CellBounds);
                }

                // セル内に線を描画するためのペンを作成
                using (Pen pen = new Pen(property.DiagramLineColor))
                {
                    // スタイル設定
                    pen.DashStyle = property.DiagramLineStyle;

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

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType_CellPainting(object, DataGridViewCellPaintingEventArgs)");
        }

        /// <summary>
        /// DataGridViewTrainType_CellDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTrainType_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::DataGridViewTrainType_CellDoubleClick(object, DataGridViewCellEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 行ヘッダー
            if (e.RowIndex == -1)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType_CellDoubleClick(object, DataGridViewCellEventArgs)");

                // 何もしない
                return;
            }
            // 列ヘッダー
            if (e.ColumnIndex == -1)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType_CellDoubleClick(object, DataGridViewCellEventArgs)");

                // 何もしない
                return;
            }

            // 選択項目取得
            TrainTypeProperty result = GetSelectedCondition(e.RowIndex);

            // 編集
            Edit(result);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType_CellDoubleClick(object, DataGridViewCellEventArgs)");
        }

        /// <summary>
        /// DataGridViewTrainType_OnUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTrainType_OnUpdate(object sender, TrainTypePropertyUpdateEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::DataGridViewTrainType_OnUpdate(object, TrainTypePropertyUpdateEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender);
            Logger.DebugFormat("e     :[{0}]", e);

            // 更新
            Update(e.Property);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::DataGridViewTrainType_OnUpdate(object, TrainTypePropertyUpdateEventArgs)");
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
            Logger.Debug("=>>>> DataGridViewTrainType::GetSelectedCondition()");

            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::GetSelectedCondition()");

                // 選択なし
                return -1;
            }

            // 選択インデックス設定
            int result = SelectedCells[0].RowIndex;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewTrainType::GetSelectedCondition()");

            // 返却
            return result;
        }

        #region 選択情報取得
        /// <summary>
        /// 選択情報取得
        /// </summary>
        /// <returns></returns>
        private TrainTypeProperty GetSelectedCondition()
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::GetSelectedCondition()");

            // 選択状態設定
            if (SelectedCells.Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= DataGridViewTrainType::GetSelectedCondition()");

                // 選択なし
                return null;
            }

            // 選択オブジェクト
            TrainTypeProperty result = GetSelectedCondition(SelectedCells[0].RowIndex);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewTrainType::GetSelectedCondition()");

            // 返却
            return result;
        }

        /// <summary>
        /// 選択情報取得
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private TrainTypeProperty GetSelectedCondition(int row)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::GetSelectedCondition(int)");
            Logger.DebugFormat("row:[{0}]", row);

            // TrainTypeSequencePropertyオブジェクト取得
            List<TrainTypeSequenceProperty> trainTypeSequence = m_RouteFileProperty.TrainTypeSequences.OrderBy(t => t.Seq).ToList();

            // 選択オブジェクト
            TrainTypeProperty result = m_RouteFileProperty.TrainTypes.Find(t => t.Name == trainTypeSequence[row].Name);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DataGridViewTrainType::GetSelectedCondition(int)");

            // 返却
            return result;
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        private void Update(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::Update(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            m_RouteFileProperty.TrainTypes.Find(t => t.Name == property.Name).Copy(property);

            // 更新
            Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::Update(StationProperty)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sequences"></param>
        /// <param name="properties"></param>
        private void Update(TrainTypeSequenceProperties sequences, TrainTypeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::Update(TrainTypeSequenceProperties, TrainTypeProperties)");
            Logger.DebugFormat("sequences :[{0}]", sequences);
            Logger.DebugFormat("properties:[{0}]", properties);

            // 全行クリア
            Rows.Clear();

            // シーケンス分繰り返す
            foreach (var sequence in sequences.OrderBy(t => t.Seq))
            {
                // TrainTypePropertyオブジェクト取得
                TrainTypeProperty property = properties.Find(t=>t.Name == sequence.Name);

                // 追加オブジェクト生成
                List<string> values = new List<string>
                {
                    sequence.Seq.ToString(),
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

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::Update(TrainTypeSequenceProperties, TrainTypeProperties)");
        }
        #endregion

        #region 編集
        /// <summary>
        /// 編集
        /// </summary>
        /// <param name="property"></param>
        private void Edit(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DataGridViewTrainType::Edit(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // TrainTypePropertiesUpdateEventArgsオブジェクト生成
            TrainTypePropertiesUpdateEventArgs eventArgs = new TrainTypePropertiesUpdateEventArgs();
            eventArgs.OldTrainTypeName = property.Name;
            eventArgs.Properties = m_RouteFileProperty.TrainTypes;
            eventArgs.Sequences = m_RouteFileProperty.TrainTypeSequences;
            eventArgs.OldProperties.Copy(m_RouteFileProperty.TrainTypes);

            // FormTrainTypePropertyオブジェクト生成
            FormTrainTypeProperty form = new FormTrainTypeProperty(m_RouteFileProperty.Fonts, property);

            // 結果判定
            DialogResult dialogResult = form.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                // 駅名(キーが変更されたか？)
                if (eventArgs.OldTrainTypeName != form.Property.Name)
                {
                    // 同一名判定
                    if (m_RouteFileProperty.TrainTypes.Find(t => t.Name == form.Property.Name) != null)
                    {
                        // エラーメッセージ
                        MessageBox.Show(string.Format("既に登録されている列車種別名は使用できません:[{0}]", form.Property.Name), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // イベント引数設定
                    eventArgs.NewTrainTypeName = form.Property.Name;

                    // 列車種別変更
                    m_RouteFileProperty.ChangeTrainType(eventArgs.OldTrainTypeName, form.Property.Name);
                }

                // 結果保存
                property.Copy(form.Property);

                // 変更されたか？
                if (!eventArgs.OldProperties.Compare(m_RouteFileProperty.TrainTypes))
                {
                    // 更新
                    Update(m_RouteFileProperty.TrainTypeSequences, m_RouteFileProperty.TrainTypes);

                    // 更新通知
                    OnUpdate(this, eventArgs);
                }
            }

            // ロギング
            Logger.Debug("<<<<= DataGridViewTrainType::Edit(TrainTypeProperty)");
        }
        #endregion
        #endregion
    }
}
