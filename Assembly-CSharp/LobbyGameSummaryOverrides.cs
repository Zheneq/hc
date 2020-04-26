using System;
using System.Collections.Generic;

[Serializable]
public class LobbyGameSummaryOverrides
{
	public Dictionary<long, int> GGPacksUsedList;

	public bool PlayWithFriendsBonus;

	public bool PlayedLastTurn;
}
