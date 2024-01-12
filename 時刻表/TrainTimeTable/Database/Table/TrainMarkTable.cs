﻿using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Database.Table;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database
{
    /// <summary>
    /// TrainMarkTableクラス
    /// </summary>
    public class TrainMarkTable : TableListCore<TrainMarkProperties, TrainMarkProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public TrainMarkTable(SQLiteConnection connection)
            : base("TrainMark", connection)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::TrainMarkTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::TrainMarkTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("DiagramName TEXT NOT NULL,");             // ダイヤグラム名
            query.Append("Direction INTEGER NOT NULL DEFAULT 0,");  // 方向種別
            query.Append("TrainId INTEGER NOT NULL DEFAULT -1,");   // 列車ID
            query.Append("MarkName TEXT,");                         // 記号名
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(DiagramName, Direction, TrainId, MarkName));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::Create()");
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
            Logger.Debug("=>>>> TrainMarkTable::Load(KeyValuePair<DirectionType, TrainProperties>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // 列車毎に繰り返す
            foreach (var train in keyValuePair.Value)
            {
                // SQLクエリ生成
                StringBuilder query = new StringBuilder();
                query.Append(string.Format("SELECT * FROM {0} WHERE DiagramName = '{1}' AND TrainId = {2} AND Direction = {3} ORDER BY Direction;", m_TableName, train.DiagramName, train.Id, (int)train.Direction));

                // クエリ実行
                using (SQLiteDataReader sqliteDataReader = Load(query.ToString()))
                {
                    // TrainMarkPropertiesオブジェクト生成
                    TrainMarkProperties trainMarkProperties = new TrainMarkProperties();

                    // データを取得
                    while (sqliteDataReader.Read())
                    {
                        // SELECTデータ登録
                        SelectDataRegston(sqliteDataReader, ref trainMarkProperties);
                    }

                    // 列車記号データ登録
                    train.Marks.AddRange(trainMarkProperties);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", keyValuePair.Value);
            Logger.Debug("<<<<= TrainMarkTable::Load(KeyValuePair<DirectionType, TrainProperties>)");

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
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref TrainMarkProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::SelectDataRegston(SQLiteDataReader, TrainMarkProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // TrainMarkPropertyオブジェクト生成
            TrainMarkProperty property = new TrainMarkProperty();

            // 設定
            property.DiagramName = sqliteDataReader["DiagramName"].ToString();
            property.TrainId = int.Parse(sqliteDataReader["TrainId"].ToString());
            property.Direction = (DirectionType)int.Parse(sqliteDataReader["Direction"].ToString());
            property.MarkName = sqliteDataReader["MarkName"].ToString();

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::SelectDataRegston(SQLiteDataReader, TrainMarkProperties)");
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
            Logger.Debug("=>>>> TrainMarkTable::Save(DiagramProperties)");
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
                        foreach (var mark in train.Marks)
                        {
                            // 存在判定
                            if (!Exist(mark))
                            {
                                // 存在なしの場合
                                Insert(mark);
                            }
                            else
                            {
                                // 存在ありの場合
                                Update(mark);
                            }
                        }
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::Save(DiagramProperties)");
        }
        #endregion

        #region 再構築
        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="newProperties"></param>
        public void Rebuilding(DiagramProperties oldProperties, DiagramProperties newProperties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::Rebuilding(DiagramProperties, DiagramProperties)");
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
                foreach (var key in removeProperty.Trains.Keys)
                {
                    // 列車分繰り返す
                    foreach (var removeTrain in removeProperty.Trains[key])
                    {
                        // 削除
                        Remove(removeTrain.Marks);
                    }
                }
            }

            // ダイヤグラム分繰り返す
            foreach (var newProperty in newProperties)
            {
                // 旧DiagramPropertyオブジェクト取得
                DiagramProperty oldProperty = oldProperties.Find(d => d.Name == newProperty.Name);

                // 方向種別分繰り返す
                foreach (var key in newProperty.Trains.Keys)
                {
                    // 列車分繰り返す
                    foreach (var newTrain in newProperty.Trains[key])
                    {
                        // 旧TrainPropertyオブジェクト取得
                        TrainProperty oldTrain = oldProperty?.Trains[key].Find(t => t.Id == newTrain.Id);

                        // 再構築
                        Rebuilding(oldTrain?.Marks, newTrain.Marks);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::Rebuilding(DiagramProperties, DiagramProperties)");
        }
        #endregion

        #region 削除キー取得
        /// <summary>
        /// 削除キー取得
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        protected override TrainMarkProperties GetRemoveKeys(TrainMarkProperties srcProperties, TrainMarkProperties dstProperties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::GetRemoveKeys(TrainMarkProperties, TrainMarkProperties)");
            Logger.DebugFormat("srcProperties:[{0}]", srcProperties);
            Logger.DebugFormat("dstProperties:[{0}]", dstProperties);

            // 結果オブジェクト生成
            TrainMarkProperties result = new TrainMarkProperties();

            // 削除要素作成
            foreach (var src in srcProperties)
            {
                // 削除されたか判定する
                bool removeId = true;
                foreach (var dst in dstProperties)
                {
                    // キーを比較
                    if ((src.DiagramName == dst.DiagramName) && (src.Direction == dst.Direction) && (src.TrainId == dst.TrainId) && (src.MarkName == dst.MarkName))
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
            Logger.Debug("<<<<= TrainMarkTable::GetRemoveKeys(TrainMarkProperties, TrainMarkProperties)");

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
        protected override bool Exist(TrainMarkProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::Exist(TrainMarkProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("DiagramName = '" + property.DiagramName + "' AND TrainId = " + property.TrainId + " AND Direction = " + (int)property.Direction + " AND MarkName = '" + property.MarkName + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainMarkTable::Exist(TrainMarkProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(TrainMarkProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::Insert(TrainMarkProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("DiagramName,");   // ダイヤグラム名
            query.Append("Direction,");     // 方向種別
            query.Append("TrainId,");       // 列車ID
            query.Append("MarkName");       // 記号名
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.DiagramName + "',");
            query.Append((int)property.Direction + ",");
            query.Append(property.TrainId.ToString() + ",");
            query.Append("'" + property.MarkName.ToString() + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::Insert(TrainMarkProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(TrainMarkProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::Update(TrainMarkProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE DiagramName = '" + property.DiagramName + "' AND TrainId = " + property.TrainId + " AND Direction = " + (int)property.Direction + " AND MarkName = '" + property.MarkName + "';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::Update(TrainMarkProperty)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="properties"></param>
        protected override void Remove(TrainMarkProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainMarkTable::Remove(TrainMarkProperties)");
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
                    query.Append("WHERE DiagramName = '" + property.DiagramName + "' AND TrainId = " + property.TrainId + " AND Direction = " + (int)property.Direction + " AND MarkName = '" + property.MarkName + "';");
                }

                // 削除実行
                Remove(query.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= TrainMarkTable::Remove(TrainMarkProperties)");
        }
        #endregion
    }
}
