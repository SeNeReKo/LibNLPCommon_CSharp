using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.bgtask
{

	public abstract class AbstractBackgroundTask : IBackgroundTask
	{

		public event OnBackgroundTaskDelegate OnBackgroundTaskStarted;
		public event OnBackgroundTaskDelegate OnBackgroundTaskCompleted;

		public event OnTaskProgressDelegate OnProgress;

		////////////////////////////////////////////////////////////////

		private class PProgressHelper : IProgressIndicator
		{
			private IBackgroundTask task;
			private readonly Control parent;
			private readonly OnTaskProgressDelegate d;

			public PProgressHelper(Control parent, IBackgroundTask task, OnTaskProgressDelegate d)
			{
				this.task = task;
				this.parent = parent;
				this.d = d;
			}

			public void UpdateProgress(double progress)
			{
				if (parent.InvokeRequired) {
					parent.Invoke(d, task, progress);
				} else {
					d(task, progress);
				}
			}

			public void UpdateProgress(int current, int max)
			{
				UpdateProgress(current / (double)max);
			}
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private readonly Control parent;
		private volatile EnumBackgroundTaskState state;
		protected volatile bool bTerminate;
		private System.Threading.Thread t;
		private volatile Exception error;
		private readonly PProgressHelper ph;

		private ArgumentList arguments;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AbstractBackgroundTask(Control parent, string name)
		{
			this.parent = parent;
			this.Name = name;
			this.Output = new StringBuilder();
			this.ph = new PProgressHelper(parent, this, new OnTaskProgressDelegate(__OnProgress));
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public virtual ArgumentDescription[] ArgumentDescriptions
		{
			get {
				return new ArgumentDescription[0];
			}
		}

		public Exception Error
		{
			get {
				return error;
			}
		}

		public string Name
		{
			get;
			private set;
		}

		public StringBuilder Output
		{
			get;
			private set;
		}

		public EnumBackgroundTaskState State
		{
			get {
				return state;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void __FireOnBackgroundTaskStarted()
		{
			try {
				OnBackgroundTaskStarted(this);
			} catch (Exception ee) {
			}
		}

		private void __FireOnBackgroundTaskCompleted()
		{
			try {
				OnBackgroundTaskCompleted(this);
			} catch (Exception ee) {
			}
		}

		private void __Run()
		{
			if (OnBackgroundTaskStarted != null) {
				parent.Invoke(new System.Threading.ThreadStart(__FireOnBackgroundTaskStarted));
			}

			StringWriter log = new StringWriter();

			try {
				Run(arguments, ph, log);

				state = EnumBackgroundTaskState.Completed;
			} catch (Exception ee) {
				state = EnumBackgroundTaskState.Failed;
				error = ee;
				log.WriteLine(ee.ToString());
			}

			if (OnBackgroundTaskCompleted != null) {
				parent.Invoke(new System.Threading.ThreadStart(__FireOnBackgroundTaskCompleted));
			}
		}

		public abstract void Run(ArgumentList arguments, IProgressIndicator progress, StringWriter log);

		public void Terminate()
		{
			bTerminate = true;
		}

		public void Start(ArgumentList arguments)
		{
			if (state != EnumBackgroundTaskState.None)
				throw new Exception("Invalid state: " + state);

			state = EnumBackgroundTaskState.Running;

			this.arguments = arguments;

			t = new System.Threading.Thread(new System.Threading.ThreadStart(__Run));

			t.Start();
		}

		private void __OnProgress(IBackgroundTask task, double d)
		{
			if (OnProgress != null) {
				OnProgress(task, d);
			}
		}

	}

}
