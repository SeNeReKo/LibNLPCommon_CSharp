using System;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace LibNLPCSharp.gui
{

	public static class GUIToolkit
	{

		[Flags]
		public enum EnumLocationResult : int
		{
			None = 0,
			ToRight = 1,
			ToLeft = 2,
			ToDown = 4,
			ToUp = 8,
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private static Bitmap tempImage;
		private static Graphics tempG;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		static GUIToolkit()
		{
			tempImage = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			tempG = Graphics.FromImage(tempImage);
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static Point FindGoodScreenLocation(Control parentControl, Point mousePoint, Size size)
		{
			EnumLocationResult locationResult;
			return FindGoodScreenLocation(parentControl, mousePoint, size, out locationResult);
		}

		public static Point FindGoodScreenLocation(Control parentControl, Point mousePoint, Size size, out EnumLocationResult locationResult)
		{
			int x = mousePoint.X;
			int y = mousePoint.Y;
			Rectangle r = Screen.GetWorkingArea(parentControl);

			locationResult = EnumLocationResult.None;

			if (mousePoint.Y + size.Height < r.Bottom) {
				y = mousePoint.Y;
				locationResult |= EnumLocationResult.ToDown;
			} else {
				y = mousePoint.Y - size.Height;
				if (y < r.Top) y = r.Top;
				locationResult |= EnumLocationResult.ToUp;
			}

			if (mousePoint.X + size.Width < r.Right) {
				x = mousePoint.X;
				locationResult |= EnumLocationResult.ToRight;
			} else {
				x = mousePoint.X - size.Width;
				if (x < r.Left) x = r.Left;
				locationResult |= EnumLocationResult.ToLeft;
			}

			return new Point(x, y);
		}

		public static F GetForm<F>(Control c)
			where F : Form
		{
			Form f = GetForm(c);
			if (f is F) return (F)f;
			throw new Exception("Form is of type " + f.GetType().Name + ", not of type " + typeof(F).Name + "!");
		}

		public static Form GetForm(Control c)
		{
			if (c == null) return null;
			if (c is Form) return (Form)c;
			return GetForm(c.Parent);
		}

		public static void FillComboBox(ComboBox comboBox, string[] items)
		{
			comboBox.BeginUpdate();
			try {
				string prevName = (comboBox.SelectedIndex >= 0) ? comboBox.SelectedItem.ToString() : "";
				int selectedIndex = 0;

				comboBox.Items.Clear();
				comboBox.Items.Add("");
				if (items != null) {
					int i = 1;
					foreach (string collectionName in items) {
						if (collectionName.Equals(prevName)) selectedIndex = i;
						comboBox.Items.Add(collectionName);
						i++;
					}
				}
				comboBox.SelectedIndex = selectedIndex;
			} finally {
				comboBox.EndUpdate();
			}
		}

		public static void FillListBox(ListBox listBox, string[] items)
		{
			listBox.BeginUpdate();
			try {
				string prevName = (listBox.SelectedIndex >= 0) ? listBox.SelectedItem.ToString() : "";
				int selectedIndex = -1;

				listBox.Items.Clear();
				if (items != null) {
					int i = 0;
					foreach (string collectionName in items) {
						if (collectionName.Equals(prevName)) selectedIndex = i;
						listBox.Items.Add(collectionName);
						i++;
					}
				}
				listBox.SelectedIndex = selectedIndex;
			} finally {
				listBox.EndUpdate();
			}
		}

		public static void AddContextMenuToEndOfOtherContextMenu(ContextMenuStrip menuToModify, ContextMenuStrip menuWithItemsToAdd)
		{
			AddContextMenuToEndOfOtherContextMenu(menuToModify, menuWithItemsToAdd, menuToModify.Items.Count);
		}

		public static void AddContextMenuToEndOfOtherContextMenu(ContextMenuStrip menuToModify, ContextMenuStrip menuWithItemsToAdd, int index)
		{
			while (menuToModify.Items.Count > index) menuToModify.Items.RemoveAt(index);

			if (menuWithItemsToAdd == null) return;

			menuToModify.Items.Add(new ToolStripSeparator());
			__AddMenuItems(menuWithItemsToAdd.Items, menuToModify.Items);
		}

		private static void __AddMenuItems(ToolStripItemCollection sourceCollection, ToolStripItemCollection targetCollection)
		{
			foreach (ToolStripItem tsi in sourceCollection) {
				if (tsi is ToolStripMenuItem) {
					ToolStripMenuItem tsiOld = (ToolStripMenuItem)tsi;
					ToolStripMenuItem tsiMenu = new ToolStripMenuItem(tsiOld.Text, tsiOld.Image);
					tsiMenu.Tag = tsiOld;
					tsiMenu.BackColor = tsiOld.BackColor;
					tsiMenu.BackgroundImage = tsiOld.BackgroundImage;
					tsiMenu.BackgroundImageLayout = tsiOld.BackgroundImageLayout;
					tsiMenu.CheckOnClick = tsiOld.CheckOnClick;
					tsiMenu.CheckState = tsiOld.CheckState;
					tsiMenu.DisplayStyle = tsiOld.DisplayStyle;
					tsiMenu.Enabled = tsiOld.Enabled;
					tsiMenu.Font = tsiOld.Font;
					tsiMenu.ForeColor = tsiOld.ForeColor;
					tsiMenu.Image = tsiOld.Image;
					tsiMenu.ImageAlign = tsiOld.ImageAlign;
					tsiMenu.ImageIndex = tsiOld.ImageIndex;
					tsiMenu.ImageKey = tsiOld.ImageKey;
					tsiMenu.ImageScaling = tsiOld.ImageScaling;
					tsiMenu.ImageTransparentColor = tsiOld.ImageTransparentColor;
					tsiMenu.Margin = tsiOld.Margin;
					tsiMenu.MergeAction = tsiOld.MergeAction;
					tsiMenu.MergeIndex = tsiOld.MergeIndex;
					tsiMenu.Name = tsiOld.Name;
					tsiMenu.Padding = tsiOld.Padding;
					tsiMenu.RightToLeft = tsiOld.RightToLeft;
					tsiMenu.RightToLeftAutoMirrorImage = tsiOld.RightToLeftAutoMirrorImage;
					tsiMenu.ShortcutKeyDisplayString = tsiOld.ShortcutKeyDisplayString;
					tsiMenu.ShortcutKeys = tsiOld.ShortcutKeys;
					tsiMenu.ShowShortcutKeys = tsiOld.ShowShortcutKeys;
					tsiMenu.Size = tsiOld.Size;
					tsiMenu.TextAlign = tsiOld.TextAlign;
					tsiMenu.TextDirection = tsiOld.TextDirection;
					tsiMenu.TextImageRelation = tsiOld.TextImageRelation;
					tsiMenu.ToolTipText = tsiOld.ToolTipText;
					tsiMenu.Click += new EventHandler(toolStripItem_Click);
					targetCollection.Add(tsiMenu);
					__AddMenuItems(tsiOld.DropDownItems, tsiMenu.DropDownItems);
				}
			}
		}

		private static void toolStripItem_Click(object sender, EventArgs e)
		{
			ToolStripItem tsi = (ToolStripItem)sender;
			tsi = (ToolStripItem)(tsi.Tag);
			tsi.PerformClick();
		}

		public static void ShowErrorMessage(string message, Exception ee)
		{
			ErrorForm form = new ErrorForm(message, ee);
			form.ShowDialog();
			// MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
		}

		public static void ShowErrorMessage(string message, string details)
		{
			ErrorForm form = new ErrorForm(message, details);
			form.ShowDialog();
		}

		public static void ShowErrorMessage(string message)
		{
			MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
		}

		public static void ShowInformationMessage(string message)
		{
			MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
		}

		public static SizeF MeasureText(string text, Font font)
		{
			return tempG.MeasureString(text, font, Int32.MaxValue, StringFormat.GenericTypographic);
		}

		/*
		public static RectangleF MeasureText(string text, Font font)
		{
			RectangleF bounds = new RectangleF();
			using (GraphicsPath textPath = new GraphicsPath()) {
				textPath.AddString(
					text,
					font.FontFamily,
					(int)font.Style,
					font.Size,
					new PointF(0, 0),
					StringFormat.GenericTypographic);
				bounds = textPath.GetBounds();
			}
			return bounds;
		}
		*/

	}

}
