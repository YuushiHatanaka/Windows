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
    /// ColorTableクラス
    /// </summary>
    public class ColorTable : TableDictionaryCore<ColorProperties, string, ColorProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public ColorTable(SQLiteConnection connection)
            : base("Color", connection)
        {
            // ロギング
            Logger.Debug("=>>>> ColorTable::ColorTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= ColorTable::ColorTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> ColorTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Name TEXT,");                             // 使用名称
            query.Append("Alpha INTEGER DEFAULT 0,");               // Alpha値
            query.Append("R INTEGER DEFAULT 0,");                   // R値
            query.Append("G INTEGER DEFAULT 0,");                   // G値
            query.Append("B INTEGER DEFAULT 0,");                   // B値
            query.Append("Value TEXT NOT NULL DEFAULT '#000000',"); // 色値(ColorTranslator.FromHtml)
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Name));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= ColorTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref ColorProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> ColorTable::SelectDataRegston(SQLiteDataReader, ColorProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // ColorPropertyオブジェクト生成
            ColorProperty property = new ColorProperty();

            // 設定
            string name = sqliteDataReader["Name"].ToString();
            property.Alpha = int.Parse(sqliteDataReader["Alpha"].ToString());
            property.R = int.Parse(sqliteDataReader["R"].ToString());
            property.G = int.Parse(sqliteDataReader["G"].ToString());
            property.B = int.Parse(sqliteDataReader["B"].ToString());
            property.Value = ColorTranslator.FromHtml(sqliteDataReader["Value"].ToString());

            // 登録
            result.Add(name, property);

            // ロギング
            Logger.Debug("<<<<= ColorTable::SelectDataRegston(SQLiteDataReader, ColorProperties)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override bool Exist(KeyValuePair<string, ColorProperty> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> ColorTable::Exist(KeyValuePair<string, ColorProperty>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Name = '" + keyValuePair.Key + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.Debug("<<<<= ColorTable::Exist(KeyValuePair<string, ColorProperty>)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="keyValuePair"></param>
        protected override void Insert(KeyValuePair<string, ColorProperty> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> ColorTable::Insert(KeyValuePair<string, ColorProperty>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Name,");  // 使用名称
            query.Append("Alpha,"); // Alpha値
            query.Append("R,");     // R値
            query.Append("B,");     // B値
            query.Append("G,");     // G値
            query.Append("Value");  // 色値(ColorTranslator.FromHtml)
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + keyValuePair.Key + "',");
            query.Append(keyValuePair.Value.Alpha.ToString() + ",");
            query.Append(keyValuePair.Value.R.ToString() + ",");
            query.Append(keyValuePair.Value.B.ToString() + ",");
            query.Append(keyValuePair.Value.G.ToString() + ",");
            query.Append("'" + string.Format("#{0:X2}{1:X2}{2:X2}", keyValuePair.Value.R, keyValuePair.Value.G, keyValuePair.Value.B) + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= ColorTable::Insert(KeyValuePair<string, ColorProperty>)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="keyValuePair"></param>
        protected override void Update(KeyValuePair<string, ColorProperty> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> ColorTable::Update(KeyValuePair<string, ColorProperty>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("Alpha = '" + keyValuePair.Value.Alpha.ToString() + "',");
            query.Append("R = " + keyValuePair.Value.R.ToString() + ",");
            query.Append("G = '" + keyValuePair.Value.G.ToString() + "',");
            query.Append("B = '" + keyValuePair.Value.B.ToString() + "',");
            query.Append("Value = '" + string.Format("#{0:X2}{1:X2}{2:X2}", keyValuePair.Value.R, keyValuePair.Value.G, keyValuePair.Value.B) + "',");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Name = '" + keyValuePair.Key + "';");

            // ロギング
            Logger.Debug("<<<<= ColorTable::Update(KeyValuePair<string, ColorProperty>)");
        }
        #endregion
    }
}
