using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;
using TrainTimeTable.Component;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// DiagramPropertiesクラス
    /// </summary>
    [Serializable]
    public class DiagramProperties : List<DiagramProperty>
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
        public DiagramProperties()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::DiagramProperties()");

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::DiagramProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public DiagramProperties(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::DiagramProperties(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::DiagramProperties(DiagramProperties)");
        }
        #endregion

        #region インデックス取得
        /// <summary>
        /// インデックス取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public int GetIndex(string name)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::string(name)");
            Logger.DebugFormat("name:[{0}]", name);

            // 要素を繰り返す
            for (int i = 0; i < Count; i++)
            {
                // 名称判定
                if (this[i].Name == name)
                {
                    // ロギング
                    Logger.DebugFormat("i:[{0}]", i);
                    Logger.Debug("<<<<= DiagramProperties::string(name)");

                    // 返却
                    return i;
                }
            }

            // ロギング
            Logger.DebugFormat("i:[該当データなし]");
            Logger.Debug("<<<<= DiagramProperties::string(name)");

            // 返却
            return -1;
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::Copy(DiagramProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this , properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(new DiagramProperty(property));
                }

            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::Copy(DiagramProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(DiagramProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::Compare(DiagramProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= DiagramProperties::Compare(DiagramProperties)");

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
                    Logger.Debug("<<<<= DiagramProperties::Compare(DiagramProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= DiagramProperties::Compare(DiagramProperties)");

            // 一致
            return true;
        }
        #endregion

        #region 発着駅設定
        /// <summary>
        /// 発着駅設定
        /// </summary>
        public void DepartureArrivalStationSetting()
        {
            // ロギング
            Logger.Debug("=>>>> DiagramProperties::DepartureArrivalStationSetting()");

            // リストを繰り返す
            foreach(var property in this)
            {
                // 発着駅設定(方向)
                property.Trains.DepartureArrivalStationSetting();
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::DepartureArrivalStationSetting()");
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
            Logger.Debug("=>>>> DiagramProperties::ChangeStationName(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // ダイヤ数分繰り返す
            foreach (var property in this)
            {
                // 列車下り上り分繰り返す
                foreach(var direction  in property.Trains.Values)
                {
                    // 列車分繰り返す
                    foreach (var train in direction)
                    {
                        // 発駅
                        if (train.DepartingStation == oldName)
                        {
                            // 旧駅名⇒新駅名変換
                            train.DepartingStation = newName;
                        }

                        // 着駅
                        if (train.DestinationStation == oldName)
                        {
                            // 旧駅名⇒新駅名変換
                            train.DestinationStation = newName;
                        }

                        // 旧駅名⇒新駅名変換
                        train.StationTimes.FindAll(t => t.StationName == oldName).ForEach(t => t.StationName = newName);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= DiagramProperties::ChangeStationName(string, string)");
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
