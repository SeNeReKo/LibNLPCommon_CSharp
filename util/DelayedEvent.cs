using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.util
{

	public class DelayedEvent : IDisposable
	{

		public delegate void OnEventDelegate();
		public event OnEventDelegate OnEvent;
		public event OnEventDelegate OnEventDelayed;

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		int milliseconds;
		System.Threading.Timer timer;
		Control c;
		TimerCallback callback;
		OnEventDelegate callback2;
		int suspendCounter;
		protected bool bNeedsFiring;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public DelayedEvent(Control c, int milliseconds)
		{
			this.c = c;
			this.milliseconds = milliseconds;
			callback = __OnTimer;
			callback2 = __OnImmediate;
			timer = new System.Threading.Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
		}

		public void Dispose()
		{
			if (timer != null) {
				timer.Dispose();
				timer = null;
			}
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public int Milliseconds
		{
			get {
				return this.milliseconds;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void __OnTimer(object state)
		{
			if (OnEventDelayed != null) {
				if ((c != null) && c.InvokeRequired) {
					try {
						c.Invoke(callback, 1);
					} catch (InvalidOperationException ee) {
						Dispose();
					} catch (Exception ee) {
					}
				} else {
					OnEventDelayed();
				}
			}
		}

		private void __OnImmediate()
		{
			if (OnEvent != null) {
				if ((c != null) && c.InvokeRequired) {
					try {
						c.Invoke(callback2);
					} catch (InvalidOperationException ee) {
						Dispose();
					} catch (Exception ee) {
					}
				} else {
					OnEvent();
				}
			}
		}

		public void FireEvent()
		{
			if (suspendCounter == 0) {
				__FireEvent();
			} else {
				bNeedsFiring = true;
			}
		}

		public void SuspendEvents()
		{
			lock (this) {
				if (suspendCounter == 0) __BeginningSuspend();
				suspendCounter++;
			}
		}

		protected virtual void __BeginningSuspend()
		{
		}

		public void ResumeEvents()
		{
			lock (this) {
				if (suspendCounter == 0) throw new Exception("ResumeEvent() called without prior call to SuspendEvent()!");
				suspendCounter--;
				if ((suspendCounter == 0) && bNeedsFiring && __CheckNeedsFiringOnResume()) {
					__FireEvent();
				}
			}
		}

		protected virtual bool __CheckNeedsFiringOnResume()
		{
			return true;
		}

		private void __FireEvent()
		{
			System.Threading.Timer timer = this.timer;
			if (timer == null) return;
			bNeedsFiring = false;

			__OnImmediate();

			timer.Change(milliseconds, Timeout.Infinite);
		}

	}

}
