using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using TrainTimeTable.Common;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// RouteFilePropertyクラス
    /// </summary>
    [Serializable]
    public class RouteFileProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private readonly static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// ファイル情報プロパティ
        /// </summary>
        public FileInfomationProperty FileInfo { get; set; } = new FileInfomationProperty();

        /// <summary>
        /// 路線プロパティ
        /// </summary>
        public RouteProperty Route { get; set; } = new RouteProperty();

        /// <summary>
        /// フォントプロパティ
        /// </summary>
        public FontProperties Fonts { get; set; } = new FontProperties();

        /// <summary>
        /// 色プロパティ
        /// </summary>
        public ColorProperties Colors { get; set; } = new ColorProperties();

        /// <summary>
        /// ダイヤグラム画面プロパティ
        /// </summary>
        public DiagramScreenProperty DiagramScreen { get; set; } = new DiagramScreenProperty();

        /// <summary>
        /// 駅プロパティ
        /// </summary>
        public StationProperties Stations { get; set; } = new StationProperties();

        /// <summary>
        /// 駅シーケンスプロパティ
        /// </summary>
        public StationSequenceProperties StationSequences { get; set; } = new StationSequenceProperties();

        /// <summary>
        /// 列車種別プロパティ
        /// </summary>
        public TrainTypeProperties TrainTypes { get; set; } = new TrainTypeProperties();

        /// <summary>
        /// 列車種別シーケンスプロパティ
        /// </summary>
        public TrainTypeSequenceProperties TrainTypeSequences { get; set; } = new TrainTypeSequenceProperties();

        /// <summary>
        /// コメントプロパティ
        /// </summary>
        public StringBuilder Comment { get; set; } = new StringBuilder();

        /// <summary>
        /// ダイヤグラムプロパティ
        /// </summary>
        public DiagramProperties Diagrams { get; set; } = new DiagramProperties();

        /// <summary>
        /// ダイアグラムシーケンスプロパティ
        /// </summary>
        public DiagramSequenceProperties DiagramSequences { get; set; } = new DiagramSequenceProperties();

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RouteFileProperty()
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::RouteFileProperty()");

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::RouteFileProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public RouteFileProperty(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::RouteFileProperty(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::RouteFileProperty(RouteFileProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::Copy(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                FileInfo.Copy(property.FileInfo);
                Route.Copy(property.Route);
                Fonts.Copy(property.Fonts);
                Colors.Copy(property.Colors);
                DiagramScreen.Copy(property.DiagramScreen);
                Stations.Copy(property.Stations);
                StationSequences.Copy(property.StationSequences);
                TrainTypes.Copy(property.TrainTypes);
                TrainTypeSequences.Copy(property.TrainTypeSequences);
                Comment.Clear();
                Comment.Append(property.Comment);
                Diagrams.Copy(property.Diagrams);
                DiagramSequences.Copy(property.DiagramSequences);
            }

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::Copy(RouteFileProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(RouteFileProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::Compare(RouteFileProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (!FileInfo.Compare(property.FileInfo))
            {
                // ロギング
                Logger.DebugFormat("FileInfo:[不一致][{0}][{1}]", FileInfo, property.FileInfo);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Route.Compare(property.Route))
            {
                // ロギング
                Logger.DebugFormat("Route:[不一致][{0}][{1}]", Route, property.Route);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Fonts.Compare(property.Fonts))
            {
                // ロギング
                Logger.DebugFormat("Fonts:[不一致][{0}][{1}]", Fonts, property.Fonts);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Colors.Compare(property.Colors))
            {
                // ロギング
                Logger.DebugFormat("Colors:[不一致][{0}][{1}]", Colors, property.Colors);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!DiagramScreen.Compare(property.DiagramScreen))
            {
                // ロギング
                Logger.DebugFormat("DiagramScreen:[不一致][{0}][{1}]", DiagramScreen, property.DiagramScreen);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Stations.Compare(property.Stations))
            {
                // ロギング
                Logger.DebugFormat("Stations:[不一致][{0}][{1}]", Stations, property.Stations);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!StationSequences.Compare(property.StationSequences))
            {
                // ロギング
                Logger.DebugFormat("StationSequence:[不一致][{0}][{1}]", StationSequences, property.StationSequences);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!TrainTypes.Compare(property.TrainTypes))
            {
                // ロギング
                Logger.DebugFormat("TrainTypes:[不一致][{0}][{1}]", TrainTypes, property.TrainTypes);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!TrainTypeSequences.Compare(property.TrainTypeSequences))
            {
                // ロギング
                Logger.DebugFormat("TrainTypeSequences:[不一致][{0}][{1}]", TrainTypeSequences, property.TrainTypeSequences);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (Comment.ToString() != property.Comment.ToString())
            {
                // ロギング
                Logger.DebugFormat("Comment:[不一致][{0}][{1}]", Comment.ToString(), property.Comment.ToString());
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!Diagrams.Compare(property.Diagrams))
            {
                // ロギング
                Logger.DebugFormat("Diagrams:[不一致][{0}][{1}]", Diagrams, property.Diagrams);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }
            if (!DiagramSequences.Compare(property.DiagramSequences))
            {
                // ロギング
                Logger.DebugFormat("DiagramSequences:[不一致][{0}][{1}]", DiagramSequences, property.DiagramSequences);
                Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= RouteFileProperty::Compare(RouteFileProperty)");

            // 一致
            return true;
        }
        #endregion

        #region ダイアグラム関連
        #region ダイヤグラム登録
        /// <summary>
        /// ダイヤグラム登録
        /// </summary>
        /// <param name="property"></param>
        public void RegistonDiagrams(DiagramProperty property)
        {
            Diagrams.Add(property);
            DiagramSequences.Add(new DiagramSequenceProperty() { Name = property.Name });
        }
        #endregion

        #region ダイヤグラム削除
        /// <summary>
        /// ダイヤグラム削除
        /// </summary>
        /// <param name="name"></param>
        public void RemoveDiagram(string name)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::RemoveDiagram(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // 削除
            Diagrams.RemoveAll(s => s.Name == name);
            DiagramSequences.RemoveAll(s => s.Name == name);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::RemoveDiagram(string)");
        }
        #endregion

        #region ダイアグラム名変更
        /// <summary>
        /// ダイアグラム名変更
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void ChangeDiagramName(string oldName, string newName)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::ChangeDiagramName(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // 更新設定
            DiagramProperty diagramProperty = Diagrams.Find(d => d.Name == oldName);
            DiagramSequenceProperty diagramSequenceProperty = DiagramSequences.Find(d => d.Name == oldName);

            // ダイアグラム名変更
            diagramProperty.ChangeDiagramName(newName);
            diagramSequenceProperty.Name = newName;

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::ChangeDiagramName(string, string)");
        }
        #endregion
        #endregion

        #region 駅関連
        #region 駅追加
        /// <summary>
        /// 駅追加
        /// </summary>
        /// <param name="property"></param>
        public void AddStation(StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::AddStation(StationProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 各プロパティで駅名追加
            StationSequences.AddSequenceNumber(property);
            Stations.Add(property);
            Diagrams.AddStation(property);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::AddStation(StationProperty)");
        }
        #endregion

        #region 駅挿入
        /// <summary>
        /// 駅挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="property"></param>
        public void InsertStation(int index, StationProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::InsertStation(int, StationProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("property:[{0}]", property);

            // 各プロパティで駅挿入
            StationSequences.InsertSequenceNumber(index, property);
            Stations.Insert(index, property);
            Diagrams.InsertStation(index, property);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::InsertStation(int, StationProperty)");
        }
        #endregion

        #region 駅削除
        /// <summary>
        /// 駅削除
        /// </summary>
        /// <param name="result"></param>
        public void RemoveStation(StationProperty removeProperty)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::RemoveStation(StationProperty)");
            Logger.DebugFormat("removeProperty:[{0}]", removeProperty);

            // 各プロパティで駅削除
            StationSequences.DeleteSequenceNumber(removeProperty);
            Stations.Remove(removeProperty);
            Diagrams.RemoveStation(removeProperty);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::RemoveStation(StationProperty)");
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
            Logger.Debug("=>>>> RouteFileProperty::ChangeStationName(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // 各プロパティで駅名変更
            Stations.ChangeStationName(oldName, newName);
            StationSequences.ChangeStationName(oldName, newName);
            Diagrams.ChangeStationName(oldName, newName);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::ChangeStationName(string, string)");
        }
        #endregion

        #region 前駅取得
        /// <summary>
        /// 前駅取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public StationProperty GetBeforeStation(DirectionType type, string name)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetBeforeStation(DirectionType, string)");
            Logger.DebugFormat("type:[{0}]", type);
            Logger.DebugFormat("name:[{0}]", name);

            // 結果初期化
            StationProperty result = null;

            // 自駅のStationSequencePropertyを取得
            StationSequenceProperty stationSequenceProperty = StationSequences.Find(s => s.Name == name);

            // 取得シーケンス番号初期化
            int stationSequenceNo = 0;

            // 取得シーケンス番号初期化
            switch (type)
            {
                case DirectionType.Outbound:
                    // 取得シーケンス番号設定
                    stationSequenceNo = stationSequenceProperty.Seq - 1;
                    break;
                case DirectionType.Inbound:
                    // 取得シーケンス番号設定
                    stationSequenceNo = stationSequenceProperty.Seq + 1;
                    break;
                default:
                    // 例外
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type.GetStringValue()));
            }

            // 次駅のStationSequencePropertyを取得
            StationSequenceProperty nextSequenceProperty = StationSequences.Find(s => s.Seq == stationSequenceNo);

            // 結果設定
            if (nextSequenceProperty != null)
            {
                // 該当駅検索
                result = Stations.Find(s => s.Name == nextSequenceProperty.Name);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetBeforeStation(DirectionType, string)");

            // 返却
            return result;
        }

        /// <summary>
        /// 前駅取得
        /// </summary>
        /// <param name="train"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public StationTimeProperty GetBeforeStationTime(TrainProperty train, StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetBeforeStationTime(TrainProperty, StationTimeProperty)");
            Logger.DebugFormat("train   :[{0}]", train);
            Logger.DebugFormat("property:[{0}]", property);

            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty stationSequenceProperty = StationSequences.Find(s => s.Name == property.StationName);

            // 対象駅一覧を取得
            List<StationSequenceProperty> targetList = StationSequences.FindAll(s => s.Seq < stationSequenceProperty.Seq);

            // 結果を初期化
            StationTimeProperty result = null;

            // 一覧を繰り返す
            foreach (var target in targetList.OrderByDescending(s => s.Seq))
            {
                // 条件検索
                result = train.StationTimes.Find(t => t.StationName == target.Name && (t.ArrivalTime != string.Empty || t.DepartureTime != string.Empty));

                // 条件判定
                if (result != null)
                {
                    // 条件一致
                    break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetBeforeStationTime(TrainProperty, StationTimeProperty)");

            // 返却
            return result;
        }

        /// <summary>
        /// 前駅取得
        /// </summary>
        /// <param name="train"></param>
        /// <param name="property"></param>
        /// <param name="treatment"></param>
        /// <returns></returns>
        public StationTimeProperty GetBeforeStationTime(TrainProperty train, StationTimeProperty property, StationTreatment treatment)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetBeforeStationTime(TrainProperty, StationTimeProperty, StationTreatment)");
            Logger.DebugFormat("train    :[{0}]", train);
            Logger.DebugFormat("property :[{0}]", property);
            Logger.DebugFormat("treatment:[{0}]", treatment.GetStringValue());

            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty stationSequenceProperty = StationSequences.Find(s => s.Name == property.StationName);

            // 対象駅一覧を取得
            List<StationSequenceProperty> targetList = StationSequences.FindAll(s => s.Seq < stationSequenceProperty.Seq);

            // 結果を初期化
            StationTimeProperty result = null;

            // 一覧を繰り返す
            foreach (var target in targetList.OrderByDescending(s => s.Seq))
            {
                // 条件検索
                result = train.StationTimes.Find(t => t.StationName == target.Name && t.StationTreatment == treatment);

                // 条件判定
                if (result != null)
                {
                    // 条件一致
                    break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetBeforeStationTime(TrainProperty, StationTimeProperty, StationTreatment)");

            // 返却
            return result;
        }
        #endregion

        #region 次駅取得
        /// <summary>
        /// 次駅取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sequences"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public StationProperty GetAfterStation(DirectionType type, string name)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetAfterStation(DirectionType, string)");
            Logger.DebugFormat("type:[{0}]", type);
            Logger.DebugFormat("name:[{0}]", name);

            // 結果初期化
            StationProperty result = null;

            // 自駅のStationSequencePropertyを取得
            StationSequenceProperty stationSequenceProperty = StationSequences.Find(s => s.Name == name);

            // 取得シーケンス番号初期化
            int stationSequenceNo = 0;

            // 取得シーケンス番号初期化
            switch (type)
            {
                case DirectionType.Outbound:
                    // 取得シーケンス番号設定
                    stationSequenceNo = stationSequenceProperty.Seq + 1;
                    break;
                case DirectionType.Inbound:
                    // 取得シーケンス番号設定
                    stationSequenceNo = stationSequenceProperty.Seq - 1;
                    break;
                default:
                    // 例外
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", type.GetStringValue()));
            }

            // 次駅のStationSequencePropertyを取得
            StationSequenceProperty nextSequenceProperty = StationSequences.Find(s => s.Seq == stationSequenceNo);

            // 結果設定
            if (nextSequenceProperty != null)
            {
                // 該当駅検索
                result = Stations.Find(s => s.Name == nextSequenceProperty.Name);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetAfterStation(DirectionType, string)");

            // 返却
            return result;
        }

        /// <summary>
        /// 次駅取得
        /// </summary>
        /// <param name="train"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public StationTimeProperty GetAfterStationTime(TrainProperty train, StationTimeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetAfterStationTime(TrainProperty, StationTimeProperty)");
            Logger.DebugFormat("train   :[{0}]", train);
            Logger.DebugFormat("property:[{0}]", property);

            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty stationSequenceProperty = StationSequences.Find(s => s.Name == property.StationName);

            // 対象駅一覧を取得
            List<StationSequenceProperty> targetList = StationSequences.FindAll(s => s.Seq > stationSequenceProperty.Seq);

            // 結果を初期化
            StationTimeProperty result = null;

            // 一覧を繰り返す
            foreach (var target in targetList.OrderBy(s => s.Seq))
            {
                // 条件検索
                result = train.StationTimes.Find(t => t.StationName == target.Name && (t.ArrivalTime != string.Empty || t.DepartureTime != string.Empty));

                // 条件判定
                if (result != null)
                {
                    // 条件一致
                    break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetAfterStationTime(TrainProperty, StationTimeProperty)");

            // 返却
            return result;
        }

        /// <summary>
        /// 次駅取得
        /// </summary>
        /// <param name="train"></param>
        /// <param name="property"></param>
        /// <param name="treatment"></param>
        /// <returns></returns>
        public StationTimeProperty GetAfterStationTime(TrainProperty train, StationTimeProperty property, StationTreatment treatment)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetAfterStationTime(TrainProperty, StationTimeProperty, StationTreatment)");
            Logger.DebugFormat("train    :[{0}]", train);
            Logger.DebugFormat("property :[{0}]", property);
            Logger.DebugFormat("treatment:[{0}]", treatment.GetStringValue());

            // StationSequencePropertyオブジェクト取得
            StationSequenceProperty stationSequenceProperty = StationSequences.Find(s => s.Name == property.StationName);

            // 対象駅一覧を取得
            List<StationSequenceProperty> targetList = StationSequences.FindAll(s => s.Seq > stationSequenceProperty.Seq);

            // 結果を初期化
            StationTimeProperty result = null;

            // 一覧を繰り返す
            foreach (var target in targetList.OrderBy(s => s.Seq))
            {
                // 条件検索
                result = train.StationTimes.Find(t => t.StationName == target.Name && t.StationTreatment == treatment);

                // 条件判定
                if (result != null)
                {
                    // 条件一致
                    break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetAfterStationTime(TrainProperty, StationTimeProperty, StationTreatment)");

            // 返却
            return result;
        }
        #endregion
        #endregion

        #region 列車種別駅関連
        #region 列車種別追加
        /// <summary>
        /// 列車種別追加
        /// </summary>
        /// <param name="property"></param>
        public void AddTrainType(TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::AddTrainType(TrainTypeProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 各プロパティで列車種別追加
            TrainTypeSequences.AddSequenceNumber(property);
            TrainTypes.Add(property);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::AddTrainType(TrainTypeProperty)");
        }
        #endregion

        #region 列車種別挿入
        /// <summary>
        /// 列車種別挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="property"></param>
        public void InsertTrainType(int index, TrainTypeProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::InsertTrainType(int, TrainTypeProperty)");
            Logger.DebugFormat("index   :[{0}]", index);
            Logger.DebugFormat("property:[{0}]", property);

            // 各プロパティで列車種別挿入
            TrainTypeSequences.InsertSequenceNumber(index, property);
            TrainTypes.Add(property);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::InsertTrainType(int, TrainTypeProperty)");
        }
        #endregion

        #region 列車種別削除
        /// <summary>
        /// 列車種別削除
        /// </summary>
        /// <param name="removeProperty"></param>
        internal void RemoveTrainType(TrainTypeProperty removeProperty)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::RemoveTrainType(TrainTypeProperty)");
            Logger.DebugFormat("removeProperty:[{0}]", removeProperty);

            // 各プロパティで列車種別削除
            TrainTypeSequences.DeleteSequenceNumber(removeProperty);
            TrainTypes.Remove(removeProperty);
            Diagrams.RemoveTrainType(removeProperty);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::RemoveTrainType(TrainTypeProperty)");
        }
        #endregion

        #region 列車種別変更
        /// <summary>
        /// 列車種別変更
        /// </summary>
        /// <param name="oldStationName"></param>
        /// <param name="name"></param>
        public void ChangeTrainType(string oldName, string newName)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::ChangeTrainType(string, string)");
            Logger.DebugFormat("oldName:[{0}]", oldName);
            Logger.DebugFormat("newName:[{0}]", newName);

            // 各プロパティで列車種別変更
            TrainTypes.ChangeTrainType(oldName, newName);
            TrainTypeSequences.ChangeTrainType(oldName, newName);
            Diagrams.ChangeTrainType(oldName, newName);

            // ロギング
            Logger.Debug("<<<<= RouteFileProperty::ChangeTrainType(string, string)");
        }
        #endregion
        #endregion

        #region StationTimeProperty取得
        /// <summary>
        /// 始発駅StationTimeProperty取得
        /// </summary>
        /// <param name="train"></param>
        /// <param name="treatment"></param>
        /// <returns></returns>
        public StationTimeProperty GetStartingStationTime(TrainProperty train, StationTreatment treatment)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetStartingStationTime(TrainProperty, StationTreatment)");
            Logger.DebugFormat("train    :[{0}]", train);
            Logger.DebugFormat("treatment:[{0}]", treatment.GetStringValue());

            // 結果を初期化
            StationTimeProperty result = null;

            // 駅扱い該当駅一覧取得
            List<StationTimeProperty> targetStationTimeProperties = train.StationTimes.FindAll(s => s.StationTreatment == treatment);

            // 駅シーケンスリスト
            List<StationSequenceProperty> stationSequenceProperties = null;

            // 方向種別で分岐
            switch (train.Direction)
            {
                case DirectionType.Outbound:
                    // 駅シーケンスリスト取得
                    stationSequenceProperties = StationSequences.OrderBy(s => s.Seq).ToList();
                    break;
                case DirectionType.Inbound:
                    // 駅シーケンスリスト取得
                    stationSequenceProperties = StationSequences.OrderByDescending(s => s.Seq).ToList();
                    break;
                default:
                    // 例外
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", train.Direction.GetStringValue()));
            }

            // シーケンスを繰り返す
            foreach (var stationSequence in stationSequenceProperties)
            {
                // 駅名存在判定
                result = targetStationTimeProperties.Find(s => s.StationName == stationSequence.Name);
                if (result != null)
                {
                    break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetStartingStationTime(TrainProperty, StationTreatment)");

            // 返却
            return result;
        }

        /// <summary>
        /// 終着駅StationTimeProperty取得
        /// </summary>
        /// <param name="train"></param>
        /// <param name="treatment"></param>
        /// <returns></returns>
        public StationTimeProperty GetTerminalStationTime(TrainProperty train, StationTreatment treatment)
        {
            // ロギング
            Logger.Debug("=>>>> RouteFileProperty::GetTerminalStationTime(TrainProperty, StationTreatment)");
            Logger.DebugFormat("train    :[{0}]", train);
            Logger.DebugFormat("treatment:[{0}]", treatment.GetStringValue());

            // 結果を初期化
            StationTimeProperty result = null;

            // 駅扱い該当駅一覧取得
            List<StationTimeProperty> targetStationTimeProperties = train.StationTimes.FindAll(s => s.StationTreatment == treatment);

            // 駅シーケンスリスト
            List<StationSequenceProperty> stationSequenceProperties = null;

            // 方向種別で分岐
            switch (train.Direction)
            {
                case DirectionType.Outbound:
                    // 駅シーケンスリスト取得
                    stationSequenceProperties = StationSequences.OrderByDescending(s => s.Seq).ToList();
                    break;
                case DirectionType.Inbound:
                    // 駅シーケンスリスト取得
                    stationSequenceProperties = StationSequences.OrderBy(s => s.Seq).ToList();
                    break;
                default:
                    // 例外
                    throw new AggregateException(string.Format("方向種別の異常を検出しました:[{0}]", train.Direction.GetStringValue()));
            }

            // シーケンスを繰り返す
            foreach (var stationSequence in stationSequenceProperties)
            {
                // 駅名存在判定
                result = targetStationTimeProperties.Find(s => s.StationName == stationSequence.Name);
                if (result != null)
                {
                    break;
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= RouteFileProperty::GetTerminalStationTime(TrainProperty, StationTreatment)");

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

            // 文字列追加
            result.Append(FileInfo.ToString(indent + 1));
            result.Append(Route.ToString(indent + 1));
            result.Append(Fonts.ToString(indent + 1));
            result.Append(Colors.ToString(indent + 1));
            result.Append(DiagramScreen.ToString(indent + 1));
            result.Append(Stations.ToString(indent + 1));
            result.Append(StationSequences.ToString(indent + 1));
            result.Append(TrainTypes.ToString(indent + 1));
            result.AppendLine(indentstr + string.Format("＜コメント＞"));
            result.AppendLine(indentstr + string.Format("　{0}", Comment.ToString()));
            result.Append(Diagrams.ToString(indent + 1));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
