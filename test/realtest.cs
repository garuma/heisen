using System;
using Heisen;

namespace FooTest
{
	public class FooTestClass
	{
		public static void Main ()
		{
			Scheduler.EnqueueWork (new HeisenThread (Foo));
			Scheduler.EnqueueWork (new HeisenThread (Bar));

			Scheduler.Run ();
		}

		static void Foo ()
		{
			Console.WriteLine ("Ping");
			Console.WriteLine ("Pong");
		}

		static void Bar ()
		{
			Console.WriteLine ("Prout");
			Console.WriteLine ("Pouet");
		}
	}
}