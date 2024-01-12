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
    /// CommentTableクラス
    /// </summary>
    public class CommentTable : TablePropertyCore<StringBuilder>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public CommentTable(SQLiteConnection connection)
            : base("Comment", connection)
        {
            // ロギング
            Logger.Debug("=>>>> CommentTable::CommentTable(SQLiteConnection)");
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= CommentTable::CommentTable(SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
        {
            // ロギング
            Logger.Debug("=>>>> CommentTable::Create()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", m_TableName));
            query.Append("Key TEXT,");      // 仮キー文字
            query.Append("Comment TEXT,");  // コメント本文
            query.Append("created TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("updated TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime')),");
            query.Append("deleted TIMESTAMP,");
            query.Append("PRIMARY KEY(Key));");

            // 作成
            Create(query.ToString());

            // ロギング
            Logger.Debug("<<<<= CommentTable::Create()");
        }
        #endregion

        #region SELECTデータ登録
        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        protected override void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref StringBuilder result)
        {
            // ロギング
            Logger.Debug("=>>>> CommentTable::SelectDataRegston(SQLiteDataReader, StringBuilder)");
            Logger.DebugFormat("sqliteDataReader:[{0}]", sqliteDataReader);
            Logger.DebugFormat("result          :[{0}]", result);

            // 設定
            result.Append(sqliteDataReader["Comment"].ToString());

            // ロギング
            Logger.Debug("<<<<= CommentTable::SelectDataRegston(SQLiteDataReader, StringBuilder)");
        }
        #endregion

        #region 再構築
        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="oldComment"></param>
        /// <param name="newComment"></param>
        public void Rebuilding(StringBuilder oldComment, StringBuilder newComment)
        {
            // ロギング
            Logger.Debug("=>>>> CommentTable::Rebuilding(StringBuilder, StringBuilder)");
            Logger.DebugFormat("oldComment:[{0}]", oldComment);
            Logger.DebugFormat("newComment:[{0}]", newComment);

            // データを読込
            StringBuilder orignalComment = Load();

            // 旧データと比較
            if (orignalComment.ToString() != oldComment.ToString())
            {
                // ロギング
                Logger.Warn("旧データ相違検出(CommentTable)");
                Logger.Warn(oldComment.ToString());
                Logger.Warn(orignalComment.ToString());
            }

            // 比較
            if (orignalComment.ToString() != newComment.ToString())
            {
                // 保存
                Save(newComment);
            }

            // ロギング
            Logger.Debug("<<<<= CommentTable::Rebuilding(StringBuilder)");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool Exist(StringBuilder property)
        {
            // ロギング
            Logger.Debug("=>>>> CommentTable::Exist(StringBuilder)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE ", m_TableName));
            query.Append("Key = '1';");

            // 存在判定
            bool result = Exist(query.ToString());

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= CommentTable::Exist(StringBuilder)");

            // 返却
            return result;
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected override void Insert(StringBuilder property)
        {
            // ロギング
            Logger.Debug("=>>>> CommentTable::Insert(StringBuilder)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("INSERT INTO {0} ", m_TableName));
            query.Append("(");
            query.Append("Key,");       // 仮キー文字
            query.Append("Comment");    // コメント本文
            query.Append(") VALUES ");
            query.Append("(");
            query.Append("'1',");
            query.Append("'" + property.ToString() + "'");
            query.Append(");");

            // 挿入
            Insert(query.ToString());

            // ロギング
            Logger.Debug("<<<<= CommentTable::Insert(StringBuilder)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected override void Update(StringBuilder property)
        {
            // ロギング
            Logger.Debug("=>>>> CommentTable::Update(StringBuilder)");
            Logger.DebugFormat("property:[{0}]", property);

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("UPDATE {0} SET ", m_TableName));
            query.Append("Comment = '" + property.ToString() + "',");
            query.Append("updated = '" + GetCurrentDateTime() + "' ");
            query.Append("WHERE Key = '1';");

            // 更新
            Update(query.ToString());

            // ロギング
            Logger.Debug("<<<<= CommentTable::Update(StringBuilder)");
        }
        #endregion
    }
}
