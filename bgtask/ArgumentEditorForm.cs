using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using LibNLPCSharp.util;


namespace LibNLPCSharp.bgtask
{

	public partial class ArgumentEditorForm : Form
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private volatile bool bPreventEvents;
		private ArgumentDescription[] argumentDescription;
		private List<ArgumentEditorControl> controls;
		private PersistentProperties pp;
		private string scriptName;
		private string ppID;

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		public ArgumentEditorForm(PersistentProperties pp, string ppID, string scriptName, ArgumentDescription[] argumentDescription)
		{
			bPreventEvents = true;

			this.ppID = ppID;
			this.scriptName = scriptName;
			this.pp = pp;
			this.argumentDescription = argumentDescription;

			InitializeComponent();

			this.edtScript.Text = scriptName;
			this.controls = new List<ArgumentEditorControl>();

			int y = 0;
			foreach (ArgumentDescription ad in argumentDescription) {
				ArgumentEditorControl c = new ArgumentEditorControl(ad);
				c.Location = new Point(0, y);
				c.Size = new Size(panel1.Width, c.Height);

				c.Text = pp.GetAsStr(ppID + ".arg." + ad.ID);

				c.OnChanged += new ArgumentEditorControl.OnTextChanged(c_OnChanged);
				panel1.Controls.Add(c);
				controls.Add(c);
				y += c.Height + 3;
			}

			int delta = panel1.Height - y;
			Height -= delta;

			UpdateComponentStates();

			bPreventEvents = false;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public Argument[] Arguments
		{
			get {
				List<Argument> ret = new List<Argument>();
				foreach (ArgumentEditorControl c in controls) {
					ret.Add(new Argument(c.ArgumentDescription, c.Text));
				}
				return ret.ToArray();
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void btnOkay_Click(object sender, EventArgs e)
		{
			foreach (ArgumentEditorControl c in controls) {
				pp.Set(ppID + ".arg." + c.ArgumentDescription.ID, c.Text);
			}

			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		private void c_OnChanged(ArgumentDescription argumentDescription, string text)
		{
			if (bPreventEvents) return;

			UpdateComponentStates();
		}

		private void UpdateComponentStates()
		{
			bool b = true;
			foreach (ArgumentEditorControl c in controls) {
				if (!c.ArgumentDescription.IsValid(c.Text)) {
					b = false;
					break;
				}
			}
			btnOkay.Enabled = b;
		}

	}

}
