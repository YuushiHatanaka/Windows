using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Common.Diagnostics;

namespace Common.Net.UnitTest
{
    [TestClass]
    public class FtpClientLibraryUnitTest
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
            using (FtpClientLibrary ftpClientLibrary = new FtpClientLibrary("186.32.1.120"))
            {
                string result = string.Empty;
                ftpClientLibrary.UserInfo.Name = "root";
                ftpClientLibrary.UserInfo.Password = "Macross7";
                ftpClientLibrary.Connect();
                ftpClientLibrary.Login();
                ftpClientLibrary.PWD(out result);
                ftpClientLibrary.Logout();
                ftpClientLibrary.Disconnect();
            }
        }

        [TestMethod]
        public async Task AsyncExcecute()
        {
            using (FtpClientLibrary ftpClientLibrary = new FtpClientLibrary("186.32.1.120"))
            {
                ftpClientLibrary.UserInfo.Name = "root";
                ftpClientLibrary.UserInfo.Password = "Macross7";
                ftpClientLibrary.OnConnected += OnConnected;
                ftpClientLibrary.OnLogined += OnLogined;
                ftpClientLibrary.OnLogouted += OnLogouted;
                ftpClientLibrary.OnDisconnected += OnDisconnected;
                ftpClientLibrary.OnCommandExecute += OnCommandExecute;
                await ftpClientLibrary.AsyncConnect();
                await ftpClientLibrary.AsyncLogin();
                await ftpClientLibrary.AsyncPWD();
                await ftpClientLibrary.AsyncLogout();
                await ftpClientLibrary.AsyncDisconnect();
            }
        }


        private void OnConnected(object sender, TcpClientConnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibraryUnitTest::OnConnected(object, TcpClientConnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= FtpClientLibraryUnitTest::OnConnected(object, TcpClientConnectedEventArgs)");
        }

        private void OnLogined(object sender, FtpClientLoginEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibraryUnitTest::OnLogined(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= FtpClientLibraryUnitTest::OnLogined(object, TelnetClientLoginEventArgs)");
        }

        private void OnLogouted(object sender, FtpClientLogoutEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibraryUnitTest::OnLogouted(object, TelnetClientLogoutEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= FtpClientLibraryUnitTest::OnLogouted(object, TelnetClientLogoutEventArgs)");
        }

        private void OnDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibraryUnitTest::OnDisconnected(object, TcpClientDisconnectedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= FtpClientLibraryUnitTest::OnDisconnected(object, TcpClientDisconnectedEventArgs)");
        }

        private void OnCommandExecute(object sender, FtpClientCommandExecuteEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> FtpClientLibraryUnitTest::OnCommandExecute(object, FtpClientCommandExecuteEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= FtpClientLibraryUnitTest::OnCommandExecute(object, FtpClientCommandExecuteEventArgs)");
        }
    }
}
