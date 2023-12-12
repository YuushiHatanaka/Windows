using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

namespace TrainTimeTable.Control
{
    /// <summary>
    /// DataGridView印刷クラス
    /// </summary>
    /// <see cref="http://systemconcept.co.jp/blog/?p=1085"/>
    public class DataGridViewPrinter
    {
        /// <summary>
        /// 印刷ドキュメント
        /// </summary>
        private PrintDocument m_objPrinterDoc = new PrintDocument();
        /// <summary>
        /// 印刷プレビューダイアログ
        /// </summary>
        private PrintPreviewDialog m_objPrintPreview;
        /// <summary>
        /// 印刷対象のDataGridView
        /// </summary>
        private DataGridView m_objGrid;
        /// <summary>
        /// 列幅の総和
        /// </summary>
        private int m_intWidth;
        /// <summary>
        /// 現在印刷中のページ
        /// </summary>
        private int m_intNowPage;
        /// <summary>
        /// 描画中の列インデックス
        /// </summary>
        private int m_intIndx;
        /// <summary>
        /// 表本体のフォント
        /// </summary>
        private Font m_fntFont;
        /// <summary>
        /// ヘッダーフォント
        /// </summary>
        private Font m_fntHeader;
        /// <summary>
        /// セルの枠線
        /// </summary>
        private Pen m_penCell;
        /// <summary>
        /// 印刷プレビューダイアログのアイコン
        /// </summary>
        private Icon m_icoPreview;
        /// <summary>
        /// 上のセルと値が同じ時はセルを合体するための指定文字列
        /// </summary>
        private String m_strJointUpperCell;
        /// <summary>
        /// 上のセルと値が同じ時はセルを合体するリスト
        /// </summary>
        private List<string> m_lisJointUpperCell = new List<string>();
        /// <summary>
        /// カラーで描画
        /// </summary>
        private bool m_blDrawColor = false;
        /// <summary>
        /// 幅方向の拡大率をMAX1倍までに制限
        /// </summary>
        private bool m_blLimitMaxSize = true;
        /// <summary>
        /// 幅方向の拡大率をMAX1倍までに制限
        /// </summary>
        public bool LimitMaxSize { get { return m_blLimitMaxSize; } set { m_blLimitMaxSize = value; } }
        /// <summary>
        /// カラーで描画
        /// </summary>
        public bool DrawColor { get { return m_blDrawColor; } set { m_blDrawColor = value; } }
        /// <summary>
        /// 上のセルと値が同じ時はセルを合体させる列をカンマ区切りで指定します
        /// </summary>
        public String JointUpperCell
        {
            get { return m_strJointUpperCell; }
            set
            {
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d+(,\d+)*$");
                if (rg.Match(value).Success)
                {
                    m_strJointUpperCell = value;
                    m_lisJointUpperCell.Clear();
                    m_lisJointUpperCell.AddRange(value.Split(','));
                }
            }
        }
        /// <summary>
        /// 印刷プレビューダイアログのアイコン
        /// </summary>
        public Icon PreviewDialogIcon { get { return m_icoPreview; } set { m_icoPreview = value; } }
        /// <summary>
        /// ヘッダーフォント
        /// </summary>
        public Font HeaderFont { get { return m_fntHeader; } set { m_fntHeader = value; } }
        /// <summary>
        /// 表本体のフォント
        /// </summary>
        public Font Font { get { return m_fntFont; } set { m_fntFont = value; } }
        /// <summary>
        /// ｾﾙの枠線
        /// </summary>
        public Pen PenCell { get { return m_penCell; } set { m_penCell = value; } }
        /// <summary>
        /// 印刷前処理デリゲート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void BeforePrintPageHandler(object sender, DataGridViewPrinter.BeforePrintPageEventArgs e);
        /// <summary>
        /// 印刷前処理ハンドラ
        /// </summary>
        public event BeforePrintPageHandler BeforePrintPage = null;

