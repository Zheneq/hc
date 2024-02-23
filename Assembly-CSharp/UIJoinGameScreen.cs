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
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.JoinCustomGame;
	}

	public override void Awake()
	{
		s_instance = this;
		m_createButton.m_soundToPlay = FrontEndButtonSounds.Generic;
		base.Awake();
	}

	public void Start()
	{
		m_cancelButton.callback = CancelClicked;
		m_createButton.callback = CreateClicked;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += HandleGameplayOverridesChange;
		if (GameManager.Get() != null && GameManager.Get().GameplayOverrides != null)
		{
			HandleGameplayOverridesChange(GameManager.Get().GameplayOverrides);
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= HandleGameplayOverridesChange;
		}
	}

	public void SetVisible(bool visible)
	{
		foreach (RectTransform t in m_containers)
		{
			UIManager.SetGameObjectActive(t, visible);
		}
		m_cancelButton.ResetMouseState();
		m_createButton.ResetMouseState();
		UIManager.Get().SetSceneVisible(GetSceneType(), visible, new SceneVisibilityParameters());
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
		UICustomMatchEntry[] componentsInChildren = m_gridLayout.transform.GetComponentsInChildren<UICustomMatchEntry>(true);

		foreach (UICustomMatchEntry entry in componentsInChildren)
		{
			bool join = data.selectedObject == entry.m_joinButton.gameObject;
			bool joinAsSpectator = data.selectedObject == entry.m_joinAsSpectatorButton.gameObject;
			if (join || joinAsSpectator)
			{
				LobbyGameInfo value;
				if (!m_buttonLookup.TryGetValue(entry, out value))
				{
					return;
				}

				if (!AssetBundleManager.Get().SceneExistsInBundle("maps", value.GameConfig.Map)
				    && !AssetBundleManager.Get().SceneExistsInBundle("testing", value.GameConfig.Map))
				{
					UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("NoAccessToMap", "Global"),
						StringUtil.TR("Ok", "Global"));
					return;
				}

				AppState_JoinGame.Get().OnJoinClicked(value, joinAsSpectator);
				return;
			}
		}
	}

	public void Setup()
	{
		m_currentGames = ClientGameManager.Get().CustomGameInfos;
		UpdateServerList(m_currentGames);
	}

	private void AddServerButton(LobbyGameInfo game)
	{
		UICustomMatchEntry uICustomMatchEntry = Object.Instantiate(m_entryPrefab);
		uICustomMatchEntry.transform.SetParent(m_gridLayout.transform);
		Vector3 localPosition = uICustomMatchEntry.transform.localPosition;
		Vector3 localPosition2 = uICustomMatchEntry.transform.localPosition;
		uICustomMatchEntry.transform.localPosition = new Vector3(localPosition.x, localPosition2.y, 0f);
		uICustomMatchEntry.transform.localScale = Vector3.one;
		uICustomMatchEntry.m_joinButton.callback = JoinGameClicked;
		uICustomMatchEntry.m_joinAsSpectatorButton.callback = JoinGameClicked;
		uICustomMatchEntry.Setup(game);
		m_buttonLookup[uICustomMatchEntry] = game;
	}

	public void UpdateServerList(List<LobbyGameInfo> gameInfoList)
	{
		if (m_buttonLookup != null)
		{
			foreach (UICustomMatchEntry entry in m_buttonLookup.Keys)
			{
				Destroy(entry.gameObject);
			}
		}
		m_buttonLookup = new Dictionary<UICustomMatchEntry, LobbyGameInfo>();
		
		foreach (LobbyGameInfo game in gameInfoList)
		{
			if (game.ActivePlayers < game.GameConfig.TotalPlayers
			    || game.ActiveSpectators < game.GameConfig.Spectators)
			{
				AddServerButton(game);
			}
		}
		int gameNum = gameInfoList.Count;
		float totalEntryHeight = gameNum * m_gridLayout.cellSize.y;
		float totalHeight = totalEntryHeight + m_gridLayout.spacing.y * (gameNum - 1);
		RectTransform gridTransform = m_gridLayout.transform as RectTransform;
		gridTransform.sizeDelta = new Vector2(gridTransform.sizeDelta.x, totalHeight);
	}

	private bool HasGamesListChanged()
	{
		if (ClientGameManager.Get() == null)
		{
			return false;
		}
		List<LobbyGameInfo> customGameInfos = ClientGameManager.Get().CustomGameInfos;
		if (customGameInfos == null)
		{
			return false;
		}
		if (m_currentGames == null || customGameInfos.Count != m_currentGames.Count)
		{
			return true;
		}
		for (int i = 0; i < customGameInfos.Count; i++)
		{
			if (m_currentGames[i].UpdateTimestamp != customGameInfos[i].UpdateTimestamp)
			{
				return true;
			}
		}
		return false;
	}

	private void HandleGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		m_createButton.selectableButton.SetDisabled(gameplayOverrides.DisabledGameTypes.Contains(GameType.Custom));
	}

	private void Update()
	{
		if (AppState.GetCurrent() != AppState_JoinGame.Get())
		{
			return;
		}
		if (HasGamesListChanged())
		{
			m_currentGames = ClientGameManager.Get().CustomGameInfos;
			UpdateServerList(m_currentGames);
		}
		if (Input.GetKeyDown(KeyCode.Escape)
		    && UIFrontEnd.Get().CanMenuEscape()
		    && !UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
		{
			CancelClicked(null);
		}

		float width = (m_gridLayout.transform.parent as RectTransform).rect.width;
		m_gridLayout.cellSize = new Vector2(width, m_gridLayout.cellSize.y);
	}
}
