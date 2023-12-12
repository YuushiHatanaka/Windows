namespace TrainTimeTable
{
    partial class FormRouteFileProperty
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageRoute = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRoute = new System.Windows.Forms.TableLayoutPanel();
            this.labelRouteName = new System.Windows.Forms.Label();
            this.labelOutboundDiagramName = new System.Windows.Forms.Label();
            this.labelInboundDiagramName = new System.Windows.Forms.Label();
            this.textBoxRouteName = new System.Windows.Forms.TextBox();
            this.textBoxOutboundDiagramName = new System.Windows.Forms.TextBox();
            this.textBoxInboundDiagramName = new System.Windows.Forms.TextBox();
            this.tabPageFontColor = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelFontColor = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelColorSettingButton = new System.Windows.Forms.TableLayoutPanel();
            this.buttonColorSetting = new System.Windows.Forms.Button();
            this.labelFontSettings = new System.Windows.Forms.Label();
            this.labelColorSetting = new System.Windows.Forms.Label();
            this.tableLayoutPanelFontSettingsButton = new System.Windows.Forms.TableLayoutPanel();
            this.buttonFontSettings = new System.Windows.Forms.Button();
            this.labellabelFontSettingsConfirmation = new System.Windows.Forms.Label();
            this.panelColorSettingConfirmation = new System.Windows.Forms.Panel();
            this.tabPageTimetableDisplay = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTimetableDisplay = new System.Windows.Forms.TableLayoutPanel();
            this.labelWidthOfStationNameField = new System.Windows.Forms.Label();
            this.labelTimetableTrainWidth = new System.Windows.Forms.Label();
            this.numericUpDownWidthOfStationNameField = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTimetableTrainWidth = new System.Windows.Forms.NumericUpDown();
            this.labelWidthOfStationNameFieldUnit = new System.Windows.Forms.Label();
            this.labelTimetableTrainWidthUnit = new System.Windows.Forms.Label();
            this.tabPageDiagramDisplay = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelDiagramDisplay = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownStandardWidthBetweenStationsInTheDiagram = new System.Windows.Forms.NumericUpDown();
            this.labelStandardWidthBetweenStationsInTheDiagram = new System.Windows.Forms.Label();
            this.labelStandardWidthBetweenStationsInTheDiagramUnit = new System.Windows.Forms.Label();
            this.labelDiagramDtartingTime = new System.Windows.Forms.Label();
            this.dateTimePickerDiagramDtartingTime = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelButton = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageRoute.SuspendLayout();
            this.tableLayoutPanelRoute.SuspendLayout();
            this.tabPageFontColor.SuspendLayout();
            this.tableLayoutPanelFontColor.SuspendLayout();
            this.tableLayoutPanelColorSettingButton.SuspendLayout();
            this.tableLayoutPanelFontSettingsButton.SuspendLayout();
            this.tabPageTimetableDisplay.SuspendLayout();
            this.tableLayoutPanelTimetableDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidthOfStationNameField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimetableTrainWidth)).BeginInit();
            this.tabPageDiagramDisplay.SuspendLayout();
            this.tableLayoutPanelDiagramDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStandardWidthBetweenStationsInTheDiagram)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageRoute);
            this.tabControlMain.Controls.Add(this.tabPageFontColor);
            this.tabControlMain.Controls.Add(this.tabPageTimetableDisplay);
            this.tabControlMain.Controls.Add(this.tabPageDiagramDisplay);
            this.tabControlMain.Location = new System.Drawing.Point(3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(424, 368);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageRoute
            // 
            this.tabPageRoute.Controls.Add(this.tableLayoutPanelRoute);
            this.tabPageRoute.Location = new System.Drawing.Point(4, 22);
            this.tabPageRoute.Name = "tabPageRoute";
            this.tabPageRoute.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRoute.Size = new System.Drawing.Size(416, 342);
            this.tabPageRoute.TabIndex = 0;
            this.tabPageRoute.Text = "路線";
            this.tabPageRoute.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRoute
            // 
            this.tableLayoutPanelRoute.ColumnCount = 2;
            this.tableLayoutPanelRoute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanelRoute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRoute.Controls.Add(this.labelRouteName, 0, 0);
            this.tableLayoutPanelRoute.Controls.Add(this.labelOutboundDiagramName, 0, 1);
            this.tableLayoutPanelRoute.Controls.Add(this.labelInboundDiagramName, 0, 2);
            this.tableLayoutPanelRoute.Controls.Add(this.textBoxRouteName, 1, 0);
            this.tableLayoutPanelRoute.Controls.Add(this.textBoxOutboundDiagramName, 1, 1);
            this.tableLayoutPanelRoute.Controls.Add(this.textBoxInboundDiagramName, 1, 2);
            this.tableLayoutPanelRoute.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanelRoute.Name = "tableLayoutPanelRoute";
            this.tableLayoutPanelRoute.RowCount = 4;
            this.tableLayoutPanelRoute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelRoute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelRoute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelRoute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRoute.Size = new System.Drawing.Size(394, 330);
            this.tableLayoutPanelRoute.TabIndex = 0;
            // 
            // labelRouteName
            // 
            this.labelRouteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelRouteName.AutoSize = true;
            this.labelRouteName.Location = new System.Drawing.Point(3, 0);
            this.labelRouteName.Name = "labelRouteName";
            this.labelRouteName.Size = new System.Drawing.Size(57, 28);
            this.labelRouteName.TabIndex = 1;
            this.labelRouteName.Text = "路線名(&A)";
            this.labelRouteName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOutboundDiagramName
            // 
            this.labelOutboundDiagramName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelOutboundDiagramName.AutoSize = true;
            this.labelOutboundDiagramName.Location = new System.Drawing.Point(3, 28);
            this.labelOutboundDiagramName.Name = "labelOutboundDiagramName";
            this.labelOutboundDiagramName.Size = new System.Drawing.Size(81, 28);
            this.labelOutboundDiagramName.TabIndex = 2;
            this.labelOutboundDiagramName.Text = "下りダイヤ名(&D)";
            this.labelOutboundDiagramName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelInboundDiagramName
            // 
            this.labelInboundDiagramName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInboundDiagramName.AutoSize = true;
            this.labelInboundDiagramName.Location = new System.Drawing.Point(3, 56);
            this.labelInboundDiagramName.Name = "labelInboundDiagramName";
            this.labelInboundDiagramName.Size = new System.Drawing.Size(80, 28);
            this.labelInboundDiagramName.TabIndex = 3;
            this.labelInboundDiagramName.Text = "上りダイヤ名(&P)";
            this.labelInboundDiagramName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxRouteName
            // 
            this.textBoxRouteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxRouteName.Location = new System.Drawing.Point(131, 3);
            this.textBoxRouteName.Name = "textBoxRouteName";
            this.textBoxRouteName.Size = new System.Drawing.Size(247, 19);
            this.textBoxRouteName.TabIndex = 4;
            // 
            // textBoxOutboundDiagramName
            // 
            this.textBoxOutboundDiagramName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxOutboundDiagramName.Location = new System.Drawing.Point(131, 31);
            this.textBoxOutboundDiagramName.Name = "textBoxOutboundDiagramName";
            this.textBoxOutboundDiagramName.Size = new System.Drawing.Size(247, 19);
            this.textBoxOutboundDiagramName.TabIndex = 5;
            // 
            // textBoxInboundDiagramName
            // 
            this.textBoxInboundDiagramName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxInboundDiagramName.Location = new System.Drawing.Point(131, 59);
            this.textBoxInboundDiagramName.Name = "textBoxInboundDiagramName";
            this.textBoxInboundDiagramName.Size = new System.Drawing.Size(247, 19);
            this.textBoxInboundDiagramName.TabIndex = 6;
            // 
            // tabPageFontColor
            // 
            this.tabPageFontColor.Controls.Add(this.tableLayoutPanelFontColor);
            this.tabPageFontColor.Location = new System.Drawing.Point(4, 22);
            this.tabPageFontColor.Name = "tabPageFontColor";
            this.tabPageFontColor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFontColor.Size = new System.Drawing.Size(416, 342);
            this.tabPageFontColor.TabIndex = 1;
            this.tabPageFontColor.Text = "フォント・色";
            this.tabPageFontColor.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelFontColor
            // 
            this.tableLayoutPanelFontColor.ColumnCount = 2;
            this.tableLayoutPanelFontColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelFontColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelFontColor.Controls.Add(this.tableLayoutPanelColorSettingButton, 0, 6);
            this.tableLayoutPanelFontColor.Controls.Add(this.labelFontSettings, 0, 0);
            this.tableLayoutPanelFontColor.Controls.Add(this.labelColorSetting, 0, 4);
            this.tableLayoutPanelFontColor.Controls.Add(this.tableLayoutPanelFontSettingsButton, 0, 2);
            this.tableLayoutPanelFontColor.Controls.Add(this.labellabelFontSettingsConfirmation, 1, 0);
            this.tableLayoutPanelFontColor.Controls.Add(this.panelColorSettingConfirmation, 1, 4);
            this.tableLayoutPanelFontColor.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanelFontColor.Name = "tableLayoutPanelFontColor";
            this.tableLayoutPanelFontColor.RowCount = 7;
            this.tableLayoutPanelFontColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelFontColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelFontColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelFontColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFontColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelFontColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelFontColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelFontColor.Size = new System.Drawing.Size(394, 319);
            this.tableLayoutPanelFontColor.TabIndex = 1;
            // 
            // tableLayoutPanelColorSettingButton
            // 
            this.tableLayoutPanelColorSettingButton.ColumnCount = 2;
            this.tableLayoutPanelColorSettingButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelColorSettingButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanelColorSettingButton.Controls.Add(this.buttonColorSetting, 1, 0);
            this.tableLayoutPanelColorSettingButton.Location = new System.Drawing.Point(3, 289);
            this.tableLayoutPanelColorSettingButton.Name = "tableLayoutPanelColorSettingButton";
            this.tableLayoutPanelColorSettingButton.RowCount = 1;
            this.tableLayoutPanelColorSettingButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelColorSettingButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelColorSettingButton.Size = new System.Drawing.Size(154, 26);
            this.tableLayoutPanelColorSettingButton.TabIndex = 4;
            // 
            // buttonColorSetting
            // 
            this.buttonColorSetting.Location = new System.Drawing.Point(61, 3);
            this.buttonColorSetting.Name = "buttonColorSetting";
            this.buttonColorSetting.Size = new System.Drawing.Size(75, 20);
            this.buttonColorSetting.TabIndex = 2;
            this.buttonColorSetting.Text = "変更(&A)";
            this.buttonColorSetting.UseVisualStyleBackColor = true;
            this.buttonColorSetting.Click += new System.EventHandler(this.buttonColorSetting_Click);
            // 
            // labelFontSettings
            // 
            this.labelFontSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFontSettings.Location = new System.Drawing.Point(3, 0);
            this.labelFontSettings.Name = "labelFontSettings";
            this.labelFontSettings.Size = new System.Drawing.Size(100, 28);
            this.labelFontSettings.TabIndex = 1;
            this.labelFontSettings.Text = "フォント設定(&F)：";
            this.labelFontSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelColorSetting
            // 
            this.labelColorSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelColorSetting.Location = new System.Drawing.Point(3, 169);
            this.labelColorSetting.Name = "labelColorSetting";
            this.labelColorSetting.Size = new System.Drawing.Size(100, 28);
            this.labelColorSetting.TabIndex = 2;
            this.labelColorSetting.Text = "色設定(&C)：";
            this.labelColorSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanelFontSettingsButton
            // 
            this.tableLayoutPanelFontSettingsButton.ColumnCount = 2;
            this.tableLayoutPanelFontSettingsButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFontSettingsButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanelFontSettingsButton.Controls.Add(this.buttonFontSettings, 1, 0);
            this.tableLayoutPanelFontSettingsButton.Location = new System.Drawing.Point(3, 120);
            this.tableLayoutPanelFontSettingsButton.Name = "tableLayoutPanelFontSettingsButton";
            this.tableLayoutPanelFontSettingsButton.RowCount = 1;
            this.tableLayoutPanelFontSettingsButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFontSettingsButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelFontSettingsButton.Size = new System.Drawing.Size(154, 26);
            this.tableLayoutPanelFontSettingsButton.TabIndex = 3;
            // 
            // buttonFontSettings
            // 
            this.buttonFontSettings.Location = new System.Drawing.Point(61, 3);
            this.buttonFontSettings.Name = "buttonFontSettings";
            this.buttonFontSettings.Size = new System.Drawing.Size(75, 20);
            this.buttonFontSettings.TabIndex = 1;
            this.buttonFontSettings.Text = "変更(&H)";
            this.buttonFontSettings.UseVisualStyleBackColor = true;
            this.buttonFontSettings.Click += new System.EventHandler(this.buttonFontSettings_Click);
            // 
            // labellabelFontSettingsConfirmation
            // 
            this.labellabelFontSettingsConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labellabelFontSettingsConfirmation.AutoSize = true;
            this.labellabelFontSettingsConfirmation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labellabelFontSettingsConfirmation.Location = new System.Drawing.Point(200, 0);
            this.labellabelFontSettingsConfirmation.Name = "labellabelFontSettingsConfirmation";
            this.tableLayoutPanelFontColor.SetRowSpan(this.labellabelFontSettingsConfirmation, 3);
            this.labellabelFontSettingsConfirmation.Size = new System.Drawing.Size(73, 149);
            this.labellabelFontSettingsConfirmation.TabIndex = 5;
            this.labellabelFontSettingsConfirmation.Text = "路線10 13:00";
            this.labellabelFontSettingsConfirmation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelColorSettingConfirmation
            // 
            this.panelColorSettingConfirmation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorSettingConfirmation.Location = new System.Drawing.Point(200, 172);
            this.panelColorSettingConfirmation.Name = "panelColorSettingConfirmation";
            this.tableLayoutPanelFontColor.SetRowSpan(this.panelColorSettingConfirmation, 3);
            this.panelColorSettingConfirmation.Size = new System.Drawing.Size(73, 140);
            this.panelColorSettingConfirmation.TabIndex = 6;
            // 
            // tabPageTimetableDisplay
            // 
            this.tabPageTimetableDisplay.Controls.Add(this.tableLayoutPanelTimetableDisplay);
            this.tabPageTimetableDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabPageTimetableDisplay.Name = "tabPageTimetableDisplay";
            this.tabPageTimetableDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTimetableDisplay.Size = new System.Drawing.Size(416, 342);
            this.tabPageTimetableDisplay.TabIndex = 2;
            this.tabPageTimetableDisplay.Text = "時刻表画面";
            this.tabPageTimetableDisplay.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTimetableDisplay
            // 
            this.tableLayoutPanelTimetableDisplay.ColumnCount = 4;
            this.tableLayoutPanelTimetableDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanelTimetableDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanelTimetableDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanelTimetableDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTimetableDisplay.Controls.Add(this.labelWidthOfStationNameField, 0, 0);
            this.tableLayoutPanelTimetableDisplay.Controls.Add(this.labelTimetableTrainWidth, 0, 1);
            this.tableLayoutPanelTimetableDisplay.Controls.Add(this.numericUpDownWidthOfStationNameField, 1, 0);
            this.tableLayoutPanelTimetableDisplay.Controls.Add(this.numericUpDownTimetableTrainWidth, 1, 1);
            this.tableLayoutPanelTimetableDisplay.Controls.Add(this.labelWidthOfStationNameFieldUnit, 2, 0);
            this.tableLayoutPanelTimetableDisplay.Controls.Add(this.labelTimetableTrainWidthUnit, 2, 1);
            this.tableLayoutPanelTimetableDisplay.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanelTimetableDisplay.Name = "tableLayoutPanelTimetableDisplay";
            this.tableLayoutPanelTimetableDisplay.RowCount = 3;
            this.tableLayoutPanelTimetableDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelTimetableDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelTimetableDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTimetableDisplay.Size = new System.Drawing.Size(394, 318);
            this.tableLayoutPanelTimetableDisplay.TabIndex = 2;
            // 
            // labelWidthOfStationNameField
            // 
            this.labelWidthOfStationNameField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelWidthOfStationNameField.AutoSize = true;
            this.labelWidthOfStationNameField.Location = new System.Drawing.Point(3, 0);
            this.labelWidthOfStationNameField.Name = "labelWidthOfStationNameField";
            this.labelWidthOfStationNameField.Size = new System.Drawing.Size(78, 28);
            this.labelWidthOfStationNameField.TabIndex = 4;
            this.labelWidthOfStationNameField.Text = "駅名欄の幅(&S)";
            this.labelWidthOfStationNameField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTimetableTrainWidth
            // 
            this.labelTimetableTrainWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTimetableTrainWidth.AutoSize = true;
            this.labelTimetableTrainWidth.Location = new System.Drawing.Point(3, 28);
            this.labelTimetableTrainWidth.Name = "labelTimetableTrainWidth";
            this.labelTimetableTrainWidth.Size = new System.Drawing.Size(112, 28);
            this.labelTimetableTrainWidth.TabIndex = 5;
            this.labelTimetableTrainWidth.Text = "時刻表の列車の幅(&T)";
            this.labelTimetableTrainWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownWidthOfStationNameField
            // 
            this.numericUpDownWidthOfStationNameField.Location = new System.Drawing.Point(131, 3);
            this.numericUpDownWidthOfStationNameField.Name = "numericUpDownWidthOfStationNameField";
            this.numericUpDownWidthOfStationNameField.Size = new System.Drawing.Size(66, 19);
            this.numericUpDownWidthOfStationNameField.TabIndex = 6;
            // 
            // numericUpDownTimetableTrainWidth
            // 
            this.numericUpDownTimetableTrainWidth.Location = new System.Drawing.Point(131, 31);
            this.numericUpDownTimetableTrainWidth.Name = "numericUpDownTimetableTrainWidth";
            this.numericUpDownTimetableTrainWidth.Size = new System.Drawing.Size(66, 19);
            this.numericUpDownTimetableTrainWidth.TabIndex = 7;
            // 
            // labelWidthOfStationNameFieldUnit
            // 
            this.labelWidthOfStationNameFieldUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelWidthOfStationNameFieldUnit.AutoSize = true;
            this.labelWidthOfStationNameFieldUnit.Location = new System.Drawing.Point(203, 0);
            this.labelWidthOfStationNameFieldUnit.Name = "labelWidthOfStationNameFieldUnit";
            this.labelWidthOfStationNameFieldUnit.Size = new System.Drawing.Size(65, 28);
            this.labelWidthOfStationNameFieldUnit.TabIndex = 8;
            this.labelWidthOfStationNameFieldUnit.Text = "文字（全角）";
            this.labelWidthOfStationNameFieldUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTimetableTrainWidthUnit
            // 
            this.labelTimetableTrainWidthUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTimetableTrainWidthUnit.AutoSize = true;
            this.labelTimetableTrainWidthUnit.Location = new System.Drawing.Point(203, 28);
            this.labelTimetableTrainWidthUnit.Name = "labelTimetableTrainWidthUnit";
            this.labelTimetableTrainWidthUnit.Size = new System.Drawing.Size(65, 28);
            this.labelTimetableTrainWidthUnit.TabIndex = 9;
            this.labelTimetableTrainWidthUnit.Text = "文字（半角）";
            this.labelTimetableTrainWidthUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageDiagramDisplay
            // 
            this.tabPageDiagramDisplay.Controls.Add(this.tableLayoutPanelDiagramDisplay);
            this.tabPageDiagramDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiagramDisplay.Name = "tabPageDiagramDisplay";
            this.tabPageDiagramDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiagramDisplay.Size = new System.Drawing.Size(416, 342);
            this.tabPageDiagramDisplay.TabIndex = 3;
            this.tabPageDiagramDisplay.Text = "ダイヤグラム画面";
            this.tabPageDiagramDisplay.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelDiagramDisplay
            // 
            this.tableLayoutPanelDiagramDisplay.ColumnCount = 4;
            this.tableLayoutPanelDiagramDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.tableLayoutPanelDiagramDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanelDiagramDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanelDiagramDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDiagramDisplay.Controls.Add(this.numericUpDownStandardWidthBetweenStationsInTheDiagram, 1, 1);
            this.tableLayoutPanelDiagramDisplay.Controls.Add(this.labelStandardWidthBetweenStationsInTheDiagram, 0, 1);
            this.tableLayoutPanelDiagramDisplay.Controls.Add(this.labelStandardWidthBetweenStationsInTheDiagramUnit, 2, 1);
            this.tableLayoutPanelDiagramDisplay.Controls.Add(this.labelDiagramDtartingTime, 0, 0);
            this.tableLayoutPanelDiagramDisplay.Controls.Add(this.dateTimePickerDiagramDtartingTime, 1, 0);
            this.tableLayoutPanelDiagramDisplay.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanelDiagramDisplay.Name = "tableLayoutPanelDiagramDisplay";
            this.tableLayoutPanelDiagramDisplay.RowCount = 3;
            this.tableLayoutPanelDiagramDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelDiagramDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelDiagramDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDiagramDisplay.Size = new System.Drawing.Size(404, 316);
            this.tableLayoutPanelDiagramDisplay.TabIndex = 3;
            // 
            // numericUpDownStandardWidthBetweenStationsInTheDiagram
            // 
            this.numericUpDownStandardWidthBetweenStationsInTheDiagram.Location = new System.Drawing.Point(175, 31);
            this.numericUpDownStandardWidthBetweenStationsInTheDiagram.Name = "numericUpDownStandardWidthBetweenStationsInTheDiagram";
            this.numericUpDownStandardWidthBetweenStationsInTheDiagram.Size = new System.Drawing.Size(66, 19);
            this.numericUpDownStandardWidthBetweenStationsInTheDiagram.TabIndex = 1;
            // 
            // labelStandardWidthBetweenStationsInTheDiagram
            // 
            this.labelStandardWidthBetweenStationsInTheDiagram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStandardWidthBetweenStationsInTheDiagram.AutoSize = true;
            this.labelStandardWidthBetweenStationsInTheDiagram.Location = new System.Drawing.Point(3, 28);
            this.labelStandardWidthBetweenStationsInTheDiagram.Name = "labelStandardWidthBetweenStationsInTheDiagram";
            this.labelStandardWidthBetweenStationsInTheDiagram.Size = new System.Drawing.Size(155, 28);
            this.labelStandardWidthBetweenStationsInTheDiagram.TabIndex = 4;
            this.labelStandardWidthBetweenStationsInTheDiagram.Text = "ダイヤグラムの規定の駅間幅(&Y)";
            this.labelStandardWidthBetweenStationsInTheDiagram.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStandardWidthBetweenStationsInTheDiagramUnit
            // 
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.AutoSize = true;
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.Location = new System.Drawing.Point(247, 28);
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.Name = "labelStandardWidthBetweenStationsInTheDiagramUnit";
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.Size = new System.Drawing.Size(53, 28);
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.TabIndex = 6;
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.Text = "（秒相当）";
            this.labelStandardWidthBetweenStationsInTheDiagramUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDiagramDtartingTime
            // 
            this.labelDiagramDtartingTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDiagramDtartingTime.AutoSize = true;
            this.labelDiagramDtartingTime.Location = new System.Drawing.Point(3, 0);
            this.labelDiagramDtartingTime.Name = "labelDiagramDtartingTime";
            this.labelDiagramDtartingTime.Size = new System.Drawing.Size(122, 28);
            this.labelDiagramDtartingTime.TabIndex = 7;
            this.labelDiagramDtartingTime.Text = "ダイヤグラム起点時刻(&L)";
            this.labelDiagramDtartingTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerDiagramDtartingTime
            // 
            this.dateTimePickerDiagramDtartingTime.CustomFormat = "HH:mm";
            this.dateTimePickerDiagramDtartingTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerDiagramDtartingTime.Location = new System.Drawing.Point(175, 3);
            this.dateTimePickerDiagramDtartingTime.Name = "dateTimePickerDiagramDtartingTime";
            this.dateTimePickerDiagramDtartingTime.ShowUpDown = true;
            this.dateTimePickerDiagramDtartingTime.Size = new System.Drawing.Size(66, 19);
            this.dateTimePickerDiagramDtartingTime.TabIndex = 8;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelButton, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tabControlMain, 0, 0);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(440, 417);
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
            this.tableLayoutPanelButton.Location = new System.Drawing.Point(3, 388);
            this.tableLayoutPanelButton.Name = "tableLayoutPanelButton";
            this.tableLayoutPanelButton.RowCount = 1;
            this.tableLayoutPanelButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButton.Size = new System.Drawing.Size(424, 26);
            this.tableLayoutPanelButton.TabIndex = 1;
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
            // FormRouteFileProperty
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 441);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRouteFileProperty";
            this.Text = "路線ファイルのプロパティ";
            this.Load += new System.EventHandler(this.FormRouteFileProperty_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageRoute.ResumeLayout(false);
            this.tableLayoutPanelRoute.ResumeLayout(false);
            this.tableLayoutPanelRoute.PerformLayout();
            this.tabPageFontColor.ResumeLayout(false);
            this.tableLayoutPanelFontColor.ResumeLayout(false);
            this.tableLayoutPanelFontColor.PerformLayout();
            this.tableLayoutPanelColorSettingButton.ResumeLayout(false);
            this.tableLayoutPanelFontSettingsButton.ResumeLayout(false);
            this.tabPageTimetableDisplay.ResumeLayout(false);
            this.tableLayoutPanelTimetableDisplay.ResumeLayout(false);
            this.tableLayoutPanelTimetableDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidthOfStationNameField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimetableTrainWidth)).EndInit();
            this.tabPageDiagramDisplay.ResumeLayout(false);
            this.tableLayoutPanelDiagramDisplay.ResumeLayout(false);
            this.tableLayoutPanelDiagramDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStandardWidthBetweenStationsInTheDiagram)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageRoute;
        private System.Windows.Forms.TabPage tabPageFontColor;
        private System.Windows.Forms.TabPage tabPageTimetableDisplay;
        private System.Windows.Forms.TabPage tabPageDiagramDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRoute;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFontColor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTimetableDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDiagramDisplay;
        private System.Windows.Forms.Label labelRouteName;
        private System.Windows.Forms.Label labelOutboundDiagramName;
        private System.Windows.Forms.Label labelInboundDiagramName;
        private System.Windows.Forms.TextBox textBoxRouteName;
        private System.Windows.Forms.TextBox textBoxOutboundDiagramName;
        private System.Windows.Forms.TextBox textBoxInboundDiagramName;
        private System.Windows.Forms.Label labelWidthOfStationNameField;
        private System.Windows.Forms.Label labelTimetableTrainWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownWidthOfStationNameField;
        private System.Windows.Forms.NumericUpDown numericUpDownTimetableTrainWidth;
        private System.Windows.Forms.Label labelWidthOfStationNameFieldUnit;
        private System.Windows.Forms.Label labelTimetableTrainWidthUnit;
        private System.Windows.Forms.Label labelStandardWidthBetweenStationsInTheDiagram;
        private System.Windows.Forms.NumericUpDown numericUpDownStandardWidthBetweenStationsInTheDiagram;
        private System.Windows.Forms.Label labelStandardWidthBetweenStationsInTheDiagramUnit;
        private System.Windows.Forms.Label labelDiagramDtartingTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerDiagramDtartingTime;
        private System.Windows.Forms.Label labelFontSettings;
        private System.Windows.Forms.Label labelColorSetting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelColorSettingButton;
        private System.Windows.Forms.Button buttonColorSetting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFontSettingsButton;
        private System.Windows.Forms.Button buttonFontSettings;
        private System.Windows.Forms.Label labellabelFontSettingsConfirmation;
        private System.Windows.Forms.Panel panelColorSettingConfirmation;
    }
}