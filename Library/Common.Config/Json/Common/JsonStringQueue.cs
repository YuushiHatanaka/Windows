using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Json文字列Queueクラス
    /// </summary>
    public class JsonStringQueue
    {
        /// <summary>
        /// 処理中カラム
        /// </summary>
        private uint m_column_no;

        /// <summary>
        /// 文字列キュー
        /// </summary>
        private List<string> m_queue = new List<string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonStringQueue()
        {
            // クリア
            Clear();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="str"></param>
        public JsonStringQueue(string str)
        {
            // 設定
            Set(str);
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JsonStringQueue()
        {
            // クリア
            Clear();
        }

        /// <summary>
        /// 終端判定
        /// </summary>
        /// <returns></returns>
        public bool Eof()
        {
            // サイズ判定
            if (m_queue.Count <= 0)
            {
                // サイズが0未満ならtrueを返却
                return true;
            }
            else
            {
                // サイズが0以外ならfalseを返却
                return false;
            }
        }

        /// <summary>
        /// 文字列長取得
        /// </summary>
        /// <returns></returns>
        public uint Length()
        {
            // 0バイトの場合判定しない
            if (m_queue.Count <= 0)
            {
                return 0;
            }

            // 文字列
            List<string> _tmp_queue = new List<string>(m_queue);
            uint i = 0;

            // 1文字ずつ処理する
            while (_tmp_queue.Count > 0)
            {
                // 先頭文字取得
                string _result = _tmp_queue[0];

                // 改行の場合はその場でサイズを返却する
                if (_result == "\r")
                {
                    return i;
                }
                else if (_result == "\n")
                {
                    return i;
                }

                // 先頭文字(取得文字)削除
                _tmp_queue.RemoveAt(0);
                i++;
            }

            // 改行が最後までなかったら全体のサイズを返却する
            return (uint)m_queue.Count;
        }

        /// <summary>
        /// 処理中カラム
        /// </summary>
        /// <returns></returns>
        public uint CurrentColum()
        {
            // 処理中カラムを返却
            return m_column_no;
        }

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="str"></param>
        public void Set(string str)
        {
            // 設定
            m_column_no = 0;

            // クリア
            Clear();

            // 文字列設定
            for (uint i = 0; i < str.Length; i++)
            {
                // 1文字ずつキューに保存
                m_queue.Add(str.Substring((int)i, 1));
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 文字列キューをクリア
            m_queue.Clear();
        }

        /// <summary>
        /// 空白スキップ
        /// </summary>
        public void SkipBlank()
        {
            // 1文字ずつ処理する
            while (m_queue.Count > 0)
            {
                // 空白の場合
                if (m_queue[0] == " " || m_queue[0] == "\t")
                {
                    // 先頭文字削除
                    m_queue.RemoveAt(0);

                    // スキップ
                    m_column_no++;
                    continue;
                }
                break;
            }
            return;
        }

        /// <summary>
        /// 1文字取得
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            // カラム数更新
            m_column_no++;

            // 先頭文字取得
            string _result = m_queue[0];

            // 先頭文字(取得文字)削除
            m_queue.RemoveAt(0);

            // 文字返却
            return _result;
        }

        /// <summary>
        /// 文字追加
        /// </summary>
        /// <param name="str"></param>
        public void Put(string str)
        {
            // 文字数分繰り返し
            for (uint i = 0; i < str.Length; i++)
            {
                // １文字キューに戻す
                m_queue.Insert(0, str.Substring((int)i, 1));

                // カラム数更新
                m_column_no--;
            }
        }
    };
}
