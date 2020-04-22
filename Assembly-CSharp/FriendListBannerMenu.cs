using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListBannerMenu : UITooltipBase
{
	public enum FriendMenuButtonAction
	{
		SendMessage,
		InviteToParty,
		ViewProfile,
		InviteToGroupChat,
		BlockPlayer,
		ReportPlayer,
		RemoveFriend,
		InviteToGame,
		ObserveGame,
		AddNote
	}

	[Serializable]
	public struct FriendListTooltipBannerButton
	{
		public Image m_icon;

		public TextMeshProUGUI m_label;

		public Button m_button;
	}

	public TextMeshProUGUI m_playerName;

	public Color m_unhighlightedMenuItemColor;

	public FriendListTooltipBannerButton[] m_menuButtons;

	public FriendListMenuGroupChat m_groupSubMenu;

	private FriendInfo m_friendInfo;

	public void Start()
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			FriendMenuButtonAction action = (FriendMenuButtonAction)i;
			if (IsValidButtonAction(action, true))
			{
				UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerEnter, OnGroupChatMouseOver);
				UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerExit, OnGroupChatMouseExit);
				UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerClick, OnGroupChatMouseClicked);
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public bool IsValidButtonAction(FriendMenuButtonAction action, bool IsForSetup = false)
	{
		if (action == FriendMenuButtonAction.InviteToGame)
		{
			while (true)
			{
				if (GameManager.Get() != null)
				{
					if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameConfig != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
							{
								int result;
								if (GameManager.Get().GameInfo.GameConfig.GameType == GameType.Custom)
								{
									result = ((GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped) ? 1 : 0);
								}
								else
								{
									result = 0;
								}
								return (byte)result != 0;
							}
							}
						}
					}
				}
				return IsForSetup;
			}
		}
		if (action == FriendMenuButtonAction.InviteToParty)
		{
			if (GameManager.Get() != null)
			{
				if (GameManager.Get().GameInfo != null)
				{
					if (GameManager.Get().GameInfo.GameConfig != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return GameManager.Get().GameInfo.GameConfig.GameType != 0 || GameManager.Get().GameInfo.GameStatus == GameStatus.Stopped;
							}
						}
					}
				}
			}
			return true;
		}
		if (action == FriendMenuButtonAction.ObserveGame)
		{
			while (true)
			{
				if (GameManager.Get() != null)
				{
					if (GameManager.Get().GameplayOverrides != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return m_friendInfo.IsJoinable(GameManager.Get().GameplayOverrides);
							}
						}
					}
				}
				return true;
			}
		}
		int result2;
		if (action != 0)
		{
			if (action != FriendMenuButtonAction.BlockPlayer)
			{
				if (action != FriendMenuButtonAction.RemoveFriend)
				{
					if (action != FriendMenuButtonAction.ReportPlayer)
					{
						result2 = ((action == FriendMenuButtonAction.AddNote) ? 1 : 0);
						goto IL_01d4;
					}
				}
			}
		}
		result2 = 1;
		goto IL_01d4;
		IL_01d4:
		return (byte)result2 != 0;
	}

	private void OpenAddNoteBox()
	{
		string title = StringUtil.TR("FriendNote", "Global");
		string description = string.Format(StringUtil.TR("AddANoteFor", "Global"), m_friendInfo.FriendHandle);
		UIDialogPopupManager.OpenSingleLineInputDialog(title, description, StringUtil.TR("Ok", "Global"), StringUtil.TR("Cancel", "Global"), delegate(UIDialogBox box)
		{
			string text = (box as UISingleInputLineInputDialogBox).m_descriptionBoxInputField.text;
			string arguments = string.Format("{0} {1} {2}", StringUtil.TR("NoteFriend", "SlashCommand"), m_friendInfo.FriendHandle, text);
			SlashCommands.Get().RunSlashCommand("/friend", arguments);
		});
	}

	public void OnGroupChatMouseClicked(BaseEventData data)
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			if (!IsValidButtonAction((FriendMenuButtonAction)i))
			{
				continue;
			}
			if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
			{
				switch (i)
				{
				case 4:
					FriendListPanel.Get().RequestToBlockPlayer(m_friendInfo);
					break;
				case 7:
					FriendListPanel.Get().RequestToInviteToGame(m_friendInfo);
					break;
				case 1:
					FriendListPanel.Get().RequestToInviteToParty(m_friendInfo);
					break;
				case 6:
					FriendListPanel.Get().RequestToRemoveFriend(m_friendInfo);
					break;
				case 0:
					FriendListPanel.Get().RequestToSendMessage(m_friendInfo);
					break;
				case 2:
					FriendListPanel.Get().RequestToViewProfile(m_friendInfo);
					break;
				case 5:
					UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, m_friendInfo.FriendHandle, m_friendInfo.FriendAccountId);
					break;
				case 8:
					FriendListPanel.Get().RequestToObserveGame(m_friendInfo);
					break;
				case 9:
					OpenAddNoteBox();
					break;
				}
				break;
			}
		}
		SetVisible(false);
	}

	private void OnDisable()
	{
	}

	public void OnGroupChatMouseOver(BaseEventData data)
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			FriendMenuButtonAction action = (FriendMenuButtonAction)i;
			if (IsValidButtonAction(action))
			{
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
				{
					m_menuButtons[i].m_icon.color = Color.white;
					m_menuButtons[i].m_label.color = Color.white;
				}
				else
				{
					m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
					m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
				}
			}
			else
			{
				m_menuButtons[i].m_icon.color = Color.gray;
				m_menuButtons[i].m_label.color = Color.gray;
			}
		}
		if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[3].m_button.gameObject)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_groupSubMenu, true);
					m_groupSubMenu.Setup();
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_groupSubMenu, false);
	}

	public void OnGroupChatMouseExit(BaseEventData data)
	{
	}

	public void Setup(FriendInfo friendInfo)
	{
		m_friendInfo = friendInfo;
		m_playerName.text = friendInfo.FriendHandle;
		UIManager.SetGameObjectActive(m_groupSubMenu, false);
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			FriendMenuButtonAction action = (FriendMenuButtonAction)i;
			if (IsValidButtonAction(action))
			{
				m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
				m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
			}
			else
			{
				m_menuButtons[i].m_icon.color = Color.gray;
				m_menuButtons[i].m_label.color = Color.gray;
			}
		}
	}
}
