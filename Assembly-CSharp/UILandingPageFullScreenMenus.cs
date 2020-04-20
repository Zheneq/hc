using System;
using System.Collections;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILandingPageFullScreenMenus : UIScene
{
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

	private List<UILandingPageFullScreenMenus.TextChapterDisplayInfo> SeasonLoreDisplayInfos = new List<UILandingPageFullScreenMenus.TextChapterDisplayInfo>();

	private Action m_onClose = delegate()
	{
	};

	public static UILandingPageFullScreenMenus Get()
	{
		return UILandingPageFullScreenMenus.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.AdvancedOverlays;
	}

	public override void Awake()
	{
		UILandingPageFullScreenMenus.s_instance = this;
		UIManager.SetGameObjectActive(this.m_messageContainer, false, null);
		UIManager.SetGameObjectActive(this.m_feedbackContainer, false, null);
		UIManager.SetGameObjectActive(this.m_reportContainer, false, null);
		UIManager.SetGameObjectActive(this.m_facebookContainer, false, null);
		UIManager.SetGameObjectActive(this.m_textContainer, false, null);
		UIManager.SetGameObjectActive(this.m_videoContainer, false, null);
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
		this.m_messageCloseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseMessage);
		this.m_textCloseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseText);
		this.m_videoCloseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseVideo);
		this.m_feedbackCloseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseFeedback);
		this.m_reportCloseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseReport);
		this.m_facebookCloseBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseFacebook);
		this.m_feedbackSubmitBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SubmitFeedback);
		this.m_feedbackCancelBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseFeedback);
		this.m_feedbackSuggestionBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SuggestionClicked);
		this.m_feedbackBugBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BugClicked);
		_MouseEventPasser mouseEventPasser = this.m_textHeader.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(this.m_textScrollRect);
		mouseEventPasser = this.m_textDescription.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(this.m_textScrollRect);
		mouseEventPasser = this.m_textContent.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(this.m_textScrollRect);
		this.m_textScrollRect.movementType = ScrollRect.MovementType.Clamped;
		this.m_reportSubmitBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SubmitReport);
		this.m_reportCancelBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseReport);
		this.m_facebookContinueBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ContinueFacebook);
		this.m_reportPlayerButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer);
		this.m_reportPlayerReason1Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer1);
		this.m_reportPlayerReason2Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer2);
		this.m_reportPlayerReason3Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer3);
		this.m_reportPlayerReason4Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer4);
		this.m_reportPlayerReason5Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer5);
		this.m_reportPlayerReason6Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer6);
		this.m_reportPlayerReason7Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer7);
		this.m_reportPlayerReason8Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer8);
		this.m_reportPlayerReason9Button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnReportPlayer9);
		this.m_messageCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		this.m_reportCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		this.m_textCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		this.m_videoCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		this.m_facebookCloseBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		this.m_feedbackSubmitBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_feedbackCancelBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsCancel;
		this.m_feedbackBugBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_feedbackSuggestionBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportSubmitBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_reportCancelBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsCancel;
		this.m_reportPlayerButton.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason1Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason2Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason3Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason4Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason5Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason6Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason7Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason8Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_reportPlayerReason9Button.spriteController.m_soundToPlay = FrontEndButtonSounds.OptionsChoice;
		this.m_facebookContinueBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		_ButtonSwapSprite[] componentsInChildren = base.gameObject.GetComponentsInChildren<_ButtonSwapSprite>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].m_ignoreDialogboxes = true;
			}
		}
		if (this.m_textScrollRect != null)
		{
			this.m_textScrollRect.verticalScrollbar.value = 1f;
		}
		this.m_textScrollRect.scrollSensitivity = 100f;
		UIManager.SetGameObjectActive(this.m_backgroundContainer, false, null);
		UIManager.SetGameObjectActive(this.m_contentContainer, false, null);
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0015;
		this.TextListItemBtns.AddRange(this.m_TextListItemParent.GetComponentsInChildren<_SelectableBtn>(true));
		for (int j = 0; j < this.TextListItemBtns.Count; j++)
		{
			this.TextListItemBtns[j].spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TextItemClicked);
		}
		this.TextListItemLabels.Add(this.m_TextListLabel);
		this.SetupDisplayInfo();
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_textContainer.GetComponent<LayoutGroup>());
		}
		base.Awake();
	}

	public bool IsActive()
	{
		if (!this.m_messageVisible)
		{
			if (!this.m_feedbackVisible)
			{
				if (!this.m_reportVisible)
				{
					if (!this.m_facebookVisible)
					{
						if (!this.m_textVisible && !this.m_videoVisible)
						{
							return Options_UI.Get().IsVisible();
						}
					}
				}
			}
		}
		return true;
	}

	public bool IsVideoVisible()
	{
		return this.m_videoVisible;
	}

	private void DoVisible(bool visible)
	{
		this.m_windowVisible = visible;
		UIManager.SetGameObjectActive(this.m_backgroundContainer, true, null);
		UIManager.SetGameObjectActive(this.m_contentContainer, true, null);
		UIManager.SetGameObjectActive(this.m_animator, true, null);
		if (visible)
		{
			this.m_animator.Play("PanelDefaultIN", 0, 0f);
		}
		else
		{
			this.m_animator.Play("PanelDefaultOUT", 0, 0f);
		}
	}

	public void SetVisible(bool visible)
	{
		if (this.m_windowVisible == visible)
		{
			return;
		}
		this.DoVisible(visible);
	}

	public void CloseMenu()
	{
		if (this.m_messageVisible)
		{
			this.CloseMessage(null);
		}
		else if (this.m_feedbackVisible)
		{
			this.CloseFeedback(null);
		}
		else if (this.m_reportVisible)
		{
			this.CloseReport(null);
		}
		else if (this.m_facebookVisible)
		{
			this.CloseFacebook(null);
		}
		else if (this.m_textVisible)
		{
			this.CloseText(null);
		}
		else if (this.m_videoVisible)
		{
			this.CloseVideo(null);
		}
		else if (Options_UI.Get().IsVisible())
		{
			this.SetOptionsContainerVisible(false);
		}
		else if (KeyBinding_UI.Get().IsVisible())
		{
			this.SetKeyBindingContainerVisible(false);
		}
	}

	public void CloseMessage(BaseEventData data)
	{
		this.SetMessageContainerVisible(false);
		if (this.m_onClose != null)
		{
			this.m_onClose();
			this.m_onClose = null;
		}
	}

	public void CloseText(BaseEventData data)
	{
		this.SetTextContainerVisible(false, false);
	}

	public void CloseVideo(BaseEventData data)
	{
		this.SetVideoContainerVisible(false);
	}

	public void CloseFeedback(BaseEventData data)
	{
		this.SetFeedbackContainerVisible(false);
	}

	public void CloseReport(BaseEventData data)
	{
		this.SetReportContainerVisible(false, string.Empty, 0L, false);
	}

	public void CloseFacebook(BaseEventData data)
	{
		this.SetFacebookContainerVisible(false, null);
	}

	public void ToggleMessageContainerVisible()
	{
		this.SetMessageContainerVisible(!this.m_messageContainer.gameObject.activeSelf);
	}

	public void ToggleFeedbackContainerVisible()
	{
		this.SetFeedbackContainerVisible(!this.m_feedbackContainer.gameObject.activeSelf);
	}

	public void ToggleReportContainerVisible()
	{
		this.SetReportContainerVisible(!this.m_reportContainer.gameObject.activeSelf, string.Empty, 0L, false);
	}

	public void ToggleFacebookContainerVisible()
	{
		this.SetReportContainerVisible(!this.m_facebookContainer.gameObject.activeSelf, string.Empty, 0L, false);
	}

	public void ToggleTextContainerVisible()
	{
		this.SetTextContainerVisible(!this.m_textContainer.gameObject.activeSelf, false);
	}

	public void DisplayVideo(string movieAssetName, string MovieTitle)
	{
		UIVideoOverlayPanel component = this.m_videoContainer.GetComponent<UIVideoOverlayPanel>();
		if (component)
		{
			this.SetVideoContainerVisible(true);
			component.PlayVideo(movieAssetName);
		}
		this.m_videoTextHeader.text = MovieTitle;
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
			}
			else if (seasonTemplate.Index < activeSeason)
			{
				for (int j = 0; j < seasonTemplate.Chapters.Count; j++)
				{
					SeasonChapter seasonChapter = seasonTemplate.Chapters[j];
					string displayName = seasonTemplate.GetDisplayName();
					string headerTextString = StringUtil.TR_SeasonStorytimeHeader(seasonTemplate.Index, j + 1, 1);
					List<string> list = new List<string>();
					for (int k = 0; k < seasonChapter.StorytimePanels.Count; k++)
					{
						list.Add(StringUtil.TR_SeasonStorytimeLongBody(seasonTemplate.Index, j + 1, k + 1));
					}
					if (num >= this.TextListItemBtns.Count)
					{
						_SelectableBtn selectableBtn = UnityEngine.Object.Instantiate<_SelectableBtn>(this.m_TextListItem);
						UIManager.ReparentTransform(selectableBtn.transform, this.m_TextListItemParent.transform);
						selectableBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TextItemClicked);
						this.TextListItemBtns.Add(selectableBtn);
					}
					_SelectableBtn selectableBtn2 = this.TextListItemBtns[num];
					TextMeshProUGUI[] componentsInChildren = selectableBtn2.GetComponentsInChildren<TextMeshProUGUI>(true);
					for (int l = 0; l < componentsInChildren.Length; l++)
					{
						componentsInChildren[l].text = string.Format(StringUtil.TR("ChapterNumber", "Global"), j + 1);
					}
					string label = string.Empty;
					if (j == 0)
					{
						label = string.Format(StringUtil.TR("SeasonNumber", "Global"), seasonTemplate.GetPlayerFacingSeasonNumber());
					}
					this.SeasonLoreDisplayInfos.Add(new UILandingPageFullScreenMenus.TextChapterDisplayInfo
					{
						Label = label,
						TitleTextString = displayName,
						HeaderTextString = headerTextString,
						ContentTextString = list,
						DisplayBtn = selectableBtn2
					});
					num++;
				}
			}
		}
	}

	private void SetupSeasonLoreButtons()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.TextListItemBtns.Count; i++)
		{
			if (i < this.SeasonLoreDisplayInfos.Count)
			{
				if (!this.SeasonLoreDisplayInfos[i].Label.IsNullOrEmpty())
				{
					if (num >= this.TextListItemLabels.Count)
					{
						TextMeshProUGUI textMeshProUGUI = UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.m_TextListLabel);
						UIManager.ReparentTransform(textMeshProUGUI.transform, this.m_TextListItemParent.transform);
						this.TextListItemLabels.Add(textMeshProUGUI);
					}
					TextMeshProUGUI textMeshProUGUI2 = this.TextListItemLabels[num];
					UIManager.SetGameObjectActive(textMeshProUGUI2, true, null);
					textMeshProUGUI2.transform.SetSiblingIndex(num2);
					textMeshProUGUI2.text = this.SeasonLoreDisplayInfos[i].Label;
					num2++;
					num++;
				}
				this.SeasonLoreDisplayInfos[i].DisplayBtn.transform.SetSiblingIndex(num2);
				num2++;
				this.SeasonLoreDisplayInfos[i].DisplayBtn = this.TextListItemBtns[i];
			}
			else
			{
				UIManager.SetGameObjectActive(this.TextListItemBtns[i], false, null);
			}
		}
		for (int j = num; j < this.TextListItemLabels.Count; j++)
		{
			UIManager.SetGameObjectActive(this.TextListItemLabels[j], false, null);
		}
	}

	private void SetupLoreInfo()
	{
		if (this.SeasonLoreDisplayInfos.Count == 0)
		{
			this.SetupDisplayInfo();
		}
		this.SetupSeasonLoreButtons();
	}

	public void DisplayPreviousSeasonChapter()
	{
		this.SetTextContainerVisible(true, true);
		this.SetupLoreInfo();
		for (int i = 0; i < this.TextListItemBtns.Count; i++)
		{
			UIManager.SetGameObjectActive(this.TextListItemBtns[i], i < this.SeasonLoreDisplayInfos.Count, null);
		}
		if (this.SeasonLoreDisplayInfos.Count > 0)
		{
			for (int j = 0; j < this.SeasonLoreDisplayInfos.Count - 1; j++)
			{
				this.SeasonLoreDisplayInfos[j].DisplayBtn.SetSelected(false, false, string.Empty, string.Empty);
			}
			this.SeasonLoreDisplayInfos[this.SeasonLoreDisplayInfos.Count - 1].DisplayBtn.SetSelected(true, false, string.Empty, string.Empty);
			this.DisplaySeasonLore(this.SeasonLoreDisplayInfos.Count - 1, false);
		}
	}

	private IEnumerator SetupText(int index)
	{
		this.m_textHeader.text = this.SeasonLoreDisplayInfos[index].TitleTextString;
		yield return 0;
		this.m_textDescription.text = this.SeasonLoreDisplayInfos[index].HeaderTextString;
		yield return 0;
		string ContentString = string.Empty;
		for (int i = 0; i < this.SeasonLoreDisplayInfos[index].ContentTextString.Count; i++)
		{
			ContentString += this.SeasonLoreDisplayInfos[index].ContentTextString[i];
		}
		this.m_textContent.text = ContentString;
		yield break;
	}

	private void DisplaySeasonLore(int index, bool stagger = false)
	{
		if (stagger)
		{
			base.StartCoroutine(this.SetupText(index));
		}
		else
		{
			this.m_textHeader.text = this.SeasonLoreDisplayInfos[index].TitleTextString;
			this.m_textDescription.text = this.SeasonLoreDisplayInfos[index].HeaderTextString;
			string text = string.Empty;
			for (int i = 0; i < this.SeasonLoreDisplayInfos[index].ContentTextString.Count; i++)
			{
				text += this.SeasonLoreDisplayInfos[index].ContentTextString[i];
			}
			this.m_textContent.text = text;
		}
		UIManager.SetGameObjectActive(this.m_textDescription, true, null);
		UIManager.SetGameObjectActive(this.m_textImage, false, null);
		this.SetTextContainerVisible(true, true);
	}

	public void TextItemClicked(BaseEventData data)
	{
		for (int i = 0; i < this.SeasonLoreDisplayInfos.Count; i++)
		{
			if (this.SeasonLoreDisplayInfos[i].DisplayBtn.spriteController.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				this.SeasonLoreDisplayInfos[i].DisplayBtn.SetSelected(true, false, string.Empty, string.Empty);
				this.DisplaySeasonLore(i, true);
			}
			else
			{
				this.SeasonLoreDisplayInfos[i].DisplayBtn.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
	}

	public void DisplayWhatsNew()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		string language = HydrogenConfig.Get().Language;
		this.m_textDescription.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewDescription, language);
		this.m_textHeader.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewHeader, language);
		this.m_textContent.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewText, language);
		UIManager.SetGameObjectActive(this.m_textDescription, true, null);
		UIManager.SetGameObjectActive(this.m_textImage, false, null);
		this.SetTextContainerVisible(true, false);
	}

	public void DisplayPatchNotes()
	{
		string language = HydrogenConfig.Get().Language;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		this.m_textDescription.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesDescription, language);
		this.m_textHeader.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesHeader, language);
		this.m_textContent.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesText, language);
		UIManager.SetGameObjectActive(this.m_textDescription, true, null);
		UIManager.SetGameObjectActive(this.m_textImage, false, null);
		this.SetTextContainerVisible(true, false);
	}

	public void DisplayLoreArticle(LoreArticle article)
	{
		this.m_textHeader.text = article.GetTitle();
		this.m_textContent.text = article.GetArticleText();
		this.m_textImage.sprite = Resources.Load<Sprite>(article.ImagePath);
		UIManager.SetGameObjectActive(this.m_textDescription, false, null);
		UIManager.SetGameObjectActive(this.m_textImage, this.m_textImage.sprite != null, null);
		this.SetTextContainerVisible(true, false);
	}

	public void DisplayMessage(string title, string content, Action onClose = null)
	{
		this.m_broadcastMessageTitle.text = title;
		this.m_broadcastMessageContent.text = content;
		this.SetMessageContainerVisible(true);
		this.m_onClose = onClose;
	}

	public void SubmitFeedback(BaseEventData data)
	{
		ClientFeedbackReport clientFeedbackReport = new ClientFeedbackReport();
		clientFeedbackReport.Message = this.m_feedbackInput.text;
		if (this.m_feedbackSuggestionBtn.IsSelected())
		{
			clientFeedbackReport.Reason = ClientFeedbackReport.FeedbackReason.symbol_000E;
		}
		else if (this.m_feedbackBugBtn.IsSelected())
		{
			clientFeedbackReport.Reason = ClientFeedbackReport.FeedbackReason.symbol_0012;
		}
		ClientGameManager.Get().SendFeedbackReport(clientFeedbackReport);
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReportSent", "Global"), StringUtil.TR("BugReportSentBody", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
		this.CloseFeedback(data);
	}

	public void SubmitReport(BaseEventData data)
	{
		if (!this.m_botMasqueradingAsHuman)
		{
			ClientFeedbackReport clientFeedbackReport = new ClientFeedbackReport();
			clientFeedbackReport.Reason = this.m_reportReason;
			clientFeedbackReport.ReportedPlayerHandle = this.m_reportPlayerHandle;
			clientFeedbackReport.ReportedPlayerAccountId = this.m_reportPlayerAccountId;
			clientFeedbackReport.Message = this.m_reportInput.text;
			ClientGameManager.Get().SendFeedbackReport(clientFeedbackReport);
		}
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ReportSent", "Global"), StringUtil.TR("YourReportWasSent", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
		this.CloseReport(data);
	}

	public void ContinueFacebook(BaseEventData data)
	{
		ClientGameManager.Get().FacebookShareScreenshot(this.m_facebookInput.text);
		this.CloseFacebook(data);
	}

	public void SuggestionClicked(BaseEventData data)
	{
		this.m_feedbackSuggestionBtn.SetSelected(true, false, string.Empty, string.Empty);
		this.m_feedbackBugBtn.SetSelected(false, false, string.Empty, string.Empty);
	}

	public void BugClicked(BaseEventData data)
	{
		this.m_feedbackSuggestionBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_feedbackBugBtn.SetSelected(true, false, string.Empty, string.Empty);
	}

	private void CheckBG()
	{
		bool visible = this.IsActive();
		this.SetVisible(visible);
	}

	public void SetMessageContainerVisible(bool visible)
	{
		this.m_messageVisible = visible;
		UIManager.SetGameObjectActive(this.m_messageContainer, visible, null);
		this.CheckBG();
	}

	public bool IsMessageContainerVisible()
	{
		return this.m_messageVisible;
	}

	public void SetFeedbackContainerVisible(bool visible)
	{
		this.m_feedbackVisible = visible;
		UIManager.SetGameObjectActive(this.m_feedbackContainer, visible, null);
		if (visible)
		{
			this.m_feedbackInput.text = string.Empty;
			float num = this.m_feedbackInput.textComponent.preferredHeight;
			num = Mathf.Max(num, (this.m_feedbackTextContainer.transform.parent.transform as RectTransform).sizeDelta.y);
			(this.m_feedbackTextContainer.transform as RectTransform).sizeDelta = new Vector2((this.m_feedbackTextContainer.transform as RectTransform).sizeDelta.x, num);
			this.m_feedbackSuggestionBtn.SetSelected(true, false, string.Empty, string.Empty);
			this.m_feedbackBugBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		this.m_shouldSelectFeedbackInput = visible;
		this.CheckBG();
	}

	public void SetReportContainerVisible(bool visible, string playerHandle = "", long playerAccountId = 0L, bool masqueradeBot = false)
	{
		this.m_reportVisible = visible;
		UIManager.SetGameObjectActive(this.m_reportContainer, visible, null);
		if (visible)
		{
			this.m_reportPlayerHandle = playerHandle;
			this.m_reportPlayerAccountId = playerAccountId;
			this.m_botMasqueradingAsHuman = masqueradeBot;
			this.m_reportPlayerHeader.text = string.Format(StringUtil.TR("ReportPlayerTitle", "Global"), this.m_reportPlayerHandle);
			this.m_reportInput.text = string.Empty;
			float num = this.m_reportInput.textComponent.preferredHeight;
			num = Mathf.Max(num, (this.m_reportTextContainer.transform.parent.transform as RectTransform).sizeDelta.y);
			(this.m_reportTextContainer.transform as RectTransform).sizeDelta = new Vector2((this.m_reportTextContainer.transform as RectTransform).sizeDelta.x, num);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
			this.m_reportPlayerHandle = string.Empty;
			this.m_reportPlayerAccountId = 0L;
			this.m_botMasqueradingAsHuman = false;
		}
		this.m_shouldSelectReportInput = visible;
		this.CheckBG();
	}

	public void SetFacebookContainerVisible(bool visible, Texture2D texture)
	{
		this.m_facebookVisible = visible;
		UIManager.SetGameObjectActive(this.m_facebookContainer, visible, null);
		if (visible)
		{
			this.m_facebookInput.text = string.Empty;
			this.m_facebookScreenshotPreview.sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), Vector2.one * 0.5f);
			this.m_facebookScreenshotPreview.enabled = true;
			float num = this.m_facebookInput.textComponent.preferredHeight;
			num = Mathf.Max(num, (this.m_facebookTextContainer.transform.parent.transform as RectTransform).sizeDelta.y);
			(this.m_facebookTextContainer.transform as RectTransform).sizeDelta = new Vector2((this.m_facebookTextContainer.transform as RectTransform).sizeDelta.x, num);
		}
		else
		{
			this.m_facebookScreenshotPreview.enabled = false;
			EventSystem.current.SetSelectedGameObject(null);
		}
		this.m_shouldSelectFacebookInput = visible;
		this.CheckBG();
	}

	public void SetTextContainerVisible(bool visible, bool setTextListVisible = false)
	{
		StaggerComponent.SetStaggerComponent(this.m_TextItemListContainer.gameObject, setTextListVisible, true);
		if (this.m_textVisible != visible)
		{
			this.m_textVisible = visible;
			StaggerComponent.SetStaggerComponent(this.m_TextContainerContainer.gameObject, visible, true);
			UIManager.SetGameObjectActive(this.m_textContainer, visible, null);
			this.CheckBG();
		}
		if (visible)
		{
			this.m_setScrollBar = 2;
			this.m_textScrollRect.verticalNormalizedPosition = 1f;
		}
	}

	public void SetVideoContainerVisible(bool visible)
	{
		if (this.m_videoVisible != visible && GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (GameManager.Get() != null)
				{
					if (GameManager.Get().IsAllowingPlayerRequestedPause())
					{
						ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
						activeOwnedActorData.GetActorController().RequestCustomGamePause(visible, activeOwnedActorData.ActorIndex);
					}
				}
			}
		}
		this.m_videoVisible = visible;
		UIManager.SetGameObjectActive(this.m_videoContainer, visible, null);
		this.CheckBG();
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
		this.CheckBG();
	}

	public void SetKeyBindingContainerVisible(bool visible)
	{
		if (visible)
		{
			KeyBinding_UI.Get().ShowKeybinds();
		}
		else
		{
			KeyBinding_UI.Get().HideKeybinds();
		}
		this.CheckBG();
	}

	private void OnDestroy()
	{
		UILandingPageFullScreenMenus.s_instance = null;
	}

	private void Update()
	{
		if (this.m_shouldSelectFeedbackInput)
		{
			if (EventSystem.current.currentSelectedGameObject != this.m_feedbackInput.gameObject)
			{
				this.CleanCurrentlySelectedTextConsole();
				EventSystem.current.SetSelectedGameObject(this.m_feedbackInput.gameObject);
				goto IL_11B;
			}
		}
		if (this.m_shouldSelectReportInput && EventSystem.current.currentSelectedGameObject != this.m_reportInput.gameObject)
		{
			this.CleanCurrentlySelectedTextConsole();
			EventSystem.current.SetSelectedGameObject(this.m_reportInput.gameObject);
		}
		else if (this.m_shouldSelectFacebookInput)
		{
			if (EventSystem.current.currentSelectedGameObject != this.m_facebookInput.gameObject)
			{
				this.CleanCurrentlySelectedTextConsole();
				EventSystem.current.SetSelectedGameObject(this.m_facebookInput.gameObject);
			}
		}
		IL_11B:
		if (this.m_setScrollBar > 0)
		{
			this.m_setScrollBar--;
			if (this.m_setScrollBar == 0)
			{
				this.m_textScrollRect.verticalScrollbar.value = 1f;
			}
		}
	}

	private void CleanCurrentlySelectedTextConsole()
	{
		if (EventSystem.current.currentSelectedGameObject != null)
		{
			UITextConsole componentInParent = EventSystem.current.currentSelectedGameObject.GetComponentInParent<UITextConsole>();
			if (componentInParent != null)
			{
				componentInParent.Hide();
			}
		}
	}

	public void SetReportPlayerText(string text)
	{
		for (int i = 0; i < this.m_reportPlayerText.Length; i++)
		{
			this.m_reportPlayerText[i].text = text;
		}
	}

	public void OnReportPlayer(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, !this.m_reportPlayerDropdown.activeSelf, null);
	}

	public void OnReportPlayer1(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0016;
		this.SetReportPlayerText(StringUtil.TR("VerbalHarassment", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer2(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0013;
		this.SetReportPlayerText(StringUtil.TR("LeavingtheGameAFKing", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer3(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0018;
		this.SetReportPlayerText(StringUtil.TR("HateSpeech", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer4(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0009;
		this.SetReportPlayerText(StringUtil.TR("IntentionallyFeeding", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer5(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0004;
		this.SetReportPlayerText(StringUtil.TR("Botting", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer6(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0019;
		this.SetReportPlayerText(StringUtil.TR("SpammingAdvertising", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer7(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0011;
		this.SetReportPlayerText(StringUtil.TR("OffensiveName", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer8(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_0015;
		this.SetReportPlayerText(StringUtil.TR("UnsportsmanlikeConduct", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public void OnReportPlayer9(BaseEventData data)
	{
		this.m_reportReason = ClientFeedbackReport.FeedbackReason.symbol_001A;
		this.SetReportPlayerText(StringUtil.TR("Other", "PersistentScene"));
		UIManager.SetGameObjectActive(this.m_reportPlayerDropdown, false, null);
	}

	public class TextChapterDisplayInfo
	{
		public string Label;

		public _SelectableBtn DisplayBtn;

		public string TitleTextString;

		public string HeaderTextString;

		public List<string> ContentTextString = new List<string>();
	}
}
