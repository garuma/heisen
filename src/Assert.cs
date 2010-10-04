
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
				Fail ("Expected true but got false: " + (string.IsNullOrEmpty (msg) ? string.Empty : msg));
		}

		public static void AreEqual<T> (T elem1, T elem2, string msg = null)
		{
			IsTrue (elem1.Equals (elem2), string.Format ("elem1({0}) is different than elem2({1}): {2}", 
			                                             elem1.ToString (), 
			                                             elem2.ToString (), 
			                                             string.IsNullOrEmpty (msg) ? string.Empty : msg));
		}

		public static void AreNotEqual<T> (T elem1, T elem2, string msg = null)
		{
			IsTrue (!elem1.Equals (elem2), string.Format ("elem1({0}) is equal to elem2({1}): {2}", 
			                                              elem1.ToString (), 
			                                              elem2.ToString (), 
			                                              string.IsNullOrEmpty (msg) ? string.Empty : msg));
		}
	}
}