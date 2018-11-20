using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Common.Windows.Forms
{
    /// <summary>
    /// コントロールマージン
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ControlMargin: Attribute
    {
        /// <summary>
        /// マージン(Top)
        /// </summary>
        private int m_Top;
        /// <summary>
        /// マージン(Bottom)
        /// </summary>
        private int m_Bottom;
        /// <summary>
        /// マージン(Right)
        /// </summary>
        private int m_Right;
        /// <summary>
        /// マージン(Left)
        /// </summary>
        private int m_Left;

        /// <summary>
        /// マージン(Top)
        /// </summary>
        public int Top { get { return this.m_Top; } set { this.m_Top = value; } }
        /// <summary>
        /// マージン(Bottom)
        /// </summary>
        public int Bottom { get { return this.m_Bottom; } set { this.m_Bottom = value; } }
        /// <summary>
        /// マージン(Right)
        /// </summary>
        public int Right { get { return this.m_Right; } set { this.m_Right = value; } }
        /// <summary>
        /// マージン(Left)
        /// </summary>
        public int Left { get { return this.m_Left; } set { this.m_Left = value; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlMargin()
        {
            // 初期化
            this.Initialization(0, 0, 0, 0);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="right"></param>
        /// <param name="left"></param>
        public ControlMargin(int top, int bottom, int right, int left)
        {
            // 初期化
            this.Initialization(top, bottom, right, left);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="right"></param>
        /// <param name="left"></param>
        private void Initialization(int top,int bottom, int right,int left)
        {
            this.m_Top = top;
            this.m_Bottom = bottom;
            this.m_Right = right;
            this.m_Left = left;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}",
                this.Bottom, this.Left, this.Right, this.Top);
        }
    }
}
