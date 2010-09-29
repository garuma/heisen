using System;
using System.Threading;

using Heisen;

namespace Testalicious
{
	public class DummyTests : IHeisenTestFixture
	{
		public void Init ()
		{
			/* Init just init fields to have a clean state upon which each attribute marked instance methods will
			 * work against
			 */
			Console.WriteLine ("Init");
		}
		
		public Thread[] Run ()
		{
			return new Thread [] {
				new Thread (() => Console.WriteLine ("One")),
				new Thread (() => Console.WriteLine ("Two"))
			};			  
		}

		public void TestInvariants ()
		{
			Console.WriteLine ("Groovy invariants");
		}
	}
}