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

	public partial class SelectOptionsForm : Form
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		Option[] data;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public SelectOptionsForm(string title, string labelText, Option[] data)
		{
			InitializeComponent();

			Text = title;
			label1.Text = labelText;

			Options = data;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public Option[] Options
		{
			get {
				if (data == null) {
					Option[] ret = new Option[0];
					return ret;
				} else {
					Option[] ret = new Option[data.Length];
					for (int i = 0; i < data.Length; i++) {
						ret[i] = new Option(data[i].Key, data[i].Description, checkedListBox1.GetItemCheckState(i) == CheckState.Checked);
					}
					return ret;
				}
			}
			set {
				checkedListBox1.Items.Clear();
				if (value == null) {
					data = null;
				} else {
					data = value;
					for (int i = 0; i < data.Length; i++) {
						checkedListBox1.Items.Add(data[i].Description);
						checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, data[i].IsChecked);
					}
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

	}

}
