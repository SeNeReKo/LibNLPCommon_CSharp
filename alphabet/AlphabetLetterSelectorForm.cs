using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using LibNLPCSharp.util;


namespace LibNLPCSharp.alphabet
{

	public partial class AlphabetLetterSelectorForm : Form
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AlphabetLetterSelectorForm(Control target)
		{
			InitializeComponent();

			alphabetLetterSelector1.Target = target;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public Control Target
		{
			get {
				return alphabetLetterSelector1.Target;
			}
			set {
				alphabetLetterSelector1.Target = value;
			}
		}

		public Alphabet Alphabet
		{
			get {
				return alphabetLetterSelector1.Alphabet;
			}
			set {
				alphabetLetterSelector1.Alphabet = value;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void alphabetLetterSelector1_SizeChanged(object sender, EventArgs e)
		{
			Rectangle screenRectangle = RectangleToScreen(alphabetLetterSelector1.Bounds);
			Rectangle screenRectangle2 = RectangleToScreen(ClientRectangle);
			int extraTop = screenRectangle.Top - Top;
			int extraLeft = screenRectangle.Left - Left;
			int extraBottom = Bottom - screenRectangle2.Bottom;
			int extraRight = Right - screenRectangle2.Right;

			Size extraSize = alphabetLetterSelector1.Size;
			extraSize.Width += extraLeft + extraRight + 4;
			extraSize.Height += extraTop + extraBottom + 4;
			Size = extraSize;
			MinimumSize = extraSize;
			MaximumSize = extraSize;
		}

		private void AlphabetLetterSelectorForm_Deactivate(object sender, EventArgs e)
		{
			Close();
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Escape) {
				Close();
				return true;
			} else {
				return base.ProcessDialogKey(keyData);
			}
		}

	}

}
