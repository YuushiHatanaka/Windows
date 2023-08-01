using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// 動作モード
    /// </summary>
    public enum JsonMode
    {
        Non = 0,                    // 設定なし
        Object,                     // Objectモード
        Array,                      // Arrayモード
    };
}
