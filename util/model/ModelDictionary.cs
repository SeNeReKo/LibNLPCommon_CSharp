using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util.model
{

	public class ModelDictionary<K, V> : AbstractDictionary<K, V>, IObservable
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

		public ModelDictionary()
		{
		}

		public ModelDictionary(IObserver observer)
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
				foreach (V item in innerDict.Values) {
					if (item is IObservable) {
						IObservable hcf = (IObservable)item;
						hcf.Observer = observer;
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
					if (existing is IObservable) {
						IObservable hcf = (IObservable)existing;
						hcf.Observer = observer;
					}
					innerDict.Add(key, value);
					if (this.observer != null) observer.Notify();
				} else {
					innerDict.Add(key, value);
					if (value is IObservable) {
						IObservable hcf = (IObservable)value;
						hcf.Observer = observer;
					}
					if (this.observer != null) observer.Notify();
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override void Add(K key, V value)
		{
			if (value is IObservable) {
				IObservable hcf = (IObservable)value;
				hcf.Observer = observer;
			}
			base.Add(key, value);
			if (this.observer != null) observer.Notify();
		}

		public override void Add(KeyValuePair<K, V> kvp)
		{
			if (kvp.Value is IObservable) {
				IObservable hcf = (IObservable)(kvp.Value);
				hcf.Observer = observer;
			}
			base.Add(kvp);
			if (this.observer != null) observer.Notify();
		}

		public override void Clear()
		{
			int n = Count;
			if (n == 0) return;
			foreach (V value in Values) {
				if (value is IObservable) {
					IObservable hcf = (IObservable)value;
					hcf.Observer = null;
				}
			}
			base.Clear();
			if (this.observer != null) observer.Notify();
		}

		public override bool Remove(K key)
		{
			if (base.ContainsKey(key)) {
				V value = innerDict[key];
				if (value is IObservable) {
					IObservable hcf = (IObservable)value;
					hcf.Observer = null;
				}
				base.Remove(key);
				if (this.observer != null) observer.Notify();
				return true;
			} else {
				return false;
			}
		}

		public override bool Remove(KeyValuePair<K, V> kvp)
		{
			if (base.Remove(kvp)) {
				if (kvp.Value is IObservable) {
					IObservable hcf = (IObservable)(kvp.Value);
					hcf.Observer = null;
				}
				if (this.observer != null) observer.Notify();
				return true;
			} else {
				return false;
			}
		}

	}

}
