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
		DontDestroyOnLoad(gameObject);
		if (Application.isEditor)
		{
			Application.runInBackground = true;
		}
		if (m_animator == null)
		{
			m_animator = new Animator();
		}
		SetDisplayState(DisplayStates.None);
		if (!FindObjectOfType<EventSystem>())
		{
			GameObject gameObject = new GameObject("EventSystem", typeof(EventSystem));
			gameObject.AddComponent<StandaloneInputModule>();
			DontDestroyOnLoad(gameObject);
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
		if (m_Container.gameObject.activeSelf != visible)
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
		}
	}

	public bool IsSameAsInitialResolution()
	{
		return m_screenInitialHeight == Screen.height
			&& m_screenInitialWidth == Screen.width;
	}

	public void SetServerLockButtonVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_ServerLockedButton, visible);
	}

	public void UpdateServerLockLabels(ServerMessageOverrides serverMessageOverrides)
	{
		string language = HydrogenConfig.Get().Language;
		foreach (TextMeshProUGUI textMeshProUGUI in m_ServerLockedButton.GetComponentsInChildren<TextMeshProUGUI>(true))
		{
			textMeshProUGUI.text = serverMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenButtonText, language);
		}
		m_ServerLockURL = SteamManager.UsingSteam
			? serverMessageOverrides.FreeUpsellExternalBrowserSteamUrl
			: serverMessageOverrides.FreeUpsellExternalBrowserUrl;
		ShowText(StringUtil.TR("SERVERISLOCKED", "LoadingScreen"), serverMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenText, language));
	}

	public void ShutdownButtonClicked(BaseEventData data)
	{
		AppState_Shutdown.Get().Enter();
	}

	public void ServerLockedButtonClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendUIActionNotification("ServerLockedButtonClicked");
		if (!m_ServerLockURL.IsNullOrEmpty())
		{
			Application.OpenURL(m_ServerLockURL);
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
			m_bgImageanimator.enabled = true;
			UIManager.SetGameObjectActive(m_backgroundImage, false);
			return;
		}
		Dictionary<int, bool> loadingScreens = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.UnlockedLoadingScreenBackgroundIdsToActivatedState;
		int[] activatedIDs = loadingScreens
			.Where((KeyValuePair<int, bool> x) => x.Value)
			.Select((KeyValuePair<int, bool> x) => x.Key).ToArray();
		if (activatedIDs.Length == 0)
		{
			m_backgroundImage.sprite = null;
		}
		else
		{
			int loadingScreenID = activatedIDs[Random.Range(0, activatedIDs.Length)];
			m_backgroundImage.sprite = Resources.Load<Sprite>(GameBalanceVars.Get().GetLoadingScreenBackground(loadingScreenID).m_resourceString);
		}
		UIManager.SetGameObjectActive(m_backgroundImage, m_backgroundImage.sprite != null);
	}

	private void SetupMapLoadingTitleInfo(GameType gameType, string MapName)
	{
		if (MapName.IsNullOrEmpty())
		{
			UIManager.SetGameObjectActive(m_loadingMapNameInfoContainer, false);
		}
		else if (gameType == GameType.Tutorial)
		{
			UIManager.SetGameObjectActive(m_loadingMapNameInfoContainer, true);
			m_mapNameTitle.text = StringUtil.TR("TutorialName", "Prologue1");
			m_mapNameSubTitle.text = StringUtil.TR("TutorialDescription", "Prologue1");
		}
		else
		{
			UIManager.SetGameObjectActive(m_loadingMapNameInfoContainer, false);
		}
	}

	public void OnDisable()
	{
	}

	public bool DoPlayIntroVideo()
	{
		if (shouldPlayIntroVideo)
		{
			return true;
		}
		string key = "PlayIntro";
		shouldPlayIntroVideo = !PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) != 1;
		PlayerPrefs.SetInt(key, 1);
		return shouldPlayIntroVideo;
	}

	public void Update()
	{
		bool gameConfigPresent = GameManager.Get()?.GameInfo?.GameConfig != null;
		if (gameConfigPresent)
		{
			SetupMapLoadingTitleInfo(GameManager.Get().GameInfo.GameConfig.GameType, GameManager.Get().GameInfo.GameConfig.Map);
		}
		else
		{
			SetupMapLoadingTitleInfo(GameType.None, string.Empty);
		}
		if (m_displayState == DisplayStates.FadeIn && IsCurrentAnimationDone())
		{
			StartDisplayLoading();
		}
		if (!m_isPlayerAccountDataAvailable && IsVisible() && (ClientGameManager.Get()?.IsPlayerAccountDataAvailable() ?? false))
		{
			m_isPlayerAccountDataAvailable = true;
			PickBackgroundImage();
		}
		if (m_displayState == DisplayStates.FadeOut && IsCurrentAnimationDone())
		{
			SetVisible(false);
		}
		if (!gameConfigPresent && FullScreenMovie.Get() != null && DoPlayIntroVideo())
		{
			if (FullScreenMovie.Get().m_movieTexture.MovieState == PlayRawImageMovieTexture.MovieStates.Invalid
				|| FullScreenMovie.Get().m_movieTexture.MovieState == PlayRawImageMovieTexture.MovieStates.Done
				|| Input.GetKeyUp(KeyCode.Escape))
			{
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
			}
		}
		else
		{
			Finish();
		}
		if (m_startLoadTime > 0f && Time.time >= m_startLoadTime)
		{
			m_startLoadTime = -1f;
			// TODO SERVER what assets are loaded on the server
#if SERVER
			StartCoroutine(AssetBundleManager.Get().LoadSceneAsync("DevEnvironmentSingletons", "frontend", LoadSceneMode.Additive));
#else
			StartCoroutine(AssetBundleManager.Get().LoadSceneAsync("ClientEnvironmentSingletons", "frontend", LoadSceneMode.Additive));
#endif
		}
	}

	private void Finish()
	{
		if (m_startLoadTime == 0f && m_animator != null)
		{
			SetVisible(true);
			FullScreenMovie.Get().SetVisible(false);
			m_animator.Play("FrontEndLoadingScreenDefaultIN");
			m_startLoadTime = Time.time;
		}
	}

	private void SetDisplayState(DisplayStates state)
	{
		m_displayState = state;
		bool showButton = m_displayState == DisplayStates.Error
			|| m_displayState == DisplayStates.ServerLocked
			|| m_displayState == DisplayStates.ServerQueued
			|| m_displayState == DisplayStates.None;
		UIManager.SetGameObjectActive(m_ShutdownButton, showButton);
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
		if (m_animator.isActiveAndEnabled)
		{
			m_animator.Play("FrontEndLoadingScreenDefaultOUT");
		}
	}

	private void ShowText(string text, string subText = null, string subText2 = null)
	{
		UIManager.SetGameObjectActive(m_ServerLockedButton, text == StringUtil.TR("SERVERISLOCKED", "LoadingScreen"));
		if (m_mainTextBox != null)
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
		if (m_subTextBox != null)
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
		if (m_subText2Box != null)
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
		}
	}

	public bool IsReadyToReveal()
	{
		return m_displayState == DisplayStates.FadeOut;
	}

	private bool IsCurrentAnimationDone()
	{
		if (!m_animator.isActiveAndEnabled)
		{
			return true;
		}
		AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
		return currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length;
	}
}
