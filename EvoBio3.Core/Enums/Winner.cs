using System;

namespace EvoBio3.Core.Enums
{
	[Flags]
	public enum Winner
	{
		Both1 = 1 << 0,
		Both2 = 1 << 1,
		Resonation = 1 << 2,
		Null = 1 << 3,
		Fix = 1 << 4,
		Tie = 1 << 5
	}
}