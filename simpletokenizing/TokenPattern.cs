using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.simpletokenizing
{

	public class TokenPattern
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private EnumGeneralTokenType GeneralType;
		private string Text;
		private bool bMatchText;
		private bool bMatchDelimiter;
		private bool bCaseSensitive = true;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		private TokenPattern()
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool IsMatchAny
		{
			get {
				return !(bMatchText || bMatchDelimiter);
			}
		}

		public bool IsContent
		{
			get;
			set;
		}

		public SKVPSet Tags
		{
			get;
			set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public TokenPattern ToCaseInsensitive()
		{
			TokenPattern tp = new TokenPattern();
			tp.GeneralType = GeneralType;
			if (Text != null) {
				tp.Text = Text.ToLower();
			}
			tp.bMatchDelimiter = bMatchDelimiter;
			tp.bMatchText = bMatchText;
			tp.IsContent = IsContent;
			tp.Tags = Tags;
			tp.bCaseSensitive = false;
			return tp;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			switch (GeneralType) {
				case EnumGeneralTokenType.Delimiter:
					sb.Append('D');
					if (bMatchDelimiter) {
						sb.Append('\'');
						sb.Append(Text.Replace("\'", "\\\'"));
						sb.Append('\'');
					} else {
						sb.Append('*');
					}
					break;
				case EnumGeneralTokenType.Space:
					sb.Append("SPACE");
					break;
				case EnumGeneralTokenType.StringDQ:
					sb.Append("T");
					if (bMatchText) {
						sb.Append('\"');
						sb.Append(Text.Replace("\"", "\\\""));
						sb.Append('\"');
					} else {
						sb.Append('*');
					}
					break;
				case EnumGeneralTokenType.StringSQ:
					throw new Exception();
				case EnumGeneralTokenType.Word:
					sb.Append('W');
					if (bMatchText) {
						sb.Append('\"');
						sb.Append(Text.Replace("\"", "\\\""));
						sb.Append('\"');
					} else {
						sb.Append('*');
					}
					break;
				default:
					throw new Exception();
			}

			if (IsContent) {
				sb.Append('!');
			} else {
				sb.Append('.');
			}

			return sb.ToString();
		}

		public bool Match(Token token)
		{
			if (token.GeneralType != GeneralType) return false;

			switch (GeneralType) {
				case EnumGeneralTokenType.Space:
					return true;
				case EnumGeneralTokenType.Delimiter:
					if (!bMatchDelimiter) return true;
					if (bCaseSensitive) {
						if (token.Text[0] == Text[0]) return true;
					} else {
						if (token.Text[0] == char.ToLower(Text[0])) return true;
					}
					return false;
				case EnumGeneralTokenType.Word:
					if (!bMatchText) return true;
					if (bCaseSensitive) {
						return token.Text.Equals(Text);
					} else {
						return token.Text.ToLower().Equals(Text);
					}
				case EnumGeneralTokenType.StringSQ:
					if (!bMatchText) return true;
					if (bCaseSensitive) {
						return token.Text.Equals(Text);
					} else {
						return token.Text.ToLower().Equals(Text);
					}
				case EnumGeneralTokenType.StringDQ:
					if (!bMatchText) return true;
					if (bCaseSensitive) {
						return token.Text.Equals(Text);
					} else {
						return token.Text.ToLower().Equals(Text);
					}
				default:
					throw new Exception();
			}
		}

		public static TokenPattern MatchWord(string text, bool bIsContent)
		{
			if (text == null) throw new Exception("text == null!");
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Word;
			p.bMatchText = true;
			p.Text = text;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchWord(string text)
		{
			if (text == null) throw new Exception("text == null!");
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Word;
			p.bMatchText = true;
			p.Text = text;
			return p;
		}

		public static TokenPattern MatchAnyWord(bool bIsContent)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Word;
			p.bMatchText = false;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchAnyWord()
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Word;
			p.bMatchText = false;
			return p;
		}

		public static TokenPattern MatchStringSQ(string text, bool bIsContent)
		{
			if (text == null) throw new Exception("text == null!");
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringSQ;
			p.bMatchText = true;
			p.Text = text;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchStringSQ(string text)
		{
			if (text == null)
				throw new Exception("text == null!");
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringSQ;
			p.bMatchText = true;
			p.Text = text;
			return p;
		}

		public static TokenPattern MatchAnyStringSQ(bool bIsContent)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringSQ;
			p.bMatchText = false;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchAnyStringSQ()
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringSQ;
			p.bMatchText = false;
			return p;
		}

		public static TokenPattern MatchStringDQ(string text, bool bIsContent)
		{
			if (text == null) throw new Exception("text == null!");
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringDQ;
			p.bMatchText = true;
			p.Text = text;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchStringDQ(string text)
		{
			if (text == null) throw new Exception("text == null!");
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringDQ;
			p.bMatchText = true;
			p.Text = text;
			return p;
		}

		public static TokenPattern MatchAnyStringDQ(bool bIsContent)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringDQ;
			p.bMatchText = false;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchAnyStringDQ()
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.StringDQ;
			p.bMatchText = false;
			return p;
		}

		public static TokenPattern MatchDelimiter(char delimiter, bool bIsContent)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Delimiter;
			p.bMatchDelimiter = true;
			p.Text = "" + delimiter;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchDelimiter(char delimiter)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Delimiter;
			p.bMatchDelimiter = true;
			p.Text = "" + delimiter;
			return p;
		}

		public static TokenPattern MatchDelimiter(string delimiters, bool bIsContent)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Delimiter;
			p.bMatchDelimiter = true;
			p.Text = delimiters;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchDelimiter(string delimiters)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Delimiter;
			p.bMatchDelimiter = true;
			p.Text = delimiters;
			return p;
		}

		public static TokenPattern MatchAnyDelimiter(bool bIsContent)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Delimiter;
			p.bMatchDelimiter = false;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchAnyDelimiter()
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Delimiter;
			p.bMatchDelimiter = false;
			return p;
		}

		public static TokenPattern MatchSpace(bool bIsContent)
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Space;
			p.IsContent = bIsContent;
			return p;
		}

		public static TokenPattern MatchSpace()
		{
			TokenPattern p = new TokenPattern();
			p.GeneralType = EnumGeneralTokenType.Space;
			return p;
		}

	}

}
