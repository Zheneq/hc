using System.Collections.Generic;
using UnityEngine;

public class ReactorCoreMapMonitorCoordinator : MonoBehaviour, IGameEventListener
{
	public GameObject m_largePortraitObject;

	[Header("-- Prefabs --")]
	public GameObject m_portraitPrefabForVfxMonitor;

	public GameObject m_portraitPrefabForTextureMonitor;

	[Header("-- Materials for Monitors --")]
	public Material m_searchingMaterial;

	public Material m_resurrectingMaterial;

	[Header("-- Vfx prefabs for monitors --")]
	public GameObject m_vfxPrefabSmallSearching;

	public GameObject m_vfxPrefabSmallResurrecting;

	[Space(10f)]
	public GameObject m_vfxPrefabLargeSearching;

	public GameObject m_vfxPrefabLargeResurrecting;

	[Header("-- Failsafe sprite for characters without portrait --")]
	public Sprite m_fallbackPortraitSprite;

	[Header("-- Attach positions and scale --")]
	public Vector3 m_smallMonitorPos;

	public float m_smallMonitorScale = 1f;

	public Vector3 m_largeMonitorPos;

	public float m_largeMonitorScale = 1f;

	[Header("-- Portrait Switch Params --")]
	public float m_portraitSwitchDuration = 2f;

	private List<ReactorCoreMapRespawnMonitor> m_portraitControllers = new List<ReactorCoreMapRespawnMonitor>();

	private Renderer m_largePortraitObjectRenderer;

	private List<CharacterType> m_characterRespawning = new List<CharacterType>();

	private Dictionary<CharacterType, Sprite> m_characterSprites = new Dictionary<CharacterType, Sprite>();

	private float m_timeTillNextUpdate;

	private int m_currentIndex;

	private static ReactorCoreMapMonitorCoordinator s_instance;

	public static ReactorCoreMapMonitorCoordinator Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (4)
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
			Log.Error(string.Concat(GetType(), " has existing instance in scene on Awake, may have duplicates"));
		}
		s_instance = this;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		m_portraitSwitchDuration = Mathf.Max(0.3f, m_portraitSwitchDuration);
	}

	private void Start()
	{
		if (!(m_largePortraitObject != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_largePortraitObjectRenderer = m_largePortraitObject.GetComponent<Renderer>();
			m_largePortraitObject.SetActive(false);
			return;
		}
	}

	private void OnDestroy()
	{
		m_characterRespawning.Clear();
		m_characterSprites.Clear();
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		s_instance = null;
	}

	public void AddPortraitController(ReactorCoreMapRespawnMonitor controller)
	{
		if (!(controller != null))
		{
			return;
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
			if (!m_portraitControllers.Contains(controller))
			{
				m_portraitControllers.Add(controller);
			}
			return;
		}
	}

	public void RemovePortraitController(ReactorCoreMapRespawnMonitor controller)
	{
		if (!(controller != null))
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
			m_portraitControllers.Remove(controller);
			return;
		}
	}

	public void _000E()
	{
		string text = "Respawning Actors:\n";
		using (List<CharacterType>.Enumerator enumerator = m_characterRespawning.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharacterType current = enumerator.Current;
				string text2 = text;
				text = string.Concat(text2, "\t", current, "\n");
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_0014;
				}
			}
			end_IL_0014:;
		}
		Debug.LogWarning(text);
	}

	private void Update()
	{
		if (m_characterRespawning.Count <= 0)
		{
			return;
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
			m_timeTillNextUpdate -= Time.deltaTime;
			if (!(m_timeTillNextUpdate <= 0f))
			{
				return;
			}
			m_timeTillNextUpdate = m_portraitSwitchDuration;
			m_currentIndex++;
			if (m_currentIndex >= m_characterRespawning.Count)
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
				m_currentIndex = 0;
			}
			CharacterType key = m_characterRespawning[m_currentIndex];
			if (!m_characterSprites.ContainsKey(key))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				Sprite respawnPortrait = m_characterSprites[key];
				for (int i = 0; i < m_portraitControllers.Count; i++)
				{
					if (m_portraitControllers[i] != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						m_portraitControllers[i].SetRespawnPortrait(respawnPortrait);
					}
				}
				return;
			}
		}
	}

	private void HidePortraits()
	{
		for (int i = 0; i < m_portraitControllers.Count; i++)
		{
			if (m_portraitControllers[i] != null)
			{
				m_portraitControllers[i].HidePortrait();
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.TurnTick)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_largePortraitObjectRenderer != null)
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
			}
			m_characterRespawning.Clear();
			List<ActorData> actors = GameFlowData.Get().GetActors();
			for (int i = 0; i < actors.Count; i++)
			{
				ActorData actorData = actors[i];
				if (!actorData.IsDead())
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorData.NextRespawnTurn <= 0)
					{
						continue;
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
					if (actorData.NextRespawnTurn != GameFlowData.Get().CurrentTurn)
					{
						continue;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (m_characterRespawning.Contains(actorData.m_characterType))
				{
					continue;
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
				m_characterRespawning.Add(actorData.m_characterType);
				Sprite aliveHUDIcon = actorData.GetAliveHUDIcon();
				if (!m_characterSprites.ContainsKey(actorData.m_characterType))
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
					m_characterSprites.Add(actorData.m_characterType, aliveHUDIcon);
				}
			}
			int count = m_characterRespawning.Count;
			if (count > 0)
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
				m_timeTillNextUpdate = 0f;
			}
			else
			{
				HidePortraits();
			}
		}
		if (eventType != GameEventManager.EventType.UIPhaseStartedMovement)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			m_characterRespawning.Clear();
			List<ActorData> actors2 = GameFlowData.Get().GetActors();
			for (int j = 0; j < actors2.Count; j++)
			{
				ActorData actorData2 = actors2[j];
				if (!actorData2.IsDead())
				{
					continue;
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
				if (m_characterRespawning.Contains(actorData2.m_characterType))
				{
					continue;
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
				m_characterRespawning.Add(actorData2.m_characterType);
				Sprite aliveHUDIcon2 = actorData2.GetAliveHUDIcon();
				if (!m_characterSprites.ContainsKey(actorData2.m_characterType))
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
					m_characterSprites.Add(actorData2.m_characterType, aliveHUDIcon2);
				}
			}
			if (m_characterRespawning.Count > 0)
			{
				m_timeTillNextUpdate = 0f;
			}
			else
			{
				HidePortraits();
			}
			return;
		}
	}
}
