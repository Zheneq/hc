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
			Application.runInBackground = true;
		}
		if (this.m_animator == null)
		{
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
			UIManager.SetGameObjectActive(this.m_loadingMapNameInfoContainer, false, null);
			if (visible)
			{
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
			this.m_bgImageanimator.enabled = true;
			UIManager.SetGameObjectActive(this.m_backgroundImage, false, null);
			return;
		}
		IEnumerable<KeyValuePair<int, bool>> unlockedLoadingScreenBackgroundIdsToActivatedState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.UnlockedLoadingScreenBackgroundIdsToActivatedState;
		
		IEnumerable<KeyValuePair<int, bool>> source = unlockedLoadingScreenBackgroundIdsToActivatedState.Where(((KeyValuePair<int, bool> x) => x.Value));
		
		int[] array = source.Select(((KeyValuePair<int, bool> x) => x.Key)).ToArray<int>();
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
			return true;
		}
		this.shouldPlayIntroVideo = true;
		string key = "PlayIntro";
		if (PlayerPrefs.HasKey(key))
		{
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
			if (this.IsCurrentAnimationDone())
			{
				this.StartDisplayLoading(null);
			}
		}
		if (!this.m_isPlayerAccountDataAvailable)
		{
			if (this.IsVisible())
			{
				if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					this.m_isPlayerAccountDataAvailable = true;
					this.PickBackgroundImage();
				}
			}
		}
		if (this.m_displayState == UIFrontendLoadingScreen.DisplayStates.FadeOut)
		{
			if (this.IsCurrentAnimationDone())
			{
				this.SetVisible(false);
			}
		}
		if (!flag2 && FullScreenMovie.Get())
		{
			if (this.DoPlayIntroVideo())
			{
				if (FullScreenMovie.Get().m_movieTexture.MovieState != PlayRawImageMovieTexture.MovieStates.Invalid && FullScreenMovie.Get().m_movieTexture.MovieState != PlayRawImageMovieTexture.MovieStates.Done)
				{
					if (!Input.GetKeyUp(KeyCode.Escape))
					{
						goto IL_1FD;
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
			if (this.m_animator != null)
			{
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
			if (this.m_displayState != UIFrontendLoadingScreen.DisplayStates.ServerLocked)
			{
				if (this.m_displayState != UIFrontendLoadingScreen.DisplayStates.ServerQueued)
				{
					if (this.m_displayState != UIFrontendLoadingScreen.DisplayStates.None)
					{
						UIManager.SetGameObjectActive(this.m_ShutdownButton, false, null);
						return;
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
			this.m_animator.Play("FrontEndLoadingScreenDefaultOUT");
		}
	}

	private void ShowText(string text, string subText = null, string subText2 = null)
	{
		UIManager.SetGameObjectActive(this.m_ServerLockedButton, text == StringUtil.TR("SERVERISLOCKED", "LoadingScreen"), null);
		if (this.m_mainTextBox)
		{
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
			if (!subText.IsNullOrEmpty())
			{
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
			return true;
		}
		return false;
	}

	private bool IsCurrentAnimationDone()
	{
		bool result = false;
		if (this.m_animator.isActiveAndEnabled)
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
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
