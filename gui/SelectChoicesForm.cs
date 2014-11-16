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

	public partial class SelectChoicesForm : Form
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		string[] data;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public SelectChoicesForm(string title, string labelText, params string[] data)
		{
			InitializeComponent();

			UpdateComponentStates();

			Text = title;
			label1.Text = labelText;

			Options = data;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string[] Options
		{
			get {
				if (data == null) {
					string[] ret = new string[0];
					return ret;
				} else {
					return data;
				}
			}
			set {
				listBox1.Items.Clear();
				if (value == null) {
					data = null;
				} else {
					data = value;
					for (int i = 0; i < data.Length; i++) {
						listBox1.SelectedIndex = -1;
						listBox1.Items.Add(data[i]);
					}
				}
			}
		}

		public string SelectedItem
		{
			get {
				if (listBox1.SelectedIndex < 0) return null;
				return data[listBox1.SelectedIndex];
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateComponentStates();
		}

		private void UpdateComponentStates()
		{
			btnOkay.Enabled = listBox1.SelectedIndex >= 0;
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			UpdateComponentStates();
			btnOkay_Click(null, null);
		}

		private void btnOkay_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

	}

}
