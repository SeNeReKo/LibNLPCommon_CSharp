using System;


namespace LibNLPCSharp.util
{

	///<summary>
	/// Represents a pseudo-random number generator, a device that produces random data.
	///</summary>
	public class RandomNumberGenerator : System.Security.Cryptography.RandomNumberGenerator
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		System.Security.Cryptography.RandomNumberGenerator rng;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		///<summary>
		/// Creates an instance of the default implementation of a cryptographic random number generator that can be used to generate random data.
		///</summary>
		public RandomNumberGenerator()
		{
			rng = System.Security.Cryptography.RandomNumberGenerator.Create();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		///<summary>
		/// Returns a random number between 0.0 and 1.0.
		///</summary>
		public double NextDouble()
		{
			byte[] b = new byte[4];
			rng.GetBytes(b);
			return BitConverter.ToUInt32(b, 0) / ((double)UInt32.MaxValue);
		}

		///<summary>
		/// Returns a random number between 0.0 and 1.0.
		///</summary>
		public double NextDouble(double maxValue)
		{
			byte[] b = new byte[4];
			rng.GetBytes(b);
			return (maxValue * BitConverter.ToUInt32(b, 0)) / ((double)UInt32.MaxValue);
		}

		///<summary>
		/// Returns a random number within the specified range.
		///</summary>
		///<param name=”minValue”>The inclusive lower bound of the random number returned.</param>
		///<param name=”maxValue”>The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		public int Next(int minValue, int maxValue)
		{
			return (int)Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue;
		}

		///<summary>
		/// Returns a nonnegative random number.
		///</summary>
		public int Next()
		{
			return Next(0, Int32.MaxValue);
		}

		///<summary>
		/// Returns a nonnegative random number less than the specified maximum
		///</summary>
		///<param name=”maxValue”>The inclusive upper bound of the random number returned. maxValue must be greater than or equal 0</param>
		public int Next(int maxValue)
		{
			return Next(0, maxValue);
		}

		public override void GetNonZeroBytes(byte[] data)
		{
			rng.GetNonZeroBytes(data);
		}

		public override void GetBytes(byte[] data)
		{
			rng.GetBytes(data);
		}

		/*
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing) rng.Dispose();
		}
		*/

	}

}
