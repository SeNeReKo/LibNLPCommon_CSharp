using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using LibNLPCSharp.util;


namespace LibNLPCSharp.io.texttokenizing
{

	public class SimpleWordTokenizer : ITextTokenizer
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		PushbackCharacterReader reader;
		TextToken nextToken;
		StringBuilder sb;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public SimpleWordTokenizer(PushbackCharacterReader reader)
		{
			this.reader = reader;
			sb = new StringBuilder();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool HasNext
		{
			get {
				return (nextToken != null) || reader.HasNext;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public TextToken Read()
		{
			if (nextToken != null) {
				TextToken tt = nextToken;
				nextToken = null;
				return tt;
			} else {
				return __ReadNextToken();
			}
		}

		private TextToken __ReadNextToken()
		{
			sb.Remove(0, sb.Length);
			bool bIsFirst = true;
			int pos = 0;
			int lineNo = -1;
			int colNo = -1;
			while (true) {
				StreamChar c = reader.Read();
				if (bIsFirst) {
					if (c.IsEOS) {
						// EOS
						return new TextToken(EnumTextTokenType.EOS, sb.ToString(), c.Position, c.LineNo, c.ColumnNo);
					} else
					if (c.IsLetterOrDigit) {
						// start of a word
						sb.Append(c.Char);
						pos = c.Position;
						lineNo = c.LineNo;
						colNo = c.ColumnNo;
						bIsFirst = false;
					} else {
						// next token is delimiter
						if (c.CharI == 13) {
							return new TextToken(EnumTextTokenType.LineFeed, "" + c.Char, c.Position, c.LineNo, c.ColumnNo);
						} else {
							return new TextToken(EnumTextTokenType.Delimiter, "" + c.Char, c.Position, c.LineNo, c.ColumnNo);
						}
					}
				} else {
					if (c.IsEOS) {
						// EOS
						return new TextToken(EnumTextTokenType.EOS, sb.ToString(), pos, lineNo, colNo);
					} else
					if (c.IsLetterOrDigit) {
						// within of a word
						sb.Append(c.Char);
					} else {
						// within a delimiter
						reader.Unread(c);
						return new TextToken(EnumTextTokenType.Text, sb.ToString(), pos, lineNo, colNo);
					}
				}
			}
		}

		public TextToken[] ToArray()
		{
			List<TextToken> ret = new List<TextToken>();
			while (HasNext) {
				ret.Add(Read());
			}
			return ret.ToArray();
		}

		public string[] ToStringArray()
		{
			List<string> ret = new List<string>();
			while (HasNext) {
				TextToken tt = Read();
				if (tt.Text == null) ret.Add(" ");
				else ret.Add(tt.Text);
			}
			return ret.ToArray();
		}

	}

}
