using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.Database.Table.Core
{
    /// <summary>
    /// TablePropertyCoreクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TablePropertyCore<T> : TableCore where T : new()
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        public TablePropertyCore(string tableName, SQLiteConnection connection)
            : base(tableName, connection)
        {
            // ロギング
            Logger.Debug("=>>>> TablePropertyCore::TablePropertyCore(string, SQLiteConnection)");
            Logger.DebugFormat("tableName :[{0}]", tableName);
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TablePropertyCore::TablePropertyCore(string, SQLiteConnection)");
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="property"></param>
        public virtual void Save(T property)
        {
            // ロギング
            Logger.Debug("=>>>> TablePropertyCore::Save(T)");
            Logger.DebugFormat("property:[{0}]", property);

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

            // ロギング
            Logger.Debug("<<<<= TablePropertyCore::Save(properties)");
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
            Logger.Debug("=>>>> TablePropertyCore::Load()");

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
            Logger.Debug("<<<<= TablePropertyCore::Load()");

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
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual bool Exist(T property)
        {
            // 例外
            throw new NotImplementedException();
        }
        #endregion

        #region 挿入
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>
        protected virtual void Insert(T property)
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
        protected virtual void Update(T property)
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
        protected virtual void Remove(T property)
        {
            // 例外
            throw new NotImplementedException();
        }
        #endregion
    }
}
