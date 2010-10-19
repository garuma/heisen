using System;
using System.Threading;
using System.Collections.Generic;

using Mono.Tasklets;

namespace Heisen
{
	public static class Scheduler
	{
		static Continuation continuation = new Continuation ();
		static List<HeisenThread> initial = new List<HeisenThread> ();
		static Queue<HeisenThread> threads = new Queue<HeisenThread> ();
		static HeisenThread currentThread;
		static bool dontStop = true;
		static bool forceRestart = false;
		static int numberOfRun = 1;

		public static void EnqueueWork (HeisenThread thread)
		{
			threads.Enqueue (thread);
			initial.Add (thread);
		}

		public static void Run (Action init, Action checkInvariants)
		{
			continuation.Mark ();
			int count = threads.Count;

			while (dontStop) {
				RuntimeManager.SetNumberMethods (count);

				if (init != null)
					init ();

				int val = continuation.Store (0);
				if (!dontStop)
					break;
				if (forceRestart || !threads.TryDequeue (out currentThread)) {
					RuntimeManager.DisableRuntimeInjection ();
					if (!forceRestart && checkInvariants != null)
						checkInvariants ();
					Reinit ();
					++numberOfRun;
					if (forceRestart) {
						Console.WriteLine ("Forcing restart");
						forceRestart = false;
					}
					RuntimeManager.EnableRuntimeInjection ();
					continue;
				}
				
				if (currentThread.Status != HeisenThreadStatus.Started) {
					RuntimeManager.EnableRuntimeInjection ();
					currentThread.Run ();
					RuntimeManager.DisableRuntimeInjection ();
					ScheduleNext ();
				} else {
					// Restore the heisen thread
					currentThread.Continuation.Restore (1);
				}
			}
		}

		static void Reinit ()
		{
			threads.Clear ();
			threads.EnqueueRange (initial);
		}

		/* This is never called by user code but directly by the runtime */
		public static void Yield ()
		{
			// If there is only one thread left, don't take in account its yielding
			if (threads.Count == 0)
				return;

			int val = currentThread.Continuation.Store (0);

			if (val == 0)
				ScheduleNext ();
			// If val was 1 then we just return and let the remains of the task execute
		}

		public static void Stop ()
		{
			dontStop = false;
		}

		public static void ForceRestart ()
		{
			forceRestart = true;
			continuation.Restore (0);
		}

		public static int NumberOfRun {
			get {
				return numberOfRun;
			}
		}

		static void ScheduleNext ()
		{
			if (currentThread.Status != HeisenThreadStatus.Finished)
				threads.Enqueue (currentThread);

			continuation.Restore (0);
		}

		static bool TryDequeue<T> (this Queue<T> queue, out T obj)
		{
			obj = default (T);
			if (queue.Count == 0)
				return false;
			
			obj = queue.Dequeue ();
			return true;
		}

		static void EnqueueRange<T> (this Queue<T> queue, IEnumerable<T> data)
		{
			foreach (var d in data)
				queue.Enqueue (d);
		}
	}
}