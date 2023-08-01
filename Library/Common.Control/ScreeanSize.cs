using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Common.Control
{
    /// <summary>
    /// 画面サイズクラス
    /// </summary>
    public class ScreeanSize
    {
        /// <summary>
        /// 画面サイズリスト
        /// </summary>
        private Dictionary<ScreeanSizeType, Size> m_List = new Dictionary<ScreeanSizeType, Size>();

        /// <summary>
        /// インデクサー
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Size this[ScreeanSizeType type]
        {
            get
            {
                // 画面サイズリスト返却
                return this.m_List[type];
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ScreeanSize()
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// 画面サイズ取得
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public ScreeanSizeType Get(ScreeanSizeType defaultValue, int Width, int Height)
        {
            Debug.WriteLine("■{0} = Width:{1},Height:{2}", defaultValue.ToString(), Width, Height);

            // 結果を初期化
            ScreeanSizeType result = defaultValue;
            int differenceValueWidthMin = int.MaxValue;
            int differenceValueHeightMin = int.MaxValue;

            // 画面サイズリストを繰り返し
            foreach (ScreeanSizeType s in this.m_List.Keys)
            {
                Debug.WriteLine("□{0} = Width:{1}<>{3},Height:{2}<>{4}", s.ToString(), this.m_List[s].Width, this.m_List[s].Height, Width, Height);

                // どちらも超えているので対象外
                if ((this.m_List[s].Width > Width) || (this.m_List[s].Height > Height))
                {
                    Debug.WriteLine("　⇒　超過");
                    continue;
                }

                // 幅、高さともに同じなら返却する
                if ((this.m_List[s].Width == Width) && (this.m_List[s].Height == Height))
                {
                    // 結果設定
                    result = s;
                    Debug.WriteLine("　⇒　決定");
                    break;
                }

                // 候補値と差を計算
                Console.WriteLine("　⇒　比較");
                int differenceValueWidth = Width - this.m_List[s].Width;
                int differenceValueHeight = Height - this.m_List[s].Height;
                Debug.WriteLine("　└{0} =  = Width:{1},Height:{2}", s.ToString(), differenceValueWidth, differenceValueHeight);

                if ((differenceValueWidthMin >= differenceValueWidth) && (differenceValueHeightMin >= differenceValueHeight))
                {
                    result = s;
                    differenceValueWidthMin = differenceValueWidth;
                    differenceValueHeightMin = differenceValueHeight;
                    Debug.WriteLine("　　└{0}(更新) =  = Width:{1},Height:{2}", result.ToString(), differenceValueWidthMin, differenceValueHeightMin);
                }

            }

            // 結果を返却
            Debug.WriteLine("画面種別：" + result.ToString());
            return result;
        }

        /// <summary>
        /// 画面サイズ取得
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public ScreeanSizeType Get(int Width, int Height)
        {
            // 結果返却
            return this.Get(ScreeanSizeType.UNKNOWN, Width, Height);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            #region 画面サイズリスト初期化
            // 不明
            // 0 x 0
            this.m_List.Add(ScreeanSizeType.UNKNOWN, new Size());
            // QQVGA (Quarter QVGA)
            // 160 x 120
            this.m_List.Add(ScreeanSizeType.QQVGA, new Size(160, 120));
            // QCIF (Quarter CIF)
            // 176 x 144
            this.m_List.Add(ScreeanSizeType.QCIF, new Size(176, 144));
            // QVGA (Quarter VGA)
            // 320 x 240
            this.m_List.Add(ScreeanSizeType.Quarter_VGA, new Size(320, 240));
            // SIF (Source Input Format)
            // 352 x 240
            this.m_List.Add(ScreeanSizeType.SIF, new Size(352, 240));
            // CGA (Color Graphics Adapter)
            // 640 x 200
            this.m_List.Add(ScreeanSizeType.CGA, new Size(640, 200));
            // HVGA (Half VGA)
            // 480 x 320
            this.m_List.Add(ScreeanSizeType.HVGA, new Size(480, 320));
            // EGA (Enhanced Graphics Adapter)
            // 640 x 350
            this.m_List.Add(ScreeanSizeType.EGA, new Size(640, 350));
            // PC-98 DCGA (Double Scan CGA)
            // 640 x 400
            this.m_List.Add(ScreeanSizeType.DCGA, new Size(640, 400));
            // VGA (Video Graphics Array) SD
            // 640 x 480
            this.m_List.Add(ScreeanSizeType.VGA, new Size(640, 480));
            // SVGA (Super-VGA)
            // 800 x 600
            this.m_List.Add(ScreeanSizeType.SVGA, new Size(800, 600));
            // XGA (eXtended Graphics Array)
            // 1024 x 768
            this.m_List.Add(ScreeanSizeType.XGA, new Size(1024, 768));
            // HD (720p)
            // 1280 x 720
            this.m_List.Add(ScreeanSizeType.HD, new Size(1280, 720));
            // WXGA (Wide XGA)
            // 1280 x 768
            this.m_List.Add(ScreeanSizeType.WXGA_1280_768, new Size(1280, 768));
            // WXGA (Wide XGA)
            // 1280 x 800
            this.m_List.Add(ScreeanSizeType.WXGA_1280_800, new Size(1280, 800));
            // HD FWXGA (Full-WXGA)
            // 1366 x 768
            this.m_List.Add(ScreeanSizeType.HD_FWXGA, new Size(1366, 768));
            // HD+
            // 1520 x 720
            this.m_List.Add(ScreeanSizeType.HD_Plus_1520_720, new Size(1520, 720));
            // HD+
            // 1600 x 720
            this.m_List.Add(ScreeanSizeType.HD_Plus_1600_720, new Size(1600, 720));
            // QVGA (Quad VGA)
            // 1280 x 960
            this.m_List.Add(ScreeanSizeType.Quad_VGA, new Size(1280, 960));
            // WXGA+ (Wide XGA+)
            // 1440 x 900
            this.m_List.Add(ScreeanSizeType.WXGA_Plus, new Size(1440, 900));
            // SXGA (Super XGA)
            // 1280 x 1024
            this.m_List.Add(ScreeanSizeType.SXGA, new Size(1280, 1024));
            // HD+ WXGA++ (Wide XGA++)
            // 1600 x 900
            this.m_List.Add(ScreeanSizeType.HD_Plus_WXGA_Plus_Plus, new Size(1600, 900));
            // SXGA+
            // 1400 x 1050
            this.m_List.Add(ScreeanSizeType.SXGA_Plus, new Size(1400, 1050));
            // HD+
            // 1792 x 828
            this.m_List.Add(ScreeanSizeType.HD_Plus_1792_828, new Size(1792, 828));
            // WSXGA
            // 1600 x 1024
            this.m_List.Add(ScreeanSizeType.WSXGA, new Size(1600, 1024));
            // WSXGA+
            // 1680 x 1050
            this.m_List.Add(ScreeanSizeType.WSXGA_Plus, new Size(1680, 1050));
            // UXGA (Ultra XGA)
            // 1600 x 1200
            this.m_List.Add(ScreeanSizeType.UXGA, new Size(1600, 1200));
            // FHD (Full-HD, 1080p)／2K
            // 1920 x 1080
            this.m_List.Add(ScreeanSizeType.FHD, new Size(1920, 1080));
            // WUXGA (Wide Ultra-XGA)
            // 1920 x 1200
            this.m_List.Add(ScreeanSizeType.WUXGA, new Size(1920, 1200));
            // FHD+
            // 2160 x 1080
            this.m_List.Add(ScreeanSizeType.FHD_Plus_2160_1080, new Size(2160, 1080));
            // FHD+
            // 2280 x 1080
            this.m_List.Add(ScreeanSizeType.FHD_Plus_2280_1080, new Size(2280, 1080));
            // FHD+
            // 2312 x 1080
            this.m_List.Add(ScreeanSizeType.FHD_Plus_2312_1080, new Size(2312, 1080));
            // FHD+
            // 2340 x 1080
            this.m_List.Add(ScreeanSizeType.FHD_Plus_2340_1080, new Size(2340, 1080));
            // FHD+
            // 2520 x 1080
            this.m_List.Add(ScreeanSizeType.FHD_Plus_2520_1080, new Size(2520, 1080));
            // FHD+
            // 2436 x 1125
            this.m_List.Add(ScreeanSizeType.FHD_Plus_2436_1125, new Size(2436, 1125));
            // UltraWide FHD
            // 2560 x 1080
            this.m_List.Add(ScreeanSizeType.UltraWide_FHD, new Size(2560, 1080));
            // QXGA (Quad XGA)
            // 2048 x 1536
            this.m_List.Add(ScreeanSizeType.QXGA, new Size(2048, 1536));
            // FHD+
            // 2688 x 1242
            this.m_List.Add(ScreeanSizeType.FHD_Plus_2688_1242, new Size(2688, 1242));
            // WQHD (Wide Quad-HD)
            // 2560 x 1440
            this.m_List.Add(ScreeanSizeType.WQHD, new Size(2560, 1440));
            // WQXGA
            // 2560 x 1600
            this.m_List.Add(ScreeanSizeType.WQXGA, new Size(2560, 1600));
            // Full Vision QHD
            // 2880 x 1440
            this.m_List.Add(ScreeanSizeType.Full_Vision_QHD, new Size(2880, 1440));
            // 2K Square
            // 2048 x 2048
            this.m_List.Add(ScreeanSizeType._2K_Square, new Size(2048, 2048));
            // WQHD+
            // 2960 x 1440
            this.m_List.Add(ScreeanSizeType.WQHD_Plus, new Size(2960, 1440));
            // Pixel A5
            // 2560 x 1800
            this.m_List.Add(ScreeanSizeType.Pixel_A5, new Size(2560, 1800));
            // 3K
            // 2880 x 1620
            this.m_List.Add(ScreeanSizeType._3K, new Size(2880, 1620));
            // Ultra-Wide QHD (UWQHD)
            // 3440 x 1440
            this.m_List.Add(ScreeanSizeType.UWQHD, new Size(3440, 1440));
            // Surface 12.3″
            // 2736 x 1842
            this.m_List.Add(ScreeanSizeType.Surface_12_3, new Size(2736, 1842));
            // 3K (QHD+)
            // 3008 x 1692
            this.m_List.Add(ScreeanSizeType.QHD_Plus, new Size(3008, 1692));
            // QWXGA+ (Quad WXGA+)
            // 2880 x 1800
            this.m_List.Add(ScreeanSizeType.QWXGA_Plus, new Size(2880, 1800));
            // QSXGA (Quad SXGA)
            // 2560 x 2048
            this.m_List.Add(ScreeanSizeType.QSXGA, new Size(2560, 2048));
            // iPad Pro 12.9″
            // 2732 x 2048
            this.m_List.Add(ScreeanSizeType.iPad_Pro_12_9, new Size(2732, 2048));
            // QHD+ (Quad HD+)／WQXGA+
            // 3200 x 1800
            this.m_List.Add(ScreeanSizeType.WQXGA_Plus, new Size(3200, 1800));
            // Surface 13.5″
            // 3000 x 2000
            this.m_List.Add(ScreeanSizeType.Surface_13_5, new Size(3000, 2000));
            // UltraWide QHD+
            // 3840 x 1600
            this.m_List.Add(ScreeanSizeType.UltraWide_QHD_Plus_3840_1600, new Size(3840, 1600));
            // UltraWide QHD+（4K HDR）
            // 3840 x 1644
            this.m_List.Add(ScreeanSizeType.UltraWide_QHD_Plus_3840_1644, new Size(3840, 1644));
            // QUXGA (Quad UXGA)
            // 3200 x 2400
            this.m_List.Add(ScreeanSizeType.QUXGA, new Size(3200, 2400));
            // 4K／QFHD (Quad Full-HD)／UHD 4K (2160p)
            // 3840 x 2160
            this.m_List.Add(ScreeanSizeType._4K, new Size(3840, 2160));
            // DCI 4K
            // 4096 x 2160
            this.m_List.Add(ScreeanSizeType.DCI_4K, new Size(4096, 2160));
            // WQUXGA (Wide QUXGA)
            // 3840 x 2400
            this.m_List.Add(ScreeanSizeType.WQUXGA, new Size(3840, 2400));
            // iMac Retina 4K
            // 4096 x 2304
            this.m_List.Add(ScreeanSizeType.iMac_Retina_4K, new Size(4096, 2304));
            // DCI 4K+
            // 4096 x 2560
            this.m_List.Add(ScreeanSizeType.DCI_4K_Plus, new Size(4096, 2560));
            // 5K／UHD+
            // 5120 x 2880
            this.m_List.Add(ScreeanSizeType._5K, new Size(5120, 2880));
            // 6K／XDR(Extreme Dynamic Range)
            // 6016 x 3384
            this.m_List.Add(ScreeanSizeType._6K, new Size(6016, 3384));
            // 8K FUHD (4320p)／スーパーハイビジョン[
            // 7680 x 4320
            this.m_List.Add(ScreeanSizeType._8K_FUHD, new Size(7680, 4320));
            // 10K
            // 10240 x 4320
            this.m_List.Add(ScreeanSizeType._10K, new Size(10240, 4320));
            // 16K
            // 15360 x 4320
            this.m_List.Add(ScreeanSizeType._16K_15360_4320, new Size(15360, 4320));
            // 16K
            // 15360 x 8640
            this.m_List.Add(ScreeanSizeType._16K_15360_8640, new Size(15360, 8640));
            #endregion
        }
    }

    /// <summary>
    /// 画面サイズ種別
    /// <see cref="https://ja.wikipedia.org/wiki/%E7%94%BB%E9%9D%A2%E8%A7%A3%E5%83%8F%E5%BA%A6"/>
    /// </summary>
    public enum ScreeanSizeType : int
    {
        /// <summary>
        /// 不明
        /// 0 x 0
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// QQVGA (Quarter QVGA)
        /// 160 x 120
        /// </summary>
        QQVGA,
        /// <summary>
        /// QCIF (Quarter CIF)
        /// 176 x 144
        /// </summary>
        QCIF,
        /// <summary>
        /// QVGA (Quarter VGA)
        /// 320 x 240
        /// </summary>
        Quarter_VGA,
        /// <summary>
        /// SIF (Source Input Format)
        /// 352 x 240
        /// </summary>
        SIF,
        /// <summary>
        /// CGA (Color Graphics Adapter)
        /// 640 x 200
        /// </summary>
        CGA,
        /// <summary>
        /// HVGA (Half VGA)
        /// 480 x 320
        /// </summary>
        HVGA,
        /// <summary>
        /// EGA (Enhanced Graphics Adapter)
        /// 640 x 350
        /// </summary>
        EGA,
        /// <summary>
        /// PC-98 DCGA (Double Scan CGA)
        /// 640 x 400
        /// </summary>
        DCGA,
        /// <summary>
        /// VGA (Video Graphics Array) SD
        /// 640 x 480
        /// </summary>
        VGA,
        /// <summary>
        /// SVGA (Super-VGA)
        /// 800 x 600
        /// </summary>
        SVGA,
        /// <summary>
        /// XGA (eXtended Graphics Array)
        /// 1024 x 768
        /// </summary>
        XGA,
        /// <summary>
        /// HD (720p)
        /// 1280 x 720
        /// </summary>
        HD,
        /// <summary>
        /// WXGA (Wide XGA)
        /// 1280 x 768
        /// </summary>
        WXGA_1280_768,
        /// <summary>
        /// WXGA (Wide XGA)
        /// 1280 x 800
        /// </summary>
        WXGA_1280_800,
        /// <summary>
        /// HD FWXGA (Full-WXGA)
        /// 1366 x 768
        /// </summary>
        HD_FWXGA,
        /// <summary>
        /// HD+
        /// 1520 x 720
        /// </summary>
        HD_Plus_1520_720,
        /// <summary>
        /// HD+
        /// 1600 x 720
        /// </summary>
        HD_Plus_1600_720,
        /// <summary>
        /// QVGA (Quad VGA)
        /// 1280 x 960
        /// </summary>
        Quad_VGA,
        /// <summary>
        /// WXGA+ (Wide XGA+)
        /// 1440 x 900
        /// </summary>
        WXGA_Plus,
        /// <summary>
        /// SXGA (Super XGA)
        /// 1280 x 1024
        /// </summary>
        SXGA,
        /// <summary>
        /// HD+ WXGA++ (Wide XGA++)
        /// 1600 x 900
        /// </summary>
        HD_Plus_WXGA_Plus_Plus,
        /// <summary>
        /// SXGA+
        /// 1400 x 1050
        /// </summary>
        SXGA_Plus,
        /// <summary>
        /// HD+
        /// 1792 x 828
        /// </summary>
        HD_Plus_1792_828,
        /// <summary>
        /// WSXGA
        /// 1600 x 1024
        /// </summary>
        WSXGA,
        /// <summary>
        /// WSXGA+
        /// 1680 x 1050
        /// </summary>
        WSXGA_Plus,
        /// <summary>
        /// UXGA (Ultra XGA)
        /// 1600 x 1200
        /// </summary>
        UXGA,
        /// <summary>
        /// FHD (Full-HD, 1080p)／2K
        /// 1920 x 1080
        /// </summary>
        FHD,
        /// <summary>
        /// WUXGA (Wide Ultra-XGA)
        /// 1920 x 1200
        /// </summary>
        WUXGA,
        /// <summary>
        /// FHD+
        /// 2160 x 1080
        /// </summary>
        FHD_Plus_2160_1080,
        /// <summary>
        /// FHD+
        /// 2280 x 1080
        /// </summary>
        FHD_Plus_2280_1080,
        /// <summary>
        /// FHD+
        /// 2312 x 1080
        /// </summary>
        FHD_Plus_2312_1080,
        /// <summary>
        /// FHD+
        /// 2340 x 1080
        /// </summary>
        FHD_Plus_2340_1080,
        /// <summary>
        /// FHD+
        /// 2520 x 1080
        /// </summary>
        FHD_Plus_2520_1080,
        /// <summary>
        /// FHD+
        /// 2436 x 1125
        /// </summary>
        FHD_Plus_2436_1125,
        /// <summary>
        /// UltraWide FHD
        /// 2560 x 1080
        /// </summary>
        UltraWide_FHD,
        /// <summary>
        /// QXGA (Quad XGA)
        /// 2048 x 1536
        /// </summary>
        QXGA,
        /// <summary>
        /// FHD+
        /// 2688 x 1242
        /// </summary>
        FHD_Plus_2688_1242,
        /// <summary>
        /// WQHD (Wide Quad-HD)
        /// 2560 x 1440
        /// </summary>
        WQHD,
        /// <summary>
        /// WQXGA
        /// 2560 x 1600
        /// </summary>
        WQXGA,
        /// <summary>
        /// Full Vision QHD
        /// 2880 x 1440
        /// </summary>
        Full_Vision_QHD,
        /// <summary>
        /// 2K Square
        /// 2048 x 2048
        /// </summary>
        _2K_Square,
        /// <summary>
        /// WQHD+
        /// 2960 x 1440
        /// </summary>
        WQHD_Plus,
        /// <summary>
        /// Pixel A5
        /// 2560 x 1800
        /// </summary>
        Pixel_A5,
        /// <summary>
        /// 3K
        /// 2880 x 1620
        /// </summary>
        _3K,
        /// <summary>
        /// Ultra-Wide QHD (UWQHD)
        /// 3440 x 1440
        /// </summary>
        UWQHD,
        /// <summary>
        /// Surface 12.3″
        /// 2736 x 1842
        /// </summary>
        Surface_12_3,
        /// <summary>
        /// 3K (QHD+)
        /// 3008 x 1692
        /// </summary>
        QHD_Plus,
        /// <summary>
        /// QWXGA+ (Quad WXGA+)
        /// 2880 x 1800
        /// </summary>
        QWXGA_Plus,
        /// <summary>
        /// QSXGA (Quad SXGA)
        /// 2560 x 2048
        /// </summary>
        QSXGA,
        /// <summary>
        /// iPad Pro 12.9″
        /// 2732 x 2048
        /// </summary>
        iPad_Pro_12_9,
        /// <summary>
        /// QHD+ (Quad HD+)／WQXGA+
        /// 3200 x 1800
        /// </summary>
        WQXGA_Plus,
        /// <summary>
        /// Surface 13.5″
        /// 3000 x 2000
        /// </summary>
        Surface_13_5,
        /// <summary>
        /// UltraWide QHD+
        /// 3840 x 1600
        /// </summary>
        UltraWide_QHD_Plus_3840_1600,
        /// <summary>
        /// UltraWide QHD+（4K HDR）
        /// 3840 x 1644
        /// </summary>
        UltraWide_QHD_Plus_3840_1644,
        /// <summary>
        /// QUXGA (Quad UXGA)
        /// 3200 x 2400
        /// </summary>
        QUXGA,
        /// <summary>
        /// 4K／QFHD (Quad Full-HD)／UHD 4K (2160p)
        /// 3840 x 2160
        /// </summary>
        _4K,
        /// <summary>
        /// DCI 4K
        /// 4096 x 2160
        /// </summary>
        DCI_4K,
        /// <summary>
        /// WQUXGA (Wide QUXGA)
        /// 3840 x 2400
        /// </summary>
        WQUXGA,
        /// <summary>
        /// iMac Retina 4K
        /// 4096 x 2304
        /// </summary>
        iMac_Retina_4K,
        /// <summary>
        /// DCI 4K+
        /// 4096 x 2560
        /// </summary>
        DCI_4K_Plus,
        /// <summary>
        /// 5K／UHD+
        /// 5120 x 2880
        /// </summary>
        _5K,
        /// <summary>
        /// 6K／XDR(Extreme Dynamic Range)
        /// 6016 x 3384
        /// </summary>
        _6K,
        /// <summary>
        /// 8K FUHD (4320p)／スーパーハイビジョン[
        /// 7680 x 4320
        /// </summary>
        _8K_FUHD,
        /// <summary>
        /// 10K
        /// 10240 x 4320
        /// </summary>
        _10K,
        /// <summary>
        /// 16K
        /// 15360 x 4320
        /// </summary>
        _16K_15360_4320,
        /// <summary>
        /// 16K
        /// 15360 x 8640
        /// </summary>
        _16K_15360_8640,
    }
}
