using System;
using System.Text;

namespace Common.Control
{
    public class CommandDetectionEventArgs : EventArgs
    {
        /// <summary>
        /// 検出時間
        /// </summary>
        public DateTime DetectionTime { get; set; } = DateTime.Now;

        /// <summary>
        /// コマンド
        /// </summary>
        public string Command { get; set; } = string.Empty;

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("【実行コマンド検出】");
            result.AppendLine(string.Format("実行コマンド:{0}", Command));

            return result.ToString();
        }
    }
}