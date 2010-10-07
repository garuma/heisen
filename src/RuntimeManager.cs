using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Heisen
{
	public static class RuntimeManager
	{
		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_hijack_set_num_methods (int num_methods);

		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_enable_hijack_code ();
		
		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_disable_hijack_code ();

		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_hijack_register_cont (IntPtr ptr);		

		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void mono_hijack_print_current_interleaving ();

		public static void RegisterCurrentThread (IntPtr ptr)
		{
			mono_hijack_register_cont (ptr);
		}

		public static void EnableRuntimeInjection ()
		{
			mono_enable_hijack_code ();
		}

		public static void SetNumberMethods (int numMethods)
		{
			mono_hijack_set_num_methods (numMethods);
		}

		public static void DisableRuntimeInjection ()
		{
			mono_disable_hijack_code ();
		}

		public static void PrintCurrentInterleaving ()
		{
			mono_hijack_print_current_interleaving ();
		}

	}
}