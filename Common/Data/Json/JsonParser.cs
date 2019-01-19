using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;

namespace Common.Data.Json
{
    /// <summary>
    /// 解析状態
    /// </summary>
    public enum JsonStatus : byte
    {
        NotStart = 0,
        StartObject,
        Object,
        EndObject,
        StartArray,
        Array,
        EndArray,
        StartValue,
        Value,
        EndValue,
    }

    #region イベントハンドラ
    public delegate bool EventHandlerNull(uint lineNo, uint startColum, string key, object userData);
    public delegate bool EventHandlerBool(uint lineNo, uint startColum, string key, bool value, object userData);
    public delegate bool EventHandlerValue(uint lineNo, uint startColum, string key, string value, object userData);
    public delegate bool EventHandlerString(uint lineNo, uint startColum, string key, string value, object userData);
    public delegate bool EventHandlerStartObject(uint lineNo, uint startColum, object userData);
    public delegate bool EventHandlerKey(uint lineNo, uint startColum, string key, object userData);
    public delegate bool EventHandlerEndObject(uint lineNo, uint startColum, string key, object userData);
    public delegate bool EventHandlerStartArray(uint lineNo, uint startColum, object userData);
    public delegate bool EventHandlerEndArray(uint lineNo, uint startColum, object userData);
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
        /// ユーザデータ
        /// </summary>
        private object m_user_data = null;

        /// <summary>
        /// 解析状態
        /// </summary>
        private JsonStatus m_status = JsonStatus.NotStart;

        /// <summary>
        /// 処理中行数
        /// </summary>
        private uint m_line_no = 0;

        /// <summary>
        /// 処理行オフセット
        /// </summary>
        private uint m_line_offset = 0;

        /// <summary>
        /// 処理中カラム
        /// </summary>
        private uint m_column_no = 0;

        /// <summary>
        /// 文字列Queue
        /// </summary>
        private Queue<string> m_strings_queue = new Queue<string>();

        /// <summary>
        /// Keyスタック
        /// </summary>
        private Stack<string> m_key_stack = new Stack<string>();

