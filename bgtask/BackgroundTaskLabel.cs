using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using LibNLPCSharp.gui;


namespace LibNLPCSharp.bgtask
{

	public partial class BackgroundTaskLabel : UserControl
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private Color backgroundColorProgress = Color.LightSteelBlue;
		private double progress;
		private IBackgroundTask task;
		private bool bTerminating;

		private OnBackgroundTaskDelegate dOnBackgroundTaskStarted;
		private OnBackgroundTaskDelegate dOnBackgroundTaskCompleted;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public BackgroundTaskLabel()
		{
			InitializeComponent();

			dOnBackgroundTaskStarted = new OnBackgroundTaskDelegate(value_OnBackgroundTaskStarted);
			dOnBackgroundTaskCompleted = new OnBackgroundTaskDelegate(value_OnBackgroundTaskCompleted);

			UpdateComponentStates();
		}

		public BackgroundTaskLabel(IBackgroundTask task)
		{
			InitializeComponent();

			dOnBackgroundTaskStarted = new OnBackgroundTaskDelegate(value_OnBackgroundTaskStarted);
			dOnBackgroundTaskCompleted = new OnBackgroundTaskDelegate(value_OnBackgroundTaskCompleted);

			this.Task = task;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public IBackgroundTask Task
		{
			get {
				return task;
			}
			set {
				if (this.task != null) {
					this.task.OnBackgroundTaskStarted -= dOnBackgroundTaskStarted;
					this.task.OnBackgroundTaskCompleted -= dOnBackgroundTaskCompleted;
				}

				this.task = value;

				if (value != null) {
					bTerminating = false;
					value.OnBackgroundTaskStarted += dOnBackgroundTaskStarted;
					value.OnBackgroundTaskCompleted += dOnBackgroundTaskCompleted;
				}

				UpdateComponentStates();
			}
		}

		public Color BackgroundColorProgress
		{
			get {
				return backgroundColorProgress;
			}
			set {
				this.backgroundColorProgress = value;
				Invalidate();
			}
		}

		[Browsable(false)]
		public double Progress
		{
			get {
				return progress;
			}
			set {
				if (progress != value) {
					progress = value;
					int n = (int)(progress * 100 + 0.5);
					if (n < 0) n = 0;
					if (n > 100) n = 100;
					lblProgress.Text = n + "%";
					Invalidate();
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void value_OnBackgroundTaskCompleted(IBackgroundTask task)
		{
			UpdateComponentStates();
		}

		private void value_OnBackgroundTaskStarted(IBackgroundTask task)
		{
			UpdateComponentStates();
		}

		public void UpdateComponentStates()
		{
			Color c;
			if (task == null) {
				label1.Text = "(no task)";
				c = Color.DarkGray;
				button1.Text = "Info";
				button1.Enabled = false;
			} else {
				label1.Text = task.Name;
				switch (task.State) {
					case EnumBackgroundTaskState.Completed:
						button1.Text = "Info";
						c = Color.DarkGreen;
						break;
					case EnumBackgroundTaskState.Failed:
						button1.Text = "Info";
						c = Color.DarkRed;
						break;
					case EnumBackgroundTaskState.Running:
						button1.Text = "Terminate";
						c = Color.Gray;
						break;
					default:
						button1.Text = "Info";
						c = Color.Gray;
						break;
				}
				button1.Enabled = !bTerminating;
			}
			label1.ForeColor = c;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (task == null) return;

			switch (task.State) {
				case EnumBackgroundTaskState.None:
					GUIToolkit.ShowInformationMessage("Task not yet started.");
					break;
				case EnumBackgroundTaskState.Running:
					task.Terminate();
					bTerminating = true;
					UpdateComponentStates();
					break;
				case EnumBackgroundTaskState.Completed: {
						if (task.Output.Length > 0) {
							InfoForm form = new InfoForm(task.Output.ToString());
							form.Show();
						} else {
							GUIToolkit.ShowInformationMessage("Task has successfully been completed!");
						}
					}
					break;
				case EnumBackgroundTaskState.Failed: {
						ErrorForm form = new ErrorForm(task.Error.ToString(), task.Output);
						form.Show();
					}
					break;
			}
		}

	}

}
