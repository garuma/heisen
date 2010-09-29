
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Heisen.Tests
{
	public static class HeisenFuzzer
	{
		public static void TestConcurrent (params Action[] actions)
		{
			IEnumerable<Thread> threads = actions.Select ((a) => new Thread ((_) => a ()));

			foreach (var t in threads)
				t.Start ();
			foreach (var t in threads)
				t.Join ();
		}
	}
}