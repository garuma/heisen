
using System;
using System.IO;
using System.Linq;
using System.Threading;

using Mono.Cecil;
using Mono.Cecil.Cil;

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
		TypeDefinition typeDef;
		ModuleDefinition module;

		public TestFixture (TypeDefinition typeDef, ModuleDefinition module)
		{
			this.typeDef = typeDef;
			this.module = module;
		}
		
		public ITestDriver CreateDriver ()
		{
			return null;
		}

		public string Name {
			get {
				return typeDef.FullName;
			}
		}
	}
}