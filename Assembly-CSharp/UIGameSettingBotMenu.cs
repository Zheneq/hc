using System;
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
		for (int i = 0; i < this.m_botSelection.Length; i++)
		{
			UIEventTriggerUtils.AddListener(this.m_botSelection[i].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.BotMenuItemClicked));
			UIEventTriggerUtils.AddListener(this.m_botSelection[i].gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.BotMenuItemEnter));
			UIEventTriggerUtils.AddListener(this.m_botSelection[i].gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.BotMenuItemExit));
		}
	}

	public void Setup(UITeamMemberEntry entry, Image button)
	{
		this.m_teamMemberEntry = entry;
		if (this.m_playerList == null)
		{
			this.m_playerList = new List<LobbyPlayerInfo>();
		}
		this.m_playerList.Clear();
		int num = 0;
		int num2 = 0;
		if (entry.m_playerInfo.IsRemoteControlled)
		{
			num2 = entry.m_playerInfo.ControllingPlayerId;
			this.m_playerList.Add(null);
			this.m_botSelection[0].text = StringUtil.TR("Bot", "Global");
			num = 1;
		}
		GameManager gameManager = GameManager.Get();
		using (List<LobbyPlayerInfo>.Enumerator enumerator = gameManager.TeamInfo.TeamPlayerInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
				if (!lobbyPlayerInfo.IsNPCBot)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameSettingBotMenu.Setup(UITeamMemberEntry, Image)).MethodHandle;
					}
					if (!lobbyPlayerInfo.IsRemoteControlled && num2 != lobbyPlayerInfo.PlayerId)
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
						if (!lobbyPlayerInfo.IsSpectator)
						{
							this.m_playerList.Add(lobbyPlayerInfo);
							this.m_botSelection[num].text = lobbyPlayerInfo.GetHandle();
							UIManager.SetGameObjectActive(this.m_botSelection[num], true, null);
							num++;
						}
					}
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (int i = num; i < this.m_botSelection.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_botSelection[i], false, null);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void BotMenuItemEnter(BaseEventData data)
	{
		for (int i = 0; i < this.m_botSelection.Length; i++)
		{
			if (this.m_botSelection[i].gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				this.m_botSelection[i].color = Color.white;
				break;
			}
		}
	}

	private void BotMenuItemExit(BaseEventData data)
	{
		this.ClearButtonSelections((data as PointerEventData).pointerCurrentRaycast.gameObject);
	}

	private void ClearButtonSelections(GameObject gObj)
	{
		for (int i = 0; i < this.m_botSelection.Length; i++)
		{
			if (this.m_botSelection[i].gameObject != gObj)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameSettingBotMenu.ClearButtonSelections(GameObject)).MethodHandle;
				}
				this.m_botSelection[i].color = Color.grey;
			}
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

	private void BotMenuItemClicked(BaseEventData data)
	{
		for (int i = 0; i < this.m_botSelection.Length; i++)
		{
			if (this.m_botSelection[i].gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameSettingBotMenu.BotMenuItemClicked(BaseEventData)).MethodHandle;
				}
				this.m_teamMemberEntry.SetControllingPlayerInfo(this.m_playerList[i]);
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		base.SetVisible(false);
	}

	private void OnDisable()
	{
		this.ClearButtonSelections(null);
		this.m_teamMemberEntry.ClearSelection(null);
	}
}
