
using System;

namespace Heisen.Framework
{
	public static class Assert
	{
		public static void Fail ()
		{
			Fail (string.Empty);
		}

		public static void Fail (string message)
		{
			throw new AssertException (message);
		}

		public static void IsTrue (bool condition, string msg = null)
		{
			if (!condition)
				Fail ("Assertion failed: " + (string.IsNullOrEmpty (msg) ? string.Empty : msg));
		}

		public static void AreEqual<T> (T expected, T actual, string msg = null)
		{
			IsTrue (expected.Equals (actual), string.Format ("expected({0}) is different than actual({1}): {2}", 
			                                             expected.ToString (), 
			                                             actual.ToString (), 
			                                             string.IsNullOrEmpty (msg) ? string.Empty : msg));
		}

		public static void AreNotEqual<T> (T expected, T actual, string msg = null)
		{
			IsTrue (!expected.Equals (actual), string.Format ("expected({0}) is equal to actual({1}): {2}", 
			                                              expected.ToString (), 
			                                              actual.ToString (), 
			                                              string.IsNullOrEmpty (msg) ? string.Empty : msg));
		}
	}
}