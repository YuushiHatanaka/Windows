using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// DiagramScreenTableクラス
    /// </summary>
    public class DiagramScreenTable : TablePropertyCore<DiagramScreenProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public DiagramScreenTable(SQLiteConnection connection)
            : base("DiagramScreen", connection)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenTable::DiagramScreenTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= DiagramScreenTable::DiagramScreenTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Key TEXT,");                                                              // 仮キー文字
            query.Append("DiagramDtartingTime TEXT,");                                              // ダイヤグラム起点時刻
            query.Append("StandardWidthBetweenStationsInTheDiagram INTEGER NOT NULL DEFAULT 60,");  // ダイヤグラムの規定の駅間幅
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Key));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= DiagramScreenTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref DiagramScreenProperty result)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenTable::SelectDataRegston(SQLiteDataReader, DiagramScreenProperty)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // 設定
            result.DiagramDtartingTime = DateTime.ParseExact(sqliteDataReader["DiagramDtartingTime"].ToString(), "HHmm", null);
            result.StandardWidthBetweenStationsInTheDiagram = int.Parse(sqliteDataReader["StandardWidthBetweenStationsInTheDiagram"].ToString());

            // ロギング
            Logger.Debug("<<<<= DiagramScreenTable::SelectDataRegston(SQLiteDataReader, DiagramScreenProperty)");
        }
        #endregion

        #region 再構築
        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="oldProperty"></param>
        /// <param name="newProperty"></param>
        public void Rebuilding(DiagramScreenProperty oldProperty, DiagramScreenProperty newProperty)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenTable::Rebuilding(DiagramScreenProperty, DiagramScreenProperty)");
            Logger.DebugFormat("oldProperty:[{0}]", oldProperty);
            Logger.DebugFormat("newProperty:[{0}]", newProperty);

            // データを読込
            DiagramScreenProperty orignalProperties = Load();

            // 旧データと比較
            if (!orignalProperties.Compare(oldProperty))
            {
                // ロギング
                Logger.Warn("旧データ相違検出(DiagramScreenTable)");
                Logger.Warn(oldProperty);
                Logger.Warn(orignalProperties);
            }

            // 比較
            if (!orignalProperties.Compare(newProperty))
            {
                // 保存
                Save(newProperty);
            }

            // ロギング
            Logger.Debug("<<<<= DiagramScreenTable::Rebuilding(DiagramScreenProperty, DiagramScreenProperty)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(DiagramScreenProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenTable::Exist(DiagramScreenProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Key = '1';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= DiagramScreenTable::Exist(DiagramScreenProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(DiagramScreenProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenTable::Insert(DiagramScreenProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Key,");                                       // 仮キー文字
            query.Append("DiagramDtartingTime,");                       // ダイヤグラム起点時刻
            query.Append("StandardWidthBetweenStationsInTheDiagram");   // ダイヤグラムの規定の駅間幅
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'1',");
            query.Append("'" + property.DiagramDtartingTime.ToString("HHmm") + "',");
            query.Append(property.StandardWidthBetweenStationsInTheDiagram.ToString());
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= DiagramScreenTable::Insert(DiagramScreenProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(DiagramScreenProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramScreenTable::Update(DiagramScreenProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("DiagramDtartingTime = '" + property.DiagramDtartingTime.ToString("HHmm") + "',");
            query.Append("StandardWidthBetweenStationsInTheDiagram = " + property.StandardWidthBetweenStationsInTheDiagram.ToString() + ",");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Key = '1';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= DiagramScreenTable::Update(DiagramScreenProperty)");
        }
        #endregion
    }
}
