using System;
using System.Threading;
using System.Collections.Generic;

using Mono.Tasklets;

namespace Heisen
{
	public static class Scheduler
	{
		static Continuation continuation = new Continuation ();
		static Queue<HeisenThread> threads = new Queue<HeisenThread> ();
		static Queue<HeisenThread> save = new Queue<HeisenThread> ();
		static HeisenThread currentThread;
		static bool dontStop = true;

		public static void EnqueueWork (HeisenThread thread)
		{
			threads.Enqueue (thread);
		}

		public static void Run (Action checkInvariants)
		{
			continuation.Mark ();
			int count = threads.Count;

			while (dontStop) {
				int val = continuation.Store (0);
				if (!threads.TryDequeue (out currentThread)) {
					RuntimeManager.DisableRuntimeInjection ();
					if (checkInvariants != null)
						checkInvariants ();
					Reinit ();
					RuntimeManager.EnableRuntimeInjection ();
					continue;
				}
				
				if (currentThread.Status != HeisenThreadStatus.Started) {
					RuntimeManager.EnableRuntimeInjection (count);
					currentThread.Run ();
					//RuntimeManager.DisableRuntimeInjection ();
					ScheduleNext ();
				} else {
					// Restore the heisen thread
					currentThread.Continuation.Restore (1);
				}
			}
		}

		static void Reinit ()
		{
			var tmp = save;
			save = threads;
			threads = tmp;
			
			foreach (var t in threads)
				t.Status = HeisenThreadStatus.Inited;
		}

		/* This is never called by user code but directly by the runtime */
		public static void Yield ()
		{
			int val = currentThread.Continuation.Store (0);

			if (val == 0)
				ScheduleNext ();
			// If val was 1 then we just return and let the remains of the task execute
		}

		public static void Stop ()
		{
			dontStop = false;
		}

		static void ScheduleNext ()
		{
			if (currentThread.Status != HeisenThreadStatus.Finished)
				threads.Enqueue (currentThread);
			else
				save.Enqueue (currentThread);

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
	}
}