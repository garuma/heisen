using System;
using System.IO;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;

using NDesk.Options;

using Heisen.Tests;

namespace Heisen
{
	public class Runner
	{

		public static void Main (string[] args)
		{
			OptionSet p = new OptionSet ();
			var list = p.Parse (args);

			if (list.Count != 1)
				PrintUsage ();

			string dllToLoad = list.First ();
			if (!File.Exists (dllToLoad)) {
				Console.WriteLine ("Not a valid pathname");
				Console.WriteLine ();
				PrintUsage ();
			}

			var dissecter = new AssemblyDissecter (dllToLoad);
			ReplayInformations infos;
			foreach (var driver in dissecter.LoadAllTestDriver ())
				Console.WriteLine ("Running {0}, result: {1}", driver.Name, driver.RunTest (out infos));
		}

		static void PrintUsage ()
		{
			Console.WriteLine ("Usage: mono Heisen.exe fixture-assembly.dll");
			Console.WriteLine ("Not much other usage at the moment");
			
			Environment.Exit (1);
		}
	}
}