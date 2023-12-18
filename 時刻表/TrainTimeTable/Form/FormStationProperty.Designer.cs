namespace TrainTimeTable
{
    partial class FormStationProperty
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
            this.tableLayoutPanelStationName = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxStationName = new System.Windows.Forms.TextBox();
            this.labelStationName = new System.Windows.Forms.Label();
            this.tableLayoutPanelItems = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxStationTimeFormat = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelStationTimeFormat = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonTimeFormatInboundArrivalTime = new System.Windows.Forms.RadioButton();
            this.radioButtonTimeFormatInboundDepartureAndArrival = new System.Windows.Forms.RadioButton();
            this.radioButtonTimeFormatOutboundArrivalAndDeparture = new System.Windows.Forms.RadioButton();
            this.radioButtonTimeFormatOutboundArrivalTime = new System.Windows.Forms.RadioButton();
            this.radioButtonTimeFormatDepartureAndArrival = new System.Windows.Forms.RadioButton();
            this.radioButtonTimeFormatDepartureTime = new System.Windows.Forms.RadioButton();
            this.groupBoxStationScale = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelStationScale = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonStationScaleMain = new System.Windows.Forms.RadioButton();
            this.radioButtonStationScaleGeneral = new System.Windows.Forms.RadioButton();
            this.checkBoxBorderLine = new System.Windows.Forms.CheckBox();
            this.groupBoxDiagramTrainInformation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDiagramTrainInformation = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelButton.SuspendLayout();
            this.tableLayoutPanelStationName.SuspendLayout();
            this.tableLayoutPanelItems.SuspendLayout();
            this.groupBoxStationTimeFormat.SuspendLayout();
            this.tableLayoutPanelStationTimeFormat.SuspendLayout();
            this.groupBoxStationScale.SuspendLayout();
            this.tableLayoutPanelStationScale.SuspendLayout();
            this.groupBoxDiagramTrainInformation.SuspendLayout();
            this.tableLayoutPanelDiagramTrainInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelButton, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelStationName, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelItems, 0, 1);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(2, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(458, 417);
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
            this.tableLayoutPanelButton.Size = new System.Drawing.Size(424, 26);
            this.tableLayoutPanelButton.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(329, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 20);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(231, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 20);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tableLayoutPanelStationName
            // 
            this.tableLayoutPanelStationName.ColumnCount = 3;
            this.tableLayoutPanelStationName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanelStationName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 256F));
            this.tableLayoutPanelStationName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationName.Controls.Add(this.textBoxStationName, 1, 0);
            this.tableLayoutPanelStationName.Controls.Add(this.labelStationName, 0, 0);
            this.tableLayoutPanelStationName.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelStationName.Name = "tableLayoutPanelStationName";
            this.tableLayoutPanelStationName.RowCount = 1;
            this.tableLayoutPanelStationName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationName.Size = new System.Drawing.Size(447, 26);
            this.tableLayoutPanelStationName.TabIndex = 3;
            // 
            // textBoxStationName
            // 
            this.textBoxStationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxStationName.Location = new System.Drawing.Point(99, 3);
            this.textBoxStationName.Name = "textBoxStationName";
            this.textBoxStationName.Size = new System.Drawing.Size(247, 19);
            this.textBoxStationName.TabIndex = 5;
            // 
            // labelStationName
            // 
            this.labelStationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStationName.AutoSize = true;
            this.labelStationName.Location = new System.Drawing.Point(3, 0);
            this.labelStationName.Name = "labelStationName";
            this.labelStationName.Size = new System.Drawing.Size(51, 26);
            this.labelStationName.TabIndex = 2;
            this.labelStationName.Text = "駅名(&A)：";
            this.labelStationName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanelItems
            // 
            this.tableLayoutPanelItems.ColumnCount = 3;
            this.tableLayoutPanelItems.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33533F));
            this.tableLayoutPanelItems.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33233F));
            this.tableLayoutPanelItems.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33233F));
            this.tableLayoutPanelItems.Controls.Add(this.groupBoxStationTimeFormat, 0, 0);
            this.tableLayoutPanelItems.Controls.Add(this.groupBoxStationScale, 1, 0);
            this.tableLayoutPanelItems.Controls.Add(this.checkBoxBorderLine, 1, 1);
            this.tableLayoutPanelItems.Controls.Add(this.groupBoxDiagramTrainInformation, 2, 0);
            this.tableLayoutPanelItems.Location = new System.Drawing.Point(3, 35);
            this.tableLayoutPanelItems.Name = "tableLayoutPanelItems";
            this.tableLayoutPanelItems.RowCount = 3;
            this.tableLayoutPanelItems.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelItems.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelItems.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelItems.Size = new System.Drawing.Size(447, 347);
            this.tableLayoutPanelItems.TabIndex = 4;
            // 
            // groupBoxStationTimeFormat
            // 
            this.groupBoxStationTimeFormat.Controls.Add(this.tableLayoutPanelStationTimeFormat);
            this.groupBoxStationTimeFormat.Location = new System.Drawing.Point(3, 3);
            this.groupBoxStationTimeFormat.Name = "groupBoxStationTimeFormat";
            this.tableLayoutPanelItems.SetRowSpan(this.groupBoxStationTimeFormat, 2);
            this.groupBoxStationTimeFormat.Size = new System.Drawing.Size(143, 179);
            this.groupBoxStationTimeFormat.TabIndex = 0;
            this.groupBoxStationTimeFormat.TabStop = false;
            this.groupBoxStationTimeFormat.Text = "駅時刻形式";
            // 
            // tableLayoutPanelStationTimeFormat
            // 
            this.tableLayoutPanelStationTimeFormat.ColumnCount = 1;
            this.tableLayoutPanelStationTimeFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationTimeFormat.Controls.Add(this.radioButtonTimeFormatInboundArrivalTime, 0, 3);
            this.tableLayoutPanelStationTimeFormat.Controls.Add(this.radioButtonTimeFormatInboundDepartureAndArrival, 0, 5);
            this.tableLayoutPanelStationTimeFormat.Controls.Add(this.radioButtonTimeFormatOutboundArrivalAndDeparture, 0, 4);
            this.tableLayoutPanelStationTimeFormat.Controls.Add(this.radioButtonTimeFormatOutboundArrivalTime, 0, 2);
            this.tableLayoutPanelStationTimeFormat.Controls.Add(this.radioButtonTimeFormatDepartureAndArrival, 0, 1);
            this.tableLayoutPanelStationTimeFormat.Controls.Add(this.radioButtonTimeFormatDepartureTime, 0, 0);
            this.tableLayoutPanelStationTimeFormat.Location = new System.Drawing.Point(6, 18);
            this.tableLayoutPanelStationTimeFormat.Name = "tableLayoutPanelStationTimeFormat";
            this.tableLayoutPanelStationTimeFormat.RowCount = 7;
            this.tableLayoutPanelStationTimeFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTimeFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTimeFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTimeFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTimeFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTimeFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationTimeFormat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationTimeFormat.Size = new System.Drawing.Size(123, 157);
            this.tableLayoutPanelStationTimeFormat.TabIndex = 1;
            // 
            // radioButtonTimeFormatInboundArrivalTime
            // 
            this.radioButtonTimeFormatInboundArrivalTime.AutoSize = true;
            this.radioButtonTimeFormatInboundArrivalTime.Location = new System.Drawing.Point(3, 75);
            this.radioButtonTimeFormatInboundArrivalTime.Name = "radioButtonTimeFormatInboundArrivalTime";
            this.radioButtonTimeFormatInboundArrivalTime.Size = new System.Drawing.Size(79, 16);
            this.radioButtonTimeFormatInboundArrivalTime.TabIndex = 4;
            this.radioButtonTimeFormatInboundArrivalTime.Text = "上り着時刻";
            this.radioButtonTimeFormatInboundArrivalTime.UseVisualStyleBackColor = true;
            // 
            // radioButtonTimeFormatInboundDepartureAndArrival
            // 
            this.radioButtonTimeFormatInboundDepartureAndArrival.AutoSize = true;
            this.radioButtonTimeFormatInboundDepartureAndArrival.Location = new System.Drawing.Point(3, 123);
            this.radioButtonTimeFormatInboundDepartureAndArrival.Name = "radioButtonTimeFormatInboundDepartureAndArrival";
            this.radioButtonTimeFormatInboundDepartureAndArrival.Size = new System.Drawing.Size(67, 16);
            this.radioButtonTimeFormatInboundDepartureAndArrival.TabIndex = 1;
            this.radioButtonTimeFormatInboundDepartureAndArrival.Text = "上り発着";
            this.radioButtonTimeFormatInboundDepartureAndArrival.UseVisualStyleBackColor = true;
            // 
            // radioButtonTimeFormatOutboundArrivalAndDeparture
            // 
            this.radioButtonTimeFormatOutboundArrivalAndDeparture.AutoSize = true;
            this.radioButtonTimeFormatOutboundArrivalAndDeparture.Location = new System.Drawing.Point(3, 99);
            this.radioButtonTimeFormatOutboundArrivalAndDeparture.Name = "radioButtonTimeFormatOutboundArrivalAndDeparture";
            this.radioButtonTimeFormatOutboundArrivalAndDeparture.Size = new System.Drawing.Size(67, 16);
            this.radioButtonTimeFormatOutboundArrivalAndDeparture.TabIndex = 1;
            this.radioButtonTimeFormatOutboundArrivalAndDeparture.Text = "下り発着";
            this.radioButtonTimeFormatOutboundArrivalAndDeparture.UseVisualStyleBackColor = true;
            // 
            // radioButtonTimeFormatOutboundArrivalTime
            // 
            this.radioButtonTimeFormatOutboundArrivalTime.AutoSize = true;
            this.radioButtonTimeFormatOutboundArrivalTime.Location = new System.Drawing.Point(3, 51);
            this.radioButtonTimeFormatOutboundArrivalTime.Name = "radioButtonTimeFormatOutboundArrivalTime";
            this.radioButtonTimeFormatOutboundArrivalTime.Size = new System.Drawing.Size(79, 16);
            this.radioButtonTimeFormatOutboundArrivalTime.TabIndex = 3;
            this.radioButtonTimeFormatOutboundArrivalTime.Text = "下り着時刻";
            this.radioButtonTimeFormatOutboundArrivalTime.UseVisualStyleBackColor = true;
            // 
            // radioButtonTimeFormatDepartureAndArrival
            // 
            this.radioButtonTimeFormatDepartureAndArrival.AutoSize = true;
            this.radioButtonTimeFormatDepartureAndArrival.Location = new System.Drawing.Point(3, 27);
            this.radioButtonTimeFormatDepartureAndArrival.Name = "radioButtonTimeFormatDepartureAndArrival";
            this.radioButtonTimeFormatDepartureAndArrival.Size = new System.Drawing.Size(47, 16);
            this.radioButtonTimeFormatDepartureAndArrival.TabIndex = 2;
            this.radioButtonTimeFormatDepartureAndArrival.Text = "発着";
            this.radioButtonTimeFormatDepartureAndArrival.UseVisualStyleBackColor = true;
            // 
            // radioButtonTimeFormatDepartureTime
            // 
            this.radioButtonTimeFormatDepartureTime.AutoSize = true;
            this.radioButtonTimeFormatDepartureTime.Checked = true;
            this.radioButtonTimeFormatDepartureTime.Location = new System.Drawing.Point(3, 3);
            this.radioButtonTimeFormatDepartureTime.Name = "radioButtonTimeFormatDepartureTime";
            this.radioButtonTimeFormatDepartureTime.Size = new System.Drawing.Size(59, 16);
            this.radioButtonTimeFormatDepartureTime.TabIndex = 1;
            this.radioButtonTimeFormatDepartureTime.TabStop = true;
            this.radioButtonTimeFormatDepartureTime.Text = "発時刻";
            this.radioButtonTimeFormatDepartureTime.UseVisualStyleBackColor = true;
            // 
            // groupBoxStationScale
            // 
            this.groupBoxStationScale.Controls.Add(this.tableLayoutPanelStationScale);
            this.groupBoxStationScale.Location = new System.Drawing.Point(152, 3);
            this.groupBoxStationScale.Name = "groupBoxStationScale";
            this.groupBoxStationScale.Size = new System.Drawing.Size(142, 133);
            this.groupBoxStationScale.TabIndex = 1;
            this.groupBoxStationScale.TabStop = false;
            this.groupBoxStationScale.Text = "駅規模";
            // 
            // tableLayoutPanelStationScale
            // 
            this.tableLayoutPanelStationScale.ColumnCount = 1;
            this.tableLayoutPanelStationScale.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationScale.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelStationScale.Controls.Add(this.radioButtonStationScaleMain, 0, 1);
            this.tableLayoutPanelStationScale.Controls.Add(this.radioButtonStationScaleGeneral, 0, 0);
            this.tableLayoutPanelStationScale.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanelStationScale.Name = "tableLayoutPanelStationScale";
            this.tableLayoutPanelStationScale.RowCount = 3;
            this.tableLayoutPanelStationScale.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationScale.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelStationScale.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStationScale.Size = new System.Drawing.Size(123, 100);
            this.tableLayoutPanelStationScale.TabIndex = 0;
            // 
            // radioButtonStationScaleMain
            // 
            this.radioButtonStationScaleMain.AutoSize = true;
            this.radioButtonStationScaleMain.Location = new System.Drawing.Point(3, 27);
            this.radioButtonStationScaleMain.Name = "radioButtonStationScaleMain";
            this.radioButtonStationScaleMain.Size = new System.Drawing.Size(59, 16);
            this.radioButtonStationScaleMain.TabIndex = 3;
            this.radioButtonStationScaleMain.Text = "主要駅";
            this.radioButtonStationScaleMain.UseVisualStyleBackColor = true;
            // 
            // radioButtonStationScaleGeneral
            // 
            this.radioButtonStationScaleGeneral.AutoSize = true;
            this.radioButtonStationScaleGeneral.Checked = true;
            this.radioButtonStationScaleGeneral.Location = new System.Drawing.Point(3, 3);
            this.radioButtonStationScaleGeneral.Name = "radioButtonStationScaleGeneral";
            this.radioButtonStationScaleGeneral.Size = new System.Drawing.Size(59, 16);
            this.radioButtonStationScaleGeneral.TabIndex = 2;
            this.radioButtonStationScaleGeneral.TabStop = true;
            this.radioButtonStationScaleGeneral.Text = "一般駅";
            this.radioButtonStationScaleGeneral.UseVisualStyleBackColor = true;
            // 
            // checkBoxBorderLine
            // 
            this.checkBoxBorderLine.AutoSize = true;
            this.checkBoxBorderLine.Location = new System.Drawing.Point(152, 164);
            this.checkBoxBorderLine.Name = "checkBoxBorderLine";
            this.checkBoxBorderLine.Size = new System.Drawing.Size(60, 16);
            this.checkBoxBorderLine.TabIndex = 2;
            this.checkBoxBorderLine.Text = "境界線";
            this.checkBoxBorderLine.UseVisualStyleBackColor = true;
            // 
            // groupBoxDiagramTrainInformation
            // 
            this.groupBoxDiagramTrainInformation.Controls.Add(this.tableLayoutPanelDiagramTrainInformation);
            this.groupBoxDiagramTrainInformation.Location = new System.Drawing.Point(300, 3);
            this.groupBoxDiagramTrainInformation.Name = "groupBoxDiagramTrainInformation";
            this.groupBoxDiagramTrainInformation.Size = new System.Drawing.Size(144, 155);
            this.groupBoxDiagramTrainInformation.TabIndex = 3;
            this.groupBoxDiagramTrainInformation.TabStop = false;
            this.groupBoxDiagramTrainInformation.Text = "ダイヤグラム列車情報";
            // 
            // tableLayoutPanelDiagramTrainInformation
            // 
            this.tableLayoutPanelDiagramTrainInformation.ColumnCount = 2;
            this.tableLayoutPanelDiagramTrainInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDiagramTrainInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDiagramTrainInformation.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelDiagramTrainInformation.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanelDiagramTrainInformation.Location = new System.Drawing.Point(6, 18);
            this.tableLayoutPanelDiagramTrainInformation.Name = "tableLayoutPanelDiagramTrainInformation";
            this.tableLayoutPanelDiagramTrainInformation.RowCount = 3;
            this.tableLayoutPanelDiagramTrainInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelDiagramTrainInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelDiagramTrainInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDiagramTrainInformation.Size = new System.Drawing.Size(132, 131);
            this.tableLayoutPanelDiagramTrainInformation.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "下り";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "上り";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormStationProperty
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 441);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStationProperty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "駅のプロパティ";
            this.Load += new System.EventHandler(this.FormStationProperty_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelButton.ResumeLayout(false);
            this.tableLayoutPanelStationName.ResumeLayout(false);
            this.tableLayoutPanelStationName.PerformLayout();
            this.tableLayoutPanelItems.ResumeLayout(false);
            this.tableLayoutPanelItems.PerformLayout();
            this.groupBoxStationTimeFormat.ResumeLayout(false);
            this.tableLayoutPanelStationTimeFormat.ResumeLayout(false);
            this.tableLayoutPanelStationTimeFormat.PerformLayout();
            this.groupBoxStationScale.ResumeLayout(false);
            this.tableLayoutPanelStationScale.ResumeLayout(false);
            this.tableLayoutPanelStationScale.PerformLayout();
            this.groupBoxDiagramTrainInformation.ResumeLayout(false);
            this.tableLayoutPanelDiagramTrainInformation.ResumeLayout(false);
            this.tableLayoutPanelDiagramTrainInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStationName;
        private System.Windows.Forms.Label labelStationName;
        private System.Windows.Forms.TextBox textBoxStationName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelItems;
        private System.Windows.Forms.GroupBox groupBoxStationTimeFormat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStationTimeFormat;
        private System.Windows.Forms.RadioButton radioButtonTimeFormatInboundArrivalTime;
        private System.Windows.Forms.RadioButton radioButtonTimeFormatOutboundArrivalTime;
        private System.Windows.Forms.RadioButton radioButtonTimeFormatDepartureAndArrival;
        private System.Windows.Forms.RadioButton radioButtonTimeFormatDepartureTime;
        private System.Windows.Forms.RadioButton radioButtonTimeFormatInboundDepartureAndArrival;
        private System.Windows.Forms.RadioButton radioButtonTimeFormatOutboundArrivalAndDeparture;
        private System.Windows.Forms.GroupBox groupBoxStationScale;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStationScale;
        private System.Windows.Forms.RadioButton radioButtonStationScaleMain;
        private System.Windows.Forms.RadioButton radioButtonStationScaleGeneral;
        private System.Windows.Forms.CheckBox checkBoxBorderLine;
        private System.Windows.Forms.GroupBox groupBoxDiagramTrainInformation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDiagramTrainInformation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}