using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Common.Config.UnitTest
{
    [TestClass]
    public class IniFileUnitTest
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
            Logger.Debug("=>>>> IniFileUnitTest::Exists()");

            // IniFileオブジェクト生成
            using (IniFile iniFileSuccess = new IniFile("Ini/IniFileUnitTest.ini"))
            {
                Logger.DebugFormat("IniFile.Exists():[{0}]", iniFileSuccess.Exists());
            }

            // IniFileオブジェクト生成
            using (IniFile iniFileFailure = new IniFile("Ini/IniFileUnitFailure.ini"))
            {
                Logger.DebugFormat("IniFile.Exists():[{0}]", iniFileFailure.Exists());
            }

            // ロギング
            Logger.Debug("<<<<= IniFileUnitTest::Exists()");
        }


        [TestMethod]
        public void Write()
        {
            // ロギング
            Logger.Debug("=>>>> IniFileUnitTest::Write()");

            // IniFileオブジェクト生成
            using (IniFile iniFile = new IniFile("Ini/IniFileUnitTest.ini"))
            {
                // 書込
                iniFile.Write("IniFile1", "stringKey", "string");
                iniFile.Write("IniFile1", "boolKey", true);
                iniFile.Write("IniFile1", "intKey", 256);
                iniFile.Write("IniFile2", "stringKey", "String");
                iniFile.Write("IniFile2", "boolKey", false);
                iniFile.Write("IniFile2", "intKey", 4096);
            }

            // ロギング
            Logger.Debug("<<<<= IniFileUnitTest::Write()");
        }

        [TestMethod]
        public void GetValue()
        {
            // ロギング
            Logger.Debug("=>>>> IniFileUnitTest::GetValue()");

            // IniFileオブジェクト生成
            using (IniFile iniFile = new IniFile("Ini/IniFileUnitTest.ini"))
            {
                // 読み出し
                Logger.DebugFormat("IniFile.GetStringValue():[{0}]", iniFile.GetStringValue("IniFile1", "stringKey"));
                Logger.DebugFormat("IniFile.GetBoolValue()  :[{0}]", iniFile.GetBoolValue("IniFile1", "boolKey"));
                Logger.DebugFormat("IniFile.GetIntValue()   :[{0}]", iniFile.GetIntValue("IniFile1", "intKey"));
                Logger.DebugFormat("IniFile.GetStringValue():[{0}]", iniFile.GetStringValue("IniFile2", "stringKey"));
                Logger.DebugFormat("IniFile.GetBoolValue()  :[{0}]", iniFile.GetBoolValue("IniFile2", "boolKey"));
                Logger.DebugFormat("IniFile.GetIntValue()   :[{0}]", iniFile.GetIntValue("IniFile2", "intKey"));
            }

            // ロギング
            Logger.Debug("<<<<= IniFileUnitTest::GetValue()");
        }

        [TestMethod]
        public void GetSections()
        {
            // ロギング
            Logger.Debug("=>>>> IniFileUnitTest::GetSections()");

            // IniFileオブジェクト生成
            using (IniFile iniFile = new IniFile("Ini/IniFileUnitTest.ini"))
            {
                // セクション一覧取得
                foreach (string section in iniFile.GetSections())
                {
                    // ロギング
                    Logger.DebugFormat("section:[{0}]", section);
                }
            }

            // ロギング
            Logger.Debug("<<<<= IniFileUnitTest::GetSections()");
        }

        [TestMethod]
        public void GetKeys()
        {
            // ロギング
            Logger.Debug("=>>>> IniFileUnitTest::GetKeys()");

            // IniFileオブジェクト生成
            using (IniFile iniFile = new IniFile("Ini/IniFileUnitTest.ini"))
            {
                // セクション一覧取得
                foreach (string section in iniFile.GetSections())
                {
                    // キー一覧取得
                    foreach (string key in iniFile.GetKeys(section))
                    {
                        // ロギング
                        Logger.DebugFormat("section:[{0}], key:[{1}]", section, key);
                    }
                }
            }

            // ロギング
            Logger.Debug("<<<<= IniFileUnitTest::GetKeys()");
        }

        [TestMethod]
        public void Remove()
        {
            // ロギング
            Logger.Debug("=>>>> IniFileUnitTest::Remove()");

            // IniFileオブジェクト生成
            using (IniFile iniFile = new IniFile("Ini/IniFileUnitTest.ini"))
            {
                // セクション一覧取得
                foreach (string section in iniFile.GetSections())
                {
                    // 削除
                    iniFile.Remove(section);
                }

                // 書込
                iniFile.Write("IniFile1", "stringKey", "string");
                iniFile.Write("IniFile1", "boolKey", true);
                iniFile.Write("IniFile1", "intKey", 256);
                iniFile.Write("IniFile2", "stringKey", "String");
                iniFile.Write("IniFile2", "boolKey", false);
                iniFile.Write("IniFile2", "intKey", 4096);

                // セクション一覧取得
                foreach (string section in iniFile.GetSections())
                {
                    // キー一覧取得
                    foreach (string key in iniFile.GetKeys(section))
                    {
                        // 削除
                        iniFile.Remove(section, key);
                    }
                }

            }

            // ロギング
            Logger.Debug("<<<<= IniFileUnitTest::Remove()");
        }
    }
}
