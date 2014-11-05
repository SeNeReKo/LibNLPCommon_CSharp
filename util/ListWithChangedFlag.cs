using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class ListWithChangedFlag<T> : AbstractList<T>, IHasChangedFlag
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private ChangedFlag changedFlag;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public ListWithChangedFlag()
		{
		}

		public ListWithChangedFlag(ChangedFlag changedFlag)
		{
			this.changedFlag = changedFlag;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public virtual ChangedFlag ChangedFlag
		{
			get {
				return changedFlag;
			}
			set {
				this.changedFlag = value;
				foreach (T item in innerList) {
					if (item is IHasChangedFlag) {
						IHasChangedFlag hcf = (IHasChangedFlag)item;
						hcf.ChangedFlag = changedFlag;
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
					if (value is IHasChangedFlag) {
						IHasChangedFlag hcf = (IHasChangedFlag)value;
						hcf.ChangedFlag = changedFlag;
					}
					if (changedFlag != null)
						changedFlag.IsChanged = true;
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public virtual void AddRange(IEnumerable<T> items)
		{
			if (changedFlag != null)
				changedFlag.SuppressEvents();
			foreach (T item in items) {
				Add(item);
			}
			if (changedFlag != null)
				changedFlag.ResumeEvents();
		}

		public override void Add(T item)
		{
			innerList.Add(item);
			if (item is IHasChangedFlag) {
				IHasChangedFlag hcf = (IHasChangedFlag)item;
				hcf.ChangedFlag = changedFlag;
			}
			if (changedFlag != null)
				changedFlag.IsChanged = true;
		}

		public override void Clear()
		{
			if (innerList.Count == 0)
				return;
			foreach (T item in innerList) {
				if (item is IHasChangedFlag) {
					IHasChangedFlag hcf = (IHasChangedFlag)item;
					hcf.ChangedFlag = null;
				}
			}
			innerList.Clear();
			if (changedFlag != null)
				changedFlag.IsChanged = true;
		}

		public override void Insert(int index, T item)
		{
			innerList.Insert(index, item);
			if (item is IHasChangedFlag) {
				IHasChangedFlag hcf = (IHasChangedFlag)item;
				hcf.ChangedFlag = changedFlag;
			}
			if (changedFlag != null)
				changedFlag.IsChanged = true;
		}

		public override bool Remove(T item)
		{
			if (innerList.Remove(item)) {
				if (item is IHasChangedFlag) {
					IHasChangedFlag hcf = (IHasChangedFlag)item;
					hcf.ChangedFlag = null;
				}
				if (changedFlag != null)
					changedFlag.IsChanged = true;
				return true;
			} else {
				return false;
			}
		}

		public override void RemoveAt(int index)
		{
			T item = innerList[index];
			if (item is IHasChangedFlag) {
				IHasChangedFlag hcf = (IHasChangedFlag)item;
				hcf.ChangedFlag = null;
			}
			innerList.RemoveAt(index);
			if (changedFlag != null)
				changedFlag.IsChanged = true;
		}

	}

}
