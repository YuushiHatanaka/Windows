using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// Jsonエラー情報クラス
    /// </summary>
    public class JsonErrorInfo
    {
        /// <summary>
        /// エラーコード
        /// </summary>
        private JsonError m_error;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonErrorInfo()
        {
            // 初期化
            Set(JsonError.NormalEnd);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="error"></param>
        public JsonErrorInfo(JsonError error)
        {
            // 初期化
            Set(error);
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JsonErrorInfo()
        {

        }

        /// <summary>
        /// 値設定
        /// </summary>
        /// <param name="error"></param>
        public void Set(JsonError error)
        {
            // 初期化
            m_error = error;
        }

        /// <summary>
        /// 値取得
        /// </summary>
        /// <returns></returns>
        public JsonError Get()
        {
            // エラーコードを返却
            return m_error;
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        public override string ToString()
        {
            string _error_string = "";
            switch (m_error)
            {
                case JsonError.NormalEnd: _error_string = "Normal End"; break;
                case JsonError.NotFoundStartTag: _error_string = "Not Found Start Tag"; break;
                case JsonError.NotFindEndTag: _error_string = "Not Found End Tag"; break;
                case JsonError.NotFoundKey: _error_string = "Not Found Key"; break;
                case JsonError.NotFoundValue: _error_string = "Not Found Value"; break;
                case JsonError.InvalidFormat: _error_string = "Invalid Format"; break;
                case JsonError.InvalidSyntax: _error_string = "Invalid Syntax"; break;
                case JsonError.OutOfMemory: _error_string = "Out Of Memory"; break;
                case JsonError.StackOverflow: _error_string = "Stack Overflow"; break;
                case JsonError.CannotOpenFile: _error_string = "Cannot Open File"; break;
                case JsonError.InvalidArgument: _error_string = "Invalid Argument"; break;
                case JsonError.InvalidUtf8: _error_string = "Invalid Utf8"; break;
                case JsonError.PrematureEndOfInput: _error_string = "Premature End Of Input"; break;
                case JsonError.EndOfInputExpected: _error_string = "End_of Input Expected "; break;
                case JsonError.WrongType: _error_string = "Wrong Type"; break;
                case JsonError.NullCharacter: _error_string = "Null Character"; break;
                case JsonError.NullValue: _error_string = "Null Value"; break;
                case JsonError.NullByteInKey: _error_string = "Null Byte In Key"; break;
                case JsonError.DuplicateKey: _error_string = "Duplicate Key"; break;
                case JsonError.NumericOverflow: _error_string = "Numeric Overflow"; break;
                case JsonError.ItemNotFound: _error_string = "Item Not Found"; break;
                case JsonError.IndexOutOfRange: _error_string = "Index Out Of Range"; break;
                case JsonError.SystemError: _error_string = "System Error"; break;
            }
            return _error_string;
        }
    };
}
