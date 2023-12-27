namespace TrainTimeTable
{
    partial class FormTrainProperty
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelButton = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabControlTrainProperty = new System.Windows.Forms.TabControl();
            this.tabPageTrainInfomation = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTrainProperty = new System.Windows.Forms.TableLayoutPanel();
            this.labelTrainNo = new System.Windows.Forms.Label();
            this.labelTrainType = new System.Windows.Forms.Label();
            this.labelTrainName = new System.Windows.Forms.Label();
            this.labelDepartingStation = new System.Windows.Forms.Label();
            this.labelDestinationStation = new System.Windows.Forms.Label();
            this.labelRemarks = new System.Windows.Forms.Label();
            this.textBoxTrainNo = new System.Windows.Forms.TextBox();
            this.textBoxTrainName = new System.Windows.Forms.TextBox();
            this.textBoxTrainNumber = new System.Windows.Forms.TextBox();
            this.labelTrainNumber = new System.Windows.Forms.Label();
            this.textBoxRemarks = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelButton.SuspendLayout();
            this.tabControlTrainProperty.SuspendLayout();
            this.tabPageTrainInfomation.SuspendLayout();
            this.tableLayoutPanelTrainProperty.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelButton, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tabControlTrainProperty, 0, 0);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(347, 417);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // tableLayoutPanelButton
            // 
            this.tableLayoutPanelButton.ColumnCount = 3;
            this.tableLayoutPanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanelButton.Controls.Add(this.buttonCancel, 2, 0);
            this.tableLayoutPanelButton.Controls.Add(this.buttonOK, 1, 0);
            this.tableLayoutPanelButton.Location = new System.Drawing.Point(3, 388);
            this.tableLayoutPanelButton.Name = "tableLayoutPanelButton";
            this.tableLayoutPanelButton.RowCount = 1;
            this.tableLayoutPanelButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButton.Size = new System.Drawing.Size(305, 26);
            this.tableLayoutPanelButton.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(210, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 20);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(112, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 20);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tabControlTrainProperty
            // 
            this.tabControlTrainProperty.Controls.Add(this.tabPageTrainInfomation);
            this.tabControlTrainProperty.Location = new System.Drawing.Point(3, 3);
            this.tabControlTrainProperty.Name = "tabControlTrainProperty";
            this.tabControlTrainProperty.SelectedIndex = 0;
            this.tabControlTrainProperty.Size = new System.Drawing.Size(339, 331);
            this.tabControlTrainProperty.TabIndex = 4;
            // 
            // tabPageTrainInfomation
            // 
            this.tabPageTrainInfomation.Controls.Add(this.tableLayoutPanelTrainProperty);
            this.tabPageTrainInfomation.Location = new System.Drawing.Point(4, 22);
            this.tabPageTrainInfomation.Name = "tabPageTrainInfomation";
            this.tabPageTrainInfomation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTrainInfomation.Size = new System.Drawing.Size(331, 305);
            this.tabPageTrainInfomation.TabIndex = 1;
            this.tabPageTrainInfomation.Text = "列車情報";
            this.tabPageTrainInfomation.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTrainProperty
            // 
            this.tableLayoutPanelTrainProperty.ColumnCount = 5;
            this.tableLayoutPanelTrainProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanelTrainProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanelTrainProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanelTrainProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelTrainProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTrainProperty.Controls.Add(this.labelTrainNo, 0, 0);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.labelTrainType, 0, 1);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.labelTrainName, 0, 2);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.labelDepartingStation, 0, 3);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.labelDestinationStation, 0, 4);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.labelRemarks, 0, 5);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.textBoxTrainNo, 1, 0);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.textBoxTrainName, 1, 2);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.textBoxTrainNumber, 2, 2);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.labelTrainNumber, 3, 2);
            this.tableLayoutPanelTrainProperty.Controls.Add(this.textBoxRemarks, 1, 5);
            this.tableLayoutPanelTrainProperty.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanelTrainProperty.Name = "tableLayoutPanelTrainProperty";
            this.tableLayoutPanelTrainProperty.RowCount = 7;
            this.tableLayoutPanelTrainProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTrainProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTrainProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTrainProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTrainProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTrainProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelTrainProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTrainProperty.Size = new System.Drawing.Size(315, 263);
            this.tableLayoutPanelTrainProperty.TabIndex = 0;
            // 
            // labelTrainNo
            // 
            this.labelTrainNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTrainNo.AutoSize = true;
            this.labelTrainNo.Location = new System.Drawing.Point(3, 0);
            this.labelTrainNo.Name = "labelTrainNo";
            this.labelTrainNo.Size = new System.Drawing.Size(74, 24);
            this.labelTrainNo.TabIndex = 0;
            this.labelTrainNo.Text = "列車番号(&T)：";
            this.labelTrainNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTrainType
            // 
            this.labelTrainType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTrainType.AutoSize = true;
            this.labelTrainType.Location = new System.Drawing.Point(3, 24);
            this.labelTrainType.Name = "labelTrainType";
            this.labelTrainType.Size = new System.Drawing.Size(74, 24);
            this.labelTrainType.TabIndex = 1;
            this.labelTrainType.Text = "列車種別(&K)：";
            this.labelTrainType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTrainName
            // 
            this.labelTrainName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTrainName.AutoSize = true;
            this.labelTrainName.Location = new System.Drawing.Point(3, 48);
            this.labelTrainName.Name = "labelTrainName";
            this.labelTrainName.Size = new System.Drawing.Size(63, 24);
            this.labelTrainName.TabIndex = 2;
            this.labelTrainName.Text = "列車名(&A)：";
            this.labelTrainName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDepartingStation
            // 
            this.labelDepartingStation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDepartingStation.AutoSize = true;
            this.labelDepartingStation.Location = new System.Drawing.Point(3, 72);
            this.labelDepartingStation.Name = "labelDepartingStation";
            this.labelDepartingStation.Size = new System.Drawing.Size(35, 24);
            this.labelDepartingStation.TabIndex = 3;
            this.labelDepartingStation.Text = "発駅：";
            this.labelDepartingStation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDestinationStation
            // 
            this.labelDestinationStation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDestinationStation.AutoSize = true;
            this.labelDestinationStation.Location = new System.Drawing.Point(3, 96);
            this.labelDestinationStation.Name = "labelDestinationStation";
            this.labelDestinationStation.Size = new System.Drawing.Size(35, 24);
            this.labelDestinationStation.TabIndex = 4;
            this.labelDestinationStation.Text = "着駅：";
            this.labelDestinationStation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRemarks
            // 
            this.labelRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelRemarks.AutoSize = true;
            this.labelRemarks.Location = new System.Drawing.Point(3, 120);
            this.labelRemarks.Name = "labelRemarks";
            this.labelRemarks.Size = new System.Drawing.Size(35, 32);
            this.labelRemarks.TabIndex = 5;
            this.labelRemarks.Text = "備考：";
            this.labelRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxTrainNo
            // 
            this.textBoxTrainNo.Location = new System.Drawing.Point(99, 3);
            this.textBoxTrainNo.Name = "textBoxTrainNo";
            this.textBoxTrainNo.Size = new System.Drawing.Size(111, 19);
            this.textBoxTrainNo.TabIndex = 6;
            // 
            // textBoxTrainName
            // 
            this.textBoxTrainName.Location = new System.Drawing.Point(99, 51);
            this.textBoxTrainName.Name = "textBoxTrainName";
            this.textBoxTrainName.Size = new System.Drawing.Size(111, 19);
            this.textBoxTrainName.TabIndex = 7;
            // 
            // textBoxTrainNumber
            // 
            this.textBoxTrainNumber.Location = new System.Drawing.Point(227, 51);
            this.textBoxTrainNumber.Name = "textBoxTrainNumber";
            this.textBoxTrainNumber.Size = new System.Drawing.Size(42, 19);
            this.textBoxTrainNumber.TabIndex = 8;
            // 
            // labelTrainNumber
            // 
            this.labelTrainNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTrainNumber.AutoSize = true;
            this.labelTrainNumber.Location = new System.Drawing.Point(275, 48);
            this.labelTrainNumber.Name = "labelTrainNumber";
            this.labelTrainNumber.Size = new System.Drawing.Size(17, 24);
            this.labelTrainNumber.TabIndex = 9;
            this.labelTrainNumber.Text = "号";
            this.labelTrainNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxRemarks
            // 
            this.tableLayoutPanelTrainProperty.SetColumnSpan(this.textBoxRemarks, 3);
            this.textBoxRemarks.Location = new System.Drawing.Point(99, 123);
            this.textBoxRemarks.Name = "textBoxRemarks";
            this.textBoxRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxRemarks.Size = new System.Drawing.Size(196, 19);
            this.textBoxRemarks.TabIndex = 10;
            // 
            // FormTrainProperty
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 441);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTrainProperty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "列車のプロパティ";
            this.Load += new System.EventHandler(this.FormTrainProperty_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelButton.ResumeLayout(false);
            this.tabControlTrainProperty.ResumeLayout(false);
            this.tabPageTrainInfomation.ResumeLayout(false);
            this.tableLayoutPanelTrainProperty.ResumeLayout(false);
            this.tableLayoutPanelTrainProperty.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TabControl tabControlTrainProperty;
        private System.Windows.Forms.TabPage tabPageTrainInfomation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTrainProperty;
        private System.Windows.Forms.Label labelTrainNo;
        private System.Windows.Forms.Label labelTrainType;
        private System.Windows.Forms.Label labelTrainName;
        private System.Windows.Forms.Label labelDepartingStation;
        private System.Windows.Forms.Label labelDestinationStation;
        private System.Windows.Forms.Label labelRemarks;
        private System.Windows.Forms.TextBox textBoxTrainNo;
        private System.Windows.Forms.TextBox textBoxTrainName;
        private System.Windows.Forms.TextBox textBoxTrainNumber;
        private System.Windows.Forms.Label labelTrainNumber;
        private System.Windows.Forms.TextBox textBoxRemarks;
    }
}