        /// <summary>
        /// 構築子
        /// </summary>
        /// <param name="GridView">印刷対象のDataGridView</param>
        public DataGridViewPrinter(DataGridView GridView)
        {
            this.m_objPrinterDoc.PrinterSettings.DefaultPageSettings.Margins.Left = 50;
            this.m_objPrinterDoc.PrinterSettings.DefaultPageSettings.Margins.Top = 50;
            this.m_objPrinterDoc.PrinterSettings.DefaultPageSettings.Margins.Right = 50;
            this.m_objPrinterDoc.PrinterSettings.DefaultPageSettings.Margins.Bottom = 50;
            this.m_objPrinterDoc.DefaultPageSettings = this.m_objPrinterDoc.PrinterSettings.DefaultPageSettings;
            m_objGrid = GridView;
            m_fntFont = GridView.DefaultCellStyle.Font;
            m_fntHeader = GridView.DefaultCellStyle.Font;
            this.m_objPrinterDoc.BeginPrint += PrinterDoc_BeginPrint;
            this.m_objPrinterDoc.EndPrint += PrinterDoc_EndPrint;
            this.m_objPrinterDoc.PrintPage += PrinterDoc_PrintPage;
        }
        /// <summary>
        /// プリンタ設定
        /// </summary>
        /// <param name="DocumentName">ドキュメント名</param>
        public void PrinterSetting(string DocumentName)
        {
            SubPrint(false, DocumentName);
        }
        /// <summary>
        /// ページ設定
        /// </summary>
        public void PageSetting()
        {
            PageSettings ps = (PageSettings)this.m_objPrinterDoc.DefaultPageSettings.Clone();
            PageSetupDialog frmPageSetting = new PageSetupDialog();
            frmPageSetting.PageSettings = ps;
            frmPageSetting.EnableMetric = true;
            if (frmPageSetting.ShowDialog() == DialogResult.OK)
            {
                m_objPrinterDoc.DefaultPageSettings = frmPageSetting.PageSettings;
            }
        }
        /// <summary>
        ///　印刷
        /// </summary>
        /// <param name="DocumentName">ドキュメント名</param>
        public void Print(string DocumentName)
        {
            try
            {
                SubPrint(true, DocumentName);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("DataGridViewPrinter.Print: Exception:" + e.Message);
                System.Diagnostics.Trace.WriteLine(e.StackTrace);
            }
        }
        /// <summary>
        /// 印刷プレビュー
        /// </summary>
        /// <param name="DocumentName">ドキュメント名</param>
        public void Preview(string DocumentName)
        {
            this.m_objPrinterDoc.DocumentName = DocumentName;
            try
            {
                this.m_objPrintPreview = new PrintPreviewDialog();
                this.m_objPrintPreview.Icon = m_icoPreview;
                this.m_objPrintPreview.Document = m_objPrinterDoc;
                this.m_objPrintPreview.StartPosition = FormStartPosition.CenterScreen;
                this.m_objPrintPreview.Height = (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8);
                this.m_objPrintPreview.Width = (int)(this.m_objPrintPreview.Height / 1.41421356);
                this.m_objPrintPreview.ShowDialog();
                this.m_objPrintPreview.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("DataGridViewPrinter.Preview: Exception:" + e.Message);
                System.Diagnostics.Trace.WriteLine(e.StackTrace);
            }
        }
        /// <summary>
        /// 四角にフィットするサイズのフォントを返す
        /// </summary>
        /// <param name="g">描画対象のGraphicsオブジェクト</param>
        /// <param name="basefont">ベースフォント</param>
        /// <param name="txt">描画予定の文字列</param>
        /// <param name="sz">描画する領域の大きさ</param>
        /// <returns>フォントオブジェクト</returns>
        private Font GetJustFont(Graphics g, Font basefont, string txt, SizeF sz)
        {
            int prefferdHeight = (int)(sz.Height * 0.8);
            Font f = new Font(basefont.FontFamily, prefferdHeight, GraphicsUnit.Pixel);
            SizeF s = g.MeasureString(txt, f);
            if (s.Width < sz.Width)
            {
                return f;
            }
            else
            {
                prefferdHeight = (int)(prefferdHeight * (sz.Width * 0.98) / s.Width);
                f = new Font(basefont.FontFamily, prefferdHeight, GraphicsUnit.Pixel);
            }
            return f;
        }
        /// <summary>
        /// ビットマップとして印刷ﾍﾟｰｼﾞを受け取る
        /// </summary>
        /// <param name="PicWidth">ビットマップ(ページ)の幅</param>
        /// <param name="PicHeight">ビットマップ(ページ)の高さ</param>
        /// <param name="Resolution">解像度dpi</param>
        /// <returns></returns>
        public List<Bitmap> GetPictrues(int PicWidth, int PicHeight, int Resolution = 300)
        {
            this.m_intWidth = 0;
            this.m_intNowPage = 0;
            this.m_intIndx = 0;
            foreach (DataGridViewColumn clm in this.m_objGrid.Columns)
            {
                m_intWidth += clm.Width;
            }
            List<Bitmap> retLis = new List<Bitmap>();
            bool hasmorepages;
            if (this.m_objGrid.Rows.Count == 0)
            {
                return null;
            }
            RectangleF clrct = new RectangleF();
            RectangleF txrct = new RectangleF();
            Pen myPen;
            if (this.m_penCell == null)
            {
                myPen = new Pen(Color.Black, 1);
            }
            else
            {
                myPen = this.m_penCell;
            }
            do
            {
                int sumx = 0;
                int sumy = 0;
                bool fstrw = true;
                Bitmap bmp = new Bitmap(PicWidth, PicHeight);
                bmp.SetResolution(Resolution, Resolution);
                Graphics e = Graphics.FromImage(bmp);
                e.Clear(System.Drawing.Color.White);
                m_intNowPage += 1;
                e.PixelOffsetMode = PixelOffsetMode.Half;
                e.SmoothingMode = SmoothingMode.AntiAlias;
                double rt = (1.0 * PicWidth) / m_intWidth;
                if (m_blLimitMaxSize)
                {
                    if (rt > 1)
                    {
                        rt = 1;
                    }
                }
                int hcnt = (int)PicHeight;
                for (int i = 0; i < m_objGrid.Columns.Count; i++)
                {
                    clrct = new RectangleF(sumx, sumy, (int)(this.m_objGrid.Columns[i].Width * rt), (int)(this.m_objGrid.ColumnHeadersHeight * rt));
                    txrct = new RectangleF((int)(clrct.Left + clrct.Width * 0.01), (int)(clrct.Top + clrct.Height * 0.01), clrct.Width, clrct.Height);
                    string t = this.m_objGrid.Columns[i].HeaderText;
                    Font f = GetJustFont(e, m_fntHeader, t, new SizeF(clrct.Width, clrct.Height));
                    StringFormat sfh = new StringFormat();
                    DataGridViewContentAlignment csh = this.m_objGrid.Columns[i].HeaderCell.Style.Alignment;
                    if (csh == DataGridViewContentAlignment.NotSet)
                    {
                        csh = this.m_objGrid.ColumnHeadersDefaultCellStyle.Alignment;
                    }
                    sfh.LineAlignment = StringAlignment.Center;
                    switch (csh)
                    {
                        case DataGridViewContentAlignment.BottomLeft:
                        case DataGridViewContentAlignment.MiddleLeft:
                        case DataGridViewContentAlignment.TopLeft:
                            sfh.Alignment = StringAlignment.Near;
                            break;
                        case DataGridViewContentAlignment.BottomCenter:
                        case DataGridViewContentAlignment.MiddleCenter:
                        case DataGridViewContentAlignment.TopCenter:
                            sfh.Alignment = StringAlignment.Center;
                            break;
                        case DataGridViewContentAlignment.BottomRight:
                        case DataGridViewContentAlignment.MiddleRight:
                        case DataGridViewContentAlignment.TopRight:
                            sfh.Alignment = StringAlignment.Far;
                            break;
                    }
                    if (this.m_blDrawColor)
                    {
                        Color hdc = this.m_objGrid.Columns[i].HeaderCell.Style.BackColor;
                        Color hdtc = this.m_objGrid.Columns[i].HeaderCell.Style.ForeColor;
                        if (hdc == Color.Empty)
                        {
                            hdc = m_objGrid.ColumnHeadersDefaultCellStyle.BackColor;
                        }
                        if (hdtc == Color.Empty)
                        {
                            hdtc = m_objGrid.ColumnHeadersDefaultCellStyle.ForeColor;
                        }
                        e.FillRectangle(new SolidBrush(hdc), (int)(clrct.Left), (int)(clrct.Top), (int)(clrct.Width), (int)(clrct.Height));
                        e.DrawString(t, f, new SolidBrush(hdtc), clrct, sfh);
                    }
                    else
                    {
                        e.DrawString(t, f, Brushes.Black, txrct, sfh);
                    }
                    e.DrawRectangle(myPen, clrct.Left, clrct.Top, clrct.Width, txrct.Height);
                    sumx += (int)clrct.Width;
                }
                sumy += (int)clrct.Height;
                do
                {
                    if (m_intIndx < m_objGrid.Rows.Count)
                    {
                        int rh = (int)(m_objGrid.Rows[m_intIndx].Height * rt);
                        bool nflg;
                        bool tflg;
                        if (m_intIndx < m_objGrid.Rows.Count - 1 && sumy + rh + (int)(m_objGrid.Rows[m_intIndx + 1].Height * rt) > hcnt)
                        {
                            nflg = true;
                        }
                        else
                        {
                            nflg = false;
                        }
                        if (sumy + rh > hcnt)
                        {
                            hasmorepages = true;
                            break;
                        }
                        else
                        {
                            sumx = 0;
                            for (int i = 0; i < m_objGrid.Columns.Count; i++)
                            {
                                clrct = new RectangleF(sumx, sumy, (int)(m_objGrid.Columns[i].Width * rt), (int)(m_objGrid.Rows[m_intIndx].Height * rt));
                                txrct = new RectangleF((int)(clrct.Left + clrct.Width * 0.01), (int)(clrct.Top + clrct.Height * 0.01), clrct.Width, clrct.Height);
                                e.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                                e.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                if (m_blDrawColor)
                                {
                                    Color cc = m_objGrid.Rows[m_intIndx].Cells[i].Style.BackColor;
                                    if (cc == Color.Empty)
                                    {
                                        cc = m_objGrid.Rows[m_intIndx].DefaultCellStyle.BackColor;
                                        if (cc == Color.Empty)
                                        {
                                            cc = m_objGrid.DefaultCellStyle.BackColor;
                                        }
                                    }
                                    e.FillRectangle(new SolidBrush(cc), (int)(clrct.Left), (int)(clrct.Top), (int)(clrct.Width), (int)(clrct.Height));
                                }
                                if (m_lisJointUpperCell.Contains((i + 1).ToString()) == false || fstrw || m_intIndx == 0 ||
                                    m_objGrid.Rows[m_intIndx - 1].Cells[i].Value == null ||
                                    m_objGrid.Rows[m_intIndx].Cells[i].Value == null ||
                                    m_objGrid.Rows[m_intIndx - 1].Cells[i].Value.ToString() != m_objGrid.Rows[m_intIndx].Cells[i].Value.ToString())
                                {
                                    e.DrawLine(myPen, clrct.Left, clrct.Top, clrct.Left + clrct.Width, clrct.Top);
                                    tflg = true;
                                }
                                else
                                {
                                    tflg = false;
                                }
                                e.DrawLine(myPen, clrct.Left, clrct.Top, clrct.Left, clrct.Top + clrct.Height + (int)(0.5));
                                e.DrawLine(myPen, clrct.Left + clrct.Width, clrct.Top, clrct.Left + clrct.Width, clrct.Top + clrct.Height + (int)(0.5));
                                if (m_lisJointUpperCell.Contains((i + 1).ToString()) == false || nflg == true || m_intIndx == m_objGrid.Rows.Count - 1 ||
                                    m_objGrid.Rows[m_intIndx].Cells[i].Value == null ||
                                    m_objGrid.Rows[m_intIndx + 1].Cells[i].Value == null ||
                                    m_objGrid.Rows[m_intIndx].Cells[i].Value.ToString() != m_objGrid.Rows[m_intIndx + 1].Cells[i].Value.ToString())
                                {
                                    e.DrawLine(myPen, clrct.Left, clrct.Top + clrct.Height, clrct.Left + clrct.Width, clrct.Top + clrct.Height);
                                }

                                if (m_objGrid.Rows[m_intIndx].Cells[i].Value != null)
                                {
                                    string t = m_objGrid.Rows[m_intIndx].Cells[i].Value.ToString();
                                    Font f = GetJustFont(e, m_fntFont, t, new SizeF(clrct.Width, clrct.Height));
                                    StringFormat sf = new StringFormat();
                                    DataGridViewContentAlignment cs = m_objGrid.Rows[m_intIndx].Cells[i].Style.Alignment;
                                    if (cs == DataGridViewContentAlignment.NotSet)
                                    {
                                        cs = m_objGrid.Columns[i].DefaultCellStyle.Alignment;
                                    }
                                    sf.LineAlignment = StringAlignment.Center;
                                    switch (cs)
                                    {
                                        case DataGridViewContentAlignment.BottomLeft:
                                        case DataGridViewContentAlignment.MiddleLeft:
                                        case DataGridViewContentAlignment.TopLeft:
                                            sf.Alignment = StringAlignment.Near;
                                            break;
                                        case DataGridViewContentAlignment.BottomCenter:
                                        case DataGridViewContentAlignment.MiddleCenter:
                                        case DataGridViewContentAlignment.TopCenter:
                                            sf.Alignment = StringAlignment.Center;
                                            break;
                                        case DataGridViewContentAlignment.BottomRight:
                                        case DataGridViewContentAlignment.MiddleRight:
                                        case DataGridViewContentAlignment.TopRight:
                                            sf.Alignment = StringAlignment.Far;
                                            break;
                                    }
                                    if (tflg)
                                    {
                                        if (m_blDrawColor)
                                        {
                                            Color ctc = m_objGrid.Rows[m_intIndx].Cells[i].Style.ForeColor;
                                            if (ctc == Color.Empty)
                                            {
                                                ctc = m_objGrid.Rows[m_intIndx].DefaultCellStyle.ForeColor;
                                                if (ctc == Color.Empty)
                                                {
                                                    ctc = m_objGrid.DefaultCellStyle.ForeColor;
                                                }
                                            }
                                            e.DrawString(t, f, new SolidBrush(ctc), txrct, sf);
                                        }
                                        else
                                        {
                                            e.DrawString(t, f, Brushes.Black, txrct, sf);
                                        }
                                    }
                                }
                                sumx += (int)clrct.Width;
                            }
                            sumy += (int)clrct.Height;
                            m_intIndx += 1;
                        }
                    }
                    else
                    {
                        hasmorepages = false;
                        break;
                    }
                    fstrw = false;
                } while (true);
                retLis.Add(bmp);
            } while (hasmorepages);
            return retLis;
        }
        /// <summary>
        /// 印刷開始イベントハンドラ
	    /// </summary>
	    /// <param name="sender">送り側</param>
	    /// <param name="e">イベント情報</param>
	    private void PrinterDoc_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
		    m_intWidth=0;
		    m_intNowPage=0;
		    m_intIndx=0;
		    foreach(DataGridViewColumn clm in m_objGrid.Columns) {
                m_intWidth += clm.Width;	
		    }
	    }
	    /// <summary>
        /// 印刷終了イベントハンドラ
	    /// </summary>
        /// <param name="sender">送り側</param>
        /// <param name="e">イベント情報</param>
        private void PrinterDoc_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (e.PrintAction == System.Drawing.Printing.PrintAction.PrintToPrinter)
            {
                m_objPrintPreview.Hide();
            }
	    }
        /// <summary>
        /// 印刷またはプリンタ設定を行う
        /// </summary>
        /// <param name="IsPrint">true/false=印刷/プリンタ設定</param>
        /// <param name="DocumentName">ドキュメント名</param>
        private void SubPrint(bool IsPrint, string DocumentName)
        {
            PrinterSettings ps = (PrinterSettings)m_objPrinterDoc.PrinterSettings.Clone();
            PrintDialog frmPrint = new PrintDialog();
            frmPrint.PrinterSettings = ps;
            if (frmPrint.ShowDialog() == DialogResult.OK)
            {
                m_objPrinterDoc.PrinterSettings = frmPrint.PrinterSettings;
                if (IsPrint)
                {
                    m_objPrinterDoc.DocumentName = DocumentName;
                    m_objPrinterDoc.Print();
                }
            }
        }
	    /// <summary>
        /// ユーザーカスタマイズ用
	    /// </summary>
	    public class BeforePrintPageEventArgs : System.Drawing.Printing.PrintPageEventArgs {
		    /// <summary>
            /// グリッドビューを印刷開始する行
		    /// </summary>
		    private int m_sngVirticalStartPosition;
		    /// <summary>
            /// グリッドビューを印刷終了する行
		    /// </summary>
		    private int m_sngVirticalEndPosition;	
		    /// <summary>
		    /// ページ番号
		    /// </summary>
		    private int m_intPageNum;
		    /// <summary>
            /// ページ番号
		    /// </summary>
		    public int PageNum { get { return m_intPageNum; } }
		    /// <summary>
            /// グリッドビューを印刷開始する行
		    /// </summary>		
		    public int VirticalStartPosition { 
                get
                {
				    return m_sngVirticalStartPosition;
                }
			    set
                {
				    m_sngVirticalStartPosition = value;
				    if(m_sngVirticalStartPosition < this.MarginBounds.Top)
					    m_sngVirticalStartPosition=this.MarginBounds.Top;
				    if(m_sngVirticalStartPosition>this.MarginBounds.Bottom)
					    m_sngVirticalStartPosition=this.MarginBounds.Bottom;
			    }
		    }
		    /// <summary>
            /// グリッドビューを印刷終了する行
		    /// </summary>
		    public int VirticalEndPosition {
			    get
                {
				    return m_sngVirticalEndPosition;
			    }
			    set
                {
				    m_sngVirticalEndPosition = value;
				    if(m_sngVirticalEndPosition<this.MarginBounds.Top)
					    m_sngVirticalEndPosition=this.MarginBounds.Top;
				    if(m_sngVirticalEndPosition>this.MarginBounds.Bottom)
					    m_sngVirticalEndPosition=this.MarginBounds.Bottom;
			    }
		    }
            /// <summary>
            /// 構築子
            /// </summary>
            /// <param name="graphics">グラフィックス</param>
            /// <param name="marginBounds">バウンド領域マージン</param>
            /// <param name="pageBounds">バウンド領域</param>
            /// <param name="pageSetteings">ページ設定情報</param>
            /// <param name="pagenum">ページ数</param>
		    public BeforePrintPageEventArgs(System.Drawing.Graphics graphics, System.Drawing.Rectangle marginBounds, System.Drawing.Rectangle pageBounds, System.Drawing.Printing.PageSettings pageSetteings, int pagenum) : base(graphics, marginBounds,pageBounds,pageSetteings)
            {
			    m_sngVirticalStartPosition=marginBounds.Top;
			    m_sngVirticalEndPosition=marginBounds.Bottom;
			    m_intPageNum=pagenum;
		    }
        }
	    /// <summary>
        /// プリントページイベントハンドラ
	    /// </summary>
        /// <param name="sender">送り側</param>
        /// <param name="e">イベント情報</param>
        private void PrinterDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
		    int sumx = 0;
		    int sumy = 0;
		    bool fstrw =true;
		    RectangleF clrct = new RectangleF();
            RectangleF txrct = new RectangleF();
		    Pen myPen;
		    if (m_penCell == null)
                myPen=new Pen(System.Drawing.Color.Black, 1);
		    else
			    myPen=m_penCell;
		    if (this.m_objGrid.Rows.Count==0) return;
		    m_intNowPage += 1;
		    BeforePrintPageEventArgs bfargs = new BeforePrintPageEventArgs(e.Graphics,e.MarginBounds,e.PageBounds,e.PageSettings,m_intNowPage);
		    if(BeforePrintPage != null)
                BeforePrintPage(this, bfargs);
		    e.Graphics.PixelOffsetMode=PixelOffsetMode.Half;
		    e.Graphics.SmoothingMode=SmoothingMode.AntiAlias;
		    e.Graphics.TranslateTransform((int)(e.MarginBounds.Left),(int)(e.MarginBounds.Top));
		    double rt= (1.0 * e.MarginBounds.Width)/m_intWidth;
		    if( m_blLimitMaxSize)
            {
			    if(rt>1)
				    rt=1;
		    }
		    int hcnt =(int)(e.MarginBounds.Height);
		    if( bfargs.VirticalStartPosition>e.MarginBounds.Top)
			    sumy=bfargs.VirticalStartPosition-e.MarginBounds.Top;
		    if(bfargs.VirticalEndPosition<e.MarginBounds.Bottom)
			    hcnt-=e.MarginBounds.Bottom-bfargs.VirticalEndPosition;
		    for(int i=0; i < m_objGrid.Columns.Count; i++)
            {
			    clrct=new RectangleF(sumx, sumy,(int)(m_objGrid.Columns[i].Width*rt),(int)(m_objGrid.ColumnHeadersHeight*rt));
			    txrct=new RectangleF((int)(clrct.Left+clrct.Width*0.01),(int)(clrct.Top+clrct.Height*0.01),clrct.Width,clrct.Height);
			    string t = m_objGrid.Columns[i].HeaderText;
			    Font f = GetJustFont(e.Graphics,m_fntHeader,t,new SizeF(clrct.Width,clrct.Height));
			    StringFormat sfh = new StringFormat();
			    DataGridViewContentAlignment csh =m_objGrid.Columns[i].HeaderCell.Style.Alignment;
			    if( csh==DataGridViewContentAlignment.NotSet)
				    csh=m_objGrid.ColumnHeadersDefaultCellStyle.Alignment;
			    sfh.LineAlignment=StringAlignment.Center;
			    switch(csh)
                {
				    case DataGridViewContentAlignment.BottomLeft:
                    case DataGridViewContentAlignment.MiddleLeft:
                    case DataGridViewContentAlignment.TopLeft:
					    sfh.Alignment=StringAlignment.Near;
                        break;
				    case DataGridViewContentAlignment.BottomCenter:
                    case DataGridViewContentAlignment.MiddleCenter:
                    case DataGridViewContentAlignment.TopCenter:
					    sfh.Alignment=StringAlignment.Center;
                        break;
				    case DataGridViewContentAlignment.BottomRight:
                    case DataGridViewContentAlignment.MiddleRight:
                    case DataGridViewContentAlignment.TopRight:
					    sfh.Alignment=StringAlignment.Far;
                        break;
                }	
				if(m_blDrawColor)
                {
					Color hdc =m_objGrid.Columns[i].HeaderCell.Style.BackColor;
					Color hdtc =m_objGrid.Columns[i].HeaderCell.Style.ForeColor;
					if( hdc==Color.Empty)
						hdc=m_objGrid.ColumnHeadersDefaultCellStyle.BackColor;
					if(hdtc==Color.Empty)
						hdtc=m_objGrid.ColumnHeadersDefaultCellStyle.ForeColor;
					e.Graphics.FillRectangle(new SolidBrush(hdc),(int)(clrct.Left),(int)(clrct.Top),(int)(clrct.Width),(int)(clrct.Height));
					e.Graphics.DrawString(t,f,new SolidBrush(hdtc),clrct,sfh);
				}
                else
                {
					e.Graphics.DrawString(t,f,Brushes.Black,txrct,sfh);
				}
				e.Graphics.DrawRectangle(myPen,clrct.Left,clrct.Top,clrct.Width,txrct.Height);	
			    sumx+=(int)clrct.Width;
		    }
		    sumy+=(int)clrct.Height;
		    do
            {
			    if( m_intIndx<m_objGrid.Rows.Count)
                {
				    int rh =(int)(m_objGrid.Rows[m_intIndx].Height*rt);
				    bool nflg;
				    bool tflg;
				    if( m_intIndx<m_objGrid.Rows.Count-1 && sumy+rh+(int)(m_objGrid.Rows[m_intIndx+1].Height*rt)>hcnt)
					    nflg=true;
				    else
					    nflg=false;
				    if(sumy+rh>hcnt)
                    {
					    e.HasMorePages=true;
					    break;
				    }
                    else
                    {
					    sumx=0;
					    for(int i=0; i < m_objGrid.Columns.Count; i++) {
						    clrct=new RectangleF(sumx,sumy,(int)(m_objGrid.Columns[i].Width*rt),(int)(m_objGrid.Rows[m_intIndx].Height*rt));
						    txrct=new RectangleF((int)(clrct.Left+clrct.Width*0.01),(int)(clrct.Top+clrct.Height*0.01),clrct.Width,clrct.Height);
							e.Graphics.PixelOffsetMode=System.Drawing.Drawing2D.PixelOffsetMode.Half;
							e.Graphics.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
							if( m_blDrawColor)
                            {
								Color cc = m_objGrid.Rows[m_intIndx].Cells[i].Style.BackColor;
								if( cc==Color.Empty) {
									cc=m_objGrid.Rows[m_intIndx].DefaultCellStyle.BackColor;
									if( cc==Color.Empty)
										cc=m_objGrid.DefaultCellStyle.BackColor;
								}
								e.Graphics.FillRectangle(new SolidBrush(cc),(int)(clrct.Left),(int)(clrct.Top),(int)(clrct.Width),(int)(clrct.Height));
							}
							if( m_lisJointUpperCell.Contains((i+1).ToString())==false || fstrw || m_intIndx==0 ||
								m_objGrid.Rows[m_intIndx-1].Cells[i].Value == null ||
								m_objGrid.Rows[m_intIndx].Cells[i].Value== null ||
                                m_objGrid.Rows[m_intIndx-1].Cells[i].Value.ToString() != m_objGrid.Rows[m_intIndx].Cells[i].Value.ToString())
                            {
								e.Graphics.DrawLine(myPen,clrct.Left,clrct.Top,clrct.Left+clrct.Width,clrct.Top);
								tflg=true;
                            }
                            else
                            {
								tflg=false;
                            }
                            e.Graphics.DrawLine(myPen,clrct.Left,clrct.Top,clrct.Left,clrct.Top+clrct.Height+(int)(0.5));
							e.Graphics.DrawLine(myPen,clrct.Left+clrct.Width,clrct.Top,clrct.Left+clrct.Width,clrct.Top+clrct.Height+(int)(0.5));
							if(m_lisJointUpperCell.Contains((i+1).ToString())==false || nflg==true || m_intIndx==m_objGrid.Rows.Count-1 ||
                                m_objGrid.Rows[m_intIndx].Cells[i].Value == null ||
							    m_objGrid.Rows[m_intIndx+1].Cells[i].Value == null ||
								m_objGrid.Rows[m_intIndx].Cells[i].Value.ToString()!=m_objGrid.Rows[m_intIndx+1].Cells[i].Value.ToString())
                            {
								e.Graphics.DrawLine(myPen,clrct.Left,clrct.Top+clrct.Height,clrct.Left+clrct.Width,clrct.Top+clrct.Height);					
                            }
							if(m_objGrid.Rows[m_intIndx].Cells[i].Value != null)
                            {
								string t = m_objGrid.Rows[m_intIndx].Cells[i].Value.ToString();
								Font f =GetJustFont(e.Graphics,m_fntFont,t,new SizeF(clrct.Width,clrct.Height));
								StringFormat sf = new StringFormat();
								DataGridViewContentAlignment cs=m_objGrid.Rows[m_intIndx].Cells[i].Style.Alignment;
								if( cs==DataGridViewContentAlignment.NotSet)
									cs=m_objGrid.Columns[i].DefaultCellStyle.Alignment;
								sf.LineAlignment=StringAlignment.Center;
								switch( cs) {
									case DataGridViewContentAlignment.BottomLeft:
                                    case DataGridViewContentAlignment.MiddleLeft:
                                    case DataGridViewContentAlignment.TopLeft:
										sf.Alignment=StringAlignment.Near;
                                        break;
									case DataGridViewContentAlignment.BottomCenter:
                                    case DataGridViewContentAlignment.MiddleCenter:
                                    case DataGridViewContentAlignment.TopCenter:
										sf.Alignment=StringAlignment.Center;
                                        break;
									case DataGridViewContentAlignment.BottomRight:
                                    case DataGridViewContentAlignment.MiddleRight:
                                    case DataGridViewContentAlignment.TopRight:
                                        sf.Alignment = StringAlignment.Far;
                                        break;
                                }
								if( tflg)
                                {
									if(m_blDrawColor) {
										Color ctc =m_objGrid.Rows[m_intIndx].Cells[i].Style.ForeColor;
										if( ctc==Color.Empty) {
											ctc=m_objGrid.Rows[m_intIndx].DefaultCellStyle.ForeColor;
											if( ctc==Color.Empty)
												ctc=m_objGrid.DefaultCellStyle.ForeColor;
										}
										e.Graphics.DrawString(t,f,new SolidBrush(ctc),txrct,sf);
									} else {
										e.Graphics.DrawString(t,f,Brushes.Black,txrct,sf);
									}
								}
							}
						    sumx+=(int)clrct.Width;
					    }
					    sumy+=(int)clrct.Height;
					    m_intIndx+=1;
				    }
			    }
                else
                {
				    e.HasMorePages=false;
				    return;
			    }
			    fstrw=false;
		    } while(true);  
	    }
    }
}
