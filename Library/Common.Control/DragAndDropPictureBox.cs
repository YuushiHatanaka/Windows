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

namespace Common.Control
{
    /// <summary>
    /// ドラック＆ドロップPictureBox
    /// </summary>
    public class DragAndDropPictureBox : PictureBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DragAndDropPictureBox()
            : base()
        {
            this.AllowDrop = true;
            this.DragEnter += this.OnDragEnter;
            this.DragDrop += this.OnDragDrop;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        /// <summary>
        /// OnDragDrop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string url = e.Data.GetData(DataFormats.Text).ToString();
            this.ImageLocation = url;
        }

        /// <summary>
        /// OnDragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UniformResourceLocator") ||
                   e.Data.GetDataPresent("UniformResourceLocatorW"))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
    }
}
