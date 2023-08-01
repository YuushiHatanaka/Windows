using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Jsonイベント情報クラス
    /// </summary>
    public class JsonEventArg
    {
        /// <summary>
        /// 処理行データ
        /// </summary>
        public string Raw = string.Empty;

        /// <summary>
        /// 動作モード
        /// </summary>
        public JsonMode Mode = JsonMode.Non;

        /// <summary>
        /// 行数
        /// </summary>
        public uint Line = 0;

        /// <summary>
        /// カラム位置
        /// </summary>
        public uint Colum = 0;

        /// <summary>
        /// Array情報
        /// </summary>
        public JsonArrayInfo Array = new JsonArrayInfo();

        /// <summary>
        /// Key情報
        /// </summary>
        public JsonKeyInfo Key = new JsonKeyInfo();

        /// <summary>
        /// Value値種別
        /// </summary>
        public JsonValueType Type = JsonValueType.Unknown;

        /// <summary>
        /// Value値
        /// </summary>
        public string Value = string.Empty;

        /// <summary>
        /// エラー情報
        /// </summary>
        public JsonErrorInfo Error = new JsonErrorInfo(JsonError.NormalEnd);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonEventArg()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="arg"></param>
        public JsonEventArg(JsonEventArg arg)
        {
            // 初期化
            Raw = arg.Raw;
            Mode = arg.Mode;
            Line = arg.Line;
            Colum = arg.Colum;
            Array.Value = arg.Array.Value;
            Array.Key = arg.Array.Key;
            Key.StartColum = arg.Key.StartColum;
            Key.Value = arg.Key.Value;
            Type = arg.Type;
            Value = arg.Value;
            Error.Set(arg.Error.Get());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="mode"></param>
        /// <param name="line"></param>
        /// <param name="colum"></param>
        /// <param name="array"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="error"></param>
        public JsonEventArg(
            string raw,
            JsonMode mode,
            uint line,
            uint colum,
            JsonArrayInfo array,
            JsonKeyInfo key,
            JsonValueType type,
            string value,
            JsonErrorInfo error)
        {
            // 初期化
            Raw = raw;
            Mode = mode;
            Line = line;
            Colum = colum;
            Array.Value = array.Value;
            Array.Key = array.Key;
            Key.StartColum = key.StartColum;
            Key.Value = key.Value;
            Type = type;
            Value = value;
            Error.Set(error.Get());
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string _result = string.Empty;
            _result += "処理行データ             ：" + Raw.Trim('\r', '\n') + Environment.NewLine;
            _result += "動作モード               ：" + Mode + Environment.NewLine;
            _result += "行数                     ：" + Line + Environment.NewLine;
            _result += "カラム位置               ：" + Colum + Environment.NewLine;
            _result += "Array情報(インデックス値)：" + Array.Value + Environment.NewLine;
            _result += "Array情報(キー値)        ：" + Array.Key + Environment.NewLine;
            _result += "Key情報(キー開始位置)    ：" + Key.StartColum + Environment.NewLine;
            _result += "Key情報(キー値)          ：" + Key.Value + Environment.NewLine;
            _result += "Value値種別              ：" + Type + Environment.NewLine;
            _result += "Value値                  ：" + Value + Environment.NewLine;
            _result += "エラー内容               ：" + Error.ToString() + Environment.NewLine;
            return _result;
        }
    };
}
