using log4net;
using PCSC.Monitoring;
using PCSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PCSC.Exceptions;
using PCSC.Iso7816;
using System.IO;

namespace Common.Felica
{
    /// <summary>
    /// FelicaMonitoringクラス
    /// </summary>
    /// <see cref="https://github.com/sakapon/felicalib-remodeled"/>
    /// <see cref="https://github.com/danm-de/pcsc-sharp"/>
    public class FelicaMonitoring : IDisposable
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// IDeviceMonitorFactoryオブジェクト
        /// </summary>
        private IDeviceMonitorFactory m_DeviceMonitorFactory = null;

        /// <summary>
        /// IDeviceMonitorオブジェクト
        /// </summary>
        private IDeviceMonitor m_DeviceMonitor = null;

        /// <summary>
        /// ISCardContextオブジェクト
        /// </summary>
        private ISCardContext m_CardContext = null;

        /// <summary>
        /// ISCardMonitorオブジェクト
        /// </summary>
        private ISCardMonitor m_CardMonitor = null;

        /// <summary>
        /// カードリーダー名
        /// </summary>
        public string CardReaderName { get; set; } = string.Empty;

        /// <summary>
        /// リーダー名
        /// </summary>
        public List<string> Readers { get; private set; } = new List<string>();

        #region event delegate
        // Declare the delegate (if using non-generic pattern).
        public delegate void DeviceChangeEventHandler(object sender, DeviceChangeEventArgs e);
        public delegate void StatusChangeEventHandler(object sender, StatusChangeEventArgs e);
        public delegate void CardInsertedEventHandler(object sencer, CardEventArgs e);
        public delegate void CardRemovedEventHandler(object sencer, CardEventArgs e);
        #endregion

        #region event
        public DeviceChangeEventHandler OnDeviceChange = delegate { };
        public StatusChangeEventHandler OnStatusChange = delegate { };
        public CardInsertedEventHandler OnCardInserted = delegate { };
        public CardRemovedEventHandler OnCardRemoved = delegate { };
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public FelicaMonitoring()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::FelicaMonitoring()");

            // Deviceモニター開始
            StartDeviceMonitor();
            StartCardContext();

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::FelicaMonitoring()");
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~FelicaMonitoring()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::~FelicaMonitoring()");

            // Dispose
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::~FelicaMonitoring()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::Dispose()");

            // Dispose
            Dispose(true);
            GC.SuppressFinalize(this); // ファイナライザが不要なことをGCに伝える

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::Dispose(bool)");
            Logger.DebugFormat("disposing:[{0}]", disposing);

