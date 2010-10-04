
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Heisen
{
	public interface IReplayInformations
	{
		void SaveToFile (string path);
		void DisplayFaultyInterleaving ();
	}
}
