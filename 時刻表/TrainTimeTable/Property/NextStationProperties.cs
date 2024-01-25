﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// NextStationsクラス
    /// </summary>
    [Serializable]
    public class NextStationProperties : List<NextStationProperty>, ISerializable
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
        public NextStationProperties()
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperties::NextStationProperties()");

            // ロギング
            Logger.Debug("<<<<= NextStationProperties::NextStationProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public NextStationProperties(NextStationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperties::NextStationProperties(NextStationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= NextStationProperties::NextStationProperties(NextStationProperties)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NextStationProperties(SerializationInfo info, StreamingContext context)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperties::NextStationProperties(SerializationInfo info, StreamingContext context)");
            Logger.DebugFormat("info   :[{0}]", info);
            Logger.DebugFormat("context:[{0}]", context);

            // デシリアライズ時の処理
            for (int i = 0; i < info.MemberCount; i++)
            {
                Add(info.GetValue(string.Format("NextStationProperty{0}", i), typeof(NextStationProperty)) as NextStationProperty);
            }

            // ロギング
            Logger.Debug("<<<<= NextStationProperties::NextStationProperties(SerializationInfo info, StreamingContext context)");
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
            Logger.Info("=>>>> NextStationProperties::GetObjectData(SerializationInfo info, StreamingContext context)");
            Logger.InfoFormat("info   :[{0}]", info);
            Logger.InfoFormat("context:[{0}]", context);

            // シリアライズ時の処理
            int i = 0;
            foreach (var item in this)
            {
                info.AddValue(string.Format("NextStationProperty{0}", i++), item, typeof(NextStationProperty));
            }

            // ロギング
            Logger.Info("<<<<= NextStationProperties::GetObjectData(SerializationInfo info, StreamingContext context)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(NextStationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperties::Copy(NextStationProperties)");
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
            Logger.Debug("<<<<= NextStationProperties::Copy(NextStationProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(NextStationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> NextStationProperties::Compare(NextStationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= NextStationProperties::Compare(NextStationProperties)");

                // 不一致
                return false;
            }

            // 要素を繰り返す
            int i = 0;
            foreach (var property in properties)
            {
                // 各要素比較
                if (!this[i].Compare(property))
                {
                    // ロギング
                    Logger.DebugFormat("Property:[不一致][{0}][{1}]", this[i].ToString(), property.ToString());
                    Logger.Debug("<<<<= NextStationProperties::Compare(NextStationProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= NextStationProperties::Compare(NextStationProperties)");

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
                result.Append(property.ToString(indent + 1));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
