using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Data.SQLite;

namespace Common.Data.SQLite
{
    /// <summary>
    /// SQLiteライブラリクラス
    /// </summary>
    public class SQLite : IDisposable
    {
        #region ファイル名
        /// <summary>
        /// ファイル名
        /// </summary>
        private string m_FileName = string.Empty;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName
        {
            get
            {
                return this.m_FileName;
            }
        }
        #endregion

        /// <summary>
        /// SQLiteConnectionオブジェクト
        /// </summary>
        protected SQLiteConnection m_SQLiteConnection = null;

        /// <summary>
        /// Disposeフラグ
        /// </summary>
        private bool m_Disposed = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public SQLite(string fileName)
        {
            // 設定
            this.m_FileName = fileName;
        }

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            //マネージリソースおよびアンマネージリソースの解放
            this.Dispose(true);

            //ガベコレから、このオブジェクトのデストラクタを対象外とする
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="p_Disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Disposeフラグを判定
                if (this.m_Disposed)
                {
                    //既に呼出済みであるならばなにもしない
                    return;
                }

                // Disposeフラグを設定
                this.m_Disposed = true;

                // パラメータ判定
                if (disposing)
                {
                    // マネージリソースの解放
                    if (this.m_SQLiteConnection != null)
                    {
                        // SQLiteConnectionオブジェクト解放
                        this.m_SQLiteConnection.Dispose();
                        this.m_SQLiteConnection = null;
                    }
                }
            }
        }
        #endregion

        #region 結果取得
        /// <summary>
        /// 結果取得
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public List<string[]> GetResult(SQLiteCommand command)
        {
            // 結果オブジェクト生成
            List<string[]> _Result = new List<string[]>();

            // SQLiteDataReaderオブジェクト取得
            using (SQLiteDataReader _SQLiteDataReader = command.ExecuteReader())
            {
                for (int _Recoad = 0; _SQLiteDataReader.Read(); _Recoad++)
                {
                    string[] _Columns = new string[_SQLiteDataReader.FieldCount];
                    for (int _Colum = 0; _Colum < _SQLiteDataReader.FieldCount; _Colum++)
                    {
                        _Columns[_Colum] = _SQLiteDataReader[_Colum].ToString();
                    }

                    // 結果に設定
                    _Result.Add(_Columns);
                }
            }

            // 結果を返却する
            return _Result;
        }

        /// <summary>
        /// 結果取得
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public List<Dictionary<string,object>> GetResultColums(SQLiteCommand command)
        {
            // 結果オブジェクト生成
            List<Dictionary<string, object>> _Result = new List<Dictionary<string, object>>();

            // SQLiteDataReaderオブジェクト取得
            using (SQLiteDataReader _SQLiteDataReader = command.ExecuteReader())
            {
                for (int _Recoad = 0; _SQLiteDataReader.Read(); _Recoad++)
                {
                    Dictionary<string, object> _Colums = new Dictionary<string, object>();

                    for (int _Colum = 0; _Colum < _SQLiteDataReader.FieldCount; _Colum++)
                    {
                        _Colums.Add(_SQLiteDataReader.GetName(_Colum), _SQLiteDataReader[_Colum]);
                    }

                    // 結果に設定
                    _Result.Add(_Colums);
                }
            }

            // 結果を返却する
            return _Result;
        }
        #endregion
    }
}
