using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using LibNLPCSharp.util;


namespace LibNLPCSharp.gui
{

	public partial class EditLargeTextForm : Form
	{

		public delegate bool TextValidatorDelegate(string text);

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		TextValidatorDelegate textValidator;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		private EditLargeTextForm()
		{
			InitializeComponent();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public TextValidatorDelegate TextValidator
		{
			get {
				return textValidator;
			}
			set {
				textValidator = value;
				if (textValidator != null) {
					bool b = textValidator(textBox1.Text);
					btnOkay.Enabled = b;
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static string Show(string message, string title)
		{
			return Show(message, title, null);
		}

		public static string Show(string message, string title, string defaultContent)
		{
			EditLargeTextForm form = new EditLargeTextForm();
			form.textBox1.Text = defaultContent;
			form.textBox1.SelectionStart = (defaultContent != null) ? defaultContent.Length : 0;
			form.textBox1.SelectionLength = 0;
			form.label1.Text = message;
			form.Text = title;
			if (form.ShowDialog() != DialogResult.OK) return null;
			string s = form.textBox1.Text;
			return s;
		}

		public static string Show(string message, string title, string defaultContent, TextValidatorDelegate textValidator)
		{
			EditLargeTextForm form = new EditLargeTextForm();
			form.textBox1.Text = defaultContent;
			form.textBox1.SelectionStart = (defaultContent != null) ? defaultContent.Length : 0;
			form.textBox1.SelectionLength = 0;
			form.label1.Text = message;
			form.Text = title;
			form.TextValidator = textValidator;
			if (form.ShowDialog() != DialogResult.OK) return null;
			return form.textBox1.Text;
		}

		private void btnOkay_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (TextValidator != null) {
				bool b = TextValidator(textBox1.Text);
				btnOkay.Enabled = b;
			}
		}

	}

}
