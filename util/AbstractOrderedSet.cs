using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public abstract class AbstractOrderedSet<T> : IMySet<T>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		protected SortedSet<T> innerSet;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AbstractOrderedSet(IComparer<T> comparer)
		{
			innerSet = new SortedSet<T>(comparer);
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

		/*
		/// <summary>
		/// Entfernt alle Elemente in der angegebenen Auflistung aus der aktuellen Gruppe.
		/// </summary>
		/// <param name="other"></param>
		public virtual void ExceptWith(IEnumerable<T> other)
		{
			innerSet.ExceptWith(other);
		}

		/// <summary>
		/// Ändert die aktuelle Gruppe, sodass sie nur Elemente enthält, die in einer angegebenen Auflistung ebenfalls enthalten sind.
		/// </summary>
		/// <param name="other"></param>
		public virtual void IntersectWith(IEnumerable<T> other)
		{
			innerSet.IntersectWith(other);
		}

		/// <summary>
		/// Bestimmt, ob die aktuelle Gruppe eine echte (strict) Teilmenge einer angegebenen Auflistung ist.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return innerSet.IsProperSubsetOf(other);
		}

		/// <summary>
		/// Bestimmt, ob die aktuelle Gruppe eine echte (strict) Obermenge von einer angegebenen Auflistung ist.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return innerSet.IsProperSupersetOf(other);
		}

		/// <summary>
		/// Bestimmt, ob eine Gruppe eine Teilmenge einer angegebenen Auflistung ist.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual bool IsSubsetOf(IEnumerable<T> other)
		{
			return IsSubsetOf(other);
		}

		/// <summary>
		/// Bestimmt, ob die aktuelle Gruppe eine Obermenge einer angegebenen Auflistung ist.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual bool IsSupersetOf(IEnumerable<T> other)
		{
			return innerSet.IsSupersetOf(other);
		}

		/// <summary>
		/// Bestimmt, ob sich die aktuelle Gruppe und die angegebene Auflistung überschneiden.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual bool Overlaps(IEnumerable<T> other)
		{
			return innerSet.Overlaps(other);
		}

		/// <summary>
		/// Bestimmt, ob die aktuelle Gruppe und die angegebene Auflistung dieselben Elemente enthalten.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual bool SetEquals(IEnumerable<T> other)
		{
			return innerSet.SetEquals(other);
		}

		/// <summary>
		/// Ändert die aktuelle Gruppe, sodass sie nur Elemente enthält, die entweder in der aktuellen Gruppe oder in der
		/// angegebenen Auflistung, nicht jedoch in beiden vorhanden sind. 
		/// </summary>
		/// <param name="other"></param>
		public virtual void SymmetricExceptWith(IEnumerable<T> other)
		{
			innerSet.SymmetricExceptWith(other);
		}

		public virtual void UnionWith(IEnumerable<T> other)
		{
			innerSet.UnionWith(other);
		}
		*/

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
