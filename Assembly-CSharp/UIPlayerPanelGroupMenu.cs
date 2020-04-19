using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerPanelGroupMenu : UITooltipBase
{
	public TextMeshProUGUI m_playerName;

	public Color m_unhighlightedMenuItemColor;

	public UIPlayerPanelGroupMenu.GroupMenuButton[] m_menuButtons;

	private string m_memberHandle;

	private long m_memberAccountId = -1L;

	private bool m_botMasqueradingAsHuman;

	private void Awake()
	{
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatMouseOver));
			UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatMouseClicked));
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerPanelGroupMenu.Awake()).MethodHandle;
		}
	}

	public void OnGroupChatMouseClicked(BaseEventData data)
	{
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			if (this.IsValidButtonAction((UIPlayerPanelGroupMenu.GroupMenuButtonAction)i))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerPanelGroupMenu.OnGroupChatMouseClicked(BaseEventData)).MethodHandle;
				}
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_menuButtons[i].m_button.gameObject)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					switch (i)
					{
					case 0:
						UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + this.m_memberHandle + " ");
						break;
					case 2:
						if (this.m_botMasqueradingAsHuman)
						{
							for (;;)
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
							}, null);
						}
						else
						{
							FriendListPanel.Get().RequestToAddFriend(this.m_memberHandle);
						}
						break;
					case 3:
						TextConsole.Get().OnInputSubmitted("/promote " + this.m_memberHandle);
						break;
					case 4:
						TextConsole.Get().OnInputSubmitted("/kick " + this.m_memberHandle);
						break;
					case 5:
						UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, this.m_memberHandle, this.m_memberAccountId, this.m_botMasqueradingAsHuman);
						break;
					case 6:
						TextConsole.Get().OnInputSubmitted("/leave");
						break;
					}
					base.SetVisible(false);
					return;
				}
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void OnGroupChatMouseOver(BaseEventData data)
	{
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			UIPlayerPanelGroupMenu.GroupMenuButtonAction action = (UIPlayerPanelGroupMenu.GroupMenuButtonAction)i;
			UIManager.SetGameObjectActive(this.m_menuButtons[i].m_container, this.IsButtonActionVisible(action), null);
			if (this.IsValidButtonAction(action))
			{
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_menuButtons[i].m_button.gameObject)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerPanelGroupMenu.OnGroupChatMouseOver(BaseEventData)).MethodHandle;
					}
					this.m_menuButtons[i].m_icon.color = Color.white;
					this.m_menuButtons[i].m_label.color = Color.white;
				}
				else
				{
					this.m_menuButtons[i].m_icon.color = this.m_unhighlightedMenuItemColor;
					this.m_menuButtons[i].m_label.color = this.m_unhighlightedMenuItemColor;
				}
			}
			else
			{
				this.m_menuButtons[i].m_icon.color = Color.gray * 0.75f;
				this.m_menuButtons[i].m_label.color = Color.gray * 0.75f;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void Setup(UpdateGroupMemberData memberInfo)
	{
		if (memberInfo != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerPanelGroupMenu.Setup(UpdateGroupMemberData)).MethodHandle;
			}
			this.m_memberHandle = memberInfo.MemberDisplayName;
			this.m_memberAccountId = memberInfo.AccountID;
		}
		else
		{
			this.m_memberHandle = string.Empty;
			this.m_memberAccountId = -1L;
			this.m_botMasqueradingAsHuman = false;
		}
		this.SetupCommon();
	}

	public void Setup(LobbyPlayerInfo memberInfo)
	{
		if (memberInfo != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerPanelGroupMenu.Setup(LobbyPlayerInfo)).MethodHandle;
			}
			this.m_memberHandle = memberInfo.GetHandle();
			this.m_memberAccountId = memberInfo.AccountId;
			this.m_botMasqueradingAsHuman = memberInfo.BotsMasqueradeAsHumans;
		}
		else
		{
			this.m_memberHandle = string.Empty;
			this.m_memberAccountId = -1L;
			this.m_botMasqueradingAsHuman = false;
		}
		this.SetupCommon();
	}

	public void SetupCommon()
	{
		if (!this.m_memberHandle.IsNullOrEmpty())
		{
			this.m_playerName.text = this.m_memberHandle;
		}
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			UIPlayerPanelGroupMenu.GroupMenuButtonAction action = (UIPlayerPanelGroupMenu.GroupMenuButtonAction)i;
			UIManager.SetGameObjectActive(this.m_menuButtons[i].m_container, this.IsButtonActionVisible(action), null);
			if (this.IsValidButtonAction(action))
			{
				this.m_menuButtons[i].m_icon.color = this.m_unhighlightedMenuItemColor;
				this.m_menuButtons[i].m_label.color = this.m_unhighlightedMenuItemColor;
			}
			else
			{
				this.m_menuButtons[i].m_icon.color = Color.gray * 0.75f;
				this.m_menuButtons[i].m_label.color = Color.gray * 0.75f;
			}
		}
	}

	public bool IsValidButtonAction(UIPlayerPanelGroupMenu.GroupMenuButtonAction action)
	{
		bool result = false;
		if (this.m_memberAccountId >= 0L)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerPanelGroupMenu.IsValidButtonAction(UIPlayerPanelGroupMenu.GroupMenuButtonAction)).MethodHandle;
			}
			switch (action)
			{
			case UIPlayerPanelGroupMenu.GroupMenuButtonAction.SendMessage:
				result = true;
				break;
			case UIPlayerPanelGroupMenu.GroupMenuButtonAction.AddToFriends:
				result = true;
				break;
			case UIPlayerPanelGroupMenu.GroupMenuButtonAction.PromoteToLeader:
				if (ClientGameManager.Get().GroupInfo.IsLeader)
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
					result = true;
				}
				break;
			case UIPlayerPanelGroupMenu.GroupMenuButtonAction.KickFromParty:
				if (ClientGameManager.Get().GroupInfo.IsLeader)
				{
					for (;;)
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
			case UIPlayerPanelGroupMenu.GroupMenuButtonAction.ReportPlayer:
				result = true;
				break;
			case UIPlayerPanelGroupMenu.GroupMenuButtonAction.LeaveParty:
				if (ClientGameManager.Get().GroupInfo.InAGroup)
				{
					for (;;)
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
			}
		}
		return result;
	}

	public bool IsButtonActionVisible(UIPlayerPanelGroupMenu.GroupMenuButtonAction action)
	{
		bool result = true;
		if (this.m_memberAccountId >= 0L && this.m_memberAccountId == ClientGameManager.Get().GetPlayerAccountData().AccountId)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerPanelGroupMenu.IsButtonActionVisible(UIPlayerPanelGroupMenu.GroupMenuButtonAction)).MethodHandle;
			}
			if (action != UIPlayerPanelGroupMenu.GroupMenuButtonAction.AddToFriends)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (action != UIPlayerPanelGroupMenu.GroupMenuButtonAction.KickFromParty && action != UIPlayerPanelGroupMenu.GroupMenuButtonAction.PromoteToLeader && action != UIPlayerPanelGroupMenu.GroupMenuButtonAction.SendMessage)
				{
					if (action != UIPlayerPanelGroupMenu.GroupMenuButtonAction.ReportPlayer)
					{
						return result;
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
			}
			return false;
		}
		return result;
	}

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
}
