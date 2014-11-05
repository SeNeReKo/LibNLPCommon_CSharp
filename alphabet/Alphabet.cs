using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.alphabet
{

	/// <summary>
	/// An instance of <code>Alphabet</code> represents a collection of letters that form an alphabet.
	/// There are two perspectives on this alphabet: A linear one and a two dimensional one. The first
	/// is used to iterate through this alphabet. The second one can be used to fill GUI controls.
	/// </summary>
	public class Alphabet
	{

		private class LetterLengthComparer : IComparer<AlphabetLetter>
		{
			public int Compare(AlphabetLetter x, AlphabetLetter y)
			{
				if (x.TextLowerCase.Length > y.TextLowerCase.Length) return -1;
				if (y.TextLowerCase.Length > x.TextLowerCase.Length) return 1;
				return 0;
			}
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private readonly AlphabetLetter[] allLetters;
		private readonly AlphabetLetter[,] letters;
		private readonly int w;
		private readonly int h;
		private readonly string allCharacters;
		private readonly string allCharactersUnicoded;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initialize this alphabet letter matrix.
		/// </summary>
		/// <param name="letters">A two dimensional array organized vertically and horizontally (in this order).</param>
		public Alphabet(AlphabetLetter[,] letters)
		{
			this.letters = letters;
			this.h = letters.GetLength(0);
			this.w = letters.GetLength(1);

			List<AlphabetLetter> allLetters = new List<AlphabetLetter>();
			allCharacters = "";
			allCharactersUnicoded = "";
			for (int y = 0; y < h; y++) {
				for (int x = 0; x < w; x++) {
					AlphabetLetter al = letters[y, x];
					if (al != null) {
						allLetters.Add(al);
						string s = al.TextLowerCase;
						for (int i = 0; i < s.Length; i++) {
							char c = s[i];
							if (allCharacters.IndexOf(c) < 0) {
								allCharacters += c;
								allCharactersUnicoded += __ToUnicode(c);
							}
						}
					}
				}
			}
			this.allLetters = allLetters.ToArray();
		}

		private static readonly string HEXCODES = "0123456789abcdef";
	
		private static string __ShortToHexLE(ushort n)
		{
			char[] s = new char[4];
			int i = 3;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i] = HEXCODES[(n & 15)];
			return new string(s);
		}

		private static string __IntToHexLE(int n)
		{
			char[] s = new char[8];
			int i = 7;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i--] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i] = HEXCODES[(n & 15)];
			return new string(s);
		}

		private static string __ShortToHexBE(ushort n)
		{
			char[] s = new char[4];
			int i = 0;
			s[i++] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i++] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i++] = HEXCODES[(n & 15)];
			n >>= 4;
			s[i++] = HEXCODES[(n & 15)];
			return new string(s);
		}

		private static string __ToUnicode(int c)
		{
			// return "\\u" + __ShortToHexLE((ushort)c);
			return "\\U" + __IntToHexLE(c);
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public int Height
		{
			get {
				return h;
			}
		}

		public int Width
		{
			get {
				return w;
			}
		}

		public int Count
		{
			get {
				return allLetters.Length;
			}
		}

		public AlphabetLetter this[int index]
		{
			get {
				return allLetters[index];
			}
		}

		public AlphabetLetter this[string singleLetter]
		{
			get {
				foreach (AlphabetLetter letter in allLetters) {
					if (letter.Equals(singleLetter)) return letter;
				}
				return null;
			}
		}

		public AlphabetLetter this[int y, int x]
		{
			get {
				return letters[y, x];
			}
		}

		public string AllCharacters
		{
			get {
				return allCharacters;
			}
		}

		public string AllCharactersUnicoded
		{
			get {
				return allCharactersUnicoded;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override string ToString()
		{
			return "Alphabet[" + h + " x " + w + "]";
		}

		public string LetterToUpperCase(string singleLetter)
		{
			AlphabetLetter letter = this[singleLetter];
			if (letter == null) return null;
			return letter.TextUpperCase;
		}

		public string LetterToLowerCase(string singleLetter)
		{
			AlphabetLetter letter = this[singleLetter];
			if (letter == null) return null;
			return letter.TextLowerCase;
		}

		public string WordToLowerCase(string letters)
		{
			if (letters == null) return null;
			AlphabetLetter[] letters2 = ConvertStringToLetters(letters);
			if (letters2 == null) return null;
			return WordToLowerCase(letters2, "");
		}

		public string WordToLowerCase(AlphabetLetter[] letters)
		{
			return WordToLowerCase(letters, "");
		}

		public string WordToLowerCase(AlphabetLetter[] letters, string infix)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < letters.Length; i++) {
				if ((infix != null) && (i > 0)) sb.Append(infix);
				sb.Append(letters[i].TextLowerCase);
			}
			return sb.ToString();
		}

		public AlphabetLetter[] ConvertStringToLetters(string word)
		{
			AlphabetLetter[] letters = new AlphabetLetter[allLetters.Length];	// TODO: optimize this! this does not need to be calculated every time!
			allLetters.CopyTo(letters, 0);
			Array.Sort(letters, new LetterLengthComparer());

			List<AlphabetLetter> ret = new List<AlphabetLetter>();
			int pos = 0;
			while (pos < word.Length) {
				bool b = false;
				for (int i = 0; i < letters.Length; i++) {
					if (__EqualsAt(letters[i].TextLowerCase, word, pos, letters[i].TextLowerCase.Length)
						|| ((letters[i].TextUpperCase != null) && __EqualsAt(letters[i].TextUpperCase, word, pos, letters[i].TextLowerCase.Length))) {
						ret.Add(letters[i]);
						pos += letters[i].TextLowerCase.Length;
						b = true;
						break;
					}
				}
				if (b) continue;

				return null;
			}
			return ret.ToArray();
		}

		public AlphabetLetter[] ConvertStringToLettersE(string word)
		{
			AlphabetLetter[] letters = new AlphabetLetter[allLetters.Length];
			allLetters.CopyTo(letters, 0);
			Array.Sort(letters, new LetterLengthComparer());

			List<AlphabetLetter> ret = new List<AlphabetLetter>();
			int pos = 0;
			while (pos < word.Length) {
				bool b = false;
				for (int i = 0; i < letters.Length; i++) {
					if (__EqualsAt(letters[i].TextLowerCase, word, pos, letters[i].TextLowerCase.Length)
						|| ((letters[i].TextUpperCase != null) && __EqualsAt(letters[i].TextUpperCase, word, pos, letters[i].TextLowerCase.Length))) {
						ret.Add(letters[i]);
						pos += letters[i].TextLowerCase.Length;
						b = true;
						break;
					}
				}
				if (b) continue;

				throw new Exception("Unknown characters encountered in word \"" + word + "\" at position " + pos + "!");
			}
			return ret.ToArray();
		}

		private bool __EqualsAt(string s1, string s2, int pos2, int len)
		{
			for (int i = 0; i < len; i++) {
				if (i >= s1.Length) return false;
				if (pos2 + i >= s2.Length) return false;
				if (s1[i] != s2[pos2 + i]) return false;
			}
			return true;
		}

		public Alphabet DeriveAdd(params AlphabetLetter[] additionalLetters)
		{
			int count = letters.GetLength(1);
			if (additionalLetters.Length > count) count = additionalLetters.Length;

			AlphabetLetter[,] newLetters = new AlphabetLetter[letters.GetLength(0) + 1, count];

			for (int i = 0; i < letters.GetLength(0); i++) {
				for (int j = 0; j < letters.GetLength(1); j++) {
					newLetters[i, j] = letters[i, j];
				}
			}

			for (int i = 0; i < additionalLetters.Length; i++) {
				newLetters[letters.GetLength(0), i] = additionalLetters[i];
			}
			return new Alphabet(newLetters);
		}

	}

}
