using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrainTimeTable.Property;

namespace TrainTimeTable.File
{
    /// <summary>
    /// Oudia2Fileクラス
    /// </summary>
    public class Oudia2File : OudiaFile
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public Oudia2File(string fileName)
            : base(fileName)
        {
            // ロギング
            Logger.Debug("=>>>> Oudia2File::Oudia2File(string)");
            Logger.DebugFormat("fileName:[{0}]", fileName);

            // ロギング
            Logger.Debug("<<<<= Oudia2File::Oudia2File(string)");
        }
        #endregion
    }
}
