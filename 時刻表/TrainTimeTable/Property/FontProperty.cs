using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrainTimeTable.Common;

namespace TrainTimeTable.Property
{
    /// <summary>
    /// FontPropertyクラス
    /// </summary>
    [Serializable]
    public class FontProperty
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// フォント名
        /// </summary>
        public string Name { get; set; } = Const.DefaultFontName;

        /// <summary>
        /// フォントサイズ
        /// </summary>
        public float Size { get; set; } = Const.DefaultFontSize;

        /// <summary>
        /// フォントスタイル
        /// </summary>
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;

        /// <summary>
        /// ボールドフラグ
        /// </summary>
        public bool Bold
        {
            get
            {
                return (FontStyle & FontStyle.Bold) == FontStyle.Bold;
            }
            set
            {
                if (value)
                {
                    FontStyle |= FontStyle.Bold;
                }
                else
                {
                    FontStyle &= ~FontStyle.Bold;
                }
            }
        }

        /// <summary>
        /// イタリックフラグ
        /// </summary>
        public bool Itaric
        {
            get
            {
                return (FontStyle & FontStyle.Italic) == FontStyle.Italic;
            }
            set
            {
                if (value)
                {
                    FontStyle |= FontStyle.Italic;
                }
                else
                {
                    FontStyle &= ~FontStyle.Italic;
                }
            }
        }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FontProperty()
        {
            // ロギング
            Logger.Debug("=>>>> FontProperty::FontProperty()");

            // ロギング
            Logger.Debug("<<<<= FontProperty::FontProperty()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="style"></param>
        public FontProperty(string name, float size, FontStyle style)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperty::FontProperty(string, float, FontStyle)");
            Logger.DebugFormat("name :[{0}]", name);
            Logger.DebugFormat("size :[{0}]", size);
            Logger.DebugFormat("style:[{0}]", style);

            // 設定
            Name = name;
            Size = size;
            FontStyle = style;

            // ロギング
            Logger.Debug("<<<<= FontProperty::FontProperty(string, float, FontStyle)");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property"></param>
        public FontProperty(FontProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperty::FontProperty(FontProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // コピー
            Copy(property);

            // ロギング
            Logger.Debug("<<<<= FontProperty::FontProperty(FontProperty)");
        }
        #endregion

        #region コピー
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="property"></param>
        public void Copy(FontProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperty::Copy(FontProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 同一オブジェクト以外に実施する
            if (!ReferenceEquals(this ,property))
            {
                // コピー
                Name = property.Name;
                Size = property.Size;
                FontStyle = property.FontStyle;
            }

            // ロギング
            Logger.Debug("<<<<= FontProperty::Copy(FontProperty)");
        }
        #endregion

        #region 比較
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Compare(FontProperty property)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperty::Compare(FontProperty)");
            Logger.DebugFormat("property:[{0}]", property);

            // 比較
            if (Name != property.Name)
            {
                // ロギング
                Logger.DebugFormat("Name:[不一致][{0}][{1}]", Name, property.Name);
                Logger.Debug("<<<<= FontProperty::Compare(FontProperty)");

                // 不一致
                return false;
            }
            if (Size != property.Size)
            {
                // ロギング
                Logger.DebugFormat("Size:[不一致][{0}][{1}]", Size, property.Size);
                Logger.Debug("<<<<= FontProperty::Compare(FontProperty)");

                // 不一致
                return false;
            }
            if (FontStyle != property.FontStyle)
            {
                // ロギング
                Logger.DebugFormat("FontStyle:[不一致][{0}][{1}]", FontStyle, property.FontStyle);
                Logger.Debug("<<<<= FontProperty::Compare(FontProperty)");

                // 不一致
                return false;
            }

            // ロギング
            Logger.DebugFormat("result:[一致]");
            Logger.Debug("<<<<= RouteProperty::Compare(RouteProperty)");

            // 一致
            return true;
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="font"></param>
        public void Update(Font font)
        {
            // ロギング
            Logger.Debug("=>>>> FontProperty::Update(Font)");
            Logger.DebugFormat("font:[{0}]", font);

            // 設定
            Name = font.Name;
            Size = font.Size;
            FontStyle = font.Style;

            // ロギング
            Logger.Debug("<<<<= FontProperty::Update(Font)");
        }
        #endregion

        #region Fontオブジェクト取得
        /// <summary>
        /// Fontオブジェクト取得
        /// </summary>
        /// <returns></returns>
        public Font GetFont()
        {
            // ロギング
            Logger.Debug("=>>>> FontProperty::GetFont()");

            // Fontオブジェクト
            Font result = null;

            try
            {
                // Fontオブジェクト生成
                result = new Font(Name, Size, FontStyle);
            }
            catch (Exception ex)
            {
                // ロギング
                Logger.Warn(ex.Message);

                // 標準Fontオブジェクト生成
                result = new Font(Const.DefaultFontName, Const.DefaultFontSize, FontStyle.Regular);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FontProperty::GetFont()");

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
            result.AppendLine(indentstr + string.Format("＜フォント情報＞"));
            result.AppendLine(indentstr + string.Format("　フォント名      :[{0}] ", Name));
            result.AppendLine(indentstr + string.Format("　フォントサイズ  :[{0}] ", Size));
            result.AppendLine(indentstr + string.Format("　フォントスタイル:[{0}] ", FontStyle));

            // 返却
            return result.ToString();
        }
        #endregion
    }
}
