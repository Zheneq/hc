using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmoticonPanel : MonoBehaviour
{
	public _SelectableBtn m_emoticonBtn;

	public RectTransform m_emoticonPanel;

	public Animator m_emoticonPanelAnimController;

	public CanvasGroup m_panelCanvasGroup;

	public GridLayoutGroup m_gridlayout;

	public EmoticonSelectBtn m_activePrefab;

	public EmoticonSelectBtn m_inactivePrefab;

	private bool m_panelOpen;

	private bool m_initialized;

	private ScrollRect m_scrollRect;

	public List<EmoticonSelectBtn> activeEmoticons;

	public List<EmoticonSelectBtn> inactiveEmoticons;

	public static EmoticonPanel Get()
	{
		return UIChatBox.Get().GetCurrentActiveEmoticonPanel();
	}

	private void Awake()
	{
		this.activeEmoticons = new List<EmoticonSelectBtn>();
		this.inactiveEmoticons = new List<EmoticonSelectBtn>();
		this.m_emoticonBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.EmoticonIconBtnClicked);
		this.Init();
		this.DoPanelOpen(false);
		ClientGameManager.Get().OnAccountDataUpdated += this.HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.HandleAccountDataUpdated;
		}
	}

	private int CompareEmoticonButton(EmoticonSelectBtn first, EmoticonSelectBtn second)
	{
		int chatEmojiIndexByName = GameBalanceVars.Get().GetChatEmojiIndexByName(first.GetEmoji().m_emojiName);
		int chatEmojiIndexByName2 = GameBalanceVars.Get().GetChatEmojiIndexByName(second.GetEmoji().m_emojiName);
		if (!(first == null))
		{
			if (chatEmojiIndexByName != -1)
			{
				if (!(second == null))
				{
					if (chatEmojiIndexByName2 == -1)
					{
					}
					else
					{
						if (first == null)
						{
							if (second == null)
							{
								return 0;
							}
						}
						GameBalanceVars.ChatEmoticon chatEmoticon = GameBalanceVars.Get().ChatEmojis[chatEmojiIndexByName];
						GameBalanceVars.ChatEmoticon chatEmoticon2 = GameBalanceVars.Get().ChatEmojis[chatEmojiIndexByName2];
						if (chatEmoticon.m_sortOrder < chatEmoticon2.m_sortOrder)
						{
							return -1;
						}
						if (chatEmoticon.m_sortOrder > chatEmoticon2.m_sortOrder)
						{
							return 1;
						}
						return 0;
					}
				}
				return 1;
			}
		}
		return -1;
	}

	public static bool IsMouseOverEmoticonPanel()
	{
		bool result = false;
		if (EventSystem.current != null)
		{
			if (EventSystem.current.IsPointerOverGameObject(-1))
			{
				StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
				if (component != null)
				{
					if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
					{
						EmoticonPanel componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<EmoticonPanel>();
						if (componentInParent != null)
						{
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	private void SortDisplayList()
	{
		this.activeEmoticons.Sort(new Comparison<EmoticonSelectBtn>(this.CompareEmoticonButton));
		this.inactiveEmoticons.Sort(new Comparison<EmoticonSelectBtn>(this.CompareEmoticonButton));
		for (int i = 0; i < this.activeEmoticons.Count; i++)
		{
			EmoticonSelectBtn emoticonSelectBtn = this.activeEmoticons[i];
			if (emoticonSelectBtn.transform.GetSiblingIndex() != i)
			{
				emoticonSelectBtn.transform.SetSiblingIndex(i);
			}
		}
		for (int j = 0; j < this.inactiveEmoticons.Count; j++)
		{
			EmoticonSelectBtn emoticonSelectBtn2 = this.inactiveEmoticons[j];
			if (emoticonSelectBtn2.transform.GetSiblingIndex() != j + this.activeEmoticons.Count)
			{
				emoticonSelectBtn2.transform.SetSiblingIndex(j + this.activeEmoticons.Count);
			}
		}
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		if (GameBalanceVars.Get() != null)
		{
			if (!(ChatEmojiManager.Get() == null))
			{
				this.Init();
				bool flag;
				if (GameManager.Get() != null)
				{
					flag = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				GameBalanceVars.ChatEmoticon[] chatEmojis = GameBalanceVars.Get().ChatEmojis;
				int i = 0;
				while (i < chatEmojis.Length)
				{
					GameBalanceVars.ChatEmoticon chatEmoticon = chatEmojis[i];
					bool flag3 = chatEmoticon.m_isHidden || !GameBalanceVarsExtensions.MeetsVisibilityConditions(chatEmoticon);
					if (flag2)
					{
						goto IL_B2;
					}
					if (!flag3)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							goto IL_B2;
						}
					}
					IL_2AF:
					i++;
					continue;
					IL_B2:
					using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = ChatEmojiManager.Get().m_emojiList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ChatEmojiManager.ChatEmoji chatEmoji = enumerator.Current;
							if (chatEmoticon.Name == chatEmoji.m_emojiName)
							{
								if (accountData != null && accountData.AccountComponent.IsChatEmojiUnlocked(chatEmoticon))
								{
									bool flag4 = false;
									foreach (EmoticonSelectBtn emoticonSelectBtn in this.activeEmoticons)
									{
										if (emoticonSelectBtn.GetEmoji().m_emojiName == chatEmoticon.Name)
										{
											flag4 = true;
											break;
										}
									}
									if (!flag4)
									{
										for (int j = 0; j < this.inactiveEmoticons.Count; j++)
										{
											if (this.inactiveEmoticons[j].GetEmoji().m_emojiName == chatEmoticon.Name)
											{
												UnityEngine.Object.Destroy(this.inactiveEmoticons[j].gameObject);
												this.inactiveEmoticons.RemoveAt(j);
												break;
											}
										}
										EmoticonSelectBtn emoticonSelectBtn2 = this.CreateNewEmoticonBtn(chatEmoji, this.m_activePrefab, true);
										int siblingIndex = 0;
										if (this.activeEmoticons.Count > 0)
										{
											siblingIndex = this.activeEmoticons[this.activeEmoticons.Count - 1].transform.GetSiblingIndex() + 1;
										}
										emoticonSelectBtn2.transform.SetSiblingIndex(siblingIndex);
										this.activeEmoticons.Add(emoticonSelectBtn2);
									}
								}
								goto IL_2AF;
							}
						}
					}
					goto IL_2AF;
				}
				this.SortDisplayList();
				return;
			}
		}
	}

	private void Init()
	{
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			if (!this.m_initialized)
			{
				this.m_initialized = true;
				this.m_scrollRect = base.GetComponentInChildren<ScrollRect>();
				PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
				List<ChatEmojiManager.ChatEmoji> list = new List<ChatEmojiManager.ChatEmoji>();
				List<ChatEmojiManager.ChatEmoji> list2 = new List<ChatEmojiManager.ChatEmoji>();
				bool flag;
				if (GameManager.Get() != null)
				{
					flag = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				foreach (GameBalanceVars.ChatEmoticon chatEmoticon in GameBalanceVars.Get().ChatEmojis)
				{
					using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = ChatEmojiManager.Get().m_emojiList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ChatEmojiManager.ChatEmoji chatEmoji = enumerator.Current;
							if (chatEmoticon.Name == chatEmoji.m_emojiName)
							{
								if (playerAccountData != null)
								{
									if (playerAccountData.AccountComponent.IsChatEmojiUnlocked(chatEmoticon))
									{
										list.Add(chatEmoji);
										goto IL_171;
									}
								}
								bool flag3;
								if (!chatEmoticon.m_isHidden)
								{
									flag3 = !GameBalanceVarsExtensions.MeetsVisibilityConditions(chatEmoticon);
								}
								else
								{
									flag3 = true;
								}
								bool flag4 = flag3;
								if (!flag2)
								{
									if (flag4)
									{
										goto IL_171;
									}
								}
								list2.Add(chatEmoji);
								IL_171:
								goto IL_199;
							}
						}
					}
					IL_199:;
				}
				using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ChatEmojiManager.ChatEmoji emoji = enumerator2.Current;
						this.activeEmoticons.Add(this.CreateNewEmoticonBtn(emoji, this.m_activePrefab, true));
					}
				}
				using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator3 = list2.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ChatEmojiManager.ChatEmoji emoji2 = enumerator3.Current;
						this.inactiveEmoticons.Add(this.CreateNewEmoticonBtn(emoji2, this.m_inactivePrefab, false));
					}
				}
				this.SortDisplayList();
				return;
			}
		}
	}

	private EmoticonSelectBtn CreateNewEmoticonBtn(ChatEmojiManager.ChatEmoji emoji, EmoticonSelectBtn btnPrefab, bool unlocked)
	{
		EmoticonSelectBtn emoticonSelectBtn = UnityEngine.Object.Instantiate<EmoticonSelectBtn>(btnPrefab);
		emoticonSelectBtn.transform.SetParent(this.m_gridlayout.transform);
		emoticonSelectBtn.transform.localPosition = Vector3.zero;
		emoticonSelectBtn.transform.localEulerAngles = Vector3.zero;
		emoticonSelectBtn.transform.localScale = Vector3.one;
		emoticonSelectBtn.Setup(emoji, unlocked);
		if (this.m_scrollRect != null)
		{
			emoticonSelectBtn.m_theBtn.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		return emoticonSelectBtn;
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void OnEnable()
	{
		if (!this.m_panelOpen)
		{
			this.m_emoticonPanelAnimController.Play("EmoticonPanelDefaultOUT");
		}
	}

	private void DoPanelOpen(bool open)
	{
		this.m_panelOpen = open;
		this.m_emoticonBtn.SetSelected(this.m_panelOpen, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_emoticonPanel.gameObject, true, null);
		this.m_panelCanvasGroup.interactable = this.m_panelOpen;
		this.m_panelCanvasGroup.blocksRaycasts = this.m_panelOpen;
		if (!this.m_panelOpen)
		{
			this.m_emoticonPanelAnimController.Play("EmoticonPanelDefaultOUT");
		}
	}

	public void EmoticonIconBtnClicked(BaseEventData data)
	{
		this.SetPanelOpen(!this.m_panelOpen);
	}

	public void SetPanelOpen(bool open)
	{
		if (this.m_panelOpen != open)
		{
			this.DoPanelOpen(open);
		}
	}

	public bool IsPanelOpen()
	{
		return this.m_panelOpen;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			bool flag = true;
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
			{
				StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
				if (component != null)
				{
					if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
					{
						EmoticonPanel componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<EmoticonPanel>();
						if (componentInParent != null)
						{
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				this.SetPanelOpen(false);
			}
		}
	}
}
