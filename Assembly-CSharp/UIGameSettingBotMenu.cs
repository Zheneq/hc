using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameSettingBotMenu : UITooltipBase
{
	public TextMeshProUGUI[] m_botSelection;

	private UITeamMemberEntry m_teamMemberEntry;

	private List<LobbyPlayerInfo> m_playerList;

	public void Awake()
	{
		for (int i = 0; i < m_botSelection.Length; i++)
		{
			UIEventTriggerUtils.AddListener(m_botSelection[i].gameObject, EventTriggerType.PointerClick, BotMenuItemClicked);
			UIEventTriggerUtils.AddListener(m_botSelection[i].gameObject, EventTriggerType.PointerEnter, BotMenuItemEnter);
			UIEventTriggerUtils.AddListener(m_botSelection[i].gameObject, EventTriggerType.PointerExit, BotMenuItemExit);
		}
	}

	public void Setup(UITeamMemberEntry entry, Image button)
	{
		m_teamMemberEntry = entry;
		if (m_playerList == null)
		{
			m_playerList = new List<LobbyPlayerInfo>();
		}
		m_playerList.Clear();
		int num = 0;
		int num2 = 0;
		if (entry.m_playerInfo.IsRemoteControlled)
		{
			num2 = entry.m_playerInfo.ControllingPlayerId;
			m_playerList.Add(null);
			m_botSelection[0].text = StringUtil.TR("Bot", "Global");
			num = 1;
		}
		GameManager gameManager = GameManager.Get();
		using (List<LobbyPlayerInfo>.Enumerator enumerator = gameManager.TeamInfo.TeamPlayerInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo current = enumerator.Current;
				if (!current.IsNPCBot)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!current.IsRemoteControlled && num2 != current.PlayerId)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!current.IsSpectator)
						{
							m_playerList.Add(current);
							m_botSelection[num].text = current.GetHandle();
							UIManager.SetGameObjectActive(m_botSelection[num], true);
							num++;
						}
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int i = num; i < m_botSelection.Length; i++)
		{
			UIManager.SetGameObjectActive(m_botSelection[i], false);
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void BotMenuItemEnter(BaseEventData data)
	{
		int num = 0;
		while (true)
		{
			if (num < m_botSelection.Length)
			{
				if (m_botSelection[num].gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		m_botSelection[num].color = Color.white;
	}

	private void BotMenuItemExit(BaseEventData data)
	{
		ClearButtonSelections((data as PointerEventData).pointerCurrentRaycast.gameObject);
	}

	private void ClearButtonSelections(GameObject gObj)
	{
		for (int i = 0; i < m_botSelection.Length; i++)
		{
			if (m_botSelection[i].gameObject != gObj)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_botSelection[i].color = Color.grey;
			}
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

	private void BotMenuItemClicked(BaseEventData data)
	{
		for (int i = 0; i < m_botSelection.Length; i++)
		{
			if (m_botSelection[i].gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_teamMemberEntry.SetControllingPlayerInfo(m_playerList[i]);
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			SetVisible(false);
			return;
		}
	}

	private void OnDisable()
	{
		ClearButtonSelections(null);
		m_teamMemberEntry.ClearSelection(null);
	}
}
