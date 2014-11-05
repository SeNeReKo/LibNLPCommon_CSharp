using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class SortedSet<T> : IMySet<T>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		HashSet<T> innerSet;
		T[] list;
		int count;
		IComparer<T> comparer;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public SortedSet()
		{
			innerSet = new HashSet<T>();
			list = new T[16];
		}

		public SortedSet(IEnumerable<T> elements)
			: this(elements, null)
		{
		}

		public SortedSet(IComparer<T> comparer)
			: this(null, comparer)
		{
		}

		public SortedSet(IEnumerable<T> elements, IComparer<T> comparer)
		{
			this.comparer = comparer;
			innerSet = new HashSet<T>();
			list = new T[16];
			if (elements != null) AddRange(elements);
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

		public int Count
		{
			get {
				return count;
			}
		}

		public T Max
		{
			get {
				if (count == 0) return default(T);
				return list[count - 1];
			}
		}

		public T Min
		{
			get {
				if (count == 0) return default(T);
				return list[0];
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private int __FindInsertPosition(T item)
		{
			int n = Array.BinarySearch<T>(list, 0, count, item, comparer);
			if (n >= 0) {
				return n;
			} else {
				n = ~n;
				return n;
			}
		}

		private int __FindPosition(T item)
		{
			int n = Array.BinarySearch<T>(list, 0, count, item, comparer);
			if (n < 0)
				return -1;
			else
				return n;
		}

		private void __MakeRoomAt(int index)
		{
			__EnsureCapacity();
			if (index < count) {
				for (int i = count - 1; i >= index; i--) {
					list[i + 1] = list[i];
				}
			}
		}

		private void __EnsureCapacity()
		{
			if (count < list.Length)
				return;

			T[] newList = new T[list.Length * 2];
			list.CopyTo(newList, 0);
			list = newList;
		}

		private void __RemoveAt(int index)
		{
			for (int i = index + 1; i < count; i++) {
				list[i - 1] = list[i];
			}
			list[count - 1] = default(T);
		}

		////////////////////////////////////////////////////////////////

		void System.Collections.Generic.ICollection<T>.Add(T item)
		{
			if (!innerSet.Add(item)) return;
			int n = __FindInsertPosition(item);
			__MakeRoomAt(n);
			list[n] = item;
			count++;
		}

		public virtual void AddRange(IEnumerable<T> elements)
		{
			foreach (T t in elements)
				Add(t);
		}

		public virtual bool Add(T item)
		{
			if (!innerSet.Add(item)) return false;
			int n = __FindInsertPosition(item);
			__MakeRoomAt(n);
			list[n] = item;
			count++;
			return true;
		}

		public virtual bool Remove(T item)
		{
			if (!innerSet.Remove(item)) return false;
			int n = __FindPosition(item);
			if (n < 0) throw new ImplementationErrorException();
			__RemoveAt(n);
			count--;
			return true;
		}

		public virtual void CopyTo(T[] array, int index)
		{
			for (int i = 0; i < count; i++) {
				array[index + i] = list[i];
			}
		}

		public virtual void Clear()
		{
			for (int i = 0; i < count; i++) {
				list[i] = default(T);
			}
			innerSet.Clear();
			count = 0;
		}

		public virtual bool Contains(T item)
		{
			int n = __FindPosition(item);
			return n >= 0;
		}

		public virtual IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < count; i++) {
				yield return list[i];
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < count; i++) {
				yield return list[i];
			}
		}

	}

}
