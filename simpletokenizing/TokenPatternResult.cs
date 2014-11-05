using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.simpletokenizing
{

	public class TokenPatternResult
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		Token[] matchedTokens;
		TokenPattern[] pattern;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TokenPatternResult(string name, TokenPattern[] pattern, Token[] matchedTokens, Token[] contentTokens)
		{
			this.Name = name;
			this.pattern = pattern;
			this.matchedTokens = matchedTokens;
			this.ContentTokens = contentTokens;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		/// <summary>
		/// A name this pattern matching result may be associated with. This can be used for identification.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// The pattern that has been matched
		/// </summary>
		public TokenPattern[] Pattern
		{
			get {
				return pattern;
			}
		}

		/// <summary>
		/// The tokens that have been matched
		/// </summary>
		public Token[] MatchedTokens
		{
			get {
				return matchedTokens;
			}
		}

		/// <summary>
		/// The content tokens (only) that have been matched. These content tokens contain the tags
		/// the matching patterns originally had.
		/// </summary>
		public Token[] ContentTokens
		{
			get;
			private set;
		}

		/// <summary>
		/// The tags the matched tokens are associated with
		/// </summary>
		public SKVPSet Tags
		{
			get {
				SKVPSet ret = new SKVPSet();
				foreach (Token t in matchedTokens) {
					ret.AddRange(t.Tags);
				}
				return ret;
			}
		}

		/// <summary>
		/// The tags the matched content tokens are associated with
		/// </summary>
		public SKVPSet TagsContent
		{
			get {
				SKVPSet ret = new SKVPSet();
				for (int i = 0; i < pattern.Length; i++) {
					if (pattern[i].IsContent) {
						ret.AddRange(matchedTokens[i].Tags);
					}
				}
				return ret;
			}
		}

		/// <summary>
		/// A string representation of the content
		/// </summary>
		public string Content
		{
			get {
				if (ContentTokens == null) return null;

				StringBuilder sb = new StringBuilder();
				foreach (Token t in ContentTokens) {
					switch (t.GeneralType) {
						case EnumGeneralTokenType.Delimiter:
							sb.Append(t.Text);
							break;
						case EnumGeneralTokenType.Space:
							sb.Append(" ");
							break;
						case EnumGeneralTokenType.StringDQ:
							sb.Append('\"');
							sb.Append(t.Text.Replace("\"", "\\\""));
							sb.Append('\"');
							break;
						case EnumGeneralTokenType.StringSQ:
							sb.Append('\'');
							sb.Append(t.Text.Replace("\'", "\\\'"));
							sb.Append('\'');
							break;
						case EnumGeneralTokenType.Word:
							sb.Append(t.Text);
							break;
						default:
							throw new Exception();
					}
				}
				return sb.ToString();
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public Token[] GetContentTokensByTag(string tagName, string tagValue)
		{
			List<Token> ret = new List<Token>();
			foreach (Token t in ContentTokens) {
				SKVP kvp = t.Tags.Get(tagName);
				if (kvp != null) {
					if (kvp.ContainsValue(tagValue)) {
						ret.Add(t);
					}
				}
			}
			return ret.ToArray();
		}

		public Token[] GetContentTokensByTag(string tagName)
		{
			List<Token> ret = new List<Token>();
			foreach (Token t in ContentTokens) {
				SKVP kvp = t.Tags.Get(tagName);
				if (kvp != null) {
					ret.Add(t);
				}
			}
			return ret.ToArray();
		}

		public Token GetContentTokenByTag(string tagName, string tagValue)
		{
			foreach (Token t in ContentTokens) {
				SKVP kvp = t.Tags.Get(tagName);
				if (kvp != null) {
					if (kvp.ContainsValue(tagValue)) {
						return t;
					}
				}
			}
			return null;
		}

		public Token GetContentTokenByTag(string tagName)
		{
			foreach (Token t in ContentTokens) {
				SKVP kvp = t.Tags.Get(tagName);
				if (kvp != null) {
					return t;
				}
			}
			return null;
		}

	}

}
