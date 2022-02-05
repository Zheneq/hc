// ROGUES
// SERVER
using System;
using System.Linq;

// server-only
#if SERVER
public struct StackingData
{
	public bool canStack;
	public int maxStackSize;
	public int maxStackSizePerTurn;
	public int[] stackCount;

	public void Refresh()
	{
		if (!stackCount.IsNullOrEmpty())
		{
			int num = stackCount.Sum();
			stackCount = new int[stackCount.Length];
			stackCount[0] = num;
		}
	}

	public void ShiftStackCount()
	{
		if (!stackCount.IsNullOrEmpty())
		{
			Array.Copy(stackCount, 0, stackCount, 1, stackCount.Length - 1);
			stackCount[0] = 0;
		}
	}
}
#endif
