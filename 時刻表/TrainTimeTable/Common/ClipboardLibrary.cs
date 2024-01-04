using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// ClipboardLibraryクラス
    /// </summary>
    public class ClipboardLibrary
    {
        /// <summary>
        /// 独自のクラスのオブジェクトをクリップボードに設定するメソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void SetObjectToClipboard<T>(T data)
        {
            // BinaryFormatterを使用してオブジェクトをシリアル化
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, data);

                // シリアル化されたデータをクリップボードに設定
                Clipboard.SetData(typeof(T).FullName, stream.ToArray());
            }
        }

        /// <summary>
        /// クリップボードから独自のクラスのオブジェクトを取得するメソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObjectFromClipboard<T>()
        {
            // クリップボードからデータを取得
            IDataObject clipboardData = Clipboard.GetDataObject();

            // データが存在するか確認
            if (clipboardData != null && clipboardData.GetDataPresent(typeof(T).FullName))
            {
                // シリアル化されたデータを取得
                object serializedData = clipboardData.GetData(typeof(T).FullName);

                if (serializedData is byte[] byteArray)
                {
                    // BinaryFormatterを使用してデータをデシリアライズ
                    using (MemoryStream stream = new MemoryStream(byteArray))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return (T)formatter.Deserialize(stream);
                    }
                }
            }

            return default(T);
        }
    }
}
