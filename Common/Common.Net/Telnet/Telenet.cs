using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace Common.Net
{
    /// <summary>
    /// Telnetクラス
    /// </summary>
    public class Telenet : NetworkVirtualTerminal
    {
        private StringBuilder m_RecvString = new StringBuilder();
        private ManualResetEvent OnWaitStringNotify = new ManualResetEvent(false);

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public Telenet(string host)
            : base(host)
        {
            // 初期化
            this.Initialization();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public Telenet(string host, int port)
            : base(host, port)
        {
            // 初期化
            this.Initialization();
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            this.OnRead += ReadEventHandler; 
        }
        #endregion

        public void Write(string str)
        {
            MemoryStream sendStream = new MemoryStream();

            // エンコード
            byte[] data = this.RemoteEncoding.GetBytes(str);

            sendStream.Write(data, 0, data.Length);


            this.Send(sendStream);
        }

        public void WriteLine(string str)
        {
            this.Write(str);
            this.Write("\n");
        }

        public void ReadEventHandler(object sender, NetworkVirtualTerminalReadEventArgs e)
        {
            Console.WriteLine("《文字列受信》" + e.ReadStringBuilder.ToString());
            this.m_RecvString.Append(e.ReadStringBuilder.ToString());
            this.OnWaitStringNotify.Set();
        }

        public StringBuilder Read(string str, int timeout)
        {
            StringBuilder result = new StringBuilder();
            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(timeout);

            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    source.Token.ThrowIfCancellationRequested();
                    if (!this.OnWaitStringNotify.WaitOne())
                    {
                        // TODO:例外
                        this.OnWaitStringNotify.Reset();
                        break;
                    }
                    this.OnWaitStringNotify.Reset();

                    // 文字列比較
                    Regex regex = new Regex(str, RegexOptions.Compiled | RegexOptions.Multiline);
                    if (regex.IsMatch(this.m_RecvString.ToString()))
                    {
                        result.Append(this.m_RecvString);
                        this.m_RecvString.Length = 0;
                        this.m_RecvString.Clear();
                        break;
                    }
                }
                return;
            }, source.Token);

            try
            {
                t.Wait(source.Token);//OperationCanceledExceptionが発生します。
                                     //t.Wait();//AggregateExceptionが発生します。
                Console.WriteLine("＜タスク終了＞");
                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("OperationCanceledExceptionが発生しました。");
                return null;
            }
            catch (AggregateException)
            {
                Console.WriteLine("AggregateExceptionが発生しました。");
                return null;
            }
        }
    }
}
