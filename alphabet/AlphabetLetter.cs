using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.alphabet
{

	public class AlphabetLetter
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		string[] upperCaseLetters;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AlphabetLetter(string lowerCaseLetter, params string[] upperCaseLetters)
		{
			this.TextLowerCase = lowerCaseLetter;
			this.upperCaseLetters = upperCaseLetters;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string TextUpperCase
		{
			get {
				if (upperCaseLetters.Length == 0) return TextLowerCase;
				else return upperCaseLetters[0];
			}
		}

		public string[] UpperCaseLetters
		{
			get {
				return upperCaseLetters;
			}
		}

		public string TextLowerCase
		{
			get;
			private set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override bool Equals(object obj)
		{
			if (obj is AlphabetLetter) {
				AlphabetLetter al = (AlphabetLetter)obj;
				return TextLowerCase.Equals(al.TextLowerCase);
			} else
			if (obj is string) {
				string al = (string)obj;
				if (TextLowerCase.Equals(al)) return true;
				foreach (string s in upperCaseLetters) {
					if (s.Equals(al)) return true;
				}
				return false;
			} else {
				return false;
			}
		}

		public override int GetHashCode()
		{
			return TextLowerCase.GetHashCode();
		}

		public override string ToString()
		{
			return TextLowerCase;
		}

	}

}
