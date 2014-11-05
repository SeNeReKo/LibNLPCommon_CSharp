using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.io
{

	public class CSVReader : IDisposable, IEnumerable<string[]>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		TextReader reader;
		char delimiter;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public CSVReader(FileInfo file, char delimiter)
		{
			FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			reader = new StreamReader(fs, Encoding.UTF8, true);
			this.delimiter = delimiter;
		}

		public CSVReader(TextReader reader, char delimiter)
		{
			this.delimiter = delimiter;
			this.reader = reader;
		}

		~CSVReader()
		{
			Dispose();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool TrimLineArray
		{
			get;
			set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Close()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (reader != null) {
				try {
					reader.Close();
				} catch {
				}
				reader = null;
				System.GC.SuppressFinalize(this);
			}
		}

		public string[] ReadLine()
		{
			if (reader == null) return null;

			string line = reader.ReadLine();
			if (line == null) return null;

			string[] elements = line.Split(delimiter);

			if (TrimLineArray) {
				int n = elements.Length - 1;
				while (n >= 0) {
					if (elements[n].Length > 0) break;
					n--;
				}
				n++;
				string[] elements2 = new string[n];
				for (int i = 0; i < n; i++) {
					elements2[i] = elements[i];
				}
				elements = elements2;
			}

			return elements;
		}

		public IEnumerator<string[]> GetEnumerator()
		{
			string[] lines;
			while ((lines = ReadLine()) != null) {
				yield return lines;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			string[] lines;
			while ((lines = ReadLine()) != null) {
				yield return lines;
			}
		}

	}

}
