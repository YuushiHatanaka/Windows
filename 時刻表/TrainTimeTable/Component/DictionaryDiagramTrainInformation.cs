using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Component
{
    /// <summary>
    /// DictionaryDiagramTrainInformationクラス
    /// </summary>
    [Serializable]
    public class DictionaryDiagramTrainInformation : Dictionary<DirectionType, DiagramTrainInformation>, ISerializable
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
        public DictionaryDiagramTrainInformation()
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryDiagramTrainInformation::DictionaryDiagramTrainInformation()");

            // ロギング
            Logger.Debug("<<<<= DictionaryDiagramTrainInformation::DictionaryDiagramTrainInformation()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DictionaryDiagramTrainInformation(DictionaryDiagramTrainInformation properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryDiagramTrainInformation::DictionaryDiagramTrainInformation(DictionaryDiagramTrainInformation)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DictionaryDiagramTrainInformation::DictionaryDiagramTrainInformation(DictionaryDiagramTrainInformation)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DictionaryDiagramTrainInformation(SerializationInfo info, StreamingContext context)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryDiagramTrainInformation::DictionaryDiagramTrainInformation(SerializationInfo info, StreamingContext context)");
            Logger.DebugFormat("info   :[{0}]", info);
            Logger.DebugFormat("context:[{0}]", context);

            // デシリアライズ時の処理
            SerializationInfoEnumerator serializationInfoEnumerator = info.GetEnumerator();
            while (serializationInfoEnumerator.MoveNext())
            {
                // 変換
                DirectionType key = (DirectionType)Enum.Parse(typeof(DirectionType), serializationInfoEnumerator.Name);
                DiagramTrainInformation value = (DiagramTrainInformation)serializationInfoEnumerator.Value;

                // 登録
                Add(key, value);
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryDiagramTrainInformation::DictionaryDiagramTrainInformation(SerializationInfo info, StreamingContext context)");
        }
        #endregion

        #region シリアライズデータ取得
        /// <summary>
        /// シリアライズデータ取得
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // ロギング
            Logger.Info("=>>>> DictionaryDiagramTrainInformation::GetObjectData(SerializationInfo info, StreamingContext context)");
            Logger.InfoFormat("info   :[{0}]", info);
            Logger.InfoFormat("context:[{0}]", context);

            // シリアライズするデータを追加
            foreach (var key in this.Keys)
            {
                info.AddValue(key.ToString(), this[key]);
            }

            // ロギング
            Logger.Info("<<<<= DictionaryDiagramTrainInformation::GetObjectData(SerializationInfo info, StreamingContext context)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DictionaryDiagramTrainInformation properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryDiagramTrainInformation::Copy(DictionaryDiagramTrainInformation)");
            Logger.DebugFormat("property:[{0}]", properties);

            // クリア
            Clear();

            // 要素を繰り返す
            foreach (var key in properties.Keys)
            {
                // 登録
                Add(key, properties[key]);
            }

            // ロギング
            Logger.Debug("<<<<= DictionaryDiagramTrainInformation::Copy(DictionaryDiagramTrainInformation)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(DictionaryDiagramTrainInformation properties)
        {
            // ロギング
            Logger.Debug("=>>>> DictionaryDiagramTrainInformation::Compare(DictionaryDiagramTrainInformation)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // キー登録判定
                if (!ContainsKey(property.Key))
                {
                    // ロギング
                    Logger.DebugFormat("key:[{0}][キー登録なし]", property.Key);
                    Logger.Debug("<<<<= DictionaryDiagramTrainInformation::Compare(DictionaryDiagramTrainInformation)");

                    // 不一致
                    return false;
                }

                // 内容判定
                if (this[property.Key] != property.Value)
                {
                    // ロギング
                    Logger.DebugFormat("Property:[{0}][{1}][不一致]", this[property.Key].ToString(), property.Value.ToString());
                    Logger.Debug("<<<<= DictionaryDiagramTrainInformation::Compare(DictionaryDiagramTrainInformation)");

                    // 不一致
                    return false;
                }
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DictionaryDiagramTrainInformation::Compare(DictionaryDiagramTrainInformation)");

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
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(indentstr + string.Format("キー:[{0}],値:[{1}]", property.Key.GetStringValue(), property.Value.GetStringValue()));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
