using I2.Loc;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITutorialDropdownlist : MonoBehaviour
{
	public class VideoDisplayInfo
	{
		public _SelectableBtn m_theBtn;

		public HUD_UIResources.TutorialVideoInfo m_videoInfo;

		public int m_videoIndex;
	}

	public RectTransform m_container;

	public _SelectableBtn m_leavePracticeBtn;

	public _SelectableBtn m_headerBtn;

	public Animator m_videoListAC;

	public VerticalLayoutGroup m_videoListContainer;

	public RectTransform m_openArrow;

	public RectTransform m_closedArrow;

	public TextMeshProUGUI m_previewText;

	public Image m_previewImage;

	public RectTransform m_previewContainer;

	public RectTransform m_videoHeightObject;

	private _SelectableBtn m_previewBtn;

	private VideoDisplayInfo[] videoListBtns;

	private bool m_videoListVisible;

	private bool m_displayPreview;

	private VideoDisplayInfo m_previewingVideo;

	private ScrollRect m_scrollRect;

	private VerticalLayoutGroup m_layoutGroup;

	private int m_setScrollPositionToTop;

	private static UITutorialDropdownlist s_instance;

	internal static UITutorialDropdownlist Get()
	{
		return s_instance;
	}

	private bool DoDisplayTutorialDropdownVideos(LobbyGameConfig gameConfig)
	{
		try
		{
			if (gameConfig.GameType != GameType.Practice && gameConfig.GameType != GameType.Solo)
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
				if (gameConfig.GameType != GameType.Tutorial)
				{
					if (!gameConfig.SubTypes.IsNullOrEmpty() && gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						return true;
					}
					return false;
				}
			}
			return true;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			return false;
		}
	}

	private void Awake()
	{
		s_instance = this;
		if (GameManager.Get() != null)
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
			if (GameManager.Get().GameInfo != null)
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
				if (GameManager.Get().GameInfo.GameConfig != null)
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
					UIManager.SetGameObjectActive(m_container, DoDisplayTutorialDropdownVideos(GameManager.Get().GameInfo.GameConfig));
					UIManager.SetGameObjectActive(m_leavePracticeBtn, GameManager.Get().GameInfo.GameConfig.GameType == GameType.Practice);
					goto IL_00ce;
				}
			}
		}
		UIManager.SetGameObjectActive(m_container, false);
		UIManager.SetGameObjectActive(m_leavePracticeBtn, false);
		goto IL_00ce;
		IL_00ce:
		Setup();
		m_previewBtn = m_previewContainer.GetComponentInChildren<_SelectableBtn>(true);
		m_previewBtn.spriteController.callback = PreviewClicked;
		m_headerBtn.spriteController.callback = HeaderClicked;
		m_leavePracticeBtn.spriteController.callback = LeavePracticeClicked;
		m_videoListVisible = true;
		UIManager.SetGameObjectActive(m_videoListAC, true);
		UIManager.SetGameObjectActive(m_openArrow, m_videoListVisible);
		UIManager.SetGameObjectActive(m_closedArrow, !m_videoListVisible);
		UIManager.SetGameObjectActive(m_previewContainer, false);
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		int num;
		if (Options_UI.Get() != null && Options_UI.Get().GetShowTutorialVideos())
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
			if (GameFlowData.Get() != null)
			{
				num = (GameFlowData.Get().IsInDecisionState() ? 1 : 0);
				goto IL_0056;
			}
		}
		num = 0;
		goto IL_0056;
		IL_0056:
		bool flag = (byte)num != 0;
		if (flag)
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
			if (DoDisplayTutorialDropdownVideos(GameManager.Get().GameInfo.GameConfig) && !m_container.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(m_container, true);
				SetVideoListVisible(m_videoListVisible);
			}
		}
		if (!flag)
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
			if (m_container.gameObject.activeSelf)
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
				UIManager.SetGameObjectActive(m_container, false);
				HidePreviewVideo();
				if (UILandingPageFullScreenMenus.Get() != null)
				{
					UILandingPageFullScreenMenus.Get().SetVisible(false);
				}
			}
		}
		if (flag && m_scrollRect != null && m_setScrollPositionToTop > 0)
		{
			m_setScrollPositionToTop--;
			if (m_setScrollPositionToTop == 0)
			{
				m_setScrollPositionToTop = -1;
				m_scrollRect.verticalScrollbar.value = 1f;
				RectTransform obj = m_layoutGroup.transform as RectTransform;
				Vector2 anchoredPosition = (m_layoutGroup.transform as RectTransform).anchoredPosition;
				float x = anchoredPosition.x;
				Vector2 sizeDelta = (m_layoutGroup.transform as RectTransform).sizeDelta;
				obj.anchoredPosition = new Vector2(x, sizeDelta.y * 0.5f);
			}
		}
	}

	public void LeavePracticeClicked(BaseEventData data)
	{
		UISystemEscapeMenu.Get().OnLeaveGameClick(data);
	}

	public void PlayTutorialVideo(int index)
	{
		if (0 > index)
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
			if (index >= HUD_UIResources.Get().m_practiceModeVideoList.Length)
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
				AccountComponent.UIStateIdentifier seenVideo = HUD_UIResources.Get().m_practiceModeVideoList[index].SeenVideo;
				if (AccountComponent.IsUIStateTutorialVideo(seenVideo))
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
					ClientGameManager.Get().RequestUpdateUIState(seenVideo, 1, null);
				}
				int practiceModeCurrentLanguageIndex = HUD_UIResources.Get().GetPracticeModeCurrentLanguageIndex(index, LocalizationManager.CurrentLanguageCode);
				UILandingPageFullScreenMenus.Get().DisplayVideo(HUD_UIResources.Get().m_practiceModeVideoList[index].TutorialVideos[practiceModeCurrentLanguageIndex].VideoPath, HUD_UIResources.Get().GetPracticeModeVideoDisplayName(index));
				return;
			}
		}
	}

	public void PreviewClicked(BaseEventData data)
	{
		bool flag = false;
		if (m_previewingVideo != null)
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
			m_previewingVideo.m_theBtn.SetSelected(true, false, string.Empty, string.Empty);
			PlayTutorialVideo(m_previewingVideo.m_videoIndex);
			flag = true;
		}
		HidePreviewVideo();
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			SetVideoListVisible(true);
			return;
		}
	}

	public void SetVideoListVisible(bool visible)
	{
		m_videoListVisible = visible;
		UIManager.SetGameObjectActive(m_videoListAC, true);
		if (m_videoListVisible)
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
			HidePreviewVideo();
			m_videoListAC.Play("FadeDefaultIN");
		}
		else
		{
			m_videoListAC.Play("FadeDefaultOUT");
			m_videoHeightObject.pivot = new Vector2(0.5f, 1f);
			RectTransform videoHeightObject = m_videoHeightObject;
			Vector2 anchoredPosition = m_videoHeightObject.anchoredPosition;
			videoHeightObject.anchoredPosition = new Vector2(anchoredPosition.x, 0f);
		}
		UIManager.SetGameObjectActive(m_openArrow, m_videoListVisible);
		UIManager.SetGameObjectActive(m_closedArrow, !m_videoListVisible);
		m_videoListAC.GetComponent<CanvasGroup>().blocksRaycasts = m_videoListVisible;
		m_videoListAC.GetComponent<CanvasGroup>().interactable = m_videoListVisible;
		m_setScrollPositionToTop = 2;
	}

	public void HeaderClicked(BaseEventData data)
	{
		SetVideoListVisible(!m_videoListVisible);
	}

	public void VideoBtnClicked(BaseEventData data)
	{
		for (int i = 0; i < videoListBtns.Length; i++)
		{
			if (!(videoListBtns[i].m_theBtn.spriteController.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject))
			{
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (i < HUD_UIResources.Get().m_practiceModeVideoList.Length)
				{
					videoListBtns[i].m_theBtn.SetSelected(true, false, string.Empty, string.Empty);
					PlayTutorialVideo(i);
				}
				return;
			}
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

	public void HidePreviewVideo()
	{
		UIManager.SetGameObjectActive(m_previewContainer, false);
		m_previewingVideo = null;
	}

	public void DisplayPreviewVideo(int videoIndex)
	{
		if (videoIndex >= 0)
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
			if (videoIndex < videoListBtns.Length)
			{
				UIManager.SetGameObjectActive(m_previewContainer, true);
				SetVideoListVisible(false);
				m_previewingVideo = videoListBtns[videoIndex];
				m_previewText.text = StringUtil.TR(m_previewingVideo.m_videoInfo.DisplayName);
				m_previewImage.sprite = (Sprite)Resources.Load(m_previewingVideo.m_videoInfo.ThumbnailResourceLocation, typeof(Sprite));
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		HidePreviewVideo();
	}

	public void DisplayPreviewVideo(string videoNameToPreview)
	{
		if (videoNameToPreview.IsNullOrEmpty())
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
					HidePreviewVideo();
					return;
				}
			}
		}
		for (int i = 0; i < videoListBtns.Length; i++)
		{
			if (!(videoListBtns[i].m_videoInfo.Name == videoNameToPreview))
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				UIManager.SetGameObjectActive(m_previewContainer, true);
				SetVideoListVisible(false);
				m_previewingVideo = videoListBtns[i];
				m_previewText.text = StringUtil.TR(m_previewingVideo.m_videoInfo.DisplayName);
				m_previewImage.sprite = (Sprite)Resources.Load(m_previewingVideo.m_videoInfo.ThumbnailResourceLocation, typeof(Sprite));
				return;
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

	private void Setup()
	{
		_SelectableBtn[] componentsInChildren = m_videoListAC.GetComponentsInChildren<_SelectableBtn>();
		videoListBtns = new VideoDisplayInfo[componentsInChildren.Length];
		m_scrollRect = m_videoListAC.GetComponentInChildren<ScrollRect>(true);
		m_layoutGroup = m_scrollRect.GetComponentInChildren<VerticalLayoutGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (i < HUD_UIResources.Get().m_practiceModeVideoList.Length)
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
				UIManager.SetGameObjectActive(componentsInChildren[i], true);
				TextMeshProUGUI[] componentsInChildren2 = componentsInChildren[i].GetComponentsInChildren<TextMeshProUGUI>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].text = HUD_UIResources.Get().GetPracticeModeVideoDisplayName(i);
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
				componentsInChildren[i].spriteController.callback = VideoBtnClicked;
				if (videoListBtns[i] == null)
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
					videoListBtns[i] = new VideoDisplayInfo();
				}
				if (m_scrollRect != null)
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
					if (componentsInChildren[i].spriteController.gameObject.GetComponent<_MouseEventPasser>() == null)
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
						_MouseEventPasser mouseEventPasser = componentsInChildren[i].spriteController.gameObject.AddComponent<_MouseEventPasser>();
						mouseEventPasser.AddNewHandler(m_scrollRect);
					}
				}
				videoListBtns[i].m_theBtn = componentsInChildren[i];
				videoListBtns[i].m_videoInfo = HUD_UIResources.Get().m_practiceModeVideoList[i];
				videoListBtns[i].m_videoIndex = i;
				videoListBtns[i].m_theBtn.SetSelected(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(videoListBtns[i].m_videoInfo.SeenVideo) == 1, false, string.Empty, string.Empty);
			}
			else
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], false);
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (m_scrollRect != null)
			{
				int num = HUD_UIResources.Get().m_practiceModeVideoList.Length;
				Vector2 sizeDelta = (componentsInChildren[0].transform as RectTransform).sizeDelta;
				float y = sizeDelta.y * (float)num + (m_layoutGroup.spacing * (float)num - 1f) - 8.1f;
				RectTransform videoHeightObject = m_videoHeightObject;
				Vector2 sizeDelta2 = m_videoHeightObject.sizeDelta;
				videoHeightObject.sizeDelta = new Vector2(sizeDelta2.x, y);
				m_scrollRect.scrollSensitivity = 40f;
			}
			m_setScrollPositionToTop = 2;
			return;
		}
	}
}
