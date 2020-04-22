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
		if (!(GameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameManager.Get().GameplayOverrides != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					HandleGameplayOverridesChange(GameManager.Get().GameplayOverrides);
					return;
				}
			}
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
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
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= HandleGameplayOverridesChange;
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		for (int i = 0; i < m_containers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_containers[i], visible);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_cancelButton.ResetMouseState();
			m_createButton.ResetMouseState();
			UIManager.Get().SetSceneVisible(GetSceneType(), visible, new SceneVisibilityParameters());
			return;
		}
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
		int num = 0;
		while (num < componentsInChildren.Length)
		{
			bool flag = data.selectedObject == componentsInChildren[num].m_joinButton.gameObject;
			bool flag2 = data.selectedObject == componentsInChildren[num].m_joinAsSpectatorButton.gameObject;
			if (!flag)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!flag2)
				{
					num++;
					continue;
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
			if (!m_buttonLookup.TryGetValue(componentsInChildren[num], out LobbyGameInfo value))
			{
				return;
			}
			if (!AssetBundleManager.Get().SceneExistsInBundle("maps", value.GameConfig.Map))
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
				if (!AssetBundleManager.Get().SceneExistsInBundle("testing", value.GameConfig.Map))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("NoAccessToMap", "Global"), StringUtil.TR("Ok", "Global"));
							return;
						}
					}
				}
			}
			AppState_JoinGame.Get().OnJoinClicked(value, flag2);
			return;
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
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
		Transform transform = uICustomMatchEntry.transform;
		Vector3 localPosition = uICustomMatchEntry.transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = uICustomMatchEntry.transform.localPosition;
		transform.localPosition = new Vector3(x, localPosition2.y, 0f);
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
			using (Dictionary<UICustomMatchEntry, LobbyGameInfo>.KeyCollection.Enumerator enumerator = m_buttonLookup.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UICustomMatchEntry current = enumerator.Current;
					Object.Destroy(current.gameObject);
				}
				while (true)
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
		m_buttonLookup = new Dictionary<UICustomMatchEntry, LobbyGameInfo>();
		using (List<LobbyGameInfo>.Enumerator enumerator2 = gameInfoList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				LobbyGameInfo current2 = enumerator2.Current;
				if (current2.ActivePlayers >= current2.GameConfig.TotalPlayers)
				{
					if (current2.ActiveSpectators >= current2.GameConfig.Spectators)
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
						break;
					}
				}
				AddServerButton(current2);
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
		int count = gameInfoList.Count;
		float num = count;
		Vector2 cellSize = m_gridLayout.cellSize;
		float num2 = num * cellSize.y;
		Vector2 spacing = m_gridLayout.spacing;
		float y = num2 + spacing.y * (float)(count - 1);
		RectTransform obj = m_gridLayout.transform as RectTransform;
		Vector2 sizeDelta = (m_gridLayout.transform as RectTransform).sizeDelta;
		obj.sizeDelta = new Vector2(sizeDelta.x, y);
	}

	private bool HasGamesListChanged()
	{
		bool result = false;
		if (ClientGameManager.Get() == null)
		{
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
				return false;
			}
		}
		List<LobbyGameInfo> customGameInfos = ClientGameManager.Get().CustomGameInfos;
		if (customGameInfos == null)
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
			result = false;
		}
		else
		{
			if (m_currentGames != null)
			{
				if (customGameInfos.Count == m_currentGames.Count)
				{
					int num = 0;
					while (true)
					{
						if (num < customGameInfos.Count)
						{
							if (m_currentGames[num].UpdateTimestamp != customGameInfos[num].UpdateTimestamp)
							{
								result = true;
								break;
							}
							num++;
							continue;
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
						break;
					}
					goto IL_00b8;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			result = true;
		}
		goto IL_00b8;
		IL_00b8:
		return result;
	}

	private void HandleGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		m_createButton.selectableButton.SetDisabled(gameplayOverrides.DisabledGameTypes.Contains(GameType.Custom));
	}

	private void Update()
	{
		if (AppState.GetCurrent() != AppState_JoinGame.Get())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (HasGamesListChanged())
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
			m_currentGames = ClientGameManager.Get().CustomGameInfos;
			UpdateServerList(m_currentGames);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
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
			if (UIFrontEnd.Get().CanMenuEscape() && !UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
			{
				CancelClicked(null);
			}
		}
		GridLayoutGroup gridLayout = m_gridLayout;
		float width = (m_gridLayout.transform.parent as RectTransform).rect.width;
		Vector2 cellSize = m_gridLayout.cellSize;
		gridLayout.cellSize = new Vector2(width, cellSize.y);
	}
}
