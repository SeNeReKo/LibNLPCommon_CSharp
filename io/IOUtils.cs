using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.io
{

	public static class IOUtils
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static string AddDirectorySeparatorChar(string path)
		{
			if (path.EndsWith("" + Path.DirectorySeparatorChar)) {
				return path;
			}

			return path + Path.DirectorySeparatorChar;
		}

		/// <summary>
		/// Create a file path such as "/somepath/filename-1.txt" from index 1 and original file path "/somepath/filename.txt"
		/// </summary>
		/// <param name="filePath">The original file path, f.e. "/somepath/filename.txt"</param>
		/// <param name="index">The index that will be appended to the file name</param>
		/// <returns>Returns the resulting file path</returns>
		public static string DeriveFilePath(string filePath, int index)
		{
			string dirPath = Path.GetDirectoryName(filePath);
			string baseFileName = Path.GetFileNameWithoutExtension(filePath);
			string ext = Path.GetExtension(filePath);

			return AddDirectorySeparatorChar(dirPath) + baseFileName + "-" + index + "." + ext;
		}

	}

}
