﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util.model
{

	public class Multicaster
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		DelayedEvent[] events;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public Multicaster(params DelayedEvent[] events)
		{
			this.events = events;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void FireEvent()
		{
			foreach (DelayedEvent e in events) {
				e.FireEvent();
			}
		}

	}

}
