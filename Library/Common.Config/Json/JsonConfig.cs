using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using log4net;
using System.Reflection;
using log4net.Repository.Hierarchy;
using System.Runtime.InteropServices.ComTypes;

namespace Common.Config
{
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
    public class JsonConfig
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

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
        public JsonConfig()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::JsonConfig()");

            // ロギング
            Logger.Debug("<<<<= JsonConfig::JsonConfig()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userData"></param>
        public JsonConfig(object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::JsonConfig(object)");
            Logger.DebugFormat("userData:[{0}]", userData.ToString());

            // 初期化
            m_user_data = userData;

            // ロギング
            Logger.Debug("<<<<= JsonConfig::JsonConfig(object)");
        }
        #endregion

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JsonConfig()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::~JsonConfig()");

            // 文字列キューをクリア
            m_strings_queue.Clear();

            // ロギング
            Logger.Debug("<<<<= JsonConfig::~JsonConfig()");
        }

        /// <summary>
        /// 処理行数取得
        /// </summary>
        /// <returns></returns>
        private uint total_line_no()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::total_line_no()");

            // 処理行数取得
            uint result = m_offset_line_no + m_line_no;

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= JsonConfig::total_line_no()");

            // 返却
            return result;
        }

        /// <summary>
        /// String判定
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool is_string(string str, ref string value)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::is_string(string, ref string)");
            Logger.DebugFormat("str:[{0}]", str);

            // 初期化
            value = string.Empty;

            // 両端の空白削除
            string _trim_str = str.Trim();

            // 実数正規表現
            if (Regex.IsMatch(_trim_str, "^\\\".*\\\"$"))
            {

                // 両端の"を削除
                value = _trim_str.Trim('"');

                // ロギング
                Logger.DebugFormat("value:[{0}]", value);
                Logger.Debug("<<<<= JsonConfig::is_string(string, ref string)");

                // 一致
                return true;
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::is_string(string, ref string)");

            // 不一致
            return false;
        }

        /// <summary>
        /// Number判定
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool is_number(string str)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::is_number(string)");
            Logger.DebugFormat("str:[{0}]", str);

            // 実数正規表現
            if (Regex.IsMatch(str, @"[\-]*[0-9]*\.[0-9]*$"))
            {
                // ロギング
                Logger.DebugFormat("一致:[実数]");
                Logger.Debug("<<<<= JsonConfig::is_number(string)");

                // 一致
                return true;
            }
            // 指数正規表現
            else if (Regex.IsMatch(str, @"[\-]*[0-9]+(\.[0-9]*)*([eE][+-]*[0-9]+)*"))
            {
                // ロギング
                Logger.DebugFormat("一致:[指数]");
                Logger.Debug("<<<<= JsonConfig::is_number(string)");

                // 一致
                return true;
            }
            // 整数正規表現
            else if (Regex.IsMatch(str, @"[\-]*[0-9]+$"))
            {
                // ロギング
                Logger.DebugFormat("一致:[整数]");
                Logger.Debug("<<<<= JsonConfig::is_number(string)");

                // 一致
                return true;
            }

            // ロギング
            Logger.DebugFormat("不一致");
            Logger.Debug("<<<<= JsonConfig::is_number(string)");

            // 不一致
            return false;
        }

        /// <summary>
        /// Arrayインデックス初期設定
        /// </summary>
        private void initialize_array_index()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::initialize_array_index()");

            // Arrayインデックススタック設定
            JsonArrayInfo _array_info = new JsonArrayInfo();
            _array_info.Value = -1;
            _array_info.Key = "";
            m_array_index_stack.Push(_array_info);

            // ロギング
            Logger.Debug("<<<<= JsonConfig::initialize_array_index()");
        }

        /// <summary>
        /// Arrayインデックス更新
        /// </summary>
        /// <returns></returns>
        private bool update_array_index()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::update_array_index()");

            // 現在のインデックス数を取得
            uint _m_array_index_stack_count = m_array_index_stack.Count();

            // 現在のインデックス数を判定
            if (_m_array_index_stack_count <= 0)
            {
                // エラー処理
                Error(JsonError.SystemError);

                // ロギング
                Logger.Error("現在インデックス数判定:[異常]");
                Logger.Debug("<<<<= JsonConfig::update_array_index()");

                // 異常終了
                return false;
            }

            // 現在のインデックス値を更新
            m_array_index_stack.Increment();

            // ロギング
            Logger.Debug("<<<<= JsonConfig::update_array_index()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// Arrayインデックス更新
        /// </summary>
        /// <param name="startColum"></param>
        /// <returns></returns>
        private bool update_array_index(uint startColum)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::update_array_index(uint)");
            Logger.DebugFormat("startColum:[{0}]", startColum);

            // Arrayインデックス更新
            if (!update_array_index())
            {

                // ロギング
                Logger.Error("Arrayインデックス更新:[異常]");
                Logger.Debug("<<<<= JsonConfig::update_array_index(uint)");

                // 異常終了
                return false;
            }

            // Key値設定
            JsonArrayInfo _array_info = m_array_index_stack.Get();
            string _Key = _array_info.Value.ToString();

            // Element開始
            if (!StartElement(startColum, _Key))
            {
                // ロギング
                Logger.Error("Element開始:[異常]");
                Logger.Debug("<<<<= JsonConfig::update_array_index(uint)");

                // 異常終了
                return false;
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::update_array_index(uint)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// オフセット行数設定
        /// </summary>
        /// <param name="offsetLineNo"></param>
        public void SetOffsetLineNo(uint offsetLineNo)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::SetOffsetLineNo(uint)");
            Logger.DebugFormat("offsetLineNo:[{0}]", offsetLineNo);

            // オフセット行数設定
            m_offset_line_no = offsetLineNo;

            // ロギング
            Logger.Debug("<<<<= JsonConfig::SetOffsetLineNo(uint)");
        }

        #region 解析
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool Parse(string buf)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Parse(string)");
            Logger.DebugFormat("buf:[{0}]", buf);

            // 文字列保存
            m_raw_string = buf;

            // 行数更新
            m_line_no++;

            // 文字列サイズを判定
            if (m_raw_string.Length <= 0)
            {
                // ロギング
                Logger.Debug("<<<<= JsonConfig::Parse(string)");

                // 空行なので処理する必要なし(正常終了)
                return true;
            }

            // 文字列キューを設定
            m_strings_queue.Set(m_raw_string);

            // 解析
            if (!Parse())
            {
                // ロギング
                Logger.Error("解析:[異常]");
                Logger.Debug("<<<<= JsonConfig::Parse(string)");

                // 異常終了
                return false;
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Parse(string)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <returns></returns>
        private bool Parse()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Parse()");

            // 先頭の空白スキップ(空行は処理させないため)
            m_strings_queue.SkipBlank();

            // 文字列サイズを判定
            if (m_strings_queue.Length() == 0)
            {
                // ロギング
                Logger.Debug("<<<<= JsonConfig::Parse()");

                // 正常終了(サイズが0で呼ばれたら処理なし)
                return true;
            }

            // ロギング
            Logger.DebugFormat("《遷移状態⇒{0}[{1}]》", m_status_stack.Get(), m_status_stack.ToString());

            // 状態で分岐
            switch (m_status_stack.Get())
            {
                //「解析未実施」状態の場合
                case JsonStatus.NotStart:
                    {
                        // 開始タグ検索
                        if (!find_start_tag())
                        {
                            // ロギング
                            Logger.Error("開始タグ検索:[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Object開始」の場合
                case JsonStatus.StartObject:
                    {
                        // Objectキー検索
                        if (!find_object_key())
                        {
                            // ロギング
                            Logger.Error("Objectキー検索:[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「次Object開始」の場合
                case JsonStatus.NextObject:
                    {
                        // 次Key検索
                        if (!find_object_next_key())
                        {
                            // ロギング
                            Logger.Error("次Key検索:[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Object終了」の場合
                case JsonStatus.EndObject:
                    {
                        // 次開始検索(Object)
                        if (!find_object_next_start())
                        {
                            // ロギング
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 次の開始タグなしのため、正常終了
                            return true;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Array開始」の場合
                case JsonStatus.StartArray:
                    {
                        // Value検索(Array)
                        if (!find_array_value())
                        {
                            // ロギング
                            Logger.Error("Value検索(Array):[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「次Array開始」の場合
                case JsonStatus.NextArray:
                    {
                        // Value検索(Array)
                        if (!find_array_value())
                        {
                            // ロギング
                            Logger.Error("Value検索(Array):[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Array終了」の場合
                case JsonStatus.EndArray:
                    {
                        // 次開始検索(Array)
                        if (!find_array_next_start())
                        {
                            // ロギング
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 次の開始タグなしのため、正常終了
                            return true;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Key終了」の場合
                case JsonStatus.EndKey:
                    {
                        // Value開始検索
                        if (!find_object_start_value())
                        {
                            // ロギング
                            Logger.Error("Value開始検索:[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Value開始」の場合
                case JsonStatus.StartValue:
                    {
                        // Value検索(Object)
                        if (!find_object_value())
                        {
                            // ロギング
                            Logger.Error("Value検索(Object):[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Value終了」の場合
                case JsonStatus.EndValue:
                    {
                        // 終了タグ検索(Object)
                        if (!find_object_end_tag())
                        {
                            // ロギング
                            Logger.Error("終了タグ検索(Object):[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「Value終了(Array)」の場合
                case JsonStatus.EndArrayValue:
                    {
                        // 次開始タグ検索
                        if (!find_array_end_tag())
                        {
                            // ロギング
                            Logger.Error("次開始タグ検索:[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「次遷移待ち」の場合
                case JsonStatus.NextWait:
                    {
                        // 次状態検索
                        if (!find_next_state())
                        {
                            // ロギング
                            Logger.Error("次状態検索:[異常]");
                            Logger.Debug("<<<<= JsonConfig::Parse()");

                            // 異常終了
                            return false;
                        }
                        // 次ステージに遷移
                        return Parse();
                    }
                //「解析終了」の場合
                case JsonStatus.End:
                    {
                        // エラー処理
                        Error(JsonError.InvalidSyntax);

                        // ロギング
                        Logger.Error("解析状態:[異常]");
                        Logger.Debug("<<<<= JsonConfig::Parse()");

                        // 異常終了
                        return false;
                    }
                default:
                    {
                        // エラー処理
                        Error(JsonError.SystemError);

                        // ロギング
                        Logger.Error("解析状態:[異常]");
                        Logger.Debug("<<<<= JsonConfig::Parse()");

                        // 異常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Parse(StreamReader)");
            Logger.DebugFormat("stream:[{0}]", stream.ToString());

            // 読込できなくなるまで繰り返す
            while (stream.Peek() >= 0)
            {
                // 1行ずつ読込
                string _stringline = stream.ReadLine();

                // 解析
                if (!Parse(_stringline))
                {
                    // ロギング
                    Logger.Error("解析:[異常]");
                    Logger.Debug("<<<<= JsonConfig::Parse(StreamReader)");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Parse(StreamReader)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(StringReader stream)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Parse(StringReader)");
            Logger.DebugFormat("stream:[{0}]", stream.ToString());

            // 読込できなくなるまで繰り返す
            while (stream.Peek() >= 0)
            {
                // 1行ずつ読込
                string _stringline = stream.ReadLine();

                // 解析
                if (!Parse(_stringline))
                {
                    // ロギング
                    Logger.Error("解析:[異常]");
                    Logger.Debug("<<<<= JsonConfig::Parse(StringReader)");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Parse(StringReader)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool Parse(MemoryStream stream)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Parse(MemoryStream)");
            Logger.DebugFormat("stream:{0}", stream.ToString());

            // 文字列を取得
            string _memoryString = m_encoding.GetString(stream.ToArray());

            // StringReaderオブジェクト生成
            StringReader _StringReader = new StringReader(_memoryString);

            // 解析
            if (!Parse(_StringReader))
            {
                // ロギング
                Logger.Error("解析:[異常]");
                Logger.Debug("<<<<= JsonConfig::Parse(MemoryStream)");

                // 異常終了
                return false;
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Parse(MemoryStream)");

            // 正常終了
            return true;
        }
        #endregion

        /// <summary>
        /// 開始タグ検索
        /// </summary>
        /// <returns></returns>
        private bool find_start_tag()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_start_tag()");

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 開始タグ(Object)の場合
                if (_column == "{")
                {
                    // Object開始
                    if (!StartObject())
                    {
                        // ロギング
                        Logger.Error("Object開始:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_start_tag()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartObject);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_start_tag()");

                    // 正常終了
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // 配列開始
                    if (!StartArray())
                    {
                        // ロギング
                        Logger.Error("配列開始:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_start_tag()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartArray);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_start_tag()");

                    // 正常終了
                    return true;
                }
            }

            // ロギング
            Logger.Error("開始タグなし");
            Logger.Debug("<<<<= JsonConfig::find_start_tag()");

            // 開始タグなし
            return false;
        }

        /// <summary>
        /// 次状態検索
        /// </summary>
        /// <returns></returns>
        private bool find_next_state()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_next_state()");
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

            // ロギング
            Logger.Error("<<<<= JsonConfig::find_next_state()");
            return false;
        }

        /// <summary>
        /// String検索
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool find_string(ref string key)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_string(ref string)");

            StringBuilder _result = new StringBuilder();
            bool _escape = false;

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // "の場合
                if (_column == "\"")
                {
                    // 直前にエスケープされているか？
                    if (!_escape)
                    {
                        // エスケープされていない場合、文字列終了
                        key = _result.ToString();

                        // ロギング
                        Logger.DebugFormat("key:{0}", key);
                        Logger.Debug("<<<<= JsonConfig::find_string(ref string)");

                        // 正常終了
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

            // ロギング
            Logger.Error("該当なし");
            Logger.Debug("<<<<= JsonConfig::find_string(ref string)");

            // 該当なし
            return false;
        }

        /// <summary>
        /// Key検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_key()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_object_key()");

            StringBuilder _key_stream = new StringBuilder();    // Key組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndKey;         // 次遷移状態

            // 開始位置設定
            uint _start_colum = m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 終了タグ(Object)の場合
                if (_column == "}")
                {
                    // Arrayインデックス終端
                    if (!terminate_end_object_array_index())
                    {
                        // ロギング
                        Logger.Error("Arrayインデックス終端:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_key()");

                        // 異常終了
                        return false;
                    }

                    // Object終了
                    if (!EndObject())
                    {
                        // ロギング
                        Logger.Error("Object終了:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_key()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(get_end_object_next_status());

                    // 正常終了
                    Logger.Debug("<<<<= JsonConfig::find_object_key()");
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
                // エラー処理
                Error(JsonError.InvalidSyntax);

                // ロギング
                Logger.Error("構文エラー");
                Logger.Debug("<<<<= JsonConfig::find_object_key()");

                // 異常終了
                return false;
            }

            // String形式を判定
            string _key = string.Empty;
            if (is_string(_key_stream.ToString(), ref _key))
            {
                // Element開始
                if (!StartElement(_start_colum + 1, _key))
                {
                    // ロギング
                    Logger.Error("Element開始:[異常]");
                    Logger.Debug("<<<<= JsonConfig::find_object_key()");

                    // 異常終了
                    return false;
                }

                // Key処理
                if (!Key(_start_colum + 1, _key))
                {
                    // ロギング
                    Logger.Error("Key処理:[異常]");
                    Logger.Debug("<<<<= JsonConfig::find_object_key()");

                    // 異常終了
                    return false;
                }

                // 状態更新
                m_status_stack.Push(_JsonStatus);

                // ロギング
                Logger.Debug("<<<<= JsonConfig::find_object_key()");

                // 正常終了
                return true;
            }

            // エラー処理
            Error(JsonError.NotFoundKey);

            // ロギング
            Logger.Error("Keyなし");
            Logger.Debug("<<<<= JsonConfig::find_object_key()");

            // 異常終了
            return false;
        }

        /// <summary>
        /// 次Key検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_next_key()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_object_next_key()");

            StringBuilder _key_stream = new StringBuilder();    // Key組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndKey;         // 次遷移状態

            // 開始位置設定
            uint _start_colum = m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

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
                // エラー処理
                Error(JsonError.InvalidSyntax);

                // ロギング
                Logger.Error("Key処理:[異常]");
                Logger.Debug("<<<<= JsonConfig::find_object_next_key()");

                // 異常終了
                return false;
            }

            // String形式を判定
            string _key = string.Empty;
            if (is_string(_key_stream.ToString(), ref _key))
            {
                // Element開始
                if (!StartElement(_start_colum + 1, _key))
                {
                    // ロギング
                    Logger.Error("Element開始:[異常]");
                    Logger.Debug("<<<<= JsonConfig::find_object_next_key()");

                    // 異常終了
                    return false;
                }

                // Key処理
                if (!Key(_start_colum + 1, _key))
                {
                    // ロギング
                    Logger.Error("Key処理:[異常]");
                    Logger.Debug("<<<<= JsonConfig::find_object_next_key()");

                    // 異常終了
                    return false;
                }

                // 状態更新
                m_status_stack.Push(_JsonStatus);

                // 正常終了
                Logger.Debug("<<<<= JsonConfig::find_object_next_key()");
                return true;
            }

            // エラー処理
            Error(JsonError.NotFoundKey);

            // ロギング
            Logger.Error("Keyなし");
            Logger.Debug("<<<<= JsonConfig::find_object_next_key()");

            // 異常終了
            return false;
        }

        /// <summary>
        /// Value開始検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_start_value()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_object_start_value()");

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // :の場合
                if (_column == ":")
                {
                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartValue);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_object_start_value()");

                    // Object値開始
                    return true;
                }
                break;
            }

            // エラー処理
            Error(JsonError.InvalidSyntax);

            // ロギング
            Logger.Error("次Value開始タグなし");
            Logger.Debug("<<<<= JsonConfig::find_object_start_value()");

            // 異常終了
            return false;
        }

        /// <summary>
        /// Value検索
        /// </summary>
        /// <returns></returns>
        private bool find_object_value()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_object_value()");

            StringBuilder _value_stream = new StringBuilder();  // Value組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndValue;       // 次遷移状態

            // 開始位置を保存
            uint _start_colum = m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 開始タグ(Object)の場合
                if (_column == "{")
                {
                    // Object開始
                    if (!StartObject())
                    {
                        // ロギング
                        Logger.Error("Object開始:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_value()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartObject);

                    // 正常終了
                    Logger.Debug("<<<<= JsonConfig::find_object_value()");
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
                    if (!StartArray())
                    {
                        // ロギング
                        Logger.Error("Array開始:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_value()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartArray);

                    // 正常終了
                    Logger.Debug("<<<<= JsonConfig::find_object_value()");
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // Array開始
                    if (!StartArray())
                    {
                        // ロギング
                        Logger.Error("Array開始:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_value()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartArray);

                    // 正常終了
                    Logger.Debug("<<<<= JsonConfig::find_object_value()");
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
                if (is_string(_value_stream.ToString(), ref _string_value))
                {
                    // 開始カラム更新
                    _start_colum = _start_colum + 1;

                    // String処理
                    if (!String(_start_colum, _string_value))
                    {
                        // ロギング
                        Logger.Error("String処理:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_value()");

                        // 異常終了
                        return false;
                    }
                }
                // true、またはfalse、null、数値の可能性がある場合
                else
                {
                    // Value処理
                    if (!Value(_start_colum, _value_stream.ToString().Trim()))
                    {
                        // ロギング
                        Logger.Error("Value処理:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_value()");

                        // 異常終了
                        return false;
                    }
                }
            }

            // Key情報取得
            JsonKeyInfo _key_info = m_key_stack.Current();

            // 次遷移状態で分岐
            switch (_JsonStatus)
            {
                case JsonStatus.NextObject:
                case JsonStatus.EndValue:
                    {
                        // Element終了
                        if (!EndElement(_key_info.StartColum, _key_info.Value))
                        {
                            // ロギング
                            Logger.Error("Element終了:[異常]");
                            Logger.Debug("<<<<= JsonConfig::find_object_value()");

                            // 異常終了
                            return false;
                        }

                        // 状態更新
                        m_status_stack.Push(_JsonStatus);

                        // ロギング
                        Logger.Debug("<<<<= JsonConfig::find_object_value()");

                        // 正常終了
                        return true;
                    }
                case JsonStatus.EndObject:
                    {
                        // Element終了
                        if (!EndElement(_key_info.StartColum, _key_info.Value))
                        {
                            // ロギング
                            Logger.Error("Element終了:[異常]");
                            Logger.Debug("<<<<= JsonConfig::find_object_value()");

                            // 異常終了
                            return false;
                        }

                        // Arrayインデックス終端
                        if (!terminate_end_object_array_index())
                        {
                            // ロギング
                            Logger.Error("Arrayインデックス終端:[異常]");
                            Logger.Debug("<<<<= JsonConfig::find_object_value()");

                            // 異常終了
                            return false;
                        }

                        // Object終了
                        if (!EndObject())
                        {
                            // ロギング
                            Logger.Error("Object終了:[異常]");
                            Logger.Debug("<<<<= JsonConfig::find_object_value()");

                            // 異常終了
                            return false;
                        }

                        // 状態更新
                        m_status_stack.Push(get_end_object_next_status());

                        // ロギング
                        Logger.Debug("<<<<= JsonConfig::find_object_value()");

                        // 正常終了
                        return true;
                    }
                default:
                    // エラー処理 
                    Error(JsonError.SystemError);

                    // ロギング
                    Logger.Error("次遷移状態:[異常]");
                    Logger.Debug("<<<<= JsonConfig::find_object_value()");

                    // 異常終了
                    return false;
            }
        }

        /// <summary>
        /// 終了タグ検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_end_tag()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_object_end_tag()");

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 終了タグ(Object)の場合
                if (_column == "}")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = m_key_stack.Current();

                    // Element終了
                    if (!EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // ロギング
                        Logger.Error("Element終了:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_end_tag()");

                        // 異常終了
                        return false;
                    }

                    // Arrayインデックス終端
                    if (!terminate_end_object_array_index())
                    {
                        // ロギング
                        Logger.Error("Arrayインデックス終端:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_end_tag()");

                        // 異常終了
                        return false;
                    }

                    // Object終了
                    if (!EndObject())
                    {
                        // ロギング
                        Logger.Error("Object終了:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_end_tag()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(get_end_object_next_status());

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_object_end_tag()");

                    // 正常終了
                    return true;
                }
                // ","の場合
                else if (_column == ",")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = m_key_stack.Current();

                    // Element終了
                    if (!EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // ロギング
                        Logger.Error("Element終了:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_end_tag()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.NextObject);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_object_end_tag()");

                    // 正常終了
                    return true;
                }
                break;
            }

            // エラー処理
            Error(JsonError.NotFindEndTag);

            // ロギング
            Logger.Error("終了タグなし");
            Logger.Debug("<<<<= JsonConfig::find_object_end_tag()");

            // 異常終了
            return false;
        }

        /// <summary>
        /// 次開始検索(Object)
        /// </summary>
        /// <returns></returns>
        private bool find_object_next_start()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_object_next_start()");

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 次Objectあり','の場合
                if (_column == ",")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = m_key_stack.Current();

                    // Element終了
                    if (!EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // ロギング
                        Logger.Error("Element終了:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_object_next_start()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.NextWait);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_object_next_start()");

                    // 正常終了
                    return true;
                }
                break;
            }

            // ロギング
            Logger.Error("次Objectなし");
            Logger.Debug("<<<<= JsonConfig::find_object_next_start()");

            // 異常終了
            return false;
        }

        /// <summary>
        /// Arrayインデックス終端(Object終了時)
        /// </summary>
        /// <returns></returns>
        private bool terminate_end_object_array_index()
        {
            // ロギング
            Logger.Debug("<<<<= JsonConfig::terminate_end_object_array_index()");

            // 動作モード(親)取得
            JsonMode _parent_mode = m_mode_stack.Parent();

            // 動作モード(親)判定
            if (_parent_mode != JsonMode.Array)
            {
                // ロギング
                Logger.Debug("<<<<= JsonConfig::terminate_end_object_array_index()");

                // Arrayではないので正常終了
                return true;
            }

            // キー情報数を判定
            if (m_key_stack.Count() <= 1)
            {
                // ロギング
                Logger.Debug("<<<<= JsonConfig::terminate_end_object_array_index()");

                // 1つ前がないので正常終了
                return true;
            }

            // Key情報取得
            JsonKeyInfo _key_info = m_key_stack.Get();

            // Element終了
            if (!EndElement(_key_info.StartColum, _key_info.Value))
            {
                // ロギング
                Logger.Error("Element終了:[異常]");
                Logger.Debug("<<<<= JsonConfig::terminate_end_object_array_index()");

                // 異常終了
                return false;
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::terminate_end_object_array_index()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// 次遷移状態取得(Object終了時)
        /// </summary>
        /// <returns></returns>
        private JsonStatus get_end_object_next_status()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::get_end_object_next_status()");

            // 動作モードで分岐
            JsonStatus _status = JsonStatus.NotStart;
            switch (m_mode_stack.Get())
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


            // ロギング
            Logger.DebugFormat("《次遷移状態⇒[{0}]》", m_status_stack.ToString(_status));
            Logger.Debug("<<<<= JsonConfig::get_end_object_next_status()");

            // 次遷移状態を返却
            return _status;
        }

        /// <summary>
        /// Value検索(Array)
        /// </summary>
        /// <returns></returns>
        private bool find_array_value()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_array_value()");

            StringBuilder _value_stream = new StringBuilder();  // Value組立用Stream
            JsonStatus _JsonStatus = JsonStatus.EndArrayValue;  // 次遷移状態

            // 開始位置を保存
            uint _start_colum = m_strings_queue.CurrentColum();

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 開始タグ(Object)の場合
                if (_column == "{")
                {
                    // Arrayインデックス更新
                    if (!update_array_index(_start_colum))
                    {
                        // ロギング
                        Logger.Error("Arrayインデックス更新:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_array_value()");

                        // 異常終了
                        return false;
                    }

                    // Object開始
                    if (!StartObject())
                    {
                        // ロギング
                        Logger.Error("Object開始:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_array_value()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartObject);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_array_value()");

                    // 正常終了
                    return true;
                }
                // 開始タグ(Array)の場合
                else if (_column == "[")
                {
                    // Array開始
                    if (!StartArray())
                    {
                        // ロギング
                        Logger.Error("Array開始:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_array_value()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.StartArray);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_array_value()");

                    // 正常終了
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
            if (!update_array_index(_start_colum))
            {
                // ロギング
                Logger.Error("Arrayインデックス更新:[異常]");
                Logger.Debug("<<<<= JsonConfig::find_array_value()");

                // 異常終了
                return false;
            }

            // 文字列長を判定
            if (_value_stream.ToString().Length > 0)
            {
                // String形式を判定
                string _string_value = string.Empty;
                if (is_string(_value_stream.ToString(), ref _string_value))
                {
                    // 開始カラム更新
                    _start_colum = _start_colum + 1;

                    // String処理
                    if (!String(_start_colum, _string_value))
                    {
                        // ロギング
                        Logger.Error("String処理:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_array_value()");

                        // 異常終了
                        return false;
                    }
                }
                // true、またはfalse、null、数値の可能性がある場合
                else
                {
                    // Value処理
                    if (!Value(_start_colum, _value_stream.ToString().Trim()))
                    {
                        // ロギング
                        Logger.Error("Value処理:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_array_value()");

                        // 異常終了
                        return false;
                    }
                }
            }

            // Key情報取得
            JsonKeyInfo _key_info = m_key_stack.Current();

            // 次遷移状態で分岐
            switch (_JsonStatus)
            {
                case JsonStatus.NextArray:
                case JsonStatus.EndArrayValue:
                    {
                        // Element終了
                        if (!EndElement(_key_info.StartColum, _key_info.Value))
                        {
                            // ロギング
                            Logger.Error("Element終了:[異常]");
                            Logger.Debug("<<<<= JsonConfig::find_array_value()");

                            // 異常終了
                            return false;
                        }

                        // 状態更新
                        m_status_stack.Push(_JsonStatus);

                        // ロギング
                        Logger.Debug("<<<<= JsonConfig::find_array_value()");

                        // 正常終了
                        return true;
                    }
                case JsonStatus.EndArray:
                    {
                        // Array終了
                        if (!EndArray())
                        {
                            // ロギング
                            Logger.Error("Array終了:[異常]");
                            Logger.Debug("<<<<= JsonConfig::find_array_value()");

                            // 異常終了
                            return false;
                        }

                        // 状態更新
                        m_status_stack.Push(get_end_object_next_status());

                        // ロギング
                        Logger.Debug("<<<<= JsonConfig::find_array_value()");

                        // 正常終了
                        return true;
                    }
                default:
                    // エラー処理
                    Error(JsonError.SystemError);

                    // ロギング
                    Logger.Error("次遷移状態:[異常]");
                    Logger.Debug("<<<<= JsonConfig::find_array_value()");

                    // 異常終了
                    return false;
            }
        }

        /// <summary>
        /// 終了タグ検索
        /// </summary>
        /// <returns></returns>
        private bool find_array_end_tag()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_array_end_tag()");

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 終了タグ(Array)の場合
                if (_column == "]")
                {
                    // Array終了
                    if (!EndArray())
                    {
                        // ロギング
                        Logger.Error("Array終了:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_array_end_tag()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(get_end_object_next_status());

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_array_end_tag()");

                    // 正常終了
                    return true;
                }
                // 次Value存在有の場合
                else if (_column == ",")
                {
                    // 状態更新
                    m_status_stack.Push(JsonStatus.NextArray);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_array_end_tag()");

                    // 正常終了
                    return true;
                }
                break;
            }

            // エラー処理
            Error(JsonError.NotFindEndTag);

            // ロギング
            Logger.Error("終了タグなし");
            Logger.Debug("<<<<= JsonConfig::find_array_end_tag()");

            // 異常終了
            return false;
        }

        /// <summary>
        /// 次開始検索(Array)
        /// </summary>
        /// <returns></returns>
        private bool find_array_next_start()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::find_array_next_start()");

            // 1文字ずつ処理する
            while (!m_strings_queue.Eof())
            {
                // 1文字取得
                string _column = m_strings_queue.Get();

                // 次Arrayあり','の場合
                if (_column == ",")
                {
                    // Key情報取得
                    JsonKeyInfo _key_info = m_key_stack.Current();

                    // Element終了
                    if (!EndElement(_key_info.StartColum, _key_info.Value))
                    {
                        // ロギング
                        Logger.Error("Element終了:[異常]");
                        Logger.Debug("<<<<= JsonConfig::find_array_next_start()");

                        // 異常終了
                        return false;
                    }

                    // 状態更新
                    m_status_stack.Push(JsonStatus.NextWait);

                    // ロギング
                    Logger.Debug("<<<<= JsonConfig::find_array_next_start()");

                    // 正常終了
                    return true;
                }
                break;
            }

            // ロギング
            Logger.Error("次Arrayなし");
            Logger.Debug("<<<<= JsonConfig::find_array_next_start()");

            // 異常終了
            return false;
        }

        /// <summary>
        /// Object開始
        /// </summary>
        /// <returns></returns>
        private bool StartObject()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::StartObject()");

            // モードスタック設定
            m_mode_stack.Push(JsonMode.Object);

            // コールバック関数呼び出し
            if (OnStartObject != null)
            {
                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    JsonMode.Object,
                    total_line_no(),
                    m_strings_queue.CurrentColum() - 1,
                    new JsonArrayInfo(),
                    new JsonKeyInfo(),
                    JsonValueType.ObjectStart,
                    "{",
                    new JsonErrorInfo()
                );

                // Object開始検出通知
                if (!OnStartObject(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Object開始検出通知:[異常]");
                    Logger.Debug("<<<<= JsonConfig::StartObject()");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::StartObject()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// Object終了
        /// </summary>
        /// <returns></returns>
        private bool EndObject()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::EndObject()");

            // モードスタックサイズ判定
            if (m_mode_stack.Count() <= 0)
            {
                // エラー処理
                Error(JsonError.SystemError);

                // ロギング
                Logger.Error("モードスタックサイズ判定:[異常]");
                Logger.Debug("<<<<= JsonConfig::EndObject()");

                // 異常終了
                return false;
            }

            // 動作モード取得
            JsonMode _mode = m_mode_stack.Get();

            // モードスタック解放
            m_mode_stack.Pop();

            // モード判定
            if (_mode != JsonMode.Object)
            {
                // エラー処理
                Error(JsonError.InvalidSyntax);

                // ロギング
                Logger.Error("モード判定:[異常]");
                Logger.Debug("<<<<= JsonConfig::EndObject()");

                // 異常終了
                return false;
            }

            // コールバック関数呼び出し
            if (OnEndObject != null)
            {
                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    JsonMode.Object,
                    total_line_no(),
                    m_strings_queue.CurrentColum() - 1,
                    new JsonArrayInfo(),
                    new JsonKeyInfo(),
                    JsonValueType.ObjectEnd,
                    "}",
                    new JsonErrorInfo()
                );

                // Object終了検出通知
                if (!OnEndObject(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Object終了検出通知:[異常]");
                    Logger.Debug("<<<<= JsonConfig::EndObject()");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::EndObject()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// Array開始
        /// </summary>
        /// <returns></returns>
        private bool StartArray()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::StartArray()");

            // モードスタック設定
            m_mode_stack.Push(JsonMode.Array);

            // Arrayインデックス初期設定
            initialize_array_index();

            // コールバック関数呼び出し
            if (OnStartArray != null)
            {
                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    JsonMode.Array,
                    total_line_no(),
                    m_strings_queue.CurrentColum() - 1,
                    _Array,
                    new JsonKeyInfo(),
                    JsonValueType.ArrayStart,
                    "[",
                    new JsonErrorInfo()
                );

                // Array開始検出通知
                if (!OnStartArray(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Object終了検出通知:[異常]");
                    Logger.Debug("<<<<= JsonConfig::StartArray()");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::StartArray()");

            // 正常終了
            return true;
        }

        /// <summary>
        /// Array終了
        /// </summary>
        /// <returns></returns>
        private bool EndArray()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::EndArray()");

            // モードスタックサイズ判定
            if (m_mode_stack.Count() <= 0)
            {
                // エラー処理
                Error(JsonError.SystemError);

                // ロギング
                Logger.Error("モードスタックサイズ判定:[異常]");
                Logger.Debug("<<<<= JsonConfig::EndArray()");

                // 異常終了
                return false;
            }

            // 動作モード取得
            JsonMode _mode = m_mode_stack.Get();

            // モードスタック解放
            m_mode_stack.Pop();

            // モード判定
            if (_mode != JsonMode.Array)
            {
                // エラー処理
                Error(JsonError.InvalidSyntax);

                // ロギング
                Logger.Error("モード判定:[異常]");
                Logger.Debug("<<<<= JsonConfig::EndArray()");

                // 異常終了
                return false;
            }

            // Arrayインデックススタック解放
            m_array_index_stack.Pop();

            // コールバック関数呼び出し
            if (OnEndArray != null)
            {
                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    JsonMode.Array,
                    total_line_no(),
                    m_strings_queue.CurrentColum() - 1,
                    _Array,
                    new JsonKeyInfo(),
                    JsonValueType.ArrayEnd,
                    "]",
                    new JsonErrorInfo()
                );

                // Array終了検出通知
                if (!OnEndArray(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Array終了検出通知:[異常]");
                    Logger.Debug("<<<<= JsonConfig::EndArray()");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::EndArray()");

            // 正常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::StartElement(uint, string)");
            Logger.DebugFormat("startColum:[{0}]", startColum);
            Logger.DebugFormat("key       :[{0}]", key);

            // Key情報スタック設定
            JsonKeyInfo _keyInfo = new JsonKeyInfo();
            _keyInfo.StartColum = startColum;
            _keyInfo.Value = key;

            // Key情報登録
            m_key_stack.Push(_keyInfo);

            // コールバック関数呼び出し
            if (OnStartElement != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    startColum,
                    _Array,
                    _keyInfo,
                    JsonValueType.Element,
                    key,
                    new JsonErrorInfo()
                );

                // Element開始通知
                if (!OnStartElement(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Array終了検出通知:[異常]");
                    Logger.Debug("<<<<= JsonConfig::StartElement(uint, string)");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::StartElement(uint, string)");

            // 正常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::EndElement(uint, string)");
            Logger.DebugFormat("startColum:[{0}]", startColum);
            Logger.DebugFormat("key       :[{0}]", key);

            // キー数を判定
            if (m_key_stack.Count() <= 0)
            {
                // ロギング
                Logger.Debug("<<<<= JsonConfig::EndElement(uint, string)");

                // 正常終了
                return true;
            }

            // コールバック関数呼び出し
            if (OnEndElement != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = m_key_stack.Get();

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Element,
                    key,
                    new JsonErrorInfo()
                );

                // Element終了通知
                if (!OnEndElement(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Element終了通知:[異常]");
                    Logger.Debug("<<<<= JsonConfig::EndElement(uint, string)");

                    // 異常終了
                    return false;
                }
            }

            // キースタック解放
            m_key_stack.Pop();

            // ロギング
            Logger.Debug("<<<<= JsonConfig::EndElement(uint, string)");

            // 正常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Key(uint, string)");
            Logger.DebugFormat("startColum:[{0}]", startColum);
            Logger.DebugFormat("key       :[{0}]", key);

            // TODO:未実装

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Key(uint, string)");

            // 正常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Value(uint, string)");
            Logger.DebugFormat("startColum:[{0}]", startColum);
            Logger.DebugFormat("value     :[{0}]", value);

            // コールバック関数呼び出し
            if (OnValue != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (m_key_stack.Count() > 0)
                {
                    _current_key = m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Value,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(Value)
                if (!OnValue(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Value検出通知(Value):[異常]");
                    Logger.Debug("<<<<= JsonConfig::Value(uint, string)");

                    // 異常終了
                    return false;
                }
            }

            // NULL判定
            if (value == "null")
            {
                // Null処理
                if (!Null(startColum))
                {
                    // ロギング
                    Logger.Error("Null処理:[異常]");
                    Logger.Debug("<<<<= JsonConfig::Value(uint, string)");

                    // 異常終了
                    return false;
                }

                // ロギング
                Logger.Debug("<<<<= JsonConfig::Value(uint, string)");

                // 正常終了
                return true;
            }
            // 値判定(bool)
            else if (value == "true" || value == "false")
            {
                // bool処理
                if (!Bool(startColum, value))
                {
                    // ロギング
                    Logger.Error("bool処理:[異常]");
                    Logger.Debug("<<<<= JsonConfig::Value(uint, string)");

                    // 異常終了
                    return false;
                }

                // 正常終了
                Logger.Debug("<<<<= JsonConfig::Value(uint, string)");
                return true;
            }
            // 値判定(Number)
            else if (is_number(value))
            {
                // Number処理
                if (!Number(startColum, value))
                {
                    // ロギング
                    Logger.Error("Number処理:[異常]");
                    Logger.Debug("<<<<= JsonConfig::Value(uint, string)");

                    // 異常終了
                    return false;
                }

                // ロギング
                Logger.Debug("<<<<= JsonConfig::Value(uint, string)");

                // 正常終了
                return true;
            }

            // ロギング
            Logger.Error("Value値:[異常]");
            Logger.Debug("<<<<= JsonConfig::Value(uint, string)");

            // 異常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Number(uint, string)");
            Logger.DebugFormat("startColum:[{0}]", startColum);
            Logger.DebugFormat("value     :[{0}]", value);

            // コールバック関数呼び出し
            if (OnNumber != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (m_key_stack.Count() > 0)
                {
                    _current_key = m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Number,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(Number)
                if (!OnNumber(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Value検出通知(Number):[異常]");
                    Logger.Debug("<<<<= JsonConfig::Number(uint, string)");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Number(uint, string)");

            // 正常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::String(uint, string)");
            Logger.DebugFormat("startColum:[{0}]", startColum);
            Logger.DebugFormat("value     :[{0}]", value);

            // コールバック関数呼び出し
            if (OnString != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (m_key_stack.Count() > 0)
                {
                    _current_key = m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.String,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(String)
                if (!OnString(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Value検出通知(String):[異常]");
                    Logger.Debug("<<<<= JsonConfig::String(uint, string)");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::String(uint, string)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// Null処理
        /// </summary>
        /// <param name="startColum"></param>
        /// <returns></returns>
        bool Null(uint startColum)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Null(uint)");
            Logger.DebugFormat("startColum:[{0}]", startColum);

            // コールバック関数呼び出し
            if (OnNull != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (m_key_stack.Count() > 0)
                {
                    _current_key = m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Null,
                    "null",
                    new JsonErrorInfo()
                );

                // Value検出通知(Null)
                if (!OnNull(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Value検出通知(Null):[異常]");
                    Logger.Debug("<<<<= JsonConfig::Null(uint)");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Null(uint)");

            // 正常終了
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
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Bool(uint, string)");
            Logger.DebugFormat("startColum:[{0}]", startColum);
            Logger.DebugFormat("value     :[{0}]", value);

            // コールバック関数呼び出し
            if (OnBool != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (m_key_stack.Count() > 0)
                {
                    _current_key = m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    startColum,
                    _Array,
                    _current_key,
                    JsonValueType.Bool,
                    value,
                    new JsonErrorInfo()
                );

                // Value検出通知(Bool)
                if (!OnBool(_Args, m_user_data))
                {
                    // ロギング
                    Logger.Error("Value検出通知(Bool):[異常]");
                    Logger.Debug("<<<<= JsonConfig::Bool(uint, string)");

                    // 異常終了
                    return false;
                }
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Bool(uint, string)");

            // 正常終了
            return true;
        }

        /// <summary>
        /// エラー処理
        /// </summary>
        /// <param name="error"></param>
        private void Error(JsonError error)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfig::Error(JsonError)");
            Logger.DebugFormat("error:[{0}]", error.ToString());

            // コールバック関数呼び出し
            if (OnError != null)
            {
                // 動作モード取得
                JsonMode _mode = m_mode_stack.Get();

                // 配列情報取得
                JsonArrayInfo _Array = new JsonArrayInfo();
                if (m_array_index_stack.Count() > 0)
                {
                    _Array = m_array_index_stack.Get();
                }

                // 現在キーを取得
                JsonKeyInfo _current_key = new JsonKeyInfo();
                if (m_key_stack.Count() > 0)
                {
                    _current_key = m_key_stack.Get();
                }

                // イベント情報設定
                JsonEventArg _Args = new JsonEventArg
                (
                    m_raw_string,
                    _mode,
                    total_line_no(),
                    m_strings_queue.CurrentColum(),
                    _Array,
                    _current_key,
                    JsonValueType.Unknown,
                    "",
                    new JsonErrorInfo(error)
                );

                // エラーイベント通知
                OnError(_Args, m_user_data);
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfig::Error(JsonError)");
        }
    }
}