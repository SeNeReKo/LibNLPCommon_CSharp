using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibNLPCSharp.gui
{
	public partial class ErrorForm : Form
	{
		Exception ee;
		string details;

		public ErrorForm(string text)
			: this(text, (string)null)
		{
		}

		public ErrorForm(string text, Exception ee)
		{
			InitializeComponent();

			if (ee == null) {
				textBox1.Text = "" + (char)13 + (char)10 + text;
				btnDetails.Visible = false;
			} else {
				this.ee = ee;
				textBox1.Text = "" + (char)13 + (char)10 + text + (char)13 + (char)10 + (char)13 + (char)10 + ee.Message;
			}

			textBox1.SelectionStart = 0;
			textBox1.SelectionLength = 0;
			textBox1.WordWrap = false;
			textBox1.ScrollBars = ScrollBars.Both;
		}

		public ErrorForm(string text, string details)
		{
			InitializeComponent();

			textBox1.Text = "" + (char)13 + (char)10 + text;
			if (details == null) {
				btnDetails.Visible = false;
			} else {
				this.details = details;
			}

			textBox1.SelectionStart = 0;
			textBox1.SelectionLength = 0;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnDetails_Click(object sender, EventArgs e)
		{
			if ((ee != null) || (details != null)) {
				StringBuilder sb = new StringBuilder();

				if (details != null) {
					sb.Append(details);
				} else {
					Exception ex = ee;
					while (ex != null) {
						sb.Append(ex.ToString());
						sb.Append((char)13 + (char)10);
						sb.Append((char)13 + (char)10);
						ex = ex.InnerException;
					}
				}

				textBox1.Text = sb.ToString();
				textBox1.TextAlign = HorizontalAlignment.Left;

				textBox1.SelectionStart = 0;
				textBox1.SelectionLength = 0;

				Size size = Size;
				size.Height += 300;
				Size = size;

				btnDetails.Visible = false;
			}
		}

	}

}
