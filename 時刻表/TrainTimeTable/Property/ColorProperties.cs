using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using TrainTimeTable.Component;
using TrainTimeTable.Property;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// ColorPropertiesクラス
    /// </summary>
    [Serializable]
    public class ColorProperties : Dictionary<string, ColorProperty>
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
        public ColorProperties()
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperties::ColorProperties()");

            // ロギング
            Logger.Debug("<<<<= ColorProperties::ColorProperties()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="properties"></param>
        public ColorProperties(ColorProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperties::ColorProperties(ColorProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // コピー
            Copy(properties);

            // ロギング
            Logger.Debug("<<<<= ColorProperties::ColorProperties(ColorProperties)");
        }
        #endregion

        #region デフォルト値取得
        /// <summary>
        /// デフォルト値取得
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// デフォルト値取得
        /// </summary>
        /// <returns></returns>
        public static ColorProperties GetDefault()
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperties::GetDefault()");

            // 結果オブジェクト生成
            ColorProperties result = new ColorProperties
            {
                // TODO:デフォルト値登録(要見直し)
                { "ダイア画面文字色"              , new ColorProperty(ColorTranslator.FromHtml("#000000")) },
                { "ダイア画面縦横軸色"            , new ColorProperty(ColorTranslator.FromHtml("#C0C0C0")) },
                { "ダイヤ背景基本色1"             , new ColorProperty(ColorTranslator.FromHtml("#FFFFFF")) },
                { "ダイヤ背景基本色2"             , new ColorProperty(ColorTranslator.FromHtml("#FAEAE2")) },
                { "ダイヤ背景基本色3"             , new ColorProperty(ColorTranslator.FromHtml("#DEFEDC")) },
                { "ダイヤ背景基本色4"             , new ColorProperty(ColorTranslator.FromHtml("#FAE3FF")) },
                { "ダイヤ背景基本色5"             , new ColorProperty(ColorTranslator.FromHtml("#FFFFFF")) },
                { "時刻表画面背景色1"             , new ColorProperty(ColorTranslator.FromHtml("#FFFFFF")) },
                { "時刻表画面背景色2"             , new ColorProperty(ColorTranslator.FromHtml("#F0F0F0")) },
                { "時刻表画面背景色3"             , new ColorProperty(ColorTranslator.FromHtml("#FFFFFF")) },
                { "時刻表画面背景色4"             , new ColorProperty(ColorTranslator.FromHtml("#FFFFFF")) },
                { "基準運転時分未満時背景色"      , new ColorProperty(ColorTranslator.FromHtml("#E0E0FF")) },
                { "基準運転時分超過時背景色"      , new ColorProperty(ColorTranslator.FromHtml("#FFFFE0")) },
                { "基準運転時分未定義時背景色"    , new ColorProperty(ColorTranslator.FromHtml("#80FFFF")) },
                { "基準運転時分不適切時背景色"    , new ColorProperty(ColorTranslator.FromHtml("#A0A0A0")) },
                { "運用・運用一覧・駅時刻表文字色", new ColorProperty(ColorTranslator.FromHtml("#000000")) },
                { "運用・運用一覧・駅時刻表枠色"  , new ColorProperty(ColorTranslator.FromHtml("#000000")) }
            };

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= ColorProperties::GetDefault()");

            // 返却
            return result;
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="properties"></param>
        public void Copy(ColorProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperties::Copy(ColorProperties)");
            Logger.DebugFormat("property:[{0}]", properties);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this , properties))
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
            Logger.Debug("<<<<= ColorProperties::Copy(ColorProperties)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool Compare(ColorProperties properties)
        {
            // ロギング
            Logger.Debug("=>>>> ColorProperties::Compare(ColorProperties)");
            Logger.DebugFormat("properties:[{0}]", properties);

            // リストを繰り返す
            foreach (var property in properties)
            {
                // キー登録判定
                if (!ContainsKey(property.Key))
                {
                    // ロギング
                    Logger.DebugFormat("key:[{0}][キー登録なし]", property.Key);
                    Logger.Debug("<<<<= ColorProperties::Compare(ColorProperties)");

                    // 不一致
                    return false;
                }

                // 内容判定
                if (!this[property.Key].Compare(property.Value))
                {
                    // ロギング
                    Logger.DebugFormat("Property:[{0}][{1}][不一致]", this[property.Key].ToString(), property.Value.ToString());
                    Logger.Debug("<<<<= ColorProperties::Compare(ColorProperties)");

                    // 不一致
                    return false;
                }
            }

            // ロギング
            Logger.Debug("result:[一致]");
            Logger.Debug("<<<<= ColorProperties::Compare(ColorProperties)");

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
            result.AppendLine(indentstr + string.Format("＜色情報＞"));

            // リストを繰り返す
            foreach (var property in this)
            {
                // 文字列追加
                result.AppendLine(indentstr + string.Format("　キー:[{0}],値:[{1}]", property.Key, property.Value));
            }

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
