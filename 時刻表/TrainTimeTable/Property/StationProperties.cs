using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationPropertiesクラス
    /// </summary>
    [Serializable]
    public class StationProperties : List<StationProperty>
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
        public StationProperties()
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::StationProperties()");

            // ロギング
            Logger.Debug("<<<<= StationProperties::StationProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public StationProperties(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::StationProperties(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= StationProperties::StationProperties(StationProperties)");
        }
        #endregion

        #region インデクサ
        /// <summary>
        /// StationPropertyオブジェクト取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StationProperty this[string name]
        {
            get
            {
                // ロギング
                Logger.Debug("=>>>> StationProperties::[](string)");
                Logger.DebugFormat("name:[{0}]", name);

                // 結果オブジェクト生成
                StationProperty result = Find(s => s.Name == name);

                // ロギング
                Logger.DebugFormat("result:[{0}]", result);
                Logger.Debug("<<<<= StationProperties::[](string)");

                // 返却
                return result;
            }
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::Copy(StationProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(new StationProperty(property));
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationProperties::Copy(StationProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(StationProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::Compare(StationProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= StationProperties::Compare(StationProperties)");

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
                    Logger.Debug("<<<<= StationProperties::Compare(StationProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= StationProperties::Compare(StationProperties)");

            // 一致
            return true;
        }
        #endregion

        #region 次駅設定
        /// <summary>
        /// 次駅設定
        /// </summary>
        public void SetNextStation()
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::SetNextStation()");

            // 駅数判定
            if (Count == 0)
            {
                // ロギング
                Logger.Debug("<<<<= StationProperties::SetNextStation()");

                // 何もしない
                return;
            }

            // 駅登録分繰り返す(昇順)
            for (int i = 0; i < Count; i++)
            {
                // StationPropertyオブジェクト設定
                StationProperty property = this[i];

                // 次駅情報を念のため削除
                property.NextStations.Clear();

                // NextStationPropertyオブジェクト生成
                NextStationProperty nextStationProperty = new NextStationProperty();

                // NextStationPropertyオブジェクト設定
                nextStationProperty.Name = property.Name;
                nextStationProperty.Direction = DirectionType.Outbound;
                if (!(i + 1 >= Count))
                {
                    nextStationProperty.NextStationName = this[i + 1].Name;
                }

                // 登録
                property.NextStations.Add(nextStationProperty);
            }

            // 終着駅距離
            float terminalStationDistance = this[Count - 1].StationDistanceFromReferenceStations[DirectionType.Outbound];

            // 駅登録分繰り返す(降順)
            for (int i = Count - 1; i >= 0; i--)
            {
                // StationPropertyオブジェクト設定
                StationProperty property = this[i];

                // 駅距離計算
                property.StationDistanceFromReferenceStations[DirectionType.Inbound] = terminalStationDistance - property.StationDistanceFromReferenceStations[DirectionType.Outbound]; ;

                // NextStationPropertyオブジェクト生成
                NextStationProperty nextStationProperty = new NextStationProperty();
                nextStationProperty.Name = property.Name;
                nextStationProperty.Direction = DirectionType.Inbound;
                if (!(i <= 0))
                {
                    nextStationProperty.NextStationName = this[i - 1].Name;
                }

                // 登録
                property.NextStations.Add(nextStationProperty);
            }

            // ロギング
            Logger.Debug("<<<<= StationProperties::SetNextStation()");
        }
        #endregion

        #region 次駅一覧取得
        /// <summary>
        /// 次駅一覧取得
        /// </summary>
        /// <returns></returns>
        public NextStationProperties GetNextStations()
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::GetNextStations()");

            // 結果オブジェクト生成
            NextStationProperties result = new NextStationProperties();

            // 駅数分繰り返す
            foreach (var station in this)
            {
                // 次駅登録
                result.AddRange(station.NextStations);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= StationProperties::GetNextStations()");

            // 返却
            return result;
        }
        #endregion

        #region 駅名変更
        /// <summary>
        /// 駅名変更
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void ChangeStationName(string oldName, string newName)
        {
            // ロギング
            Logger.Debug("=>>>> StationProperties::ChangeStationName(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // 旧駅名⇒新駅名変換
            FindAll(t => t.Name == oldName).ForEach(t => t.Name = newName);

            // 登録数分繰り返す
            foreach (var stationProperty in this)
            {
                // 旧駅名⇒新駅名変換
                stationProperty.NextStations.FindAll(t => t.Name == oldName).ForEach(t => t.Name = newName);
                stationProperty.NextStations.FindAll(t => t.NextStationName == oldName).ForEach(t => t.NextStationName = newName);
            }

            // ロギング
            Logger.Debug("<<<<= StationProperties::ChangeStationName(string, string)");
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
