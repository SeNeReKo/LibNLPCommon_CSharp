using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LibNLPCSharp.bgtask
{

	public partial class BackgroundTaskList : UserControl
	{

		public class TaskList
		{
			private BackgroundTaskList parent;

			internal List<IBackgroundTask> tasks;
			internal Dictionary<IBackgroundTask, BackgroundTaskLabel> taskCollection;
			internal Dictionary<IBackgroundTask, double> progressCollection;
			
			internal TaskList(BackgroundTaskList parent)
			{
				this.parent = parent;

				progressCollection = new Dictionary<IBackgroundTask, double>();
				taskCollection = new Dictionary<IBackgroundTask, BackgroundTaskLabel>();
				tasks = new List<IBackgroundTask>();
			}

			public void Add(IBackgroundTask task)
			{
				tasks.Add(task);
				progressCollection.Add(task, 0);
				taskCollection.Add(task, null);
				task.OnProgress += new OnTaskProgressDelegate(task_OnProgress);

				parent.__Refill();
			}

			public void Remove(IBackgroundTask task)
			{
				tasks.Remove(task);
				taskCollection.Remove(task);
				progressCollection.Remove(task);

				parent.__Refill();
			}

			public void Clean(bool bRemoveCompleted, bool bRemoveFailed)
			{
				for (int i = tasks.Count - 1; i >= 0; i--) {
					switch (tasks[i].State) {
						case EnumBackgroundTaskState.Completed:
							if (bRemoveCompleted) tasks.RemoveAt(i);
							break;
						case EnumBackgroundTaskState.Failed:
							if (bRemoveFailed) tasks.RemoveAt(i);
							break;
					}
				}

				parent.__Refill();
			}

			private void task_OnProgress(IBackgroundTask task, double progress)
			{
				progressCollection.Remove(task);
				progressCollection.Add(task, progress);

				BackgroundTaskLabel label;
				if (taskCollection.TryGetValue(task, out label)) {
					label.Progress = progress;
				} else {
					parent.__Refill();
				}
			}

		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public BackgroundTaskList()
		{
			InitializeComponent();

			Tasks = new TaskList(this);
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		[Browsable(false)]
		public TaskList Tasks
		{
			get;
			private set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		protected void __Refill()
		{
			Controls.Clear();
			int x = 0;
			Tasks.taskCollection.Clear();
			foreach (IBackgroundTask task in Tasks.tasks) {
				BackgroundTaskLabel label = new BackgroundTaskLabel(task);
				label.Progress = Tasks.progressCollection[task];
				label.Location = new Point(x, 0);
				Tasks.taskCollection.Add(task, label);
				x += label.Size.Width;
				Controls.Add(label);
			}
		}

	}

}
