using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using LibNLPCSharp.util;


namespace LibNLPCSharp.io
{

	public class CSVWriter : IDisposable
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		TextWriter writer;
		char delimiter;
		Stream stream;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public CSVWriter(FileInfo file, char delimiter)
		{
			this.delimiter = delimiter;
			this.stream = file.OpenWrite();
			this.writer = new StreamWriter(stream, Encoding.UTF8);
		}

		public CSVWriter(TextWriter writer, char delimiter)
		{
			this.delimiter = delimiter;
			this.writer = writer;
		}

		~CSVWriter()
		{
			Dispose();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Close()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (writer != null) {
				if (stream != null) {
					stream.SetLength(stream.Position);
				}
				try {
					writer.Close();
				} catch {
				}
				writer = null;
				System.GC.SuppressFinalize(this);
			}
		}

		public void WriteLine(params string[] lineElements)
		{
			if (writer == null) throw new Exception("Already closed!");

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < lineElements.Length; i++) {
				if (i > 0) sb.Append(delimiter);
				sb.Append(lineElements[i]);
			}
			// sb.Append((char)13);
			// sb.Append((char)10);

			writer.WriteLine(sb.ToString());
		}

		public void WriteLines(IEnumerable<string[]> lines)
		{
			foreach (string[] line in lines) {
				WriteLine(line);
			}
		}

	}

}
