using System;
using System.Threading;
using Heisen;

namespace FooTest
{
	public class FooTestClass
	{
		//static int invariant = 0;
		static bool processing = false;
		static int execution = 0;

		public static void Main ()
		{
			Scheduler.EnqueueWork (new HeisenThread (Foo));
			Scheduler.EnqueueWork (new HeisenThread (Bar));

			Scheduler.Run (CheckInvariants);
		}

		static void CheckInvariants ()
		{
			if (execution > 1) {
				Console.WriteLine ("Teh code is wrong but then I'm just a tool, you have to reason about multithreaded code");
				RuntimeManager.PrintCurrentInterleaving ();
				Environment.Exit (1);
			}
		}

		static void Foo ()
		{
			if (!processing) {
				processing = true;
				Console.WriteLine ("Plop");
				Interlocked.Increment (ref execution);
			}
		}

		static void Bar ()
		{
			if (!processing) {
				processing = true;
				Console.WriteLine ("Polp");
				Interlocked.Increment (ref execution);
			}
		}

		/*static void CheckInvariants ()
		{
			if (invariant != 2) {
				Console.WriteLine ("Teh code is wrong but then I'm just a tool, you have to reason about multithreaded code");
				RuntimeManager.PrintCurrentInterleaving ();
				Environment.Exit (1);
			}
		}

		static void Foo ()
		{
			int val = invariant;
			val += 1;
			invariant = val;
		}

		static void Bar ()
		{
			int val = invariant;
			val += 1;
			invariant = val;
		}*/

		/*		static void CheckInvariants ()
		{
			Console.WriteLine ();
			Console.WriteLine ("Changing scheduling pattern");
			Console.WriteLine ();
		}

		static void Foo ()
		{
			Console.WriteLine ("Ping");
			Console.WriteLine ("Pong");
			Console.WriteLine ("Ping2");
			Console.WriteLine ("Pong2");
			Console.WriteLine ("Ping3");
			Console.WriteLine ("Pong3");
			Console.WriteLine ("Ping3");
		}

		static void Bar ()
		{
			Console.WriteLine ("Prout");
			Console.WriteLine ("Pouet");
			Console.WriteLine ("Prout2");
			Console.WriteLine ("Pouet2");
			Console.WriteLine ("Prout3");
			Console.WriteLine ("Pouet3");
			Console.WriteLine ("Prout3");
		}*/
	}
}