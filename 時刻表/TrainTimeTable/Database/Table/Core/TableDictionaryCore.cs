using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Database.Table.Core
{
    /// <summary>
    /// TableDictionaryCoreクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableDictionaryCore<T, K, V> : TableCore where T : Dictionary<K, V>, new()
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        public TableDictionaryCore(string tableName, SQLiteConnection connection)
            : base(tableName, connection)
        {
            // ロギング
            Logger.Debug("=>>>> TableDictionaryCore::TableDictionaryCore(string, SQLiteConnection)");
            Logger.DebugFormat("tableName :[{0}]", tableName);
            Logger.DebugFormat("connection:[{0}]", connection);

            // ロギング
            Logger.Debug("<<<<= TableDictionaryCore::TableDictionaryCore(string, SQLiteConnection)");
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
            Logger.Debug("=>>>> TableDictionaryCore::Save(T)");
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
            Logger.Debug("<<<<= TableDictionaryCore::Save(properties)");
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
            Logger.Debug("=>>>> TableDictionaryCore::Load()");

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
            Logger.Debug("<<<<= TableDictionaryCore::Load()");

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

        #region 再構築
        /// <summary>
        /// 再構築
        /// </summary>
        /// <param name="properties"></param>
        public virtual void Rebuilding(T properties)
        {
            // ロギング
            Logger.Debug("=>>>> TableDictionaryCore::Rebuilding(T)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // データを読込
            T orignalProperties = Load();

            // 削除対象キーを取得
            T removeKeys = GetRemoveKeys(orignalProperties, properties);

            // 削除
            Remove(removeKeys);

            // 保存
            Save(properties);

            // ロギング
            Logger.Debug("<<<<= TableDictionaryCore::Rebuilding(T)");
        }
        #endregion

        #region 削除キー取得
        /// <summary>
        /// 削除キー取得
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual T GetRemoveKeys(T src, T dst)
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
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual bool Exist(KeyValuePair<K, V> key)
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
            // ロギング
            Logger.Debug("=>>>> TableDictionaryCore::Insert(T)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                // リスト分繰り返す
                foreach (var property in properties)
                {
                    // 挿入
                    Insert(property);
                }

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= TableDictionaryCore::Insert(T)");
        }

        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="property"></param>

        protected virtual void Insert(KeyValuePair<K, V> property)
        {
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
            // ロギング
            Logger.Debug("=>>>> TableDictionaryCore::Update(T)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // トランザクション開始
            using (SQLiteTransaction sqliteTransaction = m_SqliteConnection.BeginTransaction())
            {
                // リスト分繰り返す
                foreach (var property in properties)
                {
                    // 挿入
                    Update(property);
                }

                // トランザクションコミット
                sqliteTransaction.Commit();
            }

            // ロギング
            Logger.Debug("<<<<= TableDictionaryCore::Update(T)");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>

        protected virtual void Update(KeyValuePair<K, V> property)
        {
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
        #endregion
    }
}
