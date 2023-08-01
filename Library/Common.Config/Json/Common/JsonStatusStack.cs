using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// 遷移状態スタッククラス
    /// </summary>
    public class JsonStatusStack : JsonStack<JsonStatus>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonStatusStack()
        {
            base.Push(JsonStatus.NotStart);
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
        public string ToString(JsonStatus status)
        {
            string _current_status = "";
            switch (status)
            {
                case JsonStatus.NotStart: _current_status = "解析未実施"; break;
                case JsonStatus.StartObject: _current_status = "Object開始"; break;
                case JsonStatus.NextObject: _current_status = "次Object開始"; break;
                case JsonStatus.Object: _current_status = "Object処理"; break;
                case JsonStatus.EndObject: _current_status = "Object終了"; break;
                case JsonStatus.StartArray: _current_status = "Array開始"; break;
                case JsonStatus.NextArray: _current_status = "次Array処理"; break;
                case JsonStatus.EndArray: _current_status = "Array終了"; break;
                case JsonStatus.EndKey: _current_status = "Key終了"; break;
                case JsonStatus.StartValue: _current_status = "Value開始"; break;
                case JsonStatus.Value: _current_status = "Value処理"; break;
                case JsonStatus.EndValue: _current_status = "Value終了"; break;
                case JsonStatus.EndArrayValue: _current_status = "Value終了(Array)"; break;
                case JsonStatus.NextWait: _current_status = "次遷移待ち"; break;
                case JsonStatus.End: _current_status = "解析終了"; break;
            }
            return _current_status;
        }
    };
}
