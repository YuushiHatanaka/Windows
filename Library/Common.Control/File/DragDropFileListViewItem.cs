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
    public class DragDropFileListViewItem : FileListViewItem
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DragDropFileListViewItem()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        public DragDropFileListViewItem(string path)
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
                this.SubItems.Add(this.GetTypeName(path));
                this.SubItems.Add("-");
                this.SubItems.Add(this.m_DirectoryInfo.CreationTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add(this.m_DirectoryInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add(this.m_DirectoryInfo.LastAccessTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add("");
            }
            else
            {
                //ファイル情報を取得
                this.m_FileInfo = new FileInfo(path);
                this.Text = this.m_FileInfo.Name;
                this.ToolTipText = this.m_FileInfo.FullName;

                // サブアイテム追加
                this.SubItems.Add(this.GetTypeName(path));
                this.SubItems.Add(this.getFileSize(this.m_FileInfo.Length));
                this.SubItems.Add(this.m_FileInfo.CreationTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add(this.m_FileInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add(this.m_FileInfo.LastAccessTime.ToString("yyyy/MM/dd HH:mm:ss"));
                this.SubItems.Add("");
            }
        }
    }
}
