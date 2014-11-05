using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.util
{

	public class DelayedEvent<T> : IDisposable
	{

		public delegate void OnEventDelegate(T data);
		public event OnEventDelegate OnEvent;
		public event OnEventDelegate OnDelayed;

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		T data;
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

		public DelayedEvent(Control c, int milliseconds, T data)
		{
			this.c = c;
			this.milliseconds = milliseconds;
			callback = __OnTimer;
			callback2 = __OnImmediate;
			timer = new System.Threading.Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
			this.data = data;
		}

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

		public T Data
		{
			get {
				return data;
			}
			set {
				this.data = value;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void __OnTimer(object state)
		{
			if (OnDelayed != null) {
				if ((c != null) && c.InvokeRequired) {
					try {
						c.Invoke(callback, 1);
					} catch (InvalidOperationException ee) {
						Dispose();
					} catch (Exception ee) {
					}
				} else {
					OnDelayed(data);
				}
			}
		}

		private void __OnImmediate(T data)
		{
			if (OnEvent != null) {
				if ((c != null) && c.InvokeRequired) {
					try {
						c.Invoke(callback2, data);
					} catch (InvalidOperationException ee) {
						Dispose();
					} catch (Exception ee) {
					}
				} else {
					OnEvent(data);
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

		public void FireEvent(T data)
		{
			this.data = data;
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

			__OnImmediate(data);

			timer.Change(milliseconds, Timeout.Infinite);
		}

	}

}
