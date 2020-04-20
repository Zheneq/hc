using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using I2.Loc;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextConsole : MonoBehaviour
{
	public RectTransform m_ScaleBoxParent;

	public UI_NewTextList m_theTextList;

	public Image m_background;

	public TMP_InputField m_textInput;

	public TextMeshProUGUI m_chatRoomName;

	public TextMeshProUGUI m_chatText;

	public TextMeshProUGUI m_chatPlaceholder;

	public ScrollRect m_scrollRect;

	public Image m_scrollViewMask;

	public Scrollbar m_scrollBar;

	public Image[] m_inGameFadingImages;

	public Image[] m_frontEndFadingImages;

	public CanvasGroup[] m_inGameCanvasGroups;

	public CanvasGroup[] m_frontEndCanvasGroups;

	public UIAutocompleteList m_autocompleteList;

	public UIChatroomList m_chatroomList;

	public Image m_chatroomHitbox;

	public bool m_doNotAutoFade;

	public float m_startAlpha;

	public Image m_newTextGlow;

	private bool m_visible;

	private bool m_hovering;

	private float m_timeTillCollapse = -1f;

	private const float m_xScale = 0.11f;

	private const float m_yScale = 0.2f;

	private bool m_scrollChat;

	private EasedFloat chatAlpha = new EasedFloat(0f);

	private const float FADE_IN_TIME = 0.3f;

	private const float FADE_OUT_TIME = 0.3f;

	private float lastAlphaSet;

	private bool blockingRaycasts;

	private bool m_inputJustCleared;

	private bool m_checkForNextTextGlow;

	private bool m_updateCaret;

	private int m_caretPositionToUpdate;

	private int m_lastCaratPosition;

	private bool m_escapeJustPressed;

	private bool m_lastIsTextInputNotSelected;

	private bool m_ignoreNextTypeInput;

	private bool m_handlingMessages;

	private bool m_hasGame;

	private string m_chatCommand;

	private bool setInputSelected;

	private const int c_maxConsoleStringLength = 0x100;

	private const int c_maxTextSendLength = 0xC8;

	private static Queue<UITextConsole.HandledMessage> s_handledMessages = new Queue<UITextConsole.HandledMessage>();

	private static List<string> s_history = new List<string>();

	private string m_storedHistory;

	private string m_storedChatCommand;

	private int m_historyIndex;

	private float m_timeSinceLastWhisper;

	private string m_lastWhisperer = string.Empty;

	private DateTime m_lastSystemMessageCheckPST = DateTime.MinValue;

	private bool m_hadNextAlertTime;

	private bool m_hadCurrentAlert;

	private static List<SlashCommand> m_chatCommands = new List<SlashCommand>();

	private static List<SlashCommand> m_playerCommands = new List<SlashCommand>();

	private static SlashCommand_ChatWhisper m_whisperCommand = new SlashCommand_ChatWhisper();

	private static SlashCommand_ChatGeneral m_globalCommand = new SlashCommand_ChatGeneral();

	private static SlashCommand_ChatTeam m_teamCommand = new SlashCommand_ChatTeam();

	private static SlashCommand_ChatGame m_allCommand = new SlashCommand_ChatGame();

	private static SlashCommand_GroupChat m_groupCommand = new SlashCommand_GroupChat();

	private static SlashCommand_Friend m_friendCommand = new SlashCommand_Friend();

	private static List<string> m_frontendAutocomplete = new List<string>();

	private static List<string> m_inGameAutocomplete = new List<string>();

	private static List<string> m_playerAutocomplete = new List<string>();

	private static List<string> m_friendAutocomplete = new List<string>();

	private static List<TextMeshProUGUI> m_possibilitiesEntries = new List<TextMeshProUGUI>();

	private static IComparer<string> m_caseInsenitiveComparer = StringComparer.OrdinalIgnoreCase;

	private static bool m_isAutoCompleteInitialized = false;

	private static List<string> m_whisperedPlayers = new List<string>();

	[CompilerGenerated]
	private static Action<FriendStatusNotification> f__mg_cache0;

	[CompilerGenerated]
	private static Action f__mg_cache1;

	[CompilerGenerated]
	private static Action<FriendStatusNotification> f__mg_cache2;

	[CompilerGenerated]
	private static Action f__mg_cache3;

	private bool m_changeChannelAlpha
	{
		get
		{
			return UIManager.Get().CurrentState == UIManager.ClientState.InGame;
		}
	}

	private static void InitializeAutoComplete()
	{
		if (!UITextConsole.m_isAutoCompleteInitialized)
		{
			UITextConsole.m_chatCommands.Add(UITextConsole.m_globalCommand);
			UITextConsole.m_chatCommands.Add(UITextConsole.m_teamCommand);
			UITextConsole.m_chatCommands.Add(UITextConsole.m_allCommand);
			UITextConsole.m_chatCommands.Add(UITextConsole.m_groupCommand);
			UITextConsole.m_chatCommands.Add(UITextConsole.m_whisperCommand);
			UITextConsole.m_playerCommands.Add(UITextConsole.m_whisperCommand);
			UITextConsole.m_playerCommands.Add(new SlashCommand_GroupInvite());
			UITextConsole.m_playerCommands.Add(new SlashCommand_UserBlock());
			UITextConsole.BuildLocalizedSlashCommands();
			if (ClientGameManager.Get() != null)
			{
				using (Dictionary<long, FriendInfo>.ValueCollection.Enumerator enumerator = ClientGameManager.Get().FriendList.Friends.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FriendInfo friendInfo = enumerator.Current;
						if (friendInfo.FriendStatus == FriendStatus.Friend)
						{
							UITextConsole.TryAddToAutoComplete(friendInfo.FriendHandle);
						}
					}
				}
			}
			UITextConsole.m_isAutoCompleteInitialized = true;
		}
	}

	private void Start()
	{
		this.m_autocompleteList.SetVisible(false);
		this.m_chatroomList.SetVisible(false);
		UITextConsole.InitializeAutoComplete();
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnPointerEnter));
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnPointerExit));
		UIEventTriggerUtils.AddListener(this.m_textInput.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnInputClicked));
		UIEventTriggerUtils.AddListener(this.m_chatroomHitbox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnChatroomClick));
		UIEventTriggerUtils.AddListener(this.m_scrollBar.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		UIEventTriggerUtils.AddListener(this.m_background.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		this.m_textInput.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.m_textInput.onValueChanged.AddListener(new UnityAction<string>(this.OnTypeInput));
		this.m_scrollRect.scrollSensitivity = 100f;
		this.lastAlphaSet = -1f;
		this.m_visible = false;
		this.chatAlpha.EaseTo(this.m_startAlpha, 0f);
		this.ApplyChatAlpha();
		this.blockingRaycasts = false;
		for (int i = 0; i < this.m_inGameCanvasGroups.Length; i++)
		{
			this.m_inGameCanvasGroups[i].blocksRaycasts = true;
		}
		for (int j = 0; j < this.m_frontEndCanvasGroups.Length; j++)
		{
			this.m_frontEndCanvasGroups[j].blocksRaycasts = this.blockingRaycasts;
		}
		if (this.m_newTextGlow != null)
		{
			UIManager.SetGameObjectActive(this.m_newTextGlow, false, null);
		}
		this.m_storedHistory = null;
		this.m_storedChatCommand = null;
		this.m_historyIndex = UITextConsole.s_history.Count;
		UITextConsole.HandledMessage[] array = UITextConsole.s_handledMessages.ToArray();
		for (int k = 0; k < array.Length; k++)
		{
			this.DisplayMessage(array[k].Message, array[k].AllowedEmojis);
		}
		this.m_theTextList.HideRecentText();
		if (this.m_changeChannelAlpha)
		{
			if (this.m_startAlpha == 0f)
			{
				UIManager.SetGameObjectActive(this.m_chatroomHitbox, false, null);
			}
		}
		this.ChangeChatRoom();
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			
			clientGameManager.OnFriendStatusNotification += new Action<FriendStatusNotification>(UITextConsole.HandleFriendStatusNotification);
			ClientGameManager clientGameManager2 = ClientGameManager.Get();
			
			clientGameManager2.OnGroupUpdateNotification += new Action(UITextConsole.OnGroupUpdateNotification);
			ClientGameManager.Get().OnGameInfoNotification += this.OnGameInfoNotification;
		}
		this.RebuildLocalizedText();
		LocalizationManager.OnLocalizeEvent += this.RebuildLocalizedText;
	}

	private void RebuildLocalizedText()
	{
		using (List<SlashCommand>.Enumerator enumerator = UITextConsole.m_chatCommands.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SlashCommand slashCommand = enumerator.Current;
				slashCommand.Localize();
			}
		}
		using (List<SlashCommand>.Enumerator enumerator2 = UITextConsole.m_playerCommands.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				SlashCommand slashCommand2 = enumerator2.Current;
				slashCommand2.Localize();
			}
		}
	}

	public static void BuildLocalizedSlashCommands()
	{
		UITextConsole.m_friendAutocomplete.Clear();
		UITextConsole.m_friendAutocomplete.Add(StringUtil.TR("AcceptFriend", "SlashCommand"));
		UITextConsole.m_friendAutocomplete.Add(StringUtil.TR("AddFriend", "SlashCommand"));
		UITextConsole.m_friendAutocomplete.Add(StringUtil.TR("NoteFriend", "SlashCommand"));
		UITextConsole.m_friendAutocomplete.Add(StringUtil.TR("RejectFriend", "SlashCommand"));
		UITextConsole.m_friendAutocomplete.Add(StringUtil.TR("RemoveFriend", "SlashCommand"));
		UITextConsole.m_frontendAutocomplete.Clear();
		UITextConsole.m_inGameAutocomplete.Clear();
		if (SlashCommands.Get() != null)
		{
			using (List<SlashCommand>.Enumerator enumerator = SlashCommands.Get().m_slashCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlashCommand slashCommand = enumerator.Current;
					if (!slashCommand.PublicFacing)
					{
						if (!ClientGameManager.Get().HasDeveloperAccess())
						{
							continue;
						}
					}
					if (slashCommand.AvailableInFrontEnd)
					{
						UITextConsole.m_frontendAutocomplete.Add(slashCommand.Command);
					}
					if (slashCommand.AvailableInGame)
					{
						UITextConsole.m_inGameAutocomplete.Add(slashCommand.Command);
					}
					if (slashCommand.Aliases == null)
					{
					}
					else
					{
						foreach (string item in slashCommand.Aliases)
						{
							if (slashCommand.AvailableInFrontEnd)
							{
								UITextConsole.m_frontendAutocomplete.Add(item);
							}
							if (slashCommand.AvailableInGame)
							{
								UITextConsole.m_inGameAutocomplete.Add(item);
							}
						}
					}
				}
			}
		}
		UITextConsole.m_frontendAutocomplete.Add(StringUtil.TR("/reply", "SlashCommand"));
		UITextConsole.m_frontendAutocomplete.Add(StringUtil.TR("/reply", "SlashCommandAlias1"));
		UITextConsole.m_frontendAutocomplete.Sort();
		UITextConsole.m_inGameAutocomplete.Add(StringUtil.TR("/reply", "SlashCommand"));
		UITextConsole.m_inGameAutocomplete.Add(StringUtil.TR("/reply", "SlashCommandAlias1"));
		UITextConsole.m_inGameAutocomplete.Sort();
	}

	private void OnScroll(BaseEventData data)
	{
		if ((double)this.chatAlpha < 0.5)
		{
			return;
		}
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	private static void HandleFriendStatusNotification(FriendStatusNotification notification)
	{
		if (notification.FriendList.IsError)
		{
			TextConsole.Get().Write("Friends list temporarily unavailable", ConsoleMessageType.Error);
			return;
		}
		using (Dictionary<long, FriendInfo>.ValueCollection.Enumerator enumerator = notification.FriendList.Friends.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FriendInfo friendInfo = enumerator.Current;
				if (friendInfo.FriendStatus == FriendStatus.Friend)
				{
					UITextConsole.TryAddToAutoComplete(friendInfo.FriendHandle);
				}
			}
		}
	}

	public static void AddToTeamMatesToAutoComplete(LobbyTeamInfo lobbyTeamInfo)
	{
		IEnumerator<LobbyPlayerInfo> enumerator = lobbyTeamInfo.TeamAPlayerInfo.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
				if (lobbyPlayerInfo.IsNPCBot)
				{
					if (!lobbyPlayerInfo.BotsMasqueradeAsHumans)
					{
						continue;
					}
				}
				UITextConsole.TryAddToAutoComplete(lobbyPlayerInfo.Handle);
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		IEnumerator<LobbyPlayerInfo> enumerator2 = lobbyTeamInfo.TeamBPlayerInfo.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				LobbyPlayerInfo lobbyPlayerInfo2 = enumerator2.Current;
				if (lobbyPlayerInfo2.IsNPCBot)
				{
					if (!lobbyPlayerInfo2.BotsMasqueradeAsHumans)
					{
						continue;
					}
				}
				UITextConsole.TryAddToAutoComplete(lobbyPlayerInfo2.Handle);
			}
		}
		finally
		{
			if (enumerator2 != null)
			{
				enumerator2.Dispose();
			}
		}
	}

	private static bool TryAddToAutoComplete(string handle)
	{
		if (!handle.IsNullOrEmpty())
		{
			if (handle == ClientGameManager.Get().Handle)
			{
			}
			else
			{
				int num = UITextConsole.m_playerAutocomplete.BinarySearch(handle, UITextConsole.m_caseInsenitiveComparer);
				if (num < 0)
				{
					UITextConsole.m_playerAutocomplete.Insert(~num, handle);
					return true;
				}
				if (num == 0)
				{
					if (UITextConsole.m_playerAutocomplete.Count != 0)
					{
						if (handle.EqualsIgnoreCase(UITextConsole.m_playerAutocomplete[0]))
						{
							return false;
						}
					}
					UITextConsole.m_playerAutocomplete.Insert(0, handle);
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private static void OnGroupUpdateNotification()
	{
		if (!ClientGameManager.Get().GroupInfo.InAGroup)
		{
			return;
		}
		List<UpdateGroupMemberData> members = ClientGameManager.Get().GroupInfo.Members;
		for (int i = 0; i < members.Count; i++)
		{
			UITextConsole.TryAddToAutoComplete(members[i].MemberHandle);
		}
	}

	public void AddHandleMessage()
	{
		if (TextConsole.Get() != null)
		{
			if (!this.m_handlingMessages)
			{
				this.m_handlingMessages = true;
				TextConsole.Get().OnMessage += this.HandleMessage;
			}
		}
	}

	public void RemoveHandleMessage()
	{
		if (TextConsole.Get() != null)
		{
			if (this.m_handlingMessages)
			{
				this.m_handlingMessages = false;
				TextConsole.Get().OnMessage -= this.HandleMessage;
			}
		}
	}

	private void OnEnable()
	{
		this.blockingRaycasts = false;
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			for (int i = 0; i < this.m_inGameCanvasGroups.Length; i++)
			{
				this.m_inGameCanvasGroups[i].blocksRaycasts = this.blockingRaycasts;
			}
			for (int j = 0; j < this.m_frontEndCanvasGroups.Length; j++)
			{
				this.m_frontEndCanvasGroups[j].blocksRaycasts = true;
			}
		}
		else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
		{
			for (int k = 0; k < this.m_inGameCanvasGroups.Length; k++)
			{
				this.m_inGameCanvasGroups[k].blocksRaycasts = true;
			}
			for (int l = 0; l < this.m_frontEndCanvasGroups.Length; l++)
			{
				this.m_frontEndCanvasGroups[l].blocksRaycasts = this.blockingRaycasts;
			}
		}
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			for (int m = 0; m < this.m_inGameCanvasGroups.Length; m++)
			{
				this.m_inGameCanvasGroups[m].blocksRaycasts = this.blockingRaycasts;
			}
			for (int n = 0; n < this.m_frontEndCanvasGroups.Length; n++)
			{
				this.m_frontEndCanvasGroups[n].blocksRaycasts = true;
			}
		}
		else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
		{
			for (int num = 0; num < this.m_inGameCanvasGroups.Length; num++)
			{
				this.m_inGameCanvasGroups[num].blocksRaycasts = true;
			}
			for (int num2 = 0; num2 < this.m_frontEndCanvasGroups.Length; num2++)
			{
				this.m_frontEndCanvasGroups[num2].blocksRaycasts = this.blockingRaycasts;
			}
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			
			clientGameManager.OnFriendStatusNotification -= new Action<FriendStatusNotification>(UITextConsole.HandleFriendStatusNotification);
			ClientGameManager clientGameManager2 = ClientGameManager.Get();
			
			clientGameManager2.OnGroupUpdateNotification -= new Action(UITextConsole.OnGroupUpdateNotification);
			ClientGameManager.Get().OnGameInfoNotification -= this.OnGameInfoNotification;
		}
	}

	public void SetupWhisper(string whisperee)
	{
		this.m_chatCommand = UITextConsole.m_whisperCommand.Command + " " + whisperee;
		this.RefreshChatRoomDisplay();
		this.SelectInput(string.Empty);
		this.MoveCaretToEnd();
	}

	public void DisplayMenu(string clickedHandle, float yOffset)
	{
		UITextConsoleMenu.Get().SetVisible(true);
		UITextConsoleMenu.Get().Setup(clickedHandle, false);
		UITextConsoleMenu.Get().SetToMousePosition();
	}

	public void DisplayIngameMenu(string clickedHandle, float yOffset)
	{
		UITextConsoleMenu.Get().SetVisible(true);
		UITextConsoleMenu.Get().Setup(clickedHandle, true);
		UITextConsoleMenu.Get().SetToMousePosition();
	}

	public void AppendInput(string stringToAdd, bool selectInput)
	{
		TMP_InputField textInput = this.m_textInput;
		textInput.text += stringToAdd;
		if (selectInput)
		{
			EventSystem.current.SetSelectedGameObject(this.m_textInput.gameObject);
		}
		this.MoveCaretToEnd();
	}

	private void OnTypeInput(string textString)
	{
		this.OnTypeInput(textString, true);
	}

	private void OnTypeInput(string textString, bool setChatRoom)
	{
		if (this.m_ignoreNextTypeInput)
		{
			this.m_ignoreNextTypeInput = false;
			return;
		}
		if (this.m_autocompleteList.IsVisible())
		{
			if (Input.GetKey(KeyCode.Backspace))
			{
				this.m_ignoreNextTypeInput = true;
				if (this.m_textInput.caretPosition > 0)
				{
					this.m_textInput.caretPosition--;
					this.m_textInput.text = this.m_textInput.text.Substring(0, this.m_textInput.caretPosition) + this.m_textInput.text.Substring(this.m_textInput.caretPosition + 1);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Delete))
			{
				this.m_autocompleteList.SetVisible(false);
				return;
			}
		}
		if (this.m_textInput.text.Length > 0xC8)
		{
			this.m_textInput.text = this.m_textInput.text.Substring(0, 0xC8);
		}
		if (!this.m_textInput.text.StartsWith(StringUtil.TR("/reply", "SlashCommand") + " "))
		{
			if (!this.m_textInput.text.StartsWith(StringUtil.TR("/reply", "SlashCommandAlias1") + " "))
			{
				goto IL_1E3;
			}
		}
		int num = this.m_textInput.text.IndexOf(' ');
		this.m_textInput.text = this.GenerateReplyPrefix() + this.m_textInput.text.Substring(num + 1);
		this.MoveCaretToEnd();
		IL_1E3:
		if (setChatRoom)
		{
			this.SetChatRoom(this.m_textInput.text);
		}
		if (!Input.GetKey(KeyCode.Delete))
		{
			string beforeAutocomplete;
			List<string> autoCompletePossibilities = this.GetAutoCompletePossibilities(false, out beforeAutocomplete);
			int length = Mathf.Clamp(this.m_textInput.caretPosition, 0, this.m_textInput.text.Length);
			if (autoCompletePossibilities.Count == 1)
			{
				if (this.m_textInput.text.Substring(0, length).EndsWith(autoCompletePossibilities[0].Trim()))
				{
					this.m_autocompleteList.SetVisible(false);
					return;
				}
			}
			this.m_autocompleteList.Setup(this, autoCompletePossibilities, beforeAutocomplete);
			this.m_autocompleteList.SetVisible(autoCompletePossibilities.Count > 0);
		}
	}

	public void OnEndEdit(string textString)
	{
		if (!Input.GetKeyDown(KeyCode.Return))
		{
			if (!Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				return;
			}
		}
		if (this.m_autocompleteList.IsVisible())
		{
			this.m_textInput.caretPosition = this.m_lastCaratPosition;
			this.m_autocompleteList.SelectCurrent();
			return;
		}
		this.m_lastCaratPosition = -1;
		this.OnInputSubmitted();
		bool flag;
		if (GameManager.Get() != null)
		{
			flag = (UIManager.Get().CurrentState == UIManager.ClientState.InGame);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2 && this.m_timeTillCollapse <= 0f)
		{
			this.ToggleVisibility();
		}
	}

	private void ApplyChatAlpha()
	{
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			if (this.m_inGameFadingImages != null)
			{
				for (int i = 0; i < this.m_inGameFadingImages.Length; i++)
				{
					if (this.m_inGameFadingImages[i] != null)
					{
						Color color = this.m_inGameFadingImages[i].color;
						color.a = this.chatAlpha;
						this.m_inGameFadingImages[i].color = color;
					}
				}
			}
			if (this.m_frontEndFadingImages != null)
			{
				for (int j = 0; j < this.m_frontEndFadingImages.Length; j++)
				{
					if (this.m_frontEndFadingImages[j] != null)
					{
						Color color2 = this.m_frontEndFadingImages[j].color;
						color2.a = 0f;
						this.m_frontEndFadingImages[j].color = color2;
						this.m_frontEndFadingImages[j].raycastTarget = false;
					}
				}
			}
		}
		else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
		{
			if (this.m_inGameFadingImages != null)
			{
				for (int k = 0; k < this.m_inGameFadingImages.Length; k++)
				{
					if (this.m_inGameFadingImages[k] != null)
					{
						Color color3 = this.m_inGameFadingImages[k].color;
						color3.a = 1f;
						this.m_inGameFadingImages[k].color = color3;
						this.m_frontEndFadingImages[k].raycastTarget = true;
					}
				}
			}
			if (this.m_frontEndFadingImages != null)
			{
				for (int l = 0; l < this.m_frontEndFadingImages.Length; l++)
				{
					if (this.m_frontEndFadingImages[l] != null)
					{
						Color color4 = this.m_frontEndFadingImages[l].color;
						color4.a = this.chatAlpha;
						this.m_frontEndFadingImages[l].color = color4;
					}
				}
			}
		}
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			for (int m = 0; m < this.m_inGameCanvasGroups.Length; m++)
			{
				this.m_inGameCanvasGroups[m].alpha = this.chatAlpha;
			}
			for (int n = 0; n < this.m_frontEndCanvasGroups.Length; n++)
			{
				this.m_frontEndCanvasGroups[n].alpha = 1f;
			}
		}
		else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
		{
			for (int num = 0; num < this.m_inGameCanvasGroups.Length; num++)
			{
				this.m_inGameCanvasGroups[num].alpha = 1f;
			}
			for (int num2 = 0; num2 < this.m_frontEndCanvasGroups.Length; num2++)
			{
				this.m_frontEndCanvasGroups[num2].alpha = this.chatAlpha;
			}
		}
		Color color5 = this.m_chatText.color;
		color5.a = this.chatAlpha / 2f + 0.5f;
		this.m_chatText.color = color5;
		this.m_theTextList.SetTextAlpha(this.chatAlpha);
		this.m_autocompleteList.m_canvasGroup.alpha = this.chatAlpha;
		this.lastAlphaSet = this.chatAlpha;
		if (!this.blockingRaycasts && this.chatAlpha > 0f)
		{
			this.blockingRaycasts = true;
			if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
			{
				for (int num3 = 0; num3 < this.m_frontEndCanvasGroups.Length; num3++)
				{
					this.m_frontEndCanvasGroups[num3].blocksRaycasts = true;
				}
				for (int num4 = 0; num4 < this.m_inGameCanvasGroups.Length; num4++)
				{
					this.m_inGameCanvasGroups[num4].blocksRaycasts = this.blockingRaycasts;
				}
			}
			else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				for (int num5 = 0; num5 < this.m_inGameCanvasGroups.Length; num5++)
				{
					this.m_inGameCanvasGroups[num5].blocksRaycasts = true;
				}
				for (int num6 = 0; num6 < this.m_frontEndCanvasGroups.Length; num6++)
				{
					this.m_frontEndCanvasGroups[num6].blocksRaycasts = this.blockingRaycasts;
				}
			}
		}
		else if (this.blockingRaycasts)
		{
			if (this.chatAlpha <= 0f)
			{
				this.blockingRaycasts = false;
				if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
				{
					for (int num7 = 0; num7 < this.m_frontEndCanvasGroups.Length; num7++)
					{
						this.m_frontEndCanvasGroups[num7].blocksRaycasts = true;
					}
					for (int num8 = 0; num8 < this.m_inGameCanvasGroups.Length; num8++)
					{
						this.m_inGameCanvasGroups[num8].blocksRaycasts = this.blockingRaycasts;
					}
				}
				else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
				{
					for (int num9 = 0; num9 < this.m_inGameCanvasGroups.Length; num9++)
					{
						this.m_inGameCanvasGroups[num9].blocksRaycasts = true;
					}
					for (int num10 = 0; num10 < this.m_frontEndCanvasGroups.Length; num10++)
					{
						this.m_frontEndCanvasGroups[num10].blocksRaycasts = this.blockingRaycasts;
					}
				}
			}
		}
		CanvasGroup component = this.m_scrollBar.GetComponent<CanvasGroup>();
		component.blocksRaycasts = this.blockingRaycasts;
		component.alpha = this.chatAlpha;
		if (this.m_changeChannelAlpha)
		{
			this.m_chatRoomName.color = new Color(this.m_chatRoomName.color.r, this.m_chatRoomName.color.g, this.m_chatRoomName.color.b, this.chatAlpha);
		}
	}

	public bool EscapeJustPressed()
	{
		return this.m_escapeJustPressed;
	}

	public bool InputJustcleared()
	{
		return this.m_inputJustCleared;
	}

	private void ClearInputSelect()
	{
		if (EventSystem.current != null)
		{
			if (EventSystem.current.currentSelectedGameObject == this.m_textInput.gameObject)
			{
				this.m_inputJustCleared = true;
				EventSystem.current.SetSelectedGameObject(null);
			}
		}
	}

	public void SelectInput(string startingInput = "")
	{
		if (!this.m_visible)
		{
			this.ToggleVisibility();
			this.setInputSelected = true;
		}
		else
		{
			if (UIScreenManager.Get() != null)
			{
				this.SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
			}
			this.UpdateElementsVisibility();
		}
		if (EventSystem.current.currentSelectedGameObject != this.m_textInput.gameObject)
		{
			EventSystem.current.SetSelectedGameObject(this.m_textInput.gameObject);
		}
		if (startingInput != string.Empty)
		{
			this.m_textInput.text = startingInput;
		}
	}

	public void SetTimeTillCollapse(float time)
	{
		if (!this.m_doNotAutoFade)
		{
			this.m_timeTillCollapse = time;
		}
	}

	private void LateUpdate()
	{
		if (this.m_visible)
		{
			UIManager.SetGameObjectActive(this.m_textInput.placeholder, EventSystem.current == null || EventSystem.current.currentSelectedGameObject != this.m_textInput.gameObject, null);
		}
		else
		{
			bool flag = false;
			if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
			{
				foreach (CanvasGroup canvasGroup in this.m_inGameCanvasGroups)
				{
					if (canvasGroup.gameObject == this.m_textInput.gameObject)
					{
						flag = true;
						goto IL_114;
					}
				}
			}
			else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				foreach (CanvasGroup canvasGroup2 in this.m_frontEndCanvasGroups)
				{
					if (canvasGroup2.gameObject == this.m_textInput.gameObject)
					{
						flag = true;
						goto IL_114;
					}
				}
			}
			IL_114:
			UIManager.SetGameObjectActive(this.m_textInput.placeholder, !flag, null);
		}
		this.m_textInput.placeholder.GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
		if (EventSystem.current.currentSelectedGameObject == this.m_textInput.gameObject)
		{
			if (this.m_updateCaret)
			{
				if (this.m_caretPositionToUpdate >= 0)
				{
					if (this.m_caretPositionToUpdate < this.m_textInput.text.Length)
					{
						if (this.m_textInput.caretPosition == this.m_caretPositionToUpdate)
						{
							if (this.m_textInput.selectionAnchorPosition == this.m_caretPositionToUpdate)
							{
								this.m_updateCaret = false;
								goto IL_21F;
							}
						}
						this.m_textInput.caretPosition = this.m_caretPositionToUpdate;
						this.m_textInput.selectionAnchorPosition = this.m_caretPositionToUpdate;
						IL_21F:
						goto IL_28C;
					}
				}
				if (this.m_textInput.caretPosition >= this.m_textInput.text.Length)
				{
					if (this.m_textInput.selectionAnchorPosition >= this.m_textInput.text.Length)
					{
						this.m_updateCaret = false;
						goto IL_28C;
					}
				}
				this.m_textInput.MoveTextEnd(false);
				IL_28C:;
			}
			else
			{
				this.m_lastCaratPosition = this.m_textInput.caretPosition;
			}
		}
		bool flag2 = false;
		if (this.m_visible)
		{
			if (Input.GetMouseButtonDown(0))
			{
				flag2 = true;
				if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
				{
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (!component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<UITextConsole>())
					{
						if (!component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<EmoticonPanel>())
						{
							goto IL_35E;
						}
					}
					flag2 = false;
				}
			}
		}
		IL_35E:
		if (!flag2)
		{
			if (this.m_timeTillCollapse > 0f || !this.m_visible)
			{
				return;
			}
			if (this.m_hovering)
			{
				return;
			}
			if (this.IsTextInputFocused(true))
			{
				return;
			}
		}
		if (!this.IsPressedAndMousedOver())
		{
			if (!EmoticonPanel.Get().IsPanelOpen())
			{
				this.ToggleVisibility();
			}
		}
	}

	private bool CheckInputField()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			TMP_InputField component = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
			if (component != null)
			{
				if (component != this.m_textInput)
				{
					string text = "ENTER INPUT FALSE: " + component.name;
					Transform transform = component.transform;
					while (transform.parent != null)
					{
						text = text + " -> " + transform.parent.name;
						transform = transform.parent;
					}
					Log.Info(text, new object[0]);
				}
				return component == this.m_textInput;
			}
		}
		return true;
	}

	private void Update()
	{
		bool flag;
		if (!Input.GetKeyDown(KeyCode.Return))
		{
			if (!Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				flag = false;
				goto IL_41;
			}
		}
		flag = this.CheckInputField();
		IL_41:
		bool flag2 = flag;
		bool flag3;
		if (Input.GetKeyDown(KeyCode.Slash))
		{
			if (EventSystem.current != null)
			{
				flag3 = (EventSystem.current.currentSelectedGameObject != this.m_textInput.gameObject);
				goto IL_95;
			}
		}
		flag3 = false;
		IL_95:
		bool flag4 = flag3;
		bool flag5;
		if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ChatReply))
		{
			if (DebugParameters.Get() != null)
			{
				flag5 = !DebugParameters.Get().GetParameterAsBool("DebugCamera");
			}
			else
			{
				flag5 = true;
			}
		}
		else
		{
			flag5 = false;
		}
		bool flag6 = flag5;
		bool flag7 = InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ChatAll);
		bool flag8;
		if (this.m_textInput.isFocused)
		{
			flag8 = Input.GetKeyDown(KeyCode.UpArrow);
		}
		else
		{
			flag8 = false;
		}
		bool flag9 = flag8;
		bool flag10 = this.m_textInput.isFocused && Input.GetKeyDown(KeyCode.DownArrow);
		bool flag11;
		if (this.m_textInput.isFocused)
		{
			if (!Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (!Input.GetKeyDown(KeyCode.LeftArrow))
				{
					if (!Input.GetKeyDown(KeyCode.Home))
					{
						flag11 = Input.GetKeyDown(KeyCode.End);
						goto IL_19D;
					}
				}
			}
			flag11 = true;
			IL_19D:;
		}
		else
		{
			flag11 = false;
		}
		bool flag12 = flag11;
		bool flag13;
		if (this.m_textInput.isFocused)
		{
			flag13 = Input.GetKeyDown(KeyCode.Tab);
		}
		else
		{
			flag13 = false;
		}
		bool flag14 = flag13;
		bool flag15 = this.m_autocompleteList.IsVisible();
		if (this.setInputSelected && this.chatAlpha > 0.5f)
		{
			if (EventSystem.current.currentSelectedGameObject != this.m_textInput.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(this.m_textInput.gameObject);
			}
			else
			{
				this.setInputSelected = false;
			}
		}
		this.m_escapeJustPressed = false;
		if (!this.InputJustcleared())
		{
			if (!flag2)
			{
				if (!flag4)
				{
					if (!flag6)
					{
						if (!flag7)
						{
							goto IL_3EA;
						}
					}
				}
			}
			if (!(EventSystem.current.currentSelectedGameObject == null))
			{
				if (!(EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
				{
					goto IL_3EA;
				}
				if (!(EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() == null))
				{
					goto IL_3EA;
				}
			}
			if (flag4)
			{
				this.SelectInput("/");
				EventSystem.current.SetSelectedGameObject(this.m_textInput.gameObject);
				this.MoveCaretToEnd();
			}
			else if (flag6)
			{
				this.SelectInput(this.GenerateReplyPrefix());
				this.MoveCaretToEnd();
			}
			else if (flag7)
			{
				if (GameFlowData.Get() == null)
				{
					this.SelectInput(StringUtil.TR("/general", "SlashCommand") + " ");
				}
				else
				{
					this.SelectInput(StringUtil.TR("/game", "SlashCommand") + " ");
				}
				this.MoveCaretToEnd();
			}
			else
			{
				this.SelectInput(string.Empty);
				this.SetCaretToLastKnownPosition();
			}
		}
		IL_3EA:
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.IsTextInputFocused(true))
			{
				if (this.m_autocompleteList.IsVisible())
				{
					this.m_autocompleteList.SetVisible(false);
				}
				else
				{
					this.ClearInputSelect();
					if (this.m_visible && this.m_timeTillCollapse <= 0f)
					{
						this.ToggleVisibility();
					}
					this.m_escapeJustPressed = true;
					UIUtils.MarkInputFieldHasFocusDirty();
				}
				goto IL_725;
			}
		}
		if (!flag10)
		{
			if (flag9)
			{
			}
			else
			{
				if (flag14)
				{
					if (flag15)
					{
						this.m_autocompleteList.SelectCurrent();
						goto IL_725;
					}
				}
				if (flag14)
				{
					if (this.m_textInput.text.StartsWith("/"))
					{
						string text;
						this.GetAutoCompletePossibilities(true, out text);
						goto IL_725;
					}
				}
				if (flag14)
				{
					if (!this.m_textInput.text.StartsWith("/"))
					{
						List<string> availableChatRooms = this.GetAvailableChatRooms();
						availableChatRooms.Add(availableChatRooms[0]);
						for (int i = 0; i < availableChatRooms.Count; i++)
						{
							if (i == availableChatRooms.Count - 1)
							{
								this.m_chatCommand = StringUtil.TR("/general", "SlashCommand");
								break;
							}
							if (this.m_chatCommand == availableChatRooms[i])
							{
								this.m_chatCommand = availableChatRooms[i + 1];
								break;
							}
						}
						this.RefreshChatRoomDisplay();
						goto IL_725;
					}
				}
				if (flag12)
				{
					this.m_autocompleteList.SetVisible(false);
					goto IL_725;
				}
				goto IL_725;
			}
		}
		if (flag15)
		{
			if (flag10)
			{
				this.m_autocompleteList.SelectDown();
			}
			else
			{
				this.m_autocompleteList.SelectUp();
			}
		}
		else
		{
			if (this.m_historyIndex >= UITextConsole.s_history.Count)
			{
				this.m_storedChatCommand = this.m_chatCommand;
				this.m_storedHistory = this.m_textInput.text;
			}
			int historyIndex = this.m_historyIndex;
			int num;
			if (flag10)
			{
				num = 1;
			}
			else
			{
				num = -1;
			}
			this.m_historyIndex = historyIndex + num;
			if (this.m_historyIndex < 0)
			{
				this.m_historyIndex = 0;
			}
			string text2;
			if (this.m_historyIndex >= UITextConsole.s_history.Count)
			{
				this.m_chatCommand = this.m_storedChatCommand;
				text2 = this.m_storedHistory;
				this.m_historyIndex = UITextConsole.s_history.Count;
				this.m_storedHistory = null;
				this.m_storedChatCommand = null;
			}
			else
			{
				text2 = UITextConsole.s_history[this.m_historyIndex];
			}
			this.m_ignoreNextTypeInput = true;
			if (text2.IsNullOrEmpty())
			{
				this.m_textInput.text = string.Empty;
			}
			else
			{
				this.m_textInput.text = text2;
				this.SetChatRoom(this.m_textInput.text);
			}
			this.MoveCaretToEnd();
		}
		IL_725:
		this.m_inputJustCleared = false;
		if (this.m_timeTillCollapse > 0f)
		{
			this.m_timeTillCollapse -= Time.deltaTime;
		}
		if (this.lastAlphaSet != this.chatAlpha)
		{
			this.ApplyChatAlpha();
		}
		this.ScrollBarAfterNewChat();
		if (this.m_newTextGlow != null)
		{
			if (this.m_checkForNextTextGlow)
			{
				if (this.m_scrollRect.verticalScrollbar.value > 0f)
				{
					if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
					{
						UIManager.SetGameObjectActive(this.m_newTextGlow, true, null);
						goto IL_7FB;
					}
				}
				UIManager.SetGameObjectActive(this.m_newTextGlow, false, null);
				this.m_checkForNextTextGlow = false;
			}
		}
		IL_7FB:
		if (this.m_visible && EventSystem.current.currentSelectedGameObject != null)
		{
			if (EventSystem.current.currentSelectedGameObject != this.m_textInput.gameObject && EventSystem.current.currentSelectedGameObject != this.m_scrollBar.gameObject && this.m_lastIsTextInputNotSelected)
			{
				if (!(EmoticonPanel.Get() == null))
				{
					if (EmoticonPanel.Get().IsPanelOpen())
					{
						goto IL_92F;
					}
					if (!(EmoticonPanel.Get().m_emoticonBtn.spriteController.gameObject != EventSystem.current.currentSelectedGameObject))
					{
						goto IL_92F;
					}
				}
				if ((double)this.m_scrollBar.size < 0.9999 && (double)this.m_scrollBar.value <= 0.0001)
				{
					this.ToggleVisibility();
				}
			}
		}
		IL_92F:
		bool lastIsTextInputNotSelected;
		if (EventSystem.current.currentSelectedGameObject != null)
		{
			lastIsTextInputNotSelected = (EventSystem.current.currentSelectedGameObject != this.m_textInput.gameObject);
		}
		else
		{
			lastIsTextInputNotSelected = false;
		}
		this.m_lastIsTextInputNotSelected = lastIsTextInputNotSelected;
		DateTime dateTime = ClientGameManager.Get().PacificNow();
		LobbyAlertMissionDataNotification alertMissionsData = ClientGameManager.Get().AlertMissionsData;
		if (alertMissionsData != null)
		{
			if (alertMissionsData.NextAlert != null)
			{
				float num2 = 0f;
				if (!this.m_hadNextAlertTime)
				{
					float num3 = alertMissionsData.ReminderHours.DefaultIfEmpty<float>().Max();
					if (num3 != 0f)
					{
						if (!(alertMissionsData.NextAlert.Value.AddHours((double)(-(double)num3)) <= dateTime))
						{
							goto IL_A59;
						}
					}
					num2 = (float)(alertMissionsData.NextAlert.Value - dateTime).TotalHours;
					IL_A59:;
				}
				else if (!alertMissionsData.ReminderHours.IsNullOrEmpty<float>())
				{
					for (int j = 0; j < alertMissionsData.ReminderHours.Count; j++)
					{
						DateTime t = alertMissionsData.NextAlert.Value.AddHours((double)(-(double)alertMissionsData.ReminderHours[j]));
						if (t > this.m_lastSystemMessageCheckPST)
						{
							if (t <= dateTime)
							{
								num2 = alertMissionsData.ReminderHours[j];
							}
						}
					}
				}
				if (num2 > 0f)
				{
					TextConsole.Get().Write(string.Format(StringUtil.TR("NextAlertIn", "Global"), StringUtil.GetTimeDifferenceText(TimeSpan.FromHours((double)num2), true)), ConsoleMessageType.SystemMessage);
				}
			}
		}
		bool hadNextAlertTime;
		if (alertMissionsData != null)
		{
			hadNextAlertTime = (alertMissionsData.NextAlert != null);
		}
		else
		{
			hadNextAlertTime = false;
		}
		this.m_hadNextAlertTime = hadNextAlertTime;
		if (alertMissionsData != null && alertMissionsData.CurrentAlert != null)
		{
			if (!this.m_hadCurrentAlert)
			{
				TextConsole.Get().Write(StringUtil.TR("AlertActive", "Global"), ConsoleMessageType.SystemMessage);
			}
		}
		bool hadCurrentAlert;
		if (alertMissionsData != null)
		{
			hadCurrentAlert = (alertMissionsData.CurrentAlert != null);
		}
		else
		{
			hadCurrentAlert = false;
		}
		this.m_hadCurrentAlert = hadCurrentAlert;
		this.m_lastSystemMessageCheckPST = dateTime;
	}

	public bool CheckTextInput()
	{
		bool result;
		if (this.m_textInput.isFocused)
		{
			result = (this.m_textInput.text == string.Empty);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsTextInputFocused(bool checkEmoticonPanel = false)
	{
		if (checkEmoticonPanel)
		{
			if (EmoticonPanel.Get() != null && EmoticonPanel.Get().IsPanelOpen())
			{
				return true;
			}
		}
		if (!(EventSystem.current == null))
		{
			if (!(EventSystem.current.currentSelectedGameObject == null))
			{
				bool result;
				if (!(this.m_textInput.gameObject == EventSystem.current.currentSelectedGameObject))
				{
					result = (this.m_scrollBar.gameObject == EventSystem.current.currentSelectedGameObject);
				}
				else
				{
					result = true;
				}
				return result;
			}
		}
		return false;
	}

	private bool IsPressedAndMousedOver()
	{
		if (EventSystem.current == null)
		{
			return false;
		}
		if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
		{
			return false;
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
		{
			pointerId = -1
		};
		pointerEventData.position = Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].gameObject.GetComponentInParent<UITextConsole>() != null)
			{
				return true;
			}
		}
		return false;
	}

	public List<string> GetAvailableChatRooms()
	{
		bool flag = AppState.IsInGame();
		bool flag2;
		if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
		{
			flag2 = (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		bool flag4;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().GroupInfo != null)
			{
				flag4 = ClientGameManager.Get().GroupInfo.InAGroup;
				goto IL_95;
			}
		}
		flag4 = false;
		IL_95:
		bool flag5 = flag4;
		List<string> list = new List<string>();
		if (!flag)
		{
			list.Add(StringUtil.TR("/general", "SlashCommand"));
		}
		if (!flag3)
		{
			if (!flag)
			{
				goto IL_152;
			}
		}
		if (!(GameManager.Get() == null))
		{
			if (GameManager.Get().PlayerInfo != null)
			{
				if (GameManager.Get().PlayerInfo.TeamId != Team.Spectator)
				{
					list.Add(StringUtil.TR("/team", "SlashCommand"));
					goto IL_152;
				}
			}
		}
		list.Add(StringUtil.TR("/team", "SlashCommandAlias1"));
		IL_152:
		if (flag3)
		{
			if (Options_UI.Get() != null)
			{
				if (Options_UI.Get().GetShowAllChat())
				{
					goto IL_1AA;
				}
			}
			if (flag || !GameManager.Get().GameInfo.IsCustomGame)
			{
				goto IL_1BF;
			}
			IL_1AA:
			list.Add(StringUtil.TR("/game", "SlashCommandAlias1"));
		}
		IL_1BF:
		if (flag5)
		{
			list.Add(StringUtil.TR("/group", "SlashCommand"));
		}
		for (int i = 0; i < UITextConsole.m_whisperedPlayers.Count; i++)
		{
			list.Add(StringUtil.TR("/whisper", "SlashCommand") + " " + UITextConsole.m_whisperedPlayers[i]);
		}
		return list;
	}

	public unsafe List<string> GetAutoCompletePossibilities(bool doAutocomplete, out string beforeAutocomplete)
	{
		List<string> list = new List<string>();
		List<string> list2 = null;
		string text = null;
		beforeAutocomplete = string.Empty;
		int num = Mathf.Clamp(this.m_textInput.caretPosition, 0, this.m_textInput.text.Length);
		string text2 = this.m_textInput.text.Substring(0, num).ToLower();
		string text3 = this.m_textInput.text.Substring(num);
		string[] array = text2.Split((string[])null, 2, StringSplitOptions.RemoveEmptyEntries);
		if (text2.EndsWith(" "))
		{
			if (array.Length < 2)
			{
				string[] array2 = new string[array.Length + 1];
				Array.Copy(array, 0, array2, 0, array.Length);
				array2[array.Length] = string.Empty;
				array = array2;
			}
		}
		if (array.Length == 1)
		{
			if (AppState.IsInGame())
			{
				list2 = UITextConsole.m_inGameAutocomplete;
			}
			else
			{
				list2 = UITextConsole.m_frontendAutocomplete;
			}
			text = array[0];
		}
		else if (array.Length == 2)
		{
			if (UITextConsole.m_friendCommand.IsSlashCommand(array[0]))
			{
				string[] array3 = array[1].Split((string[])null, 2, StringSplitOptions.RemoveEmptyEntries);
				if (text2.EndsWith(" "))
				{
					if (array3.Length < 2)
					{
						string[] array4 = new string[array3.Length + 1];
						Array.Copy(array3, 0, array4, 0, array3.Length);
						array4[array3.Length] = string.Empty;
						array3 = array4;
					}
				}
				if (array3.Length == 1)
				{
					list2 = UITextConsole.m_friendAutocomplete;
					text = array3[0];
					beforeAutocomplete = UITextConsole.m_friendCommand.Command;
				}
				else if (array3.Length == 2)
				{
					list2 = UITextConsole.m_playerAutocomplete;
					text = array3[1];
					beforeAutocomplete = UITextConsole.m_friendCommand.Command + " " + array3[0];
				}
			}
			else
			{
				for (int i = 0; i < UITextConsole.m_playerCommands.Count; i++)
				{
					if (UITextConsole.m_playerCommands[i].IsSlashCommand(array[0]))
					{
						list2 = UITextConsole.m_playerAutocomplete;
						text = array[1];
						beforeAutocomplete = UITextConsole.m_playerCommands[i].Command;
						break;
					}
				}
			}
		}
		if (!text.IsNullOrEmpty())
		{
			if (text.Length >= 1)
			{
				if (!(text == "/"))
				{
					if (list2 != null)
					{
						int j = 0;
						int num2 = list2.Count;
						while (j < num2)
						{
							int num3 = j + (num2 - j) / 2;
							int num4 = text.CompareTo(list2[num3].ToLower());
							if (num4 < 0)
							{
								num2 = num3;
							}
							else if (num4 > 0)
							{
								j = num3 + 1;
							}
							else
							{
								j = num3;
								num2 = num3;
							}
						}
						if (j < list2.Count)
						{
							if (list2[j].ToLower().StartsWith(text))
							{
								int num5 = j;
								int k = j + 1;
								while (k < list2.Count)
								{
									if (!list2[k].ToLower().StartsWith(text))
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											goto IL_399;
										}
									}
									else
									{
										num5 = k;
										k++;
									}
								}
								IL_399:
								string text4;
								if (num5 == j)
								{
									text4 = list2[j] + " ";
									if (text != text4)
									{
										if (UITextConsole.m_allCommand.IsSlashCommand(text4.Trim()))
										{
											if (Options_UI.Get() != null)
											{
												if (Options_UI.Get().GetShowAllChat())
												{
													goto IL_47D;
												}
											}
											if (AppState.IsInGame())
											{
												goto IL_485;
											}
											if (GameManager.Get().GameInfo == null)
											{
												goto IL_485;
											}
											if (!GameManager.Get().GameInfo.IsCustomGame)
											{
												goto IL_485;
											}
											if (GameManager.Get().GameInfo.GameStatus == GameStatus.Stopped)
											{
												goto IL_485;
											}
										}
										IL_47D:
										list.Add(text4);
									}
									IL_485:;
								}
								else
								{
									text4 = string.Empty;
									string text5 = list2[j].ToLower();
									string text6 = list2[num5].ToLower();
									for (int l = 0; l < text5.Length; l++)
									{
										if (l >= text6.Length)
										{
											break;
										}
										if (text5[l] != text6[l])
										{
											break;
										}
										text4 += text5[l];
									}
									int m = j;
									while (m <= num5)
									{
										if (!UITextConsole.m_allCommand.IsSlashCommand(list2[m].Trim()))
										{
											goto IL_5E4;
										}
										if (Options_UI.Get() != null)
										{
											if (Options_UI.Get().GetShowAllChat())
											{
												goto IL_5E4;
											}
										}
										if (!AppState.IsInGame() && GameManager.Get().GameInfo != null)
										{
											if (GameManager.Get().GameInfo.IsCustomGame)
											{
												if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
												{
													for (;;)
													{
														switch (2)
														{
														case 0:
															continue;
														}
														goto IL_5E4;
													}
												}
											}
										}
										IL_5F4:
										m++;
										continue;
										IL_5E4:
										list.Add(list2[m]);
										goto IL_5F4;
									}
								}
								if (doAutocomplete)
								{
									string text7 = text4 + text3.TrimStart(new char[0]);
									int num6 = (!text4.IsNullOrEmpty()) ? text4.Length : 0;
									if (!beforeAutocomplete.IsNullOrEmpty())
									{
										text7 = beforeAutocomplete + " " + text7;
										int num7 = num6;
										int num8;
										if (beforeAutocomplete.IsNullOrEmpty())
										{
											num8 = 0;
										}
										else
										{
											num8 = beforeAutocomplete.Length;
										}
										num6 = num7 + (num8 + 1);
									}
									if (this.m_textInput.text != text7)
									{
										this.m_textInput.text = text7;
										this.UpdateCaretPosition(num6);
										this.OnTypeInput(this.m_textInput.text, false);
									}
								}
							}
						}
					}
					return list;
				}
			}
		}
		return list;
	}

	public void ScrollBarAfterNewChat()
	{
		if (this.m_scrollChat)
		{
			if (this.m_scrollRect.verticalScrollbar.value != 0f)
			{
				this.m_scrollRect.verticalScrollbar.value = 0f;
				this.m_scrollChat = false;
			}
		}
	}

	public TextMeshProUGUI AddTextEntry(string textEntry, Color textColor, bool forceShowChat, TextConsole.Message messageInfo, List<int> allowedEmojis = null)
	{
		CharacterType characterType = CharacterType.None;
		if (messageInfo.MessageType == ConsoleMessageType.GlobalChat)
		{
			if (!(GameFlowData.Get() != null))
			{
				if (Options_UI.Get() != null)
				{
					if (!Options_UI.Get().GetShowGlobalChat())
					{
						goto IL_84;
					}
				}
				if (!(AppState.GetCurrent() == AppState_RankModeDraft.Get()))
				{
					goto IL_86;
				}
			}
			IL_84:
			return null;
		}
		IL_86:
		if (GameFlowData.Get() != null)
		{
			characterType = messageInfo.CharacterType;
		}
		RectTransform rectTransform = this.m_theTextList.transform as RectTransform;
		float value = this.m_scrollRect.verticalScrollbar.value;
		float size = this.m_scrollRect.verticalScrollbar.size;
		float y = rectTransform.sizeDelta.y;
		if (UIManager.Get().CurrentState != UIManager.ClientState.InFrontEnd)
		{
			if (characterType != CharacterType.None)
			{
				if (messageInfo.SenderTeam != Team.Spectator)
				{
					int num = 0;
					if (GameFlowData.Get() != null)
					{
						Team team = Team.TeamA;
						if (GameFlowData.Get().activeOwnedActorData != null)
						{
							team = GameFlowData.Get().activeOwnedActorData.GetTeam();
						}
						else if (ClientGameManager.Get().PlayerInfo != null)
						{
							team = ClientGameManager.Get().PlayerInfo.TeamId;
						}
						if (team == Team.Spectator)
						{
							if (messageInfo.SenderTeam == Team.TeamA)
							{
								num = 0;
							}
							else
							{
								num = 1;
							}
						}
						else if (messageInfo.SenderTeam == team)
						{
							num = 0;
						}
						else
						{
							num = 1;
						}
					}
					string text = string.Format("<size=36><sprite=\"CharacterSprites\" index={0}>​</size>{1}", 2 * (int)characterType + num, textEntry);
					textEntry = text;
				}
			}
		}
		TextMeshProUGUI textMeshProUGUI = this.m_theTextList.AddEntry(textEntry, textColor, forceShowChat, HUD_UIResources.Get().m_textPaddingAmount, this.m_scrollRect, allowedEmojis);
		textMeshProUGUI.CalculateLayoutInputVertical();
		float num2 = textMeshProUGUI.preferredHeight + HUD_UIResources.Get().m_textPaddingAmount;
		bool scrollChat;
		if (this.m_theTextList.NumEntires() >= 2)
		{
			if (num2 < y * value)
			{
				scrollChat = (size >= 0.999f);
				goto IL_2A1;
			}
		}
		scrollChat = true;
		IL_2A1:
		this.m_scrollChat = scrollChat;
		if (!this.m_scrollChat)
		{
			Vector3 localPosition = rectTransform.localPosition;
			localPosition.y -= num2 * (1f - rectTransform.pivot.y);
			if (localPosition.y < -rectTransform.sizeDelta.y)
			{
				localPosition.y = -rectTransform.sizeDelta.y;
			}
			rectTransform.localPosition = localPosition;
		}
		UIEventTriggerUtils.AddListener(textMeshProUGUI.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		return textMeshProUGUI;
	}

	public TextMeshProUGUI AddTextEntry(string textEntry, Color textColor, bool forceShowChat)
	{
		RectTransform rectTransform = this.m_theTextList.transform as RectTransform;
		float value = this.m_scrollRect.verticalScrollbar.value;
		float size = this.m_scrollRect.verticalScrollbar.size;
		float y = rectTransform.sizeDelta.y;
		TextMeshProUGUI textMeshProUGUI = this.m_theTextList.AddEntry(textEntry, textColor, forceShowChat, HUD_UIResources.Get().m_textPaddingAmount, this.m_scrollRect, null);
		HUDTextConsoleItem component = textMeshProUGUI.GetComponent<HUDTextConsoleItem>();
		if (component != null)
		{
			UIManager.SetGameObjectActive(component.m_iconContainer, false, null);
		}
		this.m_checkForNextTextGlow = true;
		textMeshProUGUI.CalculateLayoutInputVertical();
		float num = textMeshProUGUI.preferredHeight + HUD_UIResources.Get().m_textPaddingAmount;
		bool scrollChat;
		if (this.m_theTextList.NumEntires() >= 2)
		{
			if (num < y * value)
			{
				scrollChat = (size >= 0.999f);
				goto IL_104;
			}
		}
		scrollChat = true;
		IL_104:
		this.m_scrollChat = scrollChat;
		if (!this.m_scrollChat)
		{
			Vector3 localPosition = rectTransform.localPosition;
			localPosition.y -= num * (1f - rectTransform.pivot.y);
			if (localPosition.y < -rectTransform.sizeDelta.y)
			{
				localPosition.y = -rectTransform.sizeDelta.y;
			}
			rectTransform.localPosition = localPosition;
		}
		UIEventTriggerUtils.AddListener(textMeshProUGUI.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		return textMeshProUGUI;
	}

	private string FormatConsoleMessage(TextConsole.Message message, bool selfMessage)
	{
		string result = string.Empty;
		if (message.DisplayDevTag)
		{
			message.SenderHandle = StringUtil.TR("DevTag", "Global") + message.SenderHandle;
		}
		switch (message.MessageType)
		{
		case ConsoleMessageType.GlobalChat:
		{
			string text = this.ColorToHex(HUD_UIResources.Get().m_GlobalChatColor);
			string text2 = string.Concat(new string[]
			{
				"<link=channel:",
				StringUtil.TR("/general", "SlashCommand").Substring(1),
				">",
				StringUtil.TR("GlobalChannel", "Chat"),
				"</link>"
			});
			if (selfMessage)
			{
				result = string.Format("<color=#{0}>{1} {2}:  {3}</color>", new object[]
				{
					text,
					text2,
					message.SenderHandle,
					message.Text
				});
			}
			else
			{
				result = string.Format("<color=#{0}>{1} [<link=name>{2}</link>]:  {3}</color>", new object[]
				{
					text,
					text2,
					message.SenderHandle,
					message.Text
				});
			}
			break;
		}
		case ConsoleMessageType.GameChat:
		{
			string arg = this.ColorToHex(HUD_UIResources.Get().m_GameChatColor);
			if (selfMessage)
			{
				result = string.Format("<color=#{0}>" + StringUtil.TR("GameChannel", "Chat") + " </color>{1}<color=#{0}>: {2}</color>", arg, message.SenderHandle, message.Text);
			}
			else
			{
				result = string.Format("<color=#{0}>" + StringUtil.TR("GameChannel", "Chat") + " <link=name>{1}</link>: {2}</color>", arg, message.SenderHandle, message.Text);
			}
			break;
		}
		case ConsoleMessageType.TeamChat:
		{
			string text = this.ColorToHex(HUD_UIResources.Get().m_TeamChatColor);
			string text2;
			if (message.SenderTeam == Team.Spectator)
			{
				text2 = string.Concat(new string[]
				{
					"<link=channel:",
					StringUtil.TR("/team", "SlashCommand").Substring(1),
					">",
					StringUtil.TR("SpectatorChannel", "Chat"),
					"</link>"
				});
			}
			else
			{
				text2 = string.Concat(new string[]
				{
					"<link=channel:",
					StringUtil.TR("/team", "SlashCommand").Substring(1),
					">",
					StringUtil.TR("TeamChannel", "Chat"),
					"</link>"
				});
			}
			if (message.SenderHandle.IsNullOrEmpty())
			{
				result = string.Format("<color=#{0}>{1}:  {2}</color>", text, text2, message.Text);
			}
			else if (selfMessage)
			{
				result = string.Format("<color=#{0}>{1} </color>{2}<color=#{0}>:  {3}</color>", new object[]
				{
					text,
					text2,
					message.SenderHandle,
					message.Text
				});
			}
			else
			{
				result = string.Format("<color=#{0}>{1} [<link=name>{2}</link>]:  {3}</color>", new object[]
				{
					text,
					text2,
					message.SenderHandle,
					message.Text
				});
			}
			break;
		}
		case ConsoleMessageType.GroupChat:
		{
			string text = this.ColorToHex(HUD_UIResources.Get().m_GroupChatColor);
			string text2 = string.Concat(new string[]
			{
				"<link=channel:",
				StringUtil.TR("/group", "SlashCommand").Substring(1),
				">",
				StringUtil.TR("GroupChannel", "Chat"),
				"</link>"
			});
			string text3;
			if (message.SenderHandle.IsNullOrEmpty())
			{
				text3 = string.Empty;
			}
			else if (selfMessage)
			{
				text3 = message.SenderHandle;
			}
			else
			{
				text3 = " [<link=name>" + message.SenderHandle + "</link>]";
			}
			if (selfMessage)
			{
				result = string.Format("<color=#{0}>{1} </color>{2}<color=#{0}>:  {3}</color>", new object[]
				{
					text,
					text2,
					text3,
					message.Text
				});
			}
			else
			{
				result = string.Format("<color=#{0}>{1}{2}:  {3}</color>", new object[]
				{
					text,
					text2,
					text3,
					message.Text
				});
			}
			break;
		}
		case ConsoleMessageType.WhisperChat:
		{
			string text4;
			string text5;
			if (selfMessage)
			{
				text4 = message.RecipientHandle;
				text5 = StringUtil.TR("To", "Chat");
			}
			else
			{
				text4 = message.SenderHandle;
				text5 = string.Empty;
			}
			string text = this.ColorToHex(HUD_UIResources.Get().m_whisperChatColor);
			result = string.Format("<color=#{0}>{1} [<link=name>{2}</link>]:  {3}</color>", new object[]
			{
				text,
				text5,
				text4,
				message.Text
			});
			break;
		}
		case ConsoleMessageType.CombatLog:
		{
			string text = this.ColorToHex(HUD_UIResources.Get().m_combatLogChatColor);
			result = string.Format("<color=#{1}>{0}</color>", message.Text, text);
			break;
		}
		case ConsoleMessageType.SystemMessage:
		case ConsoleMessageType.Exception:
		case ConsoleMessageType.BroadcastMessage:
		{
			string text = this.ColorToHex(HUD_UIResources.Get().m_systemChatColor);
			result = string.Format("<color=#{1}>{0}</color>", message.Text, text);
			break;
		}
		case ConsoleMessageType.Error:
		{
			string text = this.ColorToHex(HUD_UIResources.Get().m_systemErrorChatColor);
			result = string.Format("<color=#{1}>{0}</color>", message.Text, text);
			break;
		}
		case ConsoleMessageType.PingChat:
		case ConsoleMessageType.ScriptedChat:
		{
			string text = this.ColorToHex(HUD_UIResources.Get().m_TeamChatColor);
			result = string.Format("<color=#{1}>{0}</color>", message.Text, text);
			break;
		}
		case ConsoleMessageType.symbol_001D:
		{
			string text = this.ColorToHex(Color.yellow);
			result = string.Format("<color=#{1}>{0}</color>", message.Text, text);
			break;
		}
		default:
			result = message.Text;
			break;
		}
		return result;
	}

	private static bool ShouldDisplay(TextConsole.Message message)
	{
		if (HUD_UI.Get() != null && message.MessageType == ConsoleMessageType.GlobalChat)
		{
			return false;
		}
		if (message.MessageType == ConsoleMessageType.GameChat)
		{
			if (!(Options_UI.Get() == null))
			{
				if (Options_UI.Get().GetShowAllChat())
				{
					return true;
				}
			}
			if (!AppState.IsInGame() && GameManager.Get().GameInfo != null)
			{
				if (GameManager.Get().GameInfo.IsCustomGame)
				{
					if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
					{
						return true;
					}
				}
			}
			return false;
		}
		return true;
	}

	public void UpdateGameState()
	{
		this.ApplyChatAlpha();
		this.ChangeChatRoom();
		this.m_theTextList.RefreshTextSizes();
	}

	public static void StoreMessage(TextConsole.Message message, TextConsole.AllowedEmojis allowedEmojis)
	{
		if (UITextConsole.ShouldDisplay(message))
		{
			if (UITextConsole.s_handledMessages != null)
			{
				UITextConsole.s_handledMessages.Enqueue(new UITextConsole.HandledMessage
				{
					Message = message,
					AllowedEmojis = allowedEmojis
				});
				if (UITextConsole.s_handledMessages.Count > 0x50)
				{
					UITextConsole.s_handledMessages.Dequeue();
				}
			}
		}
	}

	public void HandleMessage(TextConsole.Message message, TextConsole.AllowedEmojis allowedEmojis)
	{
		if (UITextConsole.ShouldDisplay(message))
		{
			this.DisplayMessage(message, allowedEmojis);
		}
	}

	private TextMeshProUGUI DisplayMessage(TextConsole.Message message, TextConsole.AllowedEmojis allowedEmojis)
	{
		if (HUD_UIResources.Get() == null)
		{
			return null;
		}
		string text = string.Empty;
		bool forceShowChat = false;
		bool flag = false;
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		if (playerAccountData != null)
		{
			flag = (message.SenderAccountId == playerAccountData.AccountId);
		}
		Color textColor;
		switch (message.MessageType)
		{
		case ConsoleMessageType.GlobalChat:
			textColor = HUD_UIResources.Get().m_GlobalChatColor;
			forceShowChat = true;
			goto IL_239;
		case ConsoleMessageType.GameChat:
			textColor = HUD_UIResources.Get().m_GameChatColor;
			forceShowChat = true;
			goto IL_239;
		case ConsoleMessageType.TeamChat:
			textColor = HUD_UIResources.Get().m_TeamChatColor;
			forceShowChat = true;
			goto IL_239;
		case ConsoleMessageType.GroupChat:
			textColor = HUD_UIResources.Get().m_GroupChatColor;
			forceShowChat = true;
			goto IL_239;
		case ConsoleMessageType.WhisperChat:
		{
			textColor = HUD_UIResources.Get().m_whisperChatColor;
			forceShowChat = true;
			if (this.m_timeSinceLastWhisper + UIFrontEnd.Get().m_whisperSoundThreshold >= Time.time)
			{
				if (flag)
				{
					if (this.m_lastWhisperer != message.RecipientHandle)
					{
						goto IL_168;
					}
				}
				if (flag)
				{
					goto IL_1CD;
				}
				if (!(this.m_lastWhisperer != message.SenderHandle))
				{
					goto IL_1CD;
				}
			}
			IL_168:
			string lastWhisperer;
			if (flag)
			{
				lastWhisperer = message.RecipientHandle;
			}
			else
			{
				lastWhisperer = message.SenderHandle;
			}
			this.m_lastWhisperer = lastWhisperer;
			if (!flag)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.WhisperMessage);
			}
			if (!UITextConsole.m_whisperedPlayers.Contains(this.m_lastWhisperer))
			{
				UITextConsole.m_whisperedPlayers.Add(this.m_lastWhisperer);
			}
			IL_1CD:
			this.m_timeSinceLastWhisper = Time.time;
			goto IL_239;
		}
		case ConsoleMessageType.SystemMessage:
			textColor = HUD_UIResources.Get().m_GlobalChatColor;
			forceShowChat = true;
			goto IL_239;
		case ConsoleMessageType.Error:
			text = message.Text;
			textColor = Color.red;
			forceShowChat = true;
			goto IL_239;
		case ConsoleMessageType.Exception:
			text = message.Text;
			textColor = Color.red;
			forceShowChat = true;
			goto IL_239;
		case ConsoleMessageType.BroadcastMessage:
			textColor = HUD_UIResources.Get().m_GlobalChatColor;
			forceShowChat = true;
			goto IL_239;
		}
		text = message.Text;
		textColor = HUD_UIResources.Get().m_GlobalChatColor;
		IL_239:
		if (text.Length > 0x100)
		{
			if (message.MessageType != ConsoleMessageType.SystemMessage)
			{
				text = text.Substring(0, 0x100);
			}
		}
		text = this.FormatConsoleMessage(message, flag);
		TextMeshProUGUI result = this.AddTextEntry(text, textColor, forceShowChat, message, allowedEmojis.emojis);
		UITextConsole.TryAddToAutoComplete(message.SenderHandle);
		return result;
	}

	public bool IsVisible()
	{
		return this.m_visible;
	}

	private void UpdateElementsVisibility()
	{
		if (this.m_visible)
		{
			this.chatAlpha.EaseTo(1f, 0.3f);
			if (this.m_changeChannelAlpha)
			{
				UIManager.SetGameObjectActive(this.m_chatroomHitbox, true, null);
			}
		}
		else
		{
			this.chatAlpha.EaseTo(0f, 0.3f);
			if (this.m_changeChannelAlpha)
			{
				UIManager.SetGameObjectActive(this.m_chatroomHitbox, false, null);
			}
			if (EmoticonPanel.Get() != null)
			{
				EmoticonPanel.Get().SetPanelOpen(false);
			}
		}
	}

	public void OnInputSubmitted()
	{
		string text = this.m_textInput.text.Trim();
		if (!text.IsNullOrEmpty())
		{
			if (!text.StartsWith("/"))
			{
				text = this.m_chatCommand + " " + text;
			}
			bool flag;
			if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
			{
				flag = (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			bool flag3 = false;
			string[] array = text.Split(new char[]
			{
				' '
			});
			if (UITextConsole.m_globalCommand.IsSlashCommand(array[0]))
			{
				if (Options_UI.Get() != null && !Options_UI.Get().GetShowGlobalChat())
				{
					this.AddTextEntry(StringUtil.TR("GlobalChatDisabled", "Chat"), HUD_UIResources.Get().m_GlobalChatColor, true);
					flag3 = true;
					goto IL_2EA;
				}
			}
			if (UITextConsole.m_allCommand.IsSlashCommand(array[0]) && Options_UI.Get() != null)
			{
				if (!Options_UI.Get().GetShowAllChat())
				{
					if (!AppState.IsInGame())
					{
						if (GameManager.Get().GameInfo != null)
						{
							if (GameManager.Get().GameInfo.IsCustomGame)
							{
								if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
								{
									goto IL_1FC;
								}
							}
						}
					}
					this.AddTextEntry(StringUtil.TR("AllChatDisabled", "Chat"), HUD_UIResources.Get().m_GameChatColor, true);
					flag3 = true;
					IL_1FC:
					goto IL_2EA;
				}
			}
			if (UITextConsole.m_teamCommand.IsSlashCommand(array[0]))
			{
				if (!flag2)
				{
					this.AddTextEntry(StringUtil.TR("NotInAGame", "Invite"), HUD_UIResources.Get().m_TeamChatColor, true);
					flag3 = true;
					goto IL_2EA;
				}
			}
			if (UITextConsole.m_groupCommand.IsSlashCommand(array[0]))
			{
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().GroupInfo != null)
					{
						if (!ClientGameManager.Get().GroupInfo.InAGroup)
						{
							this.AddTextEntry(StringUtil.TR("NotInAGroup", "Invite"), HUD_UIResources.Get().m_GroupChatColor, true);
							flag3 = true;
						}
					}
				}
			}
			IL_2EA:
			if (!flag3)
			{
				if (TextConsole.Get() != null)
				{
					TextConsole.Get().OnInputSubmitted(text);
				}
				UITextConsole.s_history.Add(text);
				this.m_storedHistory = null;
				this.m_storedChatCommand = null;
				this.m_historyIndex = UITextConsole.s_history.Count;
				for (int i = 0; i < UITextConsole.m_possibilitiesEntries.Count; i++)
				{
					this.m_theTextList.RemoveEntry(UITextConsole.m_possibilitiesEntries[i]);
				}
				UITextConsole.m_possibilitiesEntries.Clear();
			}
		}
		this.m_textInput.text = string.Empty;
		if (EventSystem.current.currentSelectedGameObject == this.m_textInput.gameObject)
		{
			this.m_inputJustCleared = true;
			EventSystem.current.SetSelectedGameObject(null);
			this.setInputSelected = (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd);
		}
	}

	private void OnToggleClicked(BaseEventData data)
	{
		this.ToggleVisibility();
	}

	private void ToggleVisibility()
	{
		if (this.m_visible)
		{
			if (this.m_chatroomList.IsVisible())
			{
				return;
			}
		}
		if (this.m_visible)
		{
			if ((double)this.m_scrollBar.size < 0.9999)
			{
				if ((double)this.m_scrollBar.value > 0.0001)
				{
					return;
				}
			}
		}
		this.m_visible = !this.m_visible;
		if (this.m_visible)
		{
			if (UIScreenManager.Get() != null)
			{
				this.SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
			}
		}
		else
		{
			this.ClearInputSelect();
			this.SetTimeTillCollapse(-1f);
		}
		this.m_theTextList.NotifyVisible(this.m_visible);
		this.UpdateElementsVisibility();
	}

	public void Show()
	{
		if (!this.m_visible)
		{
			this.ToggleVisibility();
		}
	}

	public void Hide()
	{
		if (this.m_visible)
		{
			this.ToggleVisibility();
		}
	}

	private void OnPointerEnter(BaseEventData data)
	{
		this.OnTextConsoleHover(true);
	}

	private void OnPointerExit(BaseEventData data)
	{
		this.OnTextConsoleHover(false);
	}

	public bool IsHovered()
	{
		return this.m_hovering;
	}

	private void OnInputClicked(BaseEventData data)
	{
		this.Show();
		this.m_autocompleteList.SetVisible(false);
	}

	private void OnChatroomClick(BaseEventData data)
	{
		if (this.m_chatroomList.IsVisible())
		{
			this.m_chatroomList.SetVisible(false);
		}
		else
		{
			this.m_chatroomList.Setup(this);
			this.m_chatroomList.SetVisible(true);
		}
	}

	private void OnTextConsoleHover(bool hover)
	{
		this.m_hovering = hover;
		if (!hover)
		{
			if (UIScreenManager.Get() != null)
			{
				this.SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
			}
		}
	}

	private void OnDragOver(GameObject go)
	{
		this.m_hovering = true;
	}

	private void OnDragOut(GameObject go)
	{
		this.m_hovering = false;
		if (!this.m_hovering)
		{
			this.SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
		}
	}

	private string GenerateReplyPrefix()
	{
		TextConsole textConsole = TextConsole.Get();
		if (textConsole != null)
		{
			string lastWhisperSenderHandle = textConsole.LastWhisperSenderHandle;
			if (!lastWhisperSenderHandle.IsNullOrEmpty())
			{
				return StringUtil.TR("/whisper", "SlashCommandAlias1") + " " + lastWhisperSenderHandle + " ";
			}
		}
		return StringUtil.TR("/whisper", "SlashCommandAlias1") + " ";
	}

	public void ChangeChatRoom()
	{
		if (!(this.m_chatRoomName == null))
		{
			if (this.m_textInput.text.IsNullOrEmpty())
			{
				this.m_chatroomList.SetVisible(false);
				this.m_hasGame = (GameManager.Get() != null && GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
				bool flag;
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().GroupInfo != null)
					{
						flag = ClientGameManager.Get().GroupInfo.InAGroup;
						goto IL_D1;
					}
				}
				flag = false;
				IL_D1:
				bool flag2 = flag;
				if (!this.m_hasGame)
				{
					if (UITextConsole.m_teamCommand.IsSlashCommand(this.m_chatCommand))
					{
						goto IL_190;
					}
					if (UITextConsole.m_allCommand.IsSlashCommand(this.m_chatCommand))
					{
						goto IL_190;
					}
				}
				if (UITextConsole.m_globalCommand.IsSlashCommand(this.m_chatCommand))
				{
					if (flag2)
					{
						goto IL_190;
					}
					if (this.m_hasGame)
					{
						goto IL_190;
					}
				}
				if (!UITextConsole.m_groupCommand.IsSlashCommand(this.m_chatCommand))
				{
					goto IL_197;
				}
				if (flag2)
				{
					if (!this.m_hasGame)
					{
						goto IL_197;
					}
				}
				IL_190:
				this.m_chatCommand = null;
				IL_197:
				this.SetChatRoom(null);
				return;
			}
		}
	}

	private void SetChatRoom(string chatText = null)
	{
		if (this.m_chatRoomName == null)
		{
			return;
		}
		if (chatText == null)
		{
			chatText = this.m_chatText.text;
		}
		bool flag;
		if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
		{
			flag = (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		bool flag3;
		if (ClientGameManager.Get() != null && ClientGameManager.Get().GroupInfo != null)
		{
			flag3 = ClientGameManager.Get().GroupInfo.InAGroup;
		}
		else
		{
			flag3 = false;
		}
		bool flag4 = flag3;
		string[] array = chatText.Split((string[])null, 3, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length <= 1)
		{
			if (!chatText.EndsWith(" "))
			{
				goto IL_445;
			}
			if (chatText.Trim().IsNullOrEmpty())
			{
				goto IL_445;
			}
		}
		using (List<SlashCommand>.Enumerator enumerator = UITextConsole.m_chatCommands.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SlashCommand slashCommand = enumerator.Current;
				if (slashCommand.IsSlashCommand(array[0]))
				{
					if (slashCommand == UITextConsole.m_whisperCommand)
					{
						if (array.Length < 3)
						{
							if (array.Length < 2)
							{
								goto IL_1D3;
							}
							if (!chatText.EndsWith(" "))
							{
								goto IL_1D3;
							}
						}
						array = chatText.Split((string[])null, 3, StringSplitOptions.RemoveEmptyEntries);
						this.m_chatCommand = slashCommand.Command + " " + array[1];
						if (array.Length >= 3)
						{
							this.m_textInput.text = array[2];
						}
						else
						{
							this.m_textInput.text = string.Empty;
						}
						IL_1D3:;
					}
					else
					{
						if (slashCommand == UITextConsole.m_allCommand)
						{
							if (!AppState.IsInGame() && GameManager.Get().GameInfo != null)
							{
								if (GameManager.Get().GameInfo.IsCustomGame && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
								{
									goto IL_26A;
								}
							}
							if (!(Options_UI.Get() == null))
							{
								if (Options_UI.Get().GetShowAllChat())
								{
									goto IL_26A;
								}
							}
							goto IL_435;
						}
						IL_26A:
						if (!flag2)
						{
							if (slashCommand != UITextConsole.m_teamCommand)
							{
								if (slashCommand != UITextConsole.m_allCommand)
								{
									goto IL_323;
								}
							}
							this.AddTextEntry(StringUtil.TR("NotInAGame", "Invite"), HUD_UIResources.Get().m_TeamChatColor, true);
							chatText = string.Empty;
							for (int i = 1; i < array.Length; i++)
							{
								if (chatText.IsNullOrEmpty())
								{
									chatText = array[i];
								}
								else
								{
									chatText = chatText + " " + array[i];
								}
							}
							this.m_textInput.text = chatText;
							goto IL_435;
						}
						IL_323:
						if (slashCommand == UITextConsole.m_groupCommand)
						{
							if (!flag4)
							{
								this.AddTextEntry(StringUtil.TR("NotInAGroup", "Invite"), HUD_UIResources.Get().m_GroupChatColor, true);
								chatText = string.Empty;
								for (int j = 1; j < array.Length; j++)
								{
									if (chatText.IsNullOrEmpty())
									{
										chatText = array[j];
									}
									else
									{
										chatText = chatText + " " + array[j];
									}
								}
								this.m_textInput.text = chatText;
								goto IL_435;
							}
						}
						this.m_chatCommand = slashCommand.Command;
						if (array.Length > 1)
						{
							chatText = array[1];
							if (array.Length > 2)
							{
								chatText = chatText + " " + array[2];
							}
							this.m_textInput.text = chatText;
						}
						else
						{
							this.m_textInput.text = string.Empty;
						}
					}
					IL_435:
					goto IL_445;
				}
			}
		}
		IL_445:
		if (this.m_chatCommand.IsNullOrEmpty())
		{
			if (this.m_hasGame)
			{
				this.m_chatCommand = StringUtil.TR("/team", "SlashCommand");
			}
			else if (flag4)
			{
				this.m_chatCommand = StringUtil.TR("/group", "SlashCommand");
			}
			else
			{
				this.m_chatCommand = StringUtil.TR("/general", "SlashCommand");
			}
		}
		this.RefreshChatRoomDisplay();
	}

	private void RefreshChatRoomDisplay()
	{
		string text = this.m_chatCommand;
		int num = this.m_chatCommand.IndexOf(' ');
		if (num > 0)
		{
			text = this.m_chatCommand.Substring(0, num);
		}
		if (UITextConsole.m_allCommand.IsSlashCommand(text))
		{
			this.m_chatRoomName.text = StringUtil.TR("GameChannel", "Chat");
		}
		else if (UITextConsole.m_globalCommand.IsSlashCommand(text))
		{
			this.m_chatRoomName.text = StringUtil.TR("GlobalChannel", "Chat");
		}
		else if (UITextConsole.m_groupCommand.IsSlashCommand(text))
		{
			this.m_chatRoomName.text = StringUtil.TR("GroupChannel", "Chat");
		}
		else if (UITextConsole.m_teamCommand.IsSlashCommand(text))
		{
			if (!(GameManager.Get() == null))
			{
				if (GameManager.Get().PlayerInfo != null)
				{
					if (GameManager.Get().PlayerInfo.TeamId != Team.Spectator)
					{
						this.m_chatRoomName.text = StringUtil.TR("TeamChannel", "Chat");
						goto IL_17F;
					}
				}
			}
			this.m_chatRoomName.text = StringUtil.TR("SpectatorChannel", "Chat");
			IL_17F:;
		}
		else if (UITextConsole.m_whisperCommand.IsSlashCommand(text))
		{
			this.m_chatRoomName.text = this.m_chatCommand.Substring(text.Length);
		}
		else
		{
			this.m_chatRoomName.text = "[" + this.m_chatCommand.Substring(1, 1).ToUpper() + this.m_chatCommand.Substring(2) + "]";
		}
		TextMeshProUGUI chatRoomName = this.m_chatRoomName;
		chatRoomName.text += ":";
		this.AlignChatText();
		Color color = Color.white;
		if (HUD_UIResources.Get() != null)
		{
			if (this.m_chatCommand == StringUtil.TR("/group", "SlashCommand"))
			{
				color = HUD_UIResources.Get().m_GroupChatColor;
			}
			else if (this.m_chatCommand == StringUtil.TR("/general", "SlashCommand"))
			{
				color = HUD_UIResources.Get().m_GlobalChatColor;
			}
			else
			{
				if (!(this.m_chatCommand == StringUtil.TR("/game", "SlashCommand")))
				{
					if (this.m_chatCommand == StringUtil.TR("/game", "SlashCommandAlias1"))
					{
					}
					else
					{
						if (this.m_chatCommand.StartsWith(StringUtil.TR("/whisper", "SlashCommand")))
						{
							color = HUD_UIResources.Get().m_whisperChatColor;
							goto IL_36A;
						}
						if (this.m_chatCommand == StringUtil.TR("/team", "SlashCommand"))
						{
							color = HUD_UIResources.Get().m_TeamChatColor;
							goto IL_36A;
						}
						color = Color.white;
						goto IL_36A;
					}
				}
				color = HUD_UIResources.Get().m_GameChatColor;
			}
		}
		IL_36A:
		this.m_chatRoomName.color = color;
		if (this.m_changeChannelAlpha)
		{
			this.m_chatRoomName.color = new Color(this.m_chatRoomName.color.r, this.m_chatRoomName.color.g, this.m_chatRoomName.color.b, this.chatAlpha);
		}
	}

	private void UnsetChatRoom()
	{
		if (this.m_chatRoomName == null)
		{
			return;
		}
		this.m_chatRoomName.text = string.Empty;
		this.AlignChatText();
	}

	private void AlignChatText()
	{
		this.m_chatRoomName.CalculateLayoutInputHorizontal();
		float num = this.m_chatRoomName.preferredWidth + this.m_chatRoomName.rectTransform.offsetMin.x;
		num *= this.m_chatRoomName.rectTransform.localScale.x;
		RectTransform rectTransform = this.m_textInput.transform as RectTransform;
		rectTransform.offsetMin = new Vector2(num, rectTransform.offsetMin.y);
	}

	private string ColorToHex(Color color)
	{
		string str = string.Empty;
		str += ((int)(color.r * 255f)).ToString("X2");
		str += ((int)(color.g * 255f)).ToString("X2");
		return str + ((int)(color.b * 255f)).ToString("X2");
	}

	public void DehighlightTextAndPositionCarat()
	{
		this.MoveCaretToEnd();
	}

	public void ChangeChannel(string channelName)
	{
		this.m_textInput.text = "/" + channelName + " " + this.m_textInput.text;
		this.RefreshChatRoomDisplay();
		this.SelectInput(string.Empty);
		this.MoveCaretToEnd();
	}

	private void SetCaretToLastKnownPosition()
	{
		this.UpdateCaretPosition(this.m_lastCaratPosition);
	}

	private void MoveCaretToEnd()
	{
		this.UpdateCaretPosition(-1);
	}

	private void UpdateCaretPosition(int position)
	{
		this.m_updateCaret = true;
		this.m_caretPositionToUpdate = position;
	}

	private void OnGameInfoNotification(GameInfoNotification notification)
	{
		this.RefreshChatRoomDisplay();
	}

	public void IgnoreNextTextChange()
	{
		this.m_ignoreNextTypeInput = true;
	}

	private struct HandledMessage
	{
		public TextConsole.Message Message;

		public TextConsole.AllowedEmojis AllowedEmojis;
	}
}
