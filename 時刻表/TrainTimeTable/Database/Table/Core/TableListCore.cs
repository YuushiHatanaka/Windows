using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Database.Table.Core
{
    /// <summary>
    /// TableListCoreクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableListCore<T, U> : TableCore where T : List<U>, new()
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        public TableListCore(string tableName, SQLiteConnection connection)
            : base(tableName, connection)
        {
            // ロギング
            Logger.Debug("=>>>> TableListCore::TableListCore(string, SQLiteConnection)");
            Logger.DebugFormat("tableName :[{0}]", tableName);
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TableListCore::TableListCore(string, SQLiteConnection)");
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="properties"></param>
        public virtual void Save(T properties)
        {
            // ロギング
            Logger.Debug("=>>>> TableListCore::Save(T)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リスト分繰り返す
            foreach (var property in properties)
            {
                // 存在判定
                if (!Exist(property))
                {
                    // 存在していない場合
                    Insert(property);
                }
                else
                {
                    // 存在している場合
                    Update(property);
                }
            }

            // ロギング
            Logger.Debug("<<<<= TableListCore::Save(properties)");
        }
        #endregion

        #region 読込
        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public T Load()
        {
            // ロギング
            Logger.Debug("=>>>> TableListCore::Load()");

            // 結果オブジェクト生成
            T result = new T();

            // SQLクエリ生成
            StringBuilder query = new StringBuilder();
            query.Append(string.Format("SELECT * FROM {0};", m_TableName));

            // クエリ実行
            using (SQLiteDataReader sqliteDataReader = Load(query.ToString()))
            {
                // 1行のみデータを取得
                while (sqliteDataReader.Read())
                {
                    // SELECTデータ登録
                    SelectDataRegston(sqliteDataReader, ref result);
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result.ToString());
            Logger.Debug("<<<<= TableListCore::Load()");

            // 返却
            return result;
        }

        /// <summary>
        /// SELECTデータ登録
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <param name="result"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void SelectDataRegston(SQLiteDataReader sqliteDataReader, ref T result)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected virtual bool Exist(T properties)
        {
            // 例外
            throw new NotImplementedException();
        }

        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual bool Exist(U property)
        {
            // 例外
            throw new NotImplementedException();
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="properties"></param>
        protected virtual void Insert(T properties)
        {
            // 例外
            throw new NotImplementedException();
        }

        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected virtual void Insert(U property)
        {
            // 例外
            throw new NotImplementedException();
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="properties"></param>
        protected virtual void Update(T properties)
        {
            // 例外
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        protected virtual void Update(U property)
        {
            // 例外
            throw new NotImplementedException();
        }
        #endregion

        #region 削除
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="properties"></param>
        protected virtual void Remove(T properties)
        {
            // 例外
            throw new NotImplementedException();
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="property"></param>
        protected virtual void Remove(U property)
        {
            // 例外
            throw new NotImplementedException();
        }
        #endregion
    }
}
