using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Key情報スタッククラス
    /// </summary>
    public class JsonKeyInfoStack : JsonStack<JsonKeyInfo>
    {
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        public JsonKeyInfoStack()
        {
        }

        /// <summary>
        /// カレント情報
        /// </summary>
        /// <returns></returns>
        public JsonKeyInfo Current()
        {
            if (Count() <= 0)
            {
                return new JsonKeyInfo();
            }
            return this[Count() - 1];
        }

        /// <summary>
        /// 現在状態文字列取得
        /// </summary>
        public override string ToString()
        {
            if (Count() <= 0)
            {
                return "Stackなし";
            }
            return ToString(Get());
        }

        /// <summary>
        /// 状態文字列取得
        /// </summary>
        public string ToString(JsonKeyInfo info)
        {
            string _current_key_info;
            _current_key_info = "＜キー情報＞";
            _current_key_info += "　開始カラム：" + info.StartColum.ToString() + Environment.NewLine;
            _current_key_info += "　キー値　　：" + info.Value + Environment.NewLine;
            return _current_key_info;
        }
    };
}
