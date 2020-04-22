using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFrontendLoadingScreen : UIScene
{
	public enum DisplayStates
	{
		None,
		FadeIn,
		Loading,
		ServerLocked,
		ServerQueued,
		PressKey,
		Error,
		FadeOut
	}

	public RectTransform m_Container;

	public string m_frontEndBackgroundScene;

	public Animator m_animator;

	public Image m_backgroundImage;

	public Animator m_bgImageanimator;

	public GameObject m_mainTextBox;

	public GameObject m_subTextBox;

	public GameObject m_subText2Box;

	public _SelectableBtn m_ServerLockedButton;

	public _SelectableBtn m_ShutdownButton;

	public RectTransform m_loadingMapNameInfoContainer;

	public TextMeshProUGUI m_mapNameTitle;

	public TextMeshProUGUI m_mapNameSubTitle;

	private float m_visibleStartTime;

	private int m_screenInitialHeight;

	private int m_screenInitialWidth;

	private bool m_isPlayerAccountDataAvailable;

	private Queue<string> m_movieNames = new Queue<string>();

	private static UIFrontendLoadingScreen s_instance;

	private DisplayStates m_displayState;

	private string m_ServerLockURL = string.Empty;

	private static float m_startLoadTime;

	private bool shouldPlayIntroVideo;

	public DisplayStates DisplayState => m_displayState;

	public override SceneType GetSceneType()
	{
		return SceneType.FrontEndLoadingScreen;
	}

	public override void Awake()
	{
		s_instance = this;
		m_screenInitialHeight = Screen.height;
		m_screenInitialWidth = Screen.width;
		Object.DontDestroyOnLoad(base.gameObject);
		if (Application.isEditor)
		{
			Application.runInBackground = true;
		}
		if (m_animator == null)
		{
			m_animator = new Animator();
		}
		SetDisplayState(DisplayStates.None);
		if (!Object.FindObjectOfType<EventSystem>())
		{
			GameObject gameObject = new GameObject("EventSystem", typeof(EventSystem));
			gameObject.AddComponent<StandaloneInputModule>();
			Object.DontDestroyOnLoad(gameObject);
		}
		m_ServerLockedButton.spriteController.callback = ServerLockedButtonClicked;
		SetServerLockButtonVisible(false);
		m_ShutdownButton.spriteController.callback = ShutdownButtonClicked;
		m_visibleStartTime = Time.time;
	}

	private void Start()
	{
		AudioManager.StandardizeAudioLinkages();
		m_movieNames.Enqueue("Video/Logo/TRION-LOGO");
		m_movieNames.Enqueue("Video/AR_CG");
		if (m_movieNames.Count > 0)
		{
			FullScreenMovie.Get().SetVisible(true);
			string movieAssetName = m_movieNames.Dequeue();
			FullScreenMovie.Get().m_movieTexture.Play(movieAssetName, false);
		}
		RegisterWithUIManager();
	}

	internal bool IsVisible()
	{
		return m_Container.gameObject.activeSelf;
	}

	internal void SetVisible(bool visible)
	{
		if (m_Container.gameObject.activeSelf == visible)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_loadingMapNameInfoContainer, false);
			if (visible)
			{
				m_visibleStartTime = Time.time;
				PickBackgroundImage();
			}
			else
			{
				m_bgImageanimator.enabled = false;
				Log.Info(Log.Category.Loading, "F.E. Loading screen displayed for {0} seconds.", (Time.time - m_visibleStartTime).ToString("F1"));
			}
			UIManager.SetGameObjectActive(m_Container, visible);
			return;
		}
	}

	public bool IsSameAsInitialResolution()
	{
		int result;
		if (m_screenInitialHeight == Screen.height)
		{
			result = ((m_screenInitialWidth == Screen.width) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void SetServerLockButtonVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_ServerLockedButton, visible);
	}

	public void UpdateServerLockLabels(ServerMessageOverrides serverMessageOverrides)
	{
		string language = HydrogenConfig.Get().Language;
		TextMeshProUGUI[] componentsInChildren = m_ServerLockedButton.GetComponentsInChildren<TextMeshProUGUI>(true);
		TextMeshProUGUI[] array = componentsInChildren;
		foreach (TextMeshProUGUI textMeshProUGUI in array)
		{
			textMeshProUGUI.text = serverMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenButtonText, language);
		}
		while (true)
		{
			m_ServerLockURL = ((!SteamManager.UsingSteam) ? serverMessageOverrides.FreeUpsellExternalBrowserUrl : serverMessageOverrides.FreeUpsellExternalBrowserSteamUrl);
			ShowText(StringUtil.TR("SERVERISLOCKED", "LoadingScreen"), serverMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenText, language));
			return;
		}
	}

	public void ShutdownButtonClicked(BaseEventData data)
	{
		AppState_Shutdown.Get().Enter();
	}

	public void ServerLockedButtonClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendUIActionNotification("ServerLockedButtonClicked");
		if (m_ServerLockURL.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			Application.OpenURL(m_ServerLockURL);
			return;
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static UIFrontendLoadingScreen Get()
	{
		return s_instance;
	}

	public void OnEnable()
	{
		StartDisplayFadeIn();
		PickBackgroundImage();
	}

	private void PickBackgroundImage()
	{
		if (!m_isPlayerAccountDataAvailable)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_bgImageanimator.enabled = true;
					UIManager.SetGameObjectActive(m_backgroundImage, false);
					return;
				}
			}
		}
		Dictionary<int, bool> unlockedLoadingScreenBackgroundIdsToActivatedState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.UnlockedLoadingScreenBackgroundIdsToActivatedState;
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = ((KeyValuePair<int, bool> x) => x.Value);
		}
		IEnumerable<KeyValuePair<int, bool>> source = unlockedLoadingScreenBackgroundIdsToActivatedState.Where(_003C_003Ef__am_0024cache0);
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = ((KeyValuePair<int, bool> x) => x.Key);
		}
		int[] array = source.Select(_003C_003Ef__am_0024cache1).ToArray();
		if (array.Length == 0)
		{
			m_backgroundImage.sprite = null;
		}
		else
		{
			int loadingScreenID = array[Random.Range(0, array.Length)];
			m_backgroundImage.sprite = Resources.Load<Sprite>(GameBalanceVars.Get().GetLoadingScreenBackground(loadingScreenID).m_resourceString);
		}
		UIManager.SetGameObjectActive(m_backgroundImage, m_backgroundImage.sprite != null);
	}

	private void SetupMapLoadingTitleInfo(GameType gameType, string MapName)
	{
		if (MapName.IsNullOrEmpty())
		{
			UIManager.SetGameObjectActive(m_loadingMapNameInfoContainer, false);
			return;
		}
		if (gameType == GameType.Tutorial)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_loadingMapNameInfoContainer, true);
					m_mapNameTitle.text = StringUtil.TR("TutorialName", "Prologue1");
					m_mapNameSubTitle.text = StringUtil.TR("TutorialDescription", "Prologue1");
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_loadingMapNameInfoContainer, false);
	}

	public void OnDisable()
	{
	}

	public bool DoPlayIntroVideo()
	{
		if (shouldPlayIntroVideo)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		shouldPlayIntroVideo = true;
		string key = "PlayIntro";
		if (PlayerPrefs.HasKey(key))
		{
			shouldPlayIntroVideo = (PlayerPrefs.GetInt(key) != 1);
		}
		PlayerPrefs.SetInt(key, 1);
		return shouldPlayIntroVideo;
	}

	public void Update()
	{
		int num;
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameInfo != null)
			{
				num = ((GameManager.Get().GameInfo.GameConfig != null) ? 1 : 0);
				goto IL_004c;
			}
		}
		num = 0;
		goto IL_004c;
		IL_004c:
		bool flag = (byte)num != 0;
		if (flag)
		{
			SetupMapLoadingTitleInfo(GameManager.Get().GameInfo.GameConfig.GameType, GameManager.Get().GameInfo.GameConfig.Map);
		}
		else
		{
			SetupMapLoadingTitleInfo(GameType.None, string.Empty);
		}
		if (m_displayState == DisplayStates.FadeIn)
		{
			if (IsCurrentAnimationDone())
			{
				StartDisplayLoading();
			}
		}
		if (!m_isPlayerAccountDataAvailable)
		{
			if (IsVisible())
			{
				if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					m_isPlayerAccountDataAvailable = true;
					PickBackgroundImage();
				}
			}
		}
		if (m_displayState == DisplayStates.FadeOut)
		{
			if (IsCurrentAnimationDone())
			{
				SetVisible(false);
			}
		}
		if (!flag && (bool)FullScreenMovie.Get())
		{
			if (DoPlayIntroVideo())
			{
				if (FullScreenMovie.Get().m_movieTexture.MovieState != 0 && FullScreenMovie.Get().m_movieTexture.MovieState != PlayRawImageMovieTexture.MovieStates.Done)
				{
					if (!Input.GetKeyUp(KeyCode.Escape))
					{
						goto IL_0205;
					}
				}
				if (m_movieNames.Count > 0)
				{
					string movieAssetName = m_movieNames.Dequeue();
					FullScreenMovie.Get().m_movieTexture.Play(movieAssetName, false);
					SetVisible(false);
				}
				else
				{
					Finish();
				}
				goto IL_0205;
			}
		}
		Finish();
		goto IL_0205;
		IL_0205:
		if (!(m_startLoadTime > 0f))
		{
			return;
		}
		while (true)
		{
			if (Time.time >= m_startLoadTime)
			{
				m_startLoadTime = -1f;
				StartCoroutine(AssetBundleManager.Get().LoadSceneAsync("ClientEnvironmentSingletons", "frontend", LoadSceneMode.Additive));
			}
			return;
		}
	}

	private void Finish()
	{
		if (m_startLoadTime != 0f)
		{
			return;
		}
		while (true)
		{
			if (m_animator != null)
			{
				while (true)
				{
					SetVisible(true);
					FullScreenMovie.Get().SetVisible(false);
					m_animator.Play("FrontEndLoadingScreenDefaultIN");
					m_startLoadTime = Time.time;
					return;
				}
			}
			return;
		}
	}

	private void SetDisplayState(DisplayStates state)
	{
		m_displayState = state;
		if (m_displayState != DisplayStates.Error)
		{
			if (m_displayState != DisplayStates.ServerLocked)
			{
				if (m_displayState != DisplayStates.ServerQueued)
				{
					if (m_displayState != 0)
					{
						UIManager.SetGameObjectActive(m_ShutdownButton, false);
						return;
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_ShutdownButton, true);
	}

	public void StartDisplayFadeIn()
	{
		SetDisplayState(DisplayStates.FadeIn);
		m_animator.Play("FrontEndLoadingScreenDefaultIN");
	}

	public void StartDisplayLoading(string subText = null)
	{
		SetDisplayState(DisplayStates.Loading);
		ShowText(StringUtil.TR("NOWLOADING", "LoadingScreen"), subText);
		m_animator.Play("FrontEndLoadingScreenDefaultIDLE");
	}

	public void StartDisplayPressKey()
	{
		SetDisplayState(DisplayStates.PressKey);
		ShowText(StringUtil.TR("PRESSANYKEY", "LoadingScreen"));
	}

	public void StartDisplayServerLocked()
	{
		SetDisplayState(DisplayStates.ServerLocked);
	}

	public void StartDisplayServerQueued(string queueStatusString)
	{
		SetDisplayState(DisplayStates.ServerQueued);
		ShowText(StringUtil.TR("SERVERISFULL", "LoadingScreen"), queueStatusString);
	}

	public void StartDisplayError(string mainErrorText, string subErrorText = null)
	{
		SetDisplayState(DisplayStates.Error);
		ShowText(mainErrorText, subErrorText);
	}

	public void StartDisplayFadeOut()
	{
		SetDisplayState(DisplayStates.FadeOut);
		if (!m_animator.isActiveAndEnabled)
		{
			return;
		}
		while (true)
		{
			m_animator.Play("FrontEndLoadingScreenDefaultOUT");
			return;
		}
	}

	private void ShowText(string text, string subText = null, string subText2 = null)
	{
		UIManager.SetGameObjectActive(m_ServerLockedButton, text == StringUtil.TR("SERVERISLOCKED", "LoadingScreen"));
		if ((bool)m_mainTextBox)
		{
			if (!text.IsNullOrEmpty())
			{
				m_mainTextBox.GetComponent<TextMeshProUGUI>().text = text;
				UIManager.SetGameObjectActive(m_mainTextBox, true);
			}
			else
			{
				UIManager.SetGameObjectActive(m_mainTextBox, false);
			}
		}
		if ((bool)m_subTextBox)
		{
			if (!subText.IsNullOrEmpty())
			{
				m_subTextBox.GetComponent<TextMeshProUGUI>().text = subText;
				UIManager.SetGameObjectActive(m_subTextBox, true);
			}
			else
			{
				UIManager.SetGameObjectActive(m_subTextBox, false);
			}
		}
		if (!m_subText2Box)
		{
			return;
		}
		while (true)
		{
			if (!subText2.IsNullOrEmpty())
			{
				m_subText2Box.GetComponent<TextMeshProUGUI>().text = subText2;
				UIManager.SetGameObjectActive(m_subText2Box, true);
			}
			else
			{
				UIManager.SetGameObjectActive(m_subText2Box, false);
			}
			return;
		}
	}

	public bool IsReadyToReveal()
	{
		if (m_displayState == DisplayStates.FadeOut)
		{
			while (true)
			{
				switch (6)
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

	private bool IsCurrentAnimationDone()
	{
		bool result = false;
		if (m_animator.isActiveAndEnabled)
		{
			AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
			{
				result = true;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}
}
