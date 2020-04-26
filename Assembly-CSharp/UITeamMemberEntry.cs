using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITeamMemberEntry : MonoBehaviour
{
	private enum SubButtonIndex
	{
		None = -1,
		AddBot,
		BotSettings,
		KickPlayer,
		SwapTeam,
		RemoveChar,
		SwapSpectator
	}

	public GridLayoutGroup m_subActionBtnContainer;

	public Image m_hoverHighlight;

	public TextMeshProUGUI m_PlayerText;

	public Image[] m_subButtons;

	public LobbyPlayerInfo m_playerInfo;

	private Team m_teamId;

	private void Start()
	{
		for (int i = 0; i < m_subButtons.Length; i++)
		{
			m_subButtons[i].color = Color.gray;
			UIEventTriggerUtils.AddListener(m_subButtons[i].gameObject, EventTriggerType.PointerEnter, MouseSubBtnEnter);
			UIEventTriggerUtils.AddListener(m_subButtons[i].gameObject, EventTriggerType.PointerExit, MouseSubBtnExit);
			int index = i;
			m_subButtons[i].GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => SetupTooltip(tooltip, (SubButtonIndex)index));
		}
		while (true)
		{
			UIEventTriggerUtils.AddListener(m_subButtons[0].gameObject, EventTriggerType.PointerClick, DoAddBot);
			m_subButtons[1].GetComponent<UITooltipClickObject>().Setup(TooltipType.BotSettingsMenu, SetupBotSettingsMenu);
			UIEventTriggerUtils.AddListener(m_subButtons[2].gameObject, EventTriggerType.PointerClick, DoKickPlayer);
			UIEventTriggerUtils.AddListener(m_subButtons[3].gameObject, EventTriggerType.PointerClick, DoSwapTeam);
			UIEventTriggerUtils.AddListener(m_subButtons[4].gameObject, EventTriggerType.PointerClick, DoRemoveChar);
			UIEventTriggerUtils.AddListener(m_subButtons[5].gameObject, EventTriggerType.PointerClick, DoSwapSpectator);
			return;
		}
	}

	private bool SetupTooltip(UITooltipBase tooltip, SubButtonIndex type)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		switch (type)
		{
		case SubButtonIndex.AddBot:
			empty = StringUtil.TR("AddBot", "Global");
			empty2 = StringUtil.TR("AddBotToTeam", "Global");
			break;
		case SubButtonIndex.BotSettings:
			empty = StringUtil.TR("ChangeOwner", "Global");
			empty2 = StringUtil.TR("AssignOwnerOfBot", "Global");
			break;
		case SubButtonIndex.KickPlayer:
			empty = StringUtil.TR("KickPlayer", "Global");
			empty2 = StringUtil.TR("KickPlayerFromGame", "Global");
			break;
		case SubButtonIndex.RemoveChar:
			empty = StringUtil.TR("RemoveBot", "Global");
			empty2 = StringUtil.TR("RemoveBotFromGame", "Global");
			break;
		case SubButtonIndex.SwapTeam:
			empty = StringUtil.TR("SwapTeam", "Global");
			empty2 = StringUtil.TR("SwapPlayerToOtherTeam", "Global");
			break;
		case SubButtonIndex.SwapSpectator:
			empty = StringUtil.TR("SwapSpectator", "Global");
			empty2 = StringUtil.TR("SwapPlayerToFramSpectator", "Global");
			break;
		default:
			return false;
		}
		(tooltip as UITitledTooltip).Setup(empty, empty2, string.Empty);
		return true;
	}

	public void MouseSubBtnEnter(BaseEventData data)
	{
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		int num = 0;
		while (true)
		{
			if (num < m_subButtons.Length)
			{
				if (m_subButtons[num].gameObject == gameObject)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		while (true)
		{
			m_subButtons[num].color = Color.white;
			return;
		}
	}

	public void ClearSelection(GameObject selectedObject)
	{
		for (int i = 0; i < m_subButtons.Length; i++)
		{
			if (m_subButtons[i].gameObject != selectedObject)
			{
				m_subButtons[i].color = Color.gray;
			}
		}
	}

	public void MouseSubBtnExit(BaseEventData data)
	{
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		ClearSelection(gameObject);
	}

	public void DoAddBot(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
		UIGameSettingsPanel.Get().AddBot(this);
	}

	private bool SetupBotSettingsMenu(UITooltipBase tooltip)
	{
		(tooltip as UIGameSettingBotMenu).Setup(this, m_subButtons[1]);
		return true;
	}

	public void DoKickPlayer(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
		if (m_playerInfo.IsGameOwner)
		{
			return;
		}
		while (true)
		{
			UIGameSettingsPanel.Get().KickPlayer(this);
			return;
		}
	}

	public void DoSwapTeam(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
		UIGameSettingsPanel.Get().SwapTeam(this);
	}

	public void DoRemoveChar(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
		if (m_playerInfo.IsNPCBot)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UIGameSettingsPanel.Get().RemoveBot(this);
					return;
				}
			}
		}
		UIGameSettingsPanel.Get().SetControllingPlayerInfo(this, null);
	}

	public void DoSwapSpectator(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
		UIGameSettingsPanel.Get().SwapSpectator(this);
	}

	public void SetTeamPlayerInfo(LobbyPlayerInfo playerInfo)
	{
		m_playerInfo = playerInfo;
		UIManager.SetGameObjectActive(m_hoverHighlight, true);
		UIManager.SetGameObjectActive(m_subActionBtnContainer, true);
		if (m_playerInfo == null)
		{
			m_PlayerText.text = "-";
			UIManager.SetGameObjectActive(m_subButtons[0], m_teamId != Team.Spectator);
			UIManager.SetGameObjectActive(m_subButtons[1], false);
			UIManager.SetGameObjectActive(m_subButtons[2], false);
			UIManager.SetGameObjectActive(m_subButtons[4], false);
			UIManager.SetGameObjectActive(m_subButtons[3], false);
			UIManager.SetGameObjectActive(m_subButtons[5], false);
		}
		else
		{
			m_PlayerText.text = m_playerInfo.GetHandle();
			UIManager.SetGameObjectActive(m_subButtons[0], false);
			UIManager.SetGameObjectActive(m_subButtons[3], true);
			if (!m_playerInfo.IsNPCBot)
			{
				if (!m_playerInfo.IsRemoteControlled)
				{
					UIManager.SetGameObjectActive(m_subButtons[1], false);
					if (m_playerInfo.IsGameOwner)
					{
						UIManager.SetGameObjectActive(m_subButtons[2], false);
						UIManager.SetGameObjectActive(m_subButtons[5], true);
					}
					else
					{
						UIManager.SetGameObjectActive(m_subButtons[2], true);
						UIManager.SetGameObjectActive(m_subButtons[5], true);
					}
					UIManager.SetGameObjectActive(m_subButtons[4], false);
					goto IL_01c4;
				}
			}
			UIManager.SetGameObjectActive(m_subButtons[1], true);
			UIManager.SetGameObjectActive(m_subButtons[2], false);
			UIManager.SetGameObjectActive(m_subButtons[4], true);
			UIManager.SetGameObjectActive(m_subButtons[5], false);
		}
		goto IL_01c4;
		IL_01c4:
		for (int i = 0; i < m_subButtons.Length; i++)
		{
			m_subButtons[i].color = Color.gray;
		}
		while (true)
		{
			if (GameManager.Get().GameplayOverrides.AllowSpectators)
			{
				return;
			}
			while (true)
			{
				if (m_teamId == Team.Spectator)
				{
					m_PlayerText.text = StringUtil.TR("COMINGSOON", "Global");
				}
				UIManager.SetGameObjectActive(m_subButtons[5], false);
				return;
			}
		}
	}

	public void SetControllingPlayerInfo(LobbyPlayerInfo controllingPlayerInfo)
	{
		UIGameSettingsPanel.Get().SetControllingPlayerInfo(this, controllingPlayerInfo);
	}

	public void SetEmptyPlayerInfo()
	{
		m_playerInfo = null;
		UIManager.SetGameObjectActive(m_hoverHighlight, false);
		UIManager.SetGameObjectActive(m_subActionBtnContainer, false);
	}

	public LobbyPlayerInfo GetPlayerInfo()
	{
		return m_playerInfo;
	}

	public Team GetTeamId()
	{
		return m_teamId;
	}

	public void SetTeamId(Team teamId)
	{
		m_teamId = teamId;
	}
}
