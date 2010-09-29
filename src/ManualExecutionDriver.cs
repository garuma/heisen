
using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Heisen
{
	public class ManualExecutionDriver : ITestDriver
	{
		TestFixture fixture;
		string path;

		public ManualExecutionDriver (TestFixture fixture, string path)
		{
			this.fixture = fixture;
			this.path = path;
		}

		
	}
}