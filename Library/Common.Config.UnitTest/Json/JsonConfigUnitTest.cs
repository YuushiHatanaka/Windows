using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;

namespace Common.Config.UnitTest
{
    [TestClass]
    public class JsonConfigUnitTest
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [TestMethod]
        public void FileParse()
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::FileParse()");

            // JsonConfigオブジェクト生成
            using (JsonConfig jsonConfig = new JsonConfig())
            {
                jsonConfig.OnStartObject = OnStartObject;
                jsonConfig.OnEndObject = OnEndObject;
                jsonConfig.OnStartArray = OnStartArray;
                jsonConfig.OnEndArray = OnEndArray;
                jsonConfig.OnStartElement = OnStartElement;
                jsonConfig.OnEndElement = OnEndElement;
                jsonConfig.OnKey = OnKey;
                jsonConfig.OnNull = OnNull;
                jsonConfig.OnBool = OnBool;
                jsonConfig.OnValue = OnValue;
                jsonConfig.OnNumber = OnNumber;
                jsonConfig.OnString = OnString;
                jsonConfig.OnError = OnError;

                // 解析
                jsonConfig.FileParse("Json/JsonConfigUnitTest.json");
            }

            // ロギング
            Logger.Debug("<<<<= JsonConfigUnitTest::FileParse()");
        }

        private bool OnStartObject(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnStartObject(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnStartObject(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnEndObject(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnEndObject(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnEndObject(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnStartArray(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnStartArray(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnStartArray(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnEndArray(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnEndArray(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnEndArray(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnStartElement(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnStartElement(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnStartElement(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnEndElement(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnEndElement(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnEndElement(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnKey(JsonEventArg args, object userData)
        {
            throw new NotImplementedException();
        }

        private bool OnNull(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnNull(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnNull(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnBool(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnBool(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnBool(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnValue(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnValue(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnValue(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnNumber(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnNumber(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnNumber(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private bool OnString(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnString(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnString(object, TelnetClientLoginEventArgs)");

            // 正常終了
            return true;
        }

        private void OnError(JsonEventArg args, object userData)
        {
            // ロギング
            Logger.Debug("=>>>> JsonConfigUnitTest::OnError(object, TelnetClientLoginEventArgs)");
            Logger.DebugFormat("JsonEventArg:{0}", args.ToString());
            Logger.DebugFormat("object      :{0}", userData?.ToString());

            // ロギング
            Logger.Debug("<<<<<= JsonConfigUnitTest::OnError(object, TelnetClientLoginEventArgs)");
        }
    }
}
