using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Database.Table.Core;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table
{
    /// <summary>
    /// FileInfomationクラス
    /// </summary>
    public class FileInfomationTable : TablePropertyCore<FileInfomationProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public FileInfomationTable(SQLiteConnection connection)
            : base("FileInfomation", connection)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationTable::FileInfomationTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= FileInfomationTable::FileInfomationTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("RouteName TEXT,");        // 路線名
            query.Append("Version TEXT,");          // ファイルバージョン
            query.Append("ImportFileType TEXT,");   // インポートファイル種別
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(RouteName));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= FileInfomationTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref FileInfomationProperty result)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationTable::SelectDataRegston(SQLiteDataReader, FileInfomationProperty)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // 設定
            result.RouteName = sqliteDataReader["RouteName"].ToString();
            result.Version = sqliteDataReader["Version"].ToString();
            result.ImportFileType = sqliteDataReader["ImportFileType"].ToString();

            // ロギング
            Logger.Debug("<<<<= FileInfomationTable::SelectDataRegston(SQLiteDataReader, FileInfomationProperty)");
        }
        #endregion

        #region 再構築
        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="oldProperty"></param>
        /// <param name="newProperty"></param>
        public void Rebuilding(FileInfomationProperty oldProperty, FileInfomationProperty newProperty)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationTable::Rebuilding(FileInfomationProperty, FileInfomationProperty)");
            Logger.DebugFormat("oldProperty:[{0}]", oldProperty);
            Logger.DebugFormat("newProperty:[{0}]", newProperty);

            // データを読込
            FileInfomationProperty orignalProperties = Load();

            // 旧データと比較
            if (!orignalProperties.Compare(oldProperty))
            {
                // ロギング
                Logger.Warn("旧データ相違検出(FileInfomationTable)");
                Logger.Warn(oldProperty);
                Logger.Warn(orignalProperties);
            }

            // 比較
            if (!orignalProperties.Compare(newProperty))
            {
                // 保存
                Save(newProperty);
            }

            // ロギング
            Logger.Debug("<<<<= FileInfomationTable::Rebuilding(FileInfomationProperty, FileInfomationProperty)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(FileInfomationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationTable::Exist(FileInfomationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("RouteName = '" + property.RouteName + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FileInfomationTable::Exist(FileInfomationProperty)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(FileInfomationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationTable::Insert(FileInfomationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("RouteName,");     // 路線名
            query.Append("Version,");       // ファイルバージョン
            query.Append("ImportFileType"); // インポートファイル種別
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + property.RouteName + "',");
            query.Append("'" + property.Version + "',");
            query.Append("'" + property.ImportFileType + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= FileInfomationTable::Insert(FileInfomationProperty)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(FileInfomationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FileInfomationTable::Update(FileInfomationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("Version = '" + property.Version + "',");
            query.Append("ImportFileType = '" + property.ImportFileType + "',");
            query.Append("updated = '" + GetCurrentDateTime() + "'");
            query.Append("WHERE RouteName = '" + property.RouteName.ToString() + "';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= FileInfomationTable::Update(FileInfomationProperty)");
        }
        #endregion
    }
}
