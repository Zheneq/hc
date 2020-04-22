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
							LobbyPlayerInfo current = enumerator.Current;
							if (i < m_simplePartyMembers.Length)
							{
								if (current.TeamId != playerInfo.TeamId)
								{
									if (current.TeamId != Team.Spectator)
									{
										UIManager.SetGameObjectActive(m_simplePartyMembers[i], true);
										UIManager.SetGameObjectActive(m_simplePartyMembers[i].m_checkmark, current.ReadyState == ReadyState.Ready || current.IsNPCBot);
										m_simplePartyMembers[i].m_playerName.text = current.GetHandle();
										i++;
									}
								}
							}
						}
					}
				}
			}
		}
		for (; i < m_simplePartyMembers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_simplePartyMembers[i], false);
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
