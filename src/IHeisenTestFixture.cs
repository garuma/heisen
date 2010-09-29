
using System;
using System.Threading;

namespace Heisen
{
	/* Each test fixture class implements this interface
	 */
	public interface IHeisenTestFixture
	{
		void Init ();
		void TestInvariants ();
	}
}