using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIJoinGameScreen : UIScene
{
	public UICustomMatchEntry m_entryPrefab;

	public GridLayoutGroup m_gridLayout;

	public _ButtonSwapSprite m_cancelButton;

	public _ButtonSwapSprite m_createButton;

	public List<LobbyGameInfo> m_currentGames;

	public RectTransform[] m_containers;

	private static UIJoinGameScreen s_instance;

	private Dictionary<UICustomMatchEntry, LobbyGameInfo> m_buttonLookup;

	public static UIJoinGameScreen Get()
	{
		return UIJoinGameScreen.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.JoinCustomGame;
	}

	public override void Awake()
	{
		UIJoinGameScreen.s_instance = this;
		this.m_createButton.m_soundToPlay = FrontEndButtonSounds.Generic;
		base.Awake();
	}

	public void Start()
	{
		this.m_cancelButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CancelClicked);
		this.m_createButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CreateClicked);
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.HandleGameplayOverridesChange;
		if (GameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIJoinGameScreen.Start()).MethodHandle;
			}
			if (GameManager.Get().GameplayOverrides != null)
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
				this.HandleGameplayOverridesChange(GameManager.Get().GameplayOverrides);
			}
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIJoinGameScreen.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.HandleGameplayOverridesChange;
		}
	}

	public void SetVisible(bool visible)
	{
		for (int i = 0; i < this.m_containers.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_containers[i], visible, null);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIJoinGameScreen.SetVisible(bool)).MethodHandle;
		}
		this.m_cancelButton.ResetMouseState();
		this.m_createButton.ResetMouseState();
		UIManager.Get().SetSceneVisible(this.GetSceneType(), visible, new SceneVisibilityParameters());
	}

	public void CancelClicked(BaseEventData data)
	{
		AppState_JoinGame.Get().OnCancelClicked();
	}

	public void CreateClicked(BaseEventData data)
	{
		AppState_JoinGame.Get().OnCreateClicked();
	}

	public void JoinGameClicked(BaseEventData data)
	{
		UICustomMatchEntry[] componentsInChildren = this.m_gridLayout.transform.GetComponentsInChildren<UICustomMatchEntry>(true);
		int i = 0;
		while (i < componentsInChildren.Length)
		{
			bool flag = data.selectedObject == componentsInChildren[i].m_joinButton.gameObject;
			bool flag2 = data.selectedObject == componentsInChildren[i].m_joinAsSpectatorButton.gameObject;
			if (!flag)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIJoinGameScreen.JoinGameClicked(BaseEventData)).MethodHandle;
				}
				if (!flag2)
				{
					i++;
					continue;
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
			LobbyGameInfo lobbyGameInfo;
			if (this.m_buttonLookup.TryGetValue(componentsInChildren[i], out lobbyGameInfo))
			{
				if (!AssetBundleManager.Get().SceneExistsInBundle("maps", lobbyGameInfo.GameConfig.Map))
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
					if (!AssetBundleManager.Get().SceneExistsInBundle("testing", lobbyGameInfo.GameConfig.Map))
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
						UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("NoAccessToMap", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
						return;
					}
				}
				AppState_JoinGame.Get().OnJoinClicked(lobbyGameInfo, flag2);
			}
			return;
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void Setup()
	{
		this.m_currentGames = ClientGameManager.Get().CustomGameInfos;
		this.UpdateServerList(this.m_currentGames);
	}

	private void AddServerButton(LobbyGameInfo game)
	{
		UICustomMatchEntry uicustomMatchEntry = UnityEngine.Object.Instantiate<UICustomMatchEntry>(this.m_entryPrefab);
		uicustomMatchEntry.transform.SetParent(this.m_gridLayout.transform);
		uicustomMatchEntry.transform.localPosition = new Vector3(uicustomMatchEntry.transform.localPosition.x, uicustomMatchEntry.transform.localPosition.y, 0f);
		uicustomMatchEntry.transform.localScale = Vector3.one;
		uicustomMatchEntry.m_joinButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.JoinGameClicked);
		uicustomMatchEntry.m_joinAsSpectatorButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.JoinGameClicked);
		uicustomMatchEntry.Setup(game);
		this.m_buttonLookup[uicustomMatchEntry] = game;
	}

	public void UpdateServerList(List<LobbyGameInfo> gameInfoList)
	{
		if (this.m_buttonLookup != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIJoinGameScreen.UpdateServerList(List<LobbyGameInfo>)).MethodHandle;
			}
			using (Dictionary<UICustomMatchEntry, LobbyGameInfo>.KeyCollection.Enumerator enumerator = this.m_buttonLookup.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UICustomMatchEntry uicustomMatchEntry = enumerator.Current;
					UnityEngine.Object.Destroy(uicustomMatchEntry.gameObject);
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
			}
		}
		this.m_buttonLookup = new Dictionary<UICustomMatchEntry, LobbyGameInfo>();
		using (List<LobbyGameInfo>.Enumerator enumerator2 = gameInfoList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				LobbyGameInfo lobbyGameInfo = enumerator2.Current;
				if (lobbyGameInfo.ActivePlayers >= lobbyGameInfo.GameConfig.TotalPlayers)
				{
					if (lobbyGameInfo.ActiveSpectators >= lobbyGameInfo.GameConfig.Spectators)
					{
						continue;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.AddServerButton(lobbyGameInfo);
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
		int count = gameInfoList.Count;
		float y = (float)count * this.m_gridLayout.cellSize.y + this.m_gridLayout.spacing.y * (float)(count - 1);
		(this.m_gridLayout.transform as RectTransform).sizeDelta = new Vector2((this.m_gridLayout.transform as RectTransform).sizeDelta.x, y);
	}

	private bool HasGamesListChanged()
	{
		bool result = false;
		if (ClientGameManager.Get() == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIJoinGameScreen.HasGamesListChanged()).MethodHandle;
			}
			return false;
		}
		List<LobbyGameInfo> customGameInfos = ClientGameManager.Get().CustomGameInfos;
		if (customGameInfos == null)
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
			result = false;
		}
		else
		{
			if (this.m_currentGames != null)
			{
				if (customGameInfos.Count != this.m_currentGames.Count)
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
				}
				else
				{
					for (int i = 0; i < customGameInfos.Count; i++)
					{
						if (this.m_currentGames[i].UpdateTimestamp != customGameInfos[i].UpdateTimestamp)
						{
							return true;
						}
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						return result;
					}
				}
			}
			result = true;
		}
		return result;
	}

	private void HandleGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		this.m_createButton.selectableButton.SetDisabled(gameplayOverrides.DisabledGameTypes.Contains(GameType.Custom));
	}

	private void Update()
	{
		if (AppState.GetCurrent() != AppState_JoinGame.Get())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIJoinGameScreen.Update()).MethodHandle;
			}
			return;
		}
		if (this.HasGamesListChanged())
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
			this.m_currentGames = ClientGameManager.Get().CustomGameInfos;
			this.UpdateServerList(this.m_currentGames);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
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
			if (UIFrontEnd.Get().CanMenuEscape() && !UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
			{
				this.CancelClicked(null);
			}
		}
		this.m_gridLayout.cellSize = new Vector2((this.m_gridLayout.transform.parent as RectTransform).rect.width, this.m_gridLayout.cellSize.y);
	}
}
