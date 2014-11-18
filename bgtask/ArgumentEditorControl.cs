using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.bgtask
{

	[DefaultEvent("OnChanged")]
	public partial class ArgumentEditorControl : UserControl
	{

		public delegate void OnTextChanged(ArgumentDescription argumentDescription, string text);

		public event OnTextChanged OnChanged;

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private ArgumentDescription argumentDescription;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public ArgumentEditorControl()
		{
			InitializeComponent();
		}

		public ArgumentEditorControl(ArgumentDescription argumentDescription)
		{
			InitializeComponent();

			this.ArgumentDescription = argumentDescription;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public Argument Argument
		{
			get {
				return new Argument(argumentDescription, textBox1.Text);
			}
		}

		public ArgumentDescription ArgumentDescription
		{
			get {
				return argumentDescription;
			}
			set {
				this.argumentDescription = value;
				if (value != null) {
					label3.Text = value.Description;
				} else {
					label3.Text = "????";
				}
			}
		}

		public override string Text
		{
			get {
				return textBox1.Text;
			}
			set {
				textBox1.Text = value;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (OnChanged != null) {
				OnChanged(argumentDescription, textBox1.Text);
			}
		}

	}

}
