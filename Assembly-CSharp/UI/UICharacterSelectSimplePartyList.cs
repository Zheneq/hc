using System.Collections.Generic;
using UnityEngine;

public class UICharacterSelectSimplePartyList : MonoBehaviour
{
	public UISimplePartyListMember[] m_simplePartyMembers;

	public void UpdateCharacterList(LobbyPlayerInfo playerInfo, LobbyTeamInfo teamInfo, LobbyGameInfo gameInfo)
	{
		int i = 0;
		if (playerInfo != null
		    && teamInfo != null
		    && teamInfo.TeamPlayerInfo != null)
		{
			foreach (LobbyPlayerInfo lobbyPlayerInfo in teamInfo.TeamPlayerInfo)
			{
				if (i < m_simplePartyMembers.Length
				    && lobbyPlayerInfo.TeamId != playerInfo.TeamId
				    && lobbyPlayerInfo.TeamId != Team.Spectator)
				{
					UIManager.SetGameObjectActive(m_simplePartyMembers[i], true);
					UIManager.SetGameObjectActive(
						m_simplePartyMembers[i].m_checkmark,
						lobbyPlayerInfo.ReadyState == ReadyState.Ready || lobbyPlayerInfo.IsNPCBot);
					m_simplePartyMembers[i].m_playerName.text = lobbyPlayerInfo.GetHandle();
					i++;
				}
			}
		}
		for (; i < m_simplePartyMembers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_simplePartyMembers[i], false);
		}
	}
}
