using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.app
{

	public class AppInfo
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		public static readonly AppInfo Instance = new AppInfo();

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		DirectoryInfo appBaseDir;
		string varDirPath;
		string etcDirPath;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		private AppInfo()
		{
			this.appBaseDir = new DirectoryInfo(Util.RemoveSeparatorChar(Util.AppDirPath));

			DirectoryInfo d;

			d = __DetectDir(appBaseDir, "var");
			if (d != null) varDirPath = Util.AddSeparatorChar(d.FullName);

			d = __DetectDir(appBaseDir, "etc");
			if (d != null) etcDirPath = Util.AddSeparatorChar(d.FullName);
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public DirectoryInfo EtcDir
		{
			get {
				return new DirectoryInfo(Util.RemoveSeparatorChar(etcDirPath));
			}
		}

		public string EtcDirPath
		{
			get {
				return etcDirPath;
			}
		}

		public DirectoryInfo VarDir
		{
			get {
				return new DirectoryInfo(Util.RemoveSeparatorChar(varDirPath));
			}
		}

		public string VarDirPath
		{
			get {
				return varDirPath;
			}
		}

		public DirectoryInfo AppBaseDir
		{
			get {
				return appBaseDir;
			}
		}

		public string AppBaseDirPath
		{
			get {
				return Util.AppDirPath;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private DirectoryInfo __DetectDir(DirectoryInfo dir, string dirName)
		{
			DirectoryInfo[] dir2 = dir.GetDirectories(dirName, SearchOption.TopDirectoryOnly);
			if ((dir2 == null) || (dir2.Length == 0)) {
				DirectoryInfo parentDir = dir.Parent;
				if (parentDir == null) return null;

				if (parentDir.Name.Equals("Dev")) return null;

				return __DetectDir(parentDir, dirName);
			} else {
				return dir2[0];
			}
		}

		public bool CreateVarDir()
		{
			if (varDirPath == null) {
				varDirPath = Util.AddSeparatorChar(appBaseDir.FullName) + "var";
				Directory.CreateDirectory(varDirPath);
				varDirPath += Path.DirectorySeparatorChar;
				return true;
			} else {
				return false;
			}
		}

		public bool CreateEtcDir()
		{
			if (etcDirPath == null) {
				etcDirPath = Util.AddSeparatorChar(appBaseDir.FullName) + "etc";
				Directory.CreateDirectory(etcDirPath);
				etcDirPath += Path.DirectorySeparatorChar;
				return true;
			} else {
				return false;
			}
		}

	}

}
