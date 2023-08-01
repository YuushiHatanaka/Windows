using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Common.Control
{
    /// <summary>
    /// ファイルListViewItem
    /// </summary>
    public class FileListViewItem : ListViewItem
    {
        /// <summary>
        /// ディレクトリ情報
        /// </summary>
        protected DirectoryInfo m_DirectoryInfo = null;

        /// <summary>
        /// ディレクトリ情報
        /// </summary>
        public DirectoryInfo DirectoryInfo { get { return this.m_DirectoryInfo; } }

        /// <summary>
        /// ファイル情報
        /// </summary>
        protected FileInfo m_FileInfo = null;

        /// <summary>
        /// ファイル情報
        /// </summary>
        public FileInfo FileInfo { get { return this.m_FileInfo; } }

        /// <summary>
        /// ファイル属性
        /// </summary>
        protected FileAttributes m_FileAttributes = FileAttributes.Normal;

        /// <summary>
        /// ファイル属性
        /// </summary>
        public FileAttributes Attributes { get { return this.m_FileAttributes; } }

        #region SHGetFileInfo
        // SHGetFileInfo関数
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        // SHGetFileInfo関数で使用するフラグ
        private const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
        private const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
        private const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン
        private const uint SHGFI_TYPENAME = 0x400;//ファイルの種類

        // SHGetFileInfo関数で使用する構造体
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
        /// <summary>
        /// VB 関数をC#に移植する: Chr()
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Char Chr(int i)
        {
            //指定した値を Unicode 文字に変換します。
            return Convert.ToChar(i);
        }

        /// <summary>
        /// ファイルの種類を取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetTypeName(string name)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            shfi.szDisplayName = new String(Chr(0), 260);
            shfi.szTypeName = new String(Chr(0), 80);
            IntPtr hSuccess = SHGetFileInfo(name, 0, ref shfi,
                (uint)Marshal.SizeOf(shfi), SHGFI_TYPENAME);
            return shfi.szTypeName;
        }
        #endregion

        /// <summary>
        /// ファイルサイズを単位付きに変換して返します.
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        protected string getFileSize(long fileSize)
        {
            string ret = fileSize + " バイト";
            if (fileSize > (1024f * 1024f * 1024f))
            {
                ret = string.Format("{0:#,0} GB", Math.Round((fileSize / 1024f / 1024f / 1024f)));
                //ret = Math.Round((fileSize / 1024f / 1024f / 1024f), 2).ToString() + " GB";
            }
            else if (fileSize > (1024f * 1024f))
            {
                ret = string.Format("{0:#,0} MB", Math.Round((fileSize / 1024f / 1024f)));
                //ret = Math.Round((fileSize / 1024f / 1024f), 2).ToString() + " MB";
            }
            else if (fileSize > 1024f)
            {
                ret = string.Format("{0:#,0} KB", Math.Round((fileSize / 1024f)));
                //ret = Math.Round((fileSize / 1024f)).ToString() + " KB";
            }

            return ret;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileListViewItem()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        public FileListViewItem(string path)
            : base()
        {
            // ファイル属性取得
            this.m_FileAttributes = File.GetAttributes(path);

            // ディレクトリの場合
            if (this.m_FileAttributes.HasFlag(FileAttributes.Directory))
            {
                // ディレクトリ情報を設定
                this.m_DirectoryInfo = new DirectoryInfo(path);
                this.Text = this.m_DirectoryInfo.Name;
                this.ToolTipText = this.m_DirectoryInfo.FullName;

                // アイコン
                this.ImageIndex = 0;

                // サブアイテム追加
                this.SubItems.Add(this.m_DirectoryInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add(this.GetTypeName(path));
                this.SubItems.Add("-");
            }
            else
            {
                //ファイル情報を取得
                this.m_FileInfo = new FileInfo(path);
                this.Text = this.m_FileInfo.Name;
                this.ToolTipText = this.m_FileInfo.FullName;

                // サブアイテム追加
                this.SubItems.Add(this.m_FileInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add(this.GetTypeName(path));
                this.SubItems.Add(this.getFileSize(this.m_FileInfo.Length));
                //this.SubItems.Add(String.Format("{0:#,0}", this.m_FileInfo.Length));
            }
        }
    }
}
