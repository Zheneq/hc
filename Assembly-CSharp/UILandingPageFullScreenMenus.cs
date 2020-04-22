using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILandingPageFullScreenMenus : UIScene
{
	public class TextChapterDisplayInfo
	{
		public string Label;

		public _SelectableBtn DisplayBtn;

		public string TitleTextString;

		public string HeaderTextString;

		public List<string> ContentTextString = new List<string>();
	}

	public Animator m_animator;

	public RectTransform m_backgroundContainer;

	public RectTransform m_contentContainer;

	public RectTransform m_messageContainer;

	public RectTransform m_feedbackContainer;

	public RectTransform m_textContainer;

	public RectTransform m_textScrollContentContainer;

	public ScrollRect m_textScrollRect;

	public RectTransform m_videoContainer;

	public RectTransform m_reportContainer;

	public RectTransform m_facebookContainer;

	public _SelectableBtn m_messageCloseBtn;

	public _SelectableBtn m_feedbackCloseBtn;

	public _SelectableBtn m_textCloseBtn;

	public _SelectableBtn m_videoCloseBtn;

	public _SelectableBtn m_reportCloseBtn;

	public _SelectableBtn m_facebookCloseBtn;

	[Header("Text Content")]
	public RectTransform m_TextItemListContainer;

	public RectTransform m_TextContainerContainer;

	public _SelectableBtn m_TextListItem;

	public TextMeshProUGUI m_TextListLabel;

	public LayoutGroup m_TextListItemParent;

	public TextMeshProUGUI m_textHeader;

	public TextMeshProUGUI m_textDescription;

	public Image m_textImage;

	public TextMeshProUGUI m_textContent;

	[Header("Video Content")]
	public TextMeshProUGUI m_videoTextHeader;

	[Header("Feedback Content")]
	public RectTransform m_feedbackTextContainer;

	public TMP_InputField m_feedbackInput;

	public _SelectableBtn m_feedbackSubmitBtn;

	public _SelectableBtn m_feedbackCancelBtn;

	public _SelectableBtn m_feedbackSuggestionBtn;

	public _SelectableBtn m_feedbackBugBtn;

	[Header("Facebook Content")]
	public RectTransform m_facebookTextContainer;

	public TMP_InputField m_facebookInput;

	public _SelectableBtn m_facebookContinueBtn;

	public Image m_facebookScreenshotPreview;

	[Header("Braodcast Content")]
	public TextMeshProUGUI m_broadcastMessageTitle;

	public TextMeshProUGUI m_broadcastMessageContent;

	[Header("Report Content")]
	public RectTransform m_reportTextContainer;

	public TMP_InputField m_reportInput;

	public _SelectableBtn m_reportSubmitBtn;

	public _SelectableBtn m_reportCancelBtn;

	public TextMeshProUGUI m_reportPlayerHeader;

	public TextMeshProUGUI[] m_reportPlayerText;

	public _SelectableBtn m_reportPlayerButton;

	public GameObject m_reportPlayerDropdown;

	public _SelectableBtn m_reportPlayerReason1Button;

	public _SelectableBtn m_reportPlayerReason2Button;

	public _SelectableBtn m_reportPlayerReason3Button;

	public _SelectableBtn m_reportPlayerReason4Button;

	public _SelectableBtn m_reportPlayerReason5Button;

	public _SelectableBtn m_reportPlayerReason6Button;

	public _SelectableBtn m_reportPlayerReason7Button;

	public _SelectableBtn m_reportPlayerReason8Button;

	public _SelectableBtn m_reportPlayerReason9Button;

	private static UILandingPageFullScreenMenus s_instance;

	private string m_reportPlayerHandle = string.Empty;

	private long m_reportPlayerAccountId;

	private bool m_botMasqueradingAsHuman;

	private bool m_windowVisible;

	private bool m_messageVisible;

	private bool m_feedbackVisible;

	private bool m_reportVisible;

	private bool m_facebookVisible;

	private bool m_textVisible;

	private bool m_videoVisible;

	private bool m_shouldSelectFeedbackInput;

	private bool m_shouldSelectReportInput;

	private bool m_shouldSelectFacebookInput;

	private int m_setScrollBar;

	private ClientFeedbackReport.FeedbackReason m_reportReason;

	private List<_SelectableBtn> TextListItemBtns = new List<_SelectableBtn>();

	private List<TextMeshProUGUI> TextListItemLabels = new List<TextMeshProUGUI>();

	private List<TextChapterDisplayInfo> SeasonLoreDisplayInfos = new List<TextChapterDisplayInfo>();

	private Action m_onClose = delegate
	{
	};

	public static UILandingPageFullScreenMenus Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.AdvancedOverlays;
	}

	public override void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_messageContainer, false);
		UIManager.SetGameObjectActive(m_feedbackContainer, false);
		UIManager.SetGameObjectActive(m_reportContainer, false);
		UIManager.SetGameObjectActive(m_facebookContainer, false);
		UIManager.SetGameObjectActive(m_textContainer, false);
		UIManager.SetGameObjectActive(m_videoContainer, false);
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
		m_messageCloseBtn.spriteController.callback = CloseMessage;
		m_textCloseBtn.spriteController.callback = CloseText;
		m_videoCloseBtn.spriteController.callback = CloseVideo;
		m_feedbackCloseBtn.spriteController.callback = CloseFeedback;
		m_reportCloseBtn.spriteController.callback = CloseReport;
		m_facebookCloseBtn.spriteController.callback = CloseFacebook;
		m_feedbackSubmitBtn.spriteController.callback = SubmitFeedback;
		m_feedbackCancelBtn.spriteController.callback = CloseFeedback;
		m_feedbackSuggestionBtn.spriteController.callback = SuggestionClicked;
		m_feedbackBugBtn.spriteController.callback = BugClicked;
		_MouseEventPasser mouseEventPasser = m_textHeader.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(m_textScrollRect);
		mouseEventPasser = m_textDescription.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(m_textScrollRect);
		mouseEventPasser = m_textContent.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(m_textScrollRect);
		m_textScrollRect.movementType = ScrollRect.MovementType.Clamped;
		m_reportSubmitBtn.spriteController.callback = SubmitReport;
		m_reportCancelBtn.spriteController.callback = CloseReport;
		m_facebookContinueBtn.spriteController.callback = ContinueFacebook;
		m_reportPlayerButton.spriteController.callback = OnReportPlayer;
		m_reportPlayerReason1Button.spriteController.callback = OnReportPlayer1;
		m_reportPlayerReason2Button.spriteController.callback = OnReportPlayer2;
		m_reportPlayerReason3Button.spriteController.callback = OnReportPlayer3;
		m_reportPlayerReason4Button.spriteController.callback = OnReportPlayer4;
		m_reportPlayerReason5Button.spriteController.callback = OnReportPlayer5;
		m_reportPlayerReason6Button.spriteController.callback = OnReportPlayer6;
		m_reportPlayerReason7Button.spriteController.callback = OnReportPlayer7;
		m_reportPlayerReason8Button.spriteController.callback = OnReportPlayer8;
		m_reportPlayerReason9Button.spriteController.callback = OnReportPlayer9;
		m_messageCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		m_reportCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		m_textCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		m_videoCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		m_facebookCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		m_feedbackSubmitBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_feedbackCancelBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsCancel;
		m_feedbackBugBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_feedbackSuggestionBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportSubmitBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_reportCancelBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsCancel;
		m_reportPlayerButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason1Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason2Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason3Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason4Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason5Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason6Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason7Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason8Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_reportPlayerReason9Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		m_facebookContinueBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		_ButtonSwapSprite[] componentsInChildren = base.gameObject.GetComponentsInChildren<_ButtonSwapSprite>(true);
		if (componentsInChildren != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].m_ignoreDialogboxes = true;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_textScrollRect != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_textScrollRect.verticalScrollbar.value = 1f;
		}
		m_textScrollRect.scrollSensitivity = 100f;
		UIManager.SetGameObjectActive(m_backgroundContainer, false);
		UIManager.SetGameObjectActive(m_contentContainer, false);
		m_reportReason = ClientFeedbackReport.FeedbackReason._0015;
		TextListItemBtns.AddRange(m_TextListItemParent.GetComponentsInChildren<_SelectableBtn>(true));
		for (int j = 0; j < TextListItemBtns.Count; j++)
		{
			TextListItemBtns[j].spriteController.callback = TextItemClicked;
		}
		TextListItemLabels.Add(m_TextListLabel);
		SetupDisplayInfo();
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_textContainer.GetComponent<LayoutGroup>());
		}
		base.Awake();
	}

	public bool IsActive()
	{
		int result;
		if (!m_messageVisible)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_feedbackVisible)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!m_reportVisible)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_facebookVisible)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!m_textVisible && !m_videoVisible)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							result = (Options_UI.Get().IsVisible() ? 1 : 0);
							goto IL_007f;
						}
					}
				}
			}
		}
		result = 1;
		goto IL_007f;
		IL_007f:
		return (byte)result != 0;
	}

	public bool IsVideoVisible()
	{
		return m_videoVisible;
	}

	private void DoVisible(bool visible)
	{
		m_windowVisible = visible;
		UIManager.SetGameObjectActive(m_backgroundContainer, true);
		UIManager.SetGameObjectActive(m_contentContainer, true);
		UIManager.SetGameObjectActive(m_animator, true);
		if (visible)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_animator.Play("PanelDefaultIN", 0, 0f);
					return;
				}
			}
		}
		m_animator.Play("PanelDefaultOUT", 0, 0f);
	}

	public void SetVisible(bool visible)
	{
		if (m_windowVisible == visible)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		DoVisible(visible);
	}

	public void CloseMenu()
	{
		if (m_messageVisible)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					CloseMessage(null);
					return;
				}
			}
		}
		if (m_feedbackVisible)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					CloseFeedback(null);
					return;
				}
			}
		}
		if (m_reportVisible)
		{
			CloseReport(null);
			return;
		}
		if (m_facebookVisible)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					CloseFacebook(null);
					return;
				}
			}
		}
		if (m_textVisible)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					CloseText(null);
					return;
				}
			}
		}
		if (m_videoVisible)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					CloseVideo(null);
					return;
				}
			}
		}
		if (Options_UI.Get().IsVisible())
		{
			SetOptionsContainerVisible(false);
		}
		else
		{
			if (!KeyBinding_UI.Get().IsVisible())
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				SetKeyBindingContainerVisible(false);
				return;
			}
		}
	}

	public void CloseMessage(BaseEventData data)
	{
		SetMessageContainerVisible(false);
		if (m_onClose == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_onClose();
			m_onClose = null;
			return;
		}
	}

	public void CloseText(BaseEventData data)
	{
		SetTextContainerVisible(false);
	}

	public void CloseVideo(BaseEventData data)
	{
		SetVideoContainerVisible(false);
	}

	public void CloseFeedback(BaseEventData data)
	{
		SetFeedbackContainerVisible(false);
	}

	public void CloseReport(BaseEventData data)
	{
		SetReportContainerVisible(false, string.Empty, 0L);
	}

	public void CloseFacebook(BaseEventData data)
	{
		SetFacebookContainerVisible(false, null);
	}

	public void ToggleMessageContainerVisible()
	{
		SetMessageContainerVisible(!m_messageContainer.gameObject.activeSelf);
	}

	public void ToggleFeedbackContainerVisible()
	{
		SetFeedbackContainerVisible(!m_feedbackContainer.gameObject.activeSelf);
	}

	public void ToggleReportContainerVisible()
	{
		SetReportContainerVisible(!m_reportContainer.gameObject.activeSelf, string.Empty, 0L);
	}

	public void ToggleFacebookContainerVisible()
	{
		SetReportContainerVisible(!m_facebookContainer.gameObject.activeSelf, string.Empty, 0L);
	}

	public void ToggleTextContainerVisible()
	{
		SetTextContainerVisible(!m_textContainer.gameObject.activeSelf);
	}

	public void DisplayVideo(string movieAssetName, string MovieTitle)
	{
		UIVideoOverlayPanel component = m_videoContainer.GetComponent<UIVideoOverlayPanel>();
		if ((bool)component)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SetVideoContainerVisible(true);
			component.PlayVideo(movieAssetName);
		}
		m_videoTextHeader.text = MovieTitle;
	}

	public void ToggleOptionsContainerVisible()
	{
		Options_UI.Get().ToggleOptions();
	}

	private void SetupDisplayInfo()
	{
		int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		int num = 0;
		for (int i = 0; i < SeasonWideData.Get().m_seasons.Count; i++)
		{
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(i + 1);
			if (seasonTemplate.IsTutorial)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			else
			{
				if (seasonTemplate.Index >= activeSeason)
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				int num2 = 0;
				while (num2 < seasonTemplate.Chapters.Count)
				{
					SeasonChapter seasonChapter = seasonTemplate.Chapters[num2];
					string displayName = seasonTemplate.GetDisplayName();
					string headerTextString = StringUtil.TR_SeasonStorytimeHeader(seasonTemplate.Index, num2 + 1, 1);
					List<string> list = new List<string>();
					for (int j = 0; j < seasonChapter.StorytimePanels.Count; j++)
					{
						list.Add(StringUtil.TR_SeasonStorytimeLongBody(seasonTemplate.Index, num2 + 1, j + 1));
					}
					if (num >= TextListItemBtns.Count)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						_SelectableBtn selectableBtn = UnityEngine.Object.Instantiate(m_TextListItem);
						UIManager.ReparentTransform(selectableBtn.transform, m_TextListItemParent.transform);
						selectableBtn.spriteController.callback = TextItemClicked;
						TextListItemBtns.Add(selectableBtn);
					}
					_SelectableBtn selectableBtn2 = TextListItemBtns[num];
					TextMeshProUGUI[] componentsInChildren = selectableBtn2.GetComponentsInChildren<TextMeshProUGUI>(true);
					for (int k = 0; k < componentsInChildren.Length; k++)
					{
						componentsInChildren[k].text = string.Format(StringUtil.TR("ChapterNumber", "Global"), num2 + 1);
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						string label = string.Empty;
						if (num2 == 0)
						{
							label = string.Format(StringUtil.TR("SeasonNumber", "Global"), seasonTemplate.GetPlayerFacingSeasonNumber());
						}
						SeasonLoreDisplayInfos.Add(new TextChapterDisplayInfo
						{
							Label = label,
							TitleTextString = displayName,
							HeaderTextString = headerTextString,
							ContentTextString = list,
							DisplayBtn = selectableBtn2
						});
						num++;
						num2++;
						goto IL_0229;
					}
					IL_0229:;
				}
				while (true)
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

	private void SetupSeasonLoreButtons()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < TextListItemBtns.Count; i++)
		{
			if (i < SeasonLoreDisplayInfos.Count)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!SeasonLoreDisplayInfos[i].Label.IsNullOrEmpty())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num >= TextListItemLabels.Count)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						TextMeshProUGUI textMeshProUGUI = UnityEngine.Object.Instantiate(m_TextListLabel);
						UIManager.ReparentTransform(textMeshProUGUI.transform, m_TextListItemParent.transform);
						TextListItemLabels.Add(textMeshProUGUI);
					}
					TextMeshProUGUI textMeshProUGUI2 = TextListItemLabels[num];
					UIManager.SetGameObjectActive(textMeshProUGUI2, true);
					textMeshProUGUI2.transform.SetSiblingIndex(num2);
					textMeshProUGUI2.text = SeasonLoreDisplayInfos[i].Label;
					num2++;
					num++;
				}
				SeasonLoreDisplayInfos[i].DisplayBtn.transform.SetSiblingIndex(num2);
				num2++;
				SeasonLoreDisplayInfos[i].DisplayBtn = TextListItemBtns[i];
			}
			else
			{
				UIManager.SetGameObjectActive(TextListItemBtns[i], false);
			}
		}
		for (int j = num; j < TextListItemLabels.Count; j++)
		{
			UIManager.SetGameObjectActive(TextListItemLabels[j], false);
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

	private void SetupLoreInfo()
	{
		if (SeasonLoreDisplayInfos.Count == 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SetupDisplayInfo();
		}
		SetupSeasonLoreButtons();
	}

	public void DisplayPreviousSeasonChapter()
	{
		SetTextContainerVisible(true, true);
		SetupLoreInfo();
		for (int i = 0; i < TextListItemBtns.Count; i++)
		{
			UIManager.SetGameObjectActive(TextListItemBtns[i], i < SeasonLoreDisplayInfos.Count);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (SeasonLoreDisplayInfos.Count > 0)
			{
				for (int j = 0; j < SeasonLoreDisplayInfos.Count - 1; j++)
				{
					SeasonLoreDisplayInfos[j].DisplayBtn.SetSelected(false, false, string.Empty, string.Empty);
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					SeasonLoreDisplayInfos[SeasonLoreDisplayInfos.Count - 1].DisplayBtn.SetSelected(true, false, string.Empty, string.Empty);
					DisplaySeasonLore(SeasonLoreDisplayInfos.Count - 1);
					return;
				}
			}
			return;
		}
	}

	private IEnumerator SetupText(int index)
	{
		m_textHeader.text = SeasonLoreDisplayInfos[index].TitleTextString;
		yield return 0;
		/*Error: Unable to find new state assignment for yield return*/;
	}

	private void DisplaySeasonLore(int index, bool stagger = false)
	{
		if (stagger)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			StartCoroutine(SetupText(index));
		}
		else
		{
			m_textHeader.text = SeasonLoreDisplayInfos[index].TitleTextString;
			m_textDescription.text = SeasonLoreDisplayInfos[index].HeaderTextString;
			string text = string.Empty;
			for (int i = 0; i < SeasonLoreDisplayInfos[index].ContentTextString.Count; i++)
			{
				text += SeasonLoreDisplayInfos[index].ContentTextString[i];
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_textContent.text = text;
		}
		UIManager.SetGameObjectActive(m_textDescription, true);
		UIManager.SetGameObjectActive(m_textImage, false);
		SetTextContainerVisible(true, true);
	}

	public void TextItemClicked(BaseEventData data)
	{
		for (int i = 0; i < SeasonLoreDisplayInfos.Count; i++)
		{
			if (SeasonLoreDisplayInfos[i].DisplayBtn.spriteController.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				SeasonLoreDisplayInfos[i].DisplayBtn.SetSelected(true, false, string.Empty, string.Empty);
				DisplaySeasonLore(i, true);
			}
			else
			{
				SeasonLoreDisplayInfos[i].DisplayBtn.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void DisplayWhatsNew()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		string language = HydrogenConfig.Get().Language;
		m_textDescription.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewDescription, language);
		m_textHeader.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewHeader, language);
		m_textContent.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewText, language);
		UIManager.SetGameObjectActive(m_textDescription, true);
		UIManager.SetGameObjectActive(m_textImage, false);
		SetTextContainerVisible(true);
	}

	public void DisplayPatchNotes()
	{
		string language = HydrogenConfig.Get().Language;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		m_textDescription.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesDescription, language);
		m_textHeader.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesHeader, language);
		m_textContent.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesText, language);
		UIManager.SetGameObjectActive(m_textDescription, true);
		UIManager.SetGameObjectActive(m_textImage, false);
		SetTextContainerVisible(true);
	}

	public void DisplayLoreArticle(LoreArticle article)
	{
		m_textHeader.text = article.GetTitle();
		m_textContent.text = article.GetArticleText();
		m_textImage.sprite = Resources.Load<Sprite>(article.ImagePath);
		UIManager.SetGameObjectActive(m_textDescription, false);
		UIManager.SetGameObjectActive(m_textImage, m_textImage.sprite != null);
		SetTextContainerVisible(true);
	}

	public void DisplayMessage(string title, string content, Action onClose = null)
	{
		m_broadcastMessageTitle.text = title;
		m_broadcastMessageContent.text = content;
		SetMessageContainerVisible(true);
		m_onClose = onClose;
	}

	public void SubmitFeedback(BaseEventData data)
	{
		ClientFeedbackReport clientFeedbackReport = new ClientFeedbackReport();
		clientFeedbackReport.Message = m_feedbackInput.text;
		if (m_feedbackSuggestionBtn.IsSelected())
		{
			clientFeedbackReport.Reason = ClientFeedbackReport.FeedbackReason._000E;
		}
		else if (m_feedbackBugBtn.IsSelected())
		{
			clientFeedbackReport.Reason = ClientFeedbackReport.FeedbackReason._0012;
		}
		ClientGameManager.Get().SendFeedbackReport(clientFeedbackReport);
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReportSent", "Global"), StringUtil.TR("BugReportSentBody", "Global"), StringUtil.TR("Ok", "Global"));
		CloseFeedback(data);
	}

	public void SubmitReport(BaseEventData data)
	{
		if (!m_botMasqueradingAsHuman)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientFeedbackReport clientFeedbackReport = new ClientFeedbackReport();
			clientFeedbackReport.Reason = m_reportReason;
			clientFeedbackReport.ReportedPlayerHandle = m_reportPlayerHandle;
			clientFeedbackReport.ReportedPlayerAccountId = m_reportPlayerAccountId;
			clientFeedbackReport.Message = m_reportInput.text;
			ClientGameManager.Get().SendFeedbackReport(clientFeedbackReport);
		}
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReportSent", "Global"), StringUtil.TR("YourReportWasSent", "Global"), StringUtil.TR("Ok", "Global"));
		CloseReport(data);
	}

	public void ContinueFacebook(BaseEventData data)
	{
		ClientGameManager.Get().FacebookShareScreenshot(m_facebookInput.text);
		CloseFacebook(data);
	}

	public void SuggestionClicked(BaseEventData data)
	{
		m_feedbackSuggestionBtn.SetSelected(true, false, string.Empty, string.Empty);
		m_feedbackBugBtn.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void BugClicked(BaseEventData data)
	{
		m_feedbackSuggestionBtn.SetSelected(false, false, string.Empty, string.Empty);
		m_feedbackBugBtn.SetSelected(true, false, string.Empty, string.Empty);
	}

	private void CheckBG()
	{
		bool visible = IsActive();
		SetVisible(visible);
	}

	public void SetMessageContainerVisible(bool visible)
	{
		m_messageVisible = visible;
		UIManager.SetGameObjectActive(m_messageContainer, visible);
		CheckBG();
	}

	public bool IsMessageContainerVisible()
	{
		return m_messageVisible;
	}

	public void SetFeedbackContainerVisible(bool visible)
	{
		m_feedbackVisible = visible;
		UIManager.SetGameObjectActive(m_feedbackContainer, visible);
		if (visible)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_feedbackInput.text = string.Empty;
			float preferredHeight = m_feedbackInput.textComponent.preferredHeight;
			float a = preferredHeight;
			Vector2 sizeDelta = (m_feedbackTextContainer.transform.parent.transform as RectTransform).sizeDelta;
			preferredHeight = Mathf.Max(a, sizeDelta.y);
			RectTransform obj = m_feedbackTextContainer.transform as RectTransform;
			Vector2 sizeDelta2 = (m_feedbackTextContainer.transform as RectTransform).sizeDelta;
			obj.sizeDelta = new Vector2(sizeDelta2.x, preferredHeight);
			m_feedbackSuggestionBtn.SetSelected(true, false, string.Empty, string.Empty);
			m_feedbackBugBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		m_shouldSelectFeedbackInput = visible;
		CheckBG();
	}

	public void SetReportContainerVisible(bool visible, string playerHandle = "", long playerAccountId = 0L, bool masqueradeBot = false)
	{
		m_reportVisible = visible;
		UIManager.SetGameObjectActive(m_reportContainer, visible);
		if (visible)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_reportPlayerHandle = playerHandle;
			m_reportPlayerAccountId = playerAccountId;
			m_botMasqueradingAsHuman = masqueradeBot;
			m_reportPlayerHeader.text = string.Format(StringUtil.TR("ReportPlayerTitle", "Global"), m_reportPlayerHandle);
			m_reportInput.text = string.Empty;
			float preferredHeight = m_reportInput.textComponent.preferredHeight;
			float a = preferredHeight;
			Vector2 sizeDelta = (m_reportTextContainer.transform.parent.transform as RectTransform).sizeDelta;
			preferredHeight = Mathf.Max(a, sizeDelta.y);
			RectTransform obj = m_reportTextContainer.transform as RectTransform;
			Vector2 sizeDelta2 = (m_reportTextContainer.transform as RectTransform).sizeDelta;
			obj.sizeDelta = new Vector2(sizeDelta2.x, preferredHeight);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
			m_reportPlayerHandle = string.Empty;
			m_reportPlayerAccountId = 0L;
			m_botMasqueradingAsHuman = false;
		}
		m_shouldSelectReportInput = visible;
		CheckBG();
	}

	public void SetFacebookContainerVisible(bool visible, Texture2D texture)
	{
		m_facebookVisible = visible;
		UIManager.SetGameObjectActive(m_facebookContainer, visible);
		if (visible)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_facebookInput.text = string.Empty;
			m_facebookScreenshotPreview.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one * 0.5f);
			m_facebookScreenshotPreview.enabled = true;
			float preferredHeight = m_facebookInput.textComponent.preferredHeight;
			float a = preferredHeight;
			Vector2 sizeDelta = (m_facebookTextContainer.transform.parent.transform as RectTransform).sizeDelta;
			preferredHeight = Mathf.Max(a, sizeDelta.y);
			RectTransform obj = m_facebookTextContainer.transform as RectTransform;
			Vector2 sizeDelta2 = (m_facebookTextContainer.transform as RectTransform).sizeDelta;
			obj.sizeDelta = new Vector2(sizeDelta2.x, preferredHeight);
		}
		else
		{
			m_facebookScreenshotPreview.enabled = false;
			EventSystem.current.SetSelectedGameObject(null);
		}
		m_shouldSelectFacebookInput = visible;
		CheckBG();
	}

	public void SetTextContainerVisible(bool visible, bool setTextListVisible = false)
	{
		StaggerComponent.SetStaggerComponent(m_TextItemListContainer.gameObject, setTextListVisible);
		if (m_textVisible != visible)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_textVisible = visible;
			StaggerComponent.SetStaggerComponent(m_TextContainerContainer.gameObject, visible);
			UIManager.SetGameObjectActive(m_textContainer, visible);
			CheckBG();
		}
		if (!visible)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_setScrollBar = 2;
			m_textScrollRect.verticalNormalizedPosition = 1f;
			return;
		}
	}

	public void SetVideoContainerVisible(bool visible)
	{
		if (m_videoVisible != visible && GameFlowData.Get() != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get() != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameManager.Get().IsAllowingPlayerRequestedPause())
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
						activeOwnedActorData.GetActorController().RequestCustomGamePause(visible, activeOwnedActorData.ActorIndex);
					}
				}
			}
		}
		m_videoVisible = visible;
		UIManager.SetGameObjectActive(m_videoContainer, visible);
		CheckBG();
	}

	public void SetOptionsContainerVisible(bool visible)
	{
		if (visible)
		{
			Options_UI.Get().ShowOptions();
		}
		else
		{
			Options_UI.Get().HideOptions();
		}
		CheckBG();
	}

	public void SetKeyBindingContainerVisible(bool visible)
	{
		if (visible)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			KeyBinding_UI.Get().ShowKeybinds();
		}
		else
		{
			KeyBinding_UI.Get().HideKeybinds();
		}
		CheckBG();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		if (m_shouldSelectFeedbackInput)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (EventSystem.current.currentSelectedGameObject != m_feedbackInput.gameObject)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				CleanCurrentlySelectedTextConsole();
				EventSystem.current.SetSelectedGameObject(m_feedbackInput.gameObject);
				goto IL_011b;
			}
		}
		if (m_shouldSelectReportInput && EventSystem.current.currentSelectedGameObject != m_reportInput.gameObject)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			CleanCurrentlySelectedTextConsole();
			EventSystem.current.SetSelectedGameObject(m_reportInput.gameObject);
		}
		else if (m_shouldSelectFacebookInput)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (EventSystem.current.currentSelectedGameObject != m_facebookInput.gameObject)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				CleanCurrentlySelectedTextConsole();
				EventSystem.current.SetSelectedGameObject(m_facebookInput.gameObject);
			}
		}
		goto IL_011b;
		IL_011b:
		if (m_setScrollBar <= 0)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_setScrollBar--;
			if (m_setScrollBar == 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					m_textScrollRect.verticalScrollbar.value = 1f;
					return;
				}
			}
			return;
		}
	}

	private void CleanCurrentlySelectedTextConsole()
	{
		if (!(EventSystem.current.currentSelectedGameObject != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UITextConsole componentInParent = EventSystem.current.currentSelectedGameObject.GetComponentInParent<UITextConsole>();
			if (componentInParent != null)
			{
				componentInParent.Hide();
			}
			return;
		}
	}

	public void SetReportPlayerText(string text)
	{
		for (int i = 0; i < m_reportPlayerText.Length; i++)
		{
			m_reportPlayerText[i].text = text;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void OnReportPlayer(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, !m_reportPlayerDropdown.activeSelf);
	}

	public void OnReportPlayer1(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0016;
		SetReportPlayerText(StringUtil.TR("VerbalHarassment", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer2(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0013;
		SetReportPlayerText(StringUtil.TR("LeavingtheGameAFKing", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer3(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0018;
		SetReportPlayerText(StringUtil.TR("HateSpeech", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer4(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0009;
		SetReportPlayerText(StringUtil.TR("IntentionallyFeeding", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer5(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0004;
		SetReportPlayerText(StringUtil.TR("Botting", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer6(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0019;
		SetReportPlayerText(StringUtil.TR("SpammingAdvertising", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer7(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0011;
		SetReportPlayerText(StringUtil.TR("OffensiveName", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer8(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._0015;
		SetReportPlayerText(StringUtil.TR("UnsportsmanlikeConduct", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}

	public void OnReportPlayer9(BaseEventData data)
	{
		m_reportReason = ClientFeedbackReport.FeedbackReason._001A;
		SetReportPlayerText(StringUtil.TR("Other", "PersistentScene"));
		UIManager.SetGameObjectActive(m_reportPlayerDropdown, false);
	}
}
