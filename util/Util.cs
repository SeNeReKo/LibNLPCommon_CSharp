using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class Util
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		public static readonly string CRLF = "" + ((char)13) + ((char)10);

		public static readonly NumberFormatInfo NFI_DOUBLE;

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private static string appDirPathCache;

		public static readonly RandomNumberGenerator RNG;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		static Util()
		{
			NFI_DOUBLE = new CultureInfo("en-US", false).NumberFormat;
			NFI_DOUBLE.NumberDecimalDigits = 15;

			RNG = new RandomNumberGenerator();
		}

		private Util()
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public static string AppDirPath
		{
			get {
				if (appDirPathCache != null) return appDirPathCache;
				System.Reflection.Module[] modules = System.Reflection.Assembly.GetExecutingAssembly().GetModules();
				string aPath = System.IO.Path.GetDirectoryName(modules[0].FullyQualifiedName);
				if ((aPath != "") && (aPath[aPath.Length - 1] != Path.DirectorySeparatorChar)) aPath += Path.DirectorySeparatorChar;
				appDirPathCache = aPath;
				return appDirPathCache;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static bool Equals(string s1, string s2)
		{
			if (s1 == null) {
				if (s2 == null) {
					return true;
				} else {
					return false;
				}
			} else {
				if (s1 == null) {
					return false;
				} else {
					bool b = s1.Equals(s2);
					return b;
				}
			}
		}

		public static void Noop()
		{
		}

		public static string Normalize(string s)
		{
			if (s == null) return null;

			s = s.Trim();
			if (s.Length == 0) return null;

			return s;
		}

		public static int Max(params int[] values)
		{
			int max = Int32.MinValue;
			for (int i = 0; i < values.Length; i++) {
				if (values[i] > max) max = values[i];
			}
			return max;
		}

		public static int Min(params int[] values)
		{
			int min = Int32.MaxValue;
			for (int i = 0; i < values.Length; i++) {
				if (values[i] < min) min = values[i];
			}
			return min;
		}

		/// <summary>
		/// Randomize a list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="bShallowCloneFirst">If <code>true</code> clone the list first before processing. If you specify
		/// <code>false</code> here the list will get modified. After processing it will contain no more elements</param>
		/// <returns></returns>
		public static List<T> Randomize<T>(List<T> source, bool bShallowCloneFirst)
		{
			List<T> ret = new List<T>();

			if (bShallowCloneFirst) source = new List<T>(source);

			byte[] temp = new byte[8];
			while (source.Count > 0) {
				RNG.GetBytes(temp);
				double d = (double)BitConverter.ToUInt64(temp, 0) / UInt64.MaxValue;
				int n = (int)(d * source.Count);
				ret.Add(source[n]);
				source.RemoveAt(n);
			}

			return ret;
		}

		public static string AddSeparatorChar(string path)
		{
			if (path.EndsWith("/") || path.EndsWith("\\")) return path;
			return path + Path.DirectorySeparatorChar;
		}

		public static string RemoveSeparatorChar(string path)
		{
			if ((path.Length == 3) && (path[1] == ':') && ((path[2] == '\\') || (path[2] == '/'))) {
				return path.Substring(0, path.Length - 1) + "\\";
			}

			if (path.EndsWith("/") || path.EndsWith("\\")) {
				return path.Substring(0, path.Length - 1);
			} else
				return path;
		}

	}

}
