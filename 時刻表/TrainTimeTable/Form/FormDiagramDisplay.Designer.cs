namespace TrainTimeTable
{
    partial class FormDiagramDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiagramDisplay));
            this.panelDiagram = new System.Windows.Forms.Panel();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelHours = new System.Windows.Forms.Panel();
            this.panelStations = new System.Windows.Forms.Panel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrintPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrinterSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHorizontalReduction = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHorizontalExpansion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonVerticalReduction = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonVerticalExpansion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonVerticalAxisDirectionReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDisplayOfTrainNumber = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDisplayOfTrainName = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDisplayOfTrainTime = new System.Windows.Forms.ToolStripButton();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelInfomation = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDiagram
            // 
            this.panelDiagram.AutoScroll = true;
            this.panelDiagram.BackColor = System.Drawing.SystemColors.Window;
            this.panelDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDiagram.Location = new System.Drawing.Point(128, 32);
            this.panelDiagram.Margin = new System.Windows.Forms.Padding(0);
            this.panelDiagram.Name = "panelDiagram";
            this.panelDiagram.Size = new System.Drawing.Size(400, 293);
            this.panelDiagram.TabIndex = 0;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelHours, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.panelStations, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panelDiagram, 1, 1);
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(97, 70);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(528, 325);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // panelHours
            // 
            this.panelHours.BackColor = System.Drawing.SystemColors.Window;
            this.panelHours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHours.Location = new System.Drawing.Point(128, 0);
            this.panelHours.Margin = new System.Windows.Forms.Padding(0);
            this.panelHours.Name = "panelHours";
            this.panelHours.Size = new System.Drawing.Size(400, 32);
            this.panelHours.TabIndex = 3;
            // 
            // panelStations
            // 
            this.panelStations.BackColor = System.Drawing.SystemColors.Window;
            this.panelStations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStations.Location = new System.Drawing.Point(0, 32);
            this.panelStations.Margin = new System.Windows.Forms.Padding(0);
            this.panelStations.Name = "panelStations";
            this.panelStations.Size = new System.Drawing.Size(128, 293);
            this.panelStations.TabIndex = 2;
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPrint,
            this.toolStripButtonPrintPreview,
            this.toolStripButtonPrinterSetting,
            this.toolStripSeparator1,
            this.toolStripButtonHorizontalReduction,
            this.toolStripButtonHorizontalExpansion,
            this.toolStripButtonVerticalReduction,
            this.toolStripButtonVerticalExpansion,
            this.toolStripButtonVerticalAxisDirectionReset,
            this.toolStripSeparator2,
            this.toolStripButtonDisplayOfTrainNumber,
            this.toolStripButtonDisplayOfTrainName,
            this.toolStripButtonDisplayOfTrainTime});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(800, 25);
            this.toolStripMain.TabIndex = 2;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonPrint
            // 
            this.toolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrint.Image")));
            this.toolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrint.Name = "toolStripButtonPrint";
            this.toolStripButtonPrint.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPrint.Text = "印刷";
            this.toolStripButtonPrint.Click += new System.EventHandler(this.toolStripButtonPrint_Click);
            // 
            // toolStripButtonPrintPreview
            // 
            this.toolStripButtonPrintPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrintPreview.Image")));
            this.toolStripButtonPrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrintPreview.Name = "toolStripButtonPrintPreview";
            this.toolStripButtonPrintPreview.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPrintPreview.Text = "印刷プレビュー";
            this.toolStripButtonPrintPreview.Click += new System.EventHandler(this.toolStripButtonPrintPreview_Click);
            // 
            // toolStripButtonPrinterSetting
            // 
            this.toolStripButtonPrinterSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrinterSetting.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrinterSetting.Image")));
            this.toolStripButtonPrinterSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrinterSetting.Name = "toolStripButtonPrinterSetting";
            this.toolStripButtonPrinterSetting.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPrinterSetting.Text = "プリンターの設定";
            this.toolStripButtonPrinterSetting.Click += new System.EventHandler(this.toolStripButtonPrinterSetting_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonHorizontalReduction
            // 
            this.toolStripButtonHorizontalReduction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHorizontalReduction.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHorizontalReduction.Image")));
            this.toolStripButtonHorizontalReduction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHorizontalReduction.Name = "toolStripButtonHorizontalReduction";
            this.toolStripButtonHorizontalReduction.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHorizontalReduction.Text = "横軸方向縮小";
            this.toolStripButtonHorizontalReduction.Click += new System.EventHandler(this.toolStripButtonHorizontalReduction_Click);
            // 
            // toolStripButtonHorizontalExpansion
            // 
            this.toolStripButtonHorizontalExpansion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHorizontalExpansion.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHorizontalExpansion.Image")));
            this.toolStripButtonHorizontalExpansion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHorizontalExpansion.Name = "toolStripButtonHorizontalExpansion";
            this.toolStripButtonHorizontalExpansion.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHorizontalExpansion.Text = "横軸方向拡大";
            this.toolStripButtonHorizontalExpansion.Click += new System.EventHandler(this.toolStripButtonHorizontalExpansion_Click);
            // 
            // toolStripButtonVerticalReduction
            // 
            this.toolStripButtonVerticalReduction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonVerticalReduction.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonVerticalReduction.Image")));
            this.toolStripButtonVerticalReduction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonVerticalReduction.Name = "toolStripButtonVerticalReduction";
            this.toolStripButtonVerticalReduction.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonVerticalReduction.Text = "縦軸方向縮小";
            this.toolStripButtonVerticalReduction.Click += new System.EventHandler(this.toolStripButtonVerticalReduction_Click);
            // 
            // toolStripButtonVerticalExpansion
            // 
            this.toolStripButtonVerticalExpansion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonVerticalExpansion.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonVerticalExpansion.Image")));
            this.toolStripButtonVerticalExpansion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonVerticalExpansion.Name = "toolStripButtonVerticalExpansion";
            this.toolStripButtonVerticalExpansion.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonVerticalExpansion.Text = "縦軸方向拡大";
            this.toolStripButtonVerticalExpansion.Click += new System.EventHandler(this.toolStripButtonVerticalExpansion_Click);
            // 
            // toolStripButtonVerticalAxisDirectionReset
            // 
            this.toolStripButtonVerticalAxisDirectionReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonVerticalAxisDirectionReset.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonVerticalAxisDirectionReset.Image")));
            this.toolStripButtonVerticalAxisDirectionReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonVerticalAxisDirectionReset.Name = "toolStripButtonVerticalAxisDirectionReset";
            this.toolStripButtonVerticalAxisDirectionReset.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonVerticalAxisDirectionReset.Text = "縦軸方向リセット";
            this.toolStripButtonVerticalAxisDirectionReset.Click += new System.EventHandler(this.toolStripButtonVerticalAxisDirectionReset_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonDisplayOfTrainNumber
            // 
            this.toolStripButtonDisplayOfTrainNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDisplayOfTrainNumber.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisplayOfTrainNumber.Image")));
            this.toolStripButtonDisplayOfTrainNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisplayOfTrainNumber.Name = "toolStripButtonDisplayOfTrainNumber";
            this.toolStripButtonDisplayOfTrainNumber.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDisplayOfTrainNumber.Text = "列車番号の表示";
            this.toolStripButtonDisplayOfTrainNumber.Click += new System.EventHandler(this.toolStripButtonDisplayOfTrainNumber_Click);
            // 
            // toolStripButtonDisplayOfTrainName
            // 
            this.toolStripButtonDisplayOfTrainName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDisplayOfTrainName.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisplayOfTrainName.Image")));
            this.toolStripButtonDisplayOfTrainName.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisplayOfTrainName.Name = "toolStripButtonDisplayOfTrainName";
            this.toolStripButtonDisplayOfTrainName.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDisplayOfTrainName.Text = "列車名の表示";
            this.toolStripButtonDisplayOfTrainName.Click += new System.EventHandler(this.toolStripButtonDisplayOfTrainName_Click);
            // 
            // toolStripButtonDisplayOfTrainTime
            // 
            this.toolStripButtonDisplayOfTrainTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDisplayOfTrainTime.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisplayOfTrainTime.Image")));
            this.toolStripButtonDisplayOfTrainTime.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisplayOfTrainTime.Name = "toolStripButtonDisplayOfTrainTime";
            this.toolStripButtonDisplayOfTrainTime.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDisplayOfTrainTime.Text = "列車時刻の表示";
            this.toolStripButtonDisplayOfTrainTime.Click += new System.EventHandler(this.toolStripButtonDisplayOfTrainTime_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelInfomation});
            this.statusStripMain.Location = new System.Drawing.Point(0, 428);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(800, 22);
            this.statusStripMain.TabIndex = 3;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelInfomation
            // 
            this.toolStripStatusLabelInfomation.Name = "toolStripStatusLabelInfomation";
            this.toolStripStatusLabelInfomation.Size = new System.Drawing.Size(37, 17);
            this.toolStripStatusLabelInfomation.Text = "xxxxx";
            // 
            // FormDiagramDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.statusStripMain);
            this.Name = "FormDiagramDisplay";
            this.Text = "ダイヤグラム";
            this.Load += new System.EventHandler(this.FormDiagramDisplay_Load);
            this.Resize += new System.EventHandler(this.FormDiagramDisplay_Resize);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDiagram;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Panel panelStations;
        private System.Windows.Forms.Panel panelHours;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonHorizontalReduction;
        private System.Windows.Forms.ToolStripButton toolStripButtonHorizontalExpansion;
        private System.Windows.Forms.ToolStripButton toolStripButtonVerticalReduction;
        private System.Windows.Forms.ToolStripButton toolStripButtonVerticalExpansion;
        private System.Windows.Forms.ToolStripButton toolStripButtonVerticalAxisDirectionReset;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisplayOfTrainNumber;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisplayOfTrainName;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisplayOfTrainTime;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrinterSetting;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrintPreview;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelInfomation;
    }
}