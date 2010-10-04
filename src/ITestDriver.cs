
using System;

namespace Heisen
{
	/* Encapsulate a runnable instance of a heisen test and allow to control it's execution
	 */
	public interface ITestDriver
	{
		/* Run the test with all the possible interleavings
		 * if one of them fails, the method stops then set replaysInfos 
		 * with the informations to reproduce the problematic interleaving for later use
		 * and finally returns false
		 */
		bool RunTest (out IReplayInformations replayInfos);

		/* Given one interleaving scenario previously created and run by RunTest method
		 * this method directly retest to find if it has been fixed or not
		 */
		bool ReplayScenario (IReplayInformations replayInfos);

		string Name { get; }
	}
}