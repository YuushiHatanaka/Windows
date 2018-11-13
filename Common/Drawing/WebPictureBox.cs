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

namespace Common.Drawing
{
    /// <summary>
    /// Webピクチャーボックス
    /// </summary>
    public class WebPictureBox : PictureBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WebPictureBox()
            : base()
        {
            Trace.WriteLine("=>>>> WebPictureBox::WebPictureBox()");

            // 設定
            this.AllowDrop = true;
            this.DragEnter += this.OnDragEnter;
            this.DragDrop += this.OnDragDrop;
            this.SizeMode = PictureBoxSizeMode.Normal;

            Trace.WriteLine("<<<<= WebPictureBox::WebPictureBox()");
        }

        /// <summary>
        /// OnDragDrop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragDrop(object sender, DragEventArgs e)
        {
            Trace.WriteLine("=>>>> WebPictureBox::OnDragDrop(object, DragEventArgs)");

            string url = e.Data.GetData(DataFormats.Text).ToString();
            this.ImageLocation = url;

            Trace.WriteLine("<<<<= WebPictureBox::OnDragDrop(object, DragEventArgs)");
        }

        /// <summary>
        /// OnDragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragEnter(object sender, DragEventArgs e)
        {
            Trace.WriteLine("=>>>> WebPictureBox::OnDragEnter(object, DragEventArgs)");

            if (e.Data.GetDataPresent("UniformResourceLocator") ||
                   e.Data.GetDataPresent("UniformResourceLocatorW"))
            {
                e.Effect = DragDropEffects.Copy;
            }

            Trace.WriteLine("<<<<= WebPictureBox::OnDragEnter(object, DragEventArgs)");
        }
    }
}
