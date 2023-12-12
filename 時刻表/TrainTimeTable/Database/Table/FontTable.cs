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
    /// FontTableクラス
    /// </summary>
    public class FontTable : TableDictionaryCore<FontProperties, string, FontProperty>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public FontTable(SQLiteConnection connection)
            : base("Font", connection)
        {
            // ロギング
            Logger.Debug("=>>>> FontTable::FontTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= FontTable::FontTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> FontTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Name TEXT,");                         // 使用名称
            query.Append("FontFamilyName TEXT,");               // フォント名
            query.Append("Size REAL DEFAULT 9.0,");             // フォントサイズ
            query.Append("Bold TEXT DEFAULT 'False',");         // ボールドフラグ
            query.Append("Itaric TEXT DEFAULT 'False',");       // イタリックフラグ
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Name));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= FontTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref FontProperties result)
        {
            // ロギング
            Logger.Debug("=>>>> FontTable::SelectDataRegston(SQLiteDataReader, FontProperties)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // FontPropertyオブジェクト生成
            FontProperty property = new FontProperty();

            // 設定
            string name = sqliteDataReader["Name"].ToString();
            property.Name = sqliteDataReader["FontFamilyName"].ToString();
            property.Size = float.Parse(sqliteDataReader["Size"].ToString());
            bool bold = bool.Parse(sqliteDataReader["Bold"].ToString());
            bool itaric = bool.Parse(sqliteDataReader["Itaric"].ToString());
            if (bold) { property.FontStyle |= FontStyle.Bold; }
            if (itaric) { property.FontStyle |= FontStyle.Italic; }

            // 登録
            result.Add(name, property);

            // ロギング
            Logger.Debug("<<<<= FontTable::SelectDataRegston(SQLiteDataReader, FontProperties)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override bool Exist(KeyValuePair<string, FontProperty> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> FontTable::Exist(KeyValuePair<string, FontProperty>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Name = '" + keyValuePair.Key + "';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.Debug("<<<<= FontTable::Exist(KeyValuePair<string, FontProperty>)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="keyValuePair"></param>
        protected override void Insert(KeyValuePair<string, FontProperty> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> FontTable::Insert(KeyValuePair<string, FontProperty>)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Name,");          // 使用名称
            query.Append("FontFamilyName,");// フォント名
            query.Append("Size,");          // フォントサイズ
            query.Append("Bold,");          // ボールドフラグ
            query.Append("Itaric");         // イタリックフラグ
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'" + keyValuePair.Key + "',");
            query.Append("'" + keyValuePair.Value.Name + "',");
            query.Append(keyValuePair.Value.Size.ToString() + ",");
            query.Append("'" + keyValuePair.Value.Bold.ToString() + "',");
            query.Append("'" + keyValuePair.Value.Itaric.ToString() + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= FontTable::Insert(KeyValuePair<string, FontProperty>)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="keyValuePair"></param>
        protected override void Update(KeyValuePair<string, FontProperty> keyValuePair)
        {
            // ロギング
            Logger.Debug("=>>>> FontTable::Update(KeyValuePair<string, FontProperty)");
            Logger.DebugFormat("keyValuePair:[{0}]", keyValuePair);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("FontFamilyName = '" + keyValuePair.Value.Name + "',");
            query.Append("Size = " + keyValuePair.Value.Size.ToString() + ",");
            query.Append("Bold = '" + keyValuePair.Value.Bold.ToString() + "',");
            query.Append("Itaric = '" + keyValuePair.Value.Itaric.ToString() + "',");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Name = '" + keyValuePair.Key + "';");

            // ロギング
            Logger.Debug("<<<<= FontTable::Update(KeyValuePair<string, FontProperty)");
        }
        #endregion
    }
}
