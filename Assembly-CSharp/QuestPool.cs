using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestPool
{
	[Serializable]
	public class Quest
	{
		public int QuestId;

		public int Priority;

		public bool OfferedBackToBackAllowed;
	}

	public QuestPrerequisites Prerequisites;

	public List<Quest> Quests;

	public bool Valid;

	public int Priority;

	public int SlotsToFill;

	public string Note;

	[HideInInspector]
	public int Index;

	public int CalculateReverseSortOrder(int numPools)
	{
		return (Priority * 2 + ((SlotsToFill > 0) ? 1 : 0)) * numPools - Index;
	}
}
