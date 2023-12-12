using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;
using TrainTimeTable.Database.Table.Core;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// RouteTableクラス
    /// </summary>
    public class RouteTable : TablePropertyCore<RouteProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public RouteTable(SQLiteConnection connection)
            : base("Route", connection)
        {
            // ロギング
            Logger.Debug("=>>>> RouteTable::RouteTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= RouteTable::RouteTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> RouteTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Name TEXT,");                                             // 路線名
            query.Append("OutboundDiaName TEXT,");                                  // 下りダイヤ名
            query.Append("InboundDiaName TEXT,");                                   // 上りダイヤ名
            query.Append("WidthOfStationNameField INTEGER NOT NULL DEFAULT 6,");    // 駅名欄の幅
            query.Append("TimetableTrainWidth INTEGER NOT NULL DEFAULT 8,");        // 時刻表の列車の幅
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Name));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= RouteTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref RouteProperty result)
        {
            // ロギング
            Logger.Debug("=>>>> RouteTable::SelectDataRegston(SQLiteDataReader, RouteProperty)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // 設定
            result.Name = sqliteDataReader["Name"].ToString();
            result.OutboundDiaName = sqliteDataReader["OutboundDiaName"].ToString();
            result.InboundDiaName = sqliteDataReader["InboundDiaName"].ToString();
            result.WidthOfStationNameField = int.Parse(sqliteDataReader["WidthOfStationNameField"].ToString());
            result.TimetableTrainWidth = int.Parse(sqliteDataReader["TimetableTrainWidth"].ToString());

            // ロギング
            Logger.Debug("<<<<= RouteTable::SelectDataRegston(SQLiteDataReader, RouteProperty)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(RouteProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteTable::Exist(RouteProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Name = '" + property.Name + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteTable::Exist(RouteProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(RouteProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteTable::Insert(RouteProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Name,");                      // 駅名
            query.Append("OutboundDiaName,");           // 下りダイヤ名
            query.Append("InboundDiaName,");            // 上りダイヤ名
            query.Append("WidthOfStationNameField,");   // 駅名欄の幅
            query.Append("TimetableTrainWidth");        // 時刻表の列車の幅
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.Name + "',");
            query.Append("'" + property.OutboundDiaName + "',");
            query.Append("'" + property.InboundDiaName + "',");
            query.Append(property.WidthOfStationNameField.ToString() + ",");
            query.Append(property.TimetableTrainWidth.ToString());
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= RouteTable::Insert(RouteProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(RouteProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteTable::Update(RouteProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("OutboundDiaName = '" + property.OutboundDiaName + "',");
            query.Append("InboundDiaName = '" + property.InboundDiaName + "',");
            query.Append("WidthOfStationNameField = " + property.WidthOfStationNameField.ToString() + ",");
            query.Append("TimetableTrainWidth = " + property.TimetableTrainWidth.ToString() + ",");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Name = '" + property.Name.ToString() + "';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= RouteTable::Update(RouteProperty)");
        }
        #endregion
    }
}
