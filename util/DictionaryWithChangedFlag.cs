using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class DictionaryWithChangedFlag<K, V> : AbstractDictionary<K, V>, IHasChangedFlag
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

		public DictionaryWithChangedFlag()
		{
		}

		public DictionaryWithChangedFlag(ChangedFlag changedFlag)
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
				foreach (V item in innerDict.Values) {
					if (item is IHasChangedFlag) {
						IHasChangedFlag hcf = (IHasChangedFlag)item;
						hcf.ChangedFlag = changedFlag;
					}
				}
			}
		}

		public override V this[K key]
		{
			get {
				return base[key];
			}
			set {
				V existing;
				if (innerDict.TryGetValue(key, out existing)) {
					innerDict.Remove(key);
					if (existing is IHasChangedFlag) {
						IHasChangedFlag hcf = (IHasChangedFlag)existing;
						hcf.ChangedFlag = changedFlag;
					}
					innerDict.Add(key, value);
					if (this.changedFlag != null)
						this.changedFlag.IsChanged = true;
				} else {
					innerDict.Add(key, value);
					if (value is IHasChangedFlag) {
						IHasChangedFlag hcf = (IHasChangedFlag)value;
						hcf.ChangedFlag = changedFlag;
					}
					if (this.changedFlag != null)
						this.changedFlag.IsChanged = true;
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override void Add(K key, V value)
		{
			if (value is IHasChangedFlag) {
				IHasChangedFlag hcf = (IHasChangedFlag)value;
				hcf.ChangedFlag = changedFlag;
			}
			base.Add(key, value);
			if (this.changedFlag != null)
				this.changedFlag.IsChanged = true;
		}

		public override void Add(KeyValuePair<K, V> kvp)
		{
			if (kvp.Value is IHasChangedFlag) {
				IHasChangedFlag hcf = (IHasChangedFlag)(kvp.Value);
				hcf.ChangedFlag = changedFlag;
			}
			base.Add(kvp);
			if (this.changedFlag != null)
				this.changedFlag.IsChanged = true;
		}

		public override void Clear()
		{
			int n = Count;
			if (n == 0) return;
			foreach (V value in Values) {
				if (value is IHasChangedFlag) {
					IHasChangedFlag hcf = (IHasChangedFlag)value;
					hcf.ChangedFlag = null;
				}
			}
			base.Clear();
			if (this.changedFlag != null)
				this.changedFlag.IsChanged = true;
		}

		public override bool Remove(K key)
		{
			if (base.ContainsKey(key)) {
				V value = innerDict[key];
				if (value is IHasChangedFlag) {
					IHasChangedFlag hcf = (IHasChangedFlag)value;
					hcf.ChangedFlag = null;
				}
				base.Remove(key);
				if (this.changedFlag != null)
					this.changedFlag.IsChanged = true;
				return true;
			} else {
				return false;
			}
		}

		public override bool Remove(KeyValuePair<K, V> kvp)
		{
			if (base.Remove(kvp)) {
				if (kvp.Value is IHasChangedFlag) {
					IHasChangedFlag hcf = (IHasChangedFlag)(kvp.Value);
					hcf.ChangedFlag = null;
				}
				if (this.changedFlag != null)
					this.changedFlag.IsChanged = true;
				return true;
			} else {
				return false;
			}
		}

	}

}
