using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// TelnetClientLibraryクラス
    /// </summary>
    partial class TelnetClientLibrary
    {
        /// <summary>
        /// 常時読込
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TelnetClientException"></exception>
        public async Task AlwaysRead()
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibrary::AlwaysRead()");

            // 無限ループ
            while (true)
            {
                // キャンセル判定
                if (m_CancellationTokenSource.IsCancellationRequested)
                {
                    // ロギング
                    Logger.Warn("取り消し要求受信:[TelnetClientLibrary::AlwaysRead()]");

                    // キャンセル完了通知を設定
                    OnCancelCompletedNotify.Set();

                    // 無限ループキャンセル
                    break;
                }

                // 読込
                await AsyncRead();
            }

            // ロギング
            Logger.Debug("<<<<= TelnetClientLibrary::AlwaysRead()");
        }
    }
}
