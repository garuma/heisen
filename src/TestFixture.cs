
using System;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

using Heisen.Framework;
using Heisen.Drivers;

namespace Heisen
{
	/* This class represents a test fixture class composed of at least 3 methods
	 * (initialization method ran at each new interleaving test iteration, run methods for the actual
	 * operation to run in parallel and have their interleaving tested and an invariants method that check
	 * at the end of each subtest run if the invariant condition have been respected with the current interleaving)
	 * This class is responsible for wrapping these 3 method into some other class method that controls their execution.
	 */
	public class TestFixture
	{
		Type type;
		object typeInstance;
		MethodInfo[] methods;

		public TestFixture (Type type)
		{
			this.type = type;
			this.typeInstance = Activator.CreateInstance (type);
		}
		
		public ITestDriver CreateDriver ()
		{
			return new HijackingTestDriver (type, 
			                                ExtractInitMethod (),
			                                ExtractTestMethods (),
			                                ExtractInvariantsMethod ());
		}

		public string Name {
			get {
				return type.Name;
			}
		}

		Action ExtractInitMethod ()
		{
			if (methods == null)
				methods = type.GetMethods ();

			HeisenInitAttribute attribute = null;

			try {
				MethodInfo method = methods.Where ((m) => (attribute = m.GetAttribute<HeisenInitAttribute> ()) != null).First ();
				if (attribute.RunOnce) {
					bool ran = false;
					return () => { if (ran) return; ran = true; method.Invoke (typeInstance, null); };
				} else {
					return () => method.Invoke (typeInstance, null);
				}
			} catch {
				return DoNothing;
			}
		}

		Action[] ExtractTestMethods ()
		{
			if (methods == null)
				methods = type.GetMethods ();

			List<Action> actions = new List<Action> ();
			HeisenTestMethodAttribute attribute = null;

			foreach (var m in methods.Where ((m) => (attribute = m.GetAttribute<HeisenTestMethodAttribute> ()) != null)) {
				if (attribute.Duplicate == 0)
					attribute.Duplicate = 1;

				actions.AddRange (Enumerable.Repeat<Action> (() => m.Invoke (typeInstance, null), attribute.Duplicate));	
			}

			return actions.ToArray ();
		}

		Action ExtractInvariantsMethod ()
		{
			if (methods == null)
				methods = type.GetMethods ();

			try {
				MethodInfo method = methods.Single ((m) => m.GetAttribute<HeisenInvariantsAttribute> () != null);
				
				return () => method.Invoke (typeInstance, null);
			} catch {
				throw new InvalidTestFixtureException (string.Format ("The test fixture {0} has no invariants to test", type.Name));
			}
		}

		void DoNothing ()
		{
			
		}
	}

	internal static class MethodInfoExtension
	{
		internal static TAttribute GetAttribute<TAttribute> (this MethodInfo method) where TAttribute : Attribute
		{
			object[] tmp = method.GetCustomAttributes (typeof (TAttribute), false);
			if (tmp.Length == 0)
				return null;
			
			return (tmp[0] as TAttribute);
		}
	}
}