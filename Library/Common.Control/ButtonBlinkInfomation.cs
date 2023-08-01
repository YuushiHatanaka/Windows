using System.Windows.Forms;

namespace Common.Control
{
    /// <summary>
    /// 点滅情報クラス
    /// </summary>
    public class ButtonBlinkInfomation : BlinkInfomation
    {
        /// <summary>
        /// 外観指定
        /// </summary>
        public BlinkAppearance FlatAppearance = new BlinkAppearance();

        /// <summary>
        /// 描写スタイル
        /// </summary>
        public FlatStyle FlatStyle = FlatStyle.Standard;

        /// <summary>
        /// Visial Style使用フラグ
        /// </summary>
        public bool UseVisualStyleBackColor = true;
    }
}
