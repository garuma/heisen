
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Heisen.Framework;

namespace Heisen
{
	/* This class is responsible for loading a test assembly (given its valid path), search for test fixture and
	 * extract their corresponding test methods
	 */
	public class AssemblyDissecter
	{
		Assembly assembly;
		List<TestFixture> testFixtures = new List<TestFixture> ();

		public AssemblyDissecter (string assemblyPath)
		{
			assembly = Assembly.LoadFrom (assemblyPath);
			FindFixtures ();
		}

		/* The runner will call this method that summarize the following set of operations:
		 *   - Cook its blackmagic to have a modified execution of TestMethod to test interleavings
		 *   - Write the boilerplate wrapping code in a temporary result assembly
		 *   - Load that temporary assembly dynamically
		 *   - Create an instance of a type implementing ITestDriver so that the runner can execute the method and report errors
		 *   - Return the bunch of them as a IEnumerable (as each ITestDriver manipulate just one TestMethod execution)
		 */
		public IEnumerable<ITestDriver> LoadAllTestDriver ()
		{
			return testFixtures.Select ((t) => t.CreateDriver ());
		}

		void FindFixtures ()
		{
			int num = 0;
			object[] attributes = null;

			foreach (var type in assembly.GetTypes ()) {
				if (!type.IsClass || (attributes = type.GetCustomAttributes (typeof (HeisenFixtureAttribute), false)).Length == 0)
					continue;

				if (attributes.Length > 1)
					throw new InvalidTestFixtureException (string.Format ("The test fixture {0} has more than one HeisenFixtureAttribute applied", type.Name));

				if (((HeisenFixtureAttribute)attributes[0]).Disabled)
					continue;

				testFixtures.Add (new TestFixture (type));
				num++;
			}
			Console.WriteLine ("Found {0} fixtures", num.ToString ());
		}
	}
}