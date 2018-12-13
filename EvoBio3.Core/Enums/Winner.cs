using System;

namespace EvoBio3.Core.Enums
{
	[Flags]
	public enum Winner
	{
		Cooperator1 = 1 << 0,
		Cooperator2 = 1 << 1,
		Resonation = 1 << 2,
		Defector = 1 << 3,
		Fix = 1 << 4,
		Tie = 1 << 5
	}
}