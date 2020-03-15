namespace Common.Windows.Forms
{
    partial class LabeledControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.m_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_Label
            // 
            this.m_Label.AutoSize = true;
            this.m_Label.Location = new System.Drawing.Point(3, 0);
            this.m_Label.Name = "m_Label";
            this.m_Label.Size = new System.Drawing.Size(29, 12);
            this.m_Label.TabIndex = 0;
            this.m_Label.Text = "label";
            // 
            // LabeledControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_Label);
            this.Name = "LabeledControl";
            this.Size = new System.Drawing.Size(128, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_Label;
    }
}