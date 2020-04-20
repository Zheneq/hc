using System;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterSelectSimplePartyList : MonoBehaviour
{
	public UISimplePartyListMember[] m_simplePartyMembers;

	public void UpdateCharacterList(LobbyPlayerInfo playerInfo, LobbyTeamInfo teamInfo, LobbyGameInfo gameInfo)
	{
		int i = 0;
		if (playerInfo != null)
		{
			if (teamInfo != null)
			{
				if (teamInfo.TeamPlayerInfo != null)
				{
					using (List<LobbyPlayerInfo>.Enumerator enumerator = teamInfo.TeamPlayerInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
							if (i < this.m_simplePartyMembers.Length)
							{
								if (lobbyPlayerInfo.TeamId != playerInfo.TeamId)
								{
									if (lobbyPlayerInfo.TeamId != Team.Spectator)
									{
										UIManager.SetGameObjectActive(this.m_simplePartyMembers[i], true, null);
										UIManager.SetGameObjectActive(this.m_simplePartyMembers[i].m_checkmark, lobbyPlayerInfo.ReadyState == ReadyState.Ready || lobbyPlayerInfo.IsNPCBot, null);
										this.m_simplePartyMembers[i].m_playerName.text = lobbyPlayerInfo.GetHandle();
										i++;
									}
								}
							}
						}
					}
				}
			}
		}
		while (i < this.m_simplePartyMembers.Length)
		{
			UIManager.SetGameObjectActive(this.m_simplePartyMembers[i], false, null);
			i++;
		}
	}
}
