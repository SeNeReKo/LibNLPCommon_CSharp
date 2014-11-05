using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.io.texttokenizing
{

	public struct StreamChar
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		public readonly int Position;
		public readonly int LineNo;
		public readonly int ColumnNo;
		public readonly char Char;
		public readonly int CharI;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public StreamChar(int charI, int pos, int lineNo, int colNo)
		{
			this.Position = pos;
			this.LineNo = lineNo;
			this.ColumnNo = colNo;
			this.CharI = charI;
			this.Char = (char)(charI);
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool IsEOS
		{
			get {
				return CharI < 0;
			}
		}

		public bool IsLetterOrDigit
		{
			get {
				return Char.IsLetterOrDigit(Char);
			}
		}

		public bool IsDelimiter
		{
			get {
				return !Char.IsLetterOrDigit(Char);
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override bool Equals(object obj)
		{
			if (obj is StreamChar) {
				StreamChar c = (StreamChar)obj;
				return (c.CharI == CharI) && (c.Position == Position);
			} else {
				return false;
			}
		}

		public override int GetHashCode()
		{
			return CharI;
		}

		public override string ToString()
		{
			return "" + Char;
		}

		////////////////////////////////////////////////////////////////
		// Operators
		////////////////////////////////////////////////////////////////

		public static implicit operator char(StreamChar m)
		{
			return m.Char;
		}

		public static implicit operator string(StreamChar m)
		{
			return "" + m.Char;
		}

	}

}
