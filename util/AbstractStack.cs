using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public abstract class AbstractStack<T>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		protected T[] items;
		protected int count;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AbstractStack()
		{
			items = new T[8];
		}

		public AbstractStack(int initialCapacity)
		{
			int n = 8;
			while (n < initialCapacity) n *= 2;
			items = new T[n];
		}

		public AbstractStack(T element)
		{
			items = new T[8];
			items[0] = element;
			count = 1;
		}

		public AbstractStack(T[] copyFromThis, int len)
		{
			items = new T[copyFromThis.Length];
			for (int i = 0; i < len; i++) {
				items[i] = copyFromThis[i];
			}
			this.count = len;
		}

		public AbstractStack(T[] copyFromThis, int ofs, int len)
		{
			items = new T[copyFromThis.Length];
			for (int i = 0; i < len; i++) {
				items[i] = copyFromThis[ofs + i];
			}
			this.count = len;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public int Count
		{
			get {
				return count;
			}
		}

		public T this[int index]
		{
			get {
				if ((index < 0) || (index >= count)) {
					return default(T);
				} else {
					return items[index];
				}
			}
		}

		public T Top
		{
			get {
				if (count == 0) return default(T);

				return items[count - 1];
			}
		}

		public bool IsEmpty
		{
			get {
				return count == 0;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public T[] ToArray()
		{
			T[] ret = new T[count];
			for (int i = 0; i < count; i++) {
				ret[i] = items[i];
			}
			return ret;
		}

		public AbstractStack<T> Push(T e)
		{
			if (count == items.Length) {
				T[] newElements = new T[items.Length * 2];
				items.CopyTo(newElements, 0);
				items = newElements;
			}

			items[count] = e;
			count++;

			return this;
		}

		public AbstractStack<T> Push(params T[] manyElements)
		{
			if (items.Length < count + manyElements.Length) {
				int n = items.Length * 2;
				while (n < count + manyElements.Length) n *= 2;
				T[] newElements = new T[n];
				items.CopyTo(newElements, 0);
				items = newElements;
			}

			manyElements.CopyTo(items, count);
			count += manyElements.Length;

			return this;
		}

		public AbstractStack<T> Push(T[] manyElements, int len)
		{
			if (items.Length < count + len) {
				int n = items.Length * 2;
				while (n < count + len) n *= 2;
				T[] newElements = new T[n];
				items.CopyTo(newElements, 0);
				items = newElements;
			}
	
			Array.Copy(manyElements, 0, items, 0, len);

			count += manyElements.Length;

			return this;
		}

		public AbstractStack<T> Push(T[] manyElements, int ofs, int len)
		{
			if (items.Length < count + len) {
				int n = items.Length * 2;
				while (n < count + len)
					n *= 2;
				T[] newElements = new T[n];
				items.CopyTo(newElements, 0);
				items = newElements;
			}

			Array.Copy(manyElements, 0, items, ofs, len);

			count += manyElements.Length;

			return this;
		}

		public T Pop()
		{
			if (count == 0) return default(T);

			count--;
			T e = items[count];
			items[count] = default(T);
			return e;
		}

		public T Peek()
		{
			if (count == 0) return default(T);

			return items[count - 1];
		}

		public abstract override string ToString();

		public void CopyTo(Array a, int ofs)
		{
			for (int i = 0; i < count; i++) {
				a.SetValue(items[i], ofs + i);
			}
		}

		public void Clear()
		{
			if (count > 0) {
				for (int i = 0; i < count; i++) {
					items[i] = default(T);
				}
				count = 0;
			}
		}

	}

}
