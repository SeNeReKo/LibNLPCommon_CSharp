using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.simpletokenizing
{

	public class Token
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		private static readonly SKVPSet EMPTY_READONLY_SKVPSET = new SKVPSet(true);

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		public readonly EnumGeneralTokenType GeneralType;
		public readonly string Text;
		public readonly int LineNumber;
		public readonly int CharacterPosition;

		public SKVPSet Tags;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public Token(int pos, string text, EnumGeneralTokenType generalType)
		{
			this.LineNumber = -1;
			this.CharacterPosition = pos;
			this.Text = text;
			this.GeneralType = generalType;
			this.Tags = EMPTY_READONLY_SKVPSET;
		}

		public Token(int lineNo, int pos, string text, EnumGeneralTokenType generalType)
		{
			this.LineNumber = lineNo;
			this.CharacterPosition = pos;
			this.Text = text;
			this.GeneralType = generalType;
			this.Tags = EMPTY_READONLY_SKVPSET;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool IsSpace
		{
			get {
				return GeneralType == EnumGeneralTokenType.Space;
			}
		}

		public bool IsWord
		{
			get {
				return GeneralType == EnumGeneralTokenType.Word;
			}
		}

		public bool IsStringSingleQuotes
		{
			get {
				return GeneralType == EnumGeneralTokenType.StringSQ;
			}
		}

		public bool IsStringDoubleQuotes
		{
			get {
				return GeneralType == EnumGeneralTokenType.StringDQ;
			}
		}

		public bool IsDelimiter
		{
			get {
				return GeneralType == EnumGeneralTokenType.Delimiter;
			}
		}

		public bool IsEOS
		{
			get {
				return GeneralType == EnumGeneralTokenType.EOS;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public bool TokenIsWord(string text)
		{
			if (GeneralType != EnumGeneralTokenType.Word) return false;
			return Text.Equals(text);
		}

		public bool TokenIsStringDoubleQuotes(string text)
		{
			if (GeneralType != EnumGeneralTokenType.StringDQ) return false;
			return Text.Equals(text);
		}

		public bool TokenIsStringSingleQuotes(string text)
		{
			if (GeneralType != EnumGeneralTokenType.StringSQ) return false;
			return Text.Equals(text);
		}

		public bool TokenIsDelimiter(string text)
		{
			if (GeneralType != EnumGeneralTokenType.Delimiter) return false;
			return Text.Equals(text);
		}

		public bool TokenIsDelimiter(char c)
		{
			if (GeneralType != EnumGeneralTokenType.Delimiter) return false;
			return Text.Equals("" + c);
		}

		public override string ToString()
		{
			switch (GeneralType) {
				case EnumGeneralTokenType.Space:
					return "SPACE";
				case EnumGeneralTokenType.Delimiter:
					return "DELIM['" + Text + "']";
				case EnumGeneralTokenType.Word:
					return Text;
				case EnumGeneralTokenType.StringSQ:
					return "'" + Text.Replace("\'", "\\\'") + "'";
				case EnumGeneralTokenType.StringDQ:
					return "\"" + Text.Replace("\"", "\\\"") + "\"";
				case EnumGeneralTokenType.EOS:
					return "EOS";
				default:
					throw new NotImplementedException();
			}
		}

		////////////////////////////////////////////////////////////////

		public static Token CreateStringSQToken(int lineNo, int pos, string text)
		{
			return new Token(lineNo, pos, text, EnumGeneralTokenType.StringSQ);
		}

		public static Token CreateStringSQToken(int pos, string text)
		{
			return new Token(-1, pos, text, EnumGeneralTokenType.StringSQ);
		}

		public static Token CreateStringDQToken(int lineNo, int pos, string text)
		{
			return new Token(lineNo, pos, text, EnumGeneralTokenType.StringDQ);
		}

		public static Token CreateStringDQToken(int pos, string text)
		{
			return new Token(-1, pos, text, EnumGeneralTokenType.StringDQ);
		}

		public static Token CreateWordToken(int lineNo, int pos, string text)
		{
			return new Token(lineNo, pos, text, EnumGeneralTokenType.Word);
		}

		public static Token CreateWordToken(int pos, string text)
		{
			return new Token(-1, pos, text, EnumGeneralTokenType.Word);
		}

		public static Token CreateDelimiterToken(int lineNo, int pos, char delimiter)
		{
			return new Token(lineNo, pos, "" + delimiter, EnumGeneralTokenType.Delimiter);
		}

		public static Token CreateDelimiterToken(int pos, char delimiter)
		{
			return new Token(-1, pos, "" + delimiter, EnumGeneralTokenType.Delimiter);
		}

		public static Token CreateSpaceToken(int lineNo, int pos)
		{
			return new Token(lineNo, pos, "", EnumGeneralTokenType.Space);
		}

		public static Token CreateSpaceToken(int pos)
		{
			return new Token(-1, pos, "", EnumGeneralTokenType.Space);
		}

		public Token CloneObject()
		{
			Token t = new Token(LineNumber, CharacterPosition, Text, GeneralType);
			t.Tags = Tags.CloneObject();
			return t;
		}

	}

}
