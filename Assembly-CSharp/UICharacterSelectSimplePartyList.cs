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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSimplePartyList.UpdateCharacterList(LobbyPlayerInfo, LobbyTeamInfo, LobbyGameInfo)).MethodHandle;
			}
			if (teamInfo != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (teamInfo.TeamPlayerInfo != null)
				{
					using (List<LobbyPlayerInfo>.Enumerator enumerator = teamInfo.TeamPlayerInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
							if (i < this.m_simplePartyMembers.Length)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (lobbyPlayerInfo.TeamId != playerInfo.TeamId)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (lobbyPlayerInfo.TeamId != Team.Spectator)
									{
										for (;;)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
										UIManager.SetGameObjectActive(this.m_simplePartyMembers[i], true, null);
										UIManager.SetGameObjectActive(this.m_simplePartyMembers[i].m_checkmark, lobbyPlayerInfo.ReadyState == ReadyState.Ready || lobbyPlayerInfo.IsNPCBot, null);
										this.m_simplePartyMembers[i].m_playerName.text = lobbyPlayerInfo.GetHandle();
										i++;
									}
								}
							}
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
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
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
	}
}
