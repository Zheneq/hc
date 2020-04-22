using I2.Loc;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterSelectTutorialDropDownList : MonoBehaviour
{
	public class VideoDisplayInfo
	{
		public _SelectableBtn m_theBtn;

		public HUD_UIResources.TutorialVideoInfo m_videoInfo;

		public int m_videoIndex;
	}

	public RectTransform m_container;

	public VerticalLayoutGroup m_videoListContainer;

	public RectTransform m_videoHeightObject;

	private VideoDisplayInfo[] videoListBtns;

	private ScrollRect m_scrollRect;

	private VerticalLayoutGroup m_layoutGroup;

	private bool m_setScrollPositionToTop;

	private static UICharacterSelectTutorialDropDownList s_instance;

	internal static UICharacterSelectTutorialDropDownList Get()
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
				if (gameConfig.GameType != GameType.Tutorial)
				{
					if (!gameConfig.SubTypes.IsNullOrEmpty() && gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					return false;
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
		if (GameManager.Get() != null && GameManager.Get().GameInfo != null)
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
			if (GameManager.Get().GameInfo.GameConfig != null)
			{
				UIManager.SetGameObjectActive(m_container, DoDisplayTutorialDropdownVideos(GameManager.Get().GameInfo.GameConfig));
				goto IL_0083;
			}
		}
		UIManager.SetGameObjectActive(m_container, false);
		goto IL_0083;
		IL_0083:
		Setup();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		bool flag = false;
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
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
			if (NavigationBar.Get() != null && NavigationBar.Get().m_cancelBtn.gameObject.activeInHierarchy)
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
				flag = (Options_UI.Get() != null && Options_UI.Get().GetShowTutorialVideos());
			}
		}
		if (flag && m_scrollRect != null)
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
			if (m_setScrollPositionToTop)
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
				m_setScrollPositionToTop = false;
				m_scrollRect.verticalScrollbar.value = 1f;
				RectTransform obj = m_layoutGroup.transform as RectTransform;
				Vector2 anchoredPosition = (m_layoutGroup.transform as RectTransform).anchoredPosition;
				float x = anchoredPosition.x;
				Vector2 sizeDelta = (m_layoutGroup.transform as RectTransform).sizeDelta;
				obj.anchoredPosition = new Vector2(x, sizeDelta.y * 0.5f);
			}
		}
		if (!flag && m_container.gameObject.activeSelf)
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
			if (UILandingPageFullScreenMenus.Get() != null)
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
				UILandingPageFullScreenMenus.Get().SetVisible(false);
			}
		}
		UIManager.SetGameObjectActive(m_container, flag);
	}

	public void PlayTutorialVideo(int index)
	{
		if (0 > index)
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
			if (index >= HUD_UIResources.Get().m_practiceModeVideoList.Length)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				AccountComponent.UIStateIdentifier seenVideo = HUD_UIResources.Get().m_practiceModeVideoList[index].SeenVideo;
				if (AccountComponent.IsUIStateTutorialVideo(seenVideo))
				{
					ClientGameManager.Get().RequestUpdateUIState(seenVideo, 1, null);
				}
				int practiceModeCurrentLanguageIndex = HUD_UIResources.Get().GetPracticeModeCurrentLanguageIndex(index, LocalizationManager.CurrentLanguageCode);
				UILandingPageFullScreenMenus.Get().DisplayVideo(HUD_UIResources.Get().m_practiceModeVideoList[index].TutorialVideos[practiceModeCurrentLanguageIndex].VideoPath, HUD_UIResources.Get().GetPracticeModeVideoDisplayName(index));
				return;
			}
		}
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
				switch (3)
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
			switch (1)
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
		_SelectableBtn[] componentsInChildren = m_videoListContainer.GetComponentsInChildren<_SelectableBtn>(true);
		videoListBtns = new VideoDisplayInfo[componentsInChildren.Length];
		m_scrollRect = m_container.GetComponentInChildren<ScrollRect>(true);
		m_layoutGroup = m_scrollRect.GetComponentInChildren<VerticalLayoutGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (i < HUD_UIResources.Get().m_practiceModeVideoList.Length)
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
				UIManager.SetGameObjectActive(componentsInChildren[i], true);
				TextMeshProUGUI[] componentsInChildren2 = componentsInChildren[i].GetComponentsInChildren<TextMeshProUGUI>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].text = HUD_UIResources.Get().GetPracticeModeVideoDisplayName(i);
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
						switch (1)
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
							switch (6)
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
			switch (4)
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
			m_setScrollPositionToTop = true;
			return;
		}
	}
}
