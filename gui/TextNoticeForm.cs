using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


using LibNLPCSharp.util;


namespace LibNLPCSharp.gui
{

	public partial class TextNoticeForm : Form
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		PersistentProperties pp;
		string id;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		private TextNoticeForm(PersistentProperties pp, string title, string message, string id)
		{
			InitializeComponent();

			this.pp = pp;
			this.Text = title;
			this.id = id;

			this.textBox1.Text = message;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static void Show(PersistentProperties pp, DirectoryInfo dir, string id)
		{
			FileInfo fi = new FileInfo(Util.AddSeparatorChar(dir.FullName) + id + ".xml");
			if (fi.Exists) Show(pp, fi);
			else GUIToolkit.ShowErrorMessage("No data for message " + id + "!", (string)null);
		}

		public static void Show(PersistentProperties pp, FileInfo file)
		{
			XmlSerializer ser = new XmlSerializer(typeof(TextNoticeIO));
			TextNoticeIO textNotice = (TextNoticeIO)(ser.Deserialize(new XmlTextReader(file.FullName)));
			if (Util.Equals(pp[textNotice.ID], "hide")) return;
			Show(pp, textNotice.Title, textNotice.Message, textNotice.ID);
		}

		public static void Show(PersistentProperties pp, string title, string message, string messageID)
		{
			TextNoticeForm form = new TextNoticeForm(pp, title, message, messageID);
			form.ShowDialog();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (checkBox1.Checked) {
				pp[id] = "hide";
				if (pp.File != null) pp.Save();
			}
			Close();
		}

	}

}
