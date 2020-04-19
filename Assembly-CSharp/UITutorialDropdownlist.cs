using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITutorialDropdownlist : MonoBehaviour
{
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

	private UITutorialDropdownlist.VideoDisplayInfo[] videoListBtns;

	private bool m_videoListVisible;

	private bool m_displayPreview;

	private UITutorialDropdownlist.VideoDisplayInfo m_previewingVideo;

	private ScrollRect m_scrollRect;

	private VerticalLayoutGroup m_layoutGroup;

	private int m_setScrollPositionToTop;

	private static UITutorialDropdownlist s_instance;

	internal static UITutorialDropdownlist Get()
	{
		return UITutorialDropdownlist.s_instance;
	}

	private bool DoDisplayTutorialDropdownVideos(LobbyGameConfig gameConfig)
	{
		bool result;
		try
		{
			if (gameConfig.GameType != GameType.Practice && gameConfig.GameType != GameType.Solo)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.DoDisplayTutorialDropdownVideos(LobbyGameConfig)).MethodHandle;
				}
				if (gameConfig.GameType != GameType.Tutorial)
				{
					if (!gameConfig.SubTypes.IsNullOrEmpty<GameSubType>() && gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						return true;
					}
					return false;
				}
			}
			result = true;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			result = false;
		}
		return result;
	}

	private void Awake()
	{
		UITutorialDropdownlist.s_instance = this;
		if (GameManager.Get() != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.Awake()).MethodHandle;
			}
			if (GameManager.Get().GameInfo != null)
			{
				for (;;)
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(this.m_container, this.DoDisplayTutorialDropdownVideos(GameManager.Get().GameInfo.GameConfig), null);
					UIManager.SetGameObjectActive(this.m_leavePracticeBtn, GameManager.Get().GameInfo.GameConfig.GameType == GameType.Practice, null);
					goto IL_CE;
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_container, false, null);
		UIManager.SetGameObjectActive(this.m_leavePracticeBtn, false, null);
		IL_CE:
		this.Setup();
		this.m_previewBtn = this.m_previewContainer.GetComponentInChildren<_SelectableBtn>(true);
		this.m_previewBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PreviewClicked);
		this.m_headerBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HeaderClicked);
		this.m_leavePracticeBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LeavePracticeClicked);
		this.m_videoListVisible = true;
		UIManager.SetGameObjectActive(this.m_videoListAC, true, null);
		UIManager.SetGameObjectActive(this.m_openArrow, this.m_videoListVisible, null);
		UIManager.SetGameObjectActive(this.m_closedArrow, !this.m_videoListVisible, null);
		UIManager.SetGameObjectActive(this.m_previewContainer, false, null);
	}

	private void OnDestroy()
	{
		UITutorialDropdownlist.s_instance = null;
	}

	private void Update()
	{
		bool flag;
		if (Options_UI.Get() != null && Options_UI.Get().GetShowTutorialVideos())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.Update()).MethodHandle;
			}
			if (GameFlowData.Get() != null)
			{
				flag = GameFlowData.Get().IsInDecisionState();
				goto IL_56;
			}
		}
		flag = false;
		IL_56:
		bool flag2 = flag;
		if (flag2)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.DoDisplayTutorialDropdownVideos(GameManager.Get().GameInfo.GameConfig) && !this.m_container.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_container, true, null);
				this.SetVideoListVisible(this.m_videoListVisible);
			}
		}
		if (!flag2)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_container.gameObject.activeSelf)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_container, false, null);
				this.HidePreviewVideo();
				if (UILandingPageFullScreenMenus.Get() != null)
				{
					UILandingPageFullScreenMenus.Get().SetVisible(false);
				}
			}
		}
		if (flag2 && this.m_scrollRect != null && this.m_setScrollPositionToTop > 0)
		{
			this.m_setScrollPositionToTop--;
			if (this.m_setScrollPositionToTop == 0)
			{
				this.m_setScrollPositionToTop = -1;
				this.m_scrollRect.verticalScrollbar.value = 1f;
				(this.m_layoutGroup.transform as RectTransform).anchoredPosition = new Vector2((this.m_layoutGroup.transform as RectTransform).anchoredPosition.x, (this.m_layoutGroup.transform as RectTransform).sizeDelta.y * 0.5f);
			}
		}
	}

	public void LeavePracticeClicked(BaseEventData data)
	{
		UISystemEscapeMenu.Get().OnLeaveGameClick(data);
	}

	public void PlayTutorialVideo(int index)
	{
		if (0 <= index)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.PlayTutorialVideo(int)).MethodHandle;
			}
			if (index < HUD_UIResources.Get().m_practiceModeVideoList.Length)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				AccountComponent.UIStateIdentifier seenVideo = HUD_UIResources.Get().m_practiceModeVideoList[index].SeenVideo;
				if (AccountComponent.IsUIStateTutorialVideo(seenVideo))
				{
					for (;;)
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
			}
		}
	}

	public void PreviewClicked(BaseEventData data)
	{
		bool flag = false;
		if (this.m_previewingVideo != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.PreviewClicked(BaseEventData)).MethodHandle;
			}
			this.m_previewingVideo.m_theBtn.SetSelected(true, false, string.Empty, string.Empty);
			this.PlayTutorialVideo(this.m_previewingVideo.m_videoIndex);
			flag = true;
		}
		this.HidePreviewVideo();
		if (flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.SetVideoListVisible(true);
		}
	}

	public void SetVideoListVisible(bool visible)
	{
		this.m_videoListVisible = visible;
		UIManager.SetGameObjectActive(this.m_videoListAC, true, null);
		if (this.m_videoListVisible)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.SetVideoListVisible(bool)).MethodHandle;
			}
			this.HidePreviewVideo();
			this.m_videoListAC.Play("FadeDefaultIN");
		}
		else
		{
			this.m_videoListAC.Play("FadeDefaultOUT");
			this.m_videoHeightObject.pivot = new Vector2(0.5f, 1f);
			this.m_videoHeightObject.anchoredPosition = new Vector2(this.m_videoHeightObject.anchoredPosition.x, 0f);
		}
		UIManager.SetGameObjectActive(this.m_openArrow, this.m_videoListVisible, null);
		UIManager.SetGameObjectActive(this.m_closedArrow, !this.m_videoListVisible, null);
		this.m_videoListAC.GetComponent<CanvasGroup>().blocksRaycasts = this.m_videoListVisible;
		this.m_videoListAC.GetComponent<CanvasGroup>().interactable = this.m_videoListVisible;
		this.m_setScrollPositionToTop = 2;
	}

	public void HeaderClicked(BaseEventData data)
	{
		this.SetVideoListVisible(!this.m_videoListVisible);
	}

	public void VideoBtnClicked(BaseEventData data)
	{
		for (int i = 0; i < this.videoListBtns.Length; i++)
		{
			if (this.videoListBtns[i].m_theBtn.spriteController.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.VideoBtnClicked(BaseEventData)).MethodHandle;
				}
				if (i < HUD_UIResources.Get().m_practiceModeVideoList.Length)
				{
					this.videoListBtns[i].m_theBtn.SetSelected(true, false, string.Empty, string.Empty);
					this.PlayTutorialVideo(i);
				}
				return;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void HidePreviewVideo()
	{
		UIManager.SetGameObjectActive(this.m_previewContainer, false, null);
		this.m_previewingVideo = null;
	}

	public void DisplayPreviewVideo(int videoIndex)
	{
		if (videoIndex >= 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.DisplayPreviewVideo(int)).MethodHandle;
			}
			if (videoIndex < this.videoListBtns.Length)
			{
				UIManager.SetGameObjectActive(this.m_previewContainer, true, null);
				this.SetVideoListVisible(false);
				this.m_previewingVideo = this.videoListBtns[videoIndex];
				this.m_previewText.text = StringUtil.TR(this.m_previewingVideo.m_videoInfo.DisplayName);
				this.m_previewImage.sprite = (Sprite)Resources.Load(this.m_previewingVideo.m_videoInfo.ThumbnailResourceLocation, typeof(Sprite));
				return;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.HidePreviewVideo();
	}

	public void DisplayPreviewVideo(string videoNameToPreview)
	{
		if (videoNameToPreview.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.DisplayPreviewVideo(string)).MethodHandle;
			}
			this.HidePreviewVideo();
		}
		else
		{
			for (int i = 0; i < this.videoListBtns.Length; i++)
			{
				if (this.videoListBtns[i].m_videoInfo.Name == videoNameToPreview)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(this.m_previewContainer, true, null);
					this.SetVideoListVisible(false);
					this.m_previewingVideo = this.videoListBtns[i];
					this.m_previewText.text = StringUtil.TR(this.m_previewingVideo.m_videoInfo.DisplayName);
					this.m_previewImage.sprite = (Sprite)Resources.Load(this.m_previewingVideo.m_videoInfo.ThumbnailResourceLocation, typeof(Sprite));
					return;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void Setup()
	{
		_SelectableBtn[] componentsInChildren = this.m_videoListAC.GetComponentsInChildren<_SelectableBtn>();
		this.videoListBtns = new UITutorialDropdownlist.VideoDisplayInfo[componentsInChildren.Length];
		this.m_scrollRect = this.m_videoListAC.GetComponentInChildren<ScrollRect>(true);
		this.m_layoutGroup = this.m_scrollRect.GetComponentInChildren<VerticalLayoutGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (i < HUD_UIResources.Get().m_practiceModeVideoList.Length)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialDropdownlist.Setup()).MethodHandle;
				}
				UIManager.SetGameObjectActive(componentsInChildren[i], true, null);
				TextMeshProUGUI[] componentsInChildren2 = componentsInChildren[i].GetComponentsInChildren<TextMeshProUGUI>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].text = HUD_UIResources.Get().GetPracticeModeVideoDisplayName(i);
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				componentsInChildren[i].spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.VideoBtnClicked);
				if (this.videoListBtns[i] == null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.videoListBtns[i] = new UITutorialDropdownlist.VideoDisplayInfo();
				}
				if (this.m_scrollRect != null)
				{
					for (;;)
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
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						_MouseEventPasser mouseEventPasser = componentsInChildren[i].spriteController.gameObject.AddComponent<_MouseEventPasser>();
						mouseEventPasser.AddNewHandler(this.m_scrollRect);
					}
				}
				this.videoListBtns[i].m_theBtn = componentsInChildren[i];
				this.videoListBtns[i].m_videoInfo = HUD_UIResources.Get().m_practiceModeVideoList[i];
				this.videoListBtns[i].m_videoIndex = i;
				this.videoListBtns[i].m_theBtn.SetSelected(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(this.videoListBtns[i].m_videoInfo.SeenVideo) == 1, false, string.Empty, string.Empty);
			}
			else
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], false, null);
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (this.m_scrollRect != null)
		{
			int num = HUD_UIResources.Get().m_practiceModeVideoList.Length;
			float y = (componentsInChildren[0].transform as RectTransform).sizeDelta.y * (float)num + (this.m_layoutGroup.spacing * (float)num - 1f) - 8.1f;
			this.m_videoHeightObject.sizeDelta = new Vector2(this.m_videoHeightObject.sizeDelta.x, y);
			this.m_scrollRect.scrollSensitivity = 40f;
		}
		this.m_setScrollPositionToTop = 2;
	}

	public class VideoDisplayInfo
	{
		public _SelectableBtn m_theBtn;

		public HUD_UIResources.TutorialVideoInfo m_videoInfo;

		public int m_videoIndex;
	}
}
