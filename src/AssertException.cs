
using System;
using Heisen;

namespace Heisen.Framework
{
	public class AssertException : Exception
	{
		public AssertException (string failureMessage) : base (failureMessage)
		{
			
		}
	}
}