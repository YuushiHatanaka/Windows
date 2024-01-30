using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// StationSequencePropertiesクラス
    /// </summary>
    [Serializable]
    public class StationSequenceProperties : List<StationSequenceProperty>
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
        public StationSequenceProperties()
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::StationSequenceProperties()");

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::StationSequenceProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public StationSequenceProperties(StationSequenceProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::StationSequenceProperties(StationSequenceProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::StationSequenceProperties(StationSequenceProperties)");
        }
        #endregion

        #region インデクサ
        /// <summary>
        /// StationPropertyオブジェクト取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StationSequenceProperty this[string name]
        {
            get
            {
                // ロギング
                Logger.Debug("=>>>> StationSequenceProperties::[](string)");
                Logger.DebugFormat("name:[{0}]", name);

                // 結果オブジェクト生成
                StationSequenceProperty result = Find(s => s.Name == name);

                // ロギング
                Logger.DebugFormat("result:[{0}]", result);
                Logger.Debug("<<<<= StationSequenceProperties::[](string)");

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
        public void Copy(StationSequenceProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::Copy(StationSequenceProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this, properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var property in properties)
                {
                    // 登録
                    Add(new StationSequenceProperty(property));
                }
            }

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::Copy(StationSequenceProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(StationSequenceProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::Compare(StationSequenceProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= StationSequenceProperties::Compare(StationSequenceProperties)");

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
                    Logger.Debug("<<<<= StationSequenceProperties::Compare(StationSequenceProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= StationSequenceProperties::Compare(StationSequenceProperties)");

            // 一致
            return true;
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
            Logger.Debug("=>>>> StationSequenceProperties::ChangeStationName(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // 旧駅名⇒新駅名変換
            FindAll(t => t.Name == oldName).ForEach(t => t.Name = newName);

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::ChangeStationName(string, string)");
        }
        #endregion

        #region シーケンス番号関連
        /// <summary>
        /// シーケンス番号削除
        /// </summary>
        /// <param name="property"></param>
        public void DeleteSequenceNumber(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::DeleteSequenceNumber(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty targetStationSequenceProperty = Find(s => s.Name == property.Name);

            // リストから削除
            Remove(targetStationSequenceProperty);

            // シーケンス番号再構築
            SequenceNumberReconstruction();

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::DeleteSequenceNumber(StationProperty)");
        }

        /// <summary>
        /// シーケンス番号追加
        /// </summary>
        /// <param name="property"></param>
        public void AddSequenceNumber(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::AddSequenceNumber(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 最大シーケンス番号取得
            int insertSeq = this.Max(s => s.Seq) + 1;

            // シーケンス番号挿入
            InsertSequenceNumber(insertSeq, property);

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::AddSequenceNumber(StationProperty)");
        }

        /// <summary>
        /// シーケンス番号挿入
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="property"></param>
        public void InsertSequenceNumber(int seq, StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::InsertSequenceNumber(int, StationProperty)");
            Logger.DebugFormat("seq     :[{0}]", seq);
            Logger.DebugFormat("property:[{0}]", property);

            // StationSequencePropertyオブジェクト生成
            StationSequenceProperty targetStationSequenceProperty = new StationSequenceProperty(seq, property);

            // 挿入
            Insert(seq, targetStationSequenceProperty);

            // シーケンス番号再構築
            SequenceNumberReconstruction();

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::InsertSequenceNumber(int, StationProperty)");
        }

        /// <summary>
        /// シーケンス番号再構築
        /// </summary>
        private void SequenceNumberReconstruction()
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::SequenceNumberReconstruction()");

            // シーケンス番号再付与
            int seq = 1;
            foreach(var property in this)
            {
                // 設定
                property.Seq = seq++;
            }

            // ロギング
            Logger.Debug("<<<<= StationSequenceProperties::SequenceNumberReconstruction()");
        }
        #endregion

        #region 次駅StationPropertyオブジェクト取得
        /// <summary>
        /// 次駅StationPropertyオブジェクト取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="offset"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public StationSequenceProperty Next(DirectionType type, int offset, string name)
        {
            // ロギング
            Logger.Debug("=>>>> StationSequenceProperties::Next(DirectionType, int, string)");
            Logger.DebugFormat("type  :[{0}]", type.GetStringValue());
            Logger.DebugFormat("offset:[{0}]", offset);
            Logger.DebugFormat("name  :[{0}]", name);

            // 結果オブジェクト生成
            StationSequenceProperty current = Find(s => s.Name == name);

            // オフセット変換
            if (type == DirectionType.Inbound)
            {
                offset = -offset;
            }

            // 結果オブジェクト生成
            StationSequenceProperty result = Find(s => s.Seq == current.Seq + offset);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= StationSequenceProperties::Next(DirectionType, int, string)");

            // 返却
            return result;
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
