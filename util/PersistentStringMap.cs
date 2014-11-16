using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class PersistentStringMap
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private Dictionary<string, string> map;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public PersistentStringMap()
		{
			map = new Dictionary<string, string>();
		}

		public PersistentStringMap(string filePath)
		{
			map = new Dictionary<string, string>();

			this.FilePath = filePath;
			if (File.Exists(filePath)) {
				Load();
			}
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string this[string key]
		{
			get {
				string v;
				if (map.TryGetValue(key, out v)) return v;
				return null;
			}
			set {
				bool bChanged = false;

				string v;
				if (map.TryGetValue(key, out v)) {
					if (value == null) {
						map.Remove(key);
						bChanged = true;
					} else {
						if (!v.Equals(value)) {
							map.Remove(key);
							map.Add(key, value);
							bChanged = true;
						}
					}
				} else {
					if (v != null) {
						map.Add(key, value);
						bChanged = true;
					}
				}

				if (bChanged && AutoSaveEnabled && (FilePath != null)) Save();
			}
		}

		public bool AutoSaveEnabled
		{
			get;
			set;
		}

		public string FilePath
		{
			get;
			set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Save()
		{
			if (FilePath == null) throw new Exception("No file path defined!");

			StringWriter sw = new StringWriter();

			foreach (KeyValuePair<string, string> kvp in map) {
				sw.Write(__Encode(kvp.Key));
				sw.Write("=");
				sw.WriteLine(__Encode(kvp.Value));
			}

			File.WriteAllText(FilePath, sw.ToString());
		}

		public void Load()
		{
			string text = File.ReadAllText(FilePath);
			StringReader sr = new StringReader(text);

			Dictionary<string, string>  map = new Dictionary<string, string>();

			string line;
			while ((line = sr.ReadLine()) != null) {
				if (line.Length == 0) continue;
				KeyValuePair<string, string> kvp = __Decode(line);
				map.Add(kvp.Key, kvp.Value);
			}

			this.map = map;
		}

		private string __Encode(string s)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in sb.ToString()) {
				if (c == 10) {
					sb.Append("\\r");
				} else
				if (c == 13) {
					sb.Append("\\n");
				} else
				if (c == '=') {
					sb.Append('\\');
				}
				sb.Append(c);
			}
			return sb.ToString();
		}

		private KeyValuePair<string, string> __Decode(string line)
		{
			string key = "";
			string value;

			bool bIsKey = true;

			StringBuilder sb = new StringBuilder();
			bool bMasked = false;
			foreach (char c in line) {
				if (bMasked) {
					switch (c) {
						case 'r':
							sb.Append('\r');
							break;
						case 'n':
							sb.Append('\n');
							break;
						default:
							sb.Append(c);
							break;
					}
					bMasked = false;
				} else {
					if (bIsKey && (c == '=')) {
						if (sb.Length == 0) throw new Exception("File format error!");
						key = sb.ToString();
						sb = new StringBuilder();
						bIsKey = false;
					} else
					if (c == '\\') {
						bMasked = true;
					} else {
						sb.Append(c);
					}
				}
			}
			if (bMasked) throw new Exception("File format error!");

			value = sb.ToString();

			return new KeyValuePair<string, string>(key, value);
		}

		public string Get(string key)
		{
			return this[key];
		}

		public string GetAsStr(string key)
		{
			return this[key];
		}

		public int GetAsInt32(string key)
		{
			string s = this[key];
			if (s == null) return -1;
			return Int32.Parse(s);
		}

		public int GetAsInt32(string key, int defaultValue)
		{
			string s = this[key];
			if (s == null) return defaultValue;
			return Int32.Parse(s);
		}

		public void Put(string key, string value)
		{
			this[key] = value;
		}

		public void Put(string key, int value)
		{
			this[key] = "" + value;
		}

		public void Remove(string key)
		{
			this[key] = null;
		}

	}

}
