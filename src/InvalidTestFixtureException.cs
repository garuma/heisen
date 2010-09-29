
using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Cecil;

using Heisen.Tests;

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