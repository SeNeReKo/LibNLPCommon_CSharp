using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.alphabet
{

	public partial class AlphabetLetterSelectorControl : UserControl
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		Alphabet alphabet;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AlphabetLetterSelectorControl()
		{
			InitializeComponent();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public Control Target
		{
			get;
			set;
		}

		public Alphabet Alphabet
		{
			get {
				return alphabet;
			}
			set {
				this.alphabet = value;
				Controls.Clear();
				if (value != null) {
					int btnW = 28;
					int btnH = 22;
					int posY = 0;
					for (int y = 0; y < value.Height; y++) {
						int posX = 0;
						for (int x = 0; x < value.Width; x++) {
							AlphabetLetter al = value[y, x];
							if (al == null) {
								posX += btnW;
								continue;
							}
							Button btn = new Button();
							btn.Text = al.TextLowerCase;
							btn.Click += new EventHandler(btn_Click);
							btn.Bounds = new Rectangle(posX, posY, btnW, btnH);
							Controls.Add(btn);
							posX += btnW;
						}
						posY += btnH;
					}
					int maxX = value.Width * btnW;
					Size = new Size(maxX, posY);
					MinimumSize = Size;
					MaximumSize = Size;
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void btn_Click(object sender, EventArgs e)
		{
			if (Target == null) return;
			Button btn = (Button)sender;
			string s = Target.Text;
			if (s == null) s = "";
			s += btn.Text;
			Target.Text = s;
		}

	}

}
