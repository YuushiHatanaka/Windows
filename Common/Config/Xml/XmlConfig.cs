using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Common.Config.Xml
{
    public class XmlConfig<T>
    {
        /// <summary>
        /// XmlSerializerオブジェクト
        /// </summary>
        private XmlSerializer m_XmlSerializer = null;

        #region 定義ファイル名
        /// <summary>
        /// 定義ファイル名
        /// </summary>
        private string m_FileName = string.Empty;

        /// <summary>
        /// 定義ファイル名
        /// </summary>
        public string FileName
        {
            get
            {
                return this.m_FileName;
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public XmlConfig(string fileName)
        {
            Trace.WriteLine("=>>>> XmlConfig::XmlConfig(string, Type)");

            // XmlSerializerオブジェクト生成
            this.m_XmlSerializer = new XmlSerializer(typeof(T));

            // 定義ファイル名設定
            this.m_FileName = fileName;

            Trace.WriteLine("<<<<= XmlConfig::XmlConfig(string, Type)");
        }

        /// <summary>
        /// ファイル存在判定
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            // ファイル存在を返却
            return File.Exists(this.m_FileName);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="objects"></param>
        public void Save(T objects)
        {
            Trace.WriteLine("=>>>> XmlConfig::Save(T)");

            // FileStreamオブジェクト生成
            using (FileStream _FileStream = new FileStream(this.m_FileName, FileMode.OpenOrCreate))
            {
                // オブジェクトをシリアル化してXMLファイルに書き込む
                this.m_XmlSerializer.Serialize(_FileStream, objects);
            }

            Trace.WriteLine("<<<<= XmlConfig::Save(T)");
        }

        /// <summary>
        /// 読込
        /// </summary>
        /// <returns></returns>
        public T Load()
        {
            Trace.WriteLine("=>>>> XmlConfig::Load()");

            // FileStreamオブジェクト生成
            using (FileStream _FileStream = new FileStream(this.m_FileName, FileMode.Open))
            {
                // XMLファイルを読み込み、逆シリアル化（復元）する
                Trace.WriteLine("<<<<= XmlConfig::Load()");
                return (T)this.m_XmlSerializer.Deserialize(_FileStream);
            }
        }
    }
}
