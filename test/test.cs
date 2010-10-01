using System;
using Heisen;

namespace FooTest
{
	public class FooTestClass
	{
		public static void Main ()
		{
			RuntimeManager.EnableRuntimeInjection ();b
			Foo ();
			RuntimeManager.DisableRuntimeInjection ();
			Bar ();
		}

		static void Foo ()
		{
			RuntimeManager.RegisterContinuation ();
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
		}
	}
}