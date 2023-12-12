using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// WindowLibraryクラス
    /// </summary>
    public static class WindowLibrary
    {
        #region Window関連
        // Windows API関数をインポートするための定義
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        const int SM_CYCAPTION = 4;

        /// <summary>
        /// タイトルバーの高さを取得する
        /// </summary>
        /// <returns></returns>
        public static int GetTitleBarHeight()
        {
            IntPtr hwnd = IntPtr.Zero; // ここにウィンドウのハンドルを設定
            // タイトルバーの高さを取得
            int titleBarHeight = GetSystemMetrics(SM_CYCAPTION);
            return titleBarHeight;
        }
        #endregion
    }
}
