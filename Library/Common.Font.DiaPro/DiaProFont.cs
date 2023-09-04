using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Common.Font.DiaPro
{
    /// <summary>
    /// DiaProFontクラス
    /// </summary>
    public class DiaProFont
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region フォント辞書
        /// <summary>
        /// フォント辞書
        /// </summary>
        private Dictionary<string, string> m_FontData = new Dictionary<string, string>();
        #endregion

        #region 全キー取得
        /// <summary>
        /// 全キー取得
        /// </summary>
        public Dictionary<string, string>.KeyCollection Keys
        {
            get
            {
                // 全キー返却
                return m_FontData.Keys;
            }
        }

        #endregion

        /// <summary>
        /// フォント辞書インデクサー
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                // キー有無判定
                if(!IsName(name))
                {
                    // 例外
                    throw new KeyNotFoundException();
                }

                // ユニコード返却
                return m_FontData[name];
            }
        }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiaProFont()
        {
            // ロギング
            Logger.Debug("=>>>> DiaProFont::DiaProFont()");

            // 初期化
            Initialization();

            // ロギング
            Logger.Debug("<<<<= DiaProFont::DiaProFont()");
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            // ロギング
            Logger.Debug("=>>>> DiaProFont::Initialization()");

            #region フォント辞書
            #region 数字
            m_FontData.Add("時刻表数字", "3");
            m_FontData.Add("時刻表数字（太字）", "\uFF13");
            m_FontData.Add("数字幅スペース①", " ");
            m_FontData.Add("数字幅スペース②", "-");
            m_FontData.Add("数字幅スペース③", ".");
            #endregion

            #region 時刻欄記号
            m_FontData.Add("通過①", "\u21C2");
            m_FontData.Add("通過①（逆向き）", "\u21BF");
            m_FontData.Add("他線区経由", "\u2551");
            m_FontData.Add("照準線①", "\u2504");
            m_FontData.Add("照準線②", "\u254C");
            m_FontData.Add("この駅止まり①", "\u207C");
            m_FontData.Add("この駅止まり①（逆向き）", "\u208C");
            m_FontData.Add("この駅止まり②", "\u2594");
            m_FontData.Add("この駅止まり②（逆向き）", "\u2581");
            m_FontData.Add("時刻無し①", "\u2025");
            m_FontData.Add("時刻無し②", "\u2026");
            #endregion

            #region 行進方向
            m_FontData.Add("矢のパーツ1", "\u27F1");
            m_FontData.Add("矢のパーツ2", "\u290B");
            m_FontData.Add("矢の直線", "\u2503");
            m_FontData.Add("矢のパーツ3", "\u21DF");
            m_FontData.Add("矢のパーツ3（逆向き）", "\u21DE");
            m_FontData.Add("矢のパーツ2（逆向き）", "\u290A");
            m_FontData.Add("矢のパーツ1（逆向き）", "\u27F0");
            #endregion

            #region 連結線
            m_FontData.Add("分割・連結・変更など①", "\u2B0E");
            m_FontData.Add("分割・連結・変更など②", "\u2B0F");
            m_FontData.Add("分割・連結・変更など③", "\u2B10");
            m_FontData.Add("分割・連結・変更など④", "\u2B11");
            m_FontData.Add("分割・連結・変更など⑤", "\u21B2");
            m_FontData.Add("分割・連結・変更など⑥", "\u21B3");
            m_FontData.Add("分割・連結・変更など⑦", "\u21B0");
            m_FontData.Add("分割・連結・変更など⑧", "\u21B1");
            m_FontData.Add("横方向連結線", "\u2501");
            m_FontData.Add("変更など①", "\u21B6");
            m_FontData.Add("変更など②", "\u21B7");
            m_FontData.Add("変更", "\u5909");
            m_FontData.Add("メモ連結線①", "\u250C");
            m_FontData.Add("メモ連結線②", "\u2510");
            m_FontData.Add("メモ連結線③", "\u2514");
            m_FontData.Add("メモ連結線④", "\u2518");
            m_FontData.Add("メモ連結線⑤", "\u250F");
            m_FontData.Add("メモ連結線⑥", "\u2513");
            m_FontData.Add("メモ連結線⑦", "\u2517");
            m_FontData.Add("メモ連結線⑧", "\u251b");
            #endregion

            #region 発着番線
            m_FontData.Add("番線（数字無し）", "\u25C8");
            m_FontData.Add("0番線", "\u24EA");
            m_FontData.Add("1番線", "\u2460");
            m_FontData.Add("2番線", "\u2461");
            m_FontData.Add("3番線", "\u2462");
            m_FontData.Add("4番線", "\u2463");
            m_FontData.Add("5番線", "\u2464");
            m_FontData.Add("6番線", "\u2465");
            m_FontData.Add("7番線", "\u2466");
            m_FontData.Add("8番線", "\u2467");
            m_FontData.Add("9番線", "\u2468");
            m_FontData.Add("10番線", "\u2469");
            m_FontData.Add("11番線", "\u246A");
            m_FontData.Add("12番線", "\u246B");
            m_FontData.Add("13番線", "\u246C");
            m_FontData.Add("14番線", "\u246D");
            m_FontData.Add("15番線", "\u246E");
            m_FontData.Add("16番線", "\u246F");
            m_FontData.Add("17番線", "\u2470");
            m_FontData.Add("18番線", "\u2471");
            m_FontData.Add("19番線", "\u2472");
            m_FontData.Add("20番線", "\u2473");
            m_FontData.Add("21番線", "\u3251");
            m_FontData.Add("22番線", "\u3252");
            m_FontData.Add("23番線", "\u3253");
            m_FontData.Add("24番線", "\u3254");
            m_FontData.Add("25番線", "\u3255");
            m_FontData.Add("26番線", "\u3256");
            m_FontData.Add("27番線", "\u3257");
            m_FontData.Add("28番線", "\u3258");
            m_FontData.Add("29番線", "\u3259");
            m_FontData.Add("30番線", "\u325A");
            m_FontData.Add("31番線", "\u325B");
            m_FontData.Add("32番線", "\u325C");
            m_FontData.Add("33番線", "\u325D");
            m_FontData.Add("34番線", "\u325E");
            m_FontData.Add("通過②", "\u21E3");
            m_FontData.Add("通過②（逆向き）", "\u21E1");
            m_FontData.Add("通過に連結線①", "\u2356");
            m_FontData.Add("通過に連結線②", "\u234F");
            #endregion

            #region 車両（寝台車）
            m_FontData.Add("寝台車", "\uE030");
            m_FontData.Add("A寝台", "\uE033");
            m_FontData.Add("A寝台1人個室", "\uE034");
            m_FontData.Add("A寝台2人個室", "\uE035");
            m_FontData.Add("A寝台1人個室ロイヤル", "\uE031");
            m_FontData.Add("A寝台2人個室ロイヤル", "\uE032");
            m_FontData.Add("B寝台", "\uE036");
            m_FontData.Add("B寝台1人個室", "\uE037");
            m_FontData.Add("B寝台2人個室", "\uE038");
            m_FontData.Add("寝台車に1Aの文字", "\uE03A");
            m_FontData.Add("寝台車に2Aの文字", "\uE03B");
            m_FontData.Add("寝台車に2の文字", "\uE03C");
            #endregion

            #region 車両（指定席）
            m_FontData.Add("指定席車両あり", "\uE020");
            m_FontData.Add("全車指定席", "\uE022");
            m_FontData.Add("グリーン車のみ指定席", "\uE023");
            m_FontData.Add("部分指定席", "\uE021");
            m_FontData.Add("座席に2Aの文字", "\uE02B");
            m_FontData.Add("座席に2の文字", "\uE02C");
            m_FontData.Add("座席に3Aの文字", "\uE02D");
            m_FontData.Add("座席に3の文字", "\uE02E");
            #endregion

            #region 車両（その他）
            m_FontData.Add("グリーン車指定席", "\uE050");
            m_FontData.Add("グリーン車自由席", "\uE051");
            m_FontData.Add("プレミアムグリーン", "\uE052");
            m_FontData.Add("グランクラス", "\uE053");
            m_FontData.Add("グランクラス(B)", "\uE054");
            m_FontData.Add("二階建て車両", "\uE055");
            m_FontData.Add("ビュフェ連結", "\uE056");
            #endregion

            #region ビュフェ
            m_FontData.Add("食堂車連結", "\uE057");
            m_FontData.Add("禁煙車連結", "\uE059");
            #endregion

            #region 列車種別
            m_FontData.Add("寝台特急", "\uE05A");
            m_FontData.Add("寝台急行", "\uE05B");
            m_FontData.Add("エル特急", "\uE05C");
            m_FontData.Add("区間快速", "\uE040");
            m_FontData.Add("準快速", "\uE041");
            m_FontData.Add("快速", "\uE042");
            m_FontData.Add("新快速", "\uE043");
            m_FontData.Add("特別快速", "\uE044");
            m_FontData.Add("通勤快速", "\uE045");
            m_FontData.Add("直通快速", "\uE046");
            m_FontData.Add("急行", "\uE047");
            m_FontData.Add("特急", "\uE048");
            m_FontData.Add("快特", "\uE049");
            m_FontData.Add("通勤特急", "\uE04A");
            m_FontData.Add("SL", "\uE060");
            m_FontData.Add("N700", "\uE061");
            m_FontData.Add("EFive", "\uE062");
            #endregion

            #region その他記号（駅関係）
            m_FontData.Add("駅弁販売あり", "\u5F01");
            m_FontData.Add("バス", "\uE063");
            m_FontData.Add("みどりの窓口がある駅", "\uE064");
            m_FontData.Add("自転車携帯可", "\uE068");
            m_FontData.Add("空港", "\uE069");
            #endregion

            #region 括弧
            m_FontData.Add("補足説明(上)", "\uFE35");
            m_FontData.Add("補足説明(下)", "\uFE36");
            m_FontData.Add("列車名など(上)", "\uFE39");
            m_FontData.Add("列車名など(下)", "\uFE3A");
            m_FontData.Add("運休日(上)", "\uFE3B");
            m_FontData.Add("運休日(下)", "\uFE3C");
            #endregion

            #region メモ記号
            m_FontData.Add("運転日注意", "\u25C6");
            m_FontData.Add("運転区間注意など", "\u25C9");
            m_FontData.Add("メモ欄", "\u25C8");
            m_FontData.Add("注意書き①", "\u329F");
            m_FontData.Add("注意書き②", "\u2605");
            #endregion

            #region 検索
            m_FontData.Add("下り", "\uE010");
            m_FontData.Add("上り", "\uE011");
            #endregion
            #endregion

            // ロギング
            Logger.Debug("<<<<= DiaProFont::Initialization()");
        }
        #endregion

        #region 存在判定
        /// <summary>
        /// 存在判定
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsName(string name)
        {
            // ロギング
            Logger.Debug("=>>>> DiaProFont::IsName(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // キー有無判定
            if (!m_FontData.ContainsKey(name))
            {
                // ロギング
                Logger.Debug("文字列名:[なし]");
                Logger.Debug("<<<<= DiaProFont::IsName(string)");

                // なし
                return false; ;
            }

            // ロギング
            Logger.Debug("文字列名:[あり]");
            Logger.Debug("<<<<= DiaProFont::IsName(string)");

            // あり
            return true;
        }
        #endregion
    }
}
