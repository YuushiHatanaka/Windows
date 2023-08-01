using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// モードスタッククラス
    /// </summary>
    public class JsonModeStack : JsonStack<JsonMode>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonModeStack()
        {
            base.Push(JsonMode.Non);
        }

        /// <summary>
        /// 親取得
        /// </summary>
        /// <returns></returns>
        public JsonMode Parent()
        {
            // サイズを判定
            if (Count() < 2)
            {
                return JsonMode.Non;
            }

            // 1つ前を取得
            JsonMode _mode = this[Count() - 2];

            // モードを返却
            return _mode;
        }

        /// <summary>
        /// 現在状態文字列取得
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Count() <= 0)
            {
                return "Stackなし";
            }
            return ToString(base.Get());
        }

        /// <summary>
        /// 状態文字列取得
        /// </summary>
        /// <returns></returns>
        public string ToString(JsonMode mode)
        {
            string _current_mode = "";
            switch (mode)
            {
                case JsonMode.Non: _current_mode = "設定なし"; break;
                case JsonMode.Object: _current_mode = "Objectモード"; break;
                case JsonMode.Array: _current_mode = "Arrayモード"; break;
            }
            return _current_mode;
        }
    };
}
