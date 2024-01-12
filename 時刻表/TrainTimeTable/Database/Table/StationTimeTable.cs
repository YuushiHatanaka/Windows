using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// StationTimeTableクラス
    /// </summary>
    public class StationTimeTable : TableListCore<StationTimeProperties, StationTimeProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public StationTimeTable(SQLiteConnection connection)
            : base("StationTime", connection)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::StationTimeTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::StationTimeTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("DiagramName TEXT NOT NULL,");                     // ダイヤグラム名
            query.Append("Direction INTEGER NOT NULL DEFAULT 0,");          // 方向種別
            query.Append("TrainId INTEGER NOT NULL DEFAULT -1,");           // 列車ID
            query.Append("StationName TEXT,");                              // 駅名
            query.Append("StationTreatment INTEGER NOT NULL DEFAULT 0,");   // 駅扱い
            query.Append("DepartureTime TEXT,");                            // 発時刻
            query.Append("EstimatedDepartureTime TEXT DEFAULT 'FALSE',");   // 推定時刻(発時刻)
            query.Append("ArrivalTime TEXT,");                              // 着時刻
            query.Append("EstimatedArrivalTime TEXT DEFAULT 'FALSE',");     // 推定時刻(着時刻)
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(DiagramName, Direction, TrainId, StationName));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Create()");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="keyValuePair"></param>
        /// <returns></returns>

        public TrainProperties Load(KeyValuePair<DirectionType, TrainProperties> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Load(KeyValuePair<DirectionType, TrainProperties>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // 列車毎に繰り返す
            foreach (var train in keyValuePair.Value)
            {
                // SQLクエリ生成
                StringBuilder query = new StringBuilder();
                query.Append(string.Format("SELECT * FROM {0} WHERE DiagramName = '{1}' AND Direction = {2} AND TrainId = {3} ORDER BY Direction;", m_TableName, train.DiagramName, (int)train.Direction, train.Id));

                // クエリ実行
                using (SQLiteDataReader sqliteDataReader = Load(query.ToString()))
                {
                    // StationTimePropertiesオブジェクト生成
                    StationTimeProperties stationTimeProperties = new StationTimeProperties();

                    // データを取得
                    while (sqliteDataReader.Read())
                    {
                        // SELECTデータ登録
                        SelectDataRegston(sqliteDataReader, ref stationTimeProperties);
                    }

                    // 時刻表データ登録
                    train.StationTimes.AddRange(stationTimeProperties);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", keyValuePair.Value);
            Logger.Debug("<<<<= StationTimeTable::Load(KeyValuePair<DirectionType, TrainProperties>)");

            // 返却
            return keyValuePair.Value;
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="train"></param>
        /// <returns></returns>
        private StationTimeProperties Load(TrainProperty train)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Load(TrainProperty)");
            Logger.DebugFormat("train:[{0}]", train);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT * FROM {0} WHERE DiagramName = '{1}' AND Direction = {2} AND TrainId = {3} ORDER BY Direction;", m_TableName, train.DiagramName, (int)train.Direction, train.Id));

            // 結果オブジェクト生成
            StationTimeProperties result = new StationTimeProperties();

            // クエリ実行
            using (SQLiteDataReader sqliteDataReader = Load(query.ToString()))
            {

                // データを取得
                while (sqliteDataReader.Read())
                {
                    // SELECTデータ登録
                    SelectDataRegston(sqliteDataReader, ref result);
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Load(TrainProperty)");

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
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref StationTimeProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::SelectDataRegston(SQLiteDataReader, StationTimeProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // StationTimePropertyオブジェクト生成
            StationTimeProperty property = new StationTimeProperty()
            {
                // 設定
                DiagramName = sqliteDataReader["DiagramName"].ToString(),
                Direction = (DirectionType)int.Parse(sqliteDataReader["Direction"].ToString()),
                TrainId = int.Parse(sqliteDataReader["TrainId"].ToString()),
                StationName = sqliteDataReader["StationName"].ToString(),
                StationTreatment = (StationTreatment)int.Parse(sqliteDataReader["StationTreatment"].ToString()),
                DepartureTime = sqliteDataReader["DepartureTime"].ToString(),
                EstimatedDepartureTime = bool.Parse(sqliteDataReader["EstimatedDepartureTime"].ToString()),
                ArrivalTime = sqliteDataReader["ArrivalTime"].ToString(),
                EstimatedArrivalTime = bool.Parse(sqliteDataReader["EstimatedArrivalTime"].ToString()),
            };

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::SelectDataRegston(SQLiteDataReader, StationTimeProperties)");
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="diagrams"></param>
        public void Save(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Save(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // ダイヤグラム分繰り返す
            foreach (var property in properties)
            {
                // 方向種別分繰り返す
                foreach (var value in property.Trains.Values)
                {
                    // 列車分繰り返す
                    foreach (var train in value)
                    {
                        // 駅時刻分繰り返す
                        foreach (var stationtime in train.StationTimes)
                        {
                            // 存在判定
                            if (!Exist(stationtime))
                            {
                                // 存在なしの場合
                                Insert(stationtime);
                            }
                            else
                            {
                                // 存在ありの場合
                                Update(stationtime);
                            }
                        }
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Save(DiagramProperties)");
        }
        #endregion

        #region 再構築
        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="oldProperties"></param>
        /// <param name="newProperties"></param>
        public void Rebuilding(DiagramProperties oldProperties, DiagramProperties newProperties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Rebuilding(DiagramProperties, DiagramProperties)");
            Logger.DebugFormat("oldProperties:[{0}]", oldProperties);
            Logger.DebugFormat("newProperties:[{0}]", newProperties);

            // 削除プロパティ取得
            DiagramProperties removeProperties = TableLibrary.GetRemoveKeys(oldProperties, newProperties);

            // ロギング
            Logger.DebugFormat("removeProperties:[{0}]", removeProperties);

            // ダイヤグラム分繰り返す
            foreach (var removeProperty in removeProperties)
            {
                // 方向種別分繰り返す
                foreach (var direction in removeProperty.Trains.Keys)
                {
                    // 列車分繰り返す
                    foreach (var train in removeProperty.Trains[direction])
                    {
                        // 削除
                        Remove(train.StationTimes);
                    }
                }
            }

            // ダイヤグラム分繰り返す
            foreach (var property in newProperties)
            {
                // 方向種別分繰り返す
                foreach (var direction in property.Trains.Keys)
                {
                    // 列車分繰り返す
                    foreach (var train in property.Trains[direction])
                    {
                        // 再構築
                        Rebuilding(train, train.StationTimes);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Rebuilding(DiagramProperties, DiagramProperties)");
        }

        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="properties"></param>
        private void Rebuilding(TrainProperty train, StationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Rebuilding(StationTimeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // データを読込
            StationTimeProperties orignalProperties = Load(train);

            // 削除対象キーを取得
            StationTimeProperties removeKeys = GetRemoveKeys(orignalProperties, properties);

            // 削除
            Remove(removeKeys);

            // 保存
            Save(properties);

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Rebuilding(StationTimeProperties)");
        }
        #endregion

        #region 削除キー取得
        /// <summary>
        /// 削除キー取得
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        protected override StationTimeProperties GetRemoveKeys(StationTimeProperties srcProperties, StationTimeProperties dstProperties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::GetRemoveKeys(StationTimeProperties, StationTimeProperties)");
            Logger.DebugFormat("srcProperties:[{0}]", srcProperties);
            Logger.DebugFormat("dstProperties:[{0}]", dstProperties);

            // 結果オブジェクト生成
            StationTimeProperties result = new StationTimeProperties();

            // 削除要素作成
            foreach (var src in srcProperties)
            {
                // 削除されたか判定する
                bool removeId = true;
                foreach (var dst in dstProperties)
                {
                    // キーを比較
                    if ((src.DiagramName == dst.DiagramName) && (src.Direction == dst.Direction) && (src.TrainId == dst.TrainId) && (src.StationName == dst.StationName))
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
            Logger.Debug("<<<<= StationTimeTable::GetRemoveKeys(StationTimeProperties, StationTimeProperties)");

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
        protected override bool Exist(StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Exist(StationTimeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("DiagramName = '" + property.DiagramName + "' AND Direction = " + (int)property.Direction + " AND TrainId = " + property.TrainId + " AND StationName = '" + property.StationName + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= StationTimeTable::Exist(StationTimeProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Insert(StationTimeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("DiagramName,");               // ダイヤグラム名
            query.Append("Direction,");                 // 方向種別
            query.Append("TrainId,");                   // 列車ID
            query.Append("StationName,");               // 駅名
            query.Append("StationTreatment,");          // 駅扱い
            query.Append("DepartureTime,");             // 発時刻
            query.Append("EstimatedDepartureTime,");    // 推定時刻(発時刻)
            query.Append("ArrivalTime,");               // 着時刻
            query.Append("EstimatedArrivalTime");       // 推定時刻(着時刻)
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.DiagramName + "',");
            query.Append((int)property.Direction + ",");
            query.Append(property.TrainId.ToString() + ",");
            query.Append("'" + property.StationName.ToString() + "',");
            query.Append((int)property.StationTreatment + ",");
            query.Append("'" + property.DepartureTime.ToString() + "',");
            query.Append("'" + property.EstimatedDepartureTime.ToString() + "',");
            query.Append("'" + property.ArrivalTime.ToString() + "',");
            query.Append("'" + property.EstimatedArrivalTime.ToString() + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Insert(StationTimeProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Update(StationTimeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("StationName = '" + property.StationName.ToString() + "',");
            query.Append("StationTreatment = " + (int)property.StationTreatment + ",");
            query.Append("DepartureTime = '" + property.DepartureTime.ToString() + "',");
            query.Append("EstimatedDepartureTime = '" + property.EstimatedDepartureTime.ToString() + "',");
            query.Append("ArrivalTime = '" + property.ArrivalTime.ToString() + "',");
            query.Append("EstimatedArrivalTime = '" + property.EstimatedArrivalTime.ToString() + "',");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE DiagramName = '" + property.DiagramName + "' AND Direction = " + (int)property.Direction + " AND TrainId = " + property.TrainId + " AND StationName = '" + property.StationName + "';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Update(StationTimeProperty)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="properties"></param>
        protected override void Remove(StationTimeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationTimeTable::Remove(StationTimeProperties)");
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
                    query.Append("WHERE DiagramName = '" + property.DiagramName + "' AND Direction = " + (int)property.Direction + " AND TrainId = " + property.TrainId + " AND StationName = '" + property.StationName + "';");
                }

                // 削除実行
                Remove(query.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= StationTimeTable::Remove(StationTimeProperties)");
        }
        #endregion
    }
}
