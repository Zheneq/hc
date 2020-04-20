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
				UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatMouseOver));
				UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnGroupChatMouseClicked));
			}
		}
	}

	public bool IsValidButtonAction(GameOverBannerMenu.GameOverButtonAction action, bool IsForSetup = false)
	{
		if (this.m_botMasqueradingAsHuman)
		{
			return true;
		}
		if (!this.m_playerHandle.IsNullOrEmpty() && this.m_playerAccountID != 0L)
		{
			if (action == GameOverBannerMenu.GameOverButtonAction.AddFriend)
			{
				if (ClientGameManager.Get().FriendList.Friends.ContainsKey(this.m_playerAccountID))
				{
					FriendInfo friendInfo = ClientGameManager.Get().FriendList.Friends[this.m_playerAccountID];
					if (friendInfo.FriendStatus != FriendStatus.Friend)
					{
						if (friendInfo.FriendStatus != FriendStatus.Blocked)
						{
							goto IL_CA;
						}
					}
					return false;
				}
			}
			IL_CA:
			if (action == GameOverBannerMenu.GameOverButtonAction.BlockPlayer)
			{
				if (ClientGameManager.Get().FriendList.Friends.ContainsKey(this.m_playerAccountID))
				{
					FriendInfo friendInfo2 = ClientGameManager.Get().FriendList.Friends[this.m_playerAccountID];
					if (friendInfo2.FriendStatus == FriendStatus.Blocked)
					{
						return false;
					}
				}
			}
			if (action == GameOverBannerMenu.GameOverButtonAction.ReportPlayer)
			{
				if (this.m_playerAccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
				{
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
			if (response.LocalizedFailure != null)
			{
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
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, text, StringUtil.TR("Ok", "Global"), null, -1, false);
			}
		}
	}

	public void OnGroupChatMouseClicked(BaseEventData data)
	{
		if (!this.m_playerHandle.IsNullOrEmpty())
		{
			if (this.m_playerAccountID != 0L)
			{
				goto IL_47;
			}
		}
		if (!this.m_botMasqueradingAsHuman)
		{
			return;
		}
		IL_47:
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			if (this.IsValidButtonAction((GameOverBannerMenu.GameOverButtonAction)i, false))
			{
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_menuButtons[i].m_button.gameObject)
				{
					switch (i)
					{
					case 0:
						if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
						{
							if (UIFrontEnd.Get() != null)
							{
								if (UIFrontEnd.Get().m_frontEndChatConsole != null)
								{
									UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + this.m_playerHandle + " ");
									goto IL_1BA;
								}
							}
						}
						if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
						{
							if (HUD_UI.Get() != null)
							{
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
