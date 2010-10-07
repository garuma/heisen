
using System;
using System.Reflection;

using Heisen.Framework;

namespace Heisen.Drivers
{
	public class HijackingTestDriver : ITestDriver
	{
		class ReplayInfos : IReplayInformations
		{
			AssertException exception;

			internal ReplayInfos (AssertException e)
			{
				this.exception = e;
			}

			public void SaveToFile (string path)
			{
				
			}

			public void DisplayFaultyInterleaving ()
			{
				Console.WriteLine (exception.Message);
				RuntimeManager.PrintCurrentInterleaving ();
			}
		}

		Type type;

		Action initMethod;
		Action[] testMethods;
		Action invariantsMethod;

		public HijackingTestDriver (Type type, Action initMethod, Action[] testMethods, Action invariantsMethod)
		{
			this.type = type;
			this.initMethod = initMethod;
			this.testMethods = testMethods;
			this.invariantsMethod = invariantsMethod;
		}

		public bool RunTest (out IReplayInformations infos)
		{
			infos = null;
			AssertException exception = null;
			
			Action invariantsEncapsulated = delegate {
				try {
					invariantsMethod ();
				} catch (TargetInvocationException e) {
					Scheduler.Stop ();
					exception = e.InnerException as AssertException;
				}
			};

			foreach (Action action in testMethods)
				Scheduler.EnqueueWork (new HeisenThread (action));

			Scheduler.Run (initMethod, invariantsEncapsulated);
			Console.WriteLine ("Test concluded after {0} mixed-interleaving runs", Scheduler.NumberOfRun.ToString ());

			if (exception != null)
				infos = new ReplayInfos (exception);

			return exception == null;
		}

		public bool ReplayScenario (IReplayInformations infos)
		{
			return true;
		}

		public string Name {
			get {
				return type.Name;
			}
		}
	}
}