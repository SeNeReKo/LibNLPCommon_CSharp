using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.gui
{

	[DefaultEvent("OnChanged")]
	public sealed partial class SelectOptionsControl : UserControl
	{

		public delegate void OnChangedDelegate(SelectOptionsControl source);
		public event OnChangedDelegate OnChanged;

		public enum EnumCheckMode
		{
			Multiple,
			Single,
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		bool bSkipEvents;
		Option[] data;
		EnumCheckMode checkMode;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public SelectOptionsControl()
		{
			InitializeComponent();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public EnumCheckMode CheckMode
		{
			get {
				return checkMode;
			}
			set {
				if (checkMode != value) {
					checkMode = value;
					if (value == EnumCheckMode.Single) {
						int i = 0;
						bool bChanged = false;
						bool bClear = false;
						while (i < checkedListBox1.Items.Count) {
							if (bClear) {
								if (checkedListBox1.GetItemCheckState(i) != CheckState.Checked) {
									bChanged = true;
									checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
								}
							} else {
								if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked) {
									bClear = true;
								}
							}
							i++;
						}
						if (bChanged)
							FireOnChanged();
					}
				}
			}
		}

		public Option[] SelectedOptions
		{
			get {
				List<Option> ret = new List<Option>();
				for (int i = 0; i < checkedListBox1.Items.Count; i++) {
					if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked) {
						ret.Add(new Option(data[i].Key, data[i].Description, true));
					}
				}
				return ret.ToArray();
			}
		}

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
				bSkipEvents = true;

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

				bSkipEvents = false;
				FireOnChanged();
			}
		}

		private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (bSkipEvents) return;

			if (checkMode == EnumCheckMode.Single) {
				if (e.NewValue == CheckState.Checked) {
					bSkipEvents = true;
					__UncheckAllBut(e.Index);
					checkedListBox1.SetItemCheckState(e.Index, CheckState.Checked);
					bSkipEvents = false;
					FireOnChanged();
				}
			}
		}

		private bool __UncheckAllBut(int index)
		{
			bool bChanged = false;
			for (int i = 0; i < checkedListBox1.Items.Count; i++) {
				if (i != index) {
					if (checkedListBox1.GetItemCheckState(i) != CheckState.Unchecked) {
						checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
						bChanged = true;
					}
				}
			}
			return bChanged;
		}

		private void FireOnChanged()
		{
			if (OnChanged != null) {
				OnChanged(this);
			}
		}

	}

}
