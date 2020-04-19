using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFrontendLoadingScreen : UIScene
{
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

	private UIFrontendLoadingScreen.DisplayStates m_displayState;

	private string m_ServerLockURL = string.Empty;

	private static float m_startLoadTime;

	private bool shouldPlayIntroVideo;

	public UIFrontendLoadingScreen.DisplayStates DisplayState
	{
		get
		{
			return this.m_displayState;
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.FrontEndLoadingScreen;
	}

	public override void Awake()
	{
		UIFrontendLoadingScreen.s_instance = this;
		this.m_screenInitialHeight = Screen.height;
		this.m_screenInitialWidth = Screen.width;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (Application.isEditor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.Awake()).MethodHandle;
			}
			Application.runInBackground = true;
		}
		if (this.m_animator == null)
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
			this.m_animator = new Animator();
		}
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.None);
		if (!UnityEngine.Object.FindObjectOfType<EventSystem>())
		{
			GameObject gameObject = new GameObject("EventSystem", new Type[]
			{
				typeof(EventSystem)
			});
			gameObject.AddComponent<StandaloneInputModule>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		this.m_ServerLockedButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ServerLockedButtonClicked);
		this.SetServerLockButtonVisible(false);
		this.m_ShutdownButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ShutdownButtonClicked);
		this.m_visibleStartTime = Time.time;
	}

	private void Start()
	{
		AudioManager.StandardizeAudioLinkages();
		this.m_movieNames.Enqueue("Video/Logo/TRION-LOGO");
		this.m_movieNames.Enqueue("Video/AR_CG");
		if (this.m_movieNames.Count > 0)
		{
			FullScreenMovie.Get().SetVisible(true);
			string movieAssetName = this.m_movieNames.Dequeue();
			FullScreenMovie.Get().m_movieTexture.Play(movieAssetName, false, false, true);
		}
		base.RegisterWithUIManager();
	}

	internal bool IsVisible()
	{
		return this.m_Container.gameObject.activeSelf;
	}

	internal void SetVisible(bool visible)
	{
		if (this.m_Container.gameObject.activeSelf != visible)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.SetVisible(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_loadingMapNameInfoContainer, false, null);
			if (visible)
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
				this.m_visibleStartTime = Time.time;
				this.PickBackgroundImage();
			}
			else
			{
				this.m_bgImageanimator.enabled = false;
				Log.Info(Log.Category.Loading, "F.E. Loading screen displayed for {0} seconds.", new object[]
				{
					(Time.time - this.m_visibleStartTime).ToString("F1")
				});
			}
			UIManager.SetGameObjectActive(this.m_Container, visible, null);
		}
	}

	public bool IsSameAsInitialResolution()
	{
		bool result;
		if (this.m_screenInitialHeight == Screen.height)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.IsSameAsInitialResolution()).MethodHandle;
			}
			result = (this.m_screenInitialWidth == Screen.width);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void SetServerLockButtonVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_ServerLockedButton, visible, null);
	}

	public void UpdateServerLockLabels(ServerMessageOverrides serverMessageOverrides)
	{
		string language = HydrogenConfig.Get().Language;
		TextMeshProUGUI[] componentsInChildren = this.m_ServerLockedButton.GetComponentsInChildren<TextMeshProUGUI>(true);
		foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
		{
			textMeshProUGUI.text = serverMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenButtonText, language);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.UpdateServerLockLabels(ServerMessageOverrides)).MethodHandle;
		}
		this.m_ServerLockURL = ((!SteamManager.UsingSteam) ? serverMessageOverrides.FreeUpsellExternalBrowserUrl : serverMessageOverrides.FreeUpsellExternalBrowserSteamUrl);
		this.ShowText(StringUtil.TR("SERVERISLOCKED", "LoadingScreen"), serverMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenText, language), null);
	}

	public void ShutdownButtonClicked(BaseEventData data)
	{
		AppState_Shutdown.Get().Enter();
	}

	public void ServerLockedButtonClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendUIActionNotification("ServerLockedButtonClicked");
		if (!this.m_ServerLockURL.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.ServerLockedButtonClicked(BaseEventData)).MethodHandle;
			}
			Application.OpenURL(this.m_ServerLockURL);
		}
	}

	private void OnDestroy()
	{
		UIFrontendLoadingScreen.s_instance = null;
	}

	public static UIFrontendLoadingScreen Get()
	{
		return UIFrontendLoadingScreen.s_instance;
	}

	public void OnEnable()
	{
		this.StartDisplayFadeIn();
		this.PickBackgroundImage();
	}

	private void PickBackgroundImage()
	{
		if (!this.m_isPlayerAccountDataAvailable)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.PickBackgroundImage()).MethodHandle;
			}
			this.m_bgImageanimator.enabled = true;
			UIManager.SetGameObjectActive(this.m_backgroundImage, false, null);
			return;
		}
		IEnumerable<KeyValuePair<int, bool>> unlockedLoadingScreenBackgroundIdsToActivatedState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.UnlockedLoadingScreenBackgroundIdsToActivatedState;
		if (UIFrontendLoadingScreen.<>f__am$cache0 == null)
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
			UIFrontendLoadingScreen.<>f__am$cache0 = ((KeyValuePair<int, bool> x) => x.Value);
		}
		IEnumerable<KeyValuePair<int, bool>> source = unlockedLoadingScreenBackgroundIdsToActivatedState.Where(UIFrontendLoadingScreen.<>f__am$cache0);
		if (UIFrontendLoadingScreen.<>f__am$cache1 == null)
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
			UIFrontendLoadingScreen.<>f__am$cache1 = ((KeyValuePair<int, bool> x) => x.Key);
		}
		int[] array = source.Select(UIFrontendLoadingScreen.<>f__am$cache1).ToArray<int>();
		if (array.Length == 0)
		{
			this.m_backgroundImage.sprite = null;
		}
		else
		{
			int loadingScreenID = array[UnityEngine.Random.Range(0, array.Length)];
			this.m_backgroundImage.sprite = Resources.Load<Sprite>(GameBalanceVars.Get().GetLoadingScreenBackground(loadingScreenID).m_resourceString);
		}
		UIManager.SetGameObjectActive(this.m_backgroundImage, this.m_backgroundImage.sprite != null, null);
	}

	private void SetupMapLoadingTitleInfo(GameType gameType, string MapName)
	{
		if (MapName.IsNullOrEmpty())
		{
			UIManager.SetGameObjectActive(this.m_loadingMapNameInfoContainer, false, null);
		}
		else if (gameType == GameType.Tutorial)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.SetupMapLoadingTitleInfo(GameType, string)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_loadingMapNameInfoContainer, true, null);
			this.m_mapNameTitle.text = StringUtil.TR("TutorialName", "Prologue1");
			this.m_mapNameSubTitle.text = StringUtil.TR("TutorialDescription", "Prologue1");
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_loadingMapNameInfoContainer, false, null);
		}
	}

	public void OnDisable()
	{
	}

	public bool DoPlayIntroVideo()
	{
		if (this.shouldPlayIntroVideo)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.DoPlayIntroVideo()).MethodHandle;
			}
			return true;
		}
		this.shouldPlayIntroVideo = true;
		string key = "PlayIntro";
		if (PlayerPrefs.HasKey(key))
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
			this.shouldPlayIntroVideo = (PlayerPrefs.GetInt(key) != 1);
		}
		PlayerPrefs.SetInt(key, 1);
		return this.shouldPlayIntroVideo;
	}

	public void Update()
	{
		bool flag;
		if (GameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.Update()).MethodHandle;
			}
			if (GameManager.Get().GameInfo != null)
			{
				flag = (GameManager.Get().GameInfo.GameConfig != null);
				goto IL_4C;
			}
		}
		flag = false;
		IL_4C:
		bool flag2 = flag;
		if (flag2)
		{
			this.SetupMapLoadingTitleInfo(GameManager.Get().GameInfo.GameConfig.GameType, GameManager.Get().GameInfo.GameConfig.Map);
		}
		else
		{
			this.SetupMapLoadingTitleInfo(GameType.None, string.Empty);
		}
		if (this.m_displayState == UIFrontendLoadingScreen.DisplayStates.FadeIn)
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
			if (this.IsCurrentAnimationDone())
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
				this.StartDisplayLoading(null);
			}
		}
		if (!this.m_isPlayerAccountDataAvailable)
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
			if (this.IsVisible())
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
				if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
					this.m_isPlayerAccountDataAvailable = true;
					this.PickBackgroundImage();
				}
			}
		}
		if (this.m_displayState == UIFrontendLoadingScreen.DisplayStates.FadeOut)
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
			if (this.IsCurrentAnimationDone())
			{
				this.SetVisible(false);
			}
		}
		if (!flag2 && FullScreenMovie.Get())
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
			if (this.DoPlayIntroVideo())
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
				if (FullScreenMovie.Get().m_movieTexture.MovieState != PlayRawImageMovieTexture.MovieStates.Invalid && FullScreenMovie.Get().m_movieTexture.MovieState != PlayRawImageMovieTexture.MovieStates.Done)
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
					if (!Input.GetKeyUp(KeyCode.Escape))
					{
						goto IL_1FD;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (this.m_movieNames.Count > 0)
				{
					string movieAssetName = this.m_movieNames.Dequeue();
					FullScreenMovie.Get().m_movieTexture.Play(movieAssetName, false, false, true);
					this.SetVisible(false);
				}
				else
				{
					this.Finish();
				}
				IL_1FD:
				goto IL_205;
			}
		}
		this.Finish();
		IL_205:
		if (UIFrontendLoadingScreen.m_startLoadTime > 0f)
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
			if (Time.time >= UIFrontendLoadingScreen.m_startLoadTime)
			{
				UIFrontendLoadingScreen.m_startLoadTime = -1f;
				base.StartCoroutine(AssetBundleManager.Get().LoadSceneAsync("ClientEnvironmentSingletons", "frontend", LoadSceneMode.Additive));
			}
		}
	}

	private void Finish()
	{
		if (UIFrontendLoadingScreen.m_startLoadTime == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.Finish()).MethodHandle;
			}
			if (this.m_animator != null)
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
				this.SetVisible(true);
				FullScreenMovie.Get().SetVisible(false);
				this.m_animator.Play("FrontEndLoadingScreenDefaultIN");
				UIFrontendLoadingScreen.m_startLoadTime = Time.time;
			}
		}
	}

	private void SetDisplayState(UIFrontendLoadingScreen.DisplayStates state)
	{
		this.m_displayState = state;
		if (this.m_displayState != UIFrontendLoadingScreen.DisplayStates.Error)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.SetDisplayState(UIFrontendLoadingScreen.DisplayStates)).MethodHandle;
			}
			if (this.m_displayState != UIFrontendLoadingScreen.DisplayStates.ServerLocked)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_displayState != UIFrontendLoadingScreen.DisplayStates.ServerQueued)
				{
					if (this.m_displayState != UIFrontendLoadingScreen.DisplayStates.None)
					{
						UIManager.SetGameObjectActive(this.m_ShutdownButton, false, null);
						return;
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
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_ShutdownButton, true, null);
	}

	public void StartDisplayFadeIn()
	{
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.FadeIn);
		this.m_animator.Play("FrontEndLoadingScreenDefaultIN");
	}

	public void StartDisplayLoading(string subText = null)
	{
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.Loading);
		this.ShowText(StringUtil.TR("NOWLOADING", "LoadingScreen"), subText, null);
		this.m_animator.Play("FrontEndLoadingScreenDefaultIDLE");
	}

	public void StartDisplayPressKey()
	{
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.PressKey);
		this.ShowText(StringUtil.TR("PRESSANYKEY", "LoadingScreen"), null, null);
	}

	public void StartDisplayServerLocked()
	{
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.ServerLocked);
	}

	public void StartDisplayServerQueued(string queueStatusString)
	{
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.ServerQueued);
		this.ShowText(StringUtil.TR("SERVERISFULL", "LoadingScreen"), queueStatusString, null);
	}

	public void StartDisplayError(string mainErrorText, string subErrorText = null)
	{
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.Error);
		this.ShowText(mainErrorText, subErrorText, null);
	}

	public void StartDisplayFadeOut()
	{
		this.SetDisplayState(UIFrontendLoadingScreen.DisplayStates.FadeOut);
		if (this.m_animator.isActiveAndEnabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.StartDisplayFadeOut()).MethodHandle;
			}
			this.m_animator.Play("FrontEndLoadingScreenDefaultOUT");
		}
	}

	private void ShowText(string text, string subText = null, string subText2 = null)
	{
		UIManager.SetGameObjectActive(this.m_ServerLockedButton, text == StringUtil.TR("SERVERISLOCKED", "LoadingScreen"), null);
		if (this.m_mainTextBox)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.ShowText(string, string, string)).MethodHandle;
			}
			if (!text.IsNullOrEmpty())
			{
				this.m_mainTextBox.GetComponent<TextMeshProUGUI>().text = text;
				UIManager.SetGameObjectActive(this.m_mainTextBox, true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_mainTextBox, false, null);
			}
		}
		if (this.m_subTextBox)
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
			if (!subText.IsNullOrEmpty())
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
				this.m_subTextBox.GetComponent<TextMeshProUGUI>().text = subText;
				UIManager.SetGameObjectActive(this.m_subTextBox, true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_subTextBox, false, null);
			}
		}
		if (this.m_subText2Box)
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
			if (!subText2.IsNullOrEmpty())
			{
				this.m_subText2Box.GetComponent<TextMeshProUGUI>().text = subText2;
				UIManager.SetGameObjectActive(this.m_subText2Box, true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_subText2Box, false, null);
			}
		}
	}

	public bool IsReadyToReveal()
	{
		if (this.m_displayState == UIFrontendLoadingScreen.DisplayStates.FadeOut)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.IsReadyToReveal()).MethodHandle;
			}
			return true;
		}
		return false;
	}

	private bool IsCurrentAnimationDone()
	{
		bool result = false;
		if (this.m_animator.isActiveAndEnabled)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFrontendLoadingScreen.IsCurrentAnimationDone()).MethodHandle;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				result = true;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

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
}
