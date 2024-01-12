using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Component;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// TrainTableクラス
    /// </summary>
    public class TrainTable : TableListCore<TrainProperties, TrainProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public TrainTable(SQLiteConnection connection)
            : base("Train", connection)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::TrainTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TrainTable::TrainTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("DiagramName TEXT NOT NULL,");             // ダイヤグラム名
            query.Append("Direction INTEGER NOT NULL DEFAULT 0,");  // 方向種別
            query.Append("Id INTEGER NOT NULL,");                   // ID
            query.Append("TrainTypeName TEXT,");                    // 列車種別名
            query.Append("No TEXT,");                               // 列車番号
            query.Append("Name TEXT,");                             // 列車名
            query.Append("Number TEXT,");                           // 列車号数
            query.Append("DepartingStation TEXT,");                 // 発駅
            query.Append("DestinationStation TEXT,");               // 着駅
            query.Append("Remarks TEXT,");                          // 備考
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(DiagramName, Direction, Id));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainTable::Create()");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new DictionaryTrain Load(string name)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Load(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // DictionaryTrainオブジェクト生成
            DictionaryTrain result = new DictionaryTrain();

            // TrainPropertiesオブジェクト生成
            TrainProperties trainProperties = new TrainProperties();

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT * FROM {0} WHERE DiagramName = '{1}' ORDER BY Direction;", m_TableName, name));

            // クエリ実行
            using (SQLiteDataReader sqliteDataReader = base.Load(query.ToString()))
            {
                // 1行のみデータを取得
                while (sqliteDataReader.Read())
                {
                    // SELECTデータ登録
                    SelectDataRegston(sqliteDataReader, ref trainProperties);
                }
            }

            // 結果登録
            result.Add(DirectionType.Outbound, new TrainProperties(trainProperties.FindAll(n => n.Direction == DirectionType.Outbound)));
            result.Add(DirectionType.Inbound, new TrainProperties(trainProperties.FindAll(n => n.Direction == DirectionType.Inbound)));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainTable::Load(string)");

            // 返却
            return result;
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TrainProperties Load(DirectionType direction, string name)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Load(DirectionType, string)");
            Logger.DebugFormat("direction:[{0}]", direction.GetStringValue());
            Logger.DebugFormat("name     :[{0}]", name);

            // TrainPropertiesオブジェクト生成
            TrainProperties result = new TrainProperties();

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT * FROM {0} WHERE DiagramName = '{1}' AND Direction = {2};", m_TableName, name, (int)direction));

            // クエリ実行
            using (SQLiteDataReader sqliteDataReader = base.Load(query.ToString()))
            {
                // 1行のみデータを取得
                while (sqliteDataReader.Read())
                {
                    // SELECTデータ登録
                    SelectDataRegston(sqliteDataReader, ref result);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainTable::Load(DirectionType, string)");

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
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref TrainProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::SelectDataRegston(SQLiteDataReader, TrainProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // TrainPropertyオブジェクト生成
            TrainProperty property = new TrainProperty();

            // 設定
            property.DiagramName = sqliteDataReader["DiagramName"].ToString();
            property.Direction = (DirectionType)int.Parse(sqliteDataReader["Direction"].ToString());
            property.Id = int.Parse(sqliteDataReader["Id"].ToString());
            property.TrainTypeName = sqliteDataReader["TrainTypeName"].ToString();
            property.No = sqliteDataReader["No"].ToString();
            property.Name = sqliteDataReader["Name"].ToString();
            property.Number = sqliteDataReader["Number"].ToString();
            property.DepartingStation = sqliteDataReader["DepartingStation"].ToString();
            property.DestinationStation = sqliteDataReader["DestinationStation"].ToString();
            property.Remarks = sqliteDataReader["Remarks"].ToString();

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= TrainTable::SelectDataRegston(SQLiteDataReader, TrainProperties)");
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
            Logger.Debug("=>>>> TrainTable::Save(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                // ダイヤグラム分繰り返す
                foreach (var property in properties)
                {
                    // 方向種別分繰り返す
                    foreach (var direction in property.Trains.Keys)
                    {
                        // 列車分繰り返す
                        foreach (var train in property.Trains[direction])
                        {
                            // 存在判定
                            if (!Exist(train))
                            {
                                // 存在なしの場合
                                Insert(train);
                            }
                            else
                            {
                                // 存在ありの場合
                                Update(train);
                            }
                        }
                    }
                }

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= TrainTable::Save(DiagramProperties)");
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
            Logger.Debug("=>>>> TrainTable::Rebuilding(DiagramProperties, DiagramProperties)");
            Logger.DebugFormat("oldProperties:[{0}]", oldProperties);
            Logger.DebugFormat("newProperties:[{0}]", newProperties);

            // 削除プロパティ取得
            DiagramProperties removeProperties = TableLibrary.GetRemoveKeys(oldProperties, newProperties);

            // ロギング
            Logger.DebugFormat("removeProperties:[{0}]", removeProperties);

            // 削除プロパティ分繰り返す
            foreach (var removeProperty in removeProperties)
            {
                // 方向種別分繰り返す
                foreach (var direction in removeProperty.Trains.Keys)
                {
                    // 削除
                    Remove(removeProperty.Trains[direction]);
                }
            }

            // ダイヤグラム分繰り返す
            foreach (var property in newProperties)
            {
                // 方向種別分繰り返す
                foreach (var direction in property.Trains.Keys)
                {
                    // 再構築
                    Rebuilding(property.Name, direction, property.Trains[direction]);
                }
            }

            // ロギング
            Logger.Debug("<<<<= TrainTable::Rebuilding(DiagramProperties, DiagramProperties)");
        }

        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="name"></param>
        /// <param name="direction"></param>
        /// <param name="properties"></param>
        public void Rebuilding(string name, DirectionType direction, TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Rebuilding(string, TrainProperties)");
            Logger.DebugFormat("name      :[{0}]", name);
            Logger.DebugFormat("direction :[{0}]", direction.GetStringValue());
            Logger.DebugFormat("properties:[{0}]", properties);

            // データを読込
            TrainProperties orignalProperties = Load(direction, name);

            // 削除対象キーを取得
            TrainProperties removeKeys = GetRemoveKeys(orignalProperties, properties);

            // 削除
            Remove(removeKeys);

            // 保存
            Save(properties);

            // ロギング
            Logger.Debug("<<<<= TrainTable::Rebuilding(string, TrainProperties)");
        }
        #endregion

        #region 削除キー取得
        /// <summary>
        /// 削除キー取得
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        protected override TrainProperties GetRemoveKeys(TrainProperties srcProperties, TrainProperties dstProperties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::GetRemoveKeys(TrainProperties, TrainProperties)");
            Logger.DebugFormat("srcProperties:[{0}]", srcProperties);
            Logger.DebugFormat("dstProperties:[{0}]", dstProperties);

            // 結果オブジェクト生成
            TrainProperties result = new TrainProperties();

            // 削除要素作成
            foreach (var src in srcProperties)
            {
                // 削除されたか判定する
                bool removeId = true;
                foreach (var dst in dstProperties)
                {
                    // キーを比較
                    if ((src.DiagramName == dst.DiagramName) && (src.Direction == dst.Direction) && (src.Id == dst.Id))
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
            Logger.Debug("<<<<= TrainTable::GetRemoveKeys(TrainProperties, TrainProperties)");

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
        protected override bool Exist(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Exist(TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("DiagramName = '" + property.DiagramName + "' AND Direction = " + (int)property.Direction + " AND Id = " + property.Id + ";");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainTable::Exist(TrainProperty)");

            // 返却
            return result;
        }
        #endregion

        #region ID最大値取得
        /// <summary>
        /// ID最大値取得
        /// </summary>
        /// <returns></returns>
        public int GetMaxId()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::GetMaxId()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT MAX(Id) FROM {0} WHERE ", m_TableName));

            // SQLiteCommandオブジェクト作成
            using (SQLiteCommand sqliteCommand = new SQLiteCommand(m_SqliteConnection))
            {
                // SQLクエリ設定
                sqliteCommand.CommandText = query.ToString();

                // ロギング
                Logger.DebugFormat("ID最大値取得:[{0}]", query);

                // クエリ実行
                int result = Convert.ToInt32(sqliteCommand.ExecuteScalar());

                // ロギング
                Logger.DebugFormat("result:[{0}]", result);
                Logger.Debug("<<<<= TrainTable::GetMaxId()");

                // 返却
                return result;
            }
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Insert(DiagramProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("DiagramName,");           // ダイヤグラム名
            query.Append("Direction,");             // 方向種別
            query.Append("Id,");                    // ID
            query.Append("TrainTypeName,");         // 列車種別名
            query.Append("No,");                    // 列車番号
            query.Append("Name,");                  // 列車名
            query.Append("Number,");                // 列車号数
            query.Append("DepartingStation,");      // 発駅
            query.Append("DestinationStation,");    // 着駅
            query.Append("Remarks");                // 備考
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.DiagramName + "',");
            query.Append((int)property.Direction + ",");
            query.Append(property.Id.ToString() + ",");
            query.Append("'" + property.TrainTypeName + "',");
            query.Append("'" + property.No + "',");
            query.Append("'" + property.Name + "',");
            query.Append("'" + property.Number + "',");
            query.Append("'" + property.DepartingStation     + "',");
            query.Append("'" + property.DestinationStation + "',");
            query.Append("'" + property.Remarks + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainTable::Insert(DiagramProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(TrainProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Update(TrainProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("TrainTypeName = '" + property.TrainTypeName + "',");
            query.Append("No = '" + property.No.ToString() + "',");
            query.Append("Name = '" + property.Name + "',");
            query.Append("Number = '" + property.Number + "',");
            query.Append("DepartingStation = '" + property.DepartingStation + "',");
            query.Append("DestinationStation = '" + property.DestinationStation + "',");
            query.Append("Remarks = '" + property.Remarks + "',");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE DiagramName = '" + property.DiagramName + "' AND Direction = " + (int)property.Direction + " AND Id = " + property.Id + ";");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainTable::Update(TrainProperty)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="properties"></param>
        protected override void Remove(TrainProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTable::Remove(TrainProperties)");
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
                    query.Append("WHERE DiagramName = '" + property.DiagramName + "' AND Direction = " + (int)property.Direction + " AND Id = " + property.Id + ";");
                }

                // 削除実行
                Remove(query.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= TrainTable::Remove(DiagramProperties)");
        }
        #endregion
    }
}
