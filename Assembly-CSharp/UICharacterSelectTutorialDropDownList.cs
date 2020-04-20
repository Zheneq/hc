using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterSelectTutorialDropDownList : MonoBehaviour
{
	public RectTransform m_container;

	public VerticalLayoutGroup m_videoListContainer;

	public RectTransform m_videoHeightObject;

	private UICharacterSelectTutorialDropDownList.VideoDisplayInfo[] videoListBtns;

	private ScrollRect m_scrollRect;

	private VerticalLayoutGroup m_layoutGroup;

	private bool m_setScrollPositionToTop;

	private static UICharacterSelectTutorialDropDownList s_instance;

	internal static UICharacterSelectTutorialDropDownList Get()
	{
		return UICharacterSelectTutorialDropDownList.s_instance;
	}

	private bool DoDisplayTutorialDropdownVideos(LobbyGameConfig gameConfig)
	{
		bool result;
		try
		{
			if (gameConfig.GameType != GameType.Practice && gameConfig.GameType != GameType.Solo)
			{
				if (gameConfig.GameType == GameType.Tutorial)
				{
				}
				else
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
		UICharacterSelectTutorialDropDownList.s_instance = this;
		if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
		{
			if (GameManager.Get().GameInfo.GameConfig != null)
			{
				UIManager.SetGameObjectActive(this.m_container, this.DoDisplayTutorialDropdownVideos(GameManager.Get().GameInfo.GameConfig), null);
				goto IL_83;
			}
		}
		UIManager.SetGameObjectActive(this.m_container, false, null);
		IL_83:
		this.Setup();
	}

	private void OnDestroy()
	{
		UICharacterSelectTutorialDropDownList.s_instance = null;
	}

	private void Update()
	{
		bool flag = false;
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			if (NavigationBar.Get() != null && NavigationBar.Get().m_cancelBtn.gameObject.activeInHierarchy)
			{
				flag = (Options_UI.Get() != null && Options_UI.Get().GetShowTutorialVideos());
			}
		}
		if (flag && this.m_scrollRect != null)
		{
			if (this.m_setScrollPositionToTop)
			{
				this.m_setScrollPositionToTop = false;
				this.m_scrollRect.verticalScrollbar.value = 1f;
				(this.m_layoutGroup.transform as RectTransform).anchoredPosition = new Vector2((this.m_layoutGroup.transform as RectTransform).anchoredPosition.x, (this.m_layoutGroup.transform as RectTransform).sizeDelta.y * 0.5f);
			}
		}
		if (!flag && this.m_container.gameObject.activeSelf)
		{
			if (UILandingPageFullScreenMenus.Get() != null)
			{
				UILandingPageFullScreenMenus.Get().SetVisible(false);
			}
		}
		UIManager.SetGameObjectActive(this.m_container, flag, null);
	}

	public void PlayTutorialVideo(int index)
	{
		if (0 <= index)
		{
			if (index < HUD_UIResources.Get().m_practiceModeVideoList.Length)
			{
				AccountComponent.UIStateIdentifier seenVideo = HUD_UIResources.Get().m_practiceModeVideoList[index].SeenVideo;
				if (AccountComponent.IsUIStateTutorialVideo(seenVideo))
				{
					ClientGameManager.Get().RequestUpdateUIState(seenVideo, 1, null);
				}
				int practiceModeCurrentLanguageIndex = HUD_UIResources.Get().GetPracticeModeCurrentLanguageIndex(index, LocalizationManager.CurrentLanguageCode);
				UILandingPageFullScreenMenus.Get().DisplayVideo(HUD_UIResources.Get().m_practiceModeVideoList[index].TutorialVideos[practiceModeCurrentLanguageIndex].VideoPath, HUD_UIResources.Get().GetPracticeModeVideoDisplayName(index));
			}
		}
	}

	public void VideoBtnClicked(BaseEventData data)
	{
		for (int i = 0; i < this.videoListBtns.Length; i++)
		{
			if (this.videoListBtns[i].m_theBtn.spriteController.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
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
			switch (1)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private void Setup()
	{
		_SelectableBtn[] componentsInChildren = this.m_videoListContainer.GetComponentsInChildren<_SelectableBtn>(true);
		this.videoListBtns = new UICharacterSelectTutorialDropDownList.VideoDisplayInfo[componentsInChildren.Length];
		this.m_scrollRect = this.m_container.GetComponentInChildren<ScrollRect>(true);
		this.m_layoutGroup = this.m_scrollRect.GetComponentInChildren<VerticalLayoutGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (i < HUD_UIResources.Get().m_practiceModeVideoList.Length)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], true, null);
				TextMeshProUGUI[] componentsInChildren2 = componentsInChildren[i].GetComponentsInChildren<TextMeshProUGUI>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].text = HUD_UIResources.Get().GetPracticeModeVideoDisplayName(i);
				}
				componentsInChildren[i].spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.VideoBtnClicked);
				if (this.videoListBtns[i] == null)
				{
					this.videoListBtns[i] = new UICharacterSelectTutorialDropDownList.VideoDisplayInfo();
				}
				if (this.m_scrollRect != null)
				{
					if (componentsInChildren[i].spriteController.gameObject.GetComponent<_MouseEventPasser>() == null)
					{
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
		if (this.m_scrollRect != null)
		{
			int num = HUD_UIResources.Get().m_practiceModeVideoList.Length;
			float y = (componentsInChildren[0].transform as RectTransform).sizeDelta.y * (float)num + (this.m_layoutGroup.spacing * (float)num - 1f) - 8.1f;
			this.m_videoHeightObject.sizeDelta = new Vector2(this.m_videoHeightObject.sizeDelta.x, y);
			this.m_scrollRect.scrollSensitivity = 40f;
		}
		this.m_setScrollPositionToTop = true;
	}

	public class VideoDisplayInfo
	{
		public _SelectableBtn m_theBtn;

		public HUD_UIResources.TutorialVideoInfo m_videoInfo;

		public int m_videoIndex;
	}
}
