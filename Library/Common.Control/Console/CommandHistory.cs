using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace Common.Control
{
    /// <summary>
    /// コマンド履歴クラス
    /// </summary>
    public class CommandHistory
    {
        /// <summary>
        /// 現在位置
        /// </summary>
        public int Position = -1;

        /// <summary>
        /// 最大数
        /// </summary>
        public int Capacity = 1000;

        /// <summary>
        /// リスト
        /// </summary>
        private List<string> m_List = new List<string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandHistory()
            : base()
        {

        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="command"></param>
        public void Add(string command)
        {
            // 最大数判定
            if (m_List.Count > Capacity - 1)
            {
                // 先頭削除
                m_List.RemoveAt(0);
            }

            // 追加
            m_List.Add(command);

            // 位置更新
            Position = m_List.Count;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public string Get(Keys keyCode)
        {
            // 結果
            string result = string.Empty;

            // キーコードで分岐する
            switch (keyCode)
            {
                case Keys.Up:
                    result = GetUpHisotry();
                    break;
                case Keys.Down:
                    result = GetDownHisotry();
                    break;
                default:
                    // 登録なし
                    break;
            }

            // 返却
            return result;
        }

        private string GetUpHisotry()
        {
            if (m_List.Count == 0)
            {
                return string.Empty;
            }

            Position--;

            if (Position < 0)
            {
                Position = -1;
                return m_List[0];
            }

            if (!((Position >= 0) && (Position < m_List.Count)))
            {
                return string.Empty;
            }

            string result = m_List[Position];

            return result;
        }

        private string GetDownHisotry()
        {
            if (m_List.Count == 0)
            {
                return string.Empty;
            }

            Position++;

            if (Position >= m_List.Count)
            {
                Position = m_List.Count;
            }

            if (!((Position >= 0) && (Position < m_List.Count)))
            {
                return string.Empty;
            }

            string result = m_List[Position];

            return result;
        }
    }
}