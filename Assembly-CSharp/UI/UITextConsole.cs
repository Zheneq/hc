using I2.Loc;
using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextConsole : MonoBehaviour
{
	private struct HandledMessage
	{
		public TextConsole.Message Message;

		public TextConsole.AllowedEmojis AllowedEmojis;
	}

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

	private const int c_maxConsoleStringLength = 256;

	private const int c_maxTextSendLength = 200;

	private static Queue<HandledMessage> s_handledMessages = new Queue<HandledMessage>();

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

	//[CompilerGenerated]
	//private static Action<FriendStatusNotification> HandleFriendStatusNotification;

	//[CompilerGenerated]
	//private static Action OnGroupUpdateNotification;

	[CompilerGenerated]
	private static Action<FriendStatusNotification> _003C_003Ef__mg_0024cache2;

	//[CompilerGenerated]
	//private static Action OnGroupUpdateNotification;

	private bool m_changeChannelAlpha => UIManager.Get().CurrentState == UIManager.ClientState.InGame;

	private static void InitializeAutoComplete()
	{
		if (m_isAutoCompleteInitialized)
		{
			return;
		}
		while (true)
		{
			m_chatCommands.Add(m_globalCommand);
			m_chatCommands.Add(m_teamCommand);
			m_chatCommands.Add(m_allCommand);
			m_chatCommands.Add(m_groupCommand);
			m_chatCommands.Add(m_whisperCommand);
			m_playerCommands.Add(m_whisperCommand);
			m_playerCommands.Add(new SlashCommand_GroupInvite());
			m_playerCommands.Add(new SlashCommand_UserBlock());
			BuildLocalizedSlashCommands();
			if (ClientGameManager.Get() != null)
			{
				using (Dictionary<long, FriendInfo>.ValueCollection.Enumerator enumerator = ClientGameManager.Get().FriendList.Friends.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FriendInfo current = enumerator.Current;
						if (current.FriendStatus == FriendStatus.Friend)
						{
							TryAddToAutoComplete(current.FriendHandle);
						}
					}
				}
			}
			m_isAutoCompleteInitialized = true;
			return;
		}
	}

	private void Start()
	{
		m_autocompleteList.SetVisible(false);
		m_chatroomList.SetVisible(false);
		InitializeAutoComplete();
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerExit, OnPointerExit);
		UIEventTriggerUtils.AddListener(m_textInput.gameObject, EventTriggerType.PointerClick, OnInputClicked);
		UIEventTriggerUtils.AddListener(m_chatroomHitbox.gameObject, EventTriggerType.PointerClick, OnChatroomClick);
		UIEventTriggerUtils.AddListener(m_scrollBar.gameObject, EventTriggerType.Scroll, OnScroll);
		UIEventTriggerUtils.AddListener(m_background.gameObject, EventTriggerType.Scroll, OnScroll);
		m_textInput.onEndEdit.AddListener(OnEndEdit);
		m_textInput.onValueChanged.AddListener(OnTypeInput);
		m_scrollRect.scrollSensitivity = 100f;
		lastAlphaSet = -1f;
		m_visible = false;
		chatAlpha.EaseTo(m_startAlpha, 0f);
		ApplyChatAlpha();
		blockingRaycasts = false;
		for (int i = 0; i < m_inGameCanvasGroups.Length; i++)
		{
			m_inGameCanvasGroups[i].blocksRaycasts = true;
		}
		while (true)
		{
			for (int j = 0; j < m_frontEndCanvasGroups.Length; j++)
			{
				m_frontEndCanvasGroups[j].blocksRaycasts = blockingRaycasts;
			}
			if (m_newTextGlow != null)
			{
				UIManager.SetGameObjectActive(m_newTextGlow, false);
			}
			m_storedHistory = null;
			m_storedChatCommand = null;
			m_historyIndex = s_history.Count;
			HandledMessage[] array = s_handledMessages.ToArray();
			for (int k = 0; k < array.Length; k++)
			{
				DisplayMessage(array[k].Message, array[k].AllowedEmojis);
			}
			m_theTextList.HideRecentText();
			if (m_changeChannelAlpha)
			{
				if (m_startAlpha == 0f)
				{
					UIManager.SetGameObjectActive(m_chatroomHitbox, false);
				}
			}
			ChangeChatRoom();
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				
				clientGameManager.OnFriendStatusNotification += HandleFriendStatusNotification;
				ClientGameManager clientGameManager2 = ClientGameManager.Get();
				
				clientGameManager2.OnGroupUpdateNotification += OnGroupUpdateNotification;
				ClientGameManager.Get().OnGameInfoNotification += OnGameInfoNotification;
			}
			RebuildLocalizedText();
			LocalizationManager.OnLocalizeEvent += RebuildLocalizedText;
			return;
		}
	}

	private void RebuildLocalizedText()
	{
		using (List<SlashCommand>.Enumerator enumerator = m_chatCommands.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SlashCommand current = enumerator.Current;
				current.Localize();
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					goto end_IL_000d;
				}
			}
			end_IL_000d:;
		}
		using (List<SlashCommand>.Enumerator enumerator2 = m_playerCommands.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				SlashCommand current2 = enumerator2.Current;
				current2.Localize();
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
	}

	public static void BuildLocalizedSlashCommands()
	{
		m_friendAutocomplete.Clear();
		m_friendAutocomplete.Add(StringUtil.TR("AcceptFriend", "SlashCommand"));
		m_friendAutocomplete.Add(StringUtil.TR("AddFriend", "SlashCommand"));
		m_friendAutocomplete.Add(StringUtil.TR("NoteFriend", "SlashCommand"));
		m_friendAutocomplete.Add(StringUtil.TR("RejectFriend", "SlashCommand"));
		m_friendAutocomplete.Add(StringUtil.TR("RemoveFriend", "SlashCommand"));
		m_frontendAutocomplete.Clear();
		m_inGameAutocomplete.Clear();
		if (SlashCommands.Get() != null)
		{
			using (List<SlashCommand>.Enumerator enumerator = SlashCommands.Get().m_slashCommands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlashCommand current = enumerator.Current;
					if (!current.PublicFacing)
					{
						if (!ClientGameManager.Get().HasDeveloperAccess())
						{
							continue;
						}
					}
					if (current.AvailableInFrontEnd)
					{
						m_frontendAutocomplete.Add(current.Command);
					}
					if (current.AvailableInGame)
					{
						m_inGameAutocomplete.Add(current.Command);
					}
					if (current.Aliases == null)
					{
					}
					else
					{
						foreach (string alias in current.Aliases)
						{
							if (current.AvailableInFrontEnd)
							{
								m_frontendAutocomplete.Add(alias);
							}
							if (current.AvailableInGame)
							{
								m_inGameAutocomplete.Add(alias);
							}
						}
					}
				}
			}
		}
		m_frontendAutocomplete.Add(StringUtil.TR("/reply", "SlashCommand"));
		m_frontendAutocomplete.Add(StringUtil.TR("/reply", "SlashCommandAlias1"));
		m_frontendAutocomplete.Sort();
		m_inGameAutocomplete.Add(StringUtil.TR("/reply", "SlashCommand"));
		m_inGameAutocomplete.Add(StringUtil.TR("/reply", "SlashCommandAlias1"));
		m_inGameAutocomplete.Sort();
	}

	private void OnScroll(BaseEventData data)
	{
		if ((double)(float)chatAlpha < 0.5)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	private static void HandleFriendStatusNotification(FriendStatusNotification notification)
	{
		if (notification.FriendList.IsError)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					TextConsole.Get().Write("Friends list temporarily unavailable", ConsoleMessageType.Error);
					return;
				}
			}
		}
		using (Dictionary<long, FriendInfo>.ValueCollection.Enumerator enumerator = notification.FriendList.Friends.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FriendInfo current = enumerator.Current;
				if (current.FriendStatus == FriendStatus.Friend)
				{
					TryAddToAutoComplete(current.FriendHandle);
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
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
				LobbyPlayerInfo current = enumerator.Current;
				if (current.IsNPCBot)
				{
					if (!current.BotsMasqueradeAsHumans)
					{
						continue;
					}
				}
				TryAddToAutoComplete(current.Handle);
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_006a;
					}
				}
			}
			end_IL_006a:;
		}
		IEnumerator<LobbyPlayerInfo> enumerator2 = lobbyTeamInfo.TeamBPlayerInfo.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				LobbyPlayerInfo current2 = enumerator2.Current;
				if (current2.IsNPCBot)
				{
					if (!current2.BotsMasqueradeAsHumans)
					{
						continue;
					}
				}
				TryAddToAutoComplete(current2.Handle);
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
		finally
		{
			if (enumerator2 != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						enumerator2.Dispose();
						goto end_IL_00df;
					}
				}
			}
			end_IL_00df:;
		}
	}

	private static bool TryAddToAutoComplete(string handle)
	{
		if (!handle.IsNullOrEmpty())
		{
			if (!(handle == ClientGameManager.Get().Handle))
			{
				int num = m_playerAutocomplete.BinarySearch(handle, m_caseInsenitiveComparer);
				if (num < 0)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							m_playerAutocomplete.Insert(~num, handle);
							return true;
						}
					}
				}
				if (num == 0)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							if (m_playerAutocomplete.Count != 0)
							{
								if (handle.EqualsIgnoreCase(m_playerAutocomplete[0]))
								{
									return false;
								}
							}
							m_playerAutocomplete.Insert(0, handle);
							return true;
						}
					}
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
			TryAddToAutoComplete(members[i].MemberHandle);
		}
		while (true)
		{
			return;
		}
	}

	public void AddHandleMessage()
	{
		if (TextConsole.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (!m_handlingMessages)
			{
				while (true)
				{
					m_handlingMessages = true;
					TextConsole.Get().OnMessage += HandleMessage;
					return;
				}
			}
			return;
		}
	}

	public void RemoveHandleMessage()
	{
		if (TextConsole.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (m_handlingMessages)
			{
				while (true)
				{
					m_handlingMessages = false;
					TextConsole.Get().OnMessage -= HandleMessage;
					return;
				}
			}
			return;
		}
	}

	private void OnEnable()
	{
		blockingRaycasts = false;
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			for (int i = 0; i < m_inGameCanvasGroups.Length; i++)
			{
				m_inGameCanvasGroups[i].blocksRaycasts = blockingRaycasts;
			}
			for (int j = 0; j < m_frontEndCanvasGroups.Length; j++)
			{
				m_frontEndCanvasGroups[j].blocksRaycasts = true;
			}
		}
		else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
		{
			for (int k = 0; k < m_inGameCanvasGroups.Length; k++)
			{
				m_inGameCanvasGroups[k].blocksRaycasts = true;
			}
			for (int l = 0; l < m_frontEndCanvasGroups.Length; l++)
			{
				m_frontEndCanvasGroups[l].blocksRaycasts = blockingRaycasts;
			}
		}
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					for (int m = 0; m < m_inGameCanvasGroups.Length; m++)
					{
						m_inGameCanvasGroups[m].blocksRaycasts = blockingRaycasts;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							for (int n = 0; n < m_frontEndCanvasGroups.Length; n++)
							{
								m_frontEndCanvasGroups[n].blocksRaycasts = true;
							}
							while (true)
							{
								switch (4)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
						}
					}
				}
				}
			}
		}
		if (UIManager.Get().CurrentState != 0)
		{
			return;
		}
		while (true)
		{
			for (int num = 0; num < m_inGameCanvasGroups.Length; num++)
			{
				m_inGameCanvasGroups[num].blocksRaycasts = true;
			}
			while (true)
			{
				for (int num2 = 0; num2 < m_frontEndCanvasGroups.Length; num2++)
				{
					m_frontEndCanvasGroups[num2].blocksRaycasts = blockingRaycasts;
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
		}
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		ClientGameManager.Get().OnFriendStatusNotification -= HandleFriendStatusNotification;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		
		clientGameManager.OnGroupUpdateNotification -= OnGroupUpdateNotification;
		ClientGameManager.Get().OnGameInfoNotification -= OnGameInfoNotification;
	}

	public void SetupWhisper(string whisperee)
	{
		m_chatCommand = m_whisperCommand.Command + " " + whisperee;
		RefreshChatRoomDisplay();
		SelectInput(string.Empty);
		MoveCaretToEnd();
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
		m_textInput.text += stringToAdd;
		if (selectInput)
		{
			EventSystem.current.SetSelectedGameObject(m_textInput.gameObject);
		}
		MoveCaretToEnd();
	}

	private void OnTypeInput(string textString)
	{
		OnTypeInput(textString, true);
	}

	private void OnTypeInput(string textString, bool setChatRoom)
	{
		if (m_ignoreNextTypeInput)
		{
			m_ignoreNextTypeInput = false;
			return;
		}
		if (m_autocompleteList.IsVisible())
		{
			if (Input.GetKey(KeyCode.Backspace))
			{
				m_ignoreNextTypeInput = true;
				if (m_textInput.caretPosition > 0)
				{
					m_textInput.caretPosition--;
					m_textInput.text = m_textInput.text.Substring(0, m_textInput.caretPosition) + m_textInput.text.Substring(m_textInput.caretPosition + 1);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Delete))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_autocompleteList.SetVisible(false);
						return;
					}
				}
			}
		}
		if (m_textInput.text.Length > 200)
		{
			m_textInput.text = m_textInput.text.Substring(0, 200);
		}
		if (!m_textInput.text.StartsWith(StringUtil.TR("/reply", "SlashCommand") + " "))
		{
			if (!m_textInput.text.StartsWith(StringUtil.TR("/reply", "SlashCommandAlias1") + " "))
			{
				goto IL_01e3;
			}
		}
		int num = m_textInput.text.IndexOf(' ');
		m_textInput.text = GenerateReplyPrefix() + m_textInput.text.Substring(num + 1);
		MoveCaretToEnd();
		goto IL_01e3;
		IL_01e3:
		if (setChatRoom)
		{
			SetChatRoom(m_textInput.text);
		}
		if (Input.GetKey(KeyCode.Delete))
		{
			return;
		}
		while (true)
		{
			string beforeAutocomplete;
			List<string> autoCompletePossibilities = GetAutoCompletePossibilities(false, out beforeAutocomplete);
			int length = Mathf.Clamp(m_textInput.caretPosition, 0, m_textInput.text.Length);
			if (autoCompletePossibilities.Count == 1)
			{
				if (m_textInput.text.Substring(0, length).EndsWith(autoCompletePossibilities[0].Trim()))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							m_autocompleteList.SetVisible(false);
							return;
						}
					}
				}
			}
			m_autocompleteList.Setup(this, autoCompletePossibilities, beforeAutocomplete);
			m_autocompleteList.SetVisible(autoCompletePossibilities.Count > 0);
			return;
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
		if (m_autocompleteList.IsVisible())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_textInput.caretPosition = m_lastCaratPosition;
					m_autocompleteList.SelectCurrent();
					return;
				}
			}
		}
		m_lastCaratPosition = -1;
		OnInputSubmitted();
		int num;
		if (GameManager.Get() != null)
		{
			num = ((UIManager.Get().CurrentState == UIManager.ClientState.InGame) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		if (num == 0 || !(m_timeTillCollapse <= 0f))
		{
			return;
		}
		while (true)
		{
			ToggleVisibility();
			return;
		}
	}

	private void ApplyChatAlpha()
	{
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			if (m_inGameFadingImages != null)
			{
				for (int i = 0; i < m_inGameFadingImages.Length; i++)
				{
					if (m_inGameFadingImages[i] != null)
					{
						Color color = m_inGameFadingImages[i].color;
						color.a = chatAlpha;
						m_inGameFadingImages[i].color = color;
					}
				}
			}
			if (m_frontEndFadingImages != null)
			{
				for (int j = 0; j < m_frontEndFadingImages.Length; j++)
				{
					if (m_frontEndFadingImages[j] != null)
					{
						Color color2 = m_frontEndFadingImages[j].color;
						color2.a = 0f;
						m_frontEndFadingImages[j].color = color2;
						m_frontEndFadingImages[j].raycastTarget = false;
					}
				}
			}
		}
		else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
		{
			if (m_inGameFadingImages != null)
			{
				for (int k = 0; k < m_inGameFadingImages.Length; k++)
				{
					if (m_inGameFadingImages[k] != null)
					{
						Color color3 = m_inGameFadingImages[k].color;
						color3.a = 1f;
						m_inGameFadingImages[k].color = color3;
						m_frontEndFadingImages[k].raycastTarget = true;
					}
				}
			}
			if (m_frontEndFadingImages != null)
			{
				for (int l = 0; l < m_frontEndFadingImages.Length; l++)
				{
					if (m_frontEndFadingImages[l] != null)
					{
						Color color4 = m_frontEndFadingImages[l].color;
						color4.a = chatAlpha;
						m_frontEndFadingImages[l].color = color4;
					}
				}
			}
		}
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
		{
			for (int m = 0; m < m_inGameCanvasGroups.Length; m++)
			{
				m_inGameCanvasGroups[m].alpha = chatAlpha;
			}
			for (int n = 0; n < m_frontEndCanvasGroups.Length; n++)
			{
				m_frontEndCanvasGroups[n].alpha = 1f;
			}
		}
		else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
		{
			for (int num = 0; num < m_inGameCanvasGroups.Length; num++)
			{
				m_inGameCanvasGroups[num].alpha = 1f;
			}
			for (int num2 = 0; num2 < m_frontEndCanvasGroups.Length; num2++)
			{
				m_frontEndCanvasGroups[num2].alpha = chatAlpha;
			}
		}
		Color color5 = m_chatText.color;
		color5.a = (float)chatAlpha / 2f + 0.5f;
		m_chatText.color = color5;
		m_theTextList.SetTextAlpha(chatAlpha);
		m_autocompleteList.m_canvasGroup.alpha = chatAlpha;
		lastAlphaSet = chatAlpha;
		if (!blockingRaycasts && (float)chatAlpha > 0f)
		{
			blockingRaycasts = true;
			if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
			{
				for (int num3 = 0; num3 < m_frontEndCanvasGroups.Length; num3++)
				{
					m_frontEndCanvasGroups[num3].blocksRaycasts = true;
				}
				for (int num4 = 0; num4 < m_inGameCanvasGroups.Length; num4++)
				{
					m_inGameCanvasGroups[num4].blocksRaycasts = blockingRaycasts;
				}
			}
			else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				for (int num5 = 0; num5 < m_inGameCanvasGroups.Length; num5++)
				{
					m_inGameCanvasGroups[num5].blocksRaycasts = true;
				}
				for (int num6 = 0; num6 < m_frontEndCanvasGroups.Length; num6++)
				{
					m_frontEndCanvasGroups[num6].blocksRaycasts = blockingRaycasts;
				}
			}
		}
		else if (blockingRaycasts)
		{
			if ((float)chatAlpha <= 0f)
			{
				blockingRaycasts = false;
				if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
				{
					for (int num7 = 0; num7 < m_frontEndCanvasGroups.Length; num7++)
					{
						m_frontEndCanvasGroups[num7].blocksRaycasts = true;
					}
					for (int num8 = 0; num8 < m_inGameCanvasGroups.Length; num8++)
					{
						m_inGameCanvasGroups[num8].blocksRaycasts = blockingRaycasts;
					}
				}
				else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
				{
					for (int num9 = 0; num9 < m_inGameCanvasGroups.Length; num9++)
					{
						m_inGameCanvasGroups[num9].blocksRaycasts = true;
					}
					for (int num10 = 0; num10 < m_frontEndCanvasGroups.Length; num10++)
					{
						m_frontEndCanvasGroups[num10].blocksRaycasts = blockingRaycasts;
					}
				}
			}
		}
		CanvasGroup component = m_scrollBar.GetComponent<CanvasGroup>();
		component.blocksRaycasts = blockingRaycasts;
		component.alpha = chatAlpha;
		if (!m_changeChannelAlpha)
		{
			return;
		}
		while (true)
		{
			TextMeshProUGUI chatRoomName = m_chatRoomName;
			Color color6 = m_chatRoomName.color;
			float r = color6.r;
			Color color7 = m_chatRoomName.color;
			float g = color7.g;
			Color color8 = m_chatRoomName.color;
			chatRoomName.color = new Color(r, g, color8.b, chatAlpha);
			return;
		}
	}

	public bool EscapeJustPressed()
	{
		return m_escapeJustPressed;
	}

	public bool InputJustcleared()
	{
		return m_inputJustCleared;
	}

	private void ClearInputSelect()
	{
		if (!(EventSystem.current != null))
		{
			return;
		}
		while (true)
		{
			if (EventSystem.current.currentSelectedGameObject == m_textInput.gameObject)
			{
				while (true)
				{
					m_inputJustCleared = true;
					EventSystem.current.SetSelectedGameObject(null);
					return;
				}
			}
			return;
		}
	}

	public void SelectInput(string startingInput = "")
	{
		if (!m_visible)
		{
			ToggleVisibility();
			setInputSelected = true;
		}
		else
		{
			if (UIScreenManager.Get() != null)
			{
				SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
			}
			UpdateElementsVisibility();
		}
		if (EventSystem.current.currentSelectedGameObject != m_textInput.gameObject)
		{
			EventSystem.current.SetSelectedGameObject(m_textInput.gameObject);
		}
		if (!(startingInput != string.Empty))
		{
			return;
		}
		while (true)
		{
			m_textInput.text = startingInput;
			return;
		}
	}

	public void SetTimeTillCollapse(float time)
	{
		if (m_doNotAutoFade)
		{
			return;
		}
		while (true)
		{
			m_timeTillCollapse = time;
			return;
		}
	}

	private void LateUpdate()
	{
		if (m_visible)
		{
			UIManager.SetGameObjectActive(m_textInput.placeholder, EventSystem.current == null || EventSystem.current.currentSelectedGameObject != m_textInput.gameObject);
		}
		else
		{
			bool flag = false;
			if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
			{
				CanvasGroup[] inGameCanvasGroups = m_inGameCanvasGroups;
				int num = 0;
				while (true)
				{
					if (num < inGameCanvasGroups.Length)
					{
						CanvasGroup canvasGroup = inGameCanvasGroups[num];
						if (canvasGroup.gameObject == m_textInput.gameObject)
						{
							flag = true;
							break;
						}
						num++;
						continue;
					}
					break;
				}
			}
			else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				CanvasGroup[] frontEndCanvasGroups = m_frontEndCanvasGroups;
				int num2 = 0;
				while (true)
				{
					if (num2 < frontEndCanvasGroups.Length)
					{
						CanvasGroup canvasGroup2 = frontEndCanvasGroups[num2];
						if (canvasGroup2.gameObject == m_textInput.gameObject)
						{
							flag = true;
							break;
						}
						num2++;
						continue;
					}
					break;
				}
			}
			UIManager.SetGameObjectActive(m_textInput.placeholder, !flag);
		}
		m_textInput.placeholder.GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
		if (EventSystem.current.currentSelectedGameObject == m_textInput.gameObject)
		{
			if (m_updateCaret)
			{
				if (m_caretPositionToUpdate >= 0)
				{
					if (m_caretPositionToUpdate < m_textInput.text.Length)
					{
						if (m_textInput.caretPosition == m_caretPositionToUpdate)
						{
							if (m_textInput.selectionAnchorPosition == m_caretPositionToUpdate)
							{
								m_updateCaret = false;
								goto IL_029f;
							}
						}
						m_textInput.caretPosition = m_caretPositionToUpdate;
						m_textInput.selectionAnchorPosition = m_caretPositionToUpdate;
						goto IL_029f;
					}
				}
				if (m_textInput.caretPosition >= m_textInput.text.Length)
				{
					if (m_textInput.selectionAnchorPosition >= m_textInput.text.Length)
					{
						m_updateCaret = false;
						goto IL_029f;
					}
				}
				m_textInput.MoveTextEnd(false);
			}
			else
			{
				m_lastCaratPosition = m_textInput.caretPosition;
			}
		}
		goto IL_029f;
		IL_029f:
		bool flag2 = false;
		if (m_visible)
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
							goto IL_035e;
						}
					}
					flag2 = false;
				}
			}
		}
		goto IL_035e;
		IL_035e:
		if (!flag2)
		{
			if (!(m_timeTillCollapse <= 0f) || !m_visible)
			{
				return;
			}
			if (m_hovering)
			{
				return;
			}
			if (IsTextInputFocused(true))
			{
				return;
			}
		}
		if (IsPressedAndMousedOver())
		{
			return;
		}
		while (true)
		{
			if (!EmoticonPanel.Get().IsPanelOpen())
			{
				while (true)
				{
					ToggleVisibility();
					return;
				}
			}
			return;
		}
	}

	private bool CheckInputField()
	{
		if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null)
		{
			return true;
		}
		
		TMP_InputField component = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
		if (component == null)
		{
			return true;
		}
		
		if (component != m_textInput)
		{
			string text = "ENTER INPUT FALSE: " + component.name;
			Transform elem = component.transform;
			while (elem.parent != null)
			{
				text += " -> " + elem.parent.name;
				elem = elem.parent;
			}
			Log.Info(text);
		}
		return component == m_textInput;
	}

	private void Update()
	{
		bool pressedEnter = (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && CheckInputField();
		bool pressedSlash = Input.GetKeyDown(KeyCode.Slash)
		            && EventSystem.current != null
		            && EventSystem.current.currentSelectedGameObject != m_textInput.gameObject;
		bool pressedChatReply = InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ChatReply)
		             && (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DebugCamera"));
		bool pressedChatAll = InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ChatAll);
		bool pressedUp = m_textInput.isFocused && Input.GetKeyDown(KeyCode.UpArrow);
		bool pressedDown = m_textInput.isFocused && Input.GetKeyDown(KeyCode.DownArrow);
		bool pressedTextNavigation = m_textInput.isFocused
		             && (Input.GetKeyDown(KeyCode.RightArrow)
		                 || Input.GetKeyDown(KeyCode.LeftArrow)
		                 || Input.GetKeyDown(KeyCode.Home)
		                 || Input.GetKeyDown(KeyCode.End));
		bool pressedTab = m_textInput.isFocused && Input.GetKeyDown(KeyCode.Tab);
		bool isAutocompleteVisible = m_autocompleteList.IsVisible();
		
		if (setInputSelected && (float)chatAlpha > 0.5f)
		{
			if (EventSystem.current.currentSelectedGameObject != m_textInput.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(m_textInput.gameObject);
			}
			else
			{
				setInputSelected = false;
			}
		}

		m_escapeJustPressed = false;
		if (!InputJustcleared()
		    && (pressedEnter || pressedSlash || pressedChatReply || pressedChatAll)
		    && (EventSystem.current.currentSelectedGameObject == null
		        || (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null
		            && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() == null)))
		{
			if (pressedSlash)
			{
				SelectInput("/");
				EventSystem.current.SetSelectedGameObject(m_textInput.gameObject);
				MoveCaretToEnd();
			}
			else if (pressedChatReply)
			{
				SelectInput(GenerateReplyPrefix());
				MoveCaretToEnd();
			}
			else if (pressedChatAll)
			{
				if (GameFlowData.Get() == null)
				{
					SelectInput(StringUtil.TR("/general", "SlashCommand") + " ");
				}
				else
				{
					SelectInput(StringUtil.TR("/game", "SlashCommand") + " ");
				}

				MoveCaretToEnd();
			}
			else
			{
				SelectInput(string.Empty);
				SetCaretToLastKnownPosition();
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape) && IsTextInputFocused(true))
		{
			if (m_autocompleteList.IsVisible())
			{
				m_autocompleteList.SetVisible(false);
			}
			else
			{
				ClearInputSelect();
				if (m_visible && m_timeTillCollapse <= 0f)
				{
					ToggleVisibility();
				}

				m_escapeJustPressed = true;
				UIUtils.MarkInputFieldHasFocusDirty();
			}
		}
		else if (!pressedDown && !pressedUp)
		{
			if (pressedTab && isAutocompleteVisible)
			{
				m_autocompleteList.SelectCurrent();
			}
			else if (pressedTab && m_textInput.text.StartsWith("/"))
			{
				GetAutoCompletePossibilities(true, out string _);
			}
			else if (pressedTab && !m_textInput.text.StartsWith("/"))
			{
				List<string> availableChatRooms = GetAvailableChatRooms();
				availableChatRooms.Add(availableChatRooms[0]);
				for (int j = 0; j < availableChatRooms.Count; j++)
				{
					if (j == availableChatRooms.Count - 1)
					{
						m_chatCommand = StringUtil.TR("/general", "SlashCommand");
						break;
					}

					if (m_chatCommand == availableChatRooms[j])
					{
						m_chatCommand = availableChatRooms[j + 1];
						break;
					}
				}

				RefreshChatRoomDisplay();
			}
			else
			{
				if (pressedTextNavigation)
				{
					m_autocompleteList.SetVisible(false);
				}
			}
		}
		else if (isAutocompleteVisible)
		{
			if (pressedDown)
			{
				m_autocompleteList.SelectDown();
			}
			else
			{
				m_autocompleteList.SelectUp();
			}
		}
		else
		{
			if (m_historyIndex >= s_history.Count)
			{
				m_storedChatCommand = m_chatCommand;
				m_storedHistory = m_textInput.text;
			}

			int historyIndex = m_historyIndex;

			m_historyIndex = historyIndex + (pressedDown ? 1 : -1);
			if (m_historyIndex < 0)
			{
				m_historyIndex = 0;
			}

			string text;
			if (m_historyIndex >= s_history.Count)
			{
				m_chatCommand = m_storedChatCommand;
				text = m_storedHistory;
				m_historyIndex = s_history.Count;
				m_storedHistory = null;
				m_storedChatCommand = null;
			}
			else
			{
				text = s_history[m_historyIndex];
			}

			m_ignoreNextTypeInput = true;
			if (text.IsNullOrEmpty())
			{
				m_textInput.text = string.Empty;
			}
			else
			{
				m_textInput.text = text;
				SetChatRoom(m_textInput.text);
			}

			MoveCaretToEnd();
		}

		m_inputJustCleared = false;
		if (m_timeTillCollapse > 0f)
		{
			m_timeTillCollapse -= Time.deltaTime;
		}

		if (lastAlphaSet != (float)chatAlpha)
		{
			ApplyChatAlpha();
		}

		ScrollBarAfterNewChat();
		if (m_newTextGlow != null && m_checkForNextTextGlow)
		{
			if (m_scrollRect.verticalScrollbar.value > 0f
			    && UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				UIManager.SetGameObjectActive(m_newTextGlow, true);
			}
			else
			{
				UIManager.SetGameObjectActive(m_newTextGlow, false);
				m_checkForNextTextGlow = false;
			}
		}
		
		if (m_visible && EventSystem.current.currentSelectedGameObject != null
		              && EventSystem.current.currentSelectedGameObject != m_textInput.gameObject
		              && EventSystem.current.currentSelectedGameObject != m_scrollBar.gameObject
		              && m_lastIsTextInputNotSelected && (EmoticonPanel.Get() == null
		                                                  || !EmoticonPanel.Get().IsPanelOpen()
		                                                  && EmoticonPanel.Get().m_emoticonBtn.spriteController
			                                                  .gameObject != EventSystem.current
			                                                  .currentSelectedGameObject))
		{
			if (m_scrollBar.size < 0.9999 && m_scrollBar.value <= 0.0001)
			{
				ToggleVisibility();
			}
		}
		
		m_lastIsTextInputNotSelected = EventSystem.current.currentSelectedGameObject != null
		                               && EventSystem.current.currentSelectedGameObject != m_textInput.gameObject;
		DateTime dateTime = ClientGameManager.Get().PacificNow();
		LobbyAlertMissionDataNotification alertMissionsData = ClientGameManager.Get().AlertMissionsData;
		if (alertMissionsData != null && alertMissionsData.NextAlert.HasValue)
		{
			float num6 = 0f;
			if (!m_hadNextAlertTime)
			{
				float num7 = alertMissionsData.ReminderHours.DefaultIfEmpty().Max();
				if (num7 == 0f || alertMissionsData.NextAlert.Value.AddHours(0f - num7) <= dateTime)
				{
					num6 = (float)(alertMissionsData.NextAlert.Value - dateTime).TotalHours;
				}
			}
			else if (!alertMissionsData.ReminderHours.IsNullOrEmpty())
			{
				for (int i = 0; i < alertMissionsData.ReminderHours.Count; i++)
				{
					DateTime t = alertMissionsData.NextAlert.Value.AddHours(0f - alertMissionsData.ReminderHours[i]);
					if (t > m_lastSystemMessageCheckPST && t <= dateTime)
					{
						num6 = alertMissionsData.ReminderHours[i];
					}
				}
			}

			if (num6 > 0f)
			{
				TextConsole.Get().Write(
					string.Format(
						StringUtil.TR("NextAlertIn", "Global"),
						StringUtil.GetTimeDifferenceText(TimeSpan.FromHours(num6), true)));
			}
		}

		m_hadNextAlertTime = alertMissionsData != null && alertMissionsData.NextAlert.HasValue;
		if (alertMissionsData != null && alertMissionsData.CurrentAlert != null && !m_hadCurrentAlert)
		{
			TextConsole.Get().Write(StringUtil.TR("AlertActive", "Global"));
		}

		m_hadCurrentAlert = alertMissionsData != null && alertMissionsData.CurrentAlert != null;
		m_lastSystemMessageCheckPST = dateTime;
	}

	public bool CheckTextInput()
	{
		int result;
		if (m_textInput.isFocused)
		{
			result = ((m_textInput.text == string.Empty) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool IsTextInputFocused(bool checkEmoticonPanel = false)
	{
		if (checkEmoticonPanel)
		{
			if (EmoticonPanel.Get() != null && EmoticonPanel.Get().IsPanelOpen())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		if (!(EventSystem.current == null))
		{
			if (!(EventSystem.current.currentSelectedGameObject == null))
			{
				int result;
				if (!(m_textInput.gameObject == EventSystem.current.currentSelectedGameObject))
				{
					result = ((m_scrollBar.gameObject == EventSystem.current.currentSelectedGameObject) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				return (byte)result != 0;
			}
		}
		return false;
	}

	private bool IsPressedAndMousedOver()
	{
		if (EventSystem.current == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.pointerId = -1;
		PointerEventData pointerEventData2 = pointerEventData;
		pointerEventData2.position = Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData2, list);
		for (int i = 0; i < list.Count; i++)
		{
			if (!(list[i].gameObject.GetComponentInParent<UITextConsole>() != null))
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public List<string> GetAvailableChatRooms()
	{
		bool isInGame = AppState.IsInGame();
		bool isInActiveGame = GameManager.Get() != null
		             && GameManager.Get().GameInfo != null
		             && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;
		bool isInGroup = ClientGameManager.Get() != null
		             && ClientGameManager.Get().GroupInfo != null
		             && ClientGameManager.Get().GroupInfo.InAGroup;
		
		List<string> list = new List<string>();
		if (!isInGame)
		{
			list.Add(StringUtil.TR("/general", "SlashCommand"));
		}

		if (isInActiveGame || isInGame)
		{
			if (GameManager.Get() != null
			    && GameManager.Get().PlayerInfo != null
			    && GameManager.Get().PlayerInfo.TeamId != Team.Spectator)
			{
				list.Add(StringUtil.TR("/team", "SlashCommand"));
			}
			else
			{
				list.Add(StringUtil.TR("/team", "SlashCommandAlias1"));
			}
		}

		if (isInActiveGame
		    && ((Options_UI.Get() != null && Options_UI.Get().GetShowAllChat())
		        || (!isInGame && GameManager.Get().GameInfo.IsCustomGame)))
		{
			list.Add(StringUtil.TR("/game", "SlashCommandAlias1"));
		}
		
		if (isInGroup)
		{
			list.Add(StringUtil.TR("/group", "SlashCommand"));
		}
		
		foreach (string player in m_whisperedPlayers)
		{
			list.Add(StringUtil.TR("/whisper", "SlashCommand") + " " + player);
		}
		
		return list;
	}

	public List<string> GetAutoCompletePossibilities(bool doAutocomplete, out string beforeAutocomplete)
	{
		List<string> list = new List<string>();
		List<string> list2 = null;
		string text = null;
		beforeAutocomplete = string.Empty;
		int num = Mathf.Clamp(m_textInput.caretPosition, 0, m_textInput.text.Length);
		string text2 = m_textInput.text.Substring(0, num).ToLower();
		string text3 = m_textInput.text.Substring(num);
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
				list2 = m_inGameAutocomplete;
			}
			else
			{
				list2 = m_frontendAutocomplete;
			}
			text = array[0];
		}
		else if (array.Length == 2)
		{
			if (m_friendCommand.IsSlashCommand(array[0]))
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
					list2 = m_friendAutocomplete;
					text = array3[0];
					beforeAutocomplete = m_friendCommand.Command;
				}
				else if (array3.Length == 2)
				{
					list2 = m_playerAutocomplete;
					text = array3[1];
					beforeAutocomplete = m_friendCommand.Command + " " + array3[0];
				}
			}
			else
			{
				for (int i = 0; i < m_playerCommands.Count; i++)
				{
					if (m_playerCommands[i].IsSlashCommand(array[0]))
					{
						list2 = m_playerAutocomplete;
						text = array[1];
						beforeAutocomplete = m_playerCommands[i].Command;
						break;
					}
				}
			}
		}
		string text4;
		if (!text.IsNullOrEmpty())
		{
			if (text.Length >= 1)
			{
				if (!(text == "/"))
				{
					if (list2 != null)
					{
						int num2 = 0;
						int num3 = list2.Count;
						while (num2 < num3)
						{
							int num4 = num2 + (num3 - num2) / 2;
							int num5 = text.CompareTo(list2[num4].ToLower());
							if (num5 < 0)
							{
								num3 = num4;
							}
							else if (num5 > 0)
							{
								num2 = num4 + 1;
							}
							else
							{
								num2 = num4;
								num3 = num4;
							}
						}
						if (num2 < list2.Count)
						{
							if (list2[num2].ToLower().StartsWith(text))
							{
								int num6 = num2;
								for (int j = num2 + 1; j < list2.Count; j++)
								{
									if (list2[j].ToLower().StartsWith(text))
									{
										num6 = j;
										continue;
									}
									break;
								}
								if (num6 == num2)
								{
									text4 = list2[num2] + " ";
									if (text != text4)
									{
										if (!m_allCommand.IsSlashCommand(text4.Trim()))
										{
											goto IL_047d;
										}
										if (Options_UI.Get() != null)
										{
											if (Options_UI.Get().GetShowAllChat())
											{
												goto IL_047d;
											}
										}
										if (!AppState.IsInGame())
										{
											if (GameManager.Get().GameInfo != null)
											{
												if (GameManager.Get().GameInfo.IsCustomGame)
												{
													if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
													{
														goto IL_047d;
													}
												}
											}
										}
									}
								}
								else
								{
									text4 = string.Empty;
									string text5 = list2[num2].ToLower();
									string text6 = list2[num6].ToLower();
									for (int k = 0; k < text5.Length; k++)
									{
										if (k < text6.Length && text5[k] == text6[k])
										{
											while (true)
											{
												switch (3)
												{
												case 0:
													break;
												default:
													goto end_IL_04d0;
												}
												continue;
												end_IL_04d0:
												break;
											}
											text4 += text5[k];
											continue;
										}
										break;
									}
									for (int l = num2; l <= num6; l++)
									{
										if (m_allCommand.IsSlashCommand(list2[l].Trim()))
										{
											if (Options_UI.Get() != null)
											{
												if (Options_UI.Get().GetShowAllChat())
												{
													goto IL_05e4;
												}
											}
											if (AppState.IsInGame() || GameManager.Get().GameInfo == null)
											{
												continue;
											}
											if (!GameManager.Get().GameInfo.IsCustomGame)
											{
												continue;
											}
											if (GameManager.Get().GameInfo.GameStatus == GameStatus.Stopped)
											{
												continue;
											}
										}
										goto IL_05e4;
										IL_05e4:
										list.Add(list2[l]);
									}
								}
								goto IL_060d;
							}
						}
					}
					goto IL_06e4;
				}
			}
		}
		return list;
		IL_047d:
		list.Add(text4);
		goto IL_060d;
		IL_06e4:
		return list;
		IL_060d:
		if (doAutocomplete)
		{
			string text7 = text4 + text3.TrimStart();
			int num7 = (!text4.IsNullOrEmpty()) ? text4.Length : 0;
			if (!beforeAutocomplete.IsNullOrEmpty())
			{
				text7 = beforeAutocomplete + " " + text7;
				int num8 = num7;
				int num9;
				if (beforeAutocomplete.IsNullOrEmpty())
				{
					num9 = 0;
				}
				else
				{
					num9 = beforeAutocomplete.Length;
				}
				num7 = num8 + (num9 + 1);
			}
			if (m_textInput.text != text7)
			{
				m_textInput.text = text7;
				UpdateCaretPosition(num7);
				OnTypeInput(m_textInput.text, false);
			}
		}
		goto IL_06e4;
	}

	public void ScrollBarAfterNewChat()
	{
		if (!m_scrollChat)
		{
			return;
		}
		while (true)
		{
			if (m_scrollRect.verticalScrollbar.value != 0f)
			{
				m_scrollRect.verticalScrollbar.value = 0f;
				m_scrollChat = false;
			}
			return;
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
						goto IL_0084;
					}
				}
				if (!(AppState.GetCurrent() == AppState_RankModeDraft.Get()))
				{
					goto IL_0086;
				}
			}
			goto IL_0084;
		}
		goto IL_0086;
		IL_02a1:
		int scrollChat;
		m_scrollChat = ((byte)scrollChat != 0);
		RectTransform rectTransform;
		float num;
		if (!m_scrollChat)
		{
			Vector3 localPosition = rectTransform.localPosition;
			float y = localPosition.y;
			Vector2 pivot = rectTransform.pivot;
			localPosition.y = y - num * (1f - pivot.y);
			float y2 = localPosition.y;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			if (y2 < 0f - sizeDelta.y)
			{
				Vector2 sizeDelta2 = rectTransform.sizeDelta;
				localPosition.y = 0f - sizeDelta2.y;
			}
			rectTransform.localPosition = localPosition;
		}
		TextMeshProUGUI textMeshProUGUI;
		UIEventTriggerUtils.AddListener(textMeshProUGUI.gameObject, EventTriggerType.Scroll, OnScroll);
		return textMeshProUGUI;
		IL_0086:
		if (GameFlowData.Get() != null)
		{
			characterType = messageInfo.CharacterType;
		}
		rectTransform = (m_theTextList.transform as RectTransform);
		float value = m_scrollRect.verticalScrollbar.value;
		float size = m_scrollRect.verticalScrollbar.size;
		Vector2 sizeDelta3 = rectTransform.sizeDelta;
		float y3 = sizeDelta3.y;
		if (UIManager.Get().CurrentState != 0)
		{
			if (characterType != 0)
			{
				if (messageInfo.SenderTeam != Team.Spectator)
				{
					int num2 = 0;
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
						if (team != Team.Spectator)
						{
							num2 = ((messageInfo.SenderTeam != team) ? 1 : 0);
						}
						else
						{
							if (messageInfo.SenderTeam == Team.TeamA)
							{
								num2 = 0;
							}
							else
							{
								num2 = 1;
							}
						}
					}
					string text = $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)characterType + num2}>\u200b</size>{textEntry}";
					textEntry = text;
				}
			}
		}
		textMeshProUGUI = m_theTextList.AddEntry(textEntry, textColor, forceShowChat, HUD_UIResources.Get().m_textPaddingAmount, m_scrollRect, allowedEmojis);
		textMeshProUGUI.CalculateLayoutInputVertical();
		num = textMeshProUGUI.preferredHeight + HUD_UIResources.Get().m_textPaddingAmount;
		if (m_theTextList.NumEntires() >= 2)
		{
			if (!(num >= y3 * value))
			{
				scrollChat = ((size >= 0.999f) ? 1 : 0);
				goto IL_02a1;
			}
		}
		scrollChat = 1;
		goto IL_02a1;
		IL_0084:
		return null;
	}

	public TextMeshProUGUI AddTextEntry(string textEntry, Color textColor, bool forceShowChat)
	{
		RectTransform rectTransform = m_theTextList.transform as RectTransform;
		float value = m_scrollRect.verticalScrollbar.value;
		float size = m_scrollRect.verticalScrollbar.size;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		float y = sizeDelta.y;
		TextMeshProUGUI textMeshProUGUI = m_theTextList.AddEntry(textEntry, textColor, forceShowChat, HUD_UIResources.Get().m_textPaddingAmount, m_scrollRect);
		HUDTextConsoleItem component = textMeshProUGUI.GetComponent<HUDTextConsoleItem>();
		if (component != null)
		{
			UIManager.SetGameObjectActive(component.m_iconContainer, false);
		}
		m_checkForNextTextGlow = true;
		textMeshProUGUI.CalculateLayoutInputVertical();
		float num = textMeshProUGUI.preferredHeight + HUD_UIResources.Get().m_textPaddingAmount;
		int scrollChat;
		if (m_theTextList.NumEntires() >= 2)
		{
			if (!(num >= y * value))
			{
				scrollChat = ((size >= 0.999f) ? 1 : 0);
				goto IL_0104;
			}
		}
		scrollChat = 1;
		goto IL_0104;
		IL_0104:
		m_scrollChat = ((byte)scrollChat != 0);
		if (!m_scrollChat)
		{
			Vector3 localPosition = rectTransform.localPosition;
			float y2 = localPosition.y;
			Vector2 pivot = rectTransform.pivot;
			localPosition.y = y2 - num * (1f - pivot.y);
			float y3 = localPosition.y;
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			if (y3 < 0f - sizeDelta2.y)
			{
				Vector2 sizeDelta3 = rectTransform.sizeDelta;
				localPosition.y = 0f - sizeDelta3.y;
			}
			rectTransform.localPosition = localPosition;
		}
		UIEventTriggerUtils.AddListener(textMeshProUGUI.gameObject, EventTriggerType.Scroll, OnScroll);
		return textMeshProUGUI;
	}

	private string FormatConsoleMessage(TextConsole.Message message, bool selfMessage)
	{
		string empty = string.Empty;
		if (message.DisplayDevTag)
		{
			message.SenderHandle = StringUtil.TR("DevTag", "Global") + message.SenderHandle;
		}
		switch (message.MessageType)
		{
		case ConsoleMessageType.GlobalChat:
		{
			string arg = ColorToHex(HUD_UIResources.Get().m_GlobalChatColor);
			string text3 = "<link=channel:" + StringUtil.TR("/general", "SlashCommand").Substring(1) + ">" + StringUtil.TR("GlobalChannel", "Chat") + "</link>";
			if (selfMessage)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return $"<color=#{arg}>{text3} {message.SenderHandle}:  {message.Text}</color>";
					}
				}
			}
			return $"<color=#{arg}>{text3} [<link=name>{message.SenderHandle}</link>]:  {message.Text}</color>";
		}
		case ConsoleMessageType.GameChat:
		{
			string arg2 = ColorToHex(HUD_UIResources.Get().m_GameChatColor);
			if (selfMessage)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return string.Format("<color=#{0}>" + StringUtil.TR("GameChannel", "Chat") + " </color>{1}<color=#{0}>: {2}</color>", arg2, message.SenderHandle, message.Text);
					}
				}
			}
			return string.Format("<color=#{0}>" + StringUtil.TR("GameChannel", "Chat") + " <link=name>{1}</link>: {2}</color>", arg2, message.SenderHandle, message.Text);
		}
		case ConsoleMessageType.TeamChat:
		{
			string arg = ColorToHex(HUD_UIResources.Get().m_TeamChatColor);
			string text3;
			if (message.SenderTeam == Team.Spectator)
			{
				text3 = "<link=channel:" + StringUtil.TR("/team", "SlashCommand").Substring(1) + ">" + StringUtil.TR("SpectatorChannel", "Chat") + "</link>";
			}
			else
			{
				text3 = "<link=channel:" + StringUtil.TR("/team", "SlashCommand").Substring(1) + ">" + StringUtil.TR("TeamChannel", "Chat") + "</link>";
			}
			if (message.SenderHandle.IsNullOrEmpty())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return $"<color=#{arg}>{text3}:  {message.Text}</color>";
					}
				}
			}
			if (selfMessage)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return string.Format("<color=#{0}>{1} </color>{2}<color=#{0}>:  {3}</color>", arg, text3, message.SenderHandle, message.Text);
					}
				}
			}
			return $"<color=#{arg}>{text3} [<link=name>{message.SenderHandle}</link>]:  {message.Text}</color>";
		}
		case ConsoleMessageType.GroupChat:
		{
			string arg = ColorToHex(HUD_UIResources.Get().m_GroupChatColor);
			string text3 = "<link=channel:" + StringUtil.TR("/group", "SlashCommand").Substring(1) + ">" + StringUtil.TR("GroupChannel", "Chat") + "</link>";
			string text4;
			if (message.SenderHandle.IsNullOrEmpty())
			{
				text4 = string.Empty;
			}
			else if (selfMessage)
			{
				text4 = message.SenderHandle;
			}
			else
			{
				text4 = " [<link=name>" + message.SenderHandle + "</link>]";
			}
			if (selfMessage)
			{
				return string.Format("<color=#{0}>{1} </color>{2}<color=#{0}>:  {3}</color>", arg, text3, text4, message.Text);
			}
			return $"<color=#{arg}>{text3}{text4}:  {message.Text}</color>";
		}
		case ConsoleMessageType.WhisperChat:
		{
			string text;
			string text2;
			if (selfMessage)
			{
				text = message.RecipientHandle;
				text2 = StringUtil.TR("To", "Chat");
			}
			else
			{
				text = message.SenderHandle;
				text2 = string.Empty;
			}
			string arg = ColorToHex(HUD_UIResources.Get().m_whisperChatColor);
			return $"<color=#{arg}>{text2} [<link=name>{text}</link>]:  {message.Text}</color>";
		}
		case ConsoleMessageType.SystemMessage:
		case ConsoleMessageType.Exception:
		case ConsoleMessageType.BroadcastMessage:
		{
			string arg = ColorToHex(HUD_UIResources.Get().m_systemChatColor);
			return string.Format("<color=#{1}>{0}</color>", message.Text, arg);
		}
		case ConsoleMessageType.Error:
		{
			string arg = ColorToHex(HUD_UIResources.Get().m_systemErrorChatColor);
			return string.Format("<color=#{1}>{0}</color>", message.Text, arg);
		}
		case ConsoleMessageType.CombatLog:
		{
			string arg = ColorToHex(HUD_UIResources.Get().m_combatLogChatColor);
			return string.Format("<color=#{1}>{0}</color>", message.Text, arg);
		}
		case ConsoleMessageType.PingChat:
		case ConsoleMessageType.ScriptedChat:
		{
			string arg = ColorToHex(HUD_UIResources.Get().m_TeamChatColor);
			return string.Format("<color=#{1}>{0}</color>", message.Text, arg);
		}
		case ConsoleMessageType.DiscordLog:
		{
			string arg = ColorToHex(Color.yellow);
			return string.Format("<color=#{1}>{0}</color>", message.Text, arg);
		}
		default:
			return message.Text;
		}
	}

	private static bool ShouldDisplay(TextConsole.Message message)
	{
		if (HUD_UI.Get() != null && message.MessageType == ConsoleMessageType.GlobalChat)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (message.MessageType == ConsoleMessageType.GameChat)
		{
			if (!(Options_UI.Get() == null))
			{
				if (Options_UI.Get().GetShowAllChat())
				{
					goto IL_00dc;
				}
			}
			if (!AppState.IsInGame() && GameManager.Get().GameInfo != null)
			{
				if (GameManager.Get().GameInfo.IsCustomGame)
				{
					if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
					{
						goto IL_00dc;
					}
				}
			}
			return false;
		}
		goto IL_00dc;
		IL_00dc:
		return true;
	}

	public void UpdateGameState()
	{
		ApplyChatAlpha();
		ChangeChatRoom();
		m_theTextList.RefreshTextSizes();
	}

	public static void StoreMessage(TextConsole.Message message, TextConsole.AllowedEmojis allowedEmojis)
	{
		if (!ShouldDisplay(message))
		{
			return;
		}
		while (true)
		{
			if (s_handledMessages == null)
			{
				return;
			}
			while (true)
			{
				s_handledMessages.Enqueue(new HandledMessage
				{
					Message = message,
					AllowedEmojis = allowedEmojis
				});
				if (s_handledMessages.Count > 80)
				{
					s_handledMessages.Dequeue();
				}
				return;
			}
		}
	}

	public void HandleMessage(TextConsole.Message message, TextConsole.AllowedEmojis allowedEmojis)
	{
		if (!ShouldDisplay(message))
		{
			return;
		}
		while (true)
		{
			DisplayMessage(message, allowedEmojis);
			return;
		}
	}

	private TextMeshProUGUI DisplayMessage(TextConsole.Message message, TextConsole.AllowedEmojis allowedEmojis)
	{
		if (HUD_UIResources.Get() == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
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
		string lastWhisperer;
		switch (message.MessageType)
		{
		case ConsoleMessageType.GlobalChat:
			textColor = HUD_UIResources.Get().m_GlobalChatColor;
			forceShowChat = true;
			break;
		case ConsoleMessageType.GameChat:
			textColor = HUD_UIResources.Get().m_GameChatColor;
			forceShowChat = true;
			break;
		case ConsoleMessageType.TeamChat:
			textColor = HUD_UIResources.Get().m_TeamChatColor;
			forceShowChat = true;
			break;
		case ConsoleMessageType.GroupChat:
			textColor = HUD_UIResources.Get().m_GroupChatColor;
			forceShowChat = true;
			break;
		case ConsoleMessageType.WhisperChat:
			textColor = HUD_UIResources.Get().m_whisperChatColor;
			forceShowChat = true;
			if (m_timeSinceLastWhisper + UIFrontEnd.Get().m_whisperSoundThreshold < Time.time)
			{
				goto IL_0168;
			}
			if (flag)
			{
				if (m_lastWhisperer != message.RecipientHandle)
				{
					goto IL_0168;
				}
			}
			if (!flag)
			{
				if (m_lastWhisperer != message.SenderHandle)
				{
					goto IL_0168;
				}
			}
			goto IL_01cd;
		case ConsoleMessageType.BroadcastMessage:
			textColor = HUD_UIResources.Get().m_GlobalChatColor;
			forceShowChat = true;
			break;
		case ConsoleMessageType.SystemMessage:
			textColor = HUD_UIResources.Get().m_GlobalChatColor;
			forceShowChat = true;
			break;
		case ConsoleMessageType.Error:
			text = message.Text;
			textColor = Color.red;
			forceShowChat = true;
			break;
		case ConsoleMessageType.Exception:
			text = message.Text;
			textColor = Color.red;
			forceShowChat = true;
			break;
		default:
			{
				text = message.Text;
				textColor = HUD_UIResources.Get().m_GlobalChatColor;
				break;
			}
			IL_01cd:
			m_timeSinceLastWhisper = Time.time;
			break;
			IL_0168:
			if (flag)
			{
				lastWhisperer = message.RecipientHandle;
			}
			else
			{
				lastWhisperer = message.SenderHandle;
			}
			m_lastWhisperer = lastWhisperer;
			if (!flag)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.WhisperMessage);
			}
			if (!m_whisperedPlayers.Contains(m_lastWhisperer))
			{
				m_whisperedPlayers.Add(m_lastWhisperer);
			}
			goto IL_01cd;
		}
		if (text.Length > 256)
		{
			if (message.MessageType != ConsoleMessageType.SystemMessage)
			{
				text = text.Substring(0, 256);
			}
		}
		text = FormatConsoleMessage(message, flag);
		TextMeshProUGUI result = AddTextEntry(text, textColor, forceShowChat, message, allowedEmojis.emojis);
		TryAddToAutoComplete(message.SenderHandle);
		return result;
	}

	public bool IsVisible()
	{
		return m_visible;
	}

	private void UpdateElementsVisibility()
	{
		if (m_visible)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					chatAlpha.EaseTo(1f);
					if (m_changeChannelAlpha)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								UIManager.SetGameObjectActive(m_chatroomHitbox, true);
								return;
							}
						}
					}
					return;
				}
			}
		}
		chatAlpha.EaseTo(0f);
		if (m_changeChannelAlpha)
		{
			UIManager.SetGameObjectActive(m_chatroomHitbox, false);
		}
		if (!(EmoticonPanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			EmoticonPanel.Get().SetPanelOpen(false);
			return;
		}
	}

	public void OnInputSubmitted()
	{
		string text = m_textInput.text.Trim();
		bool flag2;
		if (!text.IsNullOrEmpty())
		{
			if (!text.StartsWith("/"))
			{
				text = m_chatCommand + " " + text;
			}
			int num;
			if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
			{
				num = ((GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			flag2 = false;
			string[] array = text.Split(' ');
			if (m_globalCommand.IsSlashCommand(array[0]))
			{
				if (Options_UI.Get() != null && !Options_UI.Get().GetShowGlobalChat())
				{
					AddTextEntry(StringUtil.TR("GlobalChatDisabled", "Chat"), HUD_UIResources.Get().m_GlobalChatColor, true);
					flag2 = true;
					goto IL_02ea;
				}
			}
			if (m_allCommand.IsSlashCommand(array[0]) && Options_UI.Get() != null)
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
									goto IL_02ea;
								}
							}
						}
					}
					AddTextEntry(StringUtil.TR("AllChatDisabled", "Chat"), HUD_UIResources.Get().m_GameChatColor, true);
					flag2 = true;
					goto IL_02ea;
				}
			}
			if (m_teamCommand.IsSlashCommand(array[0]))
			{
				if (!flag)
				{
					AddTextEntry(StringUtil.TR("NotInAGame", "Invite"), HUD_UIResources.Get().m_TeamChatColor, true);
					flag2 = true;
					goto IL_02ea;
				}
			}
			if (m_groupCommand.IsSlashCommand(array[0]))
			{
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().GroupInfo != null)
					{
						if (!ClientGameManager.Get().GroupInfo.InAGroup)
						{
							AddTextEntry(StringUtil.TR("NotInAGroup", "Invite"), HUD_UIResources.Get().m_GroupChatColor, true);
							flag2 = true;
						}
					}
				}
			}
			goto IL_02ea;
		}
		goto IL_038d;
		IL_02ea:
		if (!flag2)
		{
			if (TextConsole.Get() != null)
			{
				TextConsole.Get().OnInputSubmitted(text);
			}
			s_history.Add(text);
			m_storedHistory = null;
			m_storedChatCommand = null;
			m_historyIndex = s_history.Count;
			for (int i = 0; i < m_possibilitiesEntries.Count; i++)
			{
				m_theTextList.RemoveEntry(m_possibilitiesEntries[i]);
			}
			m_possibilitiesEntries.Clear();
		}
		goto IL_038d;
		IL_038d:
		m_textInput.text = string.Empty;
		if (!(EventSystem.current.currentSelectedGameObject == m_textInput.gameObject))
		{
			return;
		}
		while (true)
		{
			m_inputJustCleared = true;
			EventSystem.current.SetSelectedGameObject(null);
			setInputSelected = (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd);
			return;
		}
	}

	private void OnToggleClicked(BaseEventData data)
	{
		ToggleVisibility();
	}

	private void ToggleVisibility()
	{
		if (m_visible)
		{
			if (m_chatroomList.IsVisible())
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		if (m_visible)
		{
			if ((double)m_scrollBar.size < 0.9999)
			{
				if ((double)m_scrollBar.value > 0.0001)
				{
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
			}
		}
		m_visible = !m_visible;
		if (m_visible)
		{
			if (UIScreenManager.Get() != null)
			{
				SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
			}
		}
		else
		{
			ClearInputSelect();
			SetTimeTillCollapse(-1f);
		}
		m_theTextList.NotifyVisible(m_visible);
		UpdateElementsVisibility();
	}

	public void Show()
	{
		if (m_visible)
		{
			return;
		}
		while (true)
		{
			ToggleVisibility();
			return;
		}
	}

	public void Hide()
	{
		if (!m_visible)
		{
			return;
		}
		while (true)
		{
			ToggleVisibility();
			return;
		}
	}

	private void OnPointerEnter(BaseEventData data)
	{
		OnTextConsoleHover(true);
	}

	private void OnPointerExit(BaseEventData data)
	{
		OnTextConsoleHover(false);
	}

	public bool IsHovered()
	{
		return m_hovering;
	}

	private void OnInputClicked(BaseEventData data)
	{
		Show();
		m_autocompleteList.SetVisible(false);
	}

	private void OnChatroomClick(BaseEventData data)
	{
		if (m_chatroomList.IsVisible())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_chatroomList.SetVisible(false);
					return;
				}
			}
		}
		m_chatroomList.Setup(this);
		m_chatroomList.SetVisible(true);
	}

	private void OnTextConsoleHover(bool hover)
	{
		m_hovering = hover;
		if (hover)
		{
			return;
		}
		while (true)
		{
			if (UIScreenManager.Get() != null)
			{
				while (true)
				{
					SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
					return;
				}
			}
			return;
		}
	}

	private void OnDragOver(GameObject go)
	{
		m_hovering = true;
	}

	private void OnDragOut(GameObject go)
	{
		m_hovering = false;
		if (m_hovering)
		{
			return;
		}
		while (true)
		{
			SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
			return;
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
		if (m_chatRoomName == null || !m_textInput.text.IsNullOrEmpty())
		{
			return;
		}

		m_chatroomList.SetVisible(false);
		m_hasGame = GameManager.Get() != null
		            && GameManager.Get().GameInfo != null
		            && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;
		bool isInGroup = ClientGameManager.Get() != null
		                 && ClientGameManager.Get().GroupInfo != null
		                 && ClientGameManager.Get().GroupInfo.InAGroup;
		if ((!m_hasGame && (m_teamCommand.IsSlashCommand(m_chatCommand) || m_allCommand.IsSlashCommand(m_chatCommand)))
		    || (m_globalCommand.IsSlashCommand(m_chatCommand) && (isInGroup || m_hasGame))
		    || (m_groupCommand.IsSlashCommand(m_chatCommand) && (!isInGroup || m_hasGame)))
		{
			m_chatCommand = null;
		}

		SetChatRoom();
	}

	private void SetChatRoom(string chatText = null)
	{
		if (m_chatRoomName == null)
		{
			return;
		}

		if (chatText == null)
		{
			chatText = m_chatText.text;
		}

		bool hasGame = GameManager.Get() != null
		               && GameManager.Get().GameInfo != null
		               && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;
		bool isInGroup = ClientGameManager.Get() != null
		                 && ClientGameManager.Get().GroupInfo != null
		                 && ClientGameManager.Get().GroupInfo.InAGroup;

		string[] parts = chatText.Split((string[])null, 3, StringSplitOptions.RemoveEmptyEntries);

		if (parts.Length > 1
		    || (chatText.EndsWith(" ") && !chatText.Trim().IsNullOrEmpty()))
		{
			foreach (SlashCommand command in m_chatCommands)
			{
				if (!command.IsSlashCommand(parts[0]))
				{
					continue;
				}

				if (command == m_whisperCommand)
				{
					if (parts.Length >= 3 || parts.Length >= 2 && chatText.EndsWith(" "))
					{
						parts = chatText.Split((string[])null, 3, StringSplitOptions.RemoveEmptyEntries);
						m_chatCommand = command.Command + " " + parts[1];
						m_textInput.text = parts.Length >= 3 ? parts[2] : string.Empty;
					}
				}
				else if (command != m_allCommand
				         || !AppState.IsInGame()
				         && GameManager.Get().GameInfo != null
				         && GameManager.Get().GameInfo.IsCustomGame
				         && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped
				         || Options_UI.Get() != null
				         && Options_UI.Get().GetShowAllChat())
				{
					if (!hasGame
					    && (command == m_teamCommand || command == m_allCommand))
					{
						AddTextEntry(
							StringUtil.TR("NotInAGame", "Invite"),
							HUD_UIResources.Get().m_TeamChatColor,
							true);
						chatText = string.Empty;
						for (int i = 1; i < parts.Length; i++)
						{
							chatText = chatText.IsNullOrEmpty()
								? parts[i]
								: chatText + " " + parts[i];
						}

						m_textInput.text = chatText;
					}
					else if (command == m_groupCommand && !isInGroup)
					{
						AddTextEntry(
							StringUtil.TR("NotInAGroup", "Invite"),
							HUD_UIResources.Get().m_GroupChatColor,
							true);
						chatText = string.Empty;
						for (int i = 1; i < parts.Length; i++)
						{
							chatText = !chatText.IsNullOrEmpty() ? chatText + " " + parts[i] : parts[i];
						}

						m_textInput.text = chatText;
					}
					else
					{
						m_chatCommand = command.Command;
						if (parts.Length > 1)
						{
							chatText = parts[1];
							if (parts.Length > 2)
							{
								chatText = chatText + " " + parts[2];
							}

							m_textInput.text = chatText;
						}
						else
						{
							m_textInput.text = string.Empty;
						}
					}
				}

				break;
			}
		}

		if (m_chatCommand.IsNullOrEmpty())
		{
			m_chatCommand = m_hasGame
				? StringUtil.TR("/team", "SlashCommand")
				: isInGroup
					? StringUtil.TR("/group", "SlashCommand")
					: StringUtil.TR("/general", "SlashCommand");
		}

		RefreshChatRoomDisplay();
	}

	private void RefreshChatRoomDisplay()
	{
		string command = m_chatCommand;
		int num = m_chatCommand.IndexOf(' ');
		if (num > 0)
		{
			command = m_chatCommand.Substring(0, num);
		}

		if (m_allCommand.IsSlashCommand(command))
		{
			m_chatRoomName.text = StringUtil.TR("GameChannel", "Chat");
		}
		else if (m_globalCommand.IsSlashCommand(command))
		{
			m_chatRoomName.text = StringUtil.TR("GlobalChannel", "Chat");
		}
		else if (m_groupCommand.IsSlashCommand(command))
		{
			m_chatRoomName.text = StringUtil.TR("GroupChannel", "Chat");
		}
		else if (m_teamCommand.IsSlashCommand(command))
		{
			if (GameManager.Get() != null
			    && GameManager.Get().PlayerInfo != null
			    && GameManager.Get().PlayerInfo.TeamId != Team.Spectator)
			{
				m_chatRoomName.text = StringUtil.TR("TeamChannel", "Chat");
			}
			else
			{
				m_chatRoomName.text = StringUtil.TR("SpectatorChannel", "Chat");
			}
		}
		else if (m_whisperCommand.IsSlashCommand(command))
		{
			m_chatRoomName.text = m_chatCommand.Substring(command.Length);
		}
		else
		{
			m_chatRoomName.text = "[" + m_chatCommand.Substring(1, 1).ToUpper() + m_chatCommand.Substring(2) + "]";
		}

		m_chatRoomName.text += ":";

		AlignChatText();
		Color color = Color.white;
		if (HUD_UIResources.Get() != null)
		{
			if (m_chatCommand == StringUtil.TR("/group", "SlashCommand"))
			{
				color = HUD_UIResources.Get().m_GroupChatColor;
			}
			else if (m_chatCommand == StringUtil.TR("/general", "SlashCommand"))
			{
				color = HUD_UIResources.Get().m_GlobalChatColor;
			}
			else if (m_chatCommand == StringUtil.TR("/game", "SlashCommand")
			         || m_chatCommand == StringUtil.TR("/game", "SlashCommandAlias1"))
			{
				color = HUD_UIResources.Get().m_GameChatColor;
			}
			else if (m_chatCommand.StartsWith(StringUtil.TR("/whisper", "SlashCommand")))
			{
				color = HUD_UIResources.Get().m_whisperChatColor;
			}
			else if (m_chatCommand == StringUtil.TR("/team", "SlashCommand"))
			{
				color = HUD_UIResources.Get().m_TeamChatColor;
			}
			else
			{
				color = Color.white;
			}
		}

		m_chatRoomName.color = color;

		if (m_changeChannelAlpha)
		{
			m_chatRoomName.color = new Color(
				m_chatRoomName.color.r,
				m_chatRoomName.color.g,
				m_chatRoomName.color.b,
				chatAlpha);
		}
	}

	private void UnsetChatRoom()
	{
		if (m_chatRoomName == null)
		{
			return;
		}

		m_chatRoomName.text = string.Empty;
		AlignChatText();
	}

	private void AlignChatText()
	{
		m_chatRoomName.CalculateLayoutInputHorizontal();
		float preferredWidth = m_chatRoomName.preferredWidth;
		Vector2 offsetMin = m_chatRoomName.rectTransform.offsetMin;
		float num = preferredWidth + offsetMin.x;
		float num2 = num;
		Vector3 localScale = m_chatRoomName.rectTransform.localScale;
		num = num2 * localScale.x;
		RectTransform rectTransform = m_textInput.transform as RectTransform;
		float x = num;
		Vector2 offsetMin2 = rectTransform.offsetMin;
		rectTransform.offsetMin = new Vector2(x, offsetMin2.y);
	}

	private string ColorToHex(Color color)
	{
		string empty = string.Empty;
		empty += ((int)(color.r * 255f)).ToString("X2");
		empty += ((int)(color.g * 255f)).ToString("X2");
		return empty + ((int)(color.b * 255f)).ToString("X2");
	}

	public void DehighlightTextAndPositionCarat()
	{
		MoveCaretToEnd();
	}

	public void ChangeChannel(string channelName)
	{
		m_textInput.text = "/" + channelName + " " + m_textInput.text;
		RefreshChatRoomDisplay();
		SelectInput(string.Empty);
		MoveCaretToEnd();
	}

	private void SetCaretToLastKnownPosition()
	{
		UpdateCaretPosition(m_lastCaratPosition);
	}

	private void MoveCaretToEnd()
	{
		UpdateCaretPosition(-1);
	}

	private void UpdateCaretPosition(int position)
	{
		m_updateCaret = true;
		m_caretPositionToUpdate = position;
	}

	private void OnGameInfoNotification(GameInfoNotification notification)
	{
		RefreshChatRoomDisplay();
	}

	public void IgnoreNextTextChange()
	{
		m_ignoreNextTypeInput = true;
	}
}
