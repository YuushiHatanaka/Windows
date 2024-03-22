using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Control
{
	public partial class TextBoxConsole
	{
		private void InitializeComponent()
		{
			this.SuspendLayout();

			this.Multiline = true;
			this.ScrollBars = ScrollBars.Both;
			this.ForeColor = Color.White;
			this.BackColor = Color.Black;
            this.WordWrap = false;
            this.ReadOnly = true;
            this.Font = new Font("Consolas", 10f);
            this.KeyPress += TextBoxConsole_KeyPress;
            this.KeyDown += TextBoxConsole_KeyDown;

            this.ResumeLayout(false);
		}
	}
}
