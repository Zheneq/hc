using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NewTextList : MonoBehaviour
{
	private class TextItem
	{
		public TextMeshProUGUI theText;

		public TextMeshProUGUI originalText;

		public float timeDisplayed;
	}

	private class PendingMessages
	{
		public TextMeshProUGUI m_textMeshPro;

		public ScrollRect m_scrollRect;

		public float m_paddingTextAmount;
	}

	public TextMeshProUGUI m_textPrefab;

	public const int m_maxNumberEntries = 80;

	public int m_numLinesToDisplayRecent = 4;

	public VerticalLayoutGroup m_recentlyAddedContainer;

	private const int kMessageLengthLimit = 50000;

	private List<float> m_textPadding;

	private List<TextMeshProUGUI> m_textList;

	private List<TextItem> m_recentlyAddedText;

	private RectTransform m_rectTransform;

	private float currentAlpha;

	private bool initialized;

	private string m_lastEmojiTag;

	private string m_lastEmojiText;

	private string m_lastEmojiTagLoc;

	private TextMeshProUGUI m_lastEmojiEntry;

	private Camera m_camera;

	private UITextConsole m_parent;

	private int m_hoveredLink = -1;

	private TextMeshProUGUI m_hoveredText;

	private Queue<PendingMessages> m_lastMessagesAdded = new Queue<PendingMessages>();

	private void Awake()
	{
		Init();
		m_parent = GetComponentInParent<UITextConsole>();
	}

	private void Init()
	{
		if (initialized)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		initialized = true;
		m_textList = new List<TextMeshProUGUI>();
		m_textPadding = new List<float>();
		m_recentlyAddedText = new List<TextItem>();
		TextMeshProUGUI[] componentsInChildren = m_recentlyAddedContainer.GetComponentsInChildren<TextMeshProUGUI>(true);
		foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
		{
			UnityEngine.Object.Destroy(textMeshProUGUI.gameObject);
		}
		while (true)
		{
			m_rectTransform = GetComponent<RectTransform>();
			m_lastEmojiTag = string.Empty;
			m_lastEmojiEntry = null;
			return;
		}
	}

	public void SetTextAlpha(float newAlpha)
	{
		Init();
		using (List<TextMeshProUGUI>.Enumerator enumerator = m_textList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TextMeshProUGUI current = enumerator.Current;
				CanvasGroup component = current.GetComponent<CanvasGroup>();
				if (component != null)
				{
					component.alpha = newAlpha;
					component.blocksRaycasts = (newAlpha > 0.1f);
				}
				else
				{
					Color color = current.color;
					color.a = newAlpha;
					current.color = color;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					goto end_IL_0014;
				}
			}
			end_IL_0014:;
		}
		UIManager.SetGameObjectActive(m_recentlyAddedContainer, 1f - newAlpha > 0.1f);
		currentAlpha = newAlpha;
	}

	private void UpdateLastMessages()
	{
		while (m_lastMessagesAdded.Count > 0)
		{
			PendingMessages pendingMessages = m_lastMessagesAdded.Dequeue();
			float num = 0f;
			float textChatHeight = GetTextChatHeight();
			if (pendingMessages.m_scrollRect != null)
			{
				num = (pendingMessages.m_scrollRect.gameObject.transform as RectTransform).rect.height;
			}
			pendingMessages.m_textMeshPro.CalculateLayoutInputVertical();
			float preferredHeight = pendingMessages.m_textMeshPro.preferredHeight;
			float num2 = Mathf.Max(num - textChatHeight, 0f);
			float num3 = textChatHeight + num2;
			pendingMessages.m_textMeshPro.rectTransform.anchoredPosition = new Vector2(0f, Mathf.Min(preferredHeight + pendingMessages.m_paddingTextAmount, num2) - num3);
			if (num2 > 0f)
			{
				if (num2 < preferredHeight + pendingMessages.m_paddingTextAmount)
				{
					ShiftChatEntries(num2);
				}
				else
				{
					ShiftChatEntries(preferredHeight + pendingMessages.m_paddingTextAmount);
				}
			}
			Vector2 sizeDelta = m_rectTransform.sizeDelta;
			sizeDelta.y = Mathf.Max(textChatHeight + preferredHeight + pendingMessages.m_paddingTextAmount, num);
			m_rectTransform.sizeDelta = sizeDelta;
			m_textList.Add(pendingMessages.m_textMeshPro);
			m_textPadding.Add(pendingMessages.m_paddingTextAmount);
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

	private void UpdateRecentlyAddedText()
	{
		for (int i = 0; i < m_recentlyAddedText.Count; i++)
		{
			if (!(m_recentlyAddedText[i].theText != null))
			{
				continue;
			}
			float num = currentAlpha;
			CanvasGroup component = m_recentlyAddedText[i].theText.GetComponent<CanvasGroup>();
			if (Time.time - m_recentlyAddedText[i].timeDisplayed >= UIScreenManager.Get().m_chatRecentChatDisplayTime)
			{
				num = currentAlpha;
			}
			else
			{
				num = 1f;
			}
			if (component != null)
			{
				component.alpha = num;
				int blocksRaycasts;
				if (m_recentlyAddedContainer.isActiveAndEnabled && num > 0.1f)
				{
					blocksRaycasts = ((UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd) ? 1 : 0);
				}
				else
				{
					blocksRaycasts = 0;
				}
				component.blocksRaycasts = ((byte)blocksRaycasts != 0);
			}
			else
			{
				Color color = m_recentlyAddedText[i].theText.color;
				color.a = num;
				m_recentlyAddedText[i].theText.color = color;
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

	private void UpdateRecentlyAddedTextLines()
	{
		int num = 0;
		for (int i = 0; i < m_recentlyAddedText.Count; i++)
		{
			if (num >= m_numLinesToDisplayRecent)
			{
				UnityEngine.Object.Destroy(m_recentlyAddedText[i].theText.gameObject);
				m_recentlyAddedText.RemoveAt(i);
				i--;
				continue;
			}
			TextMeshProUGUI originalText = m_recentlyAddedText[i].originalText;
			int lineCount = originalText.textInfo.lineCount;
			if (num + lineCount > m_numLinesToDisplayRecent)
			{
				int firstCharacterIndex = originalText.textInfo.lineInfo[lineCount - (m_numLinesToDisplayRecent - num)].firstCharacterIndex;
				int index = originalText.textInfo.characterInfo[firstCharacterIndex].index;
				if (index <= 0)
				{
					num = m_numLinesToDisplayRecent;
					i--;
					continue;
				}
				string empty = string.Empty;
				try
				{
					empty = originalText.text.Substring(index);
				}
				catch (ArgumentOutOfRangeException innerException)
				{
					throw new Exception($"UpdateRecentlyAddedTextLines substring issue. Text = \"{originalText.text}\". Index = {index}", innerException);
				}
				if (empty.Contains("</color>"))
				{
					if (!empty.Contains("<color"))
					{
						int num2 = originalText.text.LastIndexOf("<color=");
						int num3 = originalText.text.IndexOf(">", num2);
						empty = originalText.text.Substring(num2, num3 - num2 + 1) + empty;
					}
				}
				m_recentlyAddedText[i].theText.text = empty;
			}
			num += lineCount;
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

	private void UpdateLinkMessages()
	{
		if (m_parent.IsHovered())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					Canvas componentInParent = GetComponentInParent<Canvas>();
					if (componentInParent == null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								Log.Info("Why is there no canvas in this parent of this object?");
								return;
							}
						}
					}
					Camera worldCamera = componentInParent.worldCamera;
					int num = -1;
					TextMeshProUGUI textMeshProUGUI = null;
					for (int i = 0; i < m_recentlyAddedText.Count; i++)
					{
						if (Time.time - m_recentlyAddedText[i].timeDisplayed < UIScreenManager.Get().m_chatRecentChatDisplayTime)
						{
							CanvasGroup component = m_recentlyAddedText[i].theText.GetComponent<CanvasGroup>();
							if (component == null)
							{
								Log.Info("Canvas Group is null");
							}
							else if (!component.blocksRaycasts)
							{
							}
							else
							{
								num = TMP_TextUtilities.FindIntersectingLink(m_recentlyAddedText[i].theText, Input.mousePosition, worldCamera);
								if (num >= 0)
								{
									textMeshProUGUI = m_recentlyAddedText[i].theText;
									break;
								}
							}
						}
					}
					if (currentAlpha >= 1f)
					{
						if (textMeshProUGUI == null)
						{
							for (int j = 0; j < m_textList.Count; j++)
							{
								CanvasGroup component2 = m_textList[j].GetComponent<CanvasGroup>();
								if (component2 == null)
								{
									Log.Info("Canvas Group is null");
								}
								else if (component2.blocksRaycasts)
								{
									num = TMP_TextUtilities.FindIntersectingLink(m_textList[j], Input.mousePosition, worldCamera);
									if (num >= 0)
									{
										textMeshProUGUI = m_textList[j];
										break;
									}
								}
							}
						}
					}
					if (!(m_hoveredText != textMeshProUGUI))
					{
						if (m_hoveredLink == num)
						{
							return;
						}
					}
					ClearPreviousLinkHover();
					m_hoveredText = textMeshProUGUI;
					m_hoveredLink = num;
					if (textMeshProUGUI != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								if (num >= 0)
								{
									if (textMeshProUGUI.textInfo.linkInfo[num].GetLinkID().StartsWith("emoji-"))
									{
										while (true)
										{
											UITooltipHoverObject component3;
											switch (1)
											{
											case 0:
												break;
											default:
												{
													string text = textMeshProUGUI.textInfo.linkInfo[num].GetLinkID().Substring(6);
													text = text.Substring(0, text.LastIndexOf(':') + 1);
													if (!(m_lastEmojiTag != text))
													{
														if (!(m_lastEmojiEntry != textMeshProUGUI))
														{
															goto IL_0364;
														}
													}
													int index = ChatEmojiManager.Get().GetAllEmojisIndicesInString(text)[0];
													m_lastEmojiTag = text;
													m_lastEmojiText = ChatEmojiManager.Get().m_emojiList[index].GetHowToUnlock();
													m_lastEmojiTagLoc = ChatEmojiManager.Get().m_emojiList[index].GetEmojiTag();
													m_lastEmojiEntry = textMeshProUGUI;
													goto IL_0364;
												}
												IL_0364:
												component3 = textMeshProUGUI.GetComponent<UITooltipHoverObject>();
												if (component3 != null)
												{
													while (true)
													{
														switch (7)
														{
														case 0:
															break;
														default:
															component3.Refresh();
															return;
														}
													}
												}
												return;
											}
										}
									}
									if (textMeshProUGUI.text.Contains("</link>"))
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												break;
											default:
											{
												TMP_LinkInfo tMP_LinkInfo = textMeshProUGUI.textInfo.linkInfo[num];
												int num2 = tMP_LinkInfo.linkIdFirstCharacterIndex + tMP_LinkInfo.linkIdLength + 1;
												string text2 = textMeshProUGUI.text.Substring(Mathf.Clamp(num2, 0, textMeshProUGUI.text.Length - 1), tMP_LinkInfo.linkTextLength);
												string text3 = textMeshProUGUI.text.Substring(0, num2);
												string text4 = textMeshProUGUI.text.Substring(Mathf.Clamp(num2 + tMP_LinkInfo.linkTextLength, 0, textMeshProUGUI.text.Length - 1));
												while (!text4.StartsWith("</link>"))
												{
													if (text4.Length <= 0)
													{
														break;
													}
													text2 += text4[0];
													text4 = text4.Substring(1);
												}
												if (text4.Length > 0)
												{
													while (true)
													{
														switch (6)
														{
														case 0:
															break;
														default:
															textMeshProUGUI.text = text3 + "<u>" + text2 + "</u>" + text4;
															return;
														}
													}
												}
												return;
											}
											}
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		ClearPreviousLinkHover();
	}

	public void RefreshTextSizes()
	{
		List<TextMeshProUGUI> list = new List<TextMeshProUGUI>(m_textList);
		for (int i = 0; i < m_recentlyAddedText.Count; i++)
		{
			list.Add(m_recentlyAddedText[i].theText);
		}
		while (true)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j].text += " ";
				list[j].text = list[j].text.Substring(0, list[j].text.Length - 1);
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private bool EmojiTooltipSetup(UITooltipBase tooltip)
	{
		if (m_lastEmojiTag.IsNullOrEmpty())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		(tooltip as UITitledTooltip).Setup(m_lastEmojiTagLoc, m_lastEmojiText, string.Empty);
		return true;
	}

	private void Update()
	{
		UpdateLastMessages();
		UpdateRecentlyAddedText();
		UpdateRecentlyAddedTextLines();
		UpdateLinkMessages();
	}

	private float GetTextChatHeight()
	{
		float num = 0f;
		for (int i = 0; i < m_textList.Count; i++)
		{
			num += m_textList[i].preferredHeight;
			num += m_textPadding[i];
		}
		while (true)
		{
			return num;
		}
	}

	private void ClearPreviousLinkHover()
	{
		if (m_lastEmojiEntry != null && !m_lastEmojiTag.IsNullOrEmpty())
		{
			m_lastEmojiTag = string.Empty;
			UITooltipHoverObject component = m_lastEmojiEntry.GetComponent<UITooltipHoverObject>();
			if (component != null)
			{
				component.Refresh();
			}
		}
		if (m_hoveredText == null)
		{
			return;
		}
		if (m_hoveredLink < 0)
		{
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
		TMP_LinkInfo tMP_LinkInfo = m_hoveredText.textInfo.linkInfo[m_hoveredLink];
		int num = Mathf.Clamp(tMP_LinkInfo.linkIdFirstCharacterIndex + tMP_LinkInfo.linkIdLength + 1, 0, m_hoveredText.text.Length - 1);
		int num2 = Mathf.Clamp(num + tMP_LinkInfo.linkTextLength + 7, 0, m_hoveredText.text.Length - 1);
		string text = m_hoveredText.text.Substring(num, Mathf.Max(num2 - num, 0));
		if (text.StartsWith("<u>"))
		{
			string str = m_hoveredText.text.Substring(0, num);
			string text2 = m_hoveredText.text.Substring(num2);
			while (text2.Length > 0)
			{
				if (!text2.StartsWith("</link>"))
				{
					text += text2[0];
					text2 = text2.Substring(1);
					continue;
				}
				break;
			}
			if (text.EndsWith("</u>"))
			{
				string str2 = text.Substring(3, text.Length - 7);
				m_hoveredText.text = str + str2 + text2;
			}
		}
		m_hoveredLink = -1;
		m_hoveredText = null;
	}

	public void NotifyVisible(bool visible)
	{
		if (visible)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_recentlyAddedText.Count; i++)
			{
				m_recentlyAddedText[i].timeDisplayed = Time.time;
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void HideRecentText()
	{
		for (int i = 0; i < m_recentlyAddedText.Count; i++)
		{
			m_recentlyAddedText[i].timeDisplayed = 0f;
		}
		while (true)
		{
			return;
		}
	}

	public TextMeshProUGUI AddEntry(string newEntry, Color textColor, bool addToRecentlyAdded, float paddingTextAmount = 0f, ScrollRect scrollRect = null, List<int> allowedEmojis = null)
	{
		if (newEntry.Length > 50000)
		{
			newEntry = StringUtil.TR("MessageTooLong", "Global");
		}
		if (m_textPrefab == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		Init();
		if (m_textList.Count > 80)
		{
			float y = m_textList[0].preferredHeight + m_textPadding[0];
			UnityEngine.Object.Destroy(m_textList[0].gameObject);
			m_textList.RemoveAt(0);
			m_textPadding.RemoveAt(0);
			for (int i = 0; i < m_textList.Count; i++)
			{
				m_textList[i].rectTransform.anchoredPosition = m_textList[i].rectTransform.anchoredPosition + new Vector2(0f, y);
			}
		}
		TextMeshProUGUI textMeshProUGUI = UnityEngine.Object.Instantiate(m_textPrefab);
		textMeshProUGUI.transform.SetParent(base.transform);
		textMeshProUGUI.color = textColor;
		textMeshProUGUI.text = newEntry;
		textMeshProUGUI.rectTransform.localScale = new Vector3(1f, 1f, 1f);
		textMeshProUGUI.rectTransform.localPosition = Vector3.zero;
		textMeshProUGUI.rectTransform.anchoredPosition = new Vector2(0f, 0f);
		RectTransform rectTransform = textMeshProUGUI.rectTransform;
		Vector2 offsetMax = textMeshProUGUI.rectTransform.offsetMax;
		rectTransform.offsetMax = new Vector2(0f, offsetMax.y);
		UIManager.SetGameObjectActive(textMeshProUGUI, true);
		m_lastMessagesAdded.Enqueue(new PendingMessages
		{
			m_textMeshPro = textMeshProUGUI,
			m_paddingTextAmount = paddingTextAmount,
			m_scrollRect = scrollRect
		});
		ChatEmojiManager.Get().AddNewEmoji(textMeshProUGUI, allowedEmojis);
		UITooltipHoverObject component = textMeshProUGUI.GetComponent<UITooltipHoverObject>();
		if (component != null)
		{
			component.Setup(TooltipType.Titled, EmojiTooltipSetup);
		}
		TextMeshProUGUI textMeshProUGUI2 = UnityEngine.Object.Instantiate(m_textPrefab);
		textMeshProUGUI2.transform.SetParent(m_recentlyAddedContainer.transform);
		textMeshProUGUI2.color = textColor;
		textMeshProUGUI2.text = newEntry;
		textMeshProUGUI2.rectTransform.localScale = new Vector3(1f, 1f, 1f);
		textMeshProUGUI2.rectTransform.localPosition = Vector3.zero;
		UIManager.SetGameObjectActive(textMeshProUGUI2, true);
		m_recentlyAddedText.Insert(0, new TextItem
		{
			theText = textMeshProUGUI2,
			originalText = textMeshProUGUI,
			timeDisplayed = Time.time
		});
		CanvasGroup component2 = textMeshProUGUI2.GetComponent<CanvasGroup>();
		if (component2 != null)
		{
			CanvasGroup canvasGroup = component2;
			float alpha;
			if (addToRecentlyAdded)
			{
				alpha = 1f;
			}
			else
			{
				alpha = currentAlpha;
			}
			canvasGroup.alpha = alpha;
			Color color = textMeshProUGUI2.color;
			color.a = 1f;
			textMeshProUGUI2.color = color;
			component2.blocksRaycasts = (m_recentlyAddedContainer.gameObject.activeSelf && UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd);
		}
		else
		{
			Color color2 = textMeshProUGUI2.color;
			float a;
			if (addToRecentlyAdded)
			{
				a = 1f;
			}
			else
			{
				a = currentAlpha;
			}
			color2.a = a;
			textMeshProUGUI2.color = color2;
		}
		ChatEmojiManager.Get().AddNewEmoji(textMeshProUGUI2, allowedEmojis);
		UITooltipHoverObject component3 = textMeshProUGUI2.GetComponent<UITooltipHoverObject>();
		if (component3 != null)
		{
			component3.Setup(TooltipType.Titled, EmojiTooltipSetup);
		}
		component2 = textMeshProUGUI.GetComponent<CanvasGroup>();
		if (component2 != null)
		{
			component2.alpha = currentAlpha;
			Color color3 = textMeshProUGUI.color;
			color3.a = 1f;
			textMeshProUGUI.color = color3;
			component2.blocksRaycasts = !m_recentlyAddedContainer.gameObject.activeSelf;
		}
		else
		{
			Color color4 = textMeshProUGUI.color;
			color4.a = currentAlpha;
			textMeshProUGUI.color = color4;
		}
		UIEventTriggerUtils.AddListener(textMeshProUGUI.gameObject, EventTriggerType.PointerClick, OnTextClicked);
		UIEventTriggerUtils.AddListener(textMeshProUGUI2.gameObject, EventTriggerType.PointerClick, OnTextClicked);
		return textMeshProUGUI;
	}

	private void ShiftChatEntries(float amountToShift)
	{
		for (int i = 0; i < m_textList.Count; i++)
		{
			Vector3 localPosition = m_textList[i].rectTransform.localPosition;
			localPosition.y += amountToShift;
			m_textList[i].rectTransform.localPosition = localPosition;
		}
	}

	public void RemoveEntry(TextMeshProUGUI entry)
	{
		m_textList.Remove(entry);
		UnityEngine.Object.Destroy(entry.gameObject);
	}

	public int NumEntires()
	{
		return m_textList.Count;
	}

	public void OnTextClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		TextMeshProUGUI component = pointerEventData.pointerPress.GetComponent<TextMeshProUGUI>();
		int num = -1;
		if (m_parent.IsHovered())
		{
			if (m_camera == null)
			{
				m_camera = GetComponentInParent<Canvas>().worldCamera;
			}
			num = TMP_TextUtilities.FindIntersectingLink(component, Input.mousePosition, m_camera);
		}
		if (num < 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					List<RaycastResult> list = new List<RaycastResult>();
					EventSystem.current.RaycastAll(pointerEventData, list);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].gameObject != component.gameObject)
						{
							EventTrigger component2 = list[i].gameObject.GetComponent<EventTrigger>();
							if (component2 != null)
							{
								int num2 = 0;
								while (true)
								{
									if (num2 >= component2.triggers.Count)
									{
										break;
									}
									EventTrigger.Entry entry = component2.triggers[num2];
									if (entry.eventID == EventTriggerType.PointerClick)
									{
										pointerEventData.pointerPress = list[i].gameObject;
										entry.callback.Invoke(pointerEventData);
										break;
									}
									num2++;
								}
							}
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
				}
			}
		}
		if (component.textInfo.linkInfo[num].GetLinkID() == "name")
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					string linkText = component.textInfo.linkInfo[num].GetLinkText();
					if (Input.GetMouseButtonUp(1))
					{
						if (m_textList.Count > 0)
						{
							Vector3 localPosition = m_textList[m_textList.Count - 1].rectTransform.localPosition;
							float y = localPosition.y;
							Vector3 localPosition2 = component.rectTransform.localPosition;
							float num3 = Mathf.Abs(y - localPosition2.y);
							if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
							{
								if (UIFrontEnd.Get() != null)
								{
									if (UIFrontEnd.Get().m_frontEndChatConsole != null)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
											{
												float num4 = num3;
												Vector3 localPosition3 = (base.transform as RectTransform).localPosition;
												num3 = num4 + localPosition3.y;
												UIFrontEnd.Get().m_frontEndChatConsole.DisplayMenu(linkText, num3);
												return;
											}
											}
										}
									}
								}
							}
							if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										if (HUD_UI.Get() != null && HUD_UI.Get().m_textConsole != null)
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													break;
												default:
													if (currentAlpha < 1f && m_recentlyAddedText.Count > 0)
													{
														Vector3 localPosition4 = m_recentlyAddedText[m_recentlyAddedText.Count - 1].originalText.rectTransform.localPosition;
														float y2 = localPosition4.y;
														Vector3 localPosition5 = component.rectTransform.localPosition;
														num3 = Mathf.Abs(y2 - localPosition5.y);
														num3 -= m_rectTransform.rect.height * 0.75f;
													}
													HUD_UI.Get().m_textConsole.DisplayIngameMenu(linkText, num3);
													return;
												}
											}
										}
										return;
									}
								}
							}
							return;
						}
					}
					if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
					{
						if (UIFrontEnd.Get() != null && UIFrontEnd.Get().m_frontEndChatConsole != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									UIFrontEnd.Get().m_frontEndChatConsole.SetupWhisper(linkText);
									return;
								}
							}
						}
					}
					if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (HUD_UI.Get() != null && HUD_UI.Get().m_textConsole != null)
								{
									HUD_UI.Get().m_textConsole.SetupWhisper(linkText);
								}
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		if (component.textInfo.linkInfo[num].GetLinkID().StartsWith("invite:"))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					string arguments = component.textInfo.linkInfo[num].GetLinkID().Substring(7);
					bool flag = false;
					if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
					{
						if (GameManager.Get() != null)
						{
							if (GameManager.Get().GameInfo != null)
							{
								if (GameManager.Get().GameInfo.GameConfig != null)
								{
									if (GameManager.Get().GameInfo.GameConfig.GameType == GameType.Custom)
									{
										flag = true;
									}
								}
							}
						}
					}
					if (flag)
					{
						SlashCommands.Get().RunSlashCommand("/invitetogame", arguments);
					}
					else
					{
						SlashCommands.Get().RunSlashCommand("/invite", arguments);
					}
					return;
				}
				}
			}
		}
		if (!component.textInfo.linkInfo[num].GetLinkID().StartsWith("channel:"))
		{
			return;
		}
		while (true)
		{
			string channelName = component.textInfo.linkInfo[num].GetLinkID().Substring(8);
			if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				if (UIFrontEnd.Get() != null)
				{
					if (UIFrontEnd.Get().m_frontEndChatConsole != null)
					{
						UIFrontEnd.Get().m_frontEndChatConsole.ChangeChannel(channelName);
						return;
					}
				}
			}
			if (UIManager.Get().CurrentState != UIManager.ClientState.InGame || !(HUD_UI.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (HUD_UI.Get().m_textConsole != null)
				{
					while (true)
					{
						HUD_UI.Get().m_textConsole.ChangeChannel(channelName);
						return;
					}
				}
				return;
			}
		}
	}
}
