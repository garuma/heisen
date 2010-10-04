
using System;

namespace Heisen.Framework
{
	[AttributeUsage (AttributeTargets.Class)]
	public class HeisenFixtureAttribute : Attribute
	{
		/* If for any reason the class fixture should be disabled for this run
		 * the property allows to disable its execution
		 */
		public bool Disabled {
			get;
			set;
		}
	}

	[AttributeUsage (AttributeTargets.Method)]
	public class HeisenInitAttribute : Attribute
	{
		/* If the init method of the class fixture should only
		 * be run before the first test execution and never afterwards
		 */
		public bool RunOnce {
			get;
			set;
		}
	}
	
	[AttributeUsage (AttributeTargets.Method)]
	public class HeisenTestMethodAttribute : Attribute
	{
		/* When the same run method needs to be tested against
		 * itself instead of rewriting an identical other one
		 * we can just set this property
		 */
		public int Duplicate {
			get;
			set;
		}
	}
	
	[AttributeUsage (AttributeTargets.Method)]
	public class HeisenInvariantsAttribute : Attribute
	{
		
	}
}