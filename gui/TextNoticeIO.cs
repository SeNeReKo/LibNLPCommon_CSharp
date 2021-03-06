﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace LibNLPCSharp.gui
{

	[Serializable()]
	[XmlRoot("TextNotice")]
	public class TextNoticeIO
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

		[XmlElement("title")]
		public string Title
		{
			get;
			set;
		}

		[XmlElement("text")]
		public string Message
		{
			get;
			set;
		}

		[XmlElement("id")]
		public string ID
		{
			get;
			set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

	}

}
