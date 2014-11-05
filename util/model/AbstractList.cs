using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util.model
{

	public abstract class AbstractList<T> : IList<T>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		protected List<T> innerList;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AbstractList()
		{
			innerList = new List<T>();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public virtual bool IsReadOnly
		{
			get {
				return false;
			}
		}

		public virtual int Count
		{
			get {
				return innerList.Count;
			}
		}

		public virtual T this[int index]
		{
			get {
				return innerList[index];
			}
			set {
				innerList[index] = value;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public virtual bool Remove(T item)
		{
			return innerList.Remove(item);
		}

		public virtual void CopyTo(T[] array, int index)
		{
			innerList.CopyTo(array, index);
		}

		public virtual bool Contains(T item)
		{
			return innerList.Contains(item);
		}

		public virtual void Clear()
		{
			innerList.Clear();
		}

		public virtual void RemoveAt(int index)
		{
			innerList.RemoveAt(index);
		}

		public virtual void Insert(int index, T item)
		{
			innerList.Insert(index, item);
		}

		public virtual int IndexOf(T item)
		{
			return innerList.IndexOf(item);
		}

		public virtual void Add(T item)
		{
			innerList.Add(item);
		}

		public virtual IEnumerator<T> GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

	}

}
