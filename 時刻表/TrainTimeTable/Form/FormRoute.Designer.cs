namespace TrainTimeTable
{
    partial class FormRoute
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWindowDisplayOverlapping = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWindowDisplayVertically = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWindowDisplaySideBySide = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWindowArrangeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemWindowCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.menuStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile,
            this.toolStripMenuItemWindow});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1008, 24);
            this.menuStripMain.TabIndex = 3;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFileClose,
            this.toolStripMenuItemFileSave,
            this.toolStripMenuItemFileSaveAs,
            this.toolStripMenuItemFileExport});
            this.toolStripMenuItemFile.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.toolStripMenuItemFile.MergeIndex = 0;
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(67, 20);
            this.toolStripMenuItemFile.Text = "ファイル(&F)";
            // 
            // toolStripMenuItemFileClose
            // 
            this.toolStripMenuItemFileClose.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolStripMenuItemFileClose.Name = "toolStripMenuItemFileClose";
            this.toolStripMenuItemFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemFileClose.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileClose.Text = "閉じる(&C)";
            this.toolStripMenuItemFileClose.Click += new System.EventHandler(this.toolStripMenuItemFileClose_Click);
            // 
            // toolStripMenuItemFileSave
            // 
            this.toolStripMenuItemFileSave.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolStripMenuItemFileSave.Name = "toolStripMenuItemFileSave";
            this.toolStripMenuItemFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemFileSave.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileSave.Text = "上書き保存(&S)";
            this.toolStripMenuItemFileSave.Click += new System.EventHandler(this.toolStripMenuItemFileSave_Click);
            // 
            // toolStripMenuItemFileSaveAs
            // 
            this.toolStripMenuItemFileSaveAs.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolStripMenuItemFileSaveAs.Name = "toolStripMenuItemFileSaveAs";
            this.toolStripMenuItemFileSaveAs.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileSaveAs.Text = "名前を付けて保存(&A)";
            this.toolStripMenuItemFileSaveAs.Click += new System.EventHandler(this.toolStripMenuItemFileSaveAs_Click);
            // 
            // toolStripMenuItemFileExport
            // 
            this.toolStripMenuItemFileExport.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolStripMenuItemFileExport.Name = "toolStripMenuItemFileExport";
            this.toolStripMenuItemFileExport.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileExport.Text = "エクスポート";
            this.toolStripMenuItemFileExport.Click += new System.EventHandler(this.toolStripMenuItemFileExport_Click);
            // 
            // toolStripMenuItemWindow
            // 
            this.toolStripMenuItemWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemWindowDisplayOverlapping,
            this.toolStripMenuItemWindowDisplayVertically,
            this.toolStripMenuItemWindowDisplaySideBySide,
            this.toolStripMenuItemWindowArrangeIcons,
            this.toolStripMenuItem1,
            this.toolStripMenuItemWindowCloseAll});
            this.toolStripMenuItemWindow.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.toolStripMenuItemWindow.Name = "toolStripMenuItemWindow";
            this.toolStripMenuItemWindow.Size = new System.Drawing.Size(80, 20);
            this.toolStripMenuItemWindow.Text = "ウィンドウ(&W)";
            // 
            // toolStripMenuItemWindowDisplayOverlapping
            // 
            this.toolStripMenuItemWindowDisplayOverlapping.Name = "toolStripMenuItemWindowDisplayOverlapping";
            this.toolStripMenuItemWindowDisplayOverlapping.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemWindowDisplayOverlapping.Text = "重ねて表示(&C)";
            this.toolStripMenuItemWindowDisplayOverlapping.Click += new System.EventHandler(this.toolStripMenuItemWindowDisplayOverlapping_Click);
            // 
            // toolStripMenuItemWindowDisplayVertically
            // 
            this.toolStripMenuItemWindowDisplayVertically.Name = "toolStripMenuItemWindowDisplayVertically";
            this.toolStripMenuItemWindowDisplayVertically.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemWindowDisplayVertically.Text = "上下に並べて表示(&T)";
            this.toolStripMenuItemWindowDisplayVertically.Click += new System.EventHandler(this.toolStripMenuItemWindowDisplayVertically_Click);
            // 
            // toolStripMenuItemWindowDisplaySideBySide
            // 
            this.toolStripMenuItemWindowDisplaySideBySide.Name = "toolStripMenuItemWindowDisplaySideBySide";
            this.toolStripMenuItemWindowDisplaySideBySide.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemWindowDisplaySideBySide.Text = "左右に並べて表示(&V)";
            this.toolStripMenuItemWindowDisplaySideBySide.Click += new System.EventHandler(this.toolStripMenuItemWindowDisplaySideBySide_Click);
            // 
            // toolStripMenuItemWindowArrangeIcons
            // 
            this.toolStripMenuItemWindowArrangeIcons.Name = "toolStripMenuItemWindowArrangeIcons";
            this.toolStripMenuItemWindowArrangeIcons.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemWindowArrangeIcons.Text = "アイコンの整列(&A)";
            this.toolStripMenuItemWindowArrangeIcons.Click += new System.EventHandler(this.toolStripMenuItemWindowArrangeIcons_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.MergeIndex = 4;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripMenuItemWindowCloseAll
            // 
            this.toolStripMenuItemWindowCloseAll.Name = "toolStripMenuItemWindowCloseAll";
            this.toolStripMenuItemWindowCloseAll.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemWindowCloseAll.Text = "すべて閉じる";
            this.toolStripMenuItemWindowCloseAll.Click += new System.EventHandler(this.toolStripMenuItemWindowCloseAll_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1008, 25);
            this.toolStripMain.TabIndex = 4;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Location = new System.Drawing.Point(12, 72);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.splitContainerMain.Size = new System.Drawing.Size(741, 423);
            this.splitContainerMain.SplitterDistance = 110;
            this.splitContainerMain.TabIndex = 5;
            // 
            // FormRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormRoute";
            this.Text = "路線";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRoute_Closing);
            this.Load += new System.EventHandler(this.FormRoute_Load);
            this.Resize += new System.EventHandler(this.FormRoute_Resize);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileSaveAs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileExport;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWindow;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWindowDisplayOverlapping;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWindowDisplayVertically;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWindowDisplaySideBySide;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWindowArrangeIcons;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWindowCloseAll;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileClose;
    }
}