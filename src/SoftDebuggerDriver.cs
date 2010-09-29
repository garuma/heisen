
using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using Mono.Debugger.Soft;

namespace Heisen
{
	public class SoftDebuggerDriver : ITestDriver
	{
		TestFixture fixture;
		string path;
		readonly int brkOffset;

		VirtualMachine vm;
		Dictionary<ThreadMirror, StepEventRequest> threads = new Dictionary<ThreadMirror, StepEventRequest> ();

		public SoftDebuggerDriver (TestFixture fixture, string path, int brkOffset)
		{
			this.fixture = fixture;
			this.path = path;
			this.brkOffset = brkOffset;
		}

		void DummyRun ()
		{
			Process p = new Process ();
			p.StartInfo.FileName = "mono";
			p.StartInfo.UseShellExecute = true;
			p.StartInfo.Arguments = path;
			p.Start ();
			p.WaitForExit ();
		}

		public bool RunTest (out ReplayInformations replayInfos)
		{
			replayInfos = null;
			
			//DummyRun ();
			LaunchVirtualMachine ();
			
			return true;
		}

		void LaunchVirtualMachine ()
		{
			if (vm != null)
				ExitVirtualMachine ();

			var psi = new System.Diagnostics.ProcessStartInfo ("mono") {
				Arguments = path,
				UseShellExecute = true,
				CreateNoWindow = true,
				RedirectStandardOutput = false,
				RedirectStandardError = false
			};

			vm = VirtualMachineManager.Launch (psi, null);
			vm.EnableEvents (EventType.ThreadStart, EventType.ThreadDeath, EventType.MethodEntry);

			// Get the VMStart event
			while (GetEvent (vm).EventType != EventType.VMStart);
			vm.Resume ();

			Event evt;

			// Get Main method call
			while ((evt = GetEvent (vm)).EventType != EventType.MethodEntry);
			ThreadMirror mainThread = evt.Thread;

			// Set up breakpoint
			vm.SetBreakpoint (((MethodEntryEvent)evt).Method, brkOffset);
			vm.Resume ();
			
			// while ((evt = GetEvent (vm)).EventType != EventType.VMDisconnect && evt.EventType != EventType.VMDeath) {
			// 	if (evt.EventType == EventType.ThreadStart || evt.EventType == EventType.ThreadDeath || evt.EventType == EventType.MethodEntry)
			// 		vm.Resume ();

			while ((evt = GetEvent (vm)).EventType != EventType.Breakpoint) {				
				if (evt.EventType == EventType.ThreadStart) {
					ThreadMirror newThread = evt.Thread;
				}

				if (later)
					vm.Resume ();
				later = true;
					
				mainThread.Resume ();
			}

			//vm.Suspend ();

			// ThreadMirror[] ts = threads.Keys.ToArray ();
			// StepEventRequest req = null;

			/*while (threads.Count > 0) {
				foreach (var t in ts) {
					if (req != null)
						req.Enabled = false;

					req = vm.CreateStepRequest (t);
					SetupStepReq (req);
					//Thread.Sleep (1);
					req.Enabled = true;
					t.Resume ();
					vm.Resume ();

					Event e = null;
					while ((e = GetEvent (vm)).EventType == EventType.MethodEntry) vm.Resume ();
					Console.WriteLine ("From " + t.Id.ToString ());
					if (e.EventType != EventType.Step)
						evt = e;
					else {
						t.Suspend ();
						vm.Suspend ();
					}
				}
				
				if (evt.EventType == EventType.ThreadDeath) {
					threads.Remove (((ThreadDeathEvent)evt).Thread);
					ts = threads.Keys.ToArray ();
				}
			}*/
			// foreach (var t in ts) {
			// 	if (req != null)
			// 		req.Enabled = false;
			// 	req = vm.CreateStepRequest (t);
			// 	SetupStepReq (req);				
			// 	vm.Resume ();
			// 	t.Resume ();

			// 	req.Enabled = true;
			// 	while (GetEvent (vm).EventType != EventType.ThreadDeath);
			// 	vm.Suspend ();					
			// }
			
			// Console.WriteLine ("Resuming main thread");
			// mainThread.Resume ();
			// vm.Resume ();
			
			Console.WriteLine ("Waiting for VM to shutdown");
			vm.Process.WaitForExit ();
			Console.WriteLine ("Process exit status: " + vm.Process.ExitCode.ToString ());
		}

		void SetupStepReq (StepEventRequest req)
		{
			req.Depth = StepDepth.Into;
			req.Size = StepSize.Min;
			req.Count = 1;
		}

		static Event GetEvent (VirtualMachine vm)
		{
			//Console.WriteLine ("Calling GetEvent");
			var evt = vm.GetNextEvent ();
			//vm.Resume ();
			if (evt.EventType != EventType.MethodEntry)
				Console.WriteLine ("e:" + evt.EventType.ToString ());
			else
				Console.WriteLine ("m:" + ((MethodEntryEvent)evt).Method.Name);

			return evt;
		}

		void ExitVirtualMachine ()
		{
			try {
				vm.Exit (0);
			} catch (VMDisconnectedException) {
			}

			try {
				vm.Dispose ();
			} catch (VMDisconnectedException) {
			}
		}
		
		public bool ReplayScenario (ReplayInformations replayInfos)
		{
			return true;
		}

		public string Name {
			get {
				return fixture.Name;
			}
		}
	}
}