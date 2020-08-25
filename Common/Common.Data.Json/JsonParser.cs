using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Common.Data.Json
{
    /// <summary>
    /// 解析状態
    /// </summary>
    public enum JsonStatus : byte
    {
        NotStart = 0,                // 解析未実施
        StartObject,                 // Object開始
        NextObject,                  // 次Object開始
        Object,                      // Object処理
        EndObject,                   // Object終了
        StartArray,                  // Array開始
        NextArray,                   // 次Array処理
        EndArray,                    // Array終了
        EndKey,                      // Key終了
        StartValue,                  // Value開始
        Value,                       // Value処理
        EndValue,                    // Value終了
        EndArrayValue,               // Value終了(Array)
        NextWait,                    // 次遷移待ち
        End                          // 解析終了
    };

    /// <summary>
    /// 動作モード
    /// </summary>
    public enum JsonMode
    {
        Non = 0,                    // 設定なし
        Object,                     // Objectモード
        Array,                      // Arrayモード
    };

    /// <summary>
    /// Value種別
    /// </summary>
    public enum JsonValueType
    {
        Unknown = 0,                // 不明
        ObjectStart,                // object開始
        Object,                     // object
        ObjectEnd,                  // object終了
        ArrayStart,                 // array開始
        Array,                      // array
        ArrayEnd,                   // array終了
        Element,                    // element
        Value,                      // value
        String,                     // string
        Number,                     // number
        Bool,                       // true/false
        Null,                       // null
    };

    /// <summary>
    /// エラーコード
    /// </summary>
    public enum JsonError
    {
        NormalEnd = 0,                // 正常終了
        NotFoundStartTag,             // 開始タグなしエラー
        NotFindEndTag,                // 終了タグなしエラー
        NotFoundKey,                  // Keyなしエラー
        NotFoundValue,                // Valueなしエラー
        InvalidFormat,                // フォーマットエラー
        InvalidSyntax,                // 構文エラー
        OutOfMemory,
        StackOverflow,
        CannotOpenFile,
        InvalidArgument,
        InvalidUtf8,
        PrematureEndOfInput,
        EndOfInputExpected,
        WrongType,
        NullCharacter,
        NullValue,
        NullByteInKey,
        DuplicateKey,
        NumericOverflow,
        ItemNotFound,
        IndexOutOfRange,              // インデックス範囲エラー
        SystemError                   // システムエラー
    };

    /// <summary>
    /// JsonArray情報クラス
    /// </summary>
    public class JsonArrayInfo
    {
        /// <summary>
        /// インデックス値
        /// </summary>
        private int m_Value = 0;

        /// <summary>
        /// キー値
        /// </summary>
        private string m_Key = string.Empty;

        /// <summary>
        /// インデックス値
        /// </summary>
        public int Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                this.m_Value = value;
            }
        }

        /// <summary>
        /// キー値
        /// </summary>
        public string Key
        {
            get
            {
                return this.m_Key;
            }
            set
            {
                this.m_Key = value;
            }
        }
    }

    /// <summary>
    /// Jsonキー情報クラス
    /// </summary>
    public class JsonKeyInfo
    {
        /// <summary>
        /// キー開始位置
        /// </summary>
        private uint m_StartColum = 0;

        /// <summary>
        /// キー値
        /// </summary>
        private string m_Value = string.Empty;

        /// <summary>
        /// キー開始位置
        /// </summary>
        public uint StartColum
        {
            get
            {
                return this.m_StartColum;
            }
            set
            {
                this.m_StartColum = value;
            }
        }

        /// <summary>
        /// キー値
        /// </summary>
        public string Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                this.m_Value = value;
            }
        }
    }

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
            this.m_stack.Clear();
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
                if (this.m_stack.Count < i + 1)
                {
                    // 異常終了(例外)
                    throw new IndexOutOfRangeException();
                }
                return this.m_stack[(int)i];
            }
            set
            {
                // サイズ判定
                if (this.m_stack.Count < i + 1)
                {
                    // 異常終了(例外)
                    throw new IndexOutOfRangeException();
                }
                this.m_stack[(int)i] = value;
            }
        }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <returns></returns>
        public uint Count()
        {
            return (uint)this.m_stack.Count;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            // サイズ判定
            if (this.m_stack.Count < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 動作モード返却
            return this.m_stack[this.m_stack.Count - 1];
        }

        /// <summary>
        /// 取得(1つ前)
        /// </summary>
        /// <returns></returns>
        T Previous()
        {
            // サイズ判定
            if (this.m_stack.Count < 2)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 動作モード返却
            return this.m_stack[this.m_stack.Count - 2];
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Push(T value)
        {
            // モードスタック設定
            this.m_stack.Add(value);

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
            if (this.m_stack.Count < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // モードスタック解放
            this.m_stack.RemoveAt(this.m_stack.Count - 1);

            // 正常終了
            return true;
        }
    };

    /// <summary>
    /// 遷移状態スタッククラス
    /// </summary>
    class JsonStatusStack : JsonStack<JsonStatus>
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
            if (this.Count() <= 0)
            {
                return "Stackなし";
            }
            return this.ToString(base.Get());
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

    /// <summary>
    /// モードスタッククラス
    /// </summary>
    class JsonModeStack : JsonStack<JsonMode>
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
            if (this.Count() < 2)
            {
                return JsonMode.Non;
            }

            // 1つ前を取得
            JsonMode _mode = this[this.Count() - 2];

            // モードを返却
            return _mode;
        }

        /// <summary>
        /// 現在状態文字列取得
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Count() <= 0)
            {
                return "Stackなし";
            }
            return this.ToString(base.Get());
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
            if (this.Count() < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 現在のインデックス値を取得
            int _current_index = this.Get().Value;

            // 現在のインデックス値を更新
            this[this.Count() - 1].Value = _current_index + 1;

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
            if (this.Count() < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 現在のインデックス値を取得
            int _current_index = this.Get().Value;

            // 現在のインデックス値を更新
            this[this.Count() - 1].Value = _current_index - 1;

            // 設定値を返却
            return _current_index - 1;
        }

        /// <summary>
        /// カレント情報
        /// </summary>
        public string Current()
        {
            // サイズ判定
            if (this.Count() < 1)
            {
                // インデックスなし
                return "";
            }

            // 現在のインデックス値を取得
            int _current_index = this.Get().Value;

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
            if (this.Count() < 1)
            {
                // インデックスなし
                return "Stackなし";
            }

            JsonArrayInfo _array_info = this.Get();
            string _current_array_index;
            _current_array_index = _array_info.Key + ":" + _array_info.Value.ToString();

            // 文字列を返却
            return _current_array_index;
        }
    };

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
            if (this.Count() <= 0)
            {
                return new JsonKeyInfo();
            }
            return this[this.Count() - 1];
        }

        /// <summary>
        /// 現在状態文字列取得
        /// </summary>
        public override string ToString()
        {
            if (this.Count() <= 0)
            {
                return "Stackなし";
            }
            return this.ToString(this.Get());
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
            this.Clear();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="str"></param>
        public JsonStringQueue(string str)
        {
            // 設定
            this.Set(str);
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JsonStringQueue()
        {
            // クリア
            this.Clear();
        }

        /// <summary>
        /// 終端判定
        /// </summary>
        /// <returns></returns>
        public bool Eof()
        {
            // サイズ判定
            if (this.m_queue.Count <= 0)
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
            if (this.m_queue.Count <= 0)
            {
                return 0;
            }

            // 文字列
            List<string> _tmp_queue = new List<string>(this.m_queue);
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
            return (uint)this.m_queue.Count;
        }

        /// <summary>
        /// 処理中カラム
        /// </summary>
        /// <returns></returns>
        public uint CurrentColum()
        {
            // 処理中カラムを返却
            return this.m_column_no;
        }

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="str"></param>
        public void Set(string str)
        {
            // 設定
            this.m_column_no = 0;

            // クリア
            this.Clear();

            // 文字列設定
            for (uint i = 0; i < str.Length; i++)
            {
                // 1文字ずつキューに保存
                this.m_queue.Add(str.Substring((int)i, 1));
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 文字列キューをクリア
            this.m_queue.Clear();
        }

        /// <summary>
        /// 空白スキップ
        /// </summary>
        public void SkipBlank()
        {
            // 1文字ずつ処理する
            while (this.m_queue.Count > 0)
            {
                // 空白の場合
                if (this.m_queue[0] == " " || this.m_queue[0] == "\t")
                {
                    // 先頭文字削除
                    this.m_queue.RemoveAt(0);

                    // スキップ
                    this.m_column_no++;
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
            this.m_column_no++;
            
            // 先頭文字取得
            string _result = this.m_queue[0];

            // 先頭文字(取得文字)削除
            this.m_queue.RemoveAt(0);

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
                this.m_queue.Insert(0, str.Substring((int)i, 1));

                // カラム数更新
                this.m_column_no--;
            }
        }
    };

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
            this.Set(JsonError.NormalEnd);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="error"></param>
        public JsonErrorInfo(JsonError error)
        {
            // 初期化
            this.Set(error);
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
            this.m_error = error;
        }

        /// <summary>
        /// 値取得
        /// </summary>
        /// <returns></returns>
        public JsonError Get()
        {
            // エラーコードを返却
            return this.m_error;
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        public override string ToString()
        {
            string _error_string = "";
            switch (this.m_error)
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
            this.Raw = arg.Raw;
            this.Mode = arg.Mode;
            this.Line = arg.Line;
            this.Colum = arg.Colum;
            this.Array.Value = arg.Array.Value;
            this.Array.Key = arg.Array.Key;
            this.Key.StartColum = arg.Key.StartColum;
            this.Key.Value = arg.Key.Value;
            this.Type = arg.Type;
            this.Value = arg.Value;
            this.Error.Set(arg.Error.Get());
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
            this.Raw = raw;
            this.Mode = mode;
            this.Line = line;
            this.Colum = colum;
            this.Array.Value = array.Value;
            this.Array.Key = array.Key;
            this.Key.StartColum = key.StartColum;
            this.Key.Value = key.Value;
            this.Type = type;
            this.Value = value;
            this.Error.Set(error.Get());
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string _result = string.Empty;
            _result += "処理行データ             ：" + this.Raw.Trim('\r', '\n') + Environment.NewLine;
            _result += "動作モード               ：" + this.Mode + Environment.NewLine;
            _result += "行数                     ：" + this.Line + Environment.NewLine;
            _result += "カラム位置               ：" + this.Colum + Environment.NewLine;
            _result += "Array情報(インデックス値)：" + this.Array.Value + Environment.NewLine;
            _result += "Array情報(キー値)        ：" + this.Array.Key + Environment.NewLine;
            _result += "Key情報(キー開始位置)    ：" + this.Key.StartColum + Environment.NewLine;
            _result += "Key情報(キー値)          ：" + this.Key.Value + Environment.NewLine;
            _result += "Value値種別              ：" + this.Type + Environment.NewLine;
            _result += "Value値                  ：" + this.Value + Environment.NewLine;
            _result += "エラー内容               ：" + this.Error.ToString() + Environment.NewLine;
            return _result;
        }
    };

    #region イベントハンドラ
    public delegate bool EventHandlerStartObject(JsonEventArg args, object userData);
    public delegate bool EventHandlerEndObject(JsonEventArg args, object userData);
    public delegate bool EventHandlerStartArray(JsonEventArg args, object userData);
    public delegate bool EventHandlerEndArray(JsonEventArg args, object userData);
    public delegate bool EventHandlerStartElement(JsonEventArg args, object userData);
    public delegate bool EventHandlerEndElement(JsonEventArg args, object userData);
    public delegate bool EventHandlerKey(JsonEventArg args, object userData);
    public delegate bool EventHandlerNull(JsonEventArg args, object userData);
    public delegate bool EventHandlerBool(JsonEventArg args, object userData);
    public delegate bool EventHandlerNumber(JsonEventArg args, object userData);
    public delegate bool EventHandlerValue(JsonEventArg args, object userData);
    public delegate bool EventHandlerString(JsonEventArg args, object userData);
    public delegate void EventHandlerError(JsonEventArg args, object userData);
    #endregion

    /// <summary>
    /// Json構文解析
    /// </summary>
    public class JsonParser
    {
        /// <summary>
        /// エンコーダー
        /// </summary>
        private Encoding m_encoding = Encoding.UTF8;

        /// <summary>
        /// 文字列
        /// </summary>
        private string m_raw_string = string.Empty;

        /// <summary>
        /// オフセット行番号
        /// </summary>
        private uint m_offset_line_no = 0;

        /// <summary>
        /// 処理行番号
        /// </summary>
        private uint m_line_no = 0;

        /// <summary>
        /// 文字列キュー
        /// </summary>
        private JsonStringQueue m_strings_queue = new JsonStringQueue();

        /// <summary>
        /// 解析状態
        /// </summary>
        private JsonStatusStack m_status_stack = new JsonStatusStack();

        /// <summary>
        /// モードスタック
        /// </summary>
        JsonModeStack m_mode_stack = new JsonModeStack();

        /// <summary>
        /// Arrayインデックススタック
        /// </summary>
        JsonArrayIndexStack m_array_index_stack = new JsonArrayIndexStack();

        /// <summary>
        /// ユーザデータ
        /// </summary>
        private object m_user_data = null;

        /// <summary>
        /// Keyスタック
        /// </summary>
        JsonKeyInfoStack m_key_stack = new JsonKeyInfoStack();

        #region イベントハンドラ
        /// <summary>
        /// Object開始検出通知
        /// </summary>
        public EventHandlerStartObject OnStartObject = null;

        /// <summary>
        /// Object終了検出通知
        /// </summary>
        public EventHandlerEndObject OnEndObject = null;

        /// <summary>
        /// Array開始検出通知
        /// </summary>
        public EventHandlerStartArray OnStartArray = null;

        /// <summary>
        /// Array終了検出通知
        /// </summary>
        public EventHandlerEndArray OnEndArray = null;

        /// <summary>
        /// Element開始検出通知
        /// </summary>
        public EventHandlerStartElement OnStartElement = null;

        /// <summary>
        /// Element終了検出通知
        /// </summary>
        public EventHandlerEndElement OnEndElement = null;

        /// <summary>
        /// Key検出通知
        /// </summary>
        public EventHandlerKey OnKey = null;

        /// <summary>
        /// Value検出通知(Null)
        /// </summary>
        public EventHandlerNull OnNull = null;

        /// <summary>
        /// Value検出通知(Bool)
        /// </summary>
        public EventHandlerBool OnBool = null;

        /// <summary>
        /// Value検出通知(Value)
        /// </summary>
        public EventHandlerValue OnValue = null;

        /// <summary>
        /// Value検出通知(Number)
        /// </summary>
        public EventHandlerNumber OnNumber = null;

        /// <summary>
        /// Value検出通知(String)
        /// </summary>
        public EventHandlerString OnString = null;

        /// <summary>
        /// エラーイベント通知
        /// </summary>
        public EventHandlerError OnError = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonParser()
        {
            Trace.WriteLine("JsonParser::JsonParser()");
            Trace.WriteLine("JsonParser::JsonParser()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userData"></param>
        public JsonParser(object userData)
        {
            Trace.WriteLine("JsonParser::JsonParser(object)");

            // 初期化
            this.m_user_data = userData;

            Trace.WriteLine("JsonParser::JsonParser(object)");
        }
        #endregion

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JsonParser()
        {
            
            Trace.WriteLine("JsonParser::~JsonParser()");

            // 文字列キューをクリア
            this.m_strings_queue.Clear();

            Trace.WriteLine("JsonParser::JsonParser()");
        }

        /// <summary>
        /// 処理行数取得
        /// </summary>
        /// <returns></returns>
        private uint total_line_no()
        {
            return this.m_offset_line_no + this.m_line_no;
        }

        /// <summary>
        /// String判定
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool is_string(string str, ref string value)
        {
            Trace.WriteLine("JsonParser::is_string(string, ref string)");

            // 初期化
            value = string.Empty;

            // 両端の空白削除
            string _trim_str = str.Trim();

            // 実数正規表現
            if (Regex.IsMatch(_trim_str, "^\\\".*\\\"$"))
            {

                // 両端の"を削除
                value = _trim_str.Trim('"');

                // 一致
                Trace.WriteLine("JsonParser::is_string(string, ref string)");
                return true;
            }

            // 不一致
            Trace.WriteLine("JsonParser::is_string(string, ref string)");
            return false;
        }

        /// <summary>
        /// Number判定
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool is_number(string str)
        {
            Trace.WriteLine("JsonParser::is_number(string)");

            // 実数正規表現
            if (Regex.IsMatch(str, @"[\-]*[0-9]*\.[0-9]*$"))
            {
                // 一致
                Trace.WriteLine("JsonParser::is_number(string)");
                return true;
            }
            // 指数正規表現
            else if (Regex.IsMatch(str, @"[\-]*[0-9]+(\.[0-9]*)*([eE][+-]*[0-9]+)*"))
            {
                // 一致
                Trace.WriteLine("JsonParser::is_number(string)");
                return true;
            }
            // 整数正規表現
            else if (Regex.IsMatch(str, @"[\-]*[0-9]+$"))
            {
                // 一致
                Trace.WriteLine("JsonParser::is_number(string)");
                return true;
            }

            // 不一致
            Trace.WriteLine("JsonParser::is_number(string)");
            return false;
        }

        /// <summary>
        /// Arrayインデックス初期設定
        /// </summary>
        private void initialize_array_index()
        {
            Trace.WriteLine("JsonParser::initialize_array_index()");

            // Arrayインデックススタック設定
            JsonArrayInfo _array_info = new JsonArrayInfo();
            _array_info.Value = -1;
            _array_info.Key = "";
            this.m_array_index_stack.Push(_array_info);

            Trace.WriteLine("JsonParser::initialize_array_index()");
        }

        /// <summary>
        /// Arrayインデックス更新
        /// </summary>
        /// <returns></returns>
        private bool update_array_index()
        {
            Trace.WriteLine("JsonParser::update_array_index()");

            // 現在のインデックス数を取得
            uint _m_array_index_stack_count = this.m_array_index_stack.Count();

            // 現在のインデックス数を判定
            if (_m_array_index_stack_count <= 0)
            {
                // 異常終了
                this.Error(JsonError.SystemError);
                Debug.Fail("JsonParser::update_array_index()");
                return false;
            }

            // 現在のインデックス値を更新
            this.m_array_index_stack.Increment();

            // 正常終了
            Trace.WriteLine("JsonParser::update_array_index()");
            return true;
        }

        /// <summary>
        /// Arrayインデックス更新
        /// </summary>
        /// <param name="startColum"></param>
        /// <returns></returns>
        private bool update_array_index(uint startColum)
        {
            Trace.WriteLine("JsonParser::update_array_index(uint)");

            // Arrayインデックス更新
            if (!this.update_array_index())
            {
                // 異常終了
                Debug.Fail("JsonParser::update_array_index(uint)");
                return false;
            }

            // Key値設定
            JsonArrayInfo _array_info = this.m_array_index_stack.Get();
            string _Key = _array_info.Value.ToString();

            // Element開始
            if (!this.StartElement(startColum, _Key))
            {
                // 異常終了
                Debug.Fail("JsonParser::update_array_index(uint)");
                return false;
            }

            // 正常終了
            Trace.WriteLine("JsonParser::update_array_index(uint)");
            return true;
        }

        /// <summary>
        /// オフセット行数設定
        /// </summary>
        /// <param name="offsetLineNo"></param>
        public void SetOffsetLineNo(uint offsetLineNo)
        {
            this.m_offset_line_no = offsetLineNo;
        }

        #region 解析
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool Parse(string buf)
        {
            Trace.WriteLine("JsonParser::Parse(string)");
            Debug.WriteLine("buf:[{" + buf + "]");

            // 文字列保存
            this.m_raw_string = buf;

            // 行数更新
            this.m_line_no++;

            // 文字列サイズを判定
            if (this.m_raw_string.Length <= 0)
            {
                // 空行なので処理する必要なし(正常終了)
                Trace.WriteLine("JsonParser::Parse(string)");
                return true;
            }

            // 文字列キューを設定
            this.m_strings_queue.Set(this.m_raw_string);

            // 解析
            if (!this.Parse())
            {
                // 異常終了
                Debug.Fail("JsonParser::Parse(string)");
                return false;
            }

            // 正常終了
            Trace.WriteLine("JsonParser::Parse(string)");
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <returns></returns>
        private bool Parse()
        {
            Trace.WriteLine("JsonParser::Parse()");

            // 先頭の空白スキップ(空行は処理させないため)
            this.m_strings_queue.SkipBlank();

            // 文字列サイズを判定
            if (this.m_strings_queue.Length() == 0)
            {
                // 正常終了(サイズが0で呼ばれたら処理なし)
                Trace.WriteLine("JsonParser::Parse()");
                return true;
            }

            // 状態で分岐
            Debug.WriteLine("《遷移状態⇒{0}[{1}]》", this.m_status_stack.Get(), this.m_status_stack.ToString());
            switch (this.m_status_stack.Get())
            {
                //「解析未実施」状態の場合
                case JsonStatus.NotStart:
                    {
                        // 開始タグ検索
                        if (!this.find_start_tag())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Object開始」の場合
                case JsonStatus.StartObject:
                    {
                        // Objectキー検索
                        if (!this.find_object_key())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「次Object開始」の場合
                case JsonStatus.NextObject:
                    {
                        // 次Key検索
                        if (!this.find_object_next_key())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Object終了」の場合
                case JsonStatus.EndObject:
                    {
                        // 次開始検索(Object)
                        if (!this.find_object_next_start())
                        {
                            // 次の開始タグなしのため、正常終了
                            Trace.WriteLine("JsonParser::Parse()");
                            return true;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Array開始」の場合
                case JsonStatus.StartArray:
                    {
                        // Value検索(Array)
                        if (!this.find_array_value())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「次Array開始」の場合
                case JsonStatus.NextArray:
                    {
                        // Value検索(Array)
                        if (!this.find_array_value())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Array終了」の場合
                case JsonStatus.EndArray:
                    {
                        // 次開始検索(Array)
                        if (!this.find_array_next_start())
                        {
                            // 次の開始タグなしのため、正常終了
                            Trace.WriteLine("JsonParser::Parse()");
                            return true;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Key終了」の場合
                case JsonStatus.EndKey:
                    {
                        // Value開始検索
                        if (!this.find_object_start_value())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Value開始」の場合
                case JsonStatus.StartValue:
                    {
                        // Value検索(Object)
                        if (!this.find_object_value())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Value終了」の場合
                case JsonStatus.EndValue:
                    {
                        // 終了タグ検索(Object)
                        if (!this.find_object_end_tag())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Value終了(Array)」の場合
                case JsonStatus.EndArrayValue:
                    {
                        // 次開始タグ検索
                        if (!this.find_array_end_tag())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「次遷移待ち」の場合
                case JsonStatus.NextWait:
                    {
                        // 次状態検索
                        if (!this.find_next_state())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::Parse()");
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「解析終了」の場合
                case JsonStatus.End:
                    {
                        // 異常終了
                        this.Error(JsonError.InvalidSyntax);
                        Debug.Fail("JsonParser::Parse()");
                        return false;
                    }
                default:
                    {
                        // 異常終了
                        this.Error(JsonError.SystemError);
                        Trace.Fail("JsonParser::Parse()");
                        return false;
                    }
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(StreamReader stream)
        {
            Trace.WriteLine("JsonParser::Parse(StreamReader)");

            // 読込できなくなるまで繰り返す
            while (stream.Peek() >= 0)
            {
                // 1行ずつ読込
                string _stringline = stream.ReadLine();

                // 解析
                if (!this.Parse(_stringline))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Parse(StreamReader)");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::Parse(StreamReader)");
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(StringReader stream)
        {
            Trace.WriteLine("JsonParser::Parse(StringReader)");

            // 読込できなくなるまで繰り返す
            while (stream.Peek() >= 0)
            {
                // 1行ずつ読込
                string _stringline = stream.ReadLine();

                // 解析
                if (!this.Parse(_stringline))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Parse(StringReader)");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::Parse(StringReader)");
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(MemoryStream stream)
        {
            Trace.WriteLine("JsonParser::Parse(MemoryStream)");

            // 文字列を取得
            string _memoryString = this.m_encoding.GetString(stream.ToArray());

            // StringReaderオブジェクト生成
            StringReader _StringReader = new StringReader(_memoryString);

            // 解析
            if (!this.Parse(_StringReader))
            {
                // 異常終了
                Debug.Fail("JsonParser::Parse(MemoryStream)");
                return false;
            }

            // 正常終了
            Trace.WriteLine("JsonParser::Parse(MemoryStream)");
            return true;
        }
        #endregion

        /// <summary>
        /// 開始タグ検索
        /// </summary>
        /// <returns></returns>
        private bool find_start_tag()
        {
            Trace.WriteLine("JsonParser::find_start_tag()");

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 開始タグ(Object)の場合
                if (_column == "{")
                {
                    // Object開始
                    if (!this.StartObject())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_start_tag()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartObject);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_start_tag()");
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // 配列開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_start_tag()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartArray);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_start_tag()");
                    return true;
                }
            }

            // 開始タグなし
            Debug.Fail("JsonParser::find_start_tag()");
            return false;
        }

        /// <summary>
        /// 次状態検索
        /// </summary>
        /// <returns></returns>
        private bool find_next_state()
        {
            Trace.WriteLine("JsonParser::find_next_state()");
            /////
            /////    // 直前のモード取得
            /////    JsonMode _acquire_previous_mode = Json::m_mode_stack.Get();
            /////
            /////    // 1文字ずつ処理する
            /////    while(!Json::m_strings_queue.Eof())
            /////    {
            /////        // 1文字取得
            /////        std::string _column = Json::m_strings_queue.Get();
            /////
            /////        // 開始タグ(Object)の場合
            /////        if( _column == "{" )
            /////        {
            /////            // 直前のモードを判定
            /////            std::string _key = "";
            /////            if( _acquire_previous_mode == JsonMode_Array )
            /////            {
            /////                // 現在のインデックス値を更新
            /////                Json::m_array_index_stack.Increment();
            /////
            /////                // 現状のインデックスを取得
            /////                _key = Json::m_array_index_stack.Current();
            /////            }
            /////
            /////            // 開始位置
            /////            if(!Json::StartObject())
            /////            {
            /////                // 異常終了
            /////                TraceERR("Json::find_next_state()");
            /////                return false;
            /////            }
            /////
            /////            // Element開始
            /////            if(!Json::StartElement(Json::m_strings_queue.CurrentColum(),_key))
            /////            {
            /////                // 異常終了
            /////                TraceERR("Json::find_next_state()");
            /////                return false;
            /////            }
            /////
            /////            // 状態更新
            /////            Json::m_status_stack.Push(JsonStatus_StartObject);
            /////
            /////            // 正常終了
            /////            TraceED("Json::find_next_state()");
            /////            return true;
            /////        }
            /////        // 開始タグ(Array)の場合
            /////        else if( _column == "[" )
            /////        {
            /////            // Array開始
            /////            if(!Json::StartArray())
            /////            {
            /////                // 異常終了
            /////                TraceERR("Json::find_next_state()");
            /////                return false;
            /////            }
            /////
            /////            // Element開始
            /////            if(!Json::StartElement(Json::m_strings_queue.CurrentColum()-1,""))
            /////            {
            /////                // 異常終了
            /////                TraceERR("Json::find_next_state()");
            /////                return false;
            /////            }
            /////
            /////            // 状態更新
            /////            Json::m_status_stack.Push(JsonStatus_StartArray);
            /////
            /////            // 正常終了
            /////            TraceED("Json::find_next_state()");
            /////            return true;
            /////        }
            /////        // Stringの場合
            /////        else if( _column == "\"" )
            /////        {
            /////            // 直前のモードを判定
            /////            if( _acquire_previous_mode == JsonMode_Array )
            /////            {
            /////                // 現在のインデックス値を更新
            /////                Json::m_array_index_stack.Increment();
            /////            }
            /////
            /////            // 「"」をキューに戻す
            /////            Json::m_strings_queue.Put(_column);
            /////
            /////            // 状態更新
            /////            Json::m_status_stack.Push(JsonStatus_NextObject);
            /////
            /////            // 正常終了
            /////            TraceED("Json::find_next_state()");
            /////            return true;
            /////        }
            /////        break;
            /////    }
            /////
            Debug.Fail("JsonParser::find_next_state()");
            return false;
        }

        /// <summary>
        /// String検索
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool find_string(ref string key)
        {
            Trace.WriteLine("JsonParser::find_string(ref string)");

            StringBuilder _result = new StringBuilder();
            bool _escape = false;

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // "の場合
                if (_column == "\"")
                {
                    // 直前にエスケープされているか？
                    if (!_escape)
                    {
                        // エスケープされていない場合、文字列終了
                        key = _result.ToString();

                        // 正常終了
                        Trace.WriteLine("JsonParser::find_string(ref string)");
                        return true;
                    }
                    else
                    {
                        // されている場合は文字扱いなので、文字列に追加
                        _result.Append(_column);

                        // エスケープ解除
                        _escape = false;
                    }
                }
                // \の場合
                else if (_column == @"\")
                {
                    // 直前にエスケープされているか？
                    if (!_escape)
                    {
                        // 文字列に追加
                        _result.Append(_column);

                        // エスケープ文字設定
                        _escape = true;
                    }
                    else
                    {
                        // 文字列に追加
                        _result.Append(_column);

                        // エスケープ解除
                        _escape = false;
                    }
                }
                else
                {
                    // 文字列に追加
                    _result.Append(_column);

                    // エスケープ解除
                    _escape = false;
                }
            }

            // 該当なし
            Debug.WriteLine("JsonParser::find_string(ref string)");
            return false;
        }

        /// <summary>
        /// Key検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_key()
        {
            Trace.WriteLine("JsonParser::find_object_key()");

            StringBuilder _key_stream = new StringBuilder();    // Key組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndKey;         // 次遷移状態

            // 開始位置設定
            uint _start_colum = this.m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 終了タグ(Object)の場合
                if (_column == "}")
                {
                    // Arrayインデックス終端
                    if (!this.terminate_end_object_array_index())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_key()");
                        return false;
                    }

                    // Object終了
                    if (!this.EndObject())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_key()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(this.get_end_object_next_status());

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_object_key()");
                    return true;
                }
                // :の場合
                else if (_column == ":")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.StartValue;
                    break;
                }
                // 改行で終端している場合は終了
                else if (_column == "\r" || _column == "\n")
                {
                    break;
                }

                // 文字列を保存する
                _key_stream.Append(_column);
            }

            // 文字列長を判定
            if (_key_stream.ToString().Length == 0)
            {
                // 構文エラー
                this.Error(JsonError.InvalidSyntax);
                Debug.Fail("JsonParser::find_object_key()");
                return false;
            }

            // String形式を判定
            string _key = string.Empty;
            if (this.is_string(_key_stream.ToString(), ref _key))
            {
                // Element開始
                if (!this.StartElement(_start_colum + 1, _key))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::find_object_key()");
                    return false;
                }

                // Key処理
                if (!this.Key(_start_colum + 1, _key))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::find_object_key()");
                    return false;
                }

                // 状態更新
                this.m_status_stack.Push(_JsonStatus);

                // 正常終了
                Trace.WriteLine("JsonParser::find_object_key()");
                return true;
            }

            // Keyなし
            this.Error(JsonError.NotFoundKey);
            Debug.Fail("JsonParser::find_object_key()");
            return false;
        }

        /// <summary>
        /// 次Key検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_next_key()
        {
            Trace.WriteLine("JsonParser::find_object_next_key()");

            StringBuilder _key_stream = new StringBuilder();    // Key組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndKey;         // 次遷移状態

            // 開始位置設定
            uint _start_colum = this.m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // :の場合
                if (_column == ":")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.StartValue;
                    break;
                }
                // 改行で終端している場合は終了
                else if (_column == "\r" || _column == "\n")
                {
                    break;
                }

                // 文字列を保存する
                _key_stream.Append(_column);
            }

            // 文字列長を判定
            if (_key_stream.ToString().Length == 0)
            {
                // 構文エラー
                this.Error(JsonError.InvalidSyntax);
                Debug.Fail("JsonParser::find_object_next_key()");
                return false;
            }

            // String形式を判定
            string _key = string.Empty;
            if (this.is_string(_key_stream.ToString(), ref _key))
            {
                // Element開始
                if (!this.StartElement(_start_colum + 1, _key))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::find_object_next_key()");
                    return false;
                }

                // Key処理
                if (!this.Key(_start_colum + 1, _key))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::find_object_next_key()");
                    return false;
                }

                // 状態更新
                this.m_status_stack.Push(_JsonStatus);

                // 正常終了
                Trace.WriteLine("JsonParser::find_object_next_key()");
                return true;
            }

            // Keyなし
            this.Error(JsonError.NotFoundKey);
            Debug.Fail("JsonParser::find_object_next_key()");
            return false;
        }

        /// <summary>
        /// Value開始検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_start_value()
        {
            Trace.WriteLine("JsonParser::find_object_start_value()");

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // :の場合
                if (_column == ":")
                {
                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartValue);

                    // Object値開始
                    Trace.WriteLine("JsonParser::find_object_start_value()");
                    return true;
                }
                break;
            }

            // 次Value開始タグなし
            this.Error(JsonError.InvalidSyntax);
            Debug.Fail("JsonParser::find_object_start_value()");
            return false;
        }

        /// <summary>
        /// Value検索
        /// </summary>
        /// <returns></returns>
        private bool find_object_value()
        {
            Trace.WriteLine("JsonParser::find_object_value()");

            StringBuilder _value_stream = new StringBuilder();  // Value組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndValue;       // 次遷移状態

            // 開始位置を保存
            uint _start_colum = this.m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 開始タグ(Object)の場合
                if (_column == "{")
                {
                    // Object開始
                    if (!this.StartObject())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartObject);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_object_value()");
                    return true;
                }
                // 終了タグ(Object)の場合
                else if (_column == "}")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.EndObject;
                    break;
                }
                // ","の場合
                else if (_column == ",")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.NextObject;
                    break;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // Array開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartArray);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_object_value()");
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // Array開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartArray);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_object_value()");
                    return true;
                }
                // 改行で終端している場合は終了
                else if (_column == "\r" || _column == "\n")
                {
                    // 状態更新
                    break;
                }

                // 文字列を保存する
                _value_stream.Append(_column);
            }

            // 文字列長を判定
            if (_value_stream.ToString().Length > 0)
            {
                // String形式を判定
                string _string_value = string.Empty;
                if (this.is_string(_value_stream.ToString(), ref _string_value))
                {
                    // 開始カラム更新
                    _start_colum = _start_colum + 1;

                    // String処理
                    if (!this.String(_start_colum, _string_value))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }
                }
                // true、またはfalse、null、数値の可能性がある場合
                else
                {
                    // Value処理
                    if (!this.Value(_start_colum, _value_stream.ToString().Trim()))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }
                }
            }

            // Key情報取得
            JsonKeyInfo _key_info = this.m_key_stack.Current();

            // 次遷移状態で分岐
            switch (_JsonStatus)
            {
                case JsonStatus.NextObject:
                case JsonStatus.EndValue:
                    {
                        // Element終了
                        if (!this.EndElement(_key_info.StartColum, _key_info.Value))
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::find_object_value()");
                            return false;
                        }

                        // 状態更新
                        this.m_status_stack.Push(_JsonStatus);

                        // 正常終了
                        Trace.WriteLine("JsonParser::find_object_value()");
                        return true;
                    }
                case JsonStatus.EndObject:
                    {
                        // Element終了
                        if (!this.EndElement(_key_info.StartColum, _key_info.Value))
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::find_object_value()");
                            return false;
                        }

                        // Arrayインデックス終端
                        if (!this.terminate_end_object_array_index())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::find_object_value()");
                            return false;
                        }

                        // Object終了
                        if (!this.EndObject())
                        {
                            // 異常終了
                            return false;
                        }

                        // 状態更新
                        this.m_status_stack.Push(this.get_end_object_next_status());

                        // 正常終了
                        Trace.WriteLine("JsonParser::find_object_value()");
                        return true;
                    }
                default:
                    // 異常終了 
                    this.Error(JsonError.SystemError);
                    Debug.Fail("JsonParser::find_object_value()");
                    return false;
            }
        }

        /// <summary>
        /// 終了タグ検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_end_tag()
        {
            Trace.WriteLine("JsonParser::find_object_value()");

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 終了タグ(Object)の場合
                if (_column == "}")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = this.m_key_stack.Current();

                    // Element終了
                    if (!this.EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }

                    // Arrayインデックス終端
                    if (!this.terminate_end_object_array_index())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }

                    // Object終了
                    if (!this.EndObject())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(this.get_end_object_next_status());

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_object_value()");
                    return true;
                }
                // ","の場合
                else if (_column == ",")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = this.m_key_stack.Current();

                    // Element終了
                    if (!this.EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_value()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.NextObject);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_object_value()");
                    return true;
                }
                break;
            }

            // 終了タグなし
            this.Error(JsonError.NotFindEndTag);
            Debug.Fail("JsonParser::find_object_value()");
            return false;
        }

        /// <summary>
        /// 次開始検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_next_start()
        {
            Trace.WriteLine("JsonParser::find_object_next_start()");

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 次Objectあり','の場合
                if (_column == ",")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = this.m_key_stack.Current();

                    // Element終了
                    if (!this.EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_object_next_start()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.NextWait);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_object_next_start()");
                    return true;
                }
                break;
            }

            // 次Objectなし
            Debug.WriteLine("JsonParser::find_object_next_start()");
            return false;
        }

        /// <summary>
        /// Arrayインデックス終端(Object終了時)
        /// </summary>
        /// <returns></returns>
        private bool terminate_end_object_array_index()
        {
            Trace.WriteLine("JsonParser::terminate_end_object_array_index()");

            // 動作モード(親)取得
            JsonMode _parent_mode = this.m_mode_stack.Parent();

            // 動作モード(親)判定
            if (_parent_mode != JsonMode.Array)
            {
                // Arrayではないので処理終了
                Trace.WriteLine("JsonParser::terminate_end_object_array_index()");
                return true;
            }

            // キー情報数を判定
            if (this.m_key_stack.Count() <= 1)
            {
                // 1つ前がないので処理終了
                Trace.WriteLine("JsonParser::terminate_end_object_array_index()");
                return true;
            }

            // Key情報取得
            JsonKeyInfo _key_info = this.m_key_stack.Get();

            // Element終了
            if (!this.EndElement(_key_info.StartColum, _key_info.Value))
            {
                // 異常終了
                Debug.Fail("JsonParser::terminate_end_object_array_index()");
                return false;
            }

            // 正常終了
            Trace.WriteLine("JsonParser::terminate_end_object_array_index()");
            return true;
        }

        /// <summary>
        /// 次遷移状態取得(Object終了時)
        /// </summary>
        /// <returns></returns>
        private JsonStatus get_end_object_next_status()
        {
            Trace.WriteLine("JsonParser::get_end_object_next_status()");

            // 動作モードで分岐
            JsonStatus _status = JsonStatus.NotStart;
            switch (this.m_mode_stack.Get())
            {
                case JsonMode.Object:
                    // 次遷移状態設定
                    _status = JsonStatus.EndValue;
                    break;
                case JsonMode.Array:
                    // 次遷移状態設定
                    _status = JsonStatus.EndArrayValue;
                    break;
                default:
                    // 次遷移状態設定
                    _status = JsonStatus.End;
                    break;
            }

            // 次遷移状態を返却
            Debug.WriteLine("《次遷移状態⇒[" + this.m_status_stack.ToString(_status) + "]》");
            Trace.WriteLine("JsonParser::get_end_object_next_status()");
            return _status;
        }

        /// <summary>
        /// Value検索(Array)
        /// </summary>
        /// <returns></returns>
        private bool find_array_value()
        {
            Trace.WriteLine("JsonParser::find_array_value()");

            StringBuilder _value_stream = new StringBuilder();  // Value組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndArrayValue;  // 次遷移状態

            // 開始位置を保存
            uint _start_colum = this.m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 開始タグ(Object)の場合
                if (_column == "{")
                {
                    // Arrayインデックス更新
                    if (!this.update_array_index(_start_colum))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_array_value()");
                        return false;
                    }

                    // Object開始
                    if (!this.StartObject())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_array_value()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartObject);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_array_value()");
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // Array開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_array_value()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.StartArray);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_array_value()");
                    return true;
                }
                // 終了タグ(Array)の場合
                else if (_column == "]")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.EndArray;
                    break;
                }
                // ","の場合
                else if (_column == ",")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.NextArray;
                    break;
                }
                // 改行で終端している場合は終了
                else if (_column == "\r" || _column == "\n")
                {
                    // 状態更新
                    break;
                }

                // 文字列を保存する
                _value_stream.Append(_column);
            }

            // Arrayインデックス更新
            if (!this.update_array_index(_start_colum))
            {
                // 異常終了
                Debug.Fail("JsonParser::find_array_value()");
                return false;
            }

            // 文字列長を判定
            if (_value_stream.ToString().Length > 0)
            {
                // String形式を判定
                string _string_value = string.Empty;
                if (this.is_string(_value_stream.ToString(), ref _string_value))
                {
                    // 開始カラム更新
                    _start_colum = _start_colum + 1;

                    // String処理
                    if (!this.String(_start_colum, _string_value))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_array_value()");
                        return false;
                    }
                }
                // true、またはfalse、null、数値の可能性がある場合
                else
                {
                    // Value処理
                    if (!this.Value(_start_colum, _value_stream.ToString().Trim()))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_array_value()");
                        return false;
                    }
                }
            }

            // Key情報取得
            JsonKeyInfo _key_info = this.m_key_stack.Current();

            // 次遷移状態で分岐
            switch (_JsonStatus)
            {
                case JsonStatus.NextArray:
                case JsonStatus.EndArrayValue:
                    {
                        // Element終了
                        if (!this.EndElement(_key_info.StartColum, _key_info.Value))
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::find_array_value()");
                            return false;
                        }

                        // 状態更新
                        this.m_status_stack.Push(_JsonStatus);

                        // 正常終了
                        Trace.WriteLine("JsonParser::find_array_value()");
                        return true;
                    }
                case JsonStatus.EndArray:
                    {
                        // Array終了
                        if (!this.EndArray())
                        {
                            // 異常終了
                            Debug.Fail("JsonParser::find_array_value()");
                            return false;
                        }

                        // 状態更新
                        this.m_status_stack.Push(this.get_end_object_next_status());

                        // 正常終了
                        Trace.WriteLine("JsonParser::find_array_value()");
                        return true;
                    }
                default:
                    // 異常終了
                    this.Error(JsonError.SystemError);
                    Trace.Fail("JsonParser::find_array_value()");
                    return false;
            }
        }

        /// <summary>
        /// 終了タグ検索
        /// </summary>
        /// <returns></returns>
        private bool find_array_end_tag()
        {
            Trace.WriteLine("JsonParser::find_array_end_tag()");

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 終了タグ(Array)の場合
                if (_column == "]")
                {
                    // Array終了
                    if (!this.EndArray())
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_array_end_tag()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(this.get_end_object_next_status());

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_array_end_tag()");
                    return true;
                }
                // 次Value存在有の場合
                else if (_column == ",")
                {
                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.NextArray);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_array_end_tag()");
                    return true;
                }
                break;
            }

            // 終了タグなし
            this.Error(JsonError.NotFindEndTag);
            Debug.Fail("JsonParser::find_array_end_tag()");
            return false;
        }

        /// <summary>
        /// 次開始検索(Array)
        /// </summary>
        /// <returns></returns>
        private bool find_array_next_start()
        {
            Trace.WriteLine("JsonParser::find_array_next_start()");

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 次Arrayあり','の場合
                if (_column == ",")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = this.m_key_stack.Current();

                    // Element終了
                    if (!this.EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // 異常終了
                        Debug.Fail("JsonParser::find_array_next_start()");
                        return false;
                    }

                    // 状態更新
                    this.m_status_stack.Push(JsonStatus.NextWait);

                    // 正常終了
                    Trace.WriteLine("JsonParser::find_array_next_start()");
                    return true;
                }
                break;
            }

            // 次Arrayなし
            Debug.WriteLine("JsonParser::find_array_next_start()");
            return false;
        }

        /// <summary>
        /// Object開始
        /// </summary>
        /// <returns></returns>
        private bool StartObject()
        {
            Trace.WriteLine("JsonParser::StartObject()");

            // モードスタック設定
            this.m_mode_stack.Push(JsonMode.Object);

            // コールバック関数呼び出し
            if (this.OnStartObject != null)
            {
                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    JsonMode.Object,
                    this.total_line_no(),
                    this.m_strings_queue.CurrentColum() - 1,
                    new JsonArrayInfo(),
                    new JsonKeyInfo(),
                    JsonValueType.ObjectStart,
                    "{",
                    new JsonErrorInfo()
                );

                // Object開始検出通知
                if (!this.OnStartObject(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::StartObject()");
                    return false;
                }
            }

            // 正常尾終了
            Trace.WriteLine("JsonParser::StartObject()");
            return true;
        }

        /// <summary>
        /// Object終了
        /// </summary>
        /// <returns></returns>
        private bool EndObject()
        {
            Trace.WriteLine("JsonParser::EndObject()");

            // モードスタックサイズ判定
            if (this.m_mode_stack.Count() <= 0)
            {
                // 異常終了
                this.Error(JsonError.SystemError);
                Debug.Fail("JsonParser::EndObject()");
                return false;
            }

            // 動作モード取得
            JsonMode _mode = this.m_mode_stack.Get();

            // モードスタック解放
            this.m_mode_stack.Pop();

            // モード判定
            if (_mode != JsonMode.Object)
            {
                // 異常終了
                this.Error(JsonError.InvalidSyntax);
                Debug.Fail("JsonParser::EndObject()");
                return false;
            }

            // コールバック関数呼び出し
            if (this.OnEndObject != null)
            {
                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    JsonMode.Object,
                    this.total_line_no(),
                    this.m_strings_queue.CurrentColum() - 1,
                    new JsonArrayInfo(),
                    new JsonKeyInfo(),
                    JsonValueType.ObjectEnd,
                    "}",
                    new JsonErrorInfo()
                );

                // Object終了検出通知
                if (!this.OnEndObject(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::EndObject()");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::EndObject()");
            return true;
        }

        /// <summary>
        /// Array開始
        /// </summary>
        /// <returns></returns>
        private bool StartArray()
        {
            Trace.WriteLine("JsonParser::StartArray()");

            // モードスタック設定
            this.m_mode_stack.Push(JsonMode.Array);

            // Arrayインデックス初期設定
            this.initialize_array_index();

            // コールバック関数呼び出し
            if (this.OnStartArray != null)
            {
                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    JsonMode.Array,
                    this.total_line_no(),
                    this.m_strings_queue.CurrentColum() - 1,
                    _Array,
                    new JsonKeyInfo(),
                    JsonValueType.ArrayStart,
                    "[",
                    new JsonErrorInfo()
                );

                // Array開始検出通知
                if (!this.OnStartArray(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::StartArray()");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::StartArray()");
            return true;
        }

        /// <summary>
        /// Array終了
        /// </summary>
        /// <returns></returns>
        private bool EndArray()
        {
            Trace.WriteLine("JsonParser::EndArray()");

            // モードスタックサイズ判定
            if (this.m_mode_stack.Count() <= 0)
            {
                // 異常終了
                this.Error(JsonError.SystemError);
                Debug.Fail("JsonParser::EndArray()");
                return false;
            }

            // 動作モード取得
            JsonMode _mode = this.m_mode_stack.Get();

            // モードスタック解放
            this.m_mode_stack.Pop();

            // モード判定
            if (_mode != JsonMode.Array)
            {
                // 異常終了
                this.Error(JsonError.InvalidSyntax);
                Debug.Fail("JsonParser::EndArray()");
                return false;
            }

            // Arrayインデックススタック解放
            this.m_array_index_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnEndArray != null)
            {
                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    JsonMode.Array,
                    this.total_line_no(),
                    this.m_strings_queue.CurrentColum() - 1,
                    _Array,
                    new JsonKeyInfo(),
                    JsonValueType.ArrayEnd,
                    "]",
                    new JsonErrorInfo()
                );

                // Array終了検出通知
                if (!this.OnEndArray(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::EndArray()");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::EndArray()");
            return true;
        }

        /// <summary>
        /// Element開始
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool StartElement(uint startColum, string key)
        {
            Trace.WriteLine("JsonParser::StartElement(uint, string)");

            // Key情報スタック設定
            JsonKeyInfo _keyInfo = new JsonKeyInfo();
            _keyInfo.StartColum = startColum;
            _keyInfo.Value = key;

            // Key情報登録
            this.m_key_stack.Push(_keyInfo);

            // コールバック関数呼び出し
            if (this.OnStartElement != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    startColum,
                    _Array,
                    _keyInfo,
                    JsonValueType.Element,
                    key,
                    new JsonErrorInfo()
                );

                // Element開始通知
                if (!this.OnStartElement(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::StartElement(uint, string)");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::StartElement(uint, string)");
            return true;
        }

        /// <summary>
        /// Element終了
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool EndElement(uint startColum, string key)
        {
            Trace.WriteLine("JsonParser::EndElement(uint, string)");

            // キー数を判定
            if (this.m_key_stack.Count() <= 0)
            {
                // 正常終了
                Trace.WriteLine("JsonParser::EndElement(uint, string)");
                return true;
            }

            // コールバック関数呼び出し
            if (this.OnEndElement != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = this.m_key_stack.Get();

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Element,
                    key,
                    new JsonErrorInfo()
                );

                // Element終了通知
                if (!this.OnEndElement(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::EndElement(uint, string)");
                    return false;
                }
            }

            // キースタック解放
            this.m_key_stack.Pop();

            // 正常終了
            Trace.WriteLine("JsonParser::EndElement(uint, string)");
            return true;
        }

        /// <summary>
        /// Key処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool Key(uint startColum, string key)
        {
            Trace.WriteLine("JsonParser::Key(uint, string)");

            // TODO:未実装

            // 正常終了
            Trace.WriteLine("JsonParser::Key(uint, string)");
            return true;
        }

        /// <summary>
        /// Value処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool Value(uint startColum, string value)
        {
            Trace.WriteLine("JsonParser::Value(uint, string)");

            // コールバック関数呼び出し
            if (this.OnValue != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (this.m_key_stack.Count() > 0)
                {
                    _current_key = this.m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Value,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(Value)
                if (!this.OnValue(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Value(uint, string)");
                    return false;
                }
            }

            // NULL判定
            if (value == "null")
            {
                // Null処理
                if (!this.Null(startColum))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Value(uint, string)");
                    return false;
                }

                // 正常終了
                Trace.WriteLine("JsonParser::Value(uint, string)");
                return true;
            }
            // 値判定(bool)
            else if (value == "true" || value == "false")
            {
                // bool処理
                if (!this.Bool(startColum, value))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Value(uint, string)");
                    return false;
                }

                // 正常終了
                Trace.WriteLine("JsonParser::Value(uint, string)");
                return true;
            }
            // 値判定(Number)
            else if (this.is_number(value))
            {
                // Number処理
                if (!this.Number(startColum, value))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Value(uint, string)");
                    return false;
                }

                // 正常終了
                Trace.WriteLine("JsonParser::Value(uint, string)");
                return true;
            }

            // 異常終了
            Debug.Fail("JsonParser::Value(uint, string)");
            return false;
        }

        /// <summary>
        /// Number処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool Number(uint startColum, string value)
        {
            Trace.WriteLine("JsonParser::Value(uint, string)");

            // コールバック関数呼び出し
            if (this.OnNumber != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (this.m_key_stack.Count() > 0)
                {
                    _current_key = this.m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Number,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(Number)
                if (!this.OnNumber(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Value(uint, string)");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::Value(uint, string)");
            return true;
        }

        /// <summary>
        /// String処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool String(uint startColum, string value)
        {
            Trace.WriteLine("JsonParser::String(uint, string)");

            // コールバック関数呼び出し
            if (this.OnString != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (this.m_key_stack.Count() > 0)
                {
                    _current_key = this.m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.String,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(String)
                if (!this.OnString(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::String(uint, string)");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::String(uint, string)");
            return true;
        }

        /// <summary>
        /// Null処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <returns></returns>
        bool Null(uint startColum)
        {
            Trace.WriteLine("JsonParser::Null(uint)");

            // コールバック関数呼び出し
            if (this.OnNull != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (this.m_key_stack.Count() > 0)
                {
                    _current_key = this.m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Null,
                    "null",
                    new JsonErrorInfo()
                );

                // Value検出通知(Null)
                if (!this.OnNull(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Null(uint)");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::Null(uint)");
            return true;
        }

        /// <summary>
        /// Bool処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool Bool(uint startColum, string value)
        {
            Trace.WriteLine("JsonParser::Bool(uint, string)");

            // コールバック関数呼び出し
            if (this.OnBool != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (this.m_key_stack.Count() > 0)
                {
                    _current_key = this.m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Bool,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(Bool)
                if (!this.OnBool(_Args, this.m_user_data))
                {
                    // 異常終了
                    Debug.Fail("JsonParser::Bool(uint, string)");
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("JsonParser::Bool(uint, string)");
            return true;
        }

        /// <summary>
        /// エラー処理
        /// </summary>
        /// <param name="error"></param>
        private void Error(JsonError error)
        {
            Trace.WriteLine("JsonParser::Error(JsonError)");

            // コールバック関数呼び出し
            if (this.OnError != null)
            {
                // 動作モード取得
                JsonMode _mode = this.m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (this.m_array_index_stack.Count() > 0)
                {
                    _Array = this.m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (this.m_key_stack.Count() > 0)
                {
                    _current_key = this.m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    this.m_raw_string,
                    _mode,
                    this.total_line_no(),
                    this.m_strings_queue.CurrentColum(),
                    _Array,
                    _current_key,
                    JsonValueType.Unknown,
                    "",
                    new JsonErrorInfo(error)
                );

                // エラーイベント通知
                this.OnError(_Args, this.m_user_data);
            }

            Trace.WriteLine("JsonParser::Error(JsonError)");
        }
    }
}