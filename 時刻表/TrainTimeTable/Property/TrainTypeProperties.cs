using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// TrainTypePropertiesクラス
    /// </summary>
    [Serializable]
    public class TrainTypeProperties : List<TrainTypeProperty>
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
        public TrainTypeProperties()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperties::TrainTypeProperties()");

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperties::TrainTypeProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public TrainTypeProperties(TrainTypeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperties::TrainTypeProperties(TrainTypeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperties::TrainTypeProperties(TrainTypeProperties)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(TrainTypeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperties::Copy(TrainTypeProperties)");
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
                    Add(new TrainTypeProperty(property));
                }
            }

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperties::Copy(TrainTypeProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(TrainTypeProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperties::Compare(TrainTypeProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= TrainTypeProperties::Compare(TrainTypeProperties)");

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
                    Logger.Debug("<<<<= TrainTypeProperties::Compare(TrainTypeProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= TrainTypeProperties::Compare(TrainTypeProperties)");

            // 一致
            return true;
        }
        #endregion

        #region 列車種別取得
        /// <summary>
        /// 列車種別取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TrainTypeProperty GetTrainType(string name)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperties::GetTrainType(string)");

            // TrainTypePropertyオブジェクト取得
            TrainTypeProperty result = Find(t => t.Name == name);

            // 取得結果判定
            if (result == null)
            {
                // 文字色設定
                result = new TrainTypeProperty();
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= TrainTypeProperties::GetTrainType(string)");

            // 返却
            return result;
        }
        #endregion

        #region 列車種別変更
        /// <summary>
        /// 列車種別変更
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void ChangeTrainType(string oldName, string newName)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperties::ChangeTrainType(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // 旧列車種別⇒新列車種別変換
            FindAll(t => t.Name == oldName).ForEach(t => t.Name = newName);

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperties::ChangeTrainType(string, string)");
        }
        #endregion

        #region データなし削除
        /// <summary>
        /// データなし削除
        /// </summary>
        public void RemoveNoData()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeProperties::RemoveNoData()");

            List<TrainTypeProperty> removeList = new List<TrainTypeProperty>();
            foreach (var trainType in this)
            {
                if (trainType.Name == string.Empty)
                {
                    removeList.Add(trainType);
                }
            }
            foreach (var property in removeList)
            {
                Remove(property);
            }

            // ロギング
            Logger.Debug("<<<<= TrainTypeProperties::RemoveNoData()");
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
            foreach(var property in this)
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
