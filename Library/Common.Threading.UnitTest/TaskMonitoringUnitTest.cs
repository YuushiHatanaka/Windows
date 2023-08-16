using CommonLibrary;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Common.Threading.UnitTest
{
    [TestClass]
    public class TaskMonitoringUnitTest
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [TestMethod]
        public void Start()
        {
            // ロギング
            Logger.Debug("=>>>> TaskMonitoringUnitTest::Start()");

            using (TaskMonitoring task = new TaskMonitoring())
            {
                task.Start();
                System.Threading.Thread.Sleep(1000);
                task.End();
            }

            // ロギング
            Logger.Debug("<<<<= TaskMonitoringUnitTest::Start()");
        }
    }
}
