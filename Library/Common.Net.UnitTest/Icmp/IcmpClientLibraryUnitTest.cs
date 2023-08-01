using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.Net.UnitTest
{
    [TestClass]
    public class IcmpClientLibraryUnitTest
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
            using (IcmpClientLibrary icmpClientLibrary = new IcmpClientLibrary("186.32.1.201"))
            {
                icmpClientLibrary.Send(4, 100);
            }
        }

        [TestMethod]
        public async Task AsycExcecute()
        {
            using (IcmpClientLibrary icmpClientLibrary = new IcmpClientLibrary("186.32.1.201"))
            {
                icmpClientLibrary.OnResponse += OnResponse;
                icmpClientLibrary.OnCompleted += OnCompleted;
                await icmpClientLibrary.AsyncSend(4, 100);
            }
        }

        private void OnResponse(object sender, IcmpClientResponseEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibraryUnitTest::OnResponse(object, IcmpClientResponseEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= NtpClientLibraryUnitTest::OnResponse(object, IcmpClientResponseEventArgs)");
        }

        private void OnCompleted(object sender, IcmpClientCompletedEventArgs e)
        {
            // ロギング
            Logger.Debug("=>>>> IcmpClientLibraryUnitTest::OnCompleted(object, IcmpClientCompletedEventArgs)");
            Logger.DebugFormat("sender:{0}", sender.ToString());
            Logger.DebugFormat("e     :\n{0}", e.ToString());

            // ロギング
            Logger.Debug("<<<<<= NtpClientLibraryUnitTest::OnCompleted(object, IcmpClientCompletedEventArgs)");
        }
    }
}
