using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// NextStationTableクラス
    /// </summary>
    public class NextStationTable : TableListCore<NextStationProperties, NextStationProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public NextStationTable(SQLiteConnection connection)
            : base("NextStation", connection)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::NextStationTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= NextStationTable::NextStationTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Name TEXT NOT NULL,");                    // 駅名
            query.Append("Direction INTEGER NOT NULL DEFAULT 0,");  // 方向種別
            query.Append("NextStationName TEXT NOT NULL,");         // 次駅名
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Name, Direction, NextStationName));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= NextStationTable::Create()");
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="properties"></param>
        public void Save(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Save(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                // リスト分繰り返す
                foreach (var nextStation in property.NextStations)
                {
                    // 存在判定
                    if (!Exist(nextStation))
                    {
                        // 存在していない場合
                        Insert(nextStation);
                    }
                    else
                    {
                        // 存在している場合
                        Update(nextStation);
                    }
                }

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= NextStationTable::Save(StationProperty)");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public NextStationProperties Load(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Load(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 結果オブジェクト生成
            NextStationProperties result = new NextStationProperties();

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT * FROM {0} WHERE Name ='{1}';", m_TableName, property.Name));

            // クエリ実行
            using (SQLiteDataReader sqliteDataReader = Load(query.ToString()))
            {
                // 1行のみデータを取得
                while (sqliteDataReader.Read())
                {
                    // SELECTデータ登録
                    SelectDataRegston(sqliteDataReader, ref result);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result.ToString());
            Logger.Debug("<<<<= NextStationTable::Load(StationProperty)");

            // 返却
            return result;
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref NextStationProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::SelectDataRegston(SQLiteDataReader, NextStationProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // NextStationPropertyオブジェクト生成
            NextStationProperty property = new NextStationProperty();

            // 設定
            property.Name = sqliteDataReader["Name"].ToString();
            property.Direction = (DirectionType)int.Parse(sqliteDataReader["Direction"].ToString());
            property.NextStationName = sqliteDataReader["NextStationName"].ToString();

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= NextStationTable::SelectDataRegston(SQLiteDataReader, NextStationProperties)");
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="properties"></param>
        public void Save(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Update(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // 保存
                Save(property);
            }

            // ロギング
            Logger.Debug("<<<<= NextStationTable::Update(StationProperties)");
        }
        #endregion

        #region 再構築
        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="oldProperties"></param>
        /// <param name="newProperties"></param>
        public void Rebuilding(StationProperties oldProperties, StationProperties newProperties)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Rebuilding(StationProperties)");
            Logger.DebugFormat("newProperties:[{0}]", oldProperties);
            Logger.DebugFormat("newProperties:[{0}]", newProperties);

            // データを読込
            NextStationProperties orignalProperties = Load();

            // 次駅一覧取得
            NextStationProperties oldNextStations = oldProperties.GetNextStations();

            // 旧データと比較
            if (!orignalProperties.Compare(oldNextStations))
            {
                // ロギング
                Logger.Warn("旧データ相違検出(NextStationTable)");
                Logger.Warn(oldNextStations);
                Logger.Warn(orignalProperties);
            }

            // 次駅一覧取得
            NextStationProperties nextStations = newProperties.GetNextStations();

            // 削除対象キーを取得
            NextStationProperties removeKeys = GetRemoveKeys(orignalProperties, nextStations);

            // 削除
            Remove(removeKeys);

            // 保存
            Save(newProperties);

            // ロギング
            Logger.Debug("<<<<= NextStationTable::Rebuilding(StationProperties, StationProperties)");
        }
        #endregion

        #region 削除キー取得
        /// <summary>
        /// 削除キー取得
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        protected override NextStationProperties GetRemoveKeys(NextStationProperties srcProperties, NextStationProperties dstProperties)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::GetRemoveKeys(StationProperties, StationProperties)");
            Logger.DebugFormat("srcProperties:[{0}]", srcProperties);
            Logger.DebugFormat("dstProperties:[{0}]", dstProperties);

            // 結果オブジェクト生成
            NextStationProperties result = new NextStationProperties();

            // 削除要素作成
            foreach (var src in srcProperties)
            {
                // 削除されたか判定する
                bool removeId = true;
                foreach (var dst in dstProperties)
                {
                    // キーを比較
                    if ((src.Name == dst.Name) && (src.Direction == dst.Direction) && (src.NextStationName == dst.NextStationName))
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
            Logger.Debug("<<<<= NextStationTable::GetRemoveKeys(StationProperties, StationProperties)");

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
        protected override bool Exist(NextStationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Exist(NextStationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Name = '" + property.Name.ToString() + "' AND Direction = " + (int)property.Direction + " AND NextStationName = '" + property.NextStationName + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= NextStationTable::Exist(NextStationProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(NextStationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Insert(NextStationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Name,");              // 駅名
            query.Append("Direction,");         // 方向種別
            query.Append("NextStationName");    // 次駅名
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.Name + "',");
            query.Append((int)property.Direction + ",");
            query.Append("'" + property.NextStationName + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= NextStationTable::Insert(NextStationProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(NextStationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Update(NextStationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Name = '" + property.Name.ToString() + "' AND Direction = " + (int)property.Direction + " AND NextStationName = '" + property.NextStationName + "';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= NextStationTable::Update(NextStationProperty)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="removeKeys"></param>
        protected override void Remove(NextStationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationTable::Rebuilding(NextStationProperties)");
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
                    query.Append("WHERE Name = '" + property.Name.ToString() + "' AND Direction = " + (int)property.Direction + " AND NextStationName = '" + property.NextStationName + "';");
                }

                // 削除実行
                Remove(query.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= NextStationTable::Rebuilding(NextStationProperties)");
        }
        #endregion
    }
}
