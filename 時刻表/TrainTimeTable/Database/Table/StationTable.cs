using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// StationTableクラス
    /// </summary>
    public class StationTable : TableListCore<StationProperties, StationProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public StationTable(SQLiteConnection connection)
            : base("Station", connection)
        {
            // ロギング
            Logger.Debug("=>>>> StationTable::StationTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= StationTable::StationTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> StationTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Name TEXT NOT NULL,");                                                    // 駅名
            query.Append("Seq INTEGER NOT NULL,");                                                  // シーケンス番号
            query.Append("StartingStation TEXT NOT NULL DEFAULT 'False',");                         // 起点駅
            query.Append("TerminalStation TEXT NOT NULL DEFAULT 'False',");                         // 終点駅
            query.Append("TimeFormat INTEGER NOT NULL DEFAULT 0,");                                 // 時刻形式(TimeFormat)
            query.Append("StationScale INTEGER NOT NULL DEFAULT 0,");                               // 駅規模(StationScale)
            query.Append("StationDistanceFromReferenceStationOutbound REAL NOT NULL DEFAULT 0.0,"); // 基準駅からの駅距離(下り)
            query.Append("StationDistanceFromReferenceStationInbound REAL NOT NULL DEFAULT 0.0,");  // 基準駅からの駅距離(上り)
            query.Append("Border TEXT NOT NULL DEFAULT 'False',");                                  // 境界線
            query.Append("DiagramTrainInformationOutbound INTEGER NOT NULL DEFAULT 1,");            // ダイヤグラム列車情報(下り)(DiagramTrainInformation)
            query.Append("DiagramTrainInformationInbound INTEGER NOT NULL DEFAULT 1,");             // ダイヤグラム列車情報(上り)(DiagramTrainInformation)
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Name));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref StationProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> StationTable::SelectDataRegston(SQLiteDataReader, StationProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // StationPropertyオブジェクト生成
            StationProperty property = new StationProperty();

            // 設定
            property.Name = sqliteDataReader["Name"].ToString();
            property.Seq = int.Parse(sqliteDataReader["Seq"].ToString());
            property.StartingStation = bool.Parse(sqliteDataReader["StartingStation"].ToString());
            property.TerminalStation = bool.Parse(sqliteDataReader["TerminalStation"].ToString());
            property.TimeFormat = (TimeFormat)int.Parse(sqliteDataReader["TimeFormat"].ToString());
            property.StationScale = (StationScale)int.Parse(sqliteDataReader["StationScale"].ToString());
            property.StationDistanceFromReferenceStations[DirectionType.Outbound] = float.Parse(sqliteDataReader["StationDistanceFromReferenceStationOutbound"].ToString());
            property.StationDistanceFromReferenceStations[DirectionType.Inbound] = float.Parse(sqliteDataReader["StationDistanceFromReferenceStationInbound"].ToString());
            property.Border = bool.Parse(sqliteDataReader["Border"].ToString());
            property.DiagramTrainInformations[DirectionType.Outbound] = (DiagramTrainInformation)int.Parse(sqliteDataReader["DiagramTrainInformationOutbound"].ToString());
            property.DiagramTrainInformations[DirectionType.Inbound] = (DiagramTrainInformation)int.Parse(sqliteDataReader["DiagramTrainInformationInbound"].ToString());

            // 登録
            result.Add(property);

            // ロギング
            Logger.Debug("<<<<= StationTable::SelectDataRegston(SQLiteDataReader, StationProperties)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTable::Exist(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Name = '" + property.Name + "' AND Seq = " + property.Seq.ToString() + ";");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= StationTable::Exist(StationProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTable::Insert(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Name,");                                          // 駅名
            query.Append("Seq,");                                           // シーケンス番号
            query.Append("StartingStation,");                               // 起点駅
            query.Append("TerminalStation,");                               // 終点駅
            query.Append("TimeFormat,");                                    // 時刻形式(TimeFormat)
            query.Append("StationScale,");                                  // 駅規模(StationScale)
            query.Append("StationDistanceFromReferenceStationOutbound,");   // 基準駅からの駅距離(下り)
            query.Append("StationDistanceFromReferenceStationInbound,");    // 基準駅からの駅距離(上り)
            query.Append("Border,");                                        // 境界線
            query.Append("DiagramTrainInformationOutbound,");               // ダイヤグラム列車情報(下り)(DiagramTrainInformation)
            query.Append("DiagramTrainInformationInbound");                 // ダイヤグラム列車情報(上り)(DiagramTrainInformation)
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.Name + "',");
            query.Append(property.Seq.ToString() + ",");
            query.Append("'" + property.StartingStation.ToString() + "',");
            query.Append("'" + property.TerminalStation.ToString() + "',");
            query.Append((int)property.TimeFormat + ",");
            query.Append((int)property.StationScale + ",");
            query.Append(property.StationDistanceFromReferenceStations[DirectionType.Outbound].ToString() + ",");
            query.Append(property.StationDistanceFromReferenceStations[DirectionType.Inbound].ToString() + ",");
            query.Append("'" + property.Border.ToString() + "',");
            query.Append((int)property.DiagramTrainInformations[DirectionType.Outbound] + ",");
            query.Append((int)property.DiagramTrainInformations[DirectionType.Inbound]);
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationTable::Insert(StationProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationTable::Update(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("Seq = " + property.Seq.ToString() + ",");
            query.Append("StartingStation = '" + property.StartingStation.ToString() + "',");
            query.Append("TerminalStation = '" + property.TerminalStation.ToString() + "',");
            query.Append("TimeFormat = " + (int)property.TimeFormat + ",");
            query.Append("StationScale = " + (int)property.StationScale + ",");
            query.Append("StationDistanceFromReferenceStationOutbound = '" + property.StationDistanceFromReferenceStations[DirectionType.Outbound].ToString() + "',");
            query.Append("StationDistanceFromReferenceStationInbound = '" + property.StationDistanceFromReferenceStations[DirectionType.Inbound].ToString() + "',");
            query.Append("Border = '" + property.Border.ToString() + "',");
            query.Append("DiagramTrainInformationOutbound = " + (int)property.DiagramTrainInformations[DirectionType.Outbound] + ",");
            query.Append("DiagramTrainInformationInbound = " + (int)property.DiagramTrainInformations[DirectionType.Inbound] + ",");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Name = '" + property.Name + "' AND Seq = " + property.Seq.ToString() + ";");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= StationTable::Update(StationProperty)");
        }
        #endregion
    }
}
