namespace ServiceOperation
{
    public partial class dialogUserAuthentication
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            Common.Control.ControlMargin controlMargin2 = new Common.Control.ControlMargin();
            Common.Control.ControlMargin controlMargin3 = new Common.Control.ControlMargin();
            this.labeledTextBox_UserName = new Common.Windows.Forms.LabeledTextBox();
            this.labeledTextBox_Password = new Common.Windows.Forms.LabeledTextBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labeledTextBox_UserName
            // 
            controlMargin2.Bottom = 1;
            controlMargin2.Left = 1;
            controlMargin2.Right = 1;
            controlMargin2.Top = 1;
            this.labeledTextBox_UserName.ControlMargin = controlMargin2;
            // 
            // 
            // 
            this.labeledTextBox_UserName.Label.Location = new System.Drawing.Point(1, 1);
            this.labeledTextBox_UserName.Label.Name = "m_Label";
            this.labeledTextBox_UserName.Label.Size = new System.Drawing.Size(96, 12);
            this.labeledTextBox_UserName.Label.TabIndex = 0;
            this.labeledTextBox_UserName.Label.Text = "ユーザ名";
            this.labeledTextBox_UserName.LabelPosition = Common.Windows.Forms.ControlPositions.Left;
            this.labeledTextBox_UserName.Location = new System.Drawing.Point(12, 12);
            this.labeledTextBox_UserName.Name = "labeledTextBox_UserName";
            this.labeledTextBox_UserName.SeparateSize = 1;
            this.labeledTextBox_UserName.Size = new System.Drawing.Size(302, 21);
            this.labeledTextBox_UserName.TabIndex = 0;
            // 
            // 
            // 
            this.labeledTextBox_UserName.TextBox.Location = new System.Drawing.Point(98, 1);
            this.labeledTextBox_UserName.TextBox.Name = "";
            this.labeledTextBox_UserName.TextBox.Size = new System.Drawing.Size(203, 19);
            this.labeledTextBox_UserName.TextBox.TabIndex = 0;
            // 
            // labeledTextBox_Password
            // 
            controlMargin3.Bottom = 1;
            controlMargin3.Left = 1;
            controlMargin3.Right = 1;
            controlMargin3.Top = 1;
            this.labeledTextBox_Password.ControlMargin = controlMargin3;
            // 
            // 
            // 
            this.labeledTextBox_Password.Label.Location = new System.Drawing.Point(1, 1);
            this.labeledTextBox_Password.Label.Name = "m_Label";
            this.labeledTextBox_Password.Label.Size = new System.Drawing.Size(96, 12);
            this.labeledTextBox_Password.Label.TabIndex = 0;
            this.labeledTextBox_Password.Label.Text = "パスワード";
            this.labeledTextBox_Password.LabelPosition = Common.Windows.Forms.ControlPositions.Left;
            this.labeledTextBox_Password.Location = new System.Drawing.Point(11, 39);
            this.labeledTextBox_Password.Name = "labeledTextBox_Password";
            this.labeledTextBox_Password.SeparateSize = 1;
            this.labeledTextBox_Password.Size = new System.Drawing.Size(303, 21);
            this.labeledTextBox_Password.TabIndex = 1;
            // 
            // 
            // 
            this.labeledTextBox_Password.TextBox.Location = new System.Drawing.Point(98, 1);
            this.labeledTextBox_Password.TextBox.Name = "";
            this.labeledTextBox_Password.TextBox.PasswordChar = '*';
            this.labeledTextBox_Password.TextBox.Size = new System.Drawing.Size(204, 19);
            this.labeledTextBox_Password.TextBox.TabIndex = 0;
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(116, 66);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(96, 22);
            this.button_Ok.TabIndex = 2;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(218, 66);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(96, 22);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "キャンセル";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // dialogUserAuthentication
            // 
            this.AcceptButton = this.button_Ok;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(329, 107);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.labeledTextBox_Password);
            this.Controls.Add(this.labeledTextBox_UserName);
            this.Name = "dialogUserAuthentication";
            this.Text = "ユーザ認証";
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Windows.Forms.LabeledTextBox labeledTextBox_UserName;
        private Common.Windows.Forms.LabeledTextBox labeledTextBox_Password;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
    }
}

