namespace TrainTimeTable
{
    partial class FormStationTimeProperty
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
            this.tableLayoutPanelStationTime = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxStationTreatment = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelStationTreatment = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonNoService = new System.Windows.Forms.RadioButton();
            this.radioButtonStop = new System.Windows.Forms.RadioButton();
            this.radioButtonPassing = new System.Windows.Forms.RadioButton();
            this.radioButtonNoRoute = new System.Windows.Forms.RadioButton();
            this.labelArrivalTime = new System.Windows.Forms.Label();
            this.labelDepartureTime = new System.Windows.Forms.Label();
            this.maskedTextBoxArrivalTime = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxDepartureTime = new System.Windows.Forms.MaskedTextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelButton.SuspendLayout();
            this.tableLayoutPanelStationTime.SuspendLayout();
            this.groupBoxStationTreatment.SuspendLayout();
            this.tableLayoutPanelStationTreatment.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelButton, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelStationTime, 0, 0);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(300, 157);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // tableLayoutPanelButton
            // 
            this.tableLayoutPanelButton.ColumnCount = 3;
            this.tableLayoutPanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanelButton.Controls.Add(this.buttonCancel, 2, 0);
            this.tableLayoutPanelButton.Controls.Add(this.buttonOK, 1, 0);
            this.tableLayoutPanelButton.Location = new System.Drawing.Point(3, 128);
            this.tableLayoutPanelButton.Name = "tableLayoutPanelButton";
            this.tableLayoutPanelButton.RowCount = 1;
            this.tableLayoutPanelButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButton.Size = new System.Drawing.Size(286, 26);
            this.tableLayoutPanelButton.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(191, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 20);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(93, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 20);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tableLayoutPanelStationTime
            // 
            this.tableLayoutPanelStationTime.ColumnCount = 3;
            this.tableLayoutPanelStationTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelStationTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanelStationTime.Controls.Add(this.groupBoxStationTreatment, 0, 0);
            this.tableLayoutPanelStationTime.Controls.Add(this.labelArrivalTime, 1, 1);
            this.tableLayoutPanelStationTime.Controls.Add(this.labelDepartureTime, 1, 2);
            this.tableLayoutPanelStationTime.Controls.Add(this.maskedTextBoxArrivalTime, 2, 1);
            this.tableLayoutPanelStationTime.Controls.Add(this.maskedTextBoxDepartureTime, 2, 2);
            this.tableLayoutPanelStationTime.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelStationTime.Name = "tableLayoutPanelStationTime";
            this.tableLayoutPanelStationTime.RowCount = 5;
            this.tableLayoutPanelStationTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationTime.Size = new System.Drawing.Size(289, 119);
            this.tableLayoutPanelStationTime.TabIndex = 4;
            // 
            // groupBoxStationTreatment
            // 
            this.groupBoxStationTreatment.Controls.Add(this.tableLayoutPanelStationTreatment);
            this.groupBoxStationTreatment.Location = new System.Drawing.Point(3, 3);
            this.groupBoxStationTreatment.Name = "groupBoxStationTreatment";
            this.tableLayoutPanelStationTime.SetRowSpan(this.groupBoxStationTreatment, 5);
            this.groupBoxStationTreatment.Size = new System.Drawing.Size(155, 112);
            this.groupBoxStationTreatment.TabIndex = 0;
            this.groupBoxStationTreatment.TabStop = false;
            this.groupBoxStationTreatment.Text = "駅扱い";
            // 
            // tableLayoutPanelStationTreatment
            // 
            this.tableLayoutPanelStationTreatment.ColumnCount = 1;
            this.tableLayoutPanelStationTreatment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationTreatment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelStationTreatment.Controls.Add(this.radioButtonNoRoute, 0, 3);
            this.tableLayoutPanelStationTreatment.Controls.Add(this.radioButtonPassing, 0, 2);
            this.tableLayoutPanelStationTreatment.Controls.Add(this.radioButtonStop, 0, 1);
            this.tableLayoutPanelStationTreatment.Controls.Add(this.radioButtonNoService, 0, 0);
            this.tableLayoutPanelStationTreatment.Location = new System.Drawing.Point(6, 18);
            this.tableLayoutPanelStationTreatment.Name = "tableLayoutPanelStationTreatment";
            this.tableLayoutPanelStationTreatment.RowCount = 4;
            this.tableLayoutPanelStationTreatment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelStationTreatment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelStationTreatment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelStationTreatment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelStationTreatment.Size = new System.Drawing.Size(119, 88);
            this.tableLayoutPanelStationTreatment.TabIndex = 1;
            // 
            // radioButtonNoService
            // 
            this.radioButtonNoService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonNoService.AutoSize = true;
            this.radioButtonNoService.Checked = true;
            this.radioButtonNoService.Location = new System.Drawing.Point(3, 3);
            this.radioButtonNoService.Name = "radioButtonNoService";
            this.radioButtonNoService.Size = new System.Drawing.Size(82, 16);
            this.radioButtonNoService.TabIndex = 1;
            this.radioButtonNoService.TabStop = true;
            this.radioButtonNoService.Text = "運行なし(&N)";
            this.radioButtonNoService.UseVisualStyleBackColor = true;
            // 
            // radioButtonStop
            // 
            this.radioButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonStop.AutoSize = true;
            this.radioButtonStop.Location = new System.Drawing.Point(3, 25);
            this.radioButtonStop.Name = "radioButtonStop";
            this.radioButtonStop.Size = new System.Drawing.Size(62, 16);
            this.radioButtonStop.TabIndex = 2;
            this.radioButtonStop.Text = "停車(&S)";
            this.radioButtonStop.UseVisualStyleBackColor = true;
            // 
            // radioButtonPassing
            // 
            this.radioButtonPassing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonPassing.AutoSize = true;
            this.radioButtonPassing.Location = new System.Drawing.Point(3, 47);
            this.radioButtonPassing.Name = "radioButtonPassing";
            this.radioButtonPassing.Size = new System.Drawing.Size(62, 16);
            this.radioButtonPassing.TabIndex = 3;
            this.radioButtonPassing.Text = "通過(&P)";
            this.radioButtonPassing.UseVisualStyleBackColor = true;
            // 
            // radioButtonNoRoute
            // 
            this.radioButtonNoRoute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonNoRoute.AutoSize = true;
            this.radioButtonNoRoute.Location = new System.Drawing.Point(3, 69);
            this.radioButtonNoRoute.Name = "radioButtonNoRoute";
            this.radioButtonNoRoute.Size = new System.Drawing.Size(82, 16);
            this.radioButtonNoRoute.TabIndex = 4;
            this.radioButtonNoRoute.Text = "経由なし(&V)";
            this.radioButtonNoRoute.UseVisualStyleBackColor = true;
            // 
            // labelArrivalTime
            // 
            this.labelArrivalTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelArrivalTime.AutoSize = true;
            this.labelArrivalTime.Location = new System.Drawing.Point(164, 24);
            this.labelArrivalTime.Name = "labelArrivalTime";
            this.labelArrivalTime.Size = new System.Drawing.Size(63, 24);
            this.labelArrivalTime.TabIndex = 1;
            this.labelArrivalTime.Text = "着時刻(&A)：";
            this.labelArrivalTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDepartureTime
            // 
            this.labelDepartureTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDepartureTime.AutoSize = true;
            this.labelDepartureTime.Location = new System.Drawing.Point(164, 48);
            this.labelDepartureTime.Name = "labelDepartureTime";
            this.labelDepartureTime.Size = new System.Drawing.Size(63, 24);
            this.labelDepartureTime.TabIndex = 2;
            this.labelDepartureTime.Text = "発時刻(&D)：";
            this.labelDepartureTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // maskedTextBoxArrivalTime
            // 
            this.maskedTextBoxArrivalTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.maskedTextBoxArrivalTime.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxArrivalTime.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.maskedTextBoxArrivalTime.Location = new System.Drawing.Point(244, 27);
            this.maskedTextBoxArrivalTime.Mask = "90:00";
            this.maskedTextBoxArrivalTime.Name = "maskedTextBoxArrivalTime";
            this.maskedTextBoxArrivalTime.Size = new System.Drawing.Size(42, 19);
            this.maskedTextBoxArrivalTime.TabIndex = 3;
            this.maskedTextBoxArrivalTime.ValidatingType = typeof(System.DateTime);
            // 
            // maskedTextBoxDepartureTime
            // 
            this.maskedTextBoxDepartureTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.maskedTextBoxDepartureTime.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBoxDepartureTime.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.maskedTextBoxDepartureTime.Location = new System.Drawing.Point(244, 51);
            this.maskedTextBoxDepartureTime.Mask = "90:00";
            this.maskedTextBoxDepartureTime.Name = "maskedTextBoxDepartureTime";
            this.maskedTextBoxDepartureTime.Size = new System.Drawing.Size(42, 19);
            this.maskedTextBoxDepartureTime.TabIndex = 4;
            this.maskedTextBoxDepartureTime.ValidatingType = typeof(System.DateTime);
            // 
            // FormStationTimeProperty
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 157);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStationTimeProperty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "駅時刻";
            this.Load += new System.EventHandler(this.FormStationTimeProperty_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelButton.ResumeLayout(false);
            this.tableLayoutPanelStationTime.ResumeLayout(false);
            this.tableLayoutPanelStationTime.PerformLayout();
            this.groupBoxStationTreatment.ResumeLayout(false);
            this.tableLayoutPanelStationTreatment.ResumeLayout(false);
            this.tableLayoutPanelStationTreatment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStationTime;
        private System.Windows.Forms.GroupBox groupBoxStationTreatment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStationTreatment;
        private System.Windows.Forms.RadioButton radioButtonNoRoute;
        private System.Windows.Forms.RadioButton radioButtonPassing;
        private System.Windows.Forms.RadioButton radioButtonStop;
        private System.Windows.Forms.RadioButton radioButtonNoService;
        private System.Windows.Forms.Label labelArrivalTime;
        private System.Windows.Forms.Label labelDepartureTime;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxArrivalTime;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDepartureTime;
    }
}