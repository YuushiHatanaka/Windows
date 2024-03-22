namespace TrainTimeTable
{
    partial class FormMain
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
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileCreateNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileImportWinDIA = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileImportOudia = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileImportOudia2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripMain
            // 
            this.statusStripMain.Location = new System.Drawing.Point(0, 739);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1264, 22);
            this.statusStripMain.TabIndex = 1;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile,
            this.toolStripMenuItemWindow,
            this.toolStripMenuItemHelp});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1264, 24);
            this.menuStripMain.TabIndex = 3;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFileCreateNew,
            this.toolStripMenuItemFileOpen,
            this.toolStripMenuItemFileClose,
            this.toolStripMenuItem3,
            this.toolStripMenuItemFileSave,
            this.toolStripMenuItemFileSaveAs,
            this.toolStripMenuItem2,
            this.toolStripMenuItemFileImport,
            this.toolStripMenuItemFileExport,
            this.toolStripMenuItem1,
            this.toolStripMenuItemFileExit});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(67, 20);
            this.toolStripMenuItemFile.Text = "ファイル(&F)";
            // 
            // toolStripMenuItemFileCreateNew
            // 
            this.toolStripMenuItemFileCreateNew.Name = "toolStripMenuItemFileCreateNew";
            this.toolStripMenuItemFileCreateNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.toolStripMenuItemFileCreateNew.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileCreateNew.Text = "新規作成(&N)";
            this.toolStripMenuItemFileCreateNew.Click += new System.EventHandler(this.toolStripMenuItemFileNewCreate_Click);
            // 
            // toolStripMenuItemFileOpen
            // 
            this.toolStripMenuItemFileOpen.Name = "toolStripMenuItemFileOpen";
            this.toolStripMenuItemFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.toolStripMenuItemFileOpen.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileOpen.Text = "開く(&O)";
            this.toolStripMenuItemFileOpen.Click += new System.EventHandler(this.toolStripMenuItemFileOpen_Click);
            // 
            // toolStripMenuItemFileClose
            // 
            this.toolStripMenuItemFileClose.Enabled = false;
            this.toolStripMenuItemFileClose.Name = "toolStripMenuItemFileClose";
            this.toolStripMenuItemFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemFileClose.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileClose.Text = "閉じる(&C)";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // toolStripMenuItemFileSave
            // 
            this.toolStripMenuItemFileSave.Enabled = false;
            this.toolStripMenuItemFileSave.Name = "toolStripMenuItemFileSave";
            this.toolStripMenuItemFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemFileSave.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileSave.Text = "上書き保存(&S)";
            // 
            // toolStripMenuItemFileSaveAs
            // 
            this.toolStripMenuItemFileSaveAs.Enabled = false;
            this.toolStripMenuItemFileSaveAs.Name = "toolStripMenuItemFileSaveAs";
            this.toolStripMenuItemFileSaveAs.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileSaveAs.Text = "名前を付けて保存(&A)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // toolStripMenuItemFileImport
            // 
            this.toolStripMenuItemFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFileImportWinDIA,
            this.toolStripMenuItemFileImportOudia,
            this.toolStripMenuItemFileImportOudia2});
            this.toolStripMenuItemFileImport.Name = "toolStripMenuItemFileImport";
            this.toolStripMenuItemFileImport.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileImport.Text = "インポート";
            // 
            // toolStripMenuItemFileImportWinDIA
            // 
            this.toolStripMenuItemFileImportWinDIA.Name = "toolStripMenuItemFileImportWinDIA";
            this.toolStripMenuItemFileImportWinDIA.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItemFileImportWinDIA.Text = "WinDIA形式(*.dia)";
            this.toolStripMenuItemFileImportWinDIA.Click += new System.EventHandler(this.toolStripMenuItemFileImportWinDIA_Click);
            // 
            // toolStripMenuItemFileImportOudia
            // 
            this.toolStripMenuItemFileImportOudia.Name = "toolStripMenuItemFileImportOudia";
            this.toolStripMenuItemFileImportOudia.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItemFileImportOudia.Text = "OuDia形式(*oud)";
            this.toolStripMenuItemFileImportOudia.Click += new System.EventHandler(this.toolStripMenuItemFileImportOudia_Click);
            // 
            // toolStripMenuItemFileImportOudia2
            // 
            this.toolStripMenuItemFileImportOudia2.Enabled = false;
            this.toolStripMenuItemFileImportOudia2.Name = "toolStripMenuItemFileImportOudia2";
            this.toolStripMenuItemFileImportOudia2.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItemFileImportOudia2.Text = "OuDia2形式(*oud2)";
            this.toolStripMenuItemFileImportOudia2.Click += new System.EventHandler(this.toolStripMenuItemFileImportOudia2_Click);
            // 
            // toolStripMenuItemFileExport
            // 
            this.toolStripMenuItemFileExport.Enabled = false;
            this.toolStripMenuItemFileExport.Name = "toolStripMenuItemFileExport";
            this.toolStripMenuItemFileExport.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileExport.Text = "エクスポート";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // toolStripMenuItemFileExit
            // 
            this.toolStripMenuItemFileExit.Name = "toolStripMenuItemFileExit";
            this.toolStripMenuItemFileExit.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemFileExit.Text = "終了(&X)";
            this.toolStripMenuItemFileExit.Click += new System.EventHandler(this.toolStripMenuItemFileExit_Click);
            // 
            // toolStripMenuItemWindow
            // 
            this.toolStripMenuItemWindow.Name = "toolStripMenuItemWindow";
            this.toolStripMenuItemWindow.Size = new System.Drawing.Size(80, 20);
            this.toolStripMenuItemWindow.Text = "ウィンドウ(&W)";
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemHelpAbout});
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(65, 20);
            this.toolStripMenuItemHelp.Text = "ヘルプ(&H)";
            // 
            // toolStripMenuItemHelpAbout
            // 
            this.toolStripMenuItemHelpAbout.Name = "toolStripMenuItemHelpAbout";
            this.toolStripMenuItemHelpAbout.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemHelpAbout.Text = "バージョン情報(&A)";
            this.toolStripMenuItemHelpAbout.Click += new System.EventHandler(this.toolStripMenuItemHelpAbout_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 761);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.statusStripMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.Text = "時刻表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_Closing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileCreateNew;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileImport;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWindow;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileImportWinDIA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileImportOudia;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileImportOudia2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFileClose;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
    }
}

