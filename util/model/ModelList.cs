using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util.model
{

	public class ModelList<T> : AbstractList<T>, IObservable
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private IObserver observer;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public ModelList()
		{
		}

		public ModelList(IObserver observer)
		{
			this.observer = observer;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public virtual IObserver Observer
		{
			get {
				return observer;
			}
			set {
				this.observer = value;
				foreach (T item in innerList) {
					if (item is IObservable) {
						IObservable hcf = (IObservable)item;
						hcf.Observer = observer;
					}
				}
			}
		}

		public override T this[int index]
		{
			get {
				return base[index];
			}
			set {
				if (!Util.Equals(innerList[index], value)) {
					innerList[index] = value;
					if (value is IObservable) {
						IObservable hcf = (IObservable)value;
						hcf.Observer = observer;
					}
					if (observer != null) observer.Notify();
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public virtual void AddRange(IEnumerable<T> items)
		{
			if (observer != null) observer.SuspendEvents();
			foreach (T item in items) {
				Add(item);
			}
			if (observer != null) observer.ResumeEvents();
		}

		public override void Add(T item)
		{
			innerList.Add(item);
			if (item is IObservable) {
				IObservable hcf = (IObservable)item;
				hcf.Observer = observer;
			}
			if (observer != null) observer.Notify();
		}

		public override void Clear()
		{
			if (innerList.Count == 0)
				return;
			foreach (T item in innerList) {
				if (item is IObservable) {
					IObservable hcf = (IObservable)item;
					hcf.Observer = null;
				}
			}
			innerList.Clear();
			if (observer != null) observer.Notify();
		}

		public override void Insert(int index, T item)
		{
			innerList.Insert(index, item);
			if (item is IObservable) {
				IObservable hcf = (IObservable)item;
				hcf.Observer = observer;
			}
			if (observer != null) observer.Notify();
		}

		public override bool Remove(T item)
		{
			if (innerList.Remove(item)) {
				if (item is IObservable) {
					IObservable hcf = (IObservable)item;
					hcf.Observer = null;
				}
				if (observer != null) observer.Notify();
				return true;
			} else {
				return false;
			}
		}

		public override void RemoveAt(int index)
		{
			T item = innerList[index];
			if (item is IObservable) {
				IObservable hcf = (IObservable)item;
				hcf.Observer = null;
			}
			innerList.RemoveAt(index);
			if (observer != null) observer.Notify();
		}

	}

}