            // disposingはIDisposable.Disposeなのかファイナライザーなのかを判断する
            if (disposing)
            {
                // モニター停止
                StopCardMonitor();
                StopCardContext();
                StopDeviceMonitor();
            }

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::Dispose(bool)");
        }

        #region モニター開始
        /// <summary>
        /// モニター開始
        /// </summary>
        public void StartDeviceMonitor()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::StartDeviceMonitor()");

            // IDeviceMonitorFactoryオブジェクト設定
            m_DeviceMonitorFactory = DeviceMonitorFactory.Instance;

            // DeviceMonitorFactoryオブジェクト取得
            m_DeviceMonitor = m_DeviceMonitorFactory.Create(SCardScope.System);

            // イベント登録
            m_DeviceMonitor.StatusChanged += DeviceMonitor_StatusChanged;

            // 監視開始
            m_DeviceMonitor.Start();

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::StartDeviceMonitor()");
        }

        /// <summary>
        /// モニター開始
        /// </summary>
        public void StartCardContext()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::StartCardContext()");

            // ISCardContextオブジェクト取得
            m_CardContext = ContextFactory.Instance.Establish(SCardScope.System);

            // リーダーの機器情報を取得
            Readers = m_CardContext.GetReaders().ToList();
            if (NoReaderFound(Readers))
            {
                // ロギング
                Logger.WarnFormat("スマートカードリーダが存在していません:[{0}]", Environment.MachineName);

                // 処理しない
                return;
            }
            ShowRfidReader(Readers);

            // 先頭のカードリーダー名設定(暫定)
            CardReaderName = Readers[0];

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::StartCardContext()");
        }

        /// <summary>
        /// モニター開始
        /// </summary>
        public void StartCardMonitor()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::StartCardMonitor()");

            // カードリーダ名判定
            if (CardReaderName == string.Empty)
            {
                // 例外
                throw new NoServiceException(SCardError.NoReadersAvailable);
            }

            // ISCardMonitorオブジェクト取得
            m_CardMonitor = MonitorFactory.Instance.Create(SCardScope.System);

            // イベント登録
            m_CardMonitor.CardInserted += CardMonitor_CardInserted;
            m_CardMonitor.CardRemoved += CardMonitor_CardRemoved;
            m_CardMonitor.StatusChanged += CardMonitor_StatusChanged;

            // 開始
            m_CardMonitor.Start(CardReaderName);

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::StartCardMonitor()");
        }
        #endregion

        #region モニター停止
        /// <summary>
        /// モニター停止
        /// </summary>
        public void StopDeviceMonitor()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::StopDeviceMonitor()");

            if (m_DeviceMonitor != null)
            {
                m_DeviceMonitor.Cancel();
                m_DeviceMonitor.Dispose();
                m_DeviceMonitor = null;
            }

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::StopDeviceMonitor()");
        }

        /// <summary>
        /// モニター停止
        /// </summary>
        public void StopCardContext()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::StopCardContext()");

            if (m_CardContext != null)
            {
                m_CardContext.Cancel();
                m_CardContext.Dispose();
                m_CardContext = null;
            }

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::StopCardContext()");
        }

        /// <summary>
        /// モニター停止
        /// </summary>
        public void StopCardMonitor()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::StopCardMonitor()");

            if (m_CardMonitor != null)
            {
                m_CardMonitor.Cancel();
                m_CardMonitor.Dispose();
                m_CardMonitor = null;
            }

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::StopCardMonitor()");
        }
        #endregion

        /// <summary>
        /// カードリーダー表示
        /// </summary>
        /// <param name="readerNames"></param>
        private void ShowRfidReader(IList<string> readerNames)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::ShowRfidReader(IList<string>)");
            Logger.Debug("Available readers: ");

            // Show available readers.
            for (var i = 0; i < readerNames.Count; i++)
            {
                // ロギング
                Logger.Debug($"└[{i}] {readerNames[i]}");
            }

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::ShowRfidReader(IList<string>)");
        }

        /// <summary>
        /// カードリーダー判定
        /// </summary>
        /// <param name="readerNames"></param>
        /// <returns></returns>
        public bool NoReaderFound(ICollection<string> readerNames)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::NoReaderFound(ICollection<string>)");
            Logger.DebugFormat("readerNames:[{0}]", readerNames.Count);

            if (readerNames == null || readerNames.Count < 1)
            {
                // ロギング
                Logger.Debug("カードリーダー:[なし]");
                Logger.Debug("<<<<= FelicaMonitoring::NoReaderFound(ICollection<string>)");

                // カードリーダーなし
                return true;
            }

            // ロギング
            Logger.Debug("カードリーダー:[あり]");
            Logger.Debug("<<<<= FelicaMonitoring::NoReaderFound(ICollection<string>)");

            // カードリーダーあり
            return false;
        }

        /// <summary>
        /// CardMonitor - CardInserted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardMonitor_CardInserted(object sender, CardEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::CardMonitor_CardInserted(object, CardEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender.ToString());
            Logger.DebugFormat("e     :[{0}]", e.ToString());

            // イベント通知
            OnCardInserted.Invoke(this, e);

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::CardMonitor_CardInserted(object, CardEventArgs)");
        }

        /// <summary>
        /// CardMonitor - CardRemoved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardMonitor_CardRemoved(object sender, CardEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::CardMonitor_CardRemoved(object, CardEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender.ToString());
            Logger.DebugFormat("e     :[{0}]", e.ToString());

            // イベント通知
            OnCardRemoved.Invoke(this, e);

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::CardMonitor_CardRemoved(object, CardEventArgs)");
        }

        /// <summary>
        /// DeviceMonitor - StatusChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceMonitor_StatusChanged(object sender, DeviceChangeEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::DeviceMonitor_StatusChanged(object, DeviceChangeEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender.ToString());
            Logger.DebugFormat("e     :[{0}]", e.ToString());

            // リーダーの機器情報を取得
            if (e.AllReaders.ToList().Count > 0)
            {
                // モニター開始
                StartCardContext();

                // リスト取得
                Readers = m_CardContext.GetReaders().ToList();
            }
            else
            {
                // モニター停止
                StopCardContext();

                // リストクリア
                Readers.Clear();
            }

            // ロギング
            Logger.DebugFormat("Readers:[{0}]", Readers.Count);

            // TODO:未実装(監視再開)

            // イベント通知
            OnDeviceChange.Invoke(this, e);

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::DeviceMonitor_StatusChanged(object, DeviceChangeEventArgs)");
        }

        /// <summary>
        /// CardMonitor - StatusChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CardMonitor_StatusChanged(object sender, StatusChangeEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::CardMonitor_StatusChanged(object, StatusChangeEventArgs)");
            Logger.DebugFormat("sender:[{0}]", sender.ToString());
            Logger.DebugFormat("e     :[{0}]", e.ToString());

            // TODO:未実装

            // イベント通知
            OnStatusChange.Invoke(this, e);

            // ロギング
            Logger.Debug("<<<<= FelicaMonitoring::CardMonitor_StatusChanged(object, StatusChangeEventArgs)");
        }

        /// <summary>
        /// カードのIDを読み取る
        /// </summary>
        /// <returns></returns>
        public string ReadCardId()
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::ReadCardId()");

            // 初期化
            string result = string.Empty;

            // カードリーダ名判定
            if (CardReaderName == string.Empty)
            {
                // ロギング
                Logger.WarnFormat("カードリーダ名が設定されていません");

                // ロギング
                Logger.DebugFormat("result:[{0}]", result);
                Logger.Debug("<<<<= FelicaMonitoring::ReadCardId()");

                // 返却
                return result;
            }

            // カードID読み取り
            result = ReadCardId(CardReaderName);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FelicaMonitoring::ReadCardId()");

            // 返却
            return result;
        }

        /// <summary>
        /// カードのIDを読み取る
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ReadCardId(string name)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::ReadCardId(string)");
            Logger.DebugFormat("name:[{0}]", name);

            // カードID読み取り
            string result = ReadCardId(name, SCardShareMode.Shared, SCardProtocol.Any);

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FelicaMonitoring::ReadCardId(string)");

            // 返却
            return result;
        }

        /// <summary>
        /// カードのIDを読み取る
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public string ReadCardId(string name, SCardShareMode mode, SCardProtocol protocol)
        {
            // ロギング
            Logger.Debug("=>>>> FelicaMonitoring::ReadCardId(string, SCardShareMode, SCardProtocol)");
            Logger.DebugFormat("name    :[{0}]", name);
            Logger.DebugFormat("mode    :[{0}]", mode);
            Logger.DebugFormat("protocol:[{0}]", protocol);

            // 初期化
            string result = string.Empty;

            try
            {
                // ICardReaderオブジェクト取得
                using (var reader = m_CardContext.ConnectReader(name, SCardShareMode.Shared, SCardProtocol.Any))
                {
                    // APDUコマンドの作成
                    var apdu = new CommandApdu(IsoCase.Case2Short, reader.Protocol)
                    {
                        CLA = 0xFF,
                        Instruction = InstructionCode.GetData,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0 // We don't know the ID tag size
                    };

                    // 読み取りコマンド送信
                    using (reader.Transaction(SCardReaderDisposition.Leave))
                    {
                        var sendPci = SCardPCI.GetPci(reader.Protocol);
                        var receivePci = new SCardPCI(); // IO returned protocol control information.

                        var receiveBuffer = new byte[256];
                        var command = apdu.ToArray();

                        var bytesReceived = reader.Transmit(
                            sendPci, // Protocol Control Information (T0, T1 or Raw)
                            command, // command APDU
                            command.Length,
                            receivePci, // returning Protocol Control Information
                            receiveBuffer,
                            receiveBuffer.Length); // data buffer

                        var responseApdu = new ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, reader.Protocol);
                        if (responseApdu.HasData)
                        {
                            // バイナリ文字列の整形
                            StringBuilder id = new StringBuilder(BitConverter.ToString(responseApdu.GetData()));
                            result = id.ToString();
                        }
                        else
                        {
                            // ロギング
                            Logger.WarnFormat("このカードではIDを取得できません:[{0}]", CardReaderName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ロギング
                Logger.Warn(ex.Message);
            }

            // ロギング
            Logger.DebugFormat("result:[{0}]", result);
            Logger.Debug("<<<<= FelicaMonitoring::ReadCardId(string, SCardShareMode, SCardProtocol)");

            // 返却
            return result;
        }
    }
}
