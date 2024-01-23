using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// TrainTypeSequencePropertiesクラス
    /// </summary>
    [Serializable]
    public class TrainTypeSequenceProperties : List<TrainTypeSequenceProperty>
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
        public TrainTypeSequenceProperties()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::TrainTypeSequenceProperties()");

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::TrainTypeSequenceProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public TrainTypeSequenceProperties(TrainTypeSequenceProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::TrainTypeSequenceProperties(TrainTypeSequenceProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::TrainTypeSequenceProperties(TrainTypeSequenceProperties)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="collection"></param>
        public TrainTypeSequenceProperties(IEnumerable<TrainTypeSequenceProperty> collection)
            : base(collection)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::TrainTypeSequenceProperties(IEnumerable<TrainTypeSequenceProperty>)");
            Logger.DebugFormat("collection:[{0}]", collection);

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::TrainTypeSequenceProperties(IEnumerable<TrainTypeSequenceProperty>)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(TrainTypeSequenceProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::Copy(TrainTypeSequenceProperties)");
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
                    Add(new TrainTypeSequenceProperty(property));
                }
            }

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::Copy(TrainTypeSequenceProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(TrainTypeSequenceProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::Compare(TrainTypeSequenceProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // 要素数判定
            if (Count != properties.Count)
            {
                // ロギング
                Logger.DebugFormat("Count:[{0}][{1}][不一致]", Count, properties.Count);
                Logger.Debug("<<<<= TrainTypeSequenceProperties::Compare(TrainTypeSequenceProperties)");

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
                    Logger.Debug("<<<<= TrainTypeSequenceProperties::Compare(TrainTypeSequenceProperties)");

                    // 不一致
                    return false;
                }

                // 要素インクリメント
                i++;
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= TrainTypeSequenceProperties::Compare(TrainTypeSequenceProperties)");

            // 一致
            return true;
        }
        #endregion

        #region シーケンス番号関連
        /// <summary>
        /// シーケンス番号削除
        /// </summary>
        /// <param name="property"></param>
        public void DeleteSequenceNumber(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::DeleteSequenceNumber(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // TrainTypeSequencePropertyオブジェクト取得
            TrainTypeSequenceProperty targetProperty = Find(s => s.Name == property.Name);

            // リストから削除
            Remove(targetProperty);

            // シーケンス番号再構築
            SequenceNumberReconstruction();

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::DeleteSequenceNumber(TrainTypeProperty)");
        }

        /// <summary>
        /// シーケンス番号追加
        /// </summary>
        /// <param name="property"></param>
        public void AddSequenceNumber(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::AddSequenceNumber(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 最大シーケンス番号取得
            int insertSeq = this.Max(s => s.Seq) + 1;

            // シーケンス番号挿入
            InsertSequenceNumber(insertSeq, property);

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::AddSequenceNumber(TrainTypeProperty)");
        }

        /// <summary>
        /// シーケンス番号挿入
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="property"></param>
        public void InsertSequenceNumber(int seq, TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::InsertSequenceNumber(int, TrainTypeProperty)");
            Logger.DebugFormat("seq     :[{0}]", seq);
            Logger.DebugFormat("property:[{0}]", property);

            // TrainTypeSequencePropertyオブジェクト生成
            TrainTypeSequenceProperty targetTrainTypeSequenceProperty = new TrainTypeSequenceProperty(seq, property);

            // 挿入
            Insert(seq, targetTrainTypeSequenceProperty);

            // シーケンス番号再構築
            SequenceNumberReconstruction();

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::InsertSequenceNumber(int, TrainTypeProperty)");
        }

        /// <summary>
        /// シーケンス番号入れ替え
        /// </summary>
        /// <param name="oldSequence"></param>
        /// <param name="newSequence"></param>
        public void ChangeOrder(int oldSequence, int newSequence)
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::ChangeOrder(int, int)");
            Logger.DebugFormat("oldSequence:[{0}]", oldSequence);
            Logger.DebugFormat("newSequence:[{0}]", newSequence);

            // パラメータチェック
            if (newSequence > this.Max(t=>t.Seq))
            {
                // ロギング
                Logger.FatalFormat("引数エラー:[newIndex={0}][Max=[{1}]", newSequence, this.Max(t => t.Seq));
                Logger.Debug("<<<<= ListTrainTypeProperty::ChangeOrder(int, int)");

                // 例外
                throw new ArgumentOutOfRangeException(nameof(newSequence));
            }

            // 新旧判定
            if (oldSequence == newSequence)
            {
                // ロギング
                Logger.WarnFormat("パラメータ同一のためインデックス変更なし:[{0}][{1}]", oldSequence, newSequence);
                Logger.Debug("<<<<= ListTrainTypeProperty::ChangeOrder(int, int)");

                // 終了
                return;
            }

            // TrainTypeSequencePropertyオブジェクト設定
            TrainTypeSequenceProperty oldProperty = Find(t => t.Seq == oldSequence);
            TrainTypeSequenceProperty newProperty = Find(t => t.Seq == newSequence);
            int oldSequenceNo = oldProperty.Seq;
            int newSequenceNo = newProperty.Seq;

            // 入れ替え
            oldProperty.Seq = newSequenceNo;
            newProperty.Seq = oldSequenceNo;

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::ChangeOrder(int, int)");
        }

        /// <summary>
        /// シーケンス番号再構築
        /// </summary>
        private void SequenceNumberReconstruction()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::SequenceNumberReconstruction()");

            // シーケンス番号再付与
            int seq = 1;
            foreach (var property in this.OrderBy(s => s.Seq))
            {
                // 設定
                property.Seq = seq++;
            }

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::SequenceNumberReconstruction()");
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
            Logger.Debug("=>>>> TrainTypeSequenceProperties::ChangeTrainType(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // 旧列車種別⇒新列車種別変換
            FindAll(t => t.Name == oldName).ForEach(t => t.Name = newName);

            // ロギング
            Logger.Debug("<<<<= TrainTypeSequenceProperties::ChangeTrainType(string, string)");
        }
        #endregion

        #region データなし削除
        /// <summary>
        /// データなし削除
        /// </summary>
        public void RemoveNoData()
        {
            // ロギング
            Logger.Debug("=>>>> TrainTypeSequenceProperties::RemoveNoData()");

            List<TrainTypeSequenceProperty> removeList = new List<TrainTypeSequenceProperty>();
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
            Logger.Debug("<<<<= TrainTypeSequenceProperties::RemoveNoData()");
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
