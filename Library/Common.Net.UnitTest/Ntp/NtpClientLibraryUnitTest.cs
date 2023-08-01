using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Common.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.UnitTest
{
    [TestClass]
    public class NtpClientLibraryUnitTest
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [TestMethod]
        public void Excecute()
        {
            using (NtpClientLibrary ntpClientLibrary = new NtpClientLibrary("186.32.1.201"))
            {
                ntpClientLibrary.Connect();
                NtpPacket result = ntpClientLibrary.Excexute();
                ntpClientLibrary.Disconnect();
            }
        }

        [TestMethod]
        public async Task AsycExcecute()
        {
            using (NtpClientLibrary ntpClientLibrary = new NtpClientLibrary("186.32.1.201"))
            {
                ntpClientLibrary.OnConnected += OnConnected;
                ntpClientLibrary.OnDisconnected += OnDisconnected;
                ntpClientLibrary.OnCommandExecute += OnCommandExecute;
                await ntpClientLibrary.AsyncConnect();
                await ntpClientLibrary.AsyncExcexute();
                await ntpClientLibrary.AsyncDisconnect();
            }
        }

        private void OnConnected(object sender, UdpClientConnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibraryUnitTest::OnConnected(object, UdpClientConnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= NtpClientLibraryUnitTest::OnConnected(object, UdpClientConnectedEventArgs)");
        }
        private void OnDisconnected(object sender, UdpClientDisconnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibraryUnitTest::OnDisconnected(object, UdpClientDisconnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= NtpClientLibraryUnitTest::OnDisconnected(object, UdpClientDisconnectedEventArgs)");
        }

        private void OnCommandExecute(object sender, NtpClientCommandExecuteEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> NtpClientLibraryUnitTest::OnCommandExecute(object, NtpClientCommandExecuteEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= NtpClientLibraryUnitTest::OnCommandExecute(object, NtpClientCommandExecuteEventArgs)");
        }
    }
}
