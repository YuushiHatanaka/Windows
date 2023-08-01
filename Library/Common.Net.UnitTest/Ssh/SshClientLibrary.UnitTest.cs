using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using Common.Net;
using log4net;
using System.Reflection;
using System.Threading.Tasks;
using Common.Diagnostics;
using System.Threading;

namespace Common.Net.UnitTest
{
    [TestClass]
    public class SshClientLibraryUnitTest
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
            using (SshClientLibrary sshClientLibrary = new SshClientLibrary("186.32.1.100"))
            {
                sshClientLibrary.UserInfo.Name = "root";
                sshClientLibrary.UserInfo.Password = "Macross7";
                sshClientLibrary.ServerInfo.Prompt = "]# ";
                sshClientLibrary.ServerInfo.Encoding = Encoding.GetEncoding("euc-jp");
                sshClientLibrary.Connect();
                sshClientLibrary.Login();
                sshClientLibrary.Excexute("ls -ltra");
                sshClientLibrary.Logout();
                sshClientLibrary.Disconnect();
            }
        }

        [TestMethod]
        public async Task AsycExcecute()
        {
            using (SshClientLibrary sshClientLibrary = new SshClientLibrary("186.32.1.100"))
            {
                sshClientLibrary.UserInfo.Name = "root";
                sshClientLibrary.UserInfo.Password = "Macross7";
                sshClientLibrary.ServerInfo.Prompt = "]# ";
                sshClientLibrary.ServerInfo.Encoding = Encoding.GetEncoding("euc-jp");
                sshClientLibrary.OnConnected += OnConnected;
                sshClientLibrary.OnLogined += OnLogined;
                sshClientLibrary.OnLogouted += OnLogouted;
                sshClientLibrary.OnDisconnected += OnDisconnected;
                sshClientLibrary.OnCommandExecute += OnCommandExecute;
                await sshClientLibrary.AsyncConnect();
                await sshClientLibrary.AsyncLogin();
                await sshClientLibrary.AsyncExcexute("ls -ltra");
                await sshClientLibrary.AsyncLogout();
                await sshClientLibrary.AsyncDisconnect();
            }
        }

        [TestMethod]
        public async Task AlwaysConnected()
        {
            SshClientLibrary sshClientLibrary = new SshClientLibrary("186.32.1.101");
            sshClientLibrary.UserInfo.Name = "root";
            sshClientLibrary.UserInfo.Password = "Macross7";
            sshClientLibrary.ServerInfo.Prompt = "]# ";
            sshClientLibrary.ServerInfo.Encoding = Encoding.GetEncoding("euc-jp");
            sshClientLibrary.OnConnected += OnConnected;
            sshClientLibrary.OnLogined += OnLogined;
            sshClientLibrary.OnLogouted += OnLogouted;
            sshClientLibrary.OnDisconnected += OnDisconnected;
            sshClientLibrary.OnCommandExecute += OnCommandExecute;
            sshClientLibrary.OnRecive += OnRecive;
            sshClientLibrary.OnCanceled += OnCanceled;

            // Task開始
            Task task = Task.Run(async () =>
            {
                await sshClientLibrary.AsyncConnect();
                await sshClientLibrary.AsyncLogin();
                await sshClientLibrary.AsyncWriteLine("top");
                await sshClientLibrary.AlwaysRead();
            });

            await Task.Delay(5000);
            await sshClientLibrary.AsyncCancel();
            if (!sshClientLibrary.OnCancelCompletedNotify.WaitOne(5000))
            {
                // ロギング
                Logger.Error("OnCancelCompletedNotify Timeout");
            }
            sshClientLibrary.Reset();
            sshClientLibrary.Excexute("\x03"); /* CTRL+C送信 */
            sshClientLibrary.Excexute("ls -ltra");
            sshClientLibrary.Dispose();
        }

        private void OnConnected(object sender, TcpClientConnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibraryUnitTest::OnConnected(object, TcpClientConnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= SshClientLibraryUnitTest::OnConnected(object, TcpClientConnectedEventArgs)");
        }

        private void OnLogined(object sender, SshClientLoginEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibraryUnitTest::OnLogined(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= SshClientLibraryUnitTest::OnLogined(object, TelnetClientLoginEventArgs)");
        }

        private void OnLogouted(object sender, SshClientLogoutEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibraryUnitTest::OnLogouted(object, TelnetClientLogoutEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= SshClientLibraryUnitTest::OnLogouted(object, TelnetClientLogoutEventArgs)");
        }

        private void OnDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibraryUnitTest::OnDisconnected(object, TcpClientDisconnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= SshClientLibraryUnitTest::OnDisconnected(object, TcpClientDisconnectedEventArgs)");
        }

        private void OnCommandExecute(object sender, SshClientCommandExecuteEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibraryUnitTest::OnCommandExecute(object, SshClientCommandExecuteEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= SshClientLibraryUnitTest::OnCommandExecute(object, SshClientCommandExecuteEventArgs)");
        }

        private void OnRecive(object sender, TcpClientReciveEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibraryUnitTest::OnRecive(object, SshClientCommandExecuteEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= SshClientLibraryUnitTest::OnRecive(object, SshClientCommandExecuteEventArgs)");
        }

        private void OnCanceled(object sender, TcpClientCanceledEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> SshClientLibraryUnitTest::OnCanceled(object, TcpClientCanceledEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= SshClientLibraryUnitTest::OnCanceled(object, TcpClientCanceledEventArgs)");
        }
    }
}
