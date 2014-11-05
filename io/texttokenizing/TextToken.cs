using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.io.texttokenizing
{

	public class TextToken
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		public string Text;
		public EnumTextTokenType TokenType;
		public int Position;
		public int LineNo;
		public int ColumnNo;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TextToken(EnumTextTokenType type, string text, int position, int lineNo, int columnNo)
		{
			this.TokenType = type;
			this.Text = text;
			this.Position = position;
			this.LineNo = lineNo;
			this.ColumnNo = columnNo;
		}

		public TextToken(EnumTextTokenType type, string text)
		{
			this.TokenType = type;
			this.Text = text;
			this.Position = -1;
			this.LineNo = -1;
			this.ColumnNo = -1;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override bool Equals(object obj)
		{
			if (obj is TextToken) {
				TextToken tt = (TextToken)obj;
				return (TokenType == tt.TokenType)
					&& (Text.Equals(tt.Text));
			} else {
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Text.GetHashCode();
		}

		public override string ToString()
		{
			return Text;
		}

		public static implicit operator string(TextToken tt)
		{
			return tt.Text;
		}

	}

}