        #region イベントハンドラ
        public EventHandlerNull OnNull;
        public EventHandlerBool OnBool;
        public EventHandlerValue OnValue;
        public EventHandlerString OnString;
        public EventHandlerStartObject Object;
        public EventHandlerKey OnKey;
        public EventHandlerEndObject OnEndObject;
        public EventHandlerStartArray OnStartArray;
        public EventHandlerEndArray OnEndArray;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JsonParser()
        {
            Trace.WriteLine("=>>>> JsonParser::JsonParser()");

            Trace.WriteLine("<<<<= JsonParser::JsonParser()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userData"></param>
        public JsonParser(object userData)
        {
            Trace.WriteLine("=>>>> JsonParser::JsonParser(object)");

            // 初期化
            this.m_user_data = userData;

            Trace.WriteLine("<<<<= JsonParser::JsonParser(object)");
        }
        #endregion

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JsonParser()
        {
            Trace.WriteLine("=>>>> JsonParser::~JsonParser()");

            // 文字列キューをクリア
            this.m_strings_queue.Clear();

            Trace.WriteLine("<<<<= JsonParser::~JsonParser()");
        }

        #region 解析
        /// <summary>
        /// 解析
        /// </summary>
        /// <returns></returns>
        private bool Parse()
        {
            Trace.WriteLine("=>>>> JsonParser::Parse()");

            // 文字列(文字列キュー)サイズを判定
            if (this.m_strings_queue.Count == 0)
            {
                // 正常終了
                return true;
            }

            // 状態で分岐
            Debug.WriteLine("　状態:" + this.m_status.ToString());
            switch (this.m_status)
            {
                case JsonStatus.NotStart:
                    {
                        // 開始タグ検索
                        if (!this.Find_start_tag())
                        {
                            // 異常終了
                            return false;
                        }

                        // 次ステージに遷移
                        return this.Parse();
                    }
                case JsonStatus.StartObject:
                    {
                        // Objectキー検索
                        if (!this.Find_object_key())
                        {
                            // 異常終了
                            return false;
                        }

                        // 次ステージに遷移
                        return this.Parse();
                    }
                case JsonStatus.Object:
                    {
                        // Value開始検索
                        if (!this.Find_start_value())
                        {
                            // 異常終了
                            return false;
                        }

                        // 次ステージに遷移
                        return this.Parse();
                    }
                case JsonStatus.EndObject:
                    {
                        // 終端処理
                        if (!this.EndObject())
                        {
                            // 異常終了
                            return false;
                        }

                        // 開始タグ検索
                        if (!this.Find_start_tag())
                        {
                            // 開始タグがないので、終了
                            Trace.WriteLine("<<<<= JsonParser::Parse()");
                            return true;
                        }

                        // 次ステージに遷移
                        return this.Parse();
                    }
                case JsonStatus.StartValue:
                    {
                        // Objectキー検索
                        if (!this.Find_object_value())
                        {
                            // 異常終了
                            return false;
                        }

                        // 次ステージに遷移
                        return this.Parse();
                    }
                default:
                    break;
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Parse()");
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool Parse(string buf)
        {
            Trace.WriteLine("=>>>> JsonParser::Parse(string)");
            Debug.WriteLine("　buf:[" + buf + "]");

            // 文字列サイズを判定
            if (buf.Length <= 0)
            {
                // 行なし
                return true;
            }

            // 行数更新
            this.m_line_no++;

            // カラム数初期化
            this.m_column_no = 0;

            // 文字列キューをクリア
            this.m_strings_queue.Clear();

            // 文字列設定
            for (int i = 0; i < buf.Length; i++)
            {
                // 1文字ずつキューに保存
                this.m_strings_queue.Enqueue(buf.Substring(i,1));
            }

            // 解析
            if (!this.Parse())
            {
                // 異常終了
                return false;
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Parse(string)");
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(StreamReader stream)
        {
            Trace.WriteLine("=>>>> JsonParser::Parse(StreamReader)");

            // 読込できなくなるまで繰り返す
            while (stream.Peek() >= 0)
            {
                // 1行ずつ読込
                string _stringline = stream.ReadLine();

                // 解析
                if (!this.Parse(_stringline))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Parse(StreamReader)");
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(StringReader stream)
        {
            Trace.WriteLine("=>>>> JsonParser::Parse(StringReader)");

            // 読込できなくなるまで繰り返す
            while (stream.Peek() >= 0)
            {
                // 1行ずつ読込
                string _stringline = stream.ReadLine();

                // 解析
                if(!this.Parse(_stringline))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Parse(StringReader)");
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(MemoryStream stream)
        {
            Trace.WriteLine("=>>>> JsonParser::Parse(MemoryStream)");

            // 文字列を取得
            string _memoryString = this.m_encoding.GetString(stream.ToArray());

            // StringReaderオブジェクト生成
            StringReader _StringReader = new StringReader(_memoryString);

            // 解析
            if(!this.Parse(_StringReader))
            {
                // 異常終了
                return false;
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Parse(MemoryStream)");
            return true;
        }
        #endregion

        /// <summary>
        /// 空白スキップ
        /// </summary>
        private void Skip_blank()
        {
            Trace.WriteLine("=>>>> JsonParser::skip_blank()");

            // 1文字ずつ処理する
            while (this.m_strings_queue.Count > 0)
            {
                // 空白の場合
                if (this.m_strings_queue.First() == " " || this.m_strings_queue.First() == "\t")
                {
                    // 先頭文字削除
                    this.m_strings_queue.Dequeue();

                    // カラム数更新
                    this.m_column_no++;

                    // スキップ
                    continue;
                }
                break;
            }

            Trace.WriteLine("<<<<= JsonParser::skip_blank()");
        }

        /// <summary>
        /// 1文字取得
        /// </summary>
        /// <returns></returns>
        string Get_string()
        {
            // カラム数更新
            this.m_column_no++;

            // 先頭文字取得(キューから削除)
            string _result = this.m_strings_queue.Dequeue();

            // 文字返却
            return _result;
        }

        /// <summary>
        /// 開始タグ検索
        /// </summary>
        /// <returns></returns>
        private bool Find_start_tag()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_start_tag()");

            // 空白スキップ
            this.Skip_blank();

            // 1文字ずつ処理する
            while (this.m_strings_queue.Count > 0)
            {
                // 1文字取得
                string _column = this.Get_string();

                // 開始タグ(Object)の場合
                if (_column == "{")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // Object開始
                    if(!this.StartObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 開始タグあり(Object開始)
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartArray;

                    // 配列開始
                    if(!this.StartArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 開始タグあり(Array開始)
                    return true;
                }
            }

            // 開始タグなし
            Trace.WriteLine("<<<<= JsonParser::Find_start_tag()");
            return false;
        }

        /// <summary>
        /// 終了タグ検索
        /// </summary>
        /// <returns></returns>
        private bool Find_end_tag()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_end_tag()");

            // 空白スキップ
            this.Skip_blank();

            // 1文字ずつ処理する
            while (this.m_strings_queue.Count > 0)
            {
                // 1文字取得
                string _column = this.Get_string();

                // 終了タグ(Object)の場合
                if (_column == "}")
                {
                    // 状態更新
                    this.m_status = JsonStatus.EndObject;

                    // Object終了
                    if(!this.EndObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 終了タグあり(Object終了)
                    Trace.WriteLine("<<<<= JsonParser::Find_end_tag()");
                    return true;
                }
                // 終了タグ(Array)の場合
                else if (_column == "]")
                {
                    // 状態更新
                    this.m_status = JsonStatus.EndArray;

                    // Array終了
                    if(!this.EndArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 終了タグあり(Array終了)
                    Trace.WriteLine("<<<<= JsonParser::Find_end_tag()");
                    return true;
                }
            }

            // 終了タグなし
            return false;
        }
        /// <summary>
        /// Key検索
        /// </summary>
        /// <returns></returns>
        private bool Find_object_key()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_object_key()");

            // 空白スキップ
            this.Skip_blank();

            // 1文字ずつ処理する
            while (this.m_strings_queue.Count > 0)
            {
                // 1文字取得
                string _column = this.Get_string();

                // "の場合
                if (_column == "\"")
                {
                    string _key = string.Empty;

                    // String検索
                    if (!this.Find_string(true, ref _key))
                    {
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.Object;

                    // キー処理
                    if(!this.Key(_key))
                    {
                        // 異常終了
                        return false;
                    }

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_key()");
                    return true;
                }
                // 終了タグ(Object)の場合
                else if (_column == "}")
                {
                    // 状態更新
                    this.m_status = JsonStatus.EndObject;

                    // オブジェクト終了
                    if(!this.EndObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_key()");
                    return true;
                }
            }

            // Keyなし
            return false;
        }

        /// <summary>
        /// Value開始検索
        /// </summary>
        /// <returns></returns>
        private bool Find_start_value()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_start_value()");

            // 空白スキップ
            this.Skip_blank();

            // 1文字ずつ処理する
            while (this.m_strings_queue.Count > 0)
            {
                // 1文字取得
                string _column = this.Get_string();

                // :の場合
                if (_column == ":")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartValue;

                    // Value値開始
                    Trace.WriteLine("<<<<= JsonParser::Find_start_value()");
                    return true;
                }
            }

            // Value開始なし
            return false;
        }

        /// <summary>
        /// Value検索
        /// </summary>
        /// <returns></returns>
        private bool Find_object_value()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_object_value()");

            StringBuilder _value_stream = new StringBuilder();
            uint _start_colum;
            bool _result = false;

            // 空白スキップ
            this.Skip_blank();

            // 開始位置設定
            _start_colum = this.m_column_no;

            // 1文字ずつ処理する
            while (this.m_strings_queue.Count > 0)
            {
                // 1文字取得
                string _column = this.Get_string();

                // "の場合
                if (_column == "\"")
                {
                    string _value = string.Empty;
                    _start_colum = this.m_column_no;

                    // String検索
                    if (!this.Find_string(true, ref _value))
                    {
                        return false;
                    }

                    // String処理
                    if(!this.String(_start_colum, _value))
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndValue;

                    // 結果更新
                    _result = true;
                    continue;
                }
                // ,の場合
                else if (_column == ",")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 設定
                    if (!_result)
                    {
                        // Value処理
                        if(!this.Value(_start_colum, _value_stream.ToString().Trim()))
                        {
                            // 異常終了
                            return false;
                        }
                    }

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }
                // 開始タグ(Object)の場合
                else if (_column == "{")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 設定
                    if (!_result)
                    {
                        // オブジェクト開始
                        if(!this.StartObject())
                        {
                            // 異常終了
                            return false;
                        }
                    }

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }
                // 終了タグ(Object)の場合
                else if (_column == "}")
                {
                    // 状態更新
                    this.m_status = JsonStatus.EndObject;

                    // 設定
                    if (!_result)
                    {
                        // Value処理
                        if(!this.Value(_start_colum, _value_stream.ToString().Trim()))
                        {
                            // 異常終了
                            return false;
                        }
                    }

                    // オブジェクト終了
                    if(!this.EndObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }

                // 文字列を保存
                _value_stream.Append(_column);
            }

            // 設定
            if (!_result)
            {
                // Value処理
                if(!this.Value(_start_colum, _value_stream.ToString().Trim()))
                {
                    // 異常終了
                    return false;
                }
                _result = true;

                // 状態更新
                this.m_status = JsonStatus.EndValue;
            }

            // 結果を返却
            Debug.WriteLine("　result:[{0}]", _result);
            Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
            return _result;
        }

        /// <summary>
        /// String検索
        /// </summary>
        /// <param name="string_start"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool Find_string(bool string_start, ref string key)
        {
            Trace.WriteLine("=>>>> JsonParser::Find_string(bool, out string)");

            StringBuilder _result = new StringBuilder();
            bool _escape = false;

            // 空白スキップ
            if(!string_start)
            {
                this.Skip_blank();
            }

            // 1文字ずつ処理する
            while (this.m_strings_queue.Count > 0)
            {
                // 1文字取得
                string _column = this.Get_string();

                // 文字列開始判定
                if (string_start)
                {
                    // "の場合
                    if (_column == "\"")
                    {
                        // 直前にエスケープされているか？
                        if(!_escape)
                        {
                            // エスケープされていない場合、文字列終了
                            key = _result.ToString();
                            Debug.WriteLine("　Key:[" + key + "]");
                            Trace.WriteLine("<<<<= JsonParser::Find_string(bool, out string)");
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
                            // エスケープされていない場合、エスケープ文字設定
                            _escape = true;
                        }
                        else
                        {
                            // されている場合は文字扱いなので、文字列に追加
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
                else
                {
                    // "の場合
                    if (_column == "\"")
                    {
                        // 開始位置を設定
                        string_start = true;
                    }
                    else
                    {
                        // 文字列が始まっていないのでスキップ
                        continue;
                    }
                }
            }

            // 該当なし
            return false;
        }

        /// <summary>
        /// Object開始
        /// </summary>
        /// <returns></returns>
        private bool StartObject()
        {
            Trace.WriteLine("=>>>> JsonParser::StartObject()");

            // TODO:未実装

            // 正常尾終了
            Trace.WriteLine("<<<<= JsonParser::StartObject()");
            return true;
        }

        /// <summary>
        /// Object終了
        /// </summary>
        /// <returns></returns>
        private bool EndObject()
        {
            Trace.WriteLine("=>>>> JsonParser::EndObject()");

            // キースタックサイズ判定
            if (this.m_key_stack.Count > 0)
            {
                if (!this.EndObject(this.m_key_stack.Peek()))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::EndObject()");
            return true;
        }

        /// <summary>
        /// Object終了
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool EndObject(string key)
        {
            Trace.WriteLine("=>>>> JsonParser::EndObject(string)");

            // キースタック解放
            this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnEndObject != null)
            {
                Trace.WriteLine("<<<<<this.OnEndObject>>>>>");
                if (!this.OnEndObject(this.m_line_no, this.m_column_no - (uint)key.Length, key, this.m_user_data))
                {
                    // 異常終了
                    return true;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::EndObject(string)");
            return true;
        }

        /// <summary>
        /// Array開始
        /// </summary>
        /// <returns></returns>
        private bool StartArray()
        {
            Trace.WriteLine("=>>>> JsonParser::StartArray()");

            // TODO:未実装

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::StartArray()");
            return true;
        }

        /// <summary>
        /// Array終了
        /// </summary>
        /// <returns></returns>
        private bool EndArray()
        {
            Trace.WriteLine("=>>>> JsonParser::EndArray()");

            // TODO:未実装

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::EndArray()");
            return true;
        }

        /// <summary>
        /// Key処理
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool Key(string key)
        {
            Trace.WriteLine("=>>>> JsonParser::Key(string)");

            // キースタック設定
            this.m_key_stack.Push(key);

            // コールバック関数呼び出し
            if (OnKey != null)
            {
                Trace.WriteLine("<<<<<this.OnKey>>>>>");
                if (!this.OnKey(this.m_line_no, this.m_column_no - (uint)key.Length, key, this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Key(string)");
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
            Trace.WriteLine("=>>>> JsonParser::Value(uint, string)");

            // NULL判定
            if (value == "null")
            {
                // Null処理
                if (!this.Null(startColum))
                {
                    // 異常終了
                    return false;
                }

                // 正常終了
                Trace.WriteLine("<<<<= JsonParser::Value(uint, string)");
                return true;
            }

            // 値判定(bool)
            string _bool_tolower_value = value.ToLower();
            if (_bool_tolower_value == "true" || _bool_tolower_value == "false")
            {
                // bool処理
                if (!this.Bool(startColum, _bool_tolower_value))
                {
                    // 異常終了
                    return false;
                }

                // 正常終了
                Trace.WriteLine("<<<<= JsonParser::Value(uint, string)");
                return true;
            }

            // 現在キーを取得(キースタック削除)
            string _current_key = this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnValue != null)
            {
                Trace.WriteLine("<<<<<this.OnValue>>>>>");
                if (!this.OnValue(this.m_line_no, startColum, _current_key, value, this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Value(uint, string)");
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
            Trace.WriteLine("=>>>> JsonParser::String(uint, string)");

            // 現在キーを取得(キースタック削除)
            string _current_key = this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnString != null)
            {
                Trace.WriteLine("<<<<<this.OnString>>>>>");
                if (!this.OnString(this.m_line_no, startColum, _current_key, value, this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::String(uint, string)");
            return true;
        }

        /// <summary>
        /// Null処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <returns></returns>
        bool Null(uint startColum)
        {
            Trace.WriteLine("=>>>> JsonParser::Null(uint)");

            // 現在キーを取得(キースタック削除)
            string _current_key = this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnNull != null)
            {
                Trace.WriteLine("<<<<<this.OnNull>>>>>");
                if (!this.OnNull(this.m_line_no, startColum, _current_key, this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Null(uint)");
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
            Trace.WriteLine("=>>>> JsonParser::Bool(uint, string)");

            bool _bool_value = false;

            // 現在キーを取得(キースタック削除)
            string _current_key = this.m_key_stack.Pop();

            // 文字列を小文字化
            string _bool_tolower_value = value.ToLower();
            if (_bool_tolower_value == "true")
            {
                _bool_value = true;
            }
            else if (_bool_tolower_value == "false")
            {
                _bool_value = false;
            }
            else
            {
                // 異常終了
                return false;
            }

            // bool処理
            if (!this.Bool(startColum, _current_key, value, _bool_value))
            {
                // 異常終了
                return false;
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Bool(uint, string)");
            return true;
        }

        /// <summary>
        /// Bool処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="key"></param>
        /// <param name="boolstring"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool Bool(uint startColum, string key, string boolstring, bool value)
        {
            Trace.WriteLine("=>>>> JsonParser::Bool(uint, string, string, bool)");

            // コールバック関数呼び出し
            if (this.OnBool != null)
            {
                Trace.WriteLine("<<<<<this.OnBool>>>>>");
                if (!this.OnBool(this.m_line_no, startColum, key, value, this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Bool(uint, string, string, bool)");
            return true;
        }

        /// <summary>
        /// Number処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool Number(uint startColum, string value)
        {
            Trace.WriteLine("=>>>> JsonParser::Number(uint, string)");

            // TODO:未実装

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Number(uint, string)");
            return true;
        }
    }
}
