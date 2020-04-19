using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestPool
{
	public QuestPrerequisites Prerequisites;

	public List<QuestPool.Quest> Quests;

	public bool Valid;

	public int Priority;

	public int SlotsToFill;

	public string Note;

	[HideInInspector]
	public int Index;

	public int CalculateReverseSortOrder(int numPools)
	{
		return (this.Priority * 2 + ((this.SlotsToFill <= 0) ? 0 : 1)) * numPools - this.Index;
	}

	[Serializable]
	public class Quest
	{
		public int QuestId;

		public int Priority;

		public bool OfferedBackToBackAllowed;
	}
}
