using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.gui
{

	public class TextNoticeMgr
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		DirectoryInfo dir;
		Dictionary<string, FileInfo> files;
		string[] ids;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TextNoticeMgr(string dirPath)
		{
			dir = new DirectoryInfo(dirPath);

			files = new Dictionary<string, FileInfo>();
			foreach (FileInfo fi in dir.GetFiles()) {
				if (fi.Name.EndsWith(".xml")) {
					files.Add(fi.Name.Substring(0, fi.Name.Length - 4), fi);
				}
			}

			ids = files.Keys.ToArray();
			Array.Sort(ids);
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string[] IDs
		{
			get {
				return ids;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public bool ShowNotice(PersistentProperties pp, string id)
		{
			FileInfo fi;
			if (files.TryGetValue(id, out fi)) {
				TextNoticeForm.Show(pp, dir, id);
				return true;
			} else {
				return false;
			}
		}

		public static string DetectNoticesPath(string baseSearchPath)
		{
			DirectoryInfo di = new DirectoryInfo(baseSearchPath);
			while (di != null) {
				DirectoryInfo di2 = new DirectoryInfo(Util.AddSeparatorChar(di.FullName) + "notices");
				if (di2.Exists) return di2.FullName;
				di = di.Parent;
			}
			return null;
		}

	}

}
