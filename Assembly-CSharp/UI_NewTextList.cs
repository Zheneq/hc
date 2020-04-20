using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NewTextList : MonoBehaviour
{
	public TextMeshProUGUI m_textPrefab;

	public const int m_maxNumberEntries = 0x50;

	public int m_numLinesToDisplayRecent = 4;

	public VerticalLayoutGroup m_recentlyAddedContainer;

	private const int kMessageLengthLimit = 0xC350;

	private List<float> m_textPadding;

	private List<TextMeshProUGUI> m_textList;

	private List<UI_NewTextList.TextItem> m_recentlyAddedText;

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

	private Queue<UI_NewTextList.PendingMessages> m_lastMessagesAdded = new Queue<UI_NewTextList.PendingMessages>();

	private void Awake()
	{
		this.Init();
		this.m_parent = base.GetComponentInParent<UITextConsole>();
	}

	private void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.m_textList = new List<TextMeshProUGUI>();
		this.m_textPadding = new List<float>();
		this.m_recentlyAddedText = new List<UI_NewTextList.TextItem>();
		foreach (TextMeshProUGUI textMeshProUGUI in this.m_recentlyAddedContainer.GetComponentsInChildren<TextMeshProUGUI>(true))
		{
			UnityEngine.Object.Destroy(textMeshProUGUI.gameObject);
		}
		this.m_rectTransform = base.GetComponent<RectTransform>();
		this.m_lastEmojiTag = string.Empty;
		this.m_lastEmojiEntry = null;
	}

	public void SetTextAlpha(float newAlpha)
	{
		this.Init();
		using (List<TextMeshProUGUI>.Enumerator enumerator = this.m_textList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TextMeshProUGUI textMeshProUGUI = enumerator.Current;
				CanvasGroup component = textMeshProUGUI.GetComponent<CanvasGroup>();
				if (component != null)
				{
					component.alpha = newAlpha;
					component.blocksRaycasts = (newAlpha > 0.1f);
				}
				else
				{
					Color color = textMeshProUGUI.color;
					color.a = newAlpha;
					textMeshProUGUI.color = color;
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_recentlyAddedContainer, 1f - newAlpha > 0.1f, null);
		this.currentAlpha = newAlpha;
	}

	private void UpdateLastMessages()
	{
		while (this.m_lastMessagesAdded.Count > 0)
		{
			UI_NewTextList.PendingMessages pendingMessages = this.m_lastMessagesAdded.Dequeue();
			float num = 0f;
			float textChatHeight = this.GetTextChatHeight();
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
					this.ShiftChatEntries(num2);
				}
				else
				{
					this.ShiftChatEntries(preferredHeight + pendingMessages.m_paddingTextAmount);
				}
			}
			Vector2 sizeDelta = this.m_rectTransform.sizeDelta;
			sizeDelta.y = Mathf.Max(textChatHeight + preferredHeight + pendingMessages.m_paddingTextAmount, num);
			this.m_rectTransform.sizeDelta = sizeDelta;
			this.m_textList.Add(pendingMessages.m_textMeshPro);
			this.m_textPadding.Add(pendingMessages.m_paddingTextAmount);
		}
	}

	private void UpdateRecentlyAddedText()
	{
		for (int i = 0; i < this.m_recentlyAddedText.Count; i++)
		{
			if (this.m_recentlyAddedText[i].theText != null)
			{
				float num = this.currentAlpha;
				CanvasGroup component = this.m_recentlyAddedText[i].theText.GetComponent<CanvasGroup>();
				if (Time.time - this.m_recentlyAddedText[i].timeDisplayed >= UIScreenManager.Get().m_chatRecentChatDisplayTime)
				{
					num = this.currentAlpha;
				}
				else
				{
					num = 1f;
				}
				if (component != null)
				{
					component.alpha = num;
					CanvasGroup canvasGroup = component;
					bool blocksRaycasts;
					if (this.m_recentlyAddedContainer.isActiveAndEnabled && num > 0.1f)
					{
						blocksRaycasts = (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd);
					}
					else
					{
						blocksRaycasts = false;
					}
					canvasGroup.blocksRaycasts = blocksRaycasts;
				}
				else
				{
					Color color = this.m_recentlyAddedText[i].theText.color;
					color.a = num;
					this.m_recentlyAddedText[i].theText.color = color;
				}
			}
		}
	}

	private void UpdateRecentlyAddedTextLines()
	{
		int num = 0;
		for (int i = 0; i < this.m_recentlyAddedText.Count; i++)
		{
			if (num >= this.m_numLinesToDisplayRecent)
			{
				UnityEngine.Object.Destroy(this.m_recentlyAddedText[i].theText.gameObject);
				this.m_recentlyAddedText.RemoveAt(i);
				i--;
			}
			else
			{
				TextMeshProUGUI originalText = this.m_recentlyAddedText[i].originalText;
				int lineCount = originalText.textInfo.lineCount;
				if (num + lineCount > this.m_numLinesToDisplayRecent)
				{
					int firstCharacterIndex = originalText.textInfo.lineInfo[lineCount - (this.m_numLinesToDisplayRecent - num)].firstCharacterIndex;
					int index = (int)originalText.textInfo.characterInfo[firstCharacterIndex].index;
					if (index <= 0)
					{
						num = this.m_numLinesToDisplayRecent;
						i--;
						goto IL_1C8;
					}
					string text = string.Empty;
					try
					{
						text = originalText.text.Substring(index);
					}
					catch (ArgumentOutOfRangeException innerException)
					{
						throw new Exception(string.Format("UpdateRecentlyAddedTextLines substring issue. Text = \"{0}\". Index = {1}", originalText.text, index), innerException);
					}
					if (text.Contains("</color>"))
					{
						if (!text.Contains("<color"))
						{
							int num2 = originalText.text.LastIndexOf("<color=");
							int num3 = originalText.text.IndexOf(">", num2);
							text = originalText.text.Substring(num2, num3 - num2 + 1) + text;
						}
					}
					this.m_recentlyAddedText[i].theText.text = text;
				}
				num += lineCount;
			}
			IL_1C8:;
		}
	}

	private void UpdateLinkMessages()
	{
		if (this.m_parent.IsHovered())
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent == null)
			{
				Log.Info("Why is there no canvas in this parent of this object?", new object[0]);
				return;
			}
			Camera worldCamera = componentInParent.worldCamera;
			int num = -1;
			TextMeshProUGUI textMeshProUGUI = null;
			for (int i = 0; i < this.m_recentlyAddedText.Count; i++)
			{
				if (Time.time - this.m_recentlyAddedText[i].timeDisplayed < UIScreenManager.Get().m_chatRecentChatDisplayTime)
				{
					CanvasGroup component = this.m_recentlyAddedText[i].theText.GetComponent<CanvasGroup>();
					if (component == null)
					{
						Log.Info("Canvas Group is null", new object[0]);
					}
					else if (!component.blocksRaycasts)
					{
					}
					else
					{
						num = TMP_TextUtilities.FindIntersectingLink(this.m_recentlyAddedText[i].theText, Input.mousePosition, worldCamera);
						if (num >= 0)
						{
							textMeshProUGUI = this.m_recentlyAddedText[i].theText;
							break;
						}
					}
				}
			}
			if (this.currentAlpha >= 1f)
			{
				if (textMeshProUGUI == null)
				{
					for (int j = 0; j < this.m_textList.Count; j++)
					{
						CanvasGroup component2 = this.m_textList[j].GetComponent<CanvasGroup>();
						if (component2 == null)
						{
							Log.Info("Canvas Group is null", new object[0]);
						}
						else if (component2.blocksRaycasts)
						{
							num = TMP_TextUtilities.FindIntersectingLink(this.m_textList[j], Input.mousePosition, worldCamera);
							if (num >= 0)
							{
								textMeshProUGUI = this.m_textList[j];
								break;
							}
						}
					}
				}
			}
			if (!(this.m_hoveredText != textMeshProUGUI))
			{
				if (this.m_hoveredLink == num)
				{
					goto IL_4E3;
				}
			}
			this.ClearPreviousLinkHover();
			this.m_hoveredText = textMeshProUGUI;
			this.m_hoveredLink = num;
			if (textMeshProUGUI != null)
			{
				if (num >= 0)
				{
					if (textMeshProUGUI.textInfo.linkInfo[num].GetLinkID().StartsWith("emoji-"))
					{
						string text = textMeshProUGUI.textInfo.linkInfo[num].GetLinkID().Substring(6);
						text = text.Substring(0, text.LastIndexOf(':') + 1);
						if (!(this.m_lastEmojiTag != text))
						{
							if (!(this.m_lastEmojiEntry != textMeshProUGUI))
							{
								goto IL_364;
							}
						}
						int index = ChatEmojiManager.Get().GetAllEmojisIndicesInString(text)[0];
						this.m_lastEmojiTag = text;
						this.m_lastEmojiText = ChatEmojiManager.Get().m_emojiList[index].GetHowToUnlock();
						this.m_lastEmojiTagLoc = ChatEmojiManager.Get().m_emojiList[index].GetEmojiTag();
						this.m_lastEmojiEntry = textMeshProUGUI;
						IL_364:
						UITooltipHoverObject component3 = textMeshProUGUI.GetComponent<UITooltipHoverObject>();
						if (component3 != null)
						{
							component3.Refresh();
						}
					}
					else if (textMeshProUGUI.text.Contains("</link>"))
					{
						TMP_LinkInfo tmp_LinkInfo = textMeshProUGUI.textInfo.linkInfo[num];
						int num2 = tmp_LinkInfo.linkIdFirstCharacterIndex + tmp_LinkInfo.linkIdLength + 1;
						string text2 = textMeshProUGUI.text.Substring(Mathf.Clamp(num2, 0, textMeshProUGUI.text.Length - 1), tmp_LinkInfo.linkTextLength);
						string text3 = textMeshProUGUI.text.Substring(0, num2);
						string text4 = textMeshProUGUI.text.Substring(Mathf.Clamp(num2 + tmp_LinkInfo.linkTextLength, 0, textMeshProUGUI.text.Length - 1));
						while (!text4.StartsWith("</link>"))
						{
							if (text4.Length <= 0)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									goto IL_49B;
								}
							}
							else
							{
								text2 += text4[0];
								text4 = text4.Substring(1);
							}
						}
						IL_49B:
						if (text4.Length > 0)
						{
							textMeshProUGUI.text = string.Concat(new string[]
							{
								text3,
								"<u>",
								text2,
								"</u>",
								text4
							});
						}
					}
				}
			}
			IL_4E3:;
		}
		else
		{
			this.ClearPreviousLinkHover();
		}
	}

	public void RefreshTextSizes()
	{
		List<TextMeshProUGUI> list = new List<TextMeshProUGUI>(this.m_textList);
		for (int i = 0; i < this.m_recentlyAddedText.Count; i++)
		{
			list.Add(this.m_recentlyAddedText[i].theText);
		}
		for (int j = 0; j < list.Count; j++)
		{
			TextMeshProUGUI textMeshProUGUI = list[j];
			textMeshProUGUI.text += " ";
			list[j].text = list[j].text.Substring(0, list[j].text.Length - 1);
		}
	}

	private bool EmojiTooltipSetup(UITooltipBase tooltip)
	{
		if (this.m_lastEmojiTag.IsNullOrEmpty())
		{
			return false;
		}
		(tooltip as UITitledTooltip).Setup(this.m_lastEmojiTagLoc, this.m_lastEmojiText, string.Empty);
		return true;
	}

	private void Update()
	{
		this.UpdateLastMessages();
		this.UpdateRecentlyAddedText();
		this.UpdateRecentlyAddedTextLines();
		this.UpdateLinkMessages();
	}

	private float GetTextChatHeight()
	{
		float num = 0f;
		for (int i = 0; i < this.m_textList.Count; i++)
		{
			num += this.m_textList[i].preferredHeight;
			num += this.m_textPadding[i];
		}
		return num;
	}

	private void ClearPreviousLinkHover()
	{
		if (this.m_lastEmojiEntry != null && !this.m_lastEmojiTag.IsNullOrEmpty())
		{
			this.m_lastEmojiTag = string.Empty;
			UITooltipHoverObject component = this.m_lastEmojiEntry.GetComponent<UITooltipHoverObject>();
			if (component != null)
			{
				component.Refresh();
			}
		}
		if (!(this.m_hoveredText == null))
		{
			if (this.m_hoveredLink >= 0)
			{
				TMP_LinkInfo tmp_LinkInfo = this.m_hoveredText.textInfo.linkInfo[this.m_hoveredLink];
				int num = Mathf.Clamp(tmp_LinkInfo.linkIdFirstCharacterIndex + tmp_LinkInfo.linkIdLength + 1, 0, this.m_hoveredText.text.Length - 1);
				int num2 = Mathf.Clamp(num + tmp_LinkInfo.linkTextLength + 7, 0, this.m_hoveredText.text.Length - 1);
				string text = this.m_hoveredText.text.Substring(num, Mathf.Max(num2 - num, 0));
				if (text.StartsWith("<u>"))
				{
					string str = this.m_hoveredText.text.Substring(0, num);
					string text2 = this.m_hoveredText.text.Substring(num2);
					while (text2.Length > 0)
					{
						if (text2.StartsWith("</link>"))
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								goto IL_1AB;
							}
						}
						else
						{
							text += text2[0];
							text2 = text2.Substring(1);
						}
					}
					IL_1AB:
					if (text.EndsWith("</u>"))
					{
						string str2 = text.Substring(3, text.Length - 7);
						this.m_hoveredText.text = str + str2 + text2;
					}
				}
				this.m_hoveredLink = -1;
				this.m_hoveredText = null;
				return;
			}
		}
	}

	public void NotifyVisible(bool visible)
	{
		if (!visible)
		{
			for (int i = 0; i < this.m_recentlyAddedText.Count; i++)
			{
				this.m_recentlyAddedText[i].timeDisplayed = Time.time;
			}
		}
	}

	public void HideRecentText()
	{
		for (int i = 0; i < this.m_recentlyAddedText.Count; i++)
		{
			this.m_recentlyAddedText[i].timeDisplayed = 0f;
		}
	}

	public TextMeshProUGUI AddEntry(string newEntry, Color textColor, bool addToRecentlyAdded, float paddingTextAmount = 0f, ScrollRect scrollRect = null, List<int> allowedEmojis = null)
	{
		if (newEntry.Length > 0xC350)
		{
			newEntry = StringUtil.TR("MessageTooLong", "Global");
		}
		if (this.m_textPrefab == null)
		{
			return null;
		}
		this.Init();
		if (this.m_textList.Count > 0x50)
		{
			float y = this.m_textList[0].preferredHeight + this.m_textPadding[0];
			UnityEngine.Object.Destroy(this.m_textList[0].gameObject);
			this.m_textList.RemoveAt(0);
			this.m_textPadding.RemoveAt(0);
			for (int i = 0; i < this.m_textList.Count; i++)
			{
				this.m_textList[i].rectTransform.anchoredPosition = this.m_textList[i].rectTransform.anchoredPosition + new Vector2(0f, y);
			}
		}
		TextMeshProUGUI textMeshProUGUI = UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.m_textPrefab);
		textMeshProUGUI.transform.SetParent(base.transform);
		textMeshProUGUI.color = textColor;
		textMeshProUGUI.text = newEntry;
		textMeshProUGUI.rectTransform.localScale = new Vector3(1f, 1f, 1f);
		textMeshProUGUI.rectTransform.localPosition = Vector3.zero;
		textMeshProUGUI.rectTransform.anchoredPosition = new Vector2(0f, 0f);
		textMeshProUGUI.rectTransform.offsetMax = new Vector2(0f, textMeshProUGUI.rectTransform.offsetMax.y);
		UIManager.SetGameObjectActive(textMeshProUGUI, true, null);
		this.m_lastMessagesAdded.Enqueue(new UI_NewTextList.PendingMessages
		{
			m_textMeshPro = textMeshProUGUI,
			m_paddingTextAmount = paddingTextAmount,
			m_scrollRect = scrollRect
		});
		ChatEmojiManager.Get().AddNewEmoji(textMeshProUGUI, allowedEmojis);
		UITooltipHoverObject component = textMeshProUGUI.GetComponent<UITooltipHoverObject>();
		if (component != null)
		{
			component.Setup(TooltipType.Titled, new TooltipPopulateCall(this.EmojiTooltipSetup), null);
		}
		TextMeshProUGUI textMeshProUGUI2 = UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.m_textPrefab);
		textMeshProUGUI2.transform.SetParent(this.m_recentlyAddedContainer.transform);
		textMeshProUGUI2.color = textColor;
		textMeshProUGUI2.text = newEntry;
		textMeshProUGUI2.rectTransform.localScale = new Vector3(1f, 1f, 1f);
		textMeshProUGUI2.rectTransform.localPosition = Vector3.zero;
		UIManager.SetGameObjectActive(textMeshProUGUI2, true, null);
		this.m_recentlyAddedText.Insert(0, new UI_NewTextList.TextItem
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
				alpha = this.currentAlpha;
			}
			canvasGroup.alpha = alpha;
			Color color = textMeshProUGUI2.color;
			color.a = 1f;
			textMeshProUGUI2.color = color;
			component2.blocksRaycasts = (this.m_recentlyAddedContainer.gameObject.activeSelf && UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd);
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
				a = this.currentAlpha;
			}
			color2.a = a;
			textMeshProUGUI2.color = color2;
		}
		ChatEmojiManager.Get().AddNewEmoji(textMeshProUGUI2, allowedEmojis);
		UITooltipHoverObject component3 = textMeshProUGUI2.GetComponent<UITooltipHoverObject>();
		if (component3 != null)
		{
			component3.Setup(TooltipType.Titled, new TooltipPopulateCall(this.EmojiTooltipSetup), null);
		}
		component2 = textMeshProUGUI.GetComponent<CanvasGroup>();
		if (component2 != null)
		{
			component2.alpha = this.currentAlpha;
			Color color3 = textMeshProUGUI.color;
			color3.a = 1f;
			textMeshProUGUI.color = color3;
			component2.blocksRaycasts = !this.m_recentlyAddedContainer.gameObject.activeSelf;
		}
		else
		{
			Color color4 = textMeshProUGUI.color;
			color4.a = this.currentAlpha;
			textMeshProUGUI.color = color4;
		}
		UIEventTriggerUtils.AddListener(textMeshProUGUI.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnTextClicked));
		UIEventTriggerUtils.AddListener(textMeshProUGUI2.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnTextClicked));
		return textMeshProUGUI;
	}

	private void ShiftChatEntries(float amountToShift)
	{
		for (int i = 0; i < this.m_textList.Count; i++)
		{
			Vector3 localPosition = this.m_textList[i].rectTransform.localPosition;
			localPosition.y += amountToShift;
			this.m_textList[i].rectTransform.localPosition = localPosition;
		}
	}

	public void RemoveEntry(TextMeshProUGUI entry)
	{
		this.m_textList.Remove(entry);
		UnityEngine.Object.Destroy(entry.gameObject);
	}

	public int NumEntires()
	{
		return this.m_textList.Count;
	}

	public void OnTextClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		TextMeshProUGUI component = pointerEventData.pointerPress.GetComponent<TextMeshProUGUI>();
		int num = -1;
		if (this.m_parent.IsHovered())
		{
			if (this.m_camera == null)
			{
				this.m_camera = base.GetComponentInParent<Canvas>().worldCamera;
			}
			num = TMP_TextUtilities.FindIntersectingLink(component, Input.mousePosition, this.m_camera);
		}
		if (num < 0)
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
						for (int j = 0; j < component2.triggers.Count; j++)
						{
							EventTrigger.Entry entry = component2.triggers[j];
							if (entry.eventID == EventTriggerType.PointerClick)
							{
								pointerEventData.pointerPress = list[i].gameObject;
								entry.callback.Invoke(pointerEventData);
								goto IL_17C;
							}
						}
					}
				}
				IL_17C:;
			}
			return;
		}
		if (component.textInfo.linkInfo[num].GetLinkID() == "name")
		{
			string linkText = component.textInfo.linkInfo[num].GetLinkText();
			if (Input.GetMouseButtonUp(1))
			{
				if (this.m_textList.Count > 0)
				{
					float num2 = Mathf.Abs(this.m_textList[this.m_textList.Count - 1].rectTransform.localPosition.y - component.rectTransform.localPosition.y);
					if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
					{
						if (UIFrontEnd.Get() != null)
						{
							if (UIFrontEnd.Get().m_frontEndChatConsole != null)
							{
								num2 += (base.transform as RectTransform).localPosition.y;
								UIFrontEnd.Get().m_frontEndChatConsole.DisplayMenu(linkText, num2);
								goto IL_3FD;
							}
						}
					}
					if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
					{
						if (HUD_UI.Get() != null && HUD_UI.Get().m_textConsole != null)
						{
							if (this.currentAlpha < 1f && this.m_recentlyAddedText.Count > 0)
							{
								num2 = Mathf.Abs(this.m_recentlyAddedText[this.m_recentlyAddedText.Count - 1].originalText.rectTransform.localPosition.y - component.rectTransform.localPosition.y);
								num2 -= this.m_rectTransform.rect.height * 0.75f;
							}
							HUD_UI.Get().m_textConsole.DisplayIngameMenu(linkText, num2);
						}
					}
					IL_3FD:
					goto IL_4B5;
				}
			}
			if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				if (UIFrontEnd.Get() != null && UIFrontEnd.Get().m_frontEndChatConsole != null)
				{
					UIFrontEnd.Get().m_frontEndChatConsole.SetupWhisper(linkText);
					goto IL_4B5;
				}
			}
			if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
			{
				if (HUD_UI.Get() != null && HUD_UI.Get().m_textConsole != null)
				{
					HUD_UI.Get().m_textConsole.SetupWhisper(linkText);
				}
			}
			IL_4B5:;
		}
		else if (component.textInfo.linkInfo[num].GetLinkID().StartsWith("invite:"))
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
		}
		else if (component.textInfo.linkInfo[num].GetLinkID().StartsWith("channel:"))
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
			if (UIManager.Get().CurrentState == UIManager.ClientState.InGame && HUD_UI.Get() != null)
			{
				if (HUD_UI.Get().m_textConsole != null)
				{
					HUD_UI.Get().m_textConsole.ChangeChannel(channelName);
				}
			}
		}
	}

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
}
