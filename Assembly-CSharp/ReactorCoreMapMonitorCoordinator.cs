using System;
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
		return ReactorCoreMapMonitorCoordinator.s_instance;
	}

	private void Awake()
	{
		if (ReactorCoreMapMonitorCoordinator.s_instance != null)
		{
			Log.Error(base.GetType() + " has existing instance in scene on Awake, may have duplicates", new object[0]);
		}
		ReactorCoreMapMonitorCoordinator.s_instance = this;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		this.m_portraitSwitchDuration = Mathf.Max(0.3f, this.m_portraitSwitchDuration);
	}

	private void Start()
	{
		if (this.m_largePortraitObject != null)
		{
			this.m_largePortraitObjectRenderer = this.m_largePortraitObject.GetComponent<Renderer>();
			this.m_largePortraitObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		this.m_characterRespawning.Clear();
		this.m_characterSprites.Clear();
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedMovement);
		ReactorCoreMapMonitorCoordinator.s_instance = null;
	}

	public void AddPortraitController(ReactorCoreMapRespawnMonitor controller)
	{
		if (controller != null)
		{
			if (!this.m_portraitControllers.Contains(controller))
			{
				this.m_portraitControllers.Add(controller);
			}
		}
	}

	public void RemovePortraitController(ReactorCoreMapRespawnMonitor controller)
	{
		if (controller != null)
		{
			this.m_portraitControllers.Remove(controller);
		}
	}

	public void symbol_000E()
	{
		string text = "Respawning Actors:\n";
		using (List<CharacterType>.Enumerator enumerator = this.m_characterRespawning.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharacterType characterType = enumerator.Current;
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\t",
					characterType,
					"\n"
				});
			}
		}
		Debug.LogWarning(text);
	}

	private void Update()
	{
		if (this.m_characterRespawning.Count > 0)
		{
			this.m_timeTillNextUpdate -= Time.deltaTime;
			if (this.m_timeTillNextUpdate <= 0f)
			{
				this.m_timeTillNextUpdate = this.m_portraitSwitchDuration;
				this.m_currentIndex++;
				if (this.m_currentIndex >= this.m_characterRespawning.Count)
				{
					this.m_currentIndex = 0;
				}
				CharacterType key = this.m_characterRespawning[this.m_currentIndex];
				if (this.m_characterSprites.ContainsKey(key))
				{
					Sprite respawnPortrait = this.m_characterSprites[key];
					for (int i = 0; i < this.m_portraitControllers.Count; i++)
					{
						if (this.m_portraitControllers[i] != null)
						{
							this.m_portraitControllers[i].SetRespawnPortrait(respawnPortrait);
						}
					}
				}
			}
		}
	}

	private void HidePortraits()
	{
		for (int i = 0; i < this.m_portraitControllers.Count; i++)
		{
			if (this.m_portraitControllers[i] != null)
			{
				this.m_portraitControllers[i].HidePortrait();
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.TurnTick)
		{
			if (this.m_largePortraitObjectRenderer != null)
			{
			}
			this.m_characterRespawning.Clear();
			List<ActorData> actors = GameFlowData.Get().GetActors();
			int i = 0;
			while (i < actors.Count)
			{
				ActorData actorData = actors[i];
				if (actorData.IsDead())
				{
					goto IL_A9;
				}
				if (actorData.NextRespawnTurn > 0)
				{
					if (actorData.NextRespawnTurn == GameFlowData.Get().CurrentTurn)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							goto IL_A9;
						}
					}
				}
				IL_111:
				i++;
				continue;
				IL_A9:
				if (this.m_characterRespawning.Contains(actorData.m_characterType))
				{
					goto IL_111;
				}
				this.m_characterRespawning.Add(actorData.m_characterType);
				Sprite aliveHUDIcon = actorData.GetAliveHUDIcon();
				if (!this.m_characterSprites.ContainsKey(actorData.m_characterType))
				{
					this.m_characterSprites.Add(actorData.m_characterType, aliveHUDIcon);
					goto IL_111;
				}
				goto IL_111;
			}
			int count = this.m_characterRespawning.Count;
			if (count > 0)
			{
				this.m_timeTillNextUpdate = 0f;
			}
			else
			{
				this.HidePortraits();
			}
		}
		if (eventType == GameEventManager.EventType.UIPhaseStartedMovement)
		{
			this.m_characterRespawning.Clear();
			List<ActorData> actors2 = GameFlowData.Get().GetActors();
			for (int j = 0; j < actors2.Count; j++)
			{
				ActorData actorData2 = actors2[j];
				if (actorData2.IsDead())
				{
					if (!this.m_characterRespawning.Contains(actorData2.m_characterType))
					{
						this.m_characterRespawning.Add(actorData2.m_characterType);
						Sprite aliveHUDIcon2 = actorData2.GetAliveHUDIcon();
						if (!this.m_characterSprites.ContainsKey(actorData2.m_characterType))
						{
							this.m_characterSprites.Add(actorData2.m_characterType, aliveHUDIcon2);
						}
					}
				}
			}
			if (this.m_characterRespawning.Count > 0)
			{
				this.m_timeTillNextUpdate = 0f;
			}
			else
			{
				this.HidePortraits();
			}
		}
	}
}
