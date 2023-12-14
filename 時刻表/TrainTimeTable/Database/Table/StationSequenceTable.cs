using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// StationSequenceTable
    /// </summary>
    public class StationSequenceTable : TableListCore<StationSequenceProperties, StationSequenceProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public StationSequenceTable(SQLiteConnection connection)
            : base("StationSequence", connection)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceTable::StationSequenceTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= StationSequenceTable::StationSequenceTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Name TEXT NOT NULL,");                                                    // 駅名
            query.Append("Seq INTEGER NOT NULL,");                                                  // シーケンス番号
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Name));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationSequenceTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref StationSequenceProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceTable::SelectDataRegston(SQLiteDataReader, StationSequenceProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // オブジェクト生成
            StationSequenceProperty property = new StationSequenceProperty();

            // 設定
            property.Name = sqliteDataReader["Name"].ToString();
            property.Seq = int.Parse(sqliteDataReader["Seq"].ToString());

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= StationSequenceTable::SelectDataRegston(SQLiteDataReader, StationSequenceProperties)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(StationSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceTable::Exist(StationSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Name = '" + property.Name + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationSequenceTable::Exist(StationSequenceProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(StationSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceTable::Insert(StationSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Name,");  // 駅名
            query.Append("Seq");    // シーケンス番号
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.Name + "',");
            query.Append(property.Seq.ToString());
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationSequenceTable::Insert(StationSequenceProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(StationSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceTable::Update(StationSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("Seq = " + property.Seq.ToString() + ",");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Name = '" + property.Name + "';");

            // ロギング
            Logger.Debug("<<<<= StationSequenceTable::Update(StationSequenceProperty)");
        }
        #endregion
    }
}
