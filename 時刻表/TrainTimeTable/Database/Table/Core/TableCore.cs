using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table.Core
{
    /// <summary>
    /// TableCoreクラス
    /// </summary>
    public class TableCore
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region SQLiteConnectionオブジェクト
        /// <summary>
        /// SQLiteConnectionオブジェクト
        /// </summary>
        protected SQLiteConnection m_SqliteConnection = null;
        #endregion

        #region テーブル名
        /// <summary>
        /// テーブル名
        /// </summary>
        protected string m_TableName = string.Empty;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        public TableCore(string tableName, SQLiteConnection connection)
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::TableCore(string, SQLiteConnection)");
            Logger.DebugFormat("tableName :[{0}]", tableName);
            Logger.DebugFormat("connection:[{0}]", connection);

            // 設定
            m_TableName = tableName;
            m_SqliteConnection = connection;

            // ロギング
            Logger.Debug("<<<<= TableCore::TableCore(string, SQLiteConnection)");
        }
        #endregion

        #region 作成
        /// <summary>
        /// 作成
        /// </summary>
        /// <param name="query"></param>
        protected virtual void Create(string query)
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::Create(string)");
            Logger.DebugFormat("query:[{0}]", query);

            // SQLiteCommandオブジェクト作成
            using (SQLiteCommand sqliteCommand = new SQLiteCommand(m_SqliteConnection))
            {
                // SQLクエリ設定
                sqliteCommand.CommandText = query;

                // クエリ実行
                sqliteCommand.ExecuteNonQuery();

                // ロギング
                Logger.InfoFormat("データベース作成:[{0}]", query);
                Logger.Debug("<<<<= TableCore::Create(string)");
            }
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        protected virtual SQLiteDataReader Load(string query)
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::Load(string)");
            Logger.DebugFormat("query:[{0}]", query);

            // SQLiteCommandオブジェクト作成
            using (SQLiteCommand sqliteCommand = new SQLiteCommand(m_SqliteConnection))
            {
                // SQLクエリ設定
                sqliteCommand.CommandText = query;

                // クエリ実行
                SQLiteDataReader result = sqliteCommand.ExecuteReader();

                // ロギング
                Logger.DebugFormat("result:[{0}]", result);
                Logger.Debug("<<<<= TableCore::Load(string)");

                // 返却
                return result;
            }
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual bool Exist(string query)
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::Exist(string)");
            Logger.DebugFormat("query:[{0}]", query);

            // SQLiteCommandオブジェクト作成
            using (SQLiteCommand sqliteCommand = new SQLiteCommand(m_SqliteConnection))
            {
                // SQLクエリ設定
                sqliteCommand.CommandText = query;

                // ロギング
                Logger.DebugFormat("存在判定:[{0}]", query);

                // クエリ実行
                int count = Convert.ToInt32(sqliteCommand.ExecuteScalar());

                // 件数判定
                if (count != 0)
                {
                    // ロギング
                    Logger.DebugFormat("count:[{0}]", count);
                    Logger.Debug("<<<<= TableCore::Exist(string)");

                    // 該当あり
                    return true;
                }

                // ロギング
                Logger.DebugFormat("count:[{0}]", count);
                Logger.Debug("<<<<= TableCore::Exist(string)");

                // 該当なし
                return false;
            }
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="query"></param>
        protected virtual void Insert(string query)
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::Insert(string)");
            Logger.DebugFormat("query:[{0}]", query);

            // SQLiteCommandオブジェクト作成
            using (SQLiteCommand sqliteCommand = new SQLiteCommand(m_SqliteConnection))
            {
                // SQLクエリ設定
                sqliteCommand.CommandText = query;

                // クエリ実行
                sqliteCommand.ExecuteNonQuery();

                // ロギング
                Logger.InfoFormat("データベース挿入:[{0}]", query);
            }

            // ロギング
            Logger.Debug("<<<<= TableCore::Insert(string)");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="query"></param>
        protected virtual void Update(string query)
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::Update(string)");
            Logger.DebugFormat("query:[{0}]", query);

            // SQLiteCommandオブジェクト作成
            using (SQLiteCommand sqliteCommand = new SQLiteCommand(m_SqliteConnection))
            {
                // SQLクエリ設定
                sqliteCommand.CommandText = query;

                // クエリ実行
                sqliteCommand.ExecuteNonQuery();

                // ロギング
                Logger.InfoFormat("データベース更新:[{0}]", query);
            }

            // ロギング
            Logger.Debug("<<<<= TableCore::Update(string)");
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        public virtual void Remove()
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::Remove()");

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("DELETE FROM {0};", m_TableName));

            // 削除
            Remove(query.ToString());

            // ロギング
            Logger.Debug("<<<<= TableCore::Remove()");
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="query"></param>
        protected virtual void Remove(string query)
        {
            // ロギング
            Logger.Debug("=>>>> TableLibrary::Remove(string)");

            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                // SQLiteCommandオブジェクト作成
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(m_SqliteConnection))
                {
                    // SQLクエリ設定
                    sqliteCommand.CommandText = query;

                    // クエリ実行
                    sqliteCommand.ExecuteNonQuery();

                    // ロギング
                    Logger.InfoFormat("データベース削除:[{0}]", query);
                }

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= TableLibrary::Remove()");
        }
        #endregion

        #region 現在日時文字列取得
        /// <summary>
        /// 現在日時文字列取得
        /// </summary>
        /// <returns></returns>
        public string GetCurrentDateTime()
        {
            // ロギング
            Logger.Debug("=>>>> TableCore::GetCurrentDateTime()");

            // 結果オブジェクト設定
            string result = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TableCore::GetCurrentDateTime()");

            // 返却
            return result;
        }
        #endregion
    }
}
