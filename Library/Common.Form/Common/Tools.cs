using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Forms
{
    /// <summary>
    /// Toolsクラス
    /// </summary>
    public class Tools
    {
        #region Assembly関連
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
        #endregion
    }
}
