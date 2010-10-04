
using System;

namespace Heisen
{
	public class InvalidTestFixtureException : Exception
	{
		const string defaultMessage = "The fixture {0} is malformed";

		public InvalidTestFixtureException (string fixtureName) 
			: base (string.Format (defaultMessage, fixtureName))
		{
			
		}
	}
}