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
			if (GameManager.Get().GameplayOverrides != null)
			{
				this.HandleGameplayOverridesChange(GameManager.Get().GameplayOverrides);
			}
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.HandleGameplayOverridesChange;
		}
	}

	public void SetVisible(bool visible)
	{
		for (int i = 0; i < this.m_containers.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_containers[i], visible, null);
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
				if (!flag2)
				{
					i++;
					continue;
				}
			}
			LobbyGameInfo lobbyGameInfo;
			if (this.m_buttonLookup.TryGetValue(componentsInChildren[i], out lobbyGameInfo))
			{
				if (!AssetBundleManager.Get().SceneExistsInBundle("maps", lobbyGameInfo.GameConfig.Map))
				{
					if (!AssetBundleManager.Get().SceneExistsInBundle("testing", lobbyGameInfo.GameConfig.Map))
					{
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
			using (Dictionary<UICustomMatchEntry, LobbyGameInfo>.KeyCollection.Enumerator enumerator = this.m_buttonLookup.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UICustomMatchEntry uicustomMatchEntry = enumerator.Current;
					UnityEngine.Object.Destroy(uicustomMatchEntry.gameObject);
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
				}
				this.AddServerButton(lobbyGameInfo);
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
			return false;
		}
		List<LobbyGameInfo> customGameInfos = ClientGameManager.Get().CustomGameInfos;
		if (customGameInfos == null)
		{
			result = false;
		}
		else
		{
			if (this.m_currentGames != null)
			{
				if (customGameInfos.Count != this.m_currentGames.Count)
				{
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
			return;
		}
		if (this.HasGamesListChanged())
		{
			this.m_currentGames = ClientGameManager.Get().CustomGameInfos;
			this.UpdateServerList(this.m_currentGames);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (UIFrontEnd.Get().CanMenuEscape() && !UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
			{
				this.CancelClicked(null);
			}
		}
		this.m_gridLayout.cellSize = new Vector2((this.m_gridLayout.transform.parent as RectTransform).rect.width, this.m_gridLayout.cellSize.y);
	}
}
