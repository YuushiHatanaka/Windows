using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Font
{
    public class OpenTypeFont
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public extern static System.IntPtr CreateFont(int nHeight, int nWidth, int nEscapement,
            int nOrientation, int fnWeight, bool fdwItalic, bool fdwUnderline, bool fdwStrikeOut, int fdwCharSet,
            int fdwOutputPrecision, int fdwClipPrecision, int fdwQuality, int fdwPitchAndFamily, string lpszFace);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public extern static int TextOut(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public extern static bool DeleteObject(System.IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public extern static System.IntPtr SelectObject(System.IntPtr hObject, System.IntPtr hFont);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public extern static int SetBkMode(System.IntPtr hdc, int flag);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public extern static int SetTextColor(System.IntPtr hdc, int flag);

        public System.IntPtr mFont = System.IntPtr.Zero;
        public System.IntPtr mFontOld = System.IntPtr.Zero;
        public const int TRANSPARENT = 1;

        public void SetFont(System.IntPtr control, string fontFamily, int fontSize, int fontWeight, bool isItalic, bool isUnderlined, bool isStriked)
        {
            mFont = CreateFont(fontSize * -1, 0, 0, 0, fontWeight, isItalic, isUnderlined, isStriked, 1, 0, 0, 0, 0, fontFamily);
            mFontOld = SelectObject(control, mFont);

        }

        public void SetFont(System.IntPtr control, string fontFamily, int fontSize)
        {
            this.SetFont(control, fontFamily, fontSize, 400, false, false, false);
        }
    }
}
