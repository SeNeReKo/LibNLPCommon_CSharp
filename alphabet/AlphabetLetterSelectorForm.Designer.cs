namespace LibNLPCSharp.alphabet
{
	partial class AlphabetLetterSelectorForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.alphabetLetterSelector1 = new AlphabetLetterSelectorControl();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.LightGray;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.alphabetLetterSelector1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(500, 166);
			this.panel1.TabIndex = 1;
			// 
			// alphabetLetterSelector1
			// 
			this.alphabetLetterSelector1.Alphabet = null;
			this.alphabetLetterSelector1.Location = new System.Drawing.Point(2, 2);
			this.alphabetLetterSelector1.Name = "alphabetLetterSelector1";
			this.alphabetLetterSelector1.Size = new System.Drawing.Size(445, 140);
			this.alphabetLetterSelector1.TabIndex = 0;
			this.alphabetLetterSelector1.Target = null;
			this.alphabetLetterSelector1.SizeChanged += new System.EventHandler(this.alphabetLetterSelector1_SizeChanged);
			// 
			// AlphabetLetterSelectorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(500, 166);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "AlphabetLetterSelectorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "AlphabetLetterSelectorForm";
			this.Deactivate += new System.EventHandler(this.AlphabetLetterSelectorForm_Deactivate);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private AlphabetLetterSelectorControl alphabetLetterSelector1;
		private System.Windows.Forms.Panel panel1;
	}
}