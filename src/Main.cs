using System;
using System.IO;
using System.Linq;

using NDesk.Options;

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
			IReplayInformations infos;
			foreach (var driver in dissecter.LoadAllTestDriver ()) {
				if (driver.RunTest (out infos)) {
					Console.WriteLine ("Running {0}, result: success", driver.Name);
				} else {
					Console.WriteLine ("Running {0}, result: failure", driver.Name);
					infos.DisplayFaultyInterleaving ();
				}
			}
		}

		static void PrintUsage ()
		{
			Console.WriteLine ("Usage: mono Heisen.exe fixture-assembly.dll");
			Console.WriteLine ("Not much other usage at the moment");
			
			Environment.Exit (1);
		}
	}
}