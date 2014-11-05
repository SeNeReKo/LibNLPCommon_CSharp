using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace LibNLPCSharp.io.texttokenizing
{

	/// <summary>
	/// This is a reader that ready characters from an underlying stream. It allows
	/// push backs to ease implementations processing the character data.
	/// </summary>
	public class PushbackCharacterReader : IDisposable
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		TextReader reader;
		List<StreamChar> characters;
		int pos;
		int lineNo;
		int colNo;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public PushbackCharacterReader(TextReader reader)
		{
			this.reader = reader;
			characters = new List<StreamChar>();
		}

		public void Dispose()
		{
			Close();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool HasNext
		{
			get {
				StreamChar c = Read();
				if (c.IsEOS) return false;
				else {
					Unread(c);
					return true;
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Close()
		{
			if (reader == null) return;
			try {
				characters.Clear();
				reader.Close();
			} catch {
			}
		}

		public StreamChar Read()
		{
			if (reader == null) throw new Exception("Stream is closed.");

			if (characters.Count == 0) {
				StreamChar c = new StreamChar(reader.Read(), pos, lineNo + 1, colNo + 1);
				if (c.CharI >= 0) {
					pos++;
					if (c.CharI == 13) {
						lineNo++;
						colNo = 0;
					} else {
						colNo++;
					}
				}
				return c;
			} else {
				StreamChar c = characters[characters.Count - 1];
				characters.RemoveAt(characters.Count - 1);
				return c;
			}
		}

		public void Unread(StreamChar c)
		{
			if (reader == null) throw new Exception("Stream is closed.");

			characters.Add(c);
		}

	}

}
