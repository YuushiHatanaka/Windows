﻿using System;
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
    /// TrainMarkSequenceTableクラス
    /// </summary>
    public class TrainMarkSequenceTable : TableListCore<TrainMarkSequenceProperties, TrainMarkSequenceProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public TrainMarkSequenceTable(SQLiteConnection connection)
            : base("TrainMarkSequence", connection)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceTable::TrainMarkSequenceTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceTable::TrainMarkSequenceTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("DiagramId INTEGER NOT NULL DEFAULT -1,"); // ダイヤグラムID
            query.Append("Direction INTEGER NOT NULL DEFAULT 0,");  // 方向種別
            query.Append("TrainId INTEGER NOT NULL DEFAULT -1,");   // 列車ID
            query.Append("MarkName TEXT,");                         // 記号名
            query.Append("Seq INTEGER NOT NULL,");                  // シーケンス番号
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(DiagramId, Direction, TrainId, MarkName));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceTable::Create()");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="train"></param>
        /// <returns></returns>
        public TrainProperties Load(KeyValuePair<DirectionType, TrainProperties> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceTable::Load(KeyValuePair<DirectionType, TrainProperties>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // 列車毎に繰り返す
            foreach (var train in keyValuePair.Value)
            {
                // SQLクエリ生成
                StringBuilder query = new StringBuilder();
                query.Append(string.Format("SELECT * FROM {0} WHERE DiagramId = {1} AND TrainId = {2} AND Direction = {3} ORDER BY Direction;", m_TableName, train.DiagramId, train.Id, (int)train.Direction));

                // クエリ実行
                using (SQLiteDataReader sqliteDataReader = Load(query.ToString()))
                {
                    // TrainMarkPropertiesオブジェクト生成
                    TrainMarkSequenceProperties trainMarkProperties = new TrainMarkSequenceProperties();

                    // データを取得
                    while (sqliteDataReader.Read())
                    {
                        // SELECTデータ登録
                        SelectDataRegston(sqliteDataReader, ref trainMarkProperties);
                    }

                    // 列車記号シーケンスデータ登録
                    train.MarkSequences.AddRange(trainMarkProperties);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", keyValuePair.Value);
            Logger.Debug("<<<<= TrainMarkSequenceTable::Load(KeyValuePair<DirectionType, TrainProperties>)");

            // 返却
            return keyValuePair.Value;
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref TrainMarkSequenceProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceTable::SelectDataRegston(SQLiteDataReader, TrainMarkSequenceProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // オブジェクト生成
            TrainMarkSequenceProperty property = new TrainMarkSequenceProperty()
            {
                DiagramId = int.Parse(sqliteDataReader["DiagramId"].ToString()),
                TrainId = int.Parse(sqliteDataReader["TrainId"].ToString()),
                Direction = (DirectionType)int.Parse(sqliteDataReader["Direction"].ToString()),
                Seq = int.Parse(sqliteDataReader["Seq"].ToString()),
                MarkName = sqliteDataReader["MarkName"].ToString(),
            };

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceTable::SelectDataRegston(SQLiteDataReader, TrainMarkSequenceProperties)");
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
            Logger.Debug("=>>>> TrainMarkSequenceTable::Save(DiagramProperties)");
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
                        // 列車記号分繰り返す
                        foreach (var markSequences in train.MarkSequences)
                        {
                            // 存在判定
                            if (!Exist(markSequences))
                            {
                                // 存在なしの場合
                                Insert(markSequences);
                            }
                            else
                            {
                                // 存在ありの場合
                                Update(markSequences);
                            }
                        }
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceTable::Save(DiagramProperties)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(TrainMarkSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceTable::Exist(TrainMarkSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("DiagramId = " + property.DiagramId + " AND TrainId = " + property.TrainId + " AND Direction = " + (int)property.Direction + " AND Seq = " + property.Seq + ";");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceTable::Exist(TrainMarkSequenceProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(TrainMarkSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceTable::Insert(TrainMarkSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("DiagramId,"); // ダイヤグラムID
            query.Append("Direction,"); // 方向種別
            query.Append("TrainId,");   // 列車ID
            query.Append("Seq,");       // シーケンス番号
            query.Append("MarkName");   // 記号名
            query.Append(") VALUES ");
            query.Append("(");
            query.Append(property.DiagramId.ToString() + ",");
            query.Append((int)property.Direction + ",");
            query.Append(property.TrainId.ToString() + ",");
            query.Append(property.Seq.ToString() + ",");
            query.Append("'" + property.MarkName.ToString() + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceTable::Insert(TrainMarkSequenceProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(TrainMarkSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkSequenceTable::Update(TrainMarkSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("MarkName = '" + property.MarkName.ToString() + "',");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE DiagramId = " + property.DiagramId + " AND TrainId = " + property.TrainId + " AND Direction = " + (int)property.Direction + " AND Seq = " + property.Seq + ";");

            // ロギング
            Logger.Debug("<<<<= TrainMarkSequenceTable::Update(TrainMarkSequenceProperty)");
        }
        #endregion
    }
}
