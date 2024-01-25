using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationMarksクラス
    /// </summary>
    [Serializable]
    public class StationMarks : List<string>, ISerializable
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StationMarks()
        {
            // ロギング
            Logger.Debug("=>>>> StationMarks::StationMarks()");

            // ロギング
            Logger.Debug("<<<<= StationMarks::StationMarks()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public StationMarks(StationMarks properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationMarks::StationMarks(StationMarks)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= StationMarks::StationMarks(StationMarks)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected StationMarks(SerializationInfo info, StreamingContext context)
        {
            // ロギング
            Logger.Debug("=>>>> StationMarks::StationMarks(SerializationInfo info, StreamingContext context)");
            Logger.DebugFormat("info   :[{0}]", info);
            Logger.DebugFormat("context:[{0}]", context);

            // デシリアライズ時の処理
            for (int i = 0; i < info.MemberCount; i++)
            {
                Add(info.GetValue(string.Format("StationMarks{0}", i), typeof(string)).ToString());
            }

            // ロギング
            Logger.Debug("<<<<= StationMarks::StationMarks(SerializationInfo info, StreamingContext context)");
        }
        #endregion

        #region シリアライズデータ取得
        /// <summary>
        /// シリアライズデータ取得
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // ロギング
            Logger.Debug("=>>>> StationMarks::GetObjectData(SerializationInfo info, StreamingContext context)");
            Logger.DebugFormat("info   :[{0}]", info);
            Logger.DebugFormat("context:[{0}]", context);

            // シリアライズするデータを追加
            int i = 0;
            foreach (var item in this)
            {
                info.AddValue(string.Format("StationMarks{0}", i++), item.ToString());
            }

            // ロギング
            Logger.Debug("<<<<= StationMarks::GetObjectData(SerializationInfo info, StreamingContext context)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(StationMarks properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationMarks::Copy(StationMarks)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(property);
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationMarks::Copy(StationMarks)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(StationMarks properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationMarks::Compare(StationMarks)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= StationMarks::Compare(StationMarks)");

                // 不一致
                return false;
            }

            // 要素を繰り返す
            int i = 0;
            foreach (var property in properties)
            {
                // 各要素比較
                if (this[i] != property)
                {
                    // ロギング
                    Logger.DebugFormat("Property:[不一致][{0}][{1}]", this[i].ToString(), property.ToString());
                    Logger.Debug("<<<<= StationMarks::Compare(StationMarks)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= StationMarks::Compare(StationMarks)");

            // 一致
            return true;
        }
        #endregion

        #region 文字列化
        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 文字列化返却
            return ToString(0);
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToString(int indent)
        {
            // 結果オブジェクト生成
            StringBuilder result = new StringBuilder();

            // インデント生成
            string indentstr = new string('　', indent);

            // リストを繰り返す
            int i = 0;
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(indentstr + string.Format("《配列番号:[{0}]》", i++));
                result.Append(indentstr + property);
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
