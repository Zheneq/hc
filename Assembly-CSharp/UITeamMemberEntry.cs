using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITeamMemberEntry : MonoBehaviour
{
	public GridLayoutGroup m_subActionBtnContainer;

	public Image m_hoverHighlight;

	public TextMeshProUGUI m_PlayerText;

	public Image[] m_subButtons;

	public LobbyPlayerInfo m_playerInfo;

	private Team m_teamId;

	private void Start()
	{
		for (int i = 0; i < this.m_subButtons.Length; i++)
		{
			this.m_subButtons[i].color = Color.gray;
			UIEventTriggerUtils.AddListener(this.m_subButtons[i].gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.MouseSubBtnEnter));
			UIEventTriggerUtils.AddListener(this.m_subButtons[i].gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.MouseSubBtnExit));
			int index = i;
			this.m_subButtons[i].GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => this.SetupTooltip(tooltip, (UITeamMemberEntry.SubButtonIndex)index), null);
		}
		UIEventTriggerUtils.AddListener(this.m_subButtons[0].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.DoAddBot));
		this.m_subButtons[1].GetComponent<UITooltipClickObject>().Setup(TooltipType.BotSettingsMenu, new TooltipPopulateCall(this.SetupBotSettingsMenu), null);
		UIEventTriggerUtils.AddListener(this.m_subButtons[2].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.DoKickPlayer));
		UIEventTriggerUtils.AddListener(this.m_subButtons[3].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.DoSwapTeam));
		UIEventTriggerUtils.AddListener(this.m_subButtons[4].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.DoRemoveChar));
		UIEventTriggerUtils.AddListener(this.m_subButtons[5].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.DoSwapSpectator));
	}

	private bool SetupTooltip(UITooltipBase tooltip, UITeamMemberEntry.SubButtonIndex type)
	{
		string tooltipTitle = string.Empty;
		string tooltipText = string.Empty;
		switch (type)
		{
		case UITeamMemberEntry.SubButtonIndex.AddBot:
			tooltipTitle = StringUtil.TR("AddBot", "Global");
			tooltipText = StringUtil.TR("AddBotToTeam", "Global");
			break;
		case UITeamMemberEntry.SubButtonIndex.BotSettings:
			tooltipTitle = StringUtil.TR("ChangeOwner", "Global");
			tooltipText = StringUtil.TR("AssignOwnerOfBot", "Global");
			break;
		case UITeamMemberEntry.SubButtonIndex.KickPlayer:
			tooltipTitle = StringUtil.TR("KickPlayer", "Global");
			tooltipText = StringUtil.TR("KickPlayerFromGame", "Global");
			break;
		case UITeamMemberEntry.SubButtonIndex.SwapTeam:
			tooltipTitle = StringUtil.TR("SwapTeam", "Global");
			tooltipText = StringUtil.TR("SwapPlayerToOtherTeam", "Global");
			break;
		case UITeamMemberEntry.SubButtonIndex.RemoveChar:
			tooltipTitle = StringUtil.TR("RemoveBot", "Global");
			tooltipText = StringUtil.TR("RemoveBotFromGame", "Global");
			break;
		case UITeamMemberEntry.SubButtonIndex.SwapSpectator:
			tooltipTitle = StringUtil.TR("SwapSpectator", "Global");
			tooltipText = StringUtil.TR("SwapPlayerToFramSpectator", "Global");
			break;
		default:
			return false;
		}
		(tooltip as UITitledTooltip).Setup(tooltipTitle, tooltipText, string.Empty);
		return true;
	}

	public void MouseSubBtnEnter(BaseEventData data)
	{
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		for (int i = 0; i < this.m_subButtons.Length; i++)
		{
			if (this.m_subButtons[i].gameObject == gameObject)
			{
				this.m_subButtons[i].color = Color.white;
				break;
			}
		}
	}

	public void ClearSelection(GameObject selectedObject)
	{
		for (int i = 0; i < this.m_subButtons.Length; i++)
		{
			if (this.m_subButtons[i].gameObject != selectedObject)
			{
				this.m_subButtons[i].color = Color.gray;
			}
		}
	}

	public void MouseSubBtnExit(BaseEventData data)
	{
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		this.ClearSelection(gameObject);
	}

	public void DoAddBot(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
		UIGameSettingsPanel.Get().AddBot(this, CharacterType.None);
	}

	private bool SetupBotSettingsMenu(UITooltipBase tooltip)
	{
		(tooltip as UIGameSettingBotMenu).Setup(this, this.m_subButtons[1]);
		return true;
	}

	public void DoKickPlayer(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
		if (!this.m_playerInfo.IsGameOwner)
		{
			UIGameSettingsPanel.Get().KickPlayer(this);
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
		if (this.m_playerInfo.IsNPCBot)
		{
			UIGameSettingsPanel.Get().RemoveBot(this);
		}
		else
		{
			UIGameSettingsPanel.Get().SetControllingPlayerInfo(this, null);
		}
	}

	public void DoSwapSpectator(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
		UIGameSettingsPanel.Get().SwapSpectator(this);
	}

	public void SetTeamPlayerInfo(LobbyPlayerInfo playerInfo)
	{
		this.m_playerInfo = playerInfo;
		UIManager.SetGameObjectActive(this.m_hoverHighlight, true, null);
		UIManager.SetGameObjectActive(this.m_subActionBtnContainer, true, null);
		if (this.m_playerInfo == null)
		{
			this.m_PlayerText.text = "-";
			UIManager.SetGameObjectActive(this.m_subButtons[0], this.m_teamId != Team.Spectator, null);
			UIManager.SetGameObjectActive(this.m_subButtons[1], false, null);
			UIManager.SetGameObjectActive(this.m_subButtons[2], false, null);
			UIManager.SetGameObjectActive(this.m_subButtons[4], false, null);
			UIManager.SetGameObjectActive(this.m_subButtons[3], false, null);
			UIManager.SetGameObjectActive(this.m_subButtons[5], false, null);
		}
		else
		{
			this.m_PlayerText.text = this.m_playerInfo.GetHandle();
			UIManager.SetGameObjectActive(this.m_subButtons[0], false, null);
			UIManager.SetGameObjectActive(this.m_subButtons[3], true, null);
			if (!this.m_playerInfo.IsNPCBot)
			{
				if (!this.m_playerInfo.IsRemoteControlled)
				{
					UIManager.SetGameObjectActive(this.m_subButtons[1], false, null);
					if (this.m_playerInfo.IsGameOwner)
					{
						UIManager.SetGameObjectActive(this.m_subButtons[2], false, null);
						UIManager.SetGameObjectActive(this.m_subButtons[5], true, null);
					}
					else
					{
						UIManager.SetGameObjectActive(this.m_subButtons[2], true, null);
						UIManager.SetGameObjectActive(this.m_subButtons[5], true, null);
					}
					UIManager.SetGameObjectActive(this.m_subButtons[4], false, null);
					goto IL_1C4;
				}
			}
			UIManager.SetGameObjectActive(this.m_subButtons[1], true, null);
			UIManager.SetGameObjectActive(this.m_subButtons[2], false, null);
			UIManager.SetGameObjectActive(this.m_subButtons[4], true, null);
			UIManager.SetGameObjectActive(this.m_subButtons[5], false, null);
		}
		IL_1C4:
		for (int i = 0; i < this.m_subButtons.Length; i++)
		{
			this.m_subButtons[i].color = Color.gray;
		}
		if (!GameManager.Get().GameplayOverrides.AllowSpectators)
		{
			if (this.m_teamId == Team.Spectator)
			{
				this.m_PlayerText.text = StringUtil.TR("COMINGSOON", "Global");
			}
			UIManager.SetGameObjectActive(this.m_subButtons[5], false, null);
		}
	}

	public void SetControllingPlayerInfo(LobbyPlayerInfo controllingPlayerInfo)
	{
		UIGameSettingsPanel.Get().SetControllingPlayerInfo(this, controllingPlayerInfo);
	}

	public void SetEmptyPlayerInfo()
	{
		this.m_playerInfo = null;
		UIManager.SetGameObjectActive(this.m_hoverHighlight, false, null);
		UIManager.SetGameObjectActive(this.m_subActionBtnContainer, false, null);
	}

	public LobbyPlayerInfo GetPlayerInfo()
	{
		return this.m_playerInfo;
	}

	public Team GetTeamId()
	{
		return this.m_teamId;
	}

	public void SetTeamId(Team teamId)
	{
		this.m_teamId = teamId;
	}

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
}
