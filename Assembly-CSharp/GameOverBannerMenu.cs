using System;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverBannerMenu : UITooltipBase
{
	public TextMeshProUGUI m_playerName;

	public Color m_unhighlightedMenuItemColor;

	public RectTransform m_bannerMenuContainer;

	public GameOverBannerMenu.GameOverTooltipBannerButton[] m_menuButtons;

	private string m_playerHandle;

	private long m_playerAccountID;

	private bool m_botMasqueradingAsHuman;

	public void Start()
	{
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			GameOverBannerMenu.GameOverButtonAction action = (GameOverBannerMenu.GameOverButtonAction)i;
			if (this.IsValidButtonAction(action, true))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverBannerMenu.Start()).MethodHandle;
				}
				UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatMouseOver));
				UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatMouseClicked));
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

	public bool IsValidButtonAction(GameOverBannerMenu.GameOverButtonAction action, bool IsForSetup = false)
	{
		if (this.m_botMasqueradingAsHuman)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverBannerMenu.IsValidButtonAction(GameOverBannerMenu.GameOverButtonAction, bool)).MethodHandle;
			}
			return true;
		}
		if (!this.m_playerHandle.IsNullOrEmpty() && this.m_playerAccountID != 0L)
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
			if (action == GameOverBannerMenu.GameOverButtonAction.AddFriend)
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
				if (ClientGameManager.Get().FriendList.Friends.ContainsKey(this.m_playerAccountID))
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
					FriendInfo friendInfo = ClientGameManager.Get().FriendList.Friends[this.m_playerAccountID];
					if (friendInfo.FriendStatus != FriendStatus.Friend)
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
						if (friendInfo.FriendStatus != FriendStatus.Blocked)
						{
							goto IL_CA;
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
					return false;
				}
			}
			IL_CA:
			if (action == GameOverBannerMenu.GameOverButtonAction.BlockPlayer)
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
				if (ClientGameManager.Get().FriendList.Friends.ContainsKey(this.m_playerAccountID))
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
					FriendInfo friendInfo2 = ClientGameManager.Get().FriendList.Friends[this.m_playerAccountID];
					if (friendInfo2.FriendStatus == FriendStatus.Blocked)
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
						return false;
					}
				}
			}
			if (action == GameOverBannerMenu.GameOverButtonAction.ReportPlayer)
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
				if (this.m_playerAccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
					return false;
				}
			}
		}
		return true;
	}

	private void HandleFriendUpdateResponse(FriendUpdateResponse response)
	{
		if (!response.Success)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverBannerMenu.HandleFriendUpdateResponse(FriendUpdateResponse)).MethodHandle;
			}
			if (response.LocalizedFailure != null)
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
				response.ErrorMessage = response.LocalizedFailure.ToString();
			}
			else if (response.ErrorMessage.IsNullOrEmpty())
			{
				response.ErrorMessage = StringUtil.TR("ServerError", "Global");
			}
			string text = null;
			switch (response.FriendOperation)
			{
			case FriendOperation.Add:
				text = string.Format(StringUtil.TR("FailedFriendAdd", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Remove:
				text = string.Format(StringUtil.TR("FailedFriendRemove", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Accept:
				text = string.Format(StringUtil.TR("FailedFriendAccept", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Reject:
				text = string.Format(StringUtil.TR("FailedFriendReject", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Block:
				text = string.Format(StringUtil.TR("FailedFriendBlock", "FriendList"), response.ErrorMessage);
				break;
			}
			if (!text.IsNullOrEmpty())
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
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, text, StringUtil.TR("Ok", "Global"), null, -1, false);
			}
		}
	}

	public void OnGroupChatMouseClicked(BaseEventData data)
	{
		if (!this.m_playerHandle.IsNullOrEmpty())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverBannerMenu.OnGroupChatMouseClicked(BaseEventData)).MethodHandle;
			}
			if (this.m_playerAccountID != 0L)
			{
				goto IL_47;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!this.m_botMasqueradingAsHuman)
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
			return;
		}
		IL_47:
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			if (this.IsValidButtonAction((GameOverBannerMenu.GameOverButtonAction)i, false))
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
					switch (i)
					{
					case 0:
						if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
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
							if (UIFrontEnd.Get() != null)
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
								if (UIFrontEnd.Get().m_frontEndChatConsole != null)
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
									UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + this.m_playerHandle + " ");
									goto IL_1BA;
								}
							}
						}
						if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
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
							if (HUD_UI.Get() != null)
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
								if (HUD_UI.Get().m_textConsole != null)
								{
									HUD_UI.Get().m_textConsole.SelectInput("/whisper " + this.m_playerHandle + " ");
								}
							}
						}
						IL_1BA:
						base.SetVisible(false);
						break;
					case 1:
						if (this.m_botMasqueradingAsHuman)
						{
							TextConsole.Get().Write(new TextConsole.Message
							{
								Text = string.Format(StringUtil.TR("InviteSentTo", "SlashCommand"), this.m_playerHandle),
								MessageType = ConsoleMessageType.SystemMessage
							}, null);
						}
						else
						{
							SlashCommands.Get().RunSlashCommand("/invite", this.m_playerHandle);
						}
						base.SetVisible(false);
						break;
					case 2:
						if (this.m_botMasqueradingAsHuman)
						{
							TextConsole.Get().Write(new TextConsole.Message
							{
								Text = StringUtil.TR("AddFriendRequest", "SlashCommand"),
								MessageType = ConsoleMessageType.SystemMessage
							}, null);
						}
						else
						{
							ClientGameManager.Get().UpdateFriend(null, this.m_playerAccountID, FriendOperation.Add, string.Empty, new Action<FriendUpdateResponse>(this.HandleFriendUpdateResponse));
						}
						base.SetVisible(false);
						break;
					case 3:
					{
						string title = StringUtil.TR("BlockPlayer", "FriendList");
						string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), this.m_playerHandle);
						string blockReportHandle = this.m_playerHandle;
						bool botMasqueradingAsHuman = this.m_botMasqueradingAsHuman;
						UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox dialogReference)
						{
							if (botMasqueradingAsHuman)
							{
								TextConsole.Get().Write(new TextConsole.Message
								{
									Text = string.Format(StringUtil.TR("SuccessfullyBlocked", "SlashCommand"), blockReportHandle),
									MessageType = ConsoleMessageType.SystemMessage
								}, null);
							}
							else
							{
								SlashCommands.Get().RunSlashCommand("/block", blockReportHandle);
							}
						}, null, false, false);
						base.SetVisible(false);
						break;
					}
					case 4:
						UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, this.m_playerHandle, this.m_playerAccountID, this.m_botMasqueradingAsHuman);
						base.SetVisible(false);
						break;
					}
					break;
				}
			}
		}
	}

	private void OnDisable()
	{
		this.m_playerHandle = string.Empty;
		this.m_playerAccountID = 0L;
		this.m_botMasqueradingAsHuman = false;
	}

	public void OnGroupChatMouseOver(BaseEventData data)
	{
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			GameOverBannerMenu.GameOverButtonAction action = (GameOverBannerMenu.GameOverButtonAction)i;
			if (this.IsValidButtonAction(action, false))
			{
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_menuButtons[i].m_button.gameObject)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverBannerMenu.OnGroupChatMouseOver(BaseEventData)).MethodHandle;
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
				this.m_menuButtons[i].m_icon.color = Color.gray;
				this.m_menuButtons[i].m_label.color = Color.gray;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void Setup(string accountHandle, long accountID)
	{
		this.m_playerHandle = accountHandle;
		this.m_playerAccountID = accountID;
		this.m_botMasqueradingAsHuman = false;
		this.m_playerName.text = this.m_playerHandle;
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			GameOverBannerMenu.GameOverButtonAction action = (GameOverBannerMenu.GameOverButtonAction)i;
			if (this.IsValidButtonAction(action, false))
			{
				this.m_menuButtons[i].m_icon.color = this.m_unhighlightedMenuItemColor;
				this.m_menuButtons[i].m_label.color = this.m_unhighlightedMenuItemColor;
			}
			else
			{
				this.m_menuButtons[i].m_icon.color = Color.gray;
				this.m_menuButtons[i].m_label.color = Color.gray;
			}
		}
	}

	public void Setup(MatchResultsStatline statLine)
	{
		this.m_playerHandle = statLine.DisplayName;
		this.m_playerAccountID = statLine.AccountID;
		this.m_botMasqueradingAsHuman = statLine.IsBotMasqueradingAsHuman;
		this.m_playerName.text = this.m_playerHandle;
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			GameOverBannerMenu.GameOverButtonAction action = (GameOverBannerMenu.GameOverButtonAction)i;
			if (this.IsValidButtonAction(action, false))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverBannerMenu.Setup(MatchResultsStatline)).MethodHandle;
				}
				this.m_menuButtons[i].m_icon.color = this.m_unhighlightedMenuItemColor;
				this.m_menuButtons[i].m_label.color = this.m_unhighlightedMenuItemColor;
			}
			else
			{
				this.m_menuButtons[i].m_icon.color = Color.gray;
				this.m_menuButtons[i].m_label.color = Color.gray;
			}
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

	public void Setup(ActorData actorData)
	{
		PlayerDetails playerDetails = GameFlow.Get().playerDetails[actorData.PlayerData.GetPlayer()];
		this.m_playerHandle = playerDetails.m_handle;
		this.m_playerAccountID = playerDetails.m_accountId;
		this.m_botMasqueradingAsHuman = playerDetails.m_botsMasqueradeAsHumans;
		this.m_playerName.text = playerDetails.m_handle;
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			GameOverBannerMenu.GameOverButtonAction action = (GameOverBannerMenu.GameOverButtonAction)i;
			if (this.IsValidButtonAction(action, false))
			{
				this.m_menuButtons[i].m_icon.color = this.m_unhighlightedMenuItemColor;
				this.m_menuButtons[i].m_label.color = this.m_unhighlightedMenuItemColor;
			}
			else
			{
				this.m_menuButtons[i].m_icon.color = Color.gray;
				this.m_menuButtons[i].m_label.color = Color.gray;
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(GameOverBannerMenu.Setup(ActorData)).MethodHandle;
		}
	}

	public enum GameOverButtonAction
	{
		SendMessage,
		InviteToParty,
		AddFriend,
		BlockPlayer,
		ReportPlayer
	}

	[Serializable]
	public struct GameOverTooltipBannerButton
	{
		public Image m_icon;

		public TextMeshProUGUI m_label;

		public Button m_button;
	}
}
