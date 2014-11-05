using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class OrderedSetWithChangedFlag<T> : AbstractOrderedSet<T>, IHasChangedFlag
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

		public OrderedSetWithChangedFlag(IComparer<T> comparer)
			: base(comparer)
		{
		}

		public OrderedSetWithChangedFlag(IComparer<T> comparer, ChangedFlag changedFlag)
			: base(comparer)
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
				foreach (T item in innerSet) {
					if (item is IHasChangedFlag) {
						IHasChangedFlag hcf = (IHasChangedFlag)item;
						hcf.ChangedFlag = changedFlag;
					}
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override bool Add(T item)
		{
			if (base.Add(item)) {
				if (item is IHasChangedFlag) {
					IHasChangedFlag hcf = (IHasChangedFlag)item;
					hcf.ChangedFlag = changedFlag;
				}
				if (changedFlag != null)
					changedFlag.IsChanged = true;
				return true;
			} else {
				return false;
			}
		}

		public override void Clear()
		{
			if (Count == 0) return;
			foreach (T item in innerSet) {
				if (item is IHasChangedFlag) {
					IHasChangedFlag hcf = (IHasChangedFlag)item;
					hcf.ChangedFlag = null;
				}
			}
			base.Clear();
			if (changedFlag != null)
				changedFlag.IsChanged = true;
		}

		/*
		public override void ExceptWith(IEnumerable<T> other)
		{
			throw new Exception("Not yet implemented!");

			// TODO: add change flag mgmt for the set
			/*
			int n = Count;
			base.ExceptWith(other);
			if (n != Count) {
				if (ChangedFlag != null)
					ChangedFlag.IsChanged = true;
			}
			*
		}

		public override void IntersectWith(IEnumerable<T> other)
		{
			throw new Exception("Not yet implemented!");

			// TODO: add change flag mgmt for the set
			/*
			int n = Count;
			base.IntersectWith(other);
			if (n != Count) {
				if (ChangedFlag != null)
					ChangedFlag.IsChanged = true;
			}
			*
		}

		public override void SymmetricExceptWith(IEnumerable<T> other)
		{
			throw new Exception("Not yet implemented!");

			// TODO: add change flag mgmt for the set
			/*
			int n = Count;
			base.SymmetricExceptWith(other);
			if (n != Count) {
				if (ChangedFlag != null)
					ChangedFlag.IsChanged = true;
			}
			*
		}

		public override void UnionWith(IEnumerable<T> other)
		{
			throw new Exception("Not yet implemented!");

			// TODO: add change flag mgmt for the set
			/*
			int n = Count;
			base.UnionWith(other);
			if (n != Count) {
				if (ChangedFlag != null)
					ChangedFlag.IsChanged = true;
			}
			*
		}
		*/

		public override bool Remove(T item)
		{
			if (base.Remove(item)) {
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

	}

}
