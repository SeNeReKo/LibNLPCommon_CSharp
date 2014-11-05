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

	public partial class InfoForm : Form
	{

		public InfoForm(string text)
		{
			InitializeComponent();

			textBox1.Text = text;

			textBox1.SelectionStart = 0;
			textBox1.SelectionLength = 0;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

	}

}
