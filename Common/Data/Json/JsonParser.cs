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
        NotStart = 0,               //   0:解析未実施
        StartObject,                //   1:Object開始
        Object,                     //   2:Object処理
        EndObject,                  //   3:Object終了
        StartArray,                 //   4:Array開始
        Array,                      //   5:Array処理
        EndArray,                   //   6:Array終了
        EndKey,                     //   7:Key終了
        StartValue,                 //   8:Value開始
        Value,                      //   9:Value処理
        EndValue,                   //  10:Value終了
        EndArrayValue,              //  11:Value終了(Array)
        NextWait,                   //  12:次遷移待ち
    };

    /// <summary>
    /// 動作モード
    /// </summary>
    public enum JsonMode
    {
        Non = 0,                    //   0:設定なし
        Object,                     //   1:Objectモード
        Array,                      //   2:Arrayモード
    };

    /// <summary>
    /// Value種別
    /// </summary>
    public enum JsonValueType
    {
        Unknown = 0,                //   0:不明
        String,                     //   1:string
        Number,                     //   2:number
        Object,                     //   3:object
        Array,                      //   4:array
        Bool,                       //   5:true/false
        Null,                       //   6:null
    };

    /// <summary>
    /// エラーコード
    /// </summary>
    public enum JsonError
    {
        NormalEnd = 0,              //   0:正常終了
        NotFoundStartTag,           //   1:開始タグなしエラー
        NotFindEndTag,              //   2:終了タグなしエラー
        NotFoundKey,                //   3:Keyなしエラー
        NotFoundValue,              //   4:Valueなしエラー
        InvalidSyntax,              //   5:構文エラー
        IndexOutOfRange,            //   6:インデックス範囲エラー
        SystemError                 //   7:システムエラー
    };

    #region イベントハンドラ
    public delegate bool EventHandlerStartObject(uint lineNo, uint startColum, object userData);
    public delegate bool EventHandlerEndObject(uint lineNo, uint startColum, string key, object userData);
    public delegate bool EventHandlerStartArray(uint lineNo, uint startColum, object userData);
    public delegate bool EventHandlerEndArray(uint lineNo, uint startColum, object userData);
    public delegate bool EventHandlerKey(uint lineNo, uint startColum, string key, object userData);
    public delegate bool EventHandlerNull(uint lineNo, uint startColum, string key, object userData);
    public delegate bool EventHandlerBool(uint lineNo, uint startColum, string key, bool value, object userData);
    public delegate bool EventHandlerNumber(uint lineNo, uint startColum, string key, string value, object userData);
    public delegate bool EventHandlerValue(uint lineNo, uint startColum, string key, string value, object userData);
    public delegate bool EventHandlerString(uint lineNo, uint startColum, string key, string value, object userData);
    public delegate void EventHandlerError(JsonError error, uint lineNo, uint startColum, string value, object userData);
    #endregion

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
            if (this.m_stack.Count() < 1)
            {
                // 異常終了(例外)
                throw new IndexOutOfRangeException();
            }

            // 動作モード返却
            return this.m_stack[this.m_stack.Count - 1];
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
            // モードスタック解放
            this.m_stack.RemoveAt(this.m_stack.Count - 1);

            // 正常終了
            return true;
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
        /// 文字列Queue
        /// </summary>
        private JsonStringQueue m_strings_queue = new JsonStringQueue();

        /// <summary>
        /// 解析状態
        /// </summary>
        private JsonStatus m_status = JsonStatus.NotStart;

        /// <summary>
        /// モードスタック
        /// </summary>
        JsonStack<JsonMode> m_mode_stack = new JsonStack<JsonMode>();

        /// <summary>
        /// Arrayインデックススタック
        /// </summary>
        JsonStack<int> m_array_index_stack = new JsonStack<int>();

        /// <summary>
        /// ユーザデータ
        /// </summary>
        private object m_user_data = null;

        /// <summary>
        /// Keyスタック
        /// </summary>
        JsonStack<JsonKeyInfo> m_key_stack = new JsonStack<JsonKeyInfo>();

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

        /// <summary>
        /// 処理行数取得
        /// </summary>
        /// <returns></returns>
        private uint Total_line_no()
        {
            Debug.WriteLine("行数オフセット:{0}", this.m_offset_line_no);
            Debug.WriteLine("行数          :{0}", this.m_line_no);
            return this.m_offset_line_no + this.m_line_no;
        }
        
        /// <summary>
        /// Number判定
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        bool Is_number(string str)
        {
            Trace.WriteLine("=>>>> JsonParser::is_number(string)");

            // 実数正規表現
            if (Regex.IsMatch(str, @"[\-]*[0-9]*\.[0-9]*$"))
            {
                // 一致
                Trace.WriteLine("<<<<= JsonParser::is_number(string)");
                return true;
            }
            // 指数正規表現
            else if (Regex.IsMatch(str, @"[\-]*[0-9]+(\.[0-9]*)*([eE][+-]*[0-9]+)*"))
            {
                // 一致
                Trace.WriteLine("<<<<= JsonParser::is_number(string)");
                return true;
            }
            // 整数正規表現
            else if (Regex.IsMatch(str, @"[\-]*[0-9]+$"))
            {
                // 一致
                Trace.WriteLine("<<<<= JsonParser::is_number(string)");
                return true;
            }

            // 不一致
            return false;
        }

        /// <summary>
        /// 配列インデックス更新
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="startColum"></param>
        /// <returns></returns>
        bool Update_array_index(JsonMode mode, uint startColum)
        {
            Trace.WriteLine("=>>>> Json::update_array_index(JsonMode)");

            // 動作モードを判定
            if (mode == JsonMode.Array)
            {
                // 現在のインデックス値を取得
                int _current_index = this.m_array_index_stack.Get();

                // キーの更新処理
                StringBuilder _Key = new StringBuilder(_current_index.ToString());
                if (!this.Key(startColum, _Key.ToString()))
                {
                    // 異常終了
                    return false;
                }

                // 現在のインデックス値を更新
                this.m_array_index_stack[this.m_array_index_stack.Count() - 1] = _current_index + 1;
            }
            else
            {
                // 異常終了
                this.Error(JsonError.SystemError);
                return false;
            }

            // 正常終了
            Trace.WriteLine("<<<<= Json::update_array_index(JsonMode)");
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
            Trace.WriteLine("=>>>> JsonParser::Parse(string)");
            Debug.WriteLine("　buf:[" + buf + "]");

            // 文字列保存
            this.m_raw_string = buf;

            // 行数更新
            this.m_line_no++;

            // 文字列サイズを判定
            if (this.m_raw_string.Length <= 0)
            {
                // 空行なので処理する必要なし(正常終了)
                Trace.WriteLine("<<<<= JsonParser::Parse(string)");
                return true;
            }

            // 文字列キューを設定
            this.m_strings_queue.Set(this.m_raw_string);

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

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
        /// <returns></returns>
        private bool Parse()
        {
            Trace.WriteLine("=>>>> JsonParser::Parse()");

            // 文字列サイズを判定
            if (this.m_strings_queue.Length() == 0)
            {
                // 正常終了(サイズが0で呼ばれたら処理なし)
                Trace.WriteLine("<<<<= JsonParser::Parse()");
                return true;
            }

            // 状態で分岐
            Debug.WriteLine("　状態:" + this.m_status.ToString());
            switch (this.m_status)
            {
                //「解析未実施」状態の場合
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
                //「Object開始」の場合
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
                //「Object終了」の場合
                case JsonStatus.EndObject:
                    {
                        // 次開始検索(Object)
                        if (!this.Find_object_next_start())
                        {
                            // 次の開始タグなしのため、正常終了
                            Trace.WriteLine("<<<<= JsonParser::Parse()");
                            return true;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Array開始」の場合
                case JsonStatus.StartArray:
                    {
                        // Value検索(Array)
                        if (!this.Find_array_value())
                        {
                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Array終了」の場合
                case JsonStatus.EndArray:
                    {
                        // 次開始検索(Array)
                        if (!this.Find_array_next_start())
                        {
                            // 次の開始タグなしのため、正常終了
                            return true;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Key終了」の場合
                case JsonStatus.EndKey:
                    {
                        // Value開始検索
                        if (!this.Find_object_start_value())
                        {
                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Value開始」の場合
                case JsonStatus.StartValue:
                    {
                        // Value検索(Object)
                        if (!this.Find_object_value())
                        {
                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Value終了」の場合
                case JsonStatus.EndValue:
                    {
                        // 終了タグ検索(Object)
                        if (!this.Find_object_end_tag())
                        {
                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「Value終了(Array)」の場合
                case JsonStatus.EndArrayValue:
                    {
                        // 次開始タグ検索
                        if (!this.Find_array_start_value())
                        {
                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                //「次遷移待ち」の場合
                case JsonStatus.NextWait:
                    {
                        // 次状態検索
                        if (!this.Find_next_state())
                        {
                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return this.Parse();
                    }
                default:
                    {
                        // 異常終了
                        this.Error(JsonError.SystemError);
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
                if (!this.Parse(_stringline))
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
            if (!this.Parse(_StringReader))
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
        /// 開始タグ検索
        /// </summary>
        /// <returns></returns>
        private bool Find_start_tag()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_start_tag()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

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
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_start_tag()");
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // 配列開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_start_tag()");
                    return true;
                }
            }

            // 開始タグなし
            return false;
        }

        /// <summary>
        /// 終了タグ検索
        /// </summary>
        /// <returns></returns>
        private bool Find_object_end_tag()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_object_end_tag()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 終了タグ(Object)の場合
                if (_column == "}")
                {
                    // オブジェクト終了
                    if (!this.EndObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_end_tag()");
                    return true;
                }
                // 次Value存在有の場合
                else if (_column == ",")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_end_tag()");
                    return true;
                }
                // この時点で見つかっていなければ終了タグなしエラー
                break;
            }

            // 終了タグなし
            this.Error(JsonError.NotFindEndTag);
            return false;
        }

        /// <summary>
        /// 終了タグ検索
        /// </summary>
        /// <returns></returns>
        private bool Find_array_end_tag()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_array_end_tag()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

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
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_end_tag()");
                    return true;
                }
                // 次Value存在有の場合
                else if (_column == ",")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_end_tag()");
                    return true;
                }
                // この時点で見つかっていなければ終了タグなしエラー
                break;
            }

            // 終了タグなし
            this.Error(JsonError.NotFindEndTag);
            return false;
        }

        /// <summary>
        /// 次開始検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool Find_object_next_start()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_object_next_start()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 次Objectあり','の場合
                if (_column == ",")
                {
                    // 状態更新
                    this.m_status = JsonStatus.NextWait;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_next_start()");
                    return true;
                }
                // 終了タグ(Object)の場合
                else if (_column == "}")
                {
                    // オブジェクト終了
                    if (!this.EndObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_next_start()");
                    return true;
                }
                // この時点で見つかっていなければ次Objectなし
                break;
            }

            // 次Objectなし
            return false;
        }

        /// <summary>
        /// 次開始検索(Array)
        /// </summary>
        /// <returns></returns>
        private bool Find_array_next_start()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_object_next_start()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 次Arrayあり','の場合
                if (_column == ",")
                {
                    // 状態更新
                    this.m_status = JsonStatus.NextWait;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_next_start()");
                    return true;
                }
                // 終了タグ(Array)の場合
                else if (_column == "]")
                {
                    // 配列終了
                    if (!this.EndArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_next_start()");
                    return true;
                }
                // この時点で見つかっていなければ次Arrayなし
                break;
            }

            // 次Arrayなし
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
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // "の場合
                if (_column == "\"")
                {
                    string _key = string.Empty;

                    // 開始位置設定
                    uint _start_colum = this.m_strings_queue.CurrentColum();

                    // String検索
                    if (!this.Find_string(ref _key))
                    {
                        return false;
                    }

                    // キー処理
                    if (!this.Key(_start_colum, _key))
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndKey;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_key()");
                    return true;
                }
                // 終了タグ(Object)の場合
                else if (_column == "}")
                {
                    // オブジェクト終了
                    if (!this.EndObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_key()");
                    return true;
                }
            }

            // Keyなし
            this.Error(JsonError.NotFoundKey);
            return false;
        }

        /// <summary>
        /// Value開始検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool Find_object_start_value()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_object_start_value()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // :の場合
                if (_column == ":")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartValue;

                    // Object値開始
                    Trace.WriteLine("<<<<= JsonParser::Find_object_start_value()");
                    return true;
                }
                // この時点で見つかっていなければ構文エラー
                break;
            }

            // 次Value開始タグなし
            this.Error(JsonError.InvalidSyntax);
            return false;
        }

        /// <summary>
        /// Value開始検索(Array)
        /// </summary>
        /// <returns></returns>
        private bool Find_array_start_value()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_array_start_value()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // 次Arrayあり','の場合
                if (_column == ",")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_start_value()");
                    return true;
                }
                // 終了タグ(Array)の場合
                else if (_column == "]")
                {
                    // 配列終了
                    if (!this.EndArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_start_value()");
                    return true;
                }
                // この時点で見つかっていなければ次の開始なし
                break;
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
            uint _start_colum = this.m_strings_queue.CurrentColum();
            JsonStatus _JsonStatus = JsonStatus.EndValue;

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // "の場合
                if (_column == "\"")
                {
                    string _value = string.Empty;

                    // 開始位置設定
                    _start_colum = this.m_strings_queue.CurrentColum();

                    // String検索
                    if (!this.Find_string(ref _value))
                    {
                        return false;
                    }

                    // String処理
                    if (!this.String(_start_colum, _value))
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndValue;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }
                // 開始タグ(Object)の場合
                else if (_column == "{")
                {
                    // オブジェクト開始
                    if (!this.StartObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }
                // 終了タグ(Object)の場合
                else if (_column == "[")
                {
                    // 配列開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }                // ,の場合
                else if (_column == ",")
                {
                    // 状態更新
                    this.m_status = JsonStatus.StartObject;
                    break;
                }
                // "}"の場合
                else if (_column == "}")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.EndObject;
                    break;
                }
                // "]"の場合
                else if (_column == "]")
                {
                    // 異常終了
                    this.Error(JsonError.SystemError);
                    return false;
                }

                // 文字列を保存
                _value_stream.Append(_column);
            }

            // true、またはfalse、nullの場合
            if (_value_stream.ToString() == "true" || _value_stream.ToString() == "false" || _value_stream.ToString() == "null")
            {
                // Value処理
                if (!this.Value(_start_colum, _value_stream.ToString()))
                {
                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }

                // 状態更新
                this.m_status = _JsonStatus;

                // 正常終了
                Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                return true;
            }
            // Numberの場合
            else if (this.Is_number(_value_stream.ToString()))
            {
                // Number処理
                if (!this.Number(_start_colum, _value_stream.ToString()))
                {
                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                    return true;
                }

                // 状態更新
                this.m_status = _JsonStatus;

                // 正常終了
                Trace.WriteLine("<<<<= JsonParser::Find_object_value()");
                return true;
            }

            // Valueなし
            this.Error(JsonError.NotFoundValue);
            return false;
        }

        /// <summary>
        /// Value検索(Array)
        /// </summary>
        /// <returns></returns>
        private bool Find_array_value()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_array_value()");

            StringBuilder _value_stream = new StringBuilder();
            uint _start_colum = this.m_strings_queue.CurrentColum();
            JsonStatus _JsonStatus = JsonStatus.EndArrayValue;

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // Stringの場合
                if (_column == "\"")
                {
                    string _value = string.Empty;

                    // 開始位置設定
                    _start_colum = this.m_strings_queue.CurrentColum();

                    // 配列インデックス更新
                    if (!this.Update_array_index(JsonMode.Array, _start_colum))
                    {
                        // 異常終了
                        this.Error(JsonError.SystemError);
                        return false;
                    }

                    // String検索
                    if (!this.Find_string(ref _value))
                    {
                        // 異常終了
                        return false;
                    }

                    // String処理
                    if (!this.String(_start_colum, _value))
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.EndArrayValue;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_value()");
                    return true;
                }
                // Object開始の場合
                else if (_column == "{")
                {
                    // 配列インデックス更新
                    if (!this.Update_array_index(JsonMode.Array, _start_colum))
                    {
                        // 異常終了
                        this.Error(JsonError.SystemError);
                        return false;
                    }

                    // 開始位置
                    if (!this.StartObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_value()");
                    return true;
                }
                // Array開始の場合
                else if (_column == "[")
                {
                    // 配列インデックス更新
                    if (!this.Update_array_index(JsonMode.Array, _start_colum))
                    {
                        // 異常終了
                        this.Error(JsonError.SystemError);
                        return false;
                    }

                    // 配列開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_value()");
                    return true;
                }
                // ","の場合
                else if (_column == ",")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.StartArray;
                    break;
                }
                // "}"の場合
                else if (_column == "}")
                {
                    // 異常終了
                    this.Error(JsonError.InvalidSyntax);
                    return false;
                }
                // "]"の場合
                else if (_column == "]")
                {
                    // 状態更新
                    _JsonStatus = JsonStatus.EndArray;
                    break;
                }

                // 文字列を保存
                _value_stream.Append(_column);
            }

            // true、またはfalse、nullの場合
            if (_value_stream.ToString() == "true" || _value_stream.ToString() == "false" || _value_stream.ToString() == "null")
            {
                // Value処理
                if (!this.Value(_start_colum, _value_stream.ToString()))
                {
                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_value()");
                    return true;
                }

                // 状態更新
                this.m_status = _JsonStatus;

                // 正常終了
                Trace.WriteLine("<<<<= JsonParser::Find_array_value()");
                return true;
            }
            // Numberの場合
            else if (this.Is_number(_value_stream.ToString()))
            {
                // Number処理
                if (!this.Number(_start_colum, _value_stream.ToString()))
                {
                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_array_value()");
                    return true;
                }

                // 状態更新
                this.m_status = _JsonStatus;

                // 正常終了
                Trace.WriteLine("<<<<= JsonParser::Find_array_value()");
                return true;
            }

            // Valueなし
            this.Error(JsonError.NotFoundValue);
            return false;
        }

        /// <summary>
        /// 次状態検索
        /// </summary>
        /// <returns></returns>
        private bool Find_next_state()
        {
            Trace.WriteLine("=>>>> JsonParser::Find_next_state()");

            // 空白スキップ
            this.m_strings_queue.SkipBlank();

            // 1文字ずつ処理する
            while (!this.m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = this.m_strings_queue.Get();

                // Object開始の場合
                if (_column == "{")
                {
                    // 開始位置
                    if (!this.StartObject())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_next_state()");
                    return true;
                }
                // Array開始の場合
                else if (_column == "[")
                {
                    // 配列開始
                    if (!this.StartArray())
                    {
                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    this.m_status = JsonStatus.StartArray;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_next_state()");
                    return true;
                }
                // Stringの場合
                else if (_column == "\"")
                {
                    // 「"」をキューに戻す
                    this.m_strings_queue.Put(_column);

                    // 状態更新
                    this.m_status = JsonStatus.StartObject;

                    // 正常終了
                    Trace.WriteLine("<<<<= JsonParser::Find_next_state()");
                    return true;
                }
            }

            // 次状態なし（エラーにはしない）
            Trace.WriteLine("<<<<= JsonParser::Find_next_state()");
            return true;
        }

        /// <summary>
        /// String検索
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool Find_string(ref string key)
        {
            Trace.WriteLine("=>>>> JsonParser::Find_string(ref string)");

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
                        Trace.WriteLine("<<<<= JsonParser::Find_string(out string)");
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
            return false;
        }

        /// <summary>
        /// Object開始
        /// </summary>
        /// <returns></returns>
        private bool StartObject()
        {
            Trace.WriteLine("=>>>> JsonParser::StartObject()");

            // モードスタック設定
            this.m_mode_stack.Push(JsonMode.Object);

            // コールバック関数呼び出し
            if (this.OnStartObject != null)
            {
                // Object開始検出通知
                if (!this.OnStartObject(this.Total_line_no(), this.m_strings_queue.CurrentColum(), this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

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
            if (this.m_key_stack.Count() > 0)
            {
                // Object終了
                if (!this.EndObject(this.m_key_stack.Get().Value))
                {
                    // 異常終了
                    return false;
                }

                // キースタック解放
                this.m_key_stack.Pop();
            }

            // モードスタック解放
            this.m_mode_stack.Pop();

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

            // コールバック関数呼び出し
            if (this.OnEndObject != null)
            {
                // 現在キーを取得
                JsonKeyInfo _current_key = this.m_key_stack.Get();

                // Object終了検出通知
                if (!this.OnEndObject(this.Total_line_no(), _current_key.StartColum, _current_key.Value, this.m_user_data))
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

            // モードスタック設定
            this.m_mode_stack.Push(JsonMode.Array);

            // Arrayインデックススタック設定
            this.m_array_index_stack.Push(0);

            // コールバック関数呼び出し
            if (this.OnStartArray != null)
            {
                // Array開始検出通知
                if (!this.OnStartArray(this.Total_line_no(), this.m_strings_queue.CurrentColum(), this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

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

            // モードスタック解放
            this.m_mode_stack.Pop();

            // Arrayインデックススタック解放
            this.m_array_index_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnEndArray != null)
            {
                // Array終了検出通知
                if (!this.OnEndArray(this.Total_line_no(), this.m_strings_queue.CurrentColum(), this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::EndArray()");
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
            Trace.WriteLine("=>>>> JsonParser::Key(string)");

            // キースタック設定
            JsonKeyInfo _keyInfo = new JsonKeyInfo();
            _keyInfo.StartColum = startColum;
            _keyInfo.Value = key;
            this.m_key_stack.Push(_keyInfo);

            // コールバック関数呼び出し
            if (this.OnKey != null)
            {
                // Key検出通知
                if (!this.OnKey(this.Total_line_no(), startColum, key, this.m_user_data))
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
            if (value == "true" || value == "false")
            {
                // bool処理
                if (!this.Bool(startColum, value))
                {
                    // 異常終了
                    return false;
                }

                // 正常終了
                Trace.WriteLine("<<<<= JsonParser::Value(uint, string)");
                return true;
            }

            // 現在キーを取得
            JsonKeyInfo _current_key = this.m_key_stack.Get();

            // キースタック解放
            this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnValue != null)
            {
                // Value検出通知(Value)
                if (!this.OnValue(this.Total_line_no(), startColum, _current_key.Value, value, this.m_user_data))
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
        /// Number処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool Number(uint startColum, string value)
        {
            Trace.WriteLine("=>>>> JsonParser::Number(uint, string)");

            // 現在キーを取得
            JsonKeyInfo _current_key = this.m_key_stack.Get();

            // キースタック解放
            this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnNumber != null)
            {
                // Value検出通知(Number)
                if (!this.OnNumber(this.Total_line_no(), startColum, _current_key.Value, value, this.m_user_data))
                {
                    // 異常終了
                    return false;
                }
            }

            // 正常終了
            Trace.WriteLine("<<<<= JsonParser::Number(uint, string)");
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

            // 現在キーを取得
            JsonKeyInfo _current_key = this.m_key_stack.Get();

            // キースタック解放
            this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnString != null)
            {
                if (!this.OnString(this.Total_line_no(), startColum, _current_key.Value, value, this.m_user_data))
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

            // 現在キーを取得
            JsonKeyInfo _current_key = this.m_key_stack.Get();

            // キースタック解放
            this.m_key_stack.Pop();

            // コールバック関数呼び出し
            if (this.OnNull != null)
            {
                if (!this.OnNull(this.Total_line_no(), startColum, _current_key.Value, this.m_user_data))
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

            // 現在キーを取得
            JsonKeyInfo _current_key = this.m_key_stack.Get();

            // キースタック解放
            this.m_key_stack.Pop();

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
            if (!this.Bool(startColum, _current_key.Value, value, _bool_value))
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
        /// エラー処理
        /// </summary>
        /// <param name="error"></param>
        private void Error(JsonError error)
        {
            Trace.WriteLine("=>>>> JsonParser::Error(JsonError)");

            // コールバック関数呼び出し
            if (this.OnError != null)
            {
                // Key情報取得
                JsonKeyInfo _keyInfo = this.m_key_stack.Get();

                // エラーイベント通知
                this.OnError(error, this.Total_line_no(), _keyInfo.StartColum, this.m_raw_string, this.m_user_data);
            }

            Trace.WriteLine("<<<<= JsonParser::Error(JsonError)");
        }
    }
}