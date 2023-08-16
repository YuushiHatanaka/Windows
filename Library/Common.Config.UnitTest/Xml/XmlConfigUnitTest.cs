using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Config.UnitTest
{
    [TestClass]
    public class XmlConfigUnitTest
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [TestMethod]
        public void Exists()
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfigUnitTest::Exists()");

            // XmlConfigオブジェクト生成
            using (XmlConfig<List<string>> xmlConfig = new XmlConfig<List<string>>("Xml/XmlConfigUnitTest.xml"))
            {
                // 存在判定
                Logger.DebugFormat("xmlConfig.Exists():[{0}]", xmlConfig.Exists());
            }

            // XmlConfigオブジェクト生成
            using (XmlConfig<List<string>> xmlConfig = new XmlConfig<List<string>>("Xml/XmlConfigUnitTest2.xml"))
            {
                // 存在判定
                Logger.DebugFormat("xmlConfig.Exists():[{0}]", xmlConfig.Exists());
            }

            // ロギング
            Logger.Debug("<<<<= XmlConfigUnitTest::Exists()");
        }

        [TestMethod]
        public void Save()
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfigUnitTest::Save()");

            // オブジェクト生成
            List<string> strings = new List<string>();
            strings.Add("Steins Gate");
            strings.Add("ゆるキャン△");

            // XmlConfigオブジェクト生成
            using (XmlConfig<List<string>> xmlConfig = new XmlConfig<List<string>>("Xml/XmlConfigUnitTest.xml"))
            {
                // 保存
                xmlConfig.Save(strings);
            }

            // ロギング
            Logger.Debug("<<<<= XmlConfigUnitTest::Save()");
        }

        [TestMethod]
        public void Load()
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfigUnitTest::Load()");

            // XmlConfigオブジェクト生成
            using (XmlConfig<List<string>> xmlConfig = new XmlConfig<List<string>>("Xml/XmlConfigUnitTest.xml"))
            {
                // 読込
                List<string> strings = xmlConfig.Load();

                // ロギング
                foreach (string s in strings)
                {
                    Logger.Debug(s);
                }
            }

            // ロギング
            Logger.Debug("<<<<= XmlConfigUnitTest::Load()");
        }
    }
}
