using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Common.Net
{
    #region ネゴシエーション情報クラス
    /// <summary>
    /// ネゴシエーション情報クラス
    /// </summary>
    public class NegotiationInfomation
    {
        public TelnetCommand IAC = TelnetCommand.IAC;
        public TelnetCommand Command = TelnetCommand.UN;
        public TelnetOption Option = TelnetOption.binary;
        public MemoryStream Stream = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NegotiationInfomation()
        {
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="info"></param>
        public NegotiationInfomation(NegotiationInfomation info)
        {
            this.IAC = info.IAC;
            this.Command = info.Command;
            this.Option = info.Option;
            this.Stream = info.Stream;
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("IAC ");
            stringBuilder.Append(this.Command.ToString() + " ");
            if (this.Command != TelnetCommand.SE)
            {
                stringBuilder.Append(this.Option.ToString() + " ");
                if (this.Stream != null)
                {
                    foreach (byte data in this.Stream.ToArray())
                    {
                        stringBuilder.Append(string.Format("{0,2:x2} ", data));
                    }
                }
            }

            return stringBuilder.ToString();
        }
    };
    #endregion
}
