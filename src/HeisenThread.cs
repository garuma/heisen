using System;
using System.Threading;

using Mono.Tasklets;

namespace Heisen
{
	internal enum HeisenThreadStatus
	{
		Inited,
		Started,
		Finished
	}

	public class HeisenThread
	{
		Continuation continuation = new Continuation ();

		Action<object> action;
		object state;
		
		HeisenThreadStatus status = HeisenThreadStatus.Inited;

		public HeisenThread (Action a) : this ((o) => a (), null)
		{
			
		}		
		
		public HeisenThread (Action<object> action, object state)
		{
			this.action = action;
			this.state = state;
		}

		internal void Run ()
		{
			continuation.Mark ();
			status = HeisenThreadStatus.Started;
			action (state);
			status = HeisenThreadStatus.Finished;
		}

		internal Continuation Continuation {
			get {
				return continuation;
			}
		}

		internal HeisenThreadStatus Status {
			get {
				return status;
			}
		}
	}
}