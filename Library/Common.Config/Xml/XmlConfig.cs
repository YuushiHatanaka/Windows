using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using log4net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Net;
using System.Xml.Linq;

namespace Common.Config
{
    /// <summary>
    /// XmlConfigクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlConfig<T> : IDisposable
    {
        #region ロガーオブジェクト
        /// <summary>
        /// ロガーオブジェクト
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Disposeフラグ
        /// <summary>
        /// Disposeフラグ
        /// </summary>
        protected bool m_Disposed = false;
        #endregion

        #region XmlSerializerオブジェクト
        /// <summary>
        /// XmlSerializerオブジェクト
        /// </summary>
        private XmlSerializer m_XmlSerializer = null;
        #endregion

        #region 定義ファイル名
        /// <summary>
        /// 定義ファイル名
        /// </summary>
        private string FileName { get; set; }
        #endregion

        #region デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public XmlConfig(string fileName)
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfig::XmlConfig(string)");
            Logger.DebugFormat("fileName:{0}", fileName);

            // XmlSerializerオブジェクト生成
            m_XmlSerializer = new XmlSerializer(typeof(T));

            // 定義ファイル名設定
            FileName = fileName;

            // ロギング
            Logger.Debug("<<<<= XmlConfig::XmlConfig(string)");
        }
        #endregion

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~XmlConfig()
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfig::~XmlConfig()");

            // リソース破棄
            Dispose(false);

            // ロギング
            Logger.Debug("<<<<= XmlConfig::~XmlConfig()");
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfig::Dispose()");

            // リソース破棄
            Dispose(true);

            // ガベージコレクション
            GC.SuppressFinalize(this);

            // ロギング
            Logger.Debug("<<<<= XmlConfig::Dispose()");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfig::Dispose(bool)");

            if (!m_Disposed)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources here.
                }

                // TODO: Free unmanaged resources here.

                // Note disposing has been done.
                m_Disposed = true;
            }

            // ロギング
            Logger.Debug("<<<<= XmlConfig::Dispose(bool)");
        }
        #endregion

        /// <summary>
        /// ファイル存在判定
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfig::Exists()");

            // ファイル存在を返却
            bool result = File.Exists(FileName);

            // ロギング
            Logger.DebugFormat("result:{0}", result);
            Logger.Debug("<<<<= XmlConfig::Exists()");

            // 結果返却
            return result;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="objects"></param>
        public void Save(T objects)
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfig::Save(T)");
            Logger.DebugFormat("objects:{0}", objects.GetType());

            // FileStreamオブジェクト生成
            using (FileStream fileStream = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                // オブジェクトをシリアル化してXMLファイルに書き込む
                m_XmlSerializer.Serialize(fileStream, objects);
            }

            // ロギング
            Logger.Debug("<<<<= XmlConfig::Save(T)");
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public T Load()
        {
            // ロギング
            Logger.Debug("=>>>> XmlConfig::Load()");

            // FileStreamオブジェクト生成
            using (FileStream fileStream = new FileStream(FileName, FileMode.Open))
            {
                // XMLファイルを読み込み、逆シリアル化（復元）する
                T result = (T)m_XmlSerializer.Deserialize(fileStream);

                // ロギング
                Logger.DebugFormat("result:{0}", result.GetType());
                Logger.Debug("<<<<= XmlConfig::Load()");

                // 結果返却
                return result;
            }
        }
    }
}
