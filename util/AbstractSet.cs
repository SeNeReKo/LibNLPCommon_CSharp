using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class AbstractSet<T> : IMySet<T>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		protected HashSet<T> innerSet;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AbstractSet()
		{
			innerSet = new HashSet<T>();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public virtual int Count
		{
			get {
				return innerSet.Count;
			}
		}

		public virtual bool IsReadOnly
		{
			get {
				return false;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public virtual void ExceptWith(IEnumerable<T> other)
		{
			innerSet.ExceptWith(other);
		}

		public virtual void IntersectWith(IEnumerable<T> other)
		{
			innerSet.IntersectWith(other);
		}

		public virtual bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return innerSet.IsProperSubsetOf(other);
		}

		public virtual bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return innerSet.IsProperSupersetOf(other);
		}

		public virtual bool IsSubsetOf(IEnumerable<T> other)
		{
			return IsSubsetOf(other);
		}

		public virtual bool IsSupersetOf(IEnumerable<T> other)
		{
			return innerSet.IsSupersetOf(other);
		}

		public virtual bool Overlaps(IEnumerable<T> other)
		{
			return innerSet.Overlaps(other);
		}

		public virtual bool SetEquals(IEnumerable<T> other)
		{
			return innerSet.SetEquals(other);
		}

		public virtual void SymmetricExceptWith(IEnumerable<T> other)
		{
			innerSet.SymmetricExceptWith(other);
		}

		public virtual void UnionWith(IEnumerable<T> other)
		{
			innerSet.UnionWith(other);
		}

		void System.Collections.Generic.ICollection<T>.Add(T item)
		{
			innerSet.Add(item);
		}

		public virtual bool Add(T item)
		{
			return innerSet.Add(item);
		}

		public virtual bool Remove(T item)
		{
			return innerSet.Remove(item);
		}

		public virtual void CopyTo(T[] array, int index)
		{
			innerSet.CopyTo(array, index);
		}

		public virtual void Clear()
		{
			innerSet.Clear();
		}

		public virtual bool Contains(T item)
		{
			return innerSet.Contains(item);
		}

		public virtual IEnumerator<T> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

	}

}
