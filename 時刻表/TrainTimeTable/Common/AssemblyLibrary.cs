using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrainTimeTable.Common
{
    /// <summary>
    /// AssemblyLibraryクラス
    /// </summary>
    public class AssemblyLibrary
    {
        /// <summary>
        /// Title取得(Version付き)
        /// </summary>
        /// <returns></returns>
        public static string GetTitleVersion()
        {
            // 返却
            return string.Format("{0} {1}", GetTitle(), GetVersion());
        }

        /// <summary>
        /// Title取得
        /// </summary>
        /// <returns></returns>
        public static string GetTitle()
        {
            // AssemblyTitle取得
            AssemblyTitleAttribute asmttl =
                (AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute));

            // 返却
            return asmttl.Title;
        }

        /// <summary>
        /// Version取得
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            // Assembly取得
            Assembly asm = Assembly.GetExecutingAssembly();

            // バージョン取得
            Version ver = asm.GetName().Version;

            // 返却
            return ver.ToString();
        }

        /// <summary>
        /// Copyright取得
        /// </summary>
        /// <returns></returns>
        public static string GetCopyright()
        {
            // AssemblyCopyrightの取得
            AssemblyCopyrightAttribute asmcpy =
                (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));

            // 返却
            return asmcpy.Copyright;
        }
    }
}
