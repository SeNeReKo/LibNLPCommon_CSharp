using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class PersistentProperties : AbstractProperties
	{

		private class MyProperty
		{
			public string Key;
			public string Value;

			public MyProperty(string key, string value)
			{
				this.Key = key;
				this.Value = value;
			}

			public void Write(TextWriter tw)
			{
				tw.WriteLine(Key + "=" + Value);
			}

			public static MyProperty TryParse(string line)
			{
				if (line.StartsWith("#")) return null;
				int pos = line.IndexOf('=');
				if (pos <= 0) return null;
				string key = line.Substring(0, pos);
				string value = line.Substring(pos + 1);
				if (value.Length == 0) value = null;
				return new MyProperty(key, value);
			}
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		DelayedEvent evt;
		List<object> items;
		Dictionary<string, MyProperty> properties;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public PersistentProperties(string filePath)
			: this(new FileInfo(filePath))
		{
		}

		public PersistentProperties(FileInfo file)
			: this()
		{
			if (file != null) {
				if (file.Exists) {
					Load(file);
				} else {
					File = file;
				}
			}
		}

		public PersistentProperties()
		{
			items = new List<object>();
			properties = new Dictionary<string, MyProperty>();

			ChangedFlag = new ChangedFlag();
			evt = new DelayedEvent(null, 1500);
			evt.OnEventDelayed += new DelayedEvent.OnEventDelegate(evt_OnTimer);

			AutoSave = true;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public ChangedFlag ChangedFlag
		{
			get;
			private set;
		}

		public override string this[string key]
		{
			get {
				MyProperty value;
				lock (this) {
					if (!properties.TryGetValue(key, out value)) {
						return null;
					}
				}
				return value.Value;
			}
			set {
				MyProperty p;
				bool bChanged = false;
				lock (this) {
					if (properties.TryGetValue(key, out p)) {
						p.Value = value;
						bChanged = true;
					} else {
						if (value == null) return;
						p = new MyProperty(key, value);
						properties.Add(key, p);
						items.Add(p);
						bChanged = true;
					}
				}
				if (bChanged) {
					ChangedFlag.IsChanged = true;
					if (AutoSave) evt.FireEvent();
				}
			}
		}

		public bool AutoSave
		{
			get;
			set;
		}

		public FileInfo File
		{
			get;
			set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public bool ContainsKey(string key)
		{
			return properties.ContainsKey(key);
		}

		private void evt_OnTimer()
		{
			TrySave();
		}

		public bool TrySave()
		{
			FileInfo file = File;
			if (file == null) return false;
			if (!ChangedFlag.IsChanged) return false;
			try {
				Save(file);
				return true;
			} catch (Exception ee) {
				return false;
			}
		}

		public void Save()
		{
			FileInfo file = File;
			if (file == null) throw new Exception("Not associated with a file!");
			if (!ChangedFlag.IsChanged) return;
			Save(file);
		}

		public void Load(FileInfo file)
		{
			lock (this) {
				using (StreamReader reader = new StreamReader(file.OpenRead(), Encoding.UTF8, true)) {
					Load(reader);
				}
			}
			this.File = file;
		}

		public void Save(FileInfo file)
		{
			FileInfo tempFile = new FileInfo(file.FullName + ".tmp");
			lock (this) {
				using (Stream stream = tempFile.OpenWrite()) {
					using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8)) {
						Save(writer);
						stream.SetLength(stream.Position);
					}
				}

				if (file.Exists) file.Delete();
				tempFile.MoveTo(file.FullName);
			}
			this.File = file;
		}

		public void Save(TextWriter writer)
		{
			lock (this) {
				foreach (object item in items) {
					if (item is string) {
						writer.WriteLine((string)item);
					} else
					if (item is MyProperty) {
						((MyProperty)item).Write(writer);
					} else {
						throw new ImplementationErrorException();
					}
				}
				ChangedFlag.SuppressEvents();
				ChangedFlag.IsChanged = false;
			}
			ChangedFlag.ResumeEvents();
		}

		public void Load(TextReader reader)
		{
			List<object> items = new List<object>();
			Dictionary<string, MyProperty> properties = new Dictionary<string, MyProperty>();

			string line;
			while ((line = reader.ReadLine()) != null) {
				MyProperty p = MyProperty.TryParse(line);
				if (p == null) {
					items.Add(line);
				} else {
					items.Add(p);
					properties.Remove(p.Key);
					properties.Add(p.Key, p);
				}
			}

			lock (this) {
				this.properties = properties;
				this.items = items;
				ChangedFlag.SuppressEvents();
				ChangedFlag.IsChanged = false;
			}
			ChangedFlag.ResumeEvents();
		}

	}

}
