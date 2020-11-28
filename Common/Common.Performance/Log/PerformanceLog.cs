using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;

namespace Common.Performance
{
    public class PerformanceLog
    {
        /// <summary>
        /// ログライター
        /// </summary>
        private StreamWriter m_StreamWriter = null;
        /// <summary>
        /// ログキュー
        /// </summary>
        Queue<String> m_LogQueue = null;
        /// <summary>
        /// インターバルタイマー
        /// </summary>
        int m_Interval = 100;

        Object m_SyncObject = new Object();
        /// <summary>
        /// 
        /// </summary>
        private volatile bool m_ShouldStop = false;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pStreamWriter">StreamWriterオブジェクト</param>
        /// <param name="pCapacity">ログ容量</param>
        /// <param name="pInterval">インターバルタイマー</param>
        public PerformanceLog(StreamWriter pStreamWriter, int pCapacity, int pInterval)
        {
            Debug.WriteLine("=>>>> PerformanceLog::PerformanceLog(" + pStreamWriter.ToString() + ", " + pCapacity.ToString() + ", " + pInterval.ToString() + ")");

            // 初期設定
            m_StreamWriter = pStreamWriter;
            m_LogQueue = new Queue<String>(pCapacity);
            m_Interval = pInterval;

            // 初期化
            Initialization();

            Debug.WriteLine("<<<<= PerformanceLog::PerformanceLog()");
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~PerformanceLog()
        {
            Debug.WriteLine("=>>>> PerformanceLog::~PerformanceLog()");

            // 破棄
            Destruction();
            Debug.WriteLine("<<<<= PerformanceLog::~PerformanceLog()");
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialization()
        {
            Debug.WriteLine("=>>>> PerformanceLog::Initialization()");
            Debug.WriteLine("<<<<= PerformanceLog::Initialization()");
        }
        /// <summary>
        /// 破棄
        /// </summary>
        private void Destruction()
        {
            Debug.WriteLine("=>>>> PerformanceLog::Destruction()");
            if (m_StreamWriter != null)
            {
                try
                {
                    LogWrite();
                    m_StreamWriter.Flush();
                    m_StreamWriter.Dispose();
                    m_StreamWriter.Close();
                    m_StreamWriter = null;
                }
                catch (ObjectDisposedException ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
            Debug.WriteLine("<<<<= PerformanceLog::Destruction()");
        }
        static public StreamWriter GetInstance(String path, String basename, String dateformat, bool append)
        {
            String _path = path + @"\";
            _path += DateTime.Now.ToString(dateformat) + "_" + basename + ".log";

            return PerformanceLog.GetInstance(_path, append);
        }
        static public StreamWriter GetInstance(String path, String basename, bool append)
        {
            return PerformanceLog.GetInstance(path, basename, "yyyyMMddHHmmss", append);
        }
        static public StreamWriter GetInstance(String path, bool append)
        {
            return new StreamWriter(path, append);
        }
        public void DoWork()
        {
            Debug.WriteLine("=>>>> PerformanceLog::DoWork()");
            while (m_ShouldStop == false)
            {
                Debug.WriteLine("====> PerformanceLog::DoWork() - m_ShouldStop = " + m_ShouldStop.ToString());
                Debug.WriteLine("====> PerformanceLog::DoWork() - Thread.Sleep(" + m_Interval.ToString() + ");");
                Thread.Sleep(m_Interval);
                if (m_StreamWriter == null)
                {
                    break;
                }
                LogWrite();
            }
            Debug.WriteLine("<<<<= PerformanceLog::DoWork()");
        }
        private void LogWrite()
        {
            lock (m_SyncObject)
            {
                for (int i = 0; i < m_LogQueue.Count; i++)
                {
                    m_StreamWriter.WriteLine(m_LogQueue.Dequeue());
                    m_StreamWriter.Flush();
                }
            }
        }
        public void Add(ArrayList pValueList)
        {
            Debug.WriteLine("=>>>> PerformanceLog::Add()");
            if (m_ShouldStop == true)
            {
                return;
            }
            String _LogLine = String.Empty;
            _LogLine += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            _LogLine += ",";

            foreach (float _Value in pValueList)
            {
                _LogLine += String.Format("{0:0.000}", _Value);
                _LogLine += ",";
            }
            m_LogQueue.Enqueue(_LogLine);
            Debug.WriteLine("<<<<= PerformanceLog::Add()");
        }
        /// <summary>
        /// 開始
        /// </summary>
        public void Start()
        {
            Debug.WriteLine("=>>>> PerformanceLog::Start()");
            m_ShouldStop = false;
            Debug.WriteLine("<<<<= PerformanceLog::Start()");
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            Debug.WriteLine("=>>>> PerformanceLog::Stop()");
            m_ShouldStop = true;
            Debug.WriteLine("<<<<= PerformanceLog::Stop()");
        }
    }
}