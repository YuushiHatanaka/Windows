using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// FontPropertiesクラス
    /// </summary>
    [Serializable]
    public class FontProperties : Dictionary<string, FontProperty>
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
        public FontProperties()
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::FontProperties()");

            // ロギング
            Logger.Debug("<<<<= FontProperties::FontProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public FontProperties(FontProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::FontProperties(FontProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= FontProperties::FontProperties(FontProperties)");
        }
        #endregion

        #region デフォルト値取得
        /// <summary>
        /// デフォルト値取得
        /// </summary>
        /// <returns></returns>
        public static FontProperties GetDefault()
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::GetDefault()");

            // 結果オブジェクト生成
            FontProperties result = new FontProperties()
            {
                // デフォルト値登録
                {"時刻表ビュー"             , new FontProperty("DiaPro"       , Const.DefaultFontSize, FontStyle.Regular)},
                {"時刻表ヘッダー"           , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Regular)},
                {"路線"                     , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Italic)},
                {"駅間距離"                 , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Italic)},
                {"列車番号"                 , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Italic)},
                {"列車種別"                 , new FontProperty("DiaPro"       , Const.DefaultFontSize, FontStyle.Regular)},
                {"列車名"                   , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Bold)},
                {"列車記号"                 , new FontProperty("DiaPro"       , Const.DefaultFontSize, FontStyle.Regular)},
                {"始発駅"                   , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Regular)},
                {"終着駅"                   , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Regular)},
                {"備考"                     , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Regular)},
                {"主要駅"                   , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Bold)},
                {"一般駅"                   , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Regular)},
                {"駅時刻表一覧"             , new FontProperty("MS UI Gothic" , Const.DefaultFontSize, FontStyle.Regular)},
                {"駅時刻表"                 , new FontProperty("DiaPro"       , Const.DefaultFontSize, FontStyle.Regular)},
            };

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FontProperties::GetDefault()");

            // 返却
            return result;
        }
        #endregion

        #region FontProperty取得
        /// <summary>
        /// FontProperty取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FontProperty GetProperty(string name)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::GetProperty(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // 結果オブジェクト生成
            FontProperty result = new FontProperty();

            // 登録判定
            if (ContainsKey(name))
            {
                // 登録あり
                result = this[name];
            }
            else
            {
                // 登録なし
                Logger.WarnFormat("フォント登録なし:[{0}]", name);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FontProperties::GetProperty(string)");

            // 返却
            return result;
        }
        #endregion

        #region Fontオブジェクト取得
        /// <summary>
        /// Fontオブジェクト取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Font GetFont(string name)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::GetFont(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // FontPropertyオブジェクト取得
            FontProperty fontProperty = GetProperty(name);

            // 結果オブジェクト生成
            Font result = new Font(fontProperty.Name, fontProperty.Size, fontProperty.FontStyle);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FontProperties::GetFont(string)");

            // 返却
            return result;
        }
        #endregion

        #region Fontオブジェクト辞書取得
        /// <summary>
        /// Fontオブジェクト辞書取得
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public Dictionary<string, Font> GetFonts(List<string> names)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::GetFonts(List<string>)");
            Logger.DebugFormat("names:[{0}]", names);

            // 結果オブジェクト生成
            Dictionary<string, Font> result = new Dictionary<string, Font>();

            // 名称分繰り返す
            foreach(var name  in names)
            {
                // 登録判定(重複の場合はなにもしない(先登録優先))
                if (!result.ContainsKey(name))
                {
                    // 登録
                    result.Add(name, GetFont(name));
                }
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FontProperties::GetFonts(List<string>)");

            // 返却
            return result;
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(FontProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::Copy(FontProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,properties))
            {
                // クリア
                Clear();

                // 要素を繰り返す
                foreach (var key in properties.Keys)
                {
                    // 登録
                    Add(key, properties[key]);
                }
            }

            // ロギング
            Logger.Debug("<<<<= FontProperties::Copy(FontProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(FontProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperties::Compare(FontProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // キー登録判定
                if (!ContainsKey(property.Key))
                {
                    // ロギング
                    Logger.DebugFormat("key:[{0}][キー登録なし]", property.Key);
                    Logger.Debug("<<<<= FontProperties::Compare(FontProperties)");

                    // 不一致
                    return false;
                }

                // 内容判定
                if (!this[property.Key].Compare(property.Value))
                {
                    // ロギング
                    Logger.DebugFormat("Property:[{0}][{1}][不一致]", this[property.Key].ToString(), property.Value.ToString());
                    Logger.Debug("<<<<= FontProperties::Compare(FontProperties)");

                    // 不一致
                    return false;
                }
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= FontProperties::Compare(FontProperties)");

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

            // 文字列追加
            result.AppendLine(indentstr + string.Format("＜フォント情報＞"));

            // リストを繰り返す
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(indentstr + string.Format("　キー:[{0}],値:[{1}]", property.Key, property.Value.ToString()));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
