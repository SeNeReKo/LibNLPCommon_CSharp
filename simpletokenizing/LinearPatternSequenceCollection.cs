using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.simpletokenizing
{

	public class LinearPatternSequenceCollection : List<LinearPatternSequence>, ITokenPatternMatcher
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public LinearPatternSequenceCollection(params LinearPatternSequence[] sequences)
		{
			AddRange(sequences);

			Sort();
		}

		public LinearPatternSequenceCollection(IEnumerable<LinearPatternSequence> sequences)
		{
			AddRange(sequences);

			Sort();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the fixed length of the sequence matched (if this pattern matcher has a fixed length)
		/// </summary>
		public int? Length
		{
			get {
				return null;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		/// <summary>
		/// Does any of the stored pattern match at the current position in the stream?
		/// </summary>
		/// <param name="tokenStream"></param>
		/// <returns></returns>
		public bool Match(TokenStream tokenStream)
		{
			foreach (LinearPatternSequence seq in this) {
				if (seq.Match(tokenStream)) return true;
			}
			return false;
		}

		/// <summary>
		/// Try to eat the stored pattern from the stream. On success the stream is advanced.
		/// </summary>
		/// <param name="tokenStream"></param>
		/// <returns></returns>
		public TokenPatternResult TryEat(TokenStream tokenStream)
		{
			foreach (LinearPatternSequence seq in this) {
				TokenPatternResult r = seq.TryEat(tokenStream);
				if (r != null) {
					if (Name != null) r.Name = Name;
					return r;
				}
			}
			return null;
		}

		/*
		/// <summary>
		/// Same as <code>TryEat()</code> but without advancing the stream
		/// </summary>
		/// <param name="tokenStream"></param>
		/// <returns></returns>
		public TokenPatternResult TryMatch(TokenStream tokenStream)
		{
			foreach (LinearPatternSequence seq in this) {
				TokenPatternResult r = seq.TryMatch(tokenStream);
				if (r != null) {
					if (Name != null) r.Name = Name;
					return r;
				}
			}
			return null;
		}
		*/

		public void Sort()
		{
			LinearPatternSequence[] all = ToArray();
			Array.Sort(all, ____ReverseComparison);
			Clear();
			AddRange(all);
		}

		private int ____Comparison(LinearPatternSequence x, LinearPatternSequence y)
		{
			if (x.Length.HasValue) {
				if (y.Length.HasValue) {
					return x.Length.Value.CompareTo(y.Length.Value);
				} else {
					return -1;
				}
			} else {
				if (y.Length.HasValue) {
					return 1;
				} else {
					return 0;
				}
			}
		}

		private int ____ReverseComparison(LinearPatternSequence x, LinearPatternSequence y)
		{
			if (x.Length.HasValue) {
				if (y.Length.HasValue) {
					return - x.Length.Value.CompareTo(y.Length.Value);
				} else {
					return 1;
				}
			} else {
				if (y.Length.HasValue) {
					return - 1;
				} else {
					return 0;
				}
			}
		}

	}

}
