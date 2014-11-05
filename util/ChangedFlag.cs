using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class ChangedFlag
	{

		public delegate void OnChangedDelegate(bool bIsChanged);

		public event OnChangedDelegate OnChanged;

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		int lockCount;
		bool bChanged;
		bool? bDelayedIsChanged;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public ChangedFlag()
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
				bool bFireEvent = false;
				bool bTempChanged = false;

				lock (this) {
					if (lockCount > 0) {
						bDelayedIsChanged = value;
					} else {
						if (bChanged != value) {
							bChanged = value;
							bTempChanged = bChanged;
							bFireEvent = true;
						}
					}
				}

				if (bFireEvent) {
					if (OnChanged != null) {
						OnChanged(bTempChanged);
					}
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override string ToString()
		{
			if (bChanged)
				return "changed";
			else
				return "not changed";
		}

		public void SetChanged()
		{
			IsChanged = true;
		}

		public void SetUnchanged()
		{
			IsChanged = false;
		}

		public void ResumeEvents()
		{
			bool? b = null;

			lock (this) {
				if (lockCount == 0)
					throw new Exception("ResumeEvents() called without prior call to SuppressEvents()");
				lockCount--;

				if ((lockCount == 0) && bDelayedIsChanged.HasValue) {
					b = bDelayedIsChanged.Value;
					bDelayedIsChanged = null;
				}
			}

			if (b.HasValue) {
				bChanged = b.Value;
				if (OnChanged != null) {
					OnChanged(b.Value);
				}
			}
		}

		public void SuppressEvents()
		{
			lock (this) {
				lockCount++;
			}
		}

	}

}
