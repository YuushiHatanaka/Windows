using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Jsonスタックテンプレートクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonStack<T>
    {
        /// <summary>
        /// モードスタック
        /// </summary>
        private List<T> m_stack = new List<T>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonStack()
        {
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JsonStack()
        {
            // スタックを解放
            m_stack.Clear();
        }

        /// <summary>
        /// インデクサー[]
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[uint i]
        {
            get
            {
                // サイズ判定
                if (m_stack.Count < i + 1)
                {
                    // 異常終了(例外)
                    throw new IndexOutOfRangeException();
                }
                return m_stack[(int)i];
            }
            set
            {
                // サイズ判定
                if (m_stack.Count < i + 1)
                {
                    // 異常終了(例外)
                    throw new IndexOutOfRangeException();
                }
                m_stack[(int)i] = value;
            }
        }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <returns></returns>
        public uint Count()
        {
            return (uint)m_stack.Count;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            // サイズ判定
            if (m_stack.Count < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 動作モード返却
            return m_stack[m_stack.Count - 1];
        }

        /// <summary>
        /// 取得(1つ前)
        /// </summary>
        /// <returns></returns>
        T Previous()
        {
            // サイズ判定
            if (m_stack.Count < 2)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 動作モード返却
            return m_stack[m_stack.Count - 2];
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Push(T value)
        {
            // モードスタック設定
            m_stack.Add(value);

            // 正常終了
            return true;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <returns></returns>
        public bool Pop()
        {
            // サイズ判定
            if (m_stack.Count < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // モードスタック解放
            m_stack.RemoveAt(m_stack.Count - 1);

            // 正常終了
            return true;
        }
    };
}
