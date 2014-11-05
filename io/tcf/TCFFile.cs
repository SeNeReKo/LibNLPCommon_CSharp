using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;


using LibNLPCSharp.util;


namespace LibNLPCSharp.io.tcf
{

	public class TCFFile
	{

		public delegate bool TokenCheckDelegate(TCFToken token);

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		List<TCFToken> tokens;
		List<TCFSentence> sentences;
		List<TCFPoSTag> posTags;
		List<TCFLemma> lemmas;
		List<TCFTextSpan> textSpans;

		IDGenerator tokenIDGen;
		IDGenerator sentenceIDGen;
		IDGenerator posTagIDGen;
		IDGenerator lemmaIDGen;

		string languageID;
		string posTagSetName;
		string systemDocType;
		string text;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TCFFile()
		{
			tokens = new List<TCFToken>();
			sentences = new List<TCFSentence>();
			posTags = new List<TCFPoSTag>();
			lemmas = new List<TCFLemma>();
			textSpans = new List<TCFTextSpan>();

			tokenIDGen = new IDGenerator("T_");
			sentenceIDGen = new IDGenerator("s_");
			posTagIDGen = new IDGenerator("pt_");
			lemmaIDGen = new IDGenerator("le_");
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public TCFPoSTag[] PoSTags
		{
			get {
				return posTags.ToArray();
			}
		}

		public TCFSentence[] Sentences
		{
			get {
				return sentences.ToArray();
			}
		}

		public TCFLemma[] Lemmas
		{
			get {
				return lemmas.ToArray();
			}
		}

		public TCFToken[] Tokens
		{
			get {
				return tokens.ToArray();
			}
		}

		public TCFTextSpan[] TextSpans
		{
			get {
				return textSpans.ToArray();
			}
		}

		public string Text
		{
			get {
				return text;
			}
			set {
				if (value == null) {
					this.text = null;
					return;
				}
				value = value.Trim();
				if (value.Length == 0) {
					this.text = null;
					return;
				}
				this.text = value;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void LoadFromFile(string filePath)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(File.ReadAllText(filePath));

			// verify frame format

			if (!doc.DocumentElement.Name.Equals("D-Spin")) throw new Exception("Not a valid TCF file!");
			if (doc.DocumentElement.ChildNodes.Count != 1) throw new Exception("Not a valid TCF file!");

			XmlNode xTextCorpus = doc.DocumentElement.ChildNodes[0];
			if (!(xTextCorpus is XmlElement)) throw new Exception("Not a valid TCF file!");
			XmlElement xmlTextCorpus = (XmlElement)xTextCorpus;
			if (!xmlTextCorpus.Name.Equals("TextCorpus")) throw new Exception("Not a valid TCF file!");

			// seems to be valid. now parse the data.

			Reset();

			languageID = xmlTextCorpus.GetAttribute("lang");

			bool bHasTokens = false;
			foreach (XmlNode xNode in xmlTextCorpus.ChildNodes) {
				if (!(xNode is XmlElement)) continue;
				XmlElement xmlNode = (XmlElement)xNode;

				if (xmlNode.Name.Equals("text")) {
					this.text = xmlNode.InnerText;
				} else
				if (xmlNode.Name.Equals("tokens")) {
					bHasTokens = true;
					__ParseTokens(xmlNode, xmlNode.ChildNodes);
				} else
				if (xmlNode.Name.Equals("sentences")) {
					if (!bHasTokens) throw new Exception("Malformed TCF file: Expected \"tokens\" node before \"sentences\" node!");
					__ParseSentences(xmlNode, xmlNode.ChildNodes);
				} else
				if (xmlNode.Name.Equals("POStags")) {
					if (!bHasTokens) throw new Exception("Malformed TCF file: Expected \"tokens\" node before \"POStags\" node!");
					__ParsePoS(xmlNode, xmlNode.ChildNodes);
				} else {
					// ignore other elements
				}
			}

			Util.Noop();
		}

		public void Reset()
		{
			tokens.Clear();
			sentences.Clear();
			posTags.Clear();
			lemmas.Clear();
			textSpans.Clear();

			tokenIDGen.Reset();
			sentenceIDGen.Reset();
			posTagIDGen.Reset();
			lemmaIDGen.Reset();
		}

		private void __ParseTokens(XmlElement node, XmlNodeList children)
		{
			foreach (XmlNode xNode in children) {
				if (!(xNode is XmlElement)) continue;
				XmlElement x = (XmlElement)xNode;
				if (!(x.Name.Equals("token"))) throw new Exception("Malformed TCF file: Node with name \"" + x.Name + "\" found in list of tokens!");

				string id = x.GetAttribute("ID");
				if (id == null) throw new Exception("Malformed TCF file: Token node without ID detected!");

				string text = x.InnerText;
				if ((text == null) || (text.Length == 0)) throw new Exception("Malformed TCF file: Token node without text detected!");

				TCFToken token = new TCFToken(id, text);
				tokens.Add(token);

				// TODO: verify token IDs to detect duplicates!
			}
		}

		private void __ParseSentences(XmlElement node, XmlNodeList children)
		{
			Dictionary<string, TCFToken> tokenMap = CreateTokenMap();

			foreach (XmlNode xNode in children) {
				if (!(xNode is XmlElement)) continue;
				XmlElement x = (XmlElement)xNode;
				if (!(x.Name.Equals("sentence"))) throw new Exception("Malformed TCF file: Node with name \"" + x.Name + "\" found in list of sentences!");

				string id = x.GetAttribute("ID");
				if (id == null) throw new Exception("Malformed TCF file: Sentence node without ID detected!");

				string tokenIDs = x.GetAttribute("tokenIDs");
				if (tokenIDs == null) throw new Exception("Malformed TCF file: Sentence node without tokenIDs detected!");
				tokenIDs = tokenIDs.Trim();
				if (tokenIDs.Length == 0) throw new Exception("Malformed TCF file: Sentence node without tokenIDs detected!");

				string[] individualTokenIDs = tokenIDs.Split(' ');
				TCFToken[] sentenceTokens = new TCFToken[individualTokenIDs.Length];
				for (int i = 0; i < individualTokenIDs.Length; i++) {
					string tokenID = individualTokenIDs[i];
					TCFToken t;
					if (tokenMap.TryGetValue(tokenID, out t)) {
						sentenceTokens[i] = t;
					} else {
						throw new Exception("Sentence \"" + id + "\" contains reference to unknown token: \"" + tokenID + "\"");
					}
				}

				TCFSentence sentence = new TCFSentence(id, sentenceTokens);
				sentences.Add(sentence);

				// TODO: verify sentence IDs to detect duplicates!
			}
		}

		public Dictionary<string, TCFToken> CreateTokenMap()
		{
			return __CreateMap(tokens);
		}

		public Dictionary<string, T> __CreateMap<T>(List<T> list)
			where T : INamedTCFElement
		{
			Dictionary<string, T> map = new Dictionary<string, T>();

			foreach (T t in list) {
				if (t.ID == null) continue;	// skip elements without IDs	-> TODO
				if (map.ContainsKey(t.ID)) {
					throw new Exception("Duplicate IDs detected!");
				} else {
					map.Add(t.ID, t);
				}
			}

			return map;
		}

		private void __ParsePoS(XmlElement node, XmlNodeList children)
		{
			Dictionary<string, TCFToken> tokenMap = CreateTokenMap();

			posTagSetName = node.GetAttribute("tagset");
			if (posTagSetName != null) {
				posTagSetName = posTagSetName.Trim();
				if (posTagSetName.Length == 0) posTagSetName = null;
			}

			foreach (XmlNode xNode in children) {
				if (!(xNode is XmlElement)) continue;
				XmlElement x = (XmlElement)xNode;
				if (!(x.Name.Equals("tag"))) throw new Exception("Malformed TCF file: Node with name \"" + x.Name + "\" found in list of PoS tags!");

				string id = x.GetAttribute("ID");
				if (id == null) throw new Exception("Malformed TCF file: PoS tag node without ID detected!");

				string tokenIDs = x.GetAttribute("tokenIDs");
				if (tokenIDs == null) throw new Exception("Malformed TCF file: PoS tag node without tokenIDs detected!");
				tokenIDs = tokenIDs.Trim();
				if (tokenIDs.Length == 0) throw new Exception("Malformed TCF file: PoS tag node without tokenIDs detected!");

				string[] individualTokenIDs = tokenIDs.Split(' ');
				if (individualTokenIDs.Length != 1) throw new Exception("PoS tag nodes referring to multiple tokens is not supported!");

				TCFToken t;
				if (!tokenMap.TryGetValue(individualTokenIDs[0], out t)) {
					throw new Exception("PoS tag \"" + id + "\" contains reference to unknown token: \"" + individualTokenIDs[0] + "\"");
				}

				string text = x.InnerText;
				if ((text == null) || (text.Length == 0) || (text.Trim().Length != text.Length))
					throw new Exception("Malformed TCF file: PoS tag contains invalid text!");

				TCFPoSTag tag = new TCFPoSTag(id, t, text);
				posTags.Add(tag);

				// TODO: verify sentence IDs to detect duplicates!
			}
		}

		public Dictionary<string, List<TCFToken>> CreateTokenOverview()
		{
			Dictionary<string, List<TCFToken>> map = new Dictionary<string, List<TCFToken>>();

			foreach (TCFToken t in tokens) {
				List<TCFToken> tokenList;
				if (!map.TryGetValue(t.Text, out tokenList)) {
					tokenList = new List<TCFToken>();
					map.Add(t.Text, tokenList);
				}
				tokenList.Add(t);
			}

			return map;
		}

		public Dictionary<string, List<TCFToken>> CreateTokenOverview(TokenCheckDelegate check)
		{
			Dictionary<string, List<TCFToken>> map = new Dictionary<string, List<TCFToken>>();

			foreach (TCFToken t in tokens) {
				if (!check(t)) continue;

				List<TCFToken> tokenList;
				if (!map.TryGetValue(t.Text, out tokenList)) {
					tokenList = new List<TCFToken>();
					map.Add(t.Text, tokenList);
				}
				tokenList.Add(t);
			}

			return map;
		}

	}

}
