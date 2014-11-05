using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.alphabet
{

	public class AlphabetLetterSelectorButton : Button
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		Alphabet alphabet;
		Control target;
		AlphabetLetterSelectorForm form;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AlphabetLetterSelectorButton()
			: base()
		{
			Text = "#";

			UpdateComponentStates();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public Alphabet Alphabet
		{
			get {
				return alphabet;
			}
			set {
				this.alphabet = value;
				if (form != null) {
					if (value == null) {
						__CloseForm();
					} else {
						form.Alphabet = value;
					}
				}
				UpdateComponentStates();
			}
		}

		public Control Target
		{
			get {
				return target;
			}
			set {
				this.target = value;
				if (form != null) {
					if (value == null) {
						__CloseForm();
					} else {
						form.Target = value;
					}
				}
				UpdateComponentStates();
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		protected override void OnClick(EventArgs e)
		{
			if (target == null) return;
			if (alphabet == null) return;
			if (!Enabled) return;

			if (form == null) {
				form = new AlphabetLetterSelectorForm(target);
				form.Alphabet = alphabet;
				form.Target = target;
				Point p = target.PointToScreen(new Point(0, target.Height));
				form.Location = p;
				form.Show();
				form.FormClosed += new FormClosedEventHandler(form_FormClosed);
				Enabled = false;
			}

			UpdateComponentStates();
		}

		private void form_FormClosed(object sender, FormClosedEventArgs e)
		{
			__CloseForm();
			UpdateComponentStates();
		}

		private void UpdateComponentStates()
		{
			Enabled = (target != null) && (form == null) && (alphabet != null);
		}

		private void __CloseForm()
		{
			if (form == null) return;
			try {
				Form f = this.form;
				this.form = null;
				f.Close();
				f.Dispose();
			} catch {
			}
		}

	}

}
