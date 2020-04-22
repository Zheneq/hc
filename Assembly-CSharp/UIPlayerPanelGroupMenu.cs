using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerPanelGroupMenu : UITooltipBase
{
	public enum GroupMenuButtonAction
	{
		SendMessage,
		ViewProfile,
		AddToFriends,
		PromoteToLeader,
		KickFromParty,
		ReportPlayer,
		LeaveParty
	}

	[Serializable]
	public struct GroupMenuButton
	{
		public GameObject m_container;

		public Image m_icon;

		public TextMeshProUGUI m_label;

		public Button m_button;
	}

	public TextMeshProUGUI m_playerName;

	public Color m_unhighlightedMenuItemColor;

	public GroupMenuButton[] m_menuButtons;

	private string m_memberHandle;

	private long m_memberAccountId = -1L;

	private bool m_botMasqueradingAsHuman;

	private void Awake()
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerEnter, OnGroupChatMouseOver);
			UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerClick, OnGroupChatMouseClicked);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void OnGroupChatMouseClicked(BaseEventData data)
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			if (!IsValidButtonAction((GroupMenuButtonAction)i))
			{
				continue;
			}
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
			if (!((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject))
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				switch (i)
				{
				case 2:
					if (m_botMasqueradingAsHuman)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						TextConsole.Get().Write(new TextConsole.Message
						{
							Text = StringUtil.TR("AddFriendRequest", "SlashCommand"),
							MessageType = ConsoleMessageType.SystemMessage
						});
					}
					else
					{
						FriendListPanel.Get().RequestToAddFriend(m_memberHandle);
					}
					break;
				case 4:
					TextConsole.Get().OnInputSubmitted("/kick " + m_memberHandle);
					break;
				case 6:
					TextConsole.Get().OnInputSubmitted("/leave");
					break;
				case 3:
					TextConsole.Get().OnInputSubmitted("/promote " + m_memberHandle);
					break;
				case 0:
					UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + m_memberHandle + " ");
					break;
				case 5:
					UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, m_memberHandle, m_memberAccountId, m_botMasqueradingAsHuman);
					break;
				}
				SetVisible(false);
				return;
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

	public void OnGroupChatMouseOver(BaseEventData data)
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			GroupMenuButtonAction action = (GroupMenuButtonAction)i;
			UIManager.SetGameObjectActive(m_menuButtons[i].m_container, IsButtonActionVisible(action));
			if (IsValidButtonAction(action))
			{
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
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
				m_menuButtons[i].m_icon.color = Color.gray * 0.75f;
				m_menuButtons[i].m_label.color = Color.gray * 0.75f;
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void Setup(UpdateGroupMemberData memberInfo)
	{
		if (memberInfo != null)
		{
			while (true)
			{
				switch (6)
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
			m_memberHandle = memberInfo.MemberDisplayName;
			m_memberAccountId = memberInfo.AccountID;
		}
		else
		{
			m_memberHandle = string.Empty;
			m_memberAccountId = -1L;
			m_botMasqueradingAsHuman = false;
		}
		SetupCommon();
	}

	public void Setup(LobbyPlayerInfo memberInfo)
	{
		if (memberInfo != null)
		{
			while (true)
			{
				switch (2)
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
			m_memberHandle = memberInfo.GetHandle();
			m_memberAccountId = memberInfo.AccountId;
			m_botMasqueradingAsHuman = memberInfo.BotsMasqueradeAsHumans;
		}
		else
		{
			m_memberHandle = string.Empty;
			m_memberAccountId = -1L;
			m_botMasqueradingAsHuman = false;
		}
		SetupCommon();
	}

	public void SetupCommon()
	{
		if (!m_memberHandle.IsNullOrEmpty())
		{
			m_playerName.text = m_memberHandle;
		}
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			GroupMenuButtonAction action = (GroupMenuButtonAction)i;
			UIManager.SetGameObjectActive(m_menuButtons[i].m_container, IsButtonActionVisible(action));
			if (IsValidButtonAction(action))
			{
				m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
				m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
			}
			else
			{
				m_menuButtons[i].m_icon.color = Color.gray * 0.75f;
				m_menuButtons[i].m_label.color = Color.gray * 0.75f;
			}
		}
	}

	public bool IsValidButtonAction(GroupMenuButtonAction action)
	{
		bool result = false;
		if (m_memberAccountId >= 0)
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
			switch (action)
			{
			case GroupMenuButtonAction.AddToFriends:
				result = true;
				break;
			case GroupMenuButtonAction.KickFromParty:
				if (ClientGameManager.Get().GroupInfo.IsLeader)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
				}
				break;
			case GroupMenuButtonAction.LeaveParty:
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
				}
				break;
			case GroupMenuButtonAction.PromoteToLeader:
				if (ClientGameManager.Get().GroupInfo.IsLeader)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
				}
				break;
			case GroupMenuButtonAction.SendMessage:
				result = true;
				break;
			case GroupMenuButtonAction.ReportPlayer:
				result = true;
				break;
			}
		}
		return result;
	}

	public bool IsButtonActionVisible(GroupMenuButtonAction action)
	{
		bool result = true;
		if (m_memberAccountId >= 0 && m_memberAccountId == ClientGameManager.Get().GetPlayerAccountData().AccountId)
		{
			while (true)
			{
				switch (6)
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
			if (action != GroupMenuButtonAction.AddToFriends)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (action != GroupMenuButtonAction.KickFromParty && action != GroupMenuButtonAction.PromoteToLeader && action != 0)
				{
					if (action != GroupMenuButtonAction.ReportPlayer)
					{
						goto IL_0063;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			return false;
		}
		goto IL_0063;
		IL_0063:
		return result;
	}
}
