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
    /// ファイルListView
    /// </summary>
    public class FileListView : ListView
    {
        /// <summary>
        /// パス
        /// </summary>
        private string m_Path = string.Empty;

        /// <summary>
        /// パス
        /// </summary>
        public string Path { get { return this.m_Path; } }

        /// <summary>
        /// マスク
        /// </summary>
        private string m_Mask = string.Empty;

        /// <summary>
        /// マスク
        /// </summary>
        public string Mask { get { return this.m_Mask; } }

        /// <summary>
        /// イベントハンドラ
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileListView()
            : base()
        {
            // 初期化
            this.Initialize();

            // 更新
            this.Update(Directory.GetCurrentDirectory(), "*");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        public FileListView(string path)
            : base()
        {
            // 初期化
            this.Initialize();

            // 更新
            this.Update(path, "*");
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            // 各設定
            this.FullRowSelect = true;
            this.GridLines = true;
            this.Sorting = SortOrder.None;
            this.View = View.Details;
            this.ShowItemToolTips = true;

            // カラムヘッダ設定
            ColumnHeader[] colHeaderRegValue =
            {
                new ColumnHeader(){Text="名前"     ,Width=160},
                new ColumnHeader(){Text="更新日時" ,Width=128, TextAlign=HorizontalAlignment.Right},
                new ColumnHeader(){Text="種類"     ,Width=128, TextAlign=HorizontalAlignment.Left},
                new ColumnHeader(){Text="サイズ"   ,Width= 96, TextAlign=HorizontalAlignment.Right},
            };

            // カラムヘッダ追加
            this.Columns.AddRange(colHeaderRegValue);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="path"></param>
        public void Update(string path)
        {
            // 更新
            this.Update(path, this.m_Mask);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mask"></param>
        public void Update(string path, string mask)
        {
            // 設定
            if (!Directory.Exists(path))
            {
                // 例外
                throw new DirectoryNotFoundException("ディレクトリが存在しません：[" + path + "]");
            }

            // 設定
            this.m_Path = path;
            this.m_Mask = mask;

            // 更新開始
            this.BeginUpdate();

            // クリア
            this.Items.Clear();

            // TODO:追加(カレントディレクトリ)

            // TODO:追加(親ディレクトリ)

            // 配下のディレクトリを取得
            foreach (string directory in Directory.GetDirectories(this.m_Path))
            {
                // ファイルListViewItemオブジェクト生成
                FileListViewItem _item = new FileListViewItem(directory);

                // 追加
                this.Items.Add(_item);
            }

            // 配下のファイルを取得
            foreach (string file in Directory.EnumerateFiles(this.m_Path, this.m_Mask, SearchOption.TopDirectoryOnly))
            {
                // ファイルListViewItemオブジェクト生成
                FileListViewItem _item = new FileListViewItem(file);

                // 追加
                this.Items.Add(_item);
            }

            // イベント情報生成
            FileListViewUpdatedEventArgs _args = new FileListViewUpdatedEventArgs();
            _args.Path = this.m_Path;
            _args.Mask = this.m_Mask;

            // 更新イベント
            this.OnUpdated(_args);

            // 更新終了
            this.EndUpdate();
        }

        /// <summary>
        /// 更新イベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdated(FileListViewUpdatedEventArgs e)
        {
            // イベントハンドラ呼出し
            if (this.Updated != null)
            {
                // 呼出し
                this.Updated(this, e);
            }
        }
    }

    /// <summary>
    /// イベント引数型
    /// </summary>
    public class FileListViewUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// パス
        /// </summary>
        public string Path = string.Empty;

        /// <summary>
        /// マスク
        /// </summary>
        public string Mask = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileListViewUpdatedEventArgs()
            : base()
        {

        }
    }

    /// <summary>
    /// ファイルListViewItem
    /// </summary>
    public class FileListViewItem : ListViewItem
    {
        /// <summary>
        /// ディレクトリ情報
        /// </summary>
        public DirectoryInfo m_DirectoryInfo = null;

        /// <summary>
        /// ディレクトリ情報
        /// </summary>
        public DirectoryInfo DirectoryInfo { get { return this.m_DirectoryInfo; } }

        /// <summary>
        /// ファイル情報
        /// </summary>
        public FileInfo m_FileInfo = null;

        /// <summary>
        /// ファイル情報
        /// </summary>
        public FileInfo FileInfo { get { return this.m_FileInfo; } }

        /// <summary>
        /// ファイル属性
        /// </summary>
        private FileAttributes m_FileAttributes = FileAttributes.Normal;

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
        private string GetTypeName(string name)
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
                this.SubItems.Add(this.m_DirectoryInfo.LastWriteTime.ToString());
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
                this.SubItems.Add(this.m_FileInfo.LastWriteTime.ToString());
                this.SubItems.Add(this.GetTypeName(path));
                this.SubItems.Add(String.Format("{0:#,0}", this.m_FileInfo.Length));
            }
        }
    }
}
