using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Arrayインデックススタッククラス
    /// </summary>
    public class JsonArrayIndexStack : JsonStack<JsonArrayInfo>
    {
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        public JsonArrayIndexStack()
        {
        }

        /// <summary>
        /// インクリメント
        /// </summary>
        public int Increment()
        {
            // サイズ判定
            if (Count() < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 現在のインデックス値を取得
            int _current_index = Get().Value;

            // 現在のインデックス値を更新
            this[Count() - 1].Value = _current_index + 1;

            // 設定値を返却
            return _current_index + 1;
        }

        /// <summary>
        /// デクリメント
        /// </summary>
        /// <returns></returns>
        public int Decrement()
        {
            // サイズ判定
            if (Count() < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 現在のインデックス値を取得
            int _current_index = Get().Value;

            // 現在のインデックス値を更新
            this[Count() - 1].Value = _current_index - 1;

            // 設定値を返却
            return _current_index - 1;
        }

        /// <summary>
        /// カレント情報
        /// </summary>
        public string Current()
        {
            // サイズ判定
            if (Count() < 1)
            {
                // インデックスなし
                return "";
            }

            // 現在のインデックス値を取得
            int _current_index = Get().Value;

            // 文字列を返却
            return _current_index.ToString();
        }

        /// <summary>
        /// 現在状態文字列取得
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // サイズ判定
            if (Count() < 1)
            {
                // インデックスなし
                return "Stackなし";
            }

            JsonArrayInfo _array_info = Get();
            string _current_array_index;
            _current_array_index = _array_info.Key + ":" + _array_info.Value.ToString();

            // 文字列を返却
            return _current_array_index;
        }
    };
}
