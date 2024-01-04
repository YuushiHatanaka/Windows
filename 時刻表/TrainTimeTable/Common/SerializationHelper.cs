using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// SerializationHelperクラス
    /// </summary>
    public class SerializationHelper
    {
        public static byte[] SerializeObject(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static object DeserializeObject(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(memoryStream);
            }
        }
    }
}
