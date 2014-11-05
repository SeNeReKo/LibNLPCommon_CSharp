using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util.model
{

	public class ChangedFlag : DelayedEvent
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		bool bChanged;
		bool bChangedStored;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public ChangedFlag(System.Windows.Forms.Control c, int milliseconds)
			: base(c, milliseconds)
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool IsChanged
		{
			get {
				return bChanged;
			}
			set {
				if (bChanged != value) {
					bChanged = value;
					FireEvent();
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		protected override void __BeginningSuspend()
		{
			bChangedStored = bChanged;
		}

		protected override bool __CheckNeedsFiringOnResume()
		{
			return bChangedStored != bChanged;
		}

		public void SetChanged()
		{
			if (!bChanged) {
				bChanged = true;
				FireEvent();
			}
		}

		public void SetChanged(bool value)
		{
			if (bChanged != value) {
				bChanged = value;
				FireEvent();
			}
		}

		public void SetUnchanged()
		{
			if (bChanged) {
				bChanged = false;
				FireEvent();
			}
		}

	}

}
