using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Heisen
{
	public static class RuntimeManager
	{
		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_enable_hijack_code ();
		
		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_disable_hijack_code ();

		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_hijack_add_cont ();


		public static void EnableRuntimeInjection ()
		{
			mono_enable_hijack_code ();
		}

		public static void DisableRuntimeInjection ()
		{
			mono_disable_hijack_code ();
		}

		public static void RegisterContinuation ()
		{
			mono_hijack_add_cont ();
		}
	}
}