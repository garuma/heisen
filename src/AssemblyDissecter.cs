
using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Cecil;

using Heisen.Tests;

namespace Heisen
{
	/* This class is responsible for loading a test assembly (given its valid path), search for test fixture and
	 * extract their corresponding test methods
	 */
	public class AssemblyDissecter
	{
		ModuleDefinition module;
		List<TestFixture> testFixtures = new List<TestFixture> ();

		public AssemblyDissecter (string assemblyPath)
		{
			module = ModuleDefinition.ReadModule (assemblyPath);
			FindFixtures ();
		}

		/* The runner will call this method that summarize the following set of operations:
		 *   - Call method on TestMethod that inject breakpoints in the IL
		 *   - Write the result assembly in a temporary location
		 *   - Load that temporary assembly dynamically
		 *   - Load a special type implementing ITestDriver that caller will use to control the test execution
		 *   - Return the bunch of them as a IEnumerable (as each ITestDriver manipulate just one TestMethod execution)
		 */
		public IEnumerable<ITestDriver> LoadAllTestDriver ()
		{
			return testFixtures.Select ((t) => t.CreateDriver ());
		}

		void FindFixtures ()
		{
			int num = 0;

			foreach (var typeDef in module.Types) {
				if (!typeDef.IsPublic || !typeDef.HasInterfaces)
					continue;

				if (!typeDef.Interfaces.Any ((i) => i.FullName == typeof (IHeisenTestFixture).FullName))
					continue;

				testFixtures.Add (new TestFixture (typeDef, module));
				num++;
			}
			Console.WriteLine ("Finded {0} fixtures", num.ToString ());
		}
	}
}