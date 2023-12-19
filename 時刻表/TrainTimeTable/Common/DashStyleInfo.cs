using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    public class DashStyleInfo
    {
        /// <summary>
        /// 線種スタイル
        /// </summary>
        private DashStyle m_DashStyle = DashStyle.Solid;

        /// <summary>
        /// 線種スタイル
        /// </summary>
        public DashStyle Style { get {  return m_DashStyle; } }

        public DashStyleInfo(DashStyle style)
        {
            m_DashStyle = style;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            switch(m_DashStyle)
            {
                case DashStyle.Dash:
                    result.Append("破線");
                    break;
                case DashStyle.Dot:
                    result.Append("点線");
                    break;
                case DashStyle.DashDot:
                    result.Append("一点鎖線");
                    break;
                case DashStyle.DashDotDot:
                    result.Append("二点鎖線");
                    break;
                default:
                    result.Append("実線");
                    break;
            }
            return result.ToString();
        }
    }
}
