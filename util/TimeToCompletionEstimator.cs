using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class TimeToCompletionEstimator
	{

		public struct TimeToCompletion
		{
			public static readonly TimeToCompletion INVALID = new TimeToCompletion(false);
			public static readonly TimeToCompletion ZERO = new TimeToCompletion(0, 0);

			public readonly long DurationMillisecondsSinceStart;
			public readonly long ExpectedTotalDurationMilliseconds;
			public readonly long ExpectedMillisecondsTillCompletion;

			private TimeToCompletion(bool b)
			{
				DurationMillisecondsSinceStart = -1;
				ExpectedTotalDurationMilliseconds = -1;
				ExpectedMillisecondsTillCompletion = -1;
			}

			public TimeToCompletion(long duration, long expectedTotalDuration)
			{
				this.DurationMillisecondsSinceStart = duration;
				if (this.DurationMillisecondsSinceStart < 0)
					this.DurationMillisecondsSinceStart = 0;
				this.ExpectedTotalDurationMilliseconds = expectedTotalDuration;
				if (this.ExpectedTotalDurationMilliseconds < 0)
					this.ExpectedTotalDurationMilliseconds = 0;
				this.ExpectedMillisecondsTillCompletion = ExpectedTotalDurationMilliseconds - DurationMillisecondsSinceStart;
				if (this.ExpectedMillisecondsTillCompletion < 0)
					this.ExpectedMillisecondsTillCompletion = 0;
			}

			public bool IsInvalid
			{
				get {
					if (DurationMillisecondsSinceStart < 0) return true;
					return false;
				}
			}

			public TimeSpan ExpectedTimeTillCompletion
			{
				get {
					return new TimeSpan(ExpectedMillisecondsTillCompletion * 10000);
				}
			}

		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		double expectedMaximum;
		long beginTimeStamp;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TimeToCompletionEstimator(int expectedMaximum)
		{
			this.expectedMaximum = expectedMaximum;
			this.beginTimeStamp = DateTime.Now.Ticks / 10000L;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public TimeToCompletion Estimate(int currentValue)
		{
			if (expectedMaximum == 0) return TimeToCompletion.ZERO;
			if (currentValue == 0) return TimeToCompletion.INVALID;

			double d = currentValue / expectedMaximum;

			long duration = (DateTime.Now.Ticks / 10000L) - beginTimeStamp;
			long expectedTotalDuration = (long)(duration / d);

			return new TimeToCompletion(duration, expectedTotalDuration);
		}

	}

}
