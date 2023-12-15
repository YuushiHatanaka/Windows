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
    /// TrainSequenceTableクラス
    /// </summary>
    public class TrainSequenceTable : TableListCore<TrainSequenceProperties, TrainSequenceProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public TrainSequenceTable(SQLiteConnection connection)
            : base("TrainSequence", connection)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceTable::TrainSequenceTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TrainSequenceTable::TrainSequenceTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("DiagramId INTEGER NOT NULL DEFAULT -1,"); // ダイヤグラムID
            query.Append("Direction INTEGER NOT NULL DEFAULT 0,");  // 方向種別
            query.Append("Id INTEGER NOT NULL,");                   // ID
            query.Append("Seq INTEGER NOT NULL,");                  // シーケンス番号
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(DiagramId, Direction, Id));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainSequenceTable::Create()");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public DictionaryTrainSequence Load(int index)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceTable::Load(int)");
            Logger.DebugFormat("index:[{0}]", index);

            // DictionaryTrainSequenceオブジェクト生成
            DictionaryTrainSequence result = new DictionaryTrainSequence();

            // TrainSequencePropertiesオブジェクト生成
            TrainSequenceProperties trainProperties = new TrainSequenceProperties();

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT * FROM {0} WHERE DiagramId = {1} ORDER BY Direction,Seq;", m_TableName, index));

            // クエリ実行
            using (SQLiteDataReader sqliteDataReader = Load(query.ToString()))
            {
                // 1行のみデータを取得
                while (sqliteDataReader.Read())
                {
                    // SELECTデータ登録
                    SelectDataRegston(sqliteDataReader, ref trainProperties);
                }
            }

            // 結果登録
            result.Add(DirectionType.Outbound, new TrainSequenceProperties(trainProperties.FindAll(n => n.Direction == DirectionType.Outbound)));
            result.Add(DirectionType.Inbound, new TrainSequenceProperties(trainProperties.FindAll(n => n.Direction == DirectionType.Inbound)));

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainSequenceTable::Load(int)");

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
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref TrainSequenceProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceTable::SelectDataRegston(SQLiteDataReader, TrainSequenceProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // オブジェクト生成
            TrainSequenceProperty property = new TrainSequenceProperty();

            // 設定
            property.DiagramId = int.Parse(sqliteDataReader["DiagramId"].ToString());
            property.Direction = (DirectionType)int.Parse(sqliteDataReader["Direction"].ToString());
            property.Id = int.Parse(sqliteDataReader["Id"].ToString());
            property.Seq = int.Parse(sqliteDataReader["Seq"].ToString());

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= TrainSequenceTable::SelectDataRegston(SQLiteDataReader, TrainSequenceProperties)");
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
            Logger.Debug("=>>>> TrainSequenceTable::Save(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                // ダイヤグラム分繰り返す
                foreach (var property in properties)
                {
                    // 方向種別分繰り返す
                    foreach (var direction in property.TrainSequence.Keys)
                    {
                        // 列車分繰り返す
                        foreach (var trainSequence in property.TrainSequence[direction])
                        {
                            // 存在判定
                            if (!Exist(trainSequence))
                            {
                                // 存在なしの場合
                                Insert(trainSequence);
                            }
                            else
                            {
                                // 存在ありの場合
                                Update(trainSequence);
                            }
                        }
                    }
                }

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= TrainSequenceTable::Save(DiagramProperties)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(TrainSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceTable::Exist(TrainSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("DiagramId = " + property.DiagramId + " AND Direction = " + (int)property.Direction + " AND Id = " + property.Id.ToString() + " AND Seq = " + property.Seq + ";");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainSequenceTable::Exist(TrainSequenceProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(TrainSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceTable::Insert(TrainSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("DiagramId,"); // ダイヤグラムID
            query.Append("Direction,"); // 方向種別
            query.Append("Id,");        // ID
            query.Append("Seq");        // シーケンス番号
            query.Append(") VALUES ");
            query.Append("(");
            query.Append(property.DiagramId.ToString() + ",");
            query.Append((int)property.Direction + ",");
            query.Append(property.Id.ToString() + ",");
            query.Append(property.Seq.ToString());
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TrainSequenceTable::Insert(TrainSequenceProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(TrainSequenceProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainSequenceTable::Update(TrainSequenceProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE DiagramId = " + property.DiagramId + " AND Direction = " + (int)property.Direction + " AND Id = " + property.Id + " AND Seq = " + property.Seq +  ";");

            // ロギング
            Logger.Debug("<<<<= TrainSequenceTable::Update(TrainSequenceProperty)");
        }
        #endregion
    }
}
