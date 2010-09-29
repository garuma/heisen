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
		static HeisenThread currentThread;

		public static void EnqueueWork (HeisenThread thread)
		{
			threads.Enqueue (thread);
		}

		public static void Run ()
		{
			continuation.Mark ();

			int val = continuation.Store (0);
			if (!threads.TryDequeue (out currentThread))
				return;
			
			if (currentThread.Status != HeisenThreadStatus.Started) {
				RuntimeManager.EnableRuntimeInjection ();
				currentThread.Run ();
				//RuntimeManager.DisableRuntimeInjection ();
				ScheduleNext ();
			} else {
				// Restore the heisen thread
				currentThread.Continuation.Restore (1);
			}
		}

		/* This is never called by user code but directly by the runtime */
		public static void Yield ()
		{
			int val = currentThread.Continuation.Store (0);

			if (val == 0)
				ScheduleNext ();
			// If val was 1 then we just return and let the remains of the task execute
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
	}
}