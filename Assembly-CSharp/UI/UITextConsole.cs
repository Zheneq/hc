using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using LobbyGameClientMessages;
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

    private bool m_changeChannelAlpha => UIManager.Get().CurrentState == UIManager.ClientState.InGame;

    private static void InitializeAutoComplete()
    {
        if (m_isAutoCompleteInitialized)
        {
            return;
        }

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
            foreach (FriendInfo friend in ClientGameManager.Get().FriendList.Friends.Values)
            {
                if (friend.FriendStatus == FriendStatus.Friend)
                {
                    TryAddToAutoComplete(friend.FriendHandle);
                }
            }
        }

        m_isAutoCompleteInitialized = true;
    }

    private void Start()
    {
        m_autocompleteList.SetVisible(false);
        m_chatroomList.SetVisible(false);
        InitializeAutoComplete();
        UIEventTriggerUtils.AddListener(gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
        UIEventTriggerUtils.AddListener(gameObject, EventTriggerType.PointerExit, OnPointerExit);
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
        
        foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
        {
            canvasGroup.blocksRaycasts = true;
        }

        foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
        {
            canvasGroup.blocksRaycasts = blockingRaycasts;
        }

        if (m_newTextGlow != null)
        {
            UIManager.SetGameObjectActive(m_newTextGlow, false);
        }

        m_storedHistory = null;
        m_storedChatCommand = null;
        m_historyIndex = s_history.Count;

        foreach (HandledMessage handledMessage in s_handledMessages.ToArray())
        {
            DisplayMessage(handledMessage.Message, handledMessage.AllowedEmojis);
        }

        m_theTextList.HideRecentText();
        if (m_changeChannelAlpha && m_startAlpha == 0f)
        {
            UIManager.SetGameObjectActive(m_chatroomHitbox, false);
        }

        ChangeChatRoom();
        if (ClientGameManager.Get() != null)
        {
            ClientGameManager clientGameManager = ClientGameManager.Get();
            clientGameManager.OnFriendStatusNotification += HandleFriendStatusNotification;
            clientGameManager.OnGroupUpdateNotification += OnGroupUpdateNotification;
            clientGameManager.OnGameInfoNotification += OnGameInfoNotification;
        }

        RebuildLocalizedText();
        LocalizationManager.OnLocalizeEvent += RebuildLocalizedText;
    }

    private void RebuildLocalizedText()
    {
        foreach (SlashCommand command in m_chatCommands)
        {
            command.Localize();
        }

        foreach (SlashCommand command in m_playerCommands)
        {
            command.Localize();
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
            foreach (SlashCommand command in SlashCommands.Get().m_slashCommands)
            {
                if (!command.PublicFacing && !ClientGameManager.Get().HasDeveloperAccess())
                {
                    continue;
                }

                if (command.AvailableInFrontEnd)
                {
                    m_frontendAutocomplete.Add(command.Command);
                }

                if (command.AvailableInGame)
                {
                    m_inGameAutocomplete.Add(command.Command);
                }

                if (command.Aliases != null)
                {
                    foreach (string alias in command.Aliases)
                    {
                        if (command.AvailableInFrontEnd)
                        {
                            m_frontendAutocomplete.Add(alias);
                        }

                        if (command.AvailableInGame)
                        {
                            m_inGameAutocomplete.Add(alias);
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
        if ((float)chatAlpha >= 0.5)
        {
            m_scrollRect.OnScroll((PointerEventData)data);
        }
    }

    private static void HandleFriendStatusNotification(FriendStatusNotification notification)
    {
        if (notification.FriendList.IsError)
        {
            TextConsole.Get().Write("Friends list temporarily unavailable", ConsoleMessageType.Error);
            return;
        }

        foreach (FriendInfo friend in notification.FriendList.Friends.Values)
        {
            if (friend.FriendStatus == FriendStatus.Friend)
            {
                TryAddToAutoComplete(friend.FriendHandle);
            }
        }
    }

    public static void AddToTeamMatesToAutoComplete(LobbyTeamInfo lobbyTeamInfo)
    {
        foreach (LobbyPlayerInfo player in lobbyTeamInfo.TeamAPlayerInfo)
        {
            if (player.IsNPCBot && !player.BotsMasqueradeAsHumans)
            {
                continue;
            }

            TryAddToAutoComplete(player.Handle);
        }

        foreach (LobbyPlayerInfo player in lobbyTeamInfo.TeamBPlayerInfo)
        {
            if (player.IsNPCBot && !player.BotsMasqueradeAsHumans)
            {
                continue;
            }

            TryAddToAutoComplete(player.Handle);
        }
    }

    private static bool TryAddToAutoComplete(string handle)
    {
        if (handle.IsNullOrEmpty() || handle == ClientGameManager.Get().Handle)
        {
            return false;
        }
        
        int num = m_playerAutocomplete.BinarySearch(handle, m_caseInsenitiveComparer);
        if (num < 0)
        {
            m_playerAutocomplete.Insert(~num, handle);
            return true;
        }

        if (num != 0)
        {
            return false;
        }
        
        if (m_playerAutocomplete.Count != 0 && handle.EqualsIgnoreCase(m_playerAutocomplete[0]))
        {
            return false;
        }

        m_playerAutocomplete.Insert(0, handle);
        return true;
    }

    private static void OnGroupUpdateNotification()
    {
        if (!ClientGameManager.Get().GroupInfo.InAGroup)
        {
            return;
        }

        List<UpdateGroupMemberData> members = ClientGameManager.Get().GroupInfo.Members;
        foreach (UpdateGroupMemberData member in members)
        {
            TryAddToAutoComplete(member.MemberHandle);
        }
    }

    public void AddHandleMessage()
    {
        if (TextConsole.Get() == null)
        {
            return;
        }

        if (!m_handlingMessages)
        {
            m_handlingMessages = true;
            TextConsole.Get().OnMessage += HandleMessage;
        }
    }

    public void RemoveHandleMessage()
    {
        if (TextConsole.Get() == null)
        {
            return;
        }

        if (m_handlingMessages)
        {
            m_handlingMessages = false;
            TextConsole.Get().OnMessage -= HandleMessage;
        }
    }

    private void OnEnable()
    {
        blockingRaycasts = false;
        if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
        {
            foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
            {
                canvasGroup.blocksRaycasts = blockingRaycasts;
            }

            foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
            {
                canvasGroup.blocksRaycasts = true;
            }
        }
        else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
        {
            foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
            {
                canvasGroup.blocksRaycasts = true;
            }

            foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
            {
                canvasGroup.blocksRaycasts = blockingRaycasts;
            }
        }

        // TODO CLIENT BUG? duplicated
        if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
        {
            foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
            {
                canvasGroup.blocksRaycasts = blockingRaycasts;
            }

            foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
            {
                canvasGroup.blocksRaycasts = true;
            }
        }
        else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
        {
            foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
            {
                canvasGroup.blocksRaycasts = true;
            }

            foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
            {
                canvasGroup.blocksRaycasts = blockingRaycasts;
            }
        }
    }

    private void OnDestroy()
    {
        if (ClientGameManager.Get() == null)
        {
            return;
        }

        ClientGameManager.Get().OnFriendStatusNotification -= HandleFriendStatusNotification;
        ClientGameManager.Get().OnGroupUpdateNotification -= OnGroupUpdateNotification;
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
                    m_textInput.text = m_textInput.text.Substring(0, m_textInput.caretPosition)
                                       + m_textInput.text.Substring(m_textInput.caretPosition + 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                m_autocompleteList.SetVisible(false);
                return;
            }
        }

        if (m_textInput.text.Length > c_maxTextSendLength)
        {
            m_textInput.text = m_textInput.text.Substring(0, c_maxTextSendLength);
        }

        if (m_textInput.text.StartsWith(StringUtil.TR("/reply", "SlashCommand") + " ")
            || m_textInput.text.StartsWith(StringUtil.TR("/reply", "SlashCommandAlias1") + " "))
        {
            int num = m_textInput.text.IndexOf(' ');
            m_textInput.text = GenerateReplyPrefix() + m_textInput.text.Substring(num + 1);
            MoveCaretToEnd();
        }

        if (setChatRoom)
        {
            SetChatRoom(m_textInput.text);
        }

        if (Input.GetKey(KeyCode.Delete))
        {
            return;
        }

        List<string> autoCompletePossibilities = GetAutoCompletePossibilities(false, out string beforeAutocomplete);
        int length = Mathf.Clamp(m_textInput.caretPosition, 0, m_textInput.text.Length);
        if (autoCompletePossibilities.Count == 1
            && m_textInput.text.Substring(0, length).EndsWith(autoCompletePossibilities[0].Trim()))
        {
            m_autocompleteList.SetVisible(false);
            return;
        }

        m_autocompleteList.Setup(this, autoCompletePossibilities, beforeAutocomplete);
        m_autocompleteList.SetVisible(autoCompletePossibilities.Count > 0);
    }

    public void OnEndEdit(string textString)
    {
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            return;
        }

        if (m_autocompleteList.IsVisible())
        {
            m_textInput.caretPosition = m_lastCaratPosition;
            m_autocompleteList.SelectCurrent();
            return;
        }

        m_lastCaratPosition = -1;
        OnInputSubmitted();
        bool isInGame = GameManager.Get() != null
                        && UIManager.Get().CurrentState == UIManager.ClientState.InGame;
        if (isInGame && m_timeTillCollapse <= 0f)
        {
            ToggleVisibility();
        }
    }

    private void ApplyChatAlpha()
    {
        if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
        {
            if (m_inGameFadingImages != null)
            {
                foreach (Image image in m_inGameFadingImages)
                {
                    if (image != null)
                    {
                        Color color = image.color;
                        color.a = chatAlpha;
                        image.color = color;
                    }
                }
            }

            if (m_frontEndFadingImages != null)
            {
                foreach (Image image in m_frontEndFadingImages)
                {
                    if (image != null)
                    {
                        Color color = image.color;
                        color.a = 0f;
                        image.color = color;
                        image.raycastTarget = false;
                    }
                }
            }
        }
        else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
        {
            if (m_inGameFadingImages != null)
            {
                for (int i = 0; i < m_inGameFadingImages.Length; i++)
                {
                    if (m_inGameFadingImages[i] != null)
                    {
                        Color color = m_inGameFadingImages[i].color;
                        color.a = 1f;
                        m_inGameFadingImages[i].color = color;
                        m_frontEndFadingImages[i].raycastTarget = true;
                    }
                }
            }

            if (m_frontEndFadingImages != null)
            {
                foreach (Image image in m_frontEndFadingImages)
                {
                    if (image != null)
                    {
                        Color color = image.color;
                        color.a = chatAlpha;
                        image.color = color;
                    }
                }
            }
        }

        if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
        {
            foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
            {
                canvasGroup.alpha = chatAlpha;
            }

            foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
            {
                canvasGroup.alpha = 1f;
            }
        }
        else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
        {
            foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
            {
                canvasGroup.alpha = 1f;
            }

            foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
            {
                canvasGroup.alpha = chatAlpha;
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
                foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
                {
                    canvasGroup.blocksRaycasts = true;
                }

                foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
                {
                    canvasGroup.blocksRaycasts = blockingRaycasts;
                }
            }
            else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
            {
                foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
                {
                    canvasGroup.blocksRaycasts = true;
                }

                foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
                {
                    canvasGroup.blocksRaycasts = blockingRaycasts;
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
                    foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
                    {
                        canvasGroup.blocksRaycasts = true;
                    }

                    foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
                    {
                        canvasGroup.blocksRaycasts = blockingRaycasts;
                    }
                }
                else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
                {
                    foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
                    {
                        canvasGroup.blocksRaycasts = true;
                    }

                    foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
                    {
                        canvasGroup.blocksRaycasts = blockingRaycasts;
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

        m_chatRoomName.color = new Color(
            m_chatRoomName.color.r,
            m_chatRoomName.color.g,
            m_chatRoomName.color.b,
            chatAlpha);
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
        if (EventSystem.current != null
            && EventSystem.current.currentSelectedGameObject == m_textInput.gameObject)
        {
            m_inputJustCleared = true;
            EventSystem.current.SetSelectedGameObject(null);
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

        if (startingInput != string.Empty)
        {
            m_textInput.text = startingInput;
        }
    }

    public void SetTimeTillCollapse(float time)
    {
        if (!m_doNotAutoFade)
        {
            m_timeTillCollapse = time;
        }
    }

    private void LateUpdate()
    {
        if (m_visible)
        {
            UIManager.SetGameObjectActive(
                m_textInput.placeholder,
                EventSystem.current == null || EventSystem.current.currentSelectedGameObject != m_textInput.gameObject);
        }
        else
        {
            bool flag = false;
            if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
            {
                foreach (CanvasGroup canvasGroup in m_inGameCanvasGroups)
                {
                    if (canvasGroup.gameObject == m_textInput.gameObject)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
            {
                foreach (CanvasGroup canvasGroup in m_frontEndCanvasGroups)
                {
                    if (canvasGroup.gameObject == m_textInput.gameObject)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            UIManager.SetGameObjectActive(m_textInput.placeholder, !flag);
        }

        m_textInput.placeholder.GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
        if (EventSystem.current.currentSelectedGameObject == m_textInput.gameObject)
        {
            if (m_updateCaret)
            {
                if (m_caretPositionToUpdate >= 0 && m_caretPositionToUpdate < m_textInput.text.Length)
                {
                    if (m_textInput.caretPosition == m_caretPositionToUpdate
                        && m_textInput.selectionAnchorPosition == m_caretPositionToUpdate)
                    {
                        m_updateCaret = false;
                    }
                    else
                    {
                        m_textInput.caretPosition = m_caretPositionToUpdate;
                        m_textInput.selectionAnchorPosition = m_caretPositionToUpdate;
                    }
                }
                else if (m_textInput.caretPosition >= m_textInput.text.Length
                         && m_textInput.selectionAnchorPosition >= m_textInput.text.Length)
                {
                    m_updateCaret = false;
                }
                else
                {
                    m_textInput.MoveTextEnd(false);
                }
            }
            else
            {
                m_lastCaratPosition = m_textInput.caretPosition;
            }
        }

        bool flag2 = false;
        if (m_visible)
        {
            if (Input.GetMouseButtonDown(0))
            {
                flag2 = true;
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
                {
                    StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject
                        .GetComponent<StandaloneInputModuleWithEventDataAccess>();
                    if (component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<UITextConsole>()
                        || component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<EmoticonPanel>())
                    {
                        flag2 = false;
                    }
                }
            }
        }

        if (!flag2)
        {
            if (m_timeTillCollapse > 0f
                || !m_visible
                || m_hovering
                || IsTextInputFocused(true))
            {
                return;
            }
        }

        if (!IsPressedAndMousedOver() && !EmoticonPanel.Get().IsPanelOpen())
        {
            ToggleVisibility();
        }
    }

    private bool CheckInputField()
    {
        if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null)
        {
            return true;
        }

        TMP_InputField input = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        if (input == null)
        {
            return true;
        }

        if (input != m_textInput)
        {
            string text = "ENTER INPUT FALSE: " + input.name;
            Transform elem = input.transform;
            while (elem.parent != null)
            {
                text += " -> " + elem.parent.name;
                elem = elem.parent;
            }

            Log.Info(text);
        }

        return input == m_textInput;
    }

    private void Update()
    {
        bool pressedEnter = (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                            && CheckInputField();
        bool pressedSlash = Input.GetKeyDown(KeyCode.Slash)
                            && EventSystem.current != null
                            && EventSystem.current.currentSelectedGameObject != m_textInput.gameObject;
        bool pressedChatReply = InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ChatReply)
                                && (DebugParameters.Get() == null
                                    || !DebugParameters.Get().GetParameterAsBool("DebugCamera"));
        bool pressedChatAll = InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ChatAll);
        bool pressedUp = m_textInput.isFocused
                         && Input.GetKeyDown(KeyCode.UpArrow);
        bool pressedDown = m_textInput.isFocused
                           && Input.GetKeyDown(KeyCode.DownArrow);
        bool pressedTextNavigation = m_textInput.isFocused
                                     && (Input.GetKeyDown(KeyCode.RightArrow)
                                         || Input.GetKeyDown(KeyCode.LeftArrow)
                                         || Input.GetKeyDown(KeyCode.Home)
                                         || Input.GetKeyDown(KeyCode.End));
        bool pressedTab = m_textInput.isFocused
                          && Input.GetKeyDown(KeyCode.Tab);
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
            else if (pressedTextNavigation)
            {
                m_autocompleteList.SetVisible(false);
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
                      && m_lastIsTextInputNotSelected
                      && (EmoticonPanel.Get() == null 
                          || !EmoticonPanel.Get().IsPanelOpen()
                          && EmoticonPanel.Get().m_emoticonBtn.spriteController.gameObject
                          != EventSystem.current.currentSelectedGameObject))
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
            float nextAlertTimeHours = 0f;
            if (!m_hadNextAlertTime)
            {
                float hours = alertMissionsData.ReminderHours.DefaultIfEmpty().Max();
                if (hours == 0f || alertMissionsData.NextAlert.Value.AddHours(0f - hours) <= dateTime)
                {
                    nextAlertTimeHours = (float)(alertMissionsData.NextAlert.Value - dateTime).TotalHours;
                }
            }
            else if (!alertMissionsData.ReminderHours.IsNullOrEmpty())
            {
                foreach (float hours in alertMissionsData.ReminderHours)
                {
                    DateTime t = alertMissionsData.NextAlert.Value.AddHours(0f - hours);
                    if (t > m_lastSystemMessageCheckPST && t <= dateTime)
                    {
                        nextAlertTimeHours = hours;
                    }
                }
            }

            if (nextAlertTimeHours > 0f)
            {
                TextConsole.Get().Write(
                    string.Format(
                        StringUtil.TR("NextAlertIn", "Global"),
                        StringUtil.GetTimeDifferenceText(TimeSpan.FromHours(nextAlertTimeHours), true)));
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
        return !m_textInput.isFocused || m_textInput.text == string.Empty;
    }

    public bool IsTextInputFocused(bool checkEmoticonPanel = false)
    {
        if (checkEmoticonPanel
            && EmoticonPanel.Get() != null
            && EmoticonPanel.Get().IsPanelOpen())
        {
            return true;
        }

        return EventSystem.current != null
               && EventSystem.current.currentSelectedGameObject != null
               && (m_textInput.gameObject == EventSystem.current.currentSelectedGameObject
                   || m_scrollBar.gameObject == EventSystem.current.currentSelectedGameObject);
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

        PointerEventData mousePointer = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(mousePointer, raycastResults);
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.GetComponentInParent<UITextConsole>() != null)
            {
                return true;
            }
        }

        return false;
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

        if (text.IsNullOrEmpty() || text.Length < 1 || text == "/" || list2 == null)
        {
            return list;
        }

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

        if (num2 >= list2.Count || !list2[num2].ToLower().StartsWith(text))
        {
            return list;
        }

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

        string text4;
        if (num6 == num2)
        {
            text4 = list2[num2] + " ";
            if (text != text4
                && (!m_allCommand.IsSlashCommand(text4.Trim())
                    || (Options_UI.Get() != null
                        && Options_UI.Get().GetShowAllChat())
                    || (!AppState.IsInGame()
                        && GameManager.Get().GameInfo != null
                        && GameManager.Get().GameInfo.IsCustomGame
                        && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)))
            {
                list.Add(text4);
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
                    text4 += text5[k];
                    continue;
                }

                break;
            }

            for (int l = num2; l <= num6; l++)
            {
                if (m_allCommand.IsSlashCommand(list2[l].Trim())
                    && !(Options_UI.Get() != null && Options_UI.Get().GetShowAllChat()))
                {
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

                list.Add(list2[l]);
            }
        }

        if (doAutocomplete)
        {
            string text7 = text4 + text3.TrimStart();
            int num7 = !text4.IsNullOrEmpty() ? text4.Length : 0;
            if (!beforeAutocomplete.IsNullOrEmpty())
            {
                text7 = beforeAutocomplete + " " + text7;
                num7 += beforeAutocomplete.Length + 1;
            }

            if (m_textInput.text != text7)
            {
                m_textInput.text = text7;
                UpdateCaretPosition(num7);
                OnTypeInput(m_textInput.text, false);
            }
        }

        return list;
    }

    public void ScrollBarAfterNewChat()
    {
        if (!m_scrollChat)
        {
            return;
        }

        if (m_scrollRect.verticalScrollbar.value != 0f)
        {
            m_scrollRect.verticalScrollbar.value = 0f;
            m_scrollChat = false;
        }
    }

    public TextMeshProUGUI AddTextEntry(
        string textEntry,
        Color textColor,
        bool forceShowChat,
        TextConsole.Message messageInfo,
        List<int> allowedEmojis = null)
    {
        CharacterType characterType = CharacterType.None;
        if (messageInfo.MessageType == ConsoleMessageType.GlobalChat)
        {
            if (GameFlowData.Get() != null)
            {
                return null;
            }

            if (Options_UI.Get() != null && !Options_UI.Get().GetShowGlobalChat())
            {
                return null;
            }

            if (AppState.GetCurrent() == AppState_RankModeDraft.Get())
            {
                return null;
            }
        }

        if (GameFlowData.Get() != null)
        {
            characterType = messageInfo.CharacterType;
        }

        RectTransform rectTransform = m_theTextList.transform as RectTransform;
        float value = m_scrollRect.verticalScrollbar.value;
        float size = m_scrollRect.verticalScrollbar.size;
        float y3 = rectTransform.sizeDelta.y;
        if (UIManager.Get().CurrentState != 0 && characterType != 0 && messageInfo.SenderTeam != Team.Spectator)
        {
            int teamShift = 0;
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
                    teamShift = messageInfo.SenderTeam != team ? 1 : 0;
                }
                else if (messageInfo.SenderTeam == Team.TeamA)
                {
                    teamShift = 0;
                }
                else
                {
                    teamShift = 1;
                }
            }

            textEntry =
                $"<size=36><sprite=\"CharacterSprites\" index={2 * (int)characterType + teamShift}>\u200b</size>{textEntry}";
        }

        TextMeshProUGUI textMeshProUGUI = m_theTextList.AddEntry(
            textEntry,
            textColor,
            forceShowChat,
            HUD_UIResources.Get().m_textPaddingAmount,
            m_scrollRect,
            allowedEmojis);
        textMeshProUGUI.CalculateLayoutInputVertical();
        float num = textMeshProUGUI.preferredHeight + HUD_UIResources.Get().m_textPaddingAmount;
        if (m_theTextList.NumEntires() >= 2 && !(num >= y3 * value))
        {
            m_scrollChat = size >= 0.999f;
        }
        else
        {
            m_scrollChat = true;
        }

        if (!m_scrollChat)
        {
            Vector3 localPosition = rectTransform.localPosition;
            localPosition.y -= num * (1f - rectTransform.pivot.y);
            if (localPosition.y < 0f - rectTransform.sizeDelta.y)
            {
                localPosition.y = 0f - rectTransform.sizeDelta.y;
            }

            rectTransform.localPosition = localPosition;
        }

        UIEventTriggerUtils.AddListener(textMeshProUGUI.gameObject, EventTriggerType.Scroll, OnScroll);
        return textMeshProUGUI;
    }

    public TextMeshProUGUI AddTextEntry(string textEntry, Color textColor, bool forceShowChat)
    {
        RectTransform rectTransform = m_theTextList.transform as RectTransform;
        float value = m_scrollRect.verticalScrollbar.value;
        float size = m_scrollRect.verticalScrollbar.size;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float y = sizeDelta.y;
        TextMeshProUGUI textMeshProUGUI = m_theTextList.AddEntry(
            textEntry,
            textColor,
            forceShowChat,
            HUD_UIResources.Get().m_textPaddingAmount,
            m_scrollRect);
        HUDTextConsoleItem component = textMeshProUGUI.GetComponent<HUDTextConsoleItem>();
        if (component != null)
        {
            UIManager.SetGameObjectActive(component.m_iconContainer, false);
        }

        m_checkForNextTextGlow = true;
        textMeshProUGUI.CalculateLayoutInputVertical();
        float num = textMeshProUGUI.preferredHeight + HUD_UIResources.Get().m_textPaddingAmount;
        if (m_theTextList.NumEntires() >= 2 && !(num >= y * value))
        {
            m_scrollChat = size >= 0.999f;
        }
        else
        {
            m_scrollChat = true;
        }

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
#if EVOS
        string empty = string.Empty;
        string devtag = string.Empty;
        string mentorTag = string.Empty;
        string tag = string.Empty;

        if (message.DisplayDevTag)
        {
            devtag = " <color=red>[Dev]</color>";
        }

        // Defensive: ensure SenderHandle isn't null before using string operations
        if (message.SenderHandle == null)
        {
            message.SenderHandle = string.Empty;
        }

        if (!message.SenderHandle.IsNullOrEmpty() && message.SenderHandle.StartsWith("<size=24><sprite=2></size>"))
        {
            message.SenderHandle = message.SenderHandle.Replace("<size=24><sprite=2></size>", "");
            mentorTag = " <color=orange>[Mentor]</color>";
        }

        try
        {
            var manager = SpecialEffectsManager.GetInstance();
            string handle = message.SenderHandle ?? string.Empty;

            string tagHandle = null;
            if (manager != null)
            {
                tagHandle = manager.GetEffectForHandle(handle);
            }
            else
            {
                tagHandle = null;
            }

            if (!string.IsNullOrEmpty(tagHandle))
            {
                // tagHandle may contain multiple entries joined by ", ". Check contains for each special case.
                if (tagHandle.IndexOf("MVP", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    tag = " <color=#9e6bff>[MVP]</color>";
                }

                if (tagHandle.IndexOf("Nitro", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    tag = $" <color=#a9c9ff>[Nitro]</color>";
                }

                if (tagHandle.IndexOf("Special", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    tag = " <color=#ffb400>[VIP]</color>";
                    if (tagHandle.IndexOf("MVP", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        tag = " <color=#9e6bff>[VIP+]</color>";
                    }

                    if (tagHandle.IndexOf("Nitro", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        tag = $" <color=#a9c9ff>[VIP++]</color>";
                    }
                }

                if (tagHandle.IndexOf("TournamentWinners", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    tag = $" <color=#e91e63>[Champion]</color>";
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("SpecialEffectsManager lookup failed: " + ex);
            tag = string.Empty;
        }
#else
        if (message.DisplayDevTag)
        {
            message.SenderHandle = StringUtil.TR("DevTag", "Global") + message.SenderHandle;
        }
#endif

        switch (message.MessageType)
        {
            case ConsoleMessageType.GlobalChat:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_GlobalChatColor);
                string channel = "<link=channel:"
                               + StringUtil.TR("/general", "SlashCommand").Substring(1)
                               + ">"
                               + StringUtil.TR("GlobalChannel", "Chat")
                               + "</link>";
#if EVOS
	            return selfMessage
		            ? $"<color=#{color}>{channel}{devtag}{tag}{mentorTag} {message.SenderHandle}:  {message.Text}</color>"
		            : $"<color=#{color}>{channel}{devtag}{tag} {mentorTag} [<link=name>{message.SenderHandle}</link>]:  {message.Text}</color>";
#else 
                return selfMessage
                    ? $"<color=#{color}>{channel} {message.SenderHandle}:  {message.Text}</color>"
                    : $"<color=#{color}>{channel} [<link=name>{message.SenderHandle}</link>]:  {message.Text}</color>";
#endif
            }
            case ConsoleMessageType.GameChat:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_GameChatColor);
#if EVOS
	            return selfMessage
		            ? $"<color=#{color}>{StringUtil.TR("GameChannel", "Chat")}{devtag}{tag}{mentorTag} {message.SenderHandle}<color=#{color}>: {message.Text}</color>"
		            : $"<color=#{color}>{StringUtil.TR("GameChannel", "Chat")}{devtag}{tag}{mentorTag} <link=name>{message.SenderHandle}</link>: {message.Text}</color>";
#else
                return selfMessage
                    ? string.Format(
                        "<color=#{0}>"
                        + StringUtil.TR("GameChannel", "Chat")
                        + " </color>{1}<color=#{0}>: {2}</color>",
                        color,
                        message.SenderHandle,
                        message.Text)
                    : string.Format(
                        "<color=#{0}>"
                        + StringUtil.TR("GameChannel", "Chat")
                        + " <link=name>{1}</link>: {2}</color>",
                        color,
                        message.SenderHandle,
                        message.Text);
#endif
            }
            case ConsoleMessageType.TeamChat:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_TeamChatColor);
                string channel;
                if (message.SenderTeam == Team.Spectator)
                {
                    channel = "<link=channel:" + StringUtil.TR("/team", "SlashCommand").Substring(1) + ">"
                            + StringUtil.TR("SpectatorChannel", "Chat")
                            + "</link>";
                }
                else
                {
                    channel = "<link=channel:" + StringUtil.TR("/team", "SlashCommand").Substring(1) + ">"
                            + StringUtil.TR("TeamChannel", "Chat") + "</link>";
                }

                if (message.SenderHandle.IsNullOrEmpty())
                {
#if EVOS
	                return $"<color=#{color}>{devtag}{tag}{mentorTag} {channel}:  {message.Text}</color>";
#else 
                    return $"<color=#{color}>{channel}:  {message.Text}</color>";
#endif
                }

#if EVOS
	            return selfMessage
		            ? $"<color=#{color}>{channel}{devtag}{tag} {mentorTag} {message.SenderHandle}:  {message.Text}</color>"
		            : $"<color=#{color}>{channel}{devtag}{tag} {mentorTag} [<link=name>{message.SenderHandle}</link>]:  {message.Text}</color>";
#else
                return selfMessage
                    ? $"<color=#{color}>{channel} </color>{message.SenderHandle}<color=#{color}>:  {message.Text}</color>"
                    : $"<color=#{color}>{channel} [<link=name>{message.SenderHandle}</link>]:  {message.Text}</color>";
#endif
            }
            case ConsoleMessageType.GroupChat:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_GroupChatColor);
                string channel = "<link=channel:"
                                 + StringUtil.TR("/group", "SlashCommand").Substring(1)
                                 + ">"
                                 + StringUtil.TR("GroupChannel", "Chat")
                                 + "</link>";
                
                string sender;
                if (message.SenderHandle.IsNullOrEmpty())
                {
                    sender = string.Empty;
                }
                else if (selfMessage)
                {
                    sender = message.SenderHandle;
                }
                else
                {
#if EVOS
	                sender = "[<link=name>" + message.SenderHandle + "</link>]";
#else 
                    sender = " [<link=name>" + message.SenderHandle + "</link>]";
#endif
                }

#if EVOS
	            return selfMessage
		            ? $"<color=#{color}>{channel}{devtag}{tag}{mentorTag} {message.SenderHandle}<color=#{color}>:  {message.Text}</color>"
		            : $"<color=#{color}>{channel}{devtag}{tag}{mentorTag} {sender}:  {message.Text}</color>";
#else 
                return selfMessage
                    ? $"<color=#{color}>{channel} </color>{sender}<color=#{color}>:  {message.Text}</color>"
                    : $"<color=#{color}>{channel}{sender}:  {message.Text}</color>";
#endif
            }
            case ConsoleMessageType.WhisperChat:
            {
                string handle;
                string prefix;
                if (selfMessage)
                {
                    handle = message.RecipientHandle;
                    prefix = StringUtil.TR("To", "Chat");
                }
                else
                {
                    handle = message.SenderHandle;
                    prefix = string.Empty;
                }

                string color = ColorToHex(HUD_UIResources.Get().m_whisperChatColor);
                return $"<color=#{color}>{prefix} [<link=name>{handle}</link>]:  {message.Text}</color>";
            }
            case ConsoleMessageType.SystemMessage:
            case ConsoleMessageType.Exception:
            case ConsoleMessageType.BroadcastMessage:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_systemChatColor);
                return string.Format("<color=#{1}>{0}</color>", message.Text, color);
            }
            case ConsoleMessageType.Error:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_systemErrorChatColor);
                return string.Format("<color=#{1}>{0}</color>", message.Text, color);
            }
            case ConsoleMessageType.CombatLog:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_combatLogChatColor);
                return string.Format("<color=#{1}>{0}</color>", message.Text, color);
            }
            case ConsoleMessageType.PingChat:
            case ConsoleMessageType.ScriptedChat:
            {
                string color = ColorToHex(HUD_UIResources.Get().m_TeamChatColor);
                return string.Format("<color=#{1}>{0}</color>", message.Text, color);
            }
            case ConsoleMessageType.DiscordLog:
            {
                string color = ColorToHex(Color.yellow);
                return string.Format("<color=#{1}>{0}</color>", message.Text, color);
            }
            default:
            {
                return message.Text;
            }
        }
    }

    private static bool ShouldDisplay(TextConsole.Message message)
    {
        if (HUD_UI.Get() != null
            && message.MessageType == ConsoleMessageType.GlobalChat)
        {
            return false;
        }

        if (message.MessageType == ConsoleMessageType.GameChat)
        {
            if (Options_UI.Get() != null
                && Options_UI.Get().GetShowAllChat())
            {
                return true;
            }

            if (!AppState.IsInGame()
                && GameManager.Get().GameInfo != null
                && GameManager.Get().GameInfo.IsCustomGame
                && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
            {
                return true;
            }

            return false;
        }

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

        if (s_handledMessages == null)
        {
            return;
        }

        s_handledMessages.Enqueue(
            new HandledMessage
            {
                Message = message,
                AllowedEmojis = allowedEmojis
            });
        if (s_handledMessages.Count > 80)
        {
            s_handledMessages.Dequeue();
        }
    }

    public void HandleMessage(TextConsole.Message message, TextConsole.AllowedEmojis allowedEmojis)
    {
        if (ShouldDisplay(message))
        {
            DisplayMessage(message, allowedEmojis);
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
            flag = message.SenderAccountId == playerAccountData.AccountId;
        }

        Color textColor;
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
                if (m_timeSinceLastWhisper + UIFrontEnd.Get().m_whisperSoundThreshold < Time.time
                    || (flag && m_lastWhisperer != message.RecipientHandle)
                    || (!flag && m_lastWhisperer != message.SenderHandle))
                {
                    m_lastWhisperer = flag ? message.RecipientHandle : message.SenderHandle;
                    if (!flag)
                    {
                        UIFrontEnd.PlaySound(FrontEndButtonSounds.WhisperMessage);
                    }

                    if (!m_whisperedPlayers.Contains(m_lastWhisperer))
                    {
                        m_whisperedPlayers.Add(m_lastWhisperer);
                    }
                }

                m_timeSinceLastWhisper = Time.time;
                break;
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
                text = message.Text;
                textColor = HUD_UIResources.Get().m_GlobalChatColor;
                break;
        }

        if (text.Length > c_maxConsoleStringLength
            && message.MessageType != ConsoleMessageType.SystemMessage)
        {
            // TODO CLIENT BUG? result is unused
            text = text.Substring(0, c_maxConsoleStringLength);
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
            chatAlpha.EaseTo(1f);
            if (m_changeChannelAlpha)
            {
                UIManager.SetGameObjectActive(m_chatroomHitbox, true);
            }

            return;
        }

        chatAlpha.EaseTo(0f);
        if (m_changeChannelAlpha)
        {
            UIManager.SetGameObjectActive(m_chatroomHitbox, false);
        }

        if (EmoticonPanel.Get() != null)
        {
            EmoticonPanel.Get().SetPanelOpen(false);
        }
    }

    public void OnInputSubmitted()
    {
        string text = m_textInput.text.Trim();
        if (!text.IsNullOrEmpty())
        {
            if (!text.StartsWith("/"))
            {
                text = m_chatCommand + " " + text;
            }

            bool flag = GameManager.Get() != null && GameManager.Get().GameInfo != null
                                                  && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;
            bool flag2 = false;
            string[] array = text.Split(' ');
            if (m_globalCommand.IsSlashCommand(array[0]))
            {
                if (Options_UI.Get() != null && !Options_UI.Get().GetShowGlobalChat())
                {
                    AddTextEntry(
                        StringUtil.TR("GlobalChatDisabled", "Chat"),
                        HUD_UIResources.Get().m_GlobalChatColor,
                        true);
                    flag2 = true;
                }
            }
            else if (m_allCommand.IsSlashCommand(array[0]) && Options_UI.Get() != null)
            {
                if (!Options_UI.Get().GetShowAllChat())
                {
                    if (AppState.IsInGame()
                        || GameManager.Get().GameInfo == null
                        || !GameManager.Get().GameInfo.IsCustomGame
                        || GameManager.Get().GameInfo.GameStatus == GameStatus.Stopped)
                    {
                        AddTextEntry(
                            StringUtil.TR("AllChatDisabled", "Chat"),
                            HUD_UIResources.Get().m_GameChatColor,
                            true);
                        flag2 = true;
                    }
                }
            }
            else if (m_teamCommand.IsSlashCommand(array[0]))
            {
                if (!flag)
                {
                    AddTextEntry(StringUtil.TR("NotInAGame", "Invite"), HUD_UIResources.Get().m_TeamChatColor, true);
                    flag2 = true;
                }
            }
            else if (m_groupCommand.IsSlashCommand(array[0]))
            {
                if (ClientGameManager.Get() != null
                    && ClientGameManager.Get().GroupInfo != null
                    && !ClientGameManager.Get().GroupInfo.InAGroup)
                {
                    AddTextEntry(
                        StringUtil.TR("NotInAGroup", "Invite"),
                        HUD_UIResources.Get().m_GroupChatColor,
                        true);
                    flag2 = true;
                }
            }

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
        }

        m_textInput.text = string.Empty;
        if (EventSystem.current.currentSelectedGameObject == m_textInput.gameObject)
        {
            m_inputJustCleared = true;
            EventSystem.current.SetSelectedGameObject(null);
            setInputSelected = UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd;
        }
    }

    private void OnToggleClicked(BaseEventData data)
    {
        ToggleVisibility();
    }

    private void ToggleVisibility()
    {
        if (m_visible
            && m_chatroomList.IsVisible())
        {
            return;
        }

        if (m_visible
            && m_scrollBar.size < 0.9999
            && m_scrollBar.value > 0.0001)
        {
            return;
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
        if (!m_visible)
        {
            ToggleVisibility();
        }
    }

    public void Hide()
    {
        if (m_visible)
        {
            ToggleVisibility();
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
            m_chatroomList.SetVisible(false);
        }
        else
        {
            m_chatroomList.Setup(this);
            m_chatroomList.SetVisible(true);
        }

    }

    private void OnTextConsoleHover(bool hover)
    {
        m_hovering = hover;
        if (!hover && UIScreenManager.Get() != null)
        {
            SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
        }
    }

    private void OnDragOver(GameObject go)
    {
        m_hovering = true;
    }

    private void OnDragOut(GameObject go)
    {
        m_hovering = false;
        if (!m_hovering)
        {
            SetTimeTillCollapse(UIScreenManager.Get().m_chatDisplayTime);
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
        string text = string.Empty;
        text += ((int)(color.r * 255f)).ToString("X2");
        text += ((int)(color.g * 255f)).ToString("X2");
        return text + ((int)(color.b * 255f)).ToString("X2");
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
