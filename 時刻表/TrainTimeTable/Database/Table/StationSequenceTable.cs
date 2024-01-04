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

        #region 削除キー取得
        /// <summary>
        /// 削除キー取得
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        protected override StationSequenceProperties GetRemoveKeys(StationSequenceProperties srcProperties, StationSequenceProperties dstProperties)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::GetRemoveKeys(StationSequenceProperties, StationSequenceProperties)");
            Logger.DebugFormat("srcProperties:[{0}]", srcProperties);
            Logger.DebugFormat("dstProperties:[{0}]", dstProperties);

            // 結果オブジェクト生成
            StationSequenceProperties result = new StationSequenceProperties();

            // 削除要素作成
            foreach (var src in srcProperties)
            {
                // 削除されたか判定する
                bool removeId = true;
                foreach (var dst in dstProperties)
                {
                    // キーを比較
                    if (src.Name == dst.Name)
                    {
                        removeId = false;
                        break;
                    }
                }

                // 削除対象判定
                if (removeId)
                {
                    // 登録
                    result.Add(src);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= StationSequenceProperties::GetRemoveKeys(StationSequenceProperties, StationSequenceProperties)");

            // 返却
            return result;
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

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationSequenceTable::Update(StationSequenceProperty)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="removeKeys"></param>
        protected override void Remove(StationSequenceProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTable::Rebuilding(StationSequenceProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 件数判定
            if (properties.Count > 0)
            {
                // SQLクエリ
                StringBuilder query = new StringBuilder();

                // 削除対象プロパティ分繰り返す
                foreach (var property in properties)
                {
                    query.Append(string.Format("DELETE FROM {0} ", m_TableName));
                    query.Append("WHERE Name = '" + property.Name + "';");
                }

                // 削除実行
                Remove(query.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= StationTable::Rebuilding(StationSequenceProperties)");
        }
        #endregion
    }
}
