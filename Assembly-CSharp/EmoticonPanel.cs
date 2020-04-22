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
		activeEmoticons = new List<EmoticonSelectBtn>();
		inactiveEmoticons = new List<EmoticonSelectBtn>();
		m_emoticonBtn.spriteController.callback = EmoticonIconBtnClicked;
		Init();
		DoPanelOpen(false);
		ClientGameManager.Get().OnAccountDataUpdated += HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= HandleAccountDataUpdated;
			return;
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
					if (chatEmojiIndexByName2 != -1)
					{
						if (first == null)
						{
							if (second == null)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										return 0;
									}
								}
							}
						}
						GameBalanceVars.ChatEmoticon chatEmoticon = GameBalanceVars.Get().ChatEmojis[chatEmojiIndexByName];
						GameBalanceVars.ChatEmoticon chatEmoticon2 = GameBalanceVars.Get().ChatEmojis[chatEmojiIndexByName2];
						if (chatEmoticon.m_sortOrder < chatEmoticon2.m_sortOrder)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									return -1;
								}
							}
						}
						if (chatEmoticon.m_sortOrder > chatEmoticon2.m_sortOrder)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									return 1;
								}
							}
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
		activeEmoticons.Sort(CompareEmoticonButton);
		inactiveEmoticons.Sort(CompareEmoticonButton);
		for (int i = 0; i < activeEmoticons.Count; i++)
		{
			EmoticonSelectBtn emoticonSelectBtn = activeEmoticons[i];
			if (emoticonSelectBtn.transform.GetSiblingIndex() != i)
			{
				emoticonSelectBtn.transform.SetSiblingIndex(i);
			}
		}
		while (true)
		{
			for (int j = 0; j < inactiveEmoticons.Count; j++)
			{
				EmoticonSelectBtn emoticonSelectBtn2 = inactiveEmoticons[j];
				if (emoticonSelectBtn2.transform.GetSiblingIndex() != j + activeEmoticons.Count)
				{
					emoticonSelectBtn2.transform.SetSiblingIndex(j + activeEmoticons.Count);
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
	}

	public void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		if (GameBalanceVars.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (ChatEmojiManager.Get() == null)
			{
				return;
			}
			Init();
			int num;
			if (GameManager.Get() != null)
			{
				num = (GameManager.Get().GameplayOverrides.EnableHiddenCharacters ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			GameBalanceVars.ChatEmoticon[] chatEmojis = GameBalanceVars.Get().ChatEmojis;
			foreach (GameBalanceVars.ChatEmoticon chatEmoticon in chatEmojis)
			{
				bool flag2 = chatEmoticon.m_isHidden || !GameBalanceVarsExtensions.MeetsVisibilityConditions(chatEmoticon);
				if (!flag)
				{
					if (flag2)
					{
						continue;
					}
				}
				using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = ChatEmojiManager.Get().m_emojiList.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
						{
							break;
						}
						ChatEmojiManager.ChatEmoji current = enumerator.Current;
						if (chatEmoticon.Name == current.m_emojiName)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									if (accountData != null && accountData.AccountComponent.IsChatEmojiUnlocked(chatEmoticon))
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												break;
											default:
											{
												bool flag3 = false;
												foreach (EmoticonSelectBtn activeEmoticon in activeEmoticons)
												{
													if (activeEmoticon.GetEmoji().m_emojiName == chatEmoticon.Name)
													{
														flag3 = true;
													}
												}
												if (!flag3)
												{
													while (true)
													{
														switch (2)
														{
														case 0:
															break;
														default:
														{
															for (int j = 0; j < inactiveEmoticons.Count; j++)
															{
																if (inactiveEmoticons[j].GetEmoji().m_emojiName == chatEmoticon.Name)
																{
																	Object.Destroy(inactiveEmoticons[j].gameObject);
																	inactiveEmoticons.RemoveAt(j);
																	break;
																}
															}
															EmoticonSelectBtn emoticonSelectBtn = CreateNewEmoticonBtn(current, m_activePrefab, true);
															int siblingIndex = 0;
															if (activeEmoticons.Count > 0)
															{
																siblingIndex = activeEmoticons[activeEmoticons.Count - 1].transform.GetSiblingIndex() + 1;
															}
															emoticonSelectBtn.transform.SetSiblingIndex(siblingIndex);
															activeEmoticons.Add(emoticonSelectBtn);
															goto end_IL_00c7;
														}
														}
													}
												}
												goto end_IL_00c7;
											}
											}
										}
									}
									goto end_IL_00c7;
								}
							}
						}
					}
					end_IL_00c7:;
				}
			}
			SortDisplayList();
			return;
		}
	}

	private void Init()
	{
		if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			return;
		}
		while (true)
		{
			if (m_initialized)
			{
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
			m_initialized = true;
			m_scrollRect = GetComponentInChildren<ScrollRect>();
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			List<ChatEmojiManager.ChatEmoji> list = new List<ChatEmojiManager.ChatEmoji>();
			List<ChatEmojiManager.ChatEmoji> list2 = new List<ChatEmojiManager.ChatEmoji>();
			int num;
			if (GameManager.Get() != null)
			{
				num = (GameManager.Get().GameplayOverrides.EnableHiddenCharacters ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			GameBalanceVars.ChatEmoticon[] chatEmojis = GameBalanceVars.Get().ChatEmojis;
			foreach (GameBalanceVars.ChatEmoticon chatEmoticon in chatEmojis)
			{
				using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = ChatEmojiManager.Get().m_emojiList.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
						{
							break;
						}
						ChatEmojiManager.ChatEmoji current = enumerator.Current;
						if (chatEmoticon.Name == current.m_emojiName)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									{
										if (playerAccountData != null)
										{
											if (playerAccountData.AccountComponent.IsChatEmojiUnlocked(chatEmoticon))
											{
												while (true)
												{
													switch (5)
													{
													case 0:
														break;
													default:
														list.Add(current);
														goto end_IL_00c4;
													}
												}
											}
										}
										int num2;
										if (!chatEmoticon.m_isHidden)
										{
											num2 = ((!GameBalanceVarsExtensions.MeetsVisibilityConditions(chatEmoticon)) ? 1 : 0);
										}
										else
										{
											num2 = 1;
										}
										bool flag2 = (byte)num2 != 0;
										if (flag)
										{
											goto IL_0169;
										}
										if (!flag2)
										{
											goto IL_0169;
										}
										goto end_IL_00c4;
									}
									IL_0169:
									list2.Add(current);
									goto end_IL_00c4;
								}
							}
						}
					}
					end_IL_00c4:;
				}
			}
			using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ChatEmojiManager.ChatEmoji current2 = enumerator2.Current;
					activeEmoticons.Add(CreateNewEmoticonBtn(current2, m_activePrefab, true));
				}
			}
			using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator3 = list2.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ChatEmojiManager.ChatEmoji current3 = enumerator3.Current;
					inactiveEmoticons.Add(CreateNewEmoticonBtn(current3, m_inactivePrefab, false));
				}
			}
			SortDisplayList();
			return;
		}
	}

	private EmoticonSelectBtn CreateNewEmoticonBtn(ChatEmojiManager.ChatEmoji emoji, EmoticonSelectBtn btnPrefab, bool unlocked)
	{
		EmoticonSelectBtn emoticonSelectBtn = Object.Instantiate(btnPrefab);
		emoticonSelectBtn.transform.SetParent(m_gridlayout.transform);
		emoticonSelectBtn.transform.localPosition = Vector3.zero;
		emoticonSelectBtn.transform.localEulerAngles = Vector3.zero;
		emoticonSelectBtn.transform.localScale = Vector3.one;
		emoticonSelectBtn.Setup(emoji, unlocked);
		if (m_scrollRect != null)
		{
			emoticonSelectBtn.m_theBtn.spriteController.RegisterScrollListener(OnScroll);
		}
		return emoticonSelectBtn;
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void OnEnable()
	{
		if (m_panelOpen)
		{
			return;
		}
		while (true)
		{
			m_emoticonPanelAnimController.Play("EmoticonPanelDefaultOUT");
			return;
		}
	}

	private void DoPanelOpen(bool open)
	{
		m_panelOpen = open;
		m_emoticonBtn.SetSelected(m_panelOpen, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_emoticonPanel.gameObject, true);
		m_panelCanvasGroup.interactable = m_panelOpen;
		m_panelCanvasGroup.blocksRaycasts = m_panelOpen;
		if (m_panelOpen)
		{
			return;
		}
		while (true)
		{
			m_emoticonPanelAnimController.Play("EmoticonPanelDefaultOUT");
			return;
		}
	}

	public void EmoticonIconBtnClicked(BaseEventData data)
	{
		SetPanelOpen(!m_panelOpen);
	}

	public void SetPanelOpen(bool open)
	{
		if (m_panelOpen == open)
		{
			return;
		}
		while (true)
		{
			DoPanelOpen(open);
			return;
		}
	}

	public bool IsPanelOpen()
	{
		return m_panelOpen;
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
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
		if (!flag)
		{
			return;
		}
		while (true)
		{
			SetPanelOpen(false);
			return;
		}
	}
}
