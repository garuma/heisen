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
		}

		static void Bar ()
		{
			Console.WriteLine ("Prout");
			Console.WriteLine ("Pouet");
		}
	}
}