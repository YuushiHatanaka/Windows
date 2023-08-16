using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Diagnostics;
using System.Threading;
using System.Diagnostics.Tracing;

namespace Common.Net.UnitTest
{
    [TestClass]
    public class TelnetClientLibraryUnitTest
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        [TestMethod]
        public void Excecute()
        {
            using (TelnetClientLibrary telnetClientLibrary = new TelnetClientLibrary("186.32.1.101"))
            {
                telnetClientLibrary.UserInfo.Name = "root";
                telnetClientLibrary.UserInfo.Password = "Macross7";
                telnetClientLibrary.ServerInfo.Prompt = "]# ";
                telnetClientLibrary.ServerInfo.Encoding = Encoding.GetEncoding("euc-jp");
                telnetClientLibrary.Connect();
                telnetClientLibrary.Login();
                telnetClientLibrary.Excexute("ls -ltra");
                telnetClientLibrary.Logout();
                telnetClientLibrary.Disconnect();
            }
        }

        [TestMethod]
        public async Task AsycExcecute()
        {
            using (TelnetClientLibrary telnetClientLibrary = new TelnetClientLibrary("186.32.1.101"))
            {
                telnetClientLibrary.UserInfo.Name = "root";
                telnetClientLibrary.UserInfo.Password = "Macross7";
                telnetClientLibrary.ServerInfo.Prompt = "]# ";
                telnetClientLibrary.ServerInfo.Encoding = Encoding.GetEncoding("euc-jp");
                telnetClientLibrary.OnConnected += OnConnected;
                telnetClientLibrary.OnLogined += OnLogined;
                telnetClientLibrary.OnLogouted += OnLogouted;
                telnetClientLibrary.OnDisconnected += OnDisconnected;
                telnetClientLibrary.OnCommandExecute += OnCommandExecute;
                await telnetClientLibrary.AsyncConnect();
                await telnetClientLibrary.AsyncLogin();
                await telnetClientLibrary.AsyncExcexute("ls -ltra");
                await telnetClientLibrary.AsyncLogout();
                await telnetClientLibrary.AsyncDisconnect();
            }
        }

        [TestMethod]
        public async Task AlwaysConnected()
        {
            TelnetClientLibrary telnetClientLibrary = new TelnetClientLibrary("186.32.1.101");
            telnetClientLibrary.UserInfo.Name = "root";
            telnetClientLibrary.UserInfo.Password = "Macross7";
            telnetClientLibrary.ServerInfo.Prompt = "]# ";
            telnetClientLibrary.ServerInfo.Encoding = Encoding.GetEncoding("euc-jp");
            telnetClientLibrary.OnConnected += OnConnected;
            telnetClientLibrary.OnLogined += OnLogined;
            telnetClientLibrary.OnLogouted += OnLogouted;
            telnetClientLibrary.OnDisconnected += OnDisconnected;
            telnetClientLibrary.OnCommandExecute += OnCommandExecute;
            telnetClientLibrary.OnRecive += OnRecive;
            telnetClientLibrary.OnCanceled += OnCanceled;

            // Task開始
            Task task = Task.Run(async () =>
            {
                await telnetClientLibrary.AsyncConnect();
                await telnetClientLibrary.AsyncLogin();
                await telnetClientLibrary.AsyncWriteLine("top");
                await telnetClientLibrary.AlwaysRead();
            });

            await Task.Delay(5000);
            await telnetClientLibrary.AsyncCancel();
            if (!telnetClientLibrary.OnCancelCompletedNotify.WaitOne(10000))
            {
                // ロギング
                Logger.Error("OnCancelCompletedNotify Timeout");
            }
            telnetClientLibrary.Reset();
            telnetClientLibrary.Excexute("\x03"); /* CTRL+C送信 */
            telnetClientLibrary.Excexute("ls -ltra");
            telnetClientLibrary.Dispose();
        }


        private void OnConnected(object sender, TcpClientConnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibraryUnitTest::OnConnected(object, TcpClientConnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= TelnetClientLibraryUnitTest::OnConnected(object, TcpClientConnectedEventArgs)");
        }

        private void OnLogined(object sender, TelnetClientLoginEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibraryUnitTest::OnLogined(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= TelnetClientLibraryUnitTest::OnLogined(object, TelnetClientLoginEventArgs)");
        }

        private void OnLogouted(object sender, TelnetClientLogoutEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibraryUnitTest::OnLogouted(object, TelnetClientLogoutEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= TelnetClientLibraryUnitTest::OnLogouted(object, TelnetClientLogoutEventArgs)");
        }

        private void OnDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibraryUnitTest::OnDisconnected(object, TcpClientDisconnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= TelnetClientLibraryUnitTest::OnDisconnected(object, TcpClientDisconnectedEventArgs)");
        }

        private void OnCommandExecute(object sender, TelnetClientCommandExecuteEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibraryUnitTest::OnCommandExecute(object, TelnetClientCommandExecuteEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= TelnetClientLibraryUnitTest::OnCommandExecute(object, TelnetClientCommandExecuteEventArgs)");
        }

        private void OnRecive(object sender, TcpClientReciveEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibraryUnitTest::OnRecive(object, SshClientCommandExecuteEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= TelnetClientLibraryUnitTest::OnRecive(object, SshClientCommandExecuteEventArgs)");
        }

        private void OnCanceled(object sender, TcpClientCanceledEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> TelnetClientLibraryUnitTest::OnCanceled(object, TcpClientCanceledEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= TelnetClientLibraryUnitTest::OnCanceled(object, TcpClientCanceledEventArgs)");
        }
    }
}